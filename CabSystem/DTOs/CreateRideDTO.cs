using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class CreateRideDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

            
        public int DriverId { get; set; }

        
        [Required(ErrorMessage = "Pickup location is required")]
        public string PickupLocation { get; set; }

        
        [Required(ErrorMessage = "Dropoff location is required")]
        public string DropoffLocation { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Fare must be greater than 0")]
        public int Fare { get; set; }
    }
}
