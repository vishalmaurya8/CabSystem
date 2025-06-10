public class RequestedRideDTO
{
    public int RideId { get; set; }
    public string PickupLocation { get; set; }
    public string DropoffLocation { get; set; }
    public decimal Fare { get; set; }
    public string PassengerName { get; set; }
}
