using ApiProject_02_01_2024.DTOs;
using ApiProject_02_01_2024.Models;
using ApiProject_02_01_2024.Repository;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

using System.Net.NetworkInformation;
using static Dapper.SqlMapper;

namespace ApiProject_02_01_2024.Services.DesignationService
{
    public class DesignationService:IDesignationService
    {
        private readonly IGenericRepository<Designation, int> _designationRepository;
        private readonly IGenericRepository<HrmEmpDigitalSignature, int> hrmPhoto;
        private readonly IWebHostEnvironment _web;
        private readonly IHttpContextAccessor _httpContextAccessor;
     
        public DesignationService(IGenericRepository<Designation, int> designationRepository, IGenericRepository<HrmEmpDigitalSignature, int> hrmPhoto, IWebHostEnvironment web, IHttpContextAccessor httpContextAccessor)
        {
            _designationRepository = designationRepository;
            this.hrmPhoto = hrmPhoto;
            _web = web;
            _httpContextAccessor = httpContextAccessor;
        }
       

        public IEnumerable<CommonSelectModelVM> DropSelection()
        {
           
            return _designationRepository
                .All() 
                .Select(x => new CommonSelectModelVM
                {
                    Code = x.DesignationCode,
                    Name = x.DesignationName
                })
                .ToList(); // Convert IQueryable to List
        }
        public async Task<List<DesignationVM>> GetAllAsync()
        {
            var  designations=await _designationRepository.GetAllAsync();
         
            return designations.Select(d => new DesignationVM
            {
                DesignationAutoId=d.DesignationAutoId,
                DesignationCode=d.DesignationCode,
                DesignationName=d.DesignationName,
                ShortName = d.ShortName,
                LDate = d.LDate,
                ModifyDate = d.ModifyDate,
                ProfilePicture = d.ProfilePicture,
   
                PhotoUrl = GetPhotoUrl(d.DesignationAutoId)
            }).ToList();
        }
        private string? GetPhotoUrl(int designationId)
        {
            var photo = hrmPhoto.All().FirstOrDefault(p => p.DesignationAutoId == designationId);
            if (photo != null && photo.DigitalSignature != null)
            {
                var base64 = Convert.ToBase64String(photo.DigitalSignature);
                var imgType = photo.ImgType ?? "image/png"; // Default to PNG if null
                return $"data:{imgType};base64,{base64}";
            }
            return null; // Return null if no image exists
        }
        public async Task<DesignationVM> GetByIdAsync(int id)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var designation = await _designationRepository.GetByIdAsync(id);
            if(designation == null)
            {
                return null;
            }
            return new DesignationVM
            {
                DesignationAutoId = designation.DesignationAutoId,
                DesignationCode = designation.DesignationCode,
                DesignationName = designation.DesignationName,
                ShortName = designation.ShortName,
                LDate = designation.LDate,
                ModifyDate = designation.ModifyDate,
                ProfilePicture=designation.ProfilePicture,
               
                PhotoUrl = GetPhotoUrl(designation.DesignationAutoId)
            };
        }


        private string UploadedFile(DesignationVM designationVM)
        {
            string uniqueFileName = null;

            if (designationVM.ProfileImage != null && !string.IsNullOrEmpty(designationVM.DesignationCode) && !string.IsNullOrEmpty(designationVM.DesignationName))
            {
                string uploadsFolder = Path.Combine(_web.WebRootPath, "images");
               // uniqueFileName = $"{designationVM.DesignationCode}_{designationVM.DesignationName}_{designationVM.ProfileImage.FileName}";
                uniqueFileName = $"{designationVM.DesignationCode}_{designationVM.DesignationName}_{designationVM.ProfileImage.FileName.Replace(" ", "_")}";

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Ensure the folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    designationVM.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        public async Task<bool> SaveAsync(DesignationVM designationVM)
        {
            await _designationRepository.BeginTransactionAsync();
            try
            {
                string uniqueFileName = UploadedFile(designationVM);
                Designation designation = new Designation();
                designation.DesignationCode = await GenerateNextDesignationCodeAsync();
                designation.DesignationName = designationVM.DesignationName ?? string.Empty;
                designation.ShortName = designationVM.ShortName ?? string.Empty;
                designation.ProfilePicture = UploadedFile(designationVM) ?? string.Empty;
                designation.LDate = designationVM.LDate ?? DateTime.Now;
                designation.ModifyDate = designationVM.ModifyDate;
                designation.LIP = GetLocalIP() ?? string.Empty;
                designation.LMAC = GetMacAddress() ?? string.Empty;
                await _designationRepository.AddAsync(designation);

                if (designationVM.Photo != null && designationVM.Photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {

                        await designationVM.Photo.CopyToAsync(memoryStream);


                        HrmEmpDigitalSignature photo = new HrmEmpDigitalSignature();
                        {
                            photo. DesignationAutoId = designation.DesignationAutoId;
                            photo.DigitalSignature = memoryStream.ToArray();
                            photo.ImgType = designationVM.Photo.ContentType;
                            photo.ImgSize = designationVM.Photo.Length;
                        };

                  
                        await hrmPhoto.AddAsync(photo);
                    }
                }
                await _designationRepository.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _designationRepository.RollbackTransactionAsync();
                return false;
            }
           
        }


        public async Task<bool> UpdateAsync(DesignationVM designationVM)
        {
            await _designationRepository.BeginTransactionAsync();
            try
            {
                var designation = await _designationRepository.GetByIdAsync(designationVM.DesignationAutoId);

                if (designation == null)
                {
                    return false;
                }

                designation.DesignationCode = designationVM.DesignationCode;
                designation.DesignationName = designationVM.DesignationName;
                designation.ShortName = designationVM.ShortName ?? " ";
                designation.ProfilePicture = UploadedFile(designationVM) ?? string.Empty;
                designation.LDate = designationVM.LDate ?? new DateTime();
                designation.ModifyDate = designationVM.ModifyDate ?? new DateTime();
                designation.LIP = GetLocalIP();
                designation.LMAC = GetMacAddress();
                await _designationRepository.UpdateAsync(designation);

                if (designationVM.IsClearPhoto)
                {
                    var existingPhoto = await hrmPhoto.All().Where(x => x.DesignationAutoId == designation.DesignationAutoId).ToListAsync();
                    await hrmPhoto.DeleteRangeAsync(existingPhoto);
                }


                if (designationVM.Photo != null && designationVM.Photo.Length > 0)
                {
                    var existingPhotos = await hrmPhoto.All().Where(x => x.DesignationAutoId == designation.DesignationAutoId).ToListAsync();
                    if (existingPhotos.Any())
                    {
                        await hrmPhoto.DeleteRangeAsync(existingPhotos);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await designationVM.Photo.CopyToAsync(memoryStream);

                        HrmEmpDigitalSignature photo = new HrmEmpDigitalSignature
                        {
                            DesignationAutoId = designation.DesignationAutoId,
                            DigitalSignature = memoryStream.ToArray(),
                            ImgType = designationVM.Photo.ContentType,
                           ImgSize = designationVM.Photo.Length
                        };

                        await hrmPhoto.UpdateAsync(photo);
                    }
                }
                else
                {

                    var existingPhoto = await hrmPhoto.All().Where(x => x.DesignationAutoId == designation.DesignationAutoId).FirstOrDefaultAsync();
                    if (existingPhoto != null)
                    {
                        existingPhoto.ImgType = existingPhoto.ImgType;
                        existingPhoto.ImgSize = existingPhoto.ImgSize;
                        await hrmPhoto.UpdateAsync(existingPhoto);
                    }
                }


                await _designationRepository.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _designationRepository.RollbackTransactionAsync();
                return false;
            }
            
        }


        public async Task<bool> DeleteAsync(int id)
        {
            await _designationRepository.BeginTransactionAsync();
            try
            {
                var designation = await _designationRepository.GetByIdAsync(id);
                if (designation == null)
                {
                    await _designationRepository.RollbackTransactionAsync();
                    return false;
                }

                var photos = await hrmPhoto.All()
                                           .Where(p => p.DesignationAutoId == id)
                                           .ToListAsync(); 

                if (photos.Any())
                {
                    foreach (var photo in photos)
                    {
                        await hrmPhoto.DeleteAsync(photo); 
                    }
                }

                await _designationRepository.DeleteAsync(designation);
                await _designationRepository.CommitTransactionAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                await _designationRepository.RollbackTransactionAsync();
                return false;
            }
            catch (Exception)
            {
                await _designationRepository.RollbackTransactionAsync();
                return false;
            }
        }


        public async Task<bool> IsDesignatioNameUniqueAsync(string designationName, int? id)
        {
            var desigantionExists = await _designationRepository.AnyAsync(c => c.DesignationName == designationName && (!id.HasValue || c.DesignationAutoId != id.Value));
            return !desigantionExists;
        }
      
        public string GetLocalIP()
        {

            string ipAddress = string.Empty;
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
               .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback);

            foreach (var networkInterface in networkInterfaces)
            {
                var properties = networkInterface.GetIPProperties();
                var ipv4Address = properties.UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                if (ipv4Address != null)
                {
                    ipAddress = ipv4Address.Address.ToString();
                    break;
                }
            }

            return ipAddress;
        }

        public string GetMacAddress()
        {


            string macAddress = string.Empty;
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                  .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback);
            foreach (var networkInterface in networkInterfaces)
            {
                macAddress = networkInterface.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(macAddress))
                {
                    break;
                }
            }

            return macAddress;
        }

        public async Task<string> GenerateNextDesignationCodeAsync()
        {
            var desigantions = await _designationRepository.GetAllAsync();
            var lastcode = desigantions.Max(x => x.DesignationCode);
            int nexCode = 1;
            if (!string.IsNullOrEmpty(lastcode))
            {
                int lastNumber = int.Parse(lastcode.TrimStart('0'));
                lastNumber++;
                nexCode = lastNumber;
            }
            return nexCode.ToString("D2");
        }


        public async Task<bool> IsExistAsync(string name,int code)
        {
            return await _designationRepository.All().AnyAsync(x => x.DesignationName == name && x.DesignationAutoId !=code);
        }
    }
}

