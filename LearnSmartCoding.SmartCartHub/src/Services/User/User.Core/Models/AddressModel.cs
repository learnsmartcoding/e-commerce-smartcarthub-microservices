using System.ComponentModel.DataAnnotations;

namespace User.Core.Models
{
    public class AddressModel
    {
        public int AddressId { get; set; }

        public int? UserId { get; set; }


        [Required(ErrorMessage = "Street is required")]
        [MaxLength(255, ErrorMessage = "Street cannot exceed 255 characters")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [MaxLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "State is required")]
        [MaxLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "ZipCode is required")]
        [MaxLength(20, ErrorMessage = "ZipCode cannot exceed 20 characters")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid ZipCode format")]
        public string ZipCode { get; set; } = null!;

        public bool IsShippingAddress { get; set; }
    }
}
