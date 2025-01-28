using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> CreateUser(User user);
        public Task<User?> GetUserById(string userId);
        public Task<User?> GetUserByEmail(string email);
        public Task<List<User>> GetAllUsers();
        public Task<User?> UpdateUser(string userId, User user);
        // remove all dogs ????
        public Task<User?> DeleteUser(string userId);

    }
}
