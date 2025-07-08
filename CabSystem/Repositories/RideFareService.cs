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
                    { "ahmedabad-surat", 265 },
                    { "pune-mumbai", 145 },
                    { "hyderabad-bangalore", 560 },
                    { "noida-ghaziabad", 25 },
                    { "bengaluru-mysore", 140 },
                    { "jaipur-udaipur", 400 },
                    { "chandigarh-shimla", 115 },
                    { "goa-mumbai", 590 },
                    { "agra-mathura", 60 },
                    { "varanasi-allahabad", 120 },
                    { "coimbatore-ooty", 90 },
                    { "guwahati-shillong", 100 },
                    { "bhubaneswar-puri", 60 },
                    { "patna-gaya", 100 },
                    { "kerala-cochin", 80 },
                    { "amritsar-ludhiana", 100 },
                    { "jodhpur-jaipur", 340 },
                    { "ranchi-jamshedpur", 130 },
                    { "visakhapatnam-vijayawada", 350 },
                    { "nashik-shirdi", 85 },
                    { "dehradun-haridwar", 50 },
                    { "mumbai-goa", 580 },
                    { "surat-vadodara", 150 },
                    { "bhopal-ujjain", 190 },
                    { "kanpur-lucknow", 85 },
                    { "howrah-kolkata", 12 },
                    { "hyderabad-goa", 650 },
                    { "pune-nashik", 210 },
                    { "delhi-agra", 230 },
                    { "chennai-pondicherry", 160 },
                    { "bangalore-chennai", 350 },
                    { "mumbai-ahmedabad", 530 },
                    { "kolkata-durgapur", 160 },
                    { "delhi-jaipur", 280 },
                    { "lucknow-faizabad", 130 },
                    { "indore-delhi", 800 },
                    { "nagpur-pune", 720 },
                    { "ahmedabad-mumbai", 520 },
                    { "cochin-alleppey", 55 },
                    { "jaipur-delhi", 270 },
                    { "mysore-bangalore", 145 }
                };

            return distances.TryGetValue(key, out var dist) ? dist : 10; // default 10km
        }

    }
}
