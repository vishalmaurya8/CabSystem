using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class DriverStatsDTO
    {
        public int TotalRidesCompleted { get; set; }
        public double AverageRating { get; set; }
    }
}