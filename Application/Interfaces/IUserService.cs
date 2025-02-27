using TailBuddys.Core.Models;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> Register(User user);
        Task<string?> Login(LoginModel loginModel);
        Task<List<User>> GetAll();
        Task<User?> GetOne(string id);
        Task<User?> Update(string id, User user);
        Task<User?> Delete(string id);
    }
}
