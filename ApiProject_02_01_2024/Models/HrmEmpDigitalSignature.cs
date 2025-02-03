using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiProject_02_01_2024.Models
{
    public class HrmEmpDigitalSignature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }  // Primary Key with Identity(1,1)

        public int DesignationAutoId { get; set; }

        [Required]
        public byte[] DigitalSignature { get; set; } = Array.Empty<byte>(); // Storing Image as Byte Array

        [Required]
        [MaxLength(50)]
        public string ImgType { get; set; } = string.Empty; // Example: "image/png", "image/jpeg"

        [Required]
        public long ImgSize { get; set; } // Size in bytes
    }
}
