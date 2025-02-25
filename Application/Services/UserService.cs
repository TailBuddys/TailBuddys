using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Utils;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuth _jwtAuthService;
        private readonly GoogleAuthService _googleAuthService;


        public UserService(IUserRepository userRepository, IAuth jwtAuthService, GoogleAuthService googleAuthService)
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
                if (user == null) return null;
                return await _userRepository.CreateUserDb(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<string?> Login(string email, string? password = null, string? googleToken = null)
        {
            try
            {
                User? user = null;

                if (googleToken != null)
                {
                    // Authenticate via Google
                    user = await _googleAuthService.AuthGoogleUser(googleToken);
                }
                else if (password != null)
                {
                    // Authenticate via Email/Password
                    user = await _userRepository.GetUserByEmailDb(email);
                    if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash, user))
                    {
                        return null; // Invalid credentials
                    }
                }

                if (user == null)
                {
                    return null; // User not found or invalid login
                }

                // Generate JWT Token
                // להכניס את הכלבים לקליימס
                return _jwtAuthService.GenerateToken(user);
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
