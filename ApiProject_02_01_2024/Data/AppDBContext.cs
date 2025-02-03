using ApiProject_02_01_2024.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProject_02_01_2024.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Bank>? Banks { get; set; }  
        public DbSet<Designation> Designations { get; set; }
        public DbSet<HrmEmpDigitalSignature> HrmEmpDigitalSignatures { get; set; }
    }
}
