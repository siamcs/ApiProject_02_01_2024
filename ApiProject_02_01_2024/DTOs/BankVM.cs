using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApiProject_02_01_2024.DTOs
{
    public class BankVM
    {
      
        public int Id { get; set; }

        [Required(ErrorMessage = "Bank id is required"), DisplayName("Bank Code")]
        [StringLength(50, ErrorMessage = "Bank id cannot be longer than 50 characters")]
        [Column(TypeName = "nvarchar(50)")]
        public string? BankCode { get; set; } = string.Empty;


        [Required(ErrorMessage = "Bank name is required"), DisplayName("Bank Name")]
        [StringLength(100, ErrorMessage = "Bank name is cannot be longer than 100 characters")]
        [Column(TypeName = "nvarchar(100)")]
        public string? BankName { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? LDate { get; set; }



        [DisplayName("Modify Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
 
        public DateTime? ModifyDate { get; set; }



    }
}
