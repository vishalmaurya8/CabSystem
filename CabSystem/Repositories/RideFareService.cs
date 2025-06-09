using CabSystem.Repositories;
using CabSystem.Services;

namespace CabSystem.Services
{
    public class RideFareService : IRideFareService
    {
        public decimal CalculateFare(string pickup, string dropoff)
        {
            double distance = CalculateDistanceMock(pickup, dropoff);
            decimal baseFare = 50;
            decimal ratePerKm = 10;
            return baseFare + (decimal)distance * ratePerKm;
        }

        private double CalculateDistanceMock(string start, string end)
        {
            var key = $"{start.ToLower()}-{end.ToLower()}";

            var distances = new Dictionary<string, double>
    {
        { "delhi-noida", 15 },
        { "chennai-kerala", 685 },
        { "mumbai-pune", 150 },
        { "bangalore-hyderabad", 570 },
        { "kolkata-howrah", 10 },
        { "delhi-delhi", 2 },
        { "lucknow-kanpur", 80 },
        { "indore-bhopal", 190 },
        { "nagpur-mumbai", 820 },
        { "ahmedabad-surat", 265 }
    };

            return distances.TryGetValue(key, out var dist) ? dist : 10; // default 10km
        }

    }
}
