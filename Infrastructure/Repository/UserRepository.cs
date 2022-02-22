using Microsoft.EntityFrameworkCore;
using Infrastructure.Repository.Interfaces;
using Domain.VO.Request;
using Domain.Model;
using Infrastructure.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextBase _context;
        public UserRepository(ContextBase context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAll()
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking().OrderBy(u => u.Uid);

            return await query.ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking().OrderBy(u => u.Uid)
                .Where(u => u.Uid == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> Delete(User user)
        {

            _context.Users.Remove(user);

            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<User> ValidateCredentials(AuthVO user)
        {
            string pass = PasswordHash(user.Password);

            IQueryable<User> query = _context.Users;

            return await query.AsNoTracking().FirstOrDefaultAsync(
                u => (u.UserName == user.UserName || u.Email == user.UserName) && (u.Password == pass));
        }

        public async Task<bool> RevokeToken(string userName)
        {
            User user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => (u.UserName == userName));
            if (user is null) return false;
            user.RefreshToken = null;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> RefreshUserInfo(User user)
        {
            if (!_context.Users.AsNoTracking().Any(p => p.Uid == user.Uid)) return null;

            _context.Update(user);
            if (await _context.SaveChangesAsync() > 0)
            {
                User result = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Uid == user.Uid);

                return result;
            }
            else
            {
                throw new Exception("Ocorreu um erro.");
            }
        }

        public string PasswordHash(string input) => Hash(input);

        private static string Hash(string input) {
            SHA256 algorithm = SHA256.Create();

            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }
    }
}