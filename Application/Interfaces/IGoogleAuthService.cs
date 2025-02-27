using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<User?> AuthGoogleUser(string googleToken);
    }
}
