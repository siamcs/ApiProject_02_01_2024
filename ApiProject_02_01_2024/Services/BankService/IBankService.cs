

using ApiProject_02_01_2024.DTOs;

namespace ApiProject_02_01_2024.Services.BankService
{
    public interface IBankService
    {
      
        Task<List<BankVM>> GetAllAsync();
        Task<BankVM> GetByIdAsync(int id);
        Task<bool> SaveAsync(BankVM  bankVM);
        Task<bool> UpdateAsync(BankVM bankVM);
        Task<bool> DeleteAsync(int id);
        Task<string> GenerateNextBankCodeAsync();
        IEnumerable<CommonSelectModelVM> DropSelection();
        string GetLocalIP();
        string GetMacAddress();
        Task<bool> IsBankNameUniqueAsync(string bankName, int? id); 

    }
}



