using ApiProject_02_01_2024.DTOs;
using ApiProject_02_01_2024.Models;
using ApiProject_02_01_2024.Repository;
using Microsoft.EntityFrameworkCore;

using System.Net.NetworkInformation;

namespace ApiProject_02_01_2024.Services.DesignationService
{
    public class DesignationService:IDesignationService
    {
        private readonly IGenericRepository<Designation, int> _designationRepository;

        public DesignationService(IGenericRepository<Designation, int> designationRepository)
        {
            _designationRepository = designationRepository;
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
                ModifyDate = d.ModifyDate

            }).ToList();
        }

        public async Task<DesignationVM> GetByIdAsync(int id)
        {
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
                ModifyDate = designation.ModifyDate
            };
        }

        public async Task<bool> SaveAsync(DesignationVM designationVM)
        {
            await _designationRepository.BeginTransactionAsync();
            try
            {
                Designation designation = new Designation();
                designation.DesignationCode = await GenerateNextDesignationCodeAsync();
                designation.DesignationName = designationVM.DesignationName;
                designation.ShortName = designationVM.ShortName ?? "";
                designation.LDate = DateTime.Now;
                designation.LIP = GetLocalIP();
                designation.LMAC = GetMacAddress();
                await _designationRepository.AddAsync(designation);
                await _designationRepository.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
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
                designation.ModifyDate = DateTime.Now;
                designation.LIP = GetLocalIP();
                designation.LMAC = GetMacAddress();
                await _designationRepository.UpdateAsync(designation);
                await _designationRepository.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
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
                    return false;
                }
                await _designationRepository.DeleteAsync(designation);
                await _designationRepository.CommitTransactionAsync();
                return true;
            }
            catch(DbUpdateConcurrencyException)
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

        public async Task<List<string>> GetAllDesignationCodesAsync()
        {
            var designations = await _designationRepository.GetAllAsync();
            return designations
                .OrderBy(d => d.DesignationCode)
                .Select(d => d.DesignationCode)
                .ToList();
        }

        public async Task<string> GetLastInsertedDesignationCodeAsync()
        {
            var designations = await _designationRepository.GetAllAsync();
            var lastInsertedDesignation = designations
                .OrderByDescending(d => d.DesignationCode)
                .FirstOrDefault();

            return lastInsertedDesignation?.DesignationCode;
        }

    }
}

