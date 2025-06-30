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
                .Include(d => d.Rides)
                    .ThenInclude(r => r.Rating)
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

            var totalFare = completedRides.Sum(r => r.Fare);
            var totalProfit = Math.Round(totalFare * 0.8m, 2);

            return new DriverStatsDTO
            {
                TotalRidesCompleted = totalRidesCompleted,
                AverageRating = averageRating,
                TotalProfit = totalProfit
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


        public async Task UpdateDriverProfileAsync(int userId, UpdateDriverProfileDTO dto)
        {
            var driver = await _context.Drivers
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);

            if (driver == null)
                throw new NotFoundException("Driver not found.");

            // Update User fields
            driver.User.Email = dto.Email;
            if (long.TryParse(dto.Phone, out var phone))
                driver.User.Phone = phone;
            else
                throw new BadRequestException("Invalid phone number format.");

            // Update Driver fields
            driver.LicenseNo = dto.LicenseNo;
            driver.VehicleDetails = dto.VehicleDetails;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Ride>> GetCompletedRidesForDriverAsync(int driverId)
        {
            return await _context.Rides
                .Where(r => r.DriverId == driverId && r.Status == "Completed")
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<DriverEarningsDTO> GetDriverEarningsAsync(int userId)
        {
            // Get the driver's ID
            var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.UserId == userId);
            if (driver == null)
                throw new NotFoundException("Driver not found.");

            // Sum fares of completed rides for this driver
            var totalFare = await _context.Rides
                .Where(r => r.DriverId == driver.DriverId && r.Status == "Completed")
                .SumAsync(r => (decimal?)r.Fare) ?? 0m;

            var driverProfit = Math.Round(totalFare * 0.8m, 2);

            return new DriverEarningsDTO
            {
                TotalFare = totalFare,
                DriverProfit = driverProfit
            };
        }

    }
}
