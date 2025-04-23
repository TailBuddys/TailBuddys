using System;
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
        private readonly ILogger<UserService> _logger;


        public UserService(IUserRepository userRepository, IAuth jwtAuthService, IGoogleAuthService googleAuthService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtAuthService = jwtAuthService;
            _googleAuthService = googleAuthService;
            _logger = logger;
        }

        // להתייחס לאדמין גם פה וגם בסרוויסים אחרים כמו פארק וכו
        public async Task<User?> Register(User user)
        {
            try
            {

                if (user == null || user.Email == null || await _userRepository.GetUserByEmailDb(user.Email) != null) return null;


                if (user.PasswordHash == null) return null;
                user.PasswordHash = PasswordHelper.GenerateHashPassword(user.PasswordHash, user);

                _logger.LogInformation("Start regeistering user - {user.Email} .", user.Email);

                return await _userRepository.CreateUserDb(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while regeinstring new user."); 
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
                    user = await _googleAuthService.AuthGoogleUser(loginModel.GoogleId);
                }
                else if (!string.IsNullOrEmpty(loginModel.Email) && !string.IsNullOrEmpty(loginModel.Password))
                {
                    user = await _userRepository.GetUserByEmailDb(loginModel.Email);
                    if (user == null || string.IsNullOrEmpty(user.PasswordHash) ||
                        !PasswordHelper.VerifyPassword(loginModel.Password, user.PasswordHash, user))
                    {
                        return null;
                    }
                }

                return user != null ? _jwtAuthService.GenerateToken(user) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while login a user.");
                return null;
            }
        }

        public async Task<List<User>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all users from the database...");

                var users = await _userRepository.GetAllUsersDb();

                _logger.LogInformation("Successfully retrieved {Count} users.", users.Count);

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users.");
                return new List<User>(); // or rethrow, depending on your handling policy
            }
        }
        public async Task<User?> GetOne(int id)
        {
            try
            {
                return await _userRepository.GetUserByIdDb(id);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error occurred getting the user");
                return null;
            }
        }
        public async Task<User?> Update(int id, User user)
        {
            try
            {
                if (user == null)
                {
                    return null;
                }
                _logger.LogInformation("Start updating user {id} .", id);

                return await _userRepository.UpdateUserDb(id, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred updating the user.");
                return null;
            }
        }
        public async Task<User?> Delete(int id)
        {
            try
            {
                return await _userRepository.DeleteUserDb(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred hile deleting user."); 
                return null;
            }
        }
    }
}
