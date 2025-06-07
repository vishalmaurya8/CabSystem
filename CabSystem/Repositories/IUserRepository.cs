using CabSystem.Models;

namespace CabSystem.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(int userId);
        Task DeleteUserAsync(int userId);
    }
}
