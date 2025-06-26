using CabSystem.DTOs;
using CabSystem.Models;

namespace CabSystem.Repositories
{
    public interface IDriverRepository
    {
        Task<Driver?> GetDriverByUserIdAsync(int userId);
        Task UpdateDriverStatusAsync(int userId, string status);
        Task<DriverStatsDTO> GetDriverStatsAsync(int userId);
        Task UpdateDriverProfileAsync(int userId, UpdateDriverProfileDTO dto);
        Task<List<Ride>> GetAssignedRidesForDriverAsync(int driverId);
        Task<List<Ride>> GetAssignedRidesForDriverByStatusAsync(int driverId, string status);
        Task<int?> GetDriverIdByUserIdAsync(int userId);
    }
}