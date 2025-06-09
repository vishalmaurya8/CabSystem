// Repositories/IRideRepository.cs
using CabSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CabSystem.Repositories
{
    public interface IRideRepository
    {
        Task<IEnumerable<Ride>> GetRidesByUserIdAsync(int userId);
        Task<Ride> BookRideAsync(Ride ride);
        Task<Ride?> CompleteRideAsync(int rideId);
    }
}
