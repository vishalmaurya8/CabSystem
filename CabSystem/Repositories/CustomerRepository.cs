using CabSystem.Data;
using CabSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabSystem.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CabSystemContext dbcontext;

        public CustomerRepository(CabSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<List<Rating>> GetRatingsByUserIdAsync(int userId)
        {
            return await dbcontext.Ratings
                .Include(r => r.Ride)
                .Where(r => r.Ride.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Ride>> GetRidesByUserIdAsync(int userId)
        {
            return await dbcontext.Rides
                .Where(r => r.UserId == userId)
                .Include(r => r.Driver)
                    .ThenInclude(d => d.User)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await dbcontext.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
