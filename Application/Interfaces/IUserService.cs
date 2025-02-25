using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> Register(User user);
        Task<string?> Login(string email, string? password = null, string? googleToken = null);
        Task<List<User>> GetAll();
        Task<User?> GetOne(string id);
        Task<User?> Update(string id, User user);
        Task<User?> Delete(string id);
    }
}
