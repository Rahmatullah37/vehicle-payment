using System.ComponentModel.DataAnnotations;

namespace VisualSoft.Surveillance.Payment.API.Models.CreateRequest
{
    public class PackageCreateRequestModel
    {

        //public string Package_Type { get; set; }
        //public decimal Package_Cost { get; set; }
        //public bool Is_Active { get; set; } = true;

        [Required]
        [StringLength(100, ErrorMessage = "Package type cannot exceed 100 characters")]
        public string Package_Type { get; set; } = string.Empty;

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Package cost must be greater than 0")]
        public decimal Package_Cost { get; set; }

        
    }

}
