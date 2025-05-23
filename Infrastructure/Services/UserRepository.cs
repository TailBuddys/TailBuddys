﻿using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly TailBuddysContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(TailBuddysContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User?> CreateUserDb(User user)
        {
            try
            {
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new user.");
                return null;
            }
        }
        public async Task<User?> GetUserByIdDb(int userId)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.Dogs)
                    .FirstOrDefaultAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user by id.");
                return null;
            }
        }
        public async Task<User?> GetUserByEmailDb(string email)
        {
            try
            {
                User? user = await _context.Users
                    .Include(u => u.Dogs)
                    .FirstOrDefaultAsync(u => u.Email == email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user by Email.");
                return null;
            }
        }

        public async Task<List<User>> GetAllUsersDb()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all users.");
                return new List<User>();
            }
        }
        public async Task<User?> UpdateUserDb(int userId, User user)
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
                _logger.LogError(ex, "Error occurred while updating user.");
                return null;
            }
        }

        public async Task<User?> DeleteUserDb(int userId)
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
                _logger.LogError(ex, "Error occurred while delete user.");
                return null;
            }
        }
    }
}
