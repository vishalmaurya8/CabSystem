using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class CreateRatingDTO
    {
        public int RideId { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        public string? Comments { get; set; }
    }
}
