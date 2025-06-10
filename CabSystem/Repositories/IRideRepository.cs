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

        // 🆕 Return all rides with 'Requested' status for the current driver
        Task<List<Ride>> GetRequestedRidesByDriverIdAsync(int driverId);

        Task<Ride?> AcceptRideAsync(int rideId, int driverId);

    }
}
