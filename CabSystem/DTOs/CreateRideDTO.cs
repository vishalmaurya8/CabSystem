using System.ComponentModel.DataAnnotations;

public class CreateRideDTO
{
    

    [Required(ErrorMessage = "Driver ID is required")]
    public int DriverId { get; set; }

    [Required(ErrorMessage = "Pickup location is required")]
    public string PickupLocation { get; set; }

    [Required(ErrorMessage = "Dropoff location is required")]
    public string DropoffLocation { get; set; }
}
