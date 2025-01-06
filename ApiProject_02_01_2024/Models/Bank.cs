using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApiProject_02_01_2024.Models
{
    [Table("Banks")]
    [Index(nameof(BankCode), IsUnique = true)]
    [Index(nameof(BankName), IsUnique = true)]
    public class Bank
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bank id is required"), DisplayName("Bank Code")]
        [StringLength(50, ErrorMessage = "Bank id cannot be longer than 50 characters")]
        [Column(TypeName = "nvarchar(50)")]
        public string? BankCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Bank name is required"), DisplayName("Bank Name")]
        [StringLength(100, ErrorMessage = "Bank name is cannot be longer than 100 characters")]
        [Column(TypeName = "nvarchar(100)")]
        public string? BankName { get; set; } = string.Empty;

        public DateTime? LDate { get; set; }


        //Last modify date
        [DisplayName("Modify Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }

        //Computer last Internet Protocol address
        [Required]
        [StringLength(50, ErrorMessage = "Last IP address cannot be longer than 50 characters")]
        [Column(TypeName = "nvarchar(50)")]
        public string? LIP { get; set; } = string.Empty;


        //Computer last MAC address
        [Required]
        [StringLength(50, ErrorMessage = "Last MAC address cannot be longer than 50 characters")]
        [Column(TypeName = "nvarchar(50)")]
        public string? LMAC { get; set; } = string.Empty;

    }
}
