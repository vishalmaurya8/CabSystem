using CabSystem.DTOs;
using CabSystem.Models;

namespace CabSystem.Repositories
{
    public interface IRatingRepository
    {
        Task<Rating?> GetRatingByRideIdAsync(int rideId);
        Task<List<Rating>> GetRatingsByUserIdAsync(int userId);
        Task<Rating> AddRatingAsync(Rating rating);
        Task<Rating?> UpdateRatingAsync(int rideId, int userId, int score, string? comments);
        Task<bool> IsRideOwnedByUserAsync(int rideId, int userId); // 🔐 for security check
        //Task<List<Rating>> GetAllRatingsAsync();
        Task<DriverAverageRatingDTO?> GetAverageRatingForDriverAsync(int driverId);


    }
}
