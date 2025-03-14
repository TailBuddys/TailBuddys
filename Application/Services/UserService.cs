using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Utils;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuth _jwtAuthService;
        private readonly IGoogleAuthService _googleAuthService;


        public UserService(IUserRepository userRepository, IAuth jwtAuthService, IGoogleAuthService googleAuthService)
        {
            _userRepository = userRepository;
            _jwtAuthService = jwtAuthService;
            _googleAuthService = googleAuthService;
        }

        // להתייחס לאדמין גם פה וגם בסרוויסים אחרים כמו פארק וכו
        public async Task<User?> Register(User user)
        {
            try
            {

                if (user == null || user.Email == null || await _userRepository.GetUserByEmailDb(user.Email) != null) return null;


                if (user.PasswordHash == null) return null;
                user.PasswordHash = PasswordHelper.GenerateHashPassword(user.PasswordHash, user);


                return await _userRepository.CreateUserDb(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<string?> Login(LoginModel loginModel)
        {
            try
            {
                if (loginModel.Email != null && loginModel.Password != null && loginModel.GoogleId != null)
                    return null; // Invalid request, all fields shouldn't be filled

                User? user = null;

                if (!string.IsNullOrEmpty(loginModel.GoogleId))
                {
                    Console.WriteLine("google id is not null here!!!"); //-----
                    user = await _googleAuthService.AuthGoogleUser(loginModel.GoogleId);
                }
                else if (!string.IsNullOrEmpty(loginModel.Email) && !string.IsNullOrEmpty(loginModel.Password))
                {
                    user = await _userRepository.GetUserByEmailDb(loginModel.Email);
                    if (user == null || string.IsNullOrEmpty(user.PasswordHash) ||
                        !PasswordHelper.VerifyPassword(loginModel.Password, user.PasswordHash, user))
                    {
                        return null; // Invalid credentials
                    }
                }

                return user != null ? _jwtAuthService.GenerateToken(user) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to login {ex.Message}");
                return null;
            }
        }

        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAllUsersDb();
        }
        public async Task<User?> GetOne(int id)
        {
            return await _userRepository.GetUserByIdDb(id);
        }
        public async Task<User?> Update(int id, User user)
        {
            if (user == null)
            {
                return null;
            }
            return await _userRepository.UpdateUserDb(id, user);
        }
        public async Task<User?> Delete(int id)
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
