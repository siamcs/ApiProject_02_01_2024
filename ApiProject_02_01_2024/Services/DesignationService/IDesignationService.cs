using ApiProject_02_01_2024.DTOs;


namespace ApiProject_02_01_2024.Services.DesignationService
{
    public interface IDesignationService
    {
        IEnumerable<CommonSelectModelVM> DropSelection();
        Task<List<DesignationVM>> GetAllAsync();
        Task<DesignationVM> GetByIdAsync(int id);
        Task<bool> SaveAsync(DesignationVM  designation);
        Task<bool> UpdateAsync(DesignationVM designation);
        Task<bool> DeleteAsync(int id);
        Task<string> GenerateNextDesignationCodeAsync();
        string GetLocalIP();
        string GetMacAddress();
        Task<bool> IsDesignatioNameUniqueAsync(string designationName, int? id);
        Task<bool> IsExistAsync(string name,int code);
    }
}
