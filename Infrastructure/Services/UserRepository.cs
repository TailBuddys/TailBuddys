using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly TailBuddysContext _context;
        public UserRepository(TailBuddysContext context)
        {
            _context = context;
        }
        public async Task<User?> CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<User?> GetUserById(string userId)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<User>();
            }
        }
        public async Task<User?> UpdateUser(string userId, User user)
        {
            try
            {
                User? userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userToUpdate == null)
                {
                    return null;
                }
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Email = user.Email;
                userToUpdate.Phone = user.Phone;
                userToUpdate.BirthDate = user.BirthDate;
                userToUpdate.Gender = user.Gender;
                userToUpdate.UpdatedAt = DateTime.Now;

                _context.Users.Update(userToUpdate);
                await _context.SaveChangesAsync();
                return userToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<User?> DeleteUser(string userId)
        {
            try
            {
                User? userToRemove = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userToRemove == null)
                {
                    return null;
                }
                _context.Users.Remove(userToRemove);
                await _context.SaveChangesAsync();
                return userToRemove;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
