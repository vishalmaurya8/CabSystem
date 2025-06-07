namespace CabSystem.DTOs
{
    public class RatingDTO
    {
        public int RatingId { get; set; }
        public int RideId { get; set; }
        public int Score { get; set; }
        public string? Comments { get; set; }
    }
}
