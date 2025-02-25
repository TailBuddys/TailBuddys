using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IAuth
    {
        public string GenerateToken(User u);
    }
}
