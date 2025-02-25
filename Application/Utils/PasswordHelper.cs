using Microsoft.AspNetCore.Identity;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Utils
{
    public class PasswordHelper
    {
        private static readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        public static string GenerateHashPassword(string password, User user)
        {
            user.PasswordHash = "";
            return passwordHasher.HashPassword(user, password);
        }

        public static bool VerifyPassword(string password, string hashPassword, User user)
        {
            user.PasswordHash = "";
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, hashPassword, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }
            return true;
        }
    }
}
