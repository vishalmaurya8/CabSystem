using CabSystem.Data;
using CabSystem.DTOs;
using CabSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabSystem.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly CabSystemContext _context;

        public RatingRepository(CabSystemContext context)
        {
            _context = context;
        }

        public async Task<Rating?> GetRatingByRideIdAsync(int rideId)
        {
            return await _context.Ratings.SingleOrDefaultAsync(r => r.RideId == rideId);
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating?> UpdateRatingAsync(int rideId, int score, string? comments)
        {
            var rating = await GetRatingByRideIdAsync(rideId);
            if (rating == null) return null;

            rating.Score = score;
            rating.Comments = comments;
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<List<Rating>> GetAllRatingsAsync()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<DriverAverageRatingDTO?> GetAverageRatingForDriverAsync(int driverId)
        {
            var ratings = await _context.Ratings
        .Include(r => r.Ride)
        .Where(r => r.Ride.DriverId == driverId)
        .ToListAsync();

            if (!ratings.Any())
                return null;

            var average = new DriverAverageRatingDTO
            {
                DriverId = driverId,
                AverageScore = Math.Round(ratings.Average(r => r.Score), 2),
                TotalRatings = ratings.Count
            };

            return average;
        }
    }
}
