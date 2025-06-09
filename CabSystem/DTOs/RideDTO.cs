namespace CabSystem.DTOs
{
    public class RideDTO
    {
        public int RideId { get; set; }
        public int UserId { get; set; }
        public int DriverId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal Fare { get; set; }
        public string Status { get; set; }
        public string  DriverNames { get; set; }
    }
}
