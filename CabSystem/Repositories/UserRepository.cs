using CabSystem.Data;
using CabSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CabSystemContext dbcontext;

        public UserRepository(CabSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task AddUserAsync(User user)
        {
            await dbcontext.Users.AddAsync(user);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await dbcontext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            dbcontext.Users.Remove(user);
            dbcontext.SaveChangesAsync();
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
