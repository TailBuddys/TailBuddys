using System.Text.Json;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public GoogleAuthService(HttpClient httpClient, IUserRepository userRepository, IConfiguration config)
        {
            _httpClient = httpClient;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<User?> AuthGoogleUser(string googleToken)
        {
            var googleClientId = _config["Google:ClientId"];
            string url = $"https://oauth2.googleapis.com/tokeninfo?id_token={googleToken}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            GoogleUser? googleUser = JsonSerializer.Deserialize<GoogleUser>(await response.Content.ReadAsStringAsync());

            if (googleUser == null || googleUser.Audience != googleClientId)
                return null;

            User? user = await _userRepository.GetUserByEmailDb(googleUser.Email);
            Console.WriteLine(googleUser.Email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = googleUser.GivenName,
                    LastName = googleUser.FamilyName,
                    Email = googleUser.Email,
                    GoogleId = googleUser.Sub
                };
                return await _userRepository.CreateUserDb(user);
            }
            return user;
        }
    }
}
