using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApiProject_02_01_2024.DTOs
{
    public class DesignationVM
    {
       
        public int DesignationAutoId { get; set; }
        [Required]
        public string? DesignationCode { get; set; } = string.Empty;
        [Required]
        public string? DesignationName { get; set; } = string.Empty;
        [Required]
        public string? ShortName { get; set; } = string.Empty;

        public DateTime? LDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ModifyDate { get; set; }

        public string? PhotoUrl { get; set; }
        public bool IsClearPhoto { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
