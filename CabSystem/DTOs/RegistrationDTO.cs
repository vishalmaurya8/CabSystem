using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CabSystem.DTOs
{
    public class RegistrationDTO
    {
        public class EmailDomainValidationAttribute : ValidationAttribute
        {
            private readonly string[] _allowedDomains = { "gmail.com", "outlook.com", "yahoo.com" };

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is string email)
                {
                    var domain = email.Split('@').Last();
                    if (_allowedDomains.Contains(domain))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Email domain must be gmail.com, outlook.com, or yahoo.com.");
                    }
                }
                return new ValidationResult("Invalid email format.");
            }
        }


        [Required]
        public string Role { get; set; }  // "User" or "Driver"


        [Required]
        public string Name { get; set; }


        [Required, EmailAddress, EmailDomainValidation]
        public string Email { get; set; }


        [Required, RegularExpression(@"^9\d{9}$", ErrorMessage = "Phone must be 10 digits and start with 9")]
        public long Phone { get; set; }


        [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character")]


        public string Password { get; set; }


        // Driver-specific fields (optional unless Role == "Driver")
        public string? LicenseNumber { get; set; }

        public string? VehicleDetails { get; set; }

        public string? Status { get; set; }
    }
}