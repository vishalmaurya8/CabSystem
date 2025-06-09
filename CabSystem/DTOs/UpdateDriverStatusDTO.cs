using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class UpdateDriverStatusDTO
    {
        [Required]
        public string Status { get; set; } // e.g., "Online", "Offline", "Busy"
    }
}