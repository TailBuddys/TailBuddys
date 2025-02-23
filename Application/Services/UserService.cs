using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // להתייחס לאדמין גם פה וגם בסרוויסים אחרים כמו פארק וכו
        public async Task<User?> Register(User user)
        {
            try
            {

                if (user == null) return null;
                return await _userRepository.CreateUserDb(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<string?> Login(string email)
        {
            try
            {
                User? u = await _userRepository.GetUserByEmailDb(email);
                if (u != null)
                {
                    return "yay";
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAllUsersDb();
        }
        public async Task<User?> GetOne(string id)
        {
            return await _userRepository.GetUserByIdDb(id);
        }
        public async Task<User?> Update(string id, User user)
        {
            if (user == null)
            {
                return null;
            }
            return await _userRepository.UpdateUserDb(id, user);
        }
        public async Task<User?> Delete(string id)
        {
            try
            {
                return await _userRepository.DeleteUserDb(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
