namespace CabSystem.DTOs
{
    public class CompletedRideDTO
    {
        public int RideId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal Fare { get; set; }
        public string Status { get; set; }
        public DateTime? CompletedAt { get; set; } // Optional: if you track completion time
    }
}