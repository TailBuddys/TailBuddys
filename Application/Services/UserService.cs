using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userReposetory;

        public UserService(IUserRepository userReposetory)
        {
            _userReposetory = userReposetory;
        }

        public async Task<User?> Register(User user)
        {
            if (user == null) return null;
            return await _userReposetory.CreateUser(user);
        }

        public async Task<string?> Login()
        {
            try
            {
                User? u = await _userReposetory.GetUserByEmail("");
                if (u != null)
                {
                    return "";
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


    }
}
