namespace CabSystem.DTOs
{
    public class CreateRideDTO
    {
        public int UserId { get; set; }
        public int DriverId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal Fare { get; set; }
    }
}
