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
            return await _context.Ratings
                .Include(r => r.Ride)
                .ThenInclude(ride => ride.User)
                .FirstOrDefaultAsync(r => r.RideId == rideId);
        }

        public async Task<Rating> AddRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating?> UpdateRatingAsync(int rideId, int userId, int score, string? comments)
        {
            var rating = await _context.Ratings
                .Include(r => r.Ride)
                .FirstOrDefaultAsync(r => r.RideId == rideId && r.Ride.UserId == userId);

            if (rating == null)
                return null;

            rating.Score = score;
            rating.Comments = comments;
            await _context.SaveChangesAsync();
            return rating;
        }

        /*public async Task<List<Rating>> GetAllRatingsAsync()
        {
            return await _context.Ratings.ToListAsync();
        }*/

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

        public async Task<List<Rating>> GetRatingsByUserIdAsync(int userId)
        {
            return await _context.Ratings
                .Include(r => r.Ride)
                .Where(r => r.Ride.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> IsRideOwnedByUserAsync(int rideId, int userId)
        {
            return await _context.Rides.AnyAsync(r => r.RideId == rideId && r.UserId == userId);
        }
    }
}
