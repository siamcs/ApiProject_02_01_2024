using ApiProject_02_01_2024.DTOs;
using ApiProject_02_01_2024.Models;
using ApiProject_02_01_2024.Repository;
using Microsoft.EntityFrameworkCore;



using System.Net.NetworkInformation;

namespace ApiProject_02_01_2024.Services.BankService
{
    public class BankService : IBankService
    {
        private readonly IGenericRepository<Bank, int> _bankRepository;

        public BankService(IGenericRepository<Bank, int> bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public IEnumerable<CommonSelectModelVM> DropSelection()
        {
            return _bankRepository.All()
                .Select(x => new CommonSelectModelVM
                {
                    Code = x.BankCode,
                    Name = x.BankName
                }) .ToList(); 
        }

        public async Task<List<BankVM>> GetAllAsync()
        {
           var banks= await _bankRepository.GetAllAsync();
            return  banks.Select(b=> new BankVM
            {
                Id=b.Id,
                BankCode=b.BankCode,
                BankName=b.BankName,    
                LDate=b.LDate,
                ModifyDate=b.ModifyDate,
               

            }).ToList();
                 
        }

       
        public async Task<BankVM> GetByIdAsync(int id)
        {
           var bank=await _bankRepository.GetByIdAsync(id);
            if(bank==null)
            {
                return null;
            }
            return new BankVM
            {
                Id = bank.Id,
                BankCode = bank.BankCode,
                BankName = bank.BankName,
                LDate = bank.LDate,
                ModifyDate = bank.ModifyDate,
                //LIP=bank.LIP,
                //LMAC=bank.LMAC
              
                
            };
        }

        public async Task<bool> SaveAsync(BankVM bankVM)
        {
            await _bankRepository.BeginTransactionAsync();
            try
            {
               
                Bank bank = new Bank();
                bank.BankCode = await GenerateNextBankCodeAsync();
                bank.BankName = bankVM.BankName;
                bank.LDate = DateTime.Now;
                bank.LIP = GetLocalIP();
                bank.LMAC = GetMacAddress();
                await _bankRepository.AddAsync(bank);
                await _bankRepository.CommitTransactionAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _bankRepository.RollbackTransactionAsync();
                return false;
            }
            
        }
        public async Task<bool> UpdateAsync(BankVM bankVM)
        {
            await _bankRepository.BeginTransactionAsync();
            try
            {
                var bank = await _bankRepository.GetByIdAsync(bankVM.Id);
                if (bank == null) { return false; }
                bank.BankCode = bankVM.BankCode;
                bank.BankName = bankVM.BankName;
                bank.ModifyDate = DateTime.Now;
                await _bankRepository.UpdateAsync(bank);
                await _bankRepository.CommitTransactionAsync();
                return true;
            }
            catch(Exception)
            {
                await _bankRepository.RollbackTransactionAsync();
                return false;
            }
           
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _bankRepository.BeginTransactionAsync();
            try
            {
                var bank = await _bankRepository.GetByIdAsync(id);
                if (bank == null)
                {
                    return false;

                }
                await _bankRepository.DeleteAsync(bank);
                await _bankRepository.CommitTransactionAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                await _bankRepository.RollbackTransactionAsync();
                return false;
            }
            catch (Exception)
            {
                await _bankRepository.RollbackTransactionAsync();
                return false;
            }
           
        }
         

        public async Task<string> GenerateNextBankCodeAsync()
        {
            var banks = await _bankRepository.GetAllAsync();
            var lastCode = banks.Max(b => b.BankCode);
            int nextCode = 1;
            if (!string.IsNullOrEmpty(lastCode))
            {
                int lastNumber = int.Parse(lastCode.TrimStart('0'));
                lastNumber++;
                nextCode = lastNumber;
            }
            return nextCode.ToString("D3");
        }


        public async Task<bool> IsBankNameUniqueAsync(string bankName, int? id)
        {
            var bankExists = await _bankRepository
           .AnyAsync(b => b.BankName == bankName && (!id.HasValue || b.Id != id.Value));

            return !bankExists;
        }

        #region IPandMacAddress

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
        #endregion

    }
}
