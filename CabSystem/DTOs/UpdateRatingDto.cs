using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class UpdateRatingDto
    {
        [Range(1, 5)]
        public int Score { get; set; }

        public string? Comments { get; set; }
    }
}
