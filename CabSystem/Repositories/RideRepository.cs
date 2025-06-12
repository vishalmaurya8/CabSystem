// Repositories/RideRepository.cs
using CabSystem.Data;
using CabSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabSystem.Repositories
{
    public class RideRepository : IRideRepository
    {
        private readonly CabSystemContext _context;

        public RideRepository(CabSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ride>> GetRidesByUserIdAsync(int userId)
        {
            return await _context.Rides
                .Where(r => r.UserId == userId)
                .Include(r => r.Driver)
                .ToListAsync();
        }

        public async Task<Ride> BookRideAsync(Ride ride)
        {
            ride.Status = "Requested";
            _context.Rides.Add(ride);
            await _context.SaveChangesAsync();
            return ride;
        }

        public async Task<Ride?> CompleteRideAsync(int rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);

            if (ride == null)
                return null;

            if (ride.Status == "Completed")
                return ride;

            ride.Status = "Completed";
            await _context.SaveChangesAsync();
            return ride;
        }

        // 🆕 Updated to work with non-nullable DriverId
        public async Task<List<Ride>> GetRequestedRidesByDriverIdAsync(int driverId)
        {
            return await _context.Rides
                .Include(r => r.User)
                .Where(r => r.Status == "Requested" && r.DriverId == driverId)
                .ToListAsync();
        }

        public async Task<Ride?> AcceptRideAsync(int rideId, int driverId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null || ride.Status != "Requested")
                return null;

            // Already assigned, just update status
            if (ride.DriverId != driverId)
                return null; // Optional: prevent unauthorized acceptance

            ride.Status = "Accepted";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to assign driver: " + ex.InnerException?.Message ?? ex.Message);
            }

            return ride;
        }

        public async Task<Ride?> GetRideByIdAsync(int rideId)
        {
            return await _context.Rides
                .Include(r => r.User)
                .Include(r => r.Driver)
                .FirstOrDefaultAsync(r => r.RideId == rideId);
        }


        public async Task UpdateRideAsync(Ride ride)
        {
            _context.Rides.Update(ride);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ride>> GetUnassignedRequestedRidesAsync()
        {
            return await _context.Rides
                .Where(r => r.Status == "Requested")
                .Include(r => r.User)
                .ToListAsync();
        }
    }
}

