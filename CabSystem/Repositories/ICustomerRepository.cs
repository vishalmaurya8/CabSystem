using CabSystem.DTOs;
using CabSystem.Models;

namespace CabSystem.Repositories
{
    public interface ICustomerRepository    
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<List<Ride>> GetRidesByUserIdAsync(int userId);
        Task<List<Rating>> GetRatingsByUserIdAsync(int userId);
        Task<User?> UpdateCustomerProfileAsync(int userId, UpdateCustomerProfileDTO dto);
    }
}
