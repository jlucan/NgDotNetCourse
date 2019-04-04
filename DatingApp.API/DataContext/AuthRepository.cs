using DatingApp.API.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PswdHash = passwordHash;
            user.PswdSalt = passwordSalt;

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(usr => usr.UserName == userName);
            if (user == null) return null;
            if (!VerifyUserPassword(password, user)) return null;
            return user;
        }

        private bool VerifyUserPassword(string password, User user)
        {
            byte[] userHash = user.PswdHash;
            using (var hmac = new System.Security.Cryptography.HMACSHA512(user.PswdSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                if (user.PswdHash.Length != computedHash.Length) return false;
                for (int idx = 0; idx < computedHash.Length; idx++)
                {
                    if (user.PswdHash[idx] != computedHash[idx]) return false;
                }
            }
            return true;
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _context.User.AnyAsync(u => u.UserName == userName);
        }


    }
}