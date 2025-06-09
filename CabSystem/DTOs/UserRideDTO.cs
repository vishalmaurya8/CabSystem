namespace CabSystem.DTOs
{
    public class UserRideDTO
    {
        public int RideId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal Fare { get; set; }
        public string Status { get; set; }
        public string? DriverName { get; set; }
    }
}
