using CabSystem.DTOs;
using CabSystem.Models;

namespace CabSystem.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating?> GetRatingByRideIdAsync(int rideId);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<Rating?> UpdateRatingAsync(int rideId, int score, string? comments);
        Task<List<Rating>> GetAllRatingsAsync();
        Task<DriverAverageRatingDTO?> GetAverageRatingForDriverAsync(int driverId);


    }
}
