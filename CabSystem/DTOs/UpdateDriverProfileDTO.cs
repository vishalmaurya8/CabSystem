using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class UpdateDriverProfileDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string LicenseNo { get; set; } = string.Empty;

        [Required]
        public string VehicleDetails { get; set; } = string.Empty;
    }
}