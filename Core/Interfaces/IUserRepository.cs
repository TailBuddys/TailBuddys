using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> CreateUserDb(User user);
        public Task<User?> GetUserByIdDb(int userId);
        public Task<User?> GetUserByEmailDb(string email);
        public Task<List<User>> GetAllUsersDb();
        public Task<User?> UpdateUserDb(int userId, User user);
        // remove all dogs ????
        public Task<User?> DeleteUserDb(int userId);

    }
}
