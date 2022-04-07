using Microsoft.EntityFrameworkCore;
using Infrastructure.Repository.Interfaces;
using Domain.Model;
using Infrastructure.Context;
using System;
using System.Linq;
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
            IQueryable<User> query = _context .Users.
                OrderBy(u => u.Uid);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            IQueryable<User> query = _context.Users
                .OrderBy(u => u.Uid)
                .Where(u => u.Uid == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}