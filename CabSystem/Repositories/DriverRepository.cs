using CabSystem.Data;
using CabSystem.DTOs;
using CabSystem.Exceptions;
using CabSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabSystem.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly CabSystemContext _context;

        public DriverRepository(CabSystemContext context)
        {
            _context = context;
        }

        public async Task<Driver?> GetDriverByUserIdAsync(int userId)
        {
            return await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task UpdateDriverStatusAsync(int userId, string status)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
            if (driver == null)
                throw new NotFoundException("Driver not found.");

            driver.Status = status;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            {
                throw new Exception("DB update failed: " + (ex.InnerException?.Message ?? ex.Message));
            }
            //await _context.SaveChangesAsync();
        }

        public async Task<DriverStatsDTO> GetDriverStatsAsync(int userId)
        {
            var driver = await _context.Drivers
                .Include(d => d.Rides) //eager load related rides
                .ThenInclude(r => r.Rating) //if you're including ratings
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                throw new NotFoundException("Driver not found");

            var completedRides = driver.Rides.Where(r => r.Status == "Completed").ToList();
            var totalRidesCompleted = completedRides.Count;

            var averageRating = completedRides
                .Where(r => r.Rating != null)
                .Select(r => r.Rating.Score)
                .DefaultIfEmpty(0)
                .Average();

            return new DriverStatsDTO
            {
                TotalRidesCompleted = totalRidesCompleted,
                AverageRating = averageRating
            };
        }

        public async Task<List<Ride>> GetAssignedRidesForDriverAsync(int driverId)
        {
            return await _context.Rides
                .Where(r => r.DriverId == driverId)
                .Include(r => r.User)
                .Include(r => r.Driver)
                .Include(r => r.Rating)
                .Include(r => r.Payment)
                .ToListAsync();
        }

        public async Task<List<Ride>> GetAssignedRidesForDriverByStatusAsync(int driverId, string status)
        {
            return await _context.Rides
                .Where(r => r.DriverId == driverId && r.Status == status)
                .Include(r => r.User)
                .Include(r => r.Driver)
                .Include(r => r.Rating)
                .Include(r => r.Payment)
                .ToListAsync();
        }

        public async Task<int?> GetDriverIdByUserIdAsync(int userId)
        {
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
            return driver?.DriverId;
        }
    }
}
