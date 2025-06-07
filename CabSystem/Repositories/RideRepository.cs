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
    }
}
