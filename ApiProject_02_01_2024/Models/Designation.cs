using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProject_02_01_2024.Models
{
    [Table("Designations")]

    public class Designation
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationAutoId { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? DesignationCode { get; set; } = string.Empty;


        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string? DesignationName { get; set; } = string.Empty;


        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? ShortName { get; set; } = string.Empty;


        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? LUser { get; set; } = string.Empty;


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime")]
        public DateTime? LDate { get; set; } 


        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? LIP { get; set; } = string.Empty;



        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? LMAC { get; set; } = string.Empty;



    
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }

        [Required,MaxLength(300)]
        public string? ProfilePicture { get; set; } = string.Empty;

    }
}
