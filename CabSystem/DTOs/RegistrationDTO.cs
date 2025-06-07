using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class RegistrationDTO
    {
        [Required]
        public string Role { get; set; }  // "User" or "Driver"

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
        public int Phone { get; set; }

        [Required]
        public string Password { get; set; }

        // Driver-specific fields (optional unless Role == "Driver")
        public string? LicenseNumber { get; set; }
        public string? VehicleDetails { get; set; }
        public string? Status { get; set; }
    }
}
