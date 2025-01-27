using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> CreateUser(User user);
        public Task<User?> GetUserById(string id);
        //public Task<List<Dog?>> GetUserAllDogsAsync(string id);
        public Task<User?> GetUserByEmail(string email);
        public Task<List<User>> GetAllUsers();
        public Task<User?> UpdateUser(string id, User user);
        // remove all dogs ????
        public Task<User?> DeleteUser(string id);

    }
}
