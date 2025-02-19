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
            try
            {

                if (user == null) return null;
                return await _userReposetory.CreateUser(user);
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
                User? u = await _userReposetory.GetUserByEmail(email);
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
            return await _userReposetory.GetAllUsers();
        }
        public async Task<User?> GetOne(string id)
        {
            return await _userReposetory.GetUserById(id);
        }
        public async Task<User?> Update(string id, User user)
        {
            if (user == null)
            {
                return null;
            }
            return await _userReposetory.UpdateUser(id, user);
        }
        public async Task<User?> Delete(string id)
        {
            try
            {
                return await _userReposetory.DeleteUser(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
