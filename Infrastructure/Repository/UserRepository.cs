using Microsoft.EntityFrameworkCore;
using Infrastructure.Repository.Interfaces;
using Domain.Model;
using Infrastructure.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Infrastructure.Helpers;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextBase _context;
        public UserRepository(ContextBase context)
        {
            _context = context;
        }

        public async Task<PageList<User>> GetAll(PageParams pageParams)
        {
            if(pageParams == null) pageParams = new();

            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking().Where(e => 
                e.UserName.ToLower().Contains(pageParams.Term.ToLower())
                || e.Email.ToLower().Contains(pageParams.Term.ToLower())
                || e.FullName.ToLower().Contains(pageParams.Term.ToLower())
                )
                    .OrderBy(e => e.Uid).AsNoTracking();

            return await PageList<User>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);

            //return await query.AsNoTracking().ToListAsync();
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