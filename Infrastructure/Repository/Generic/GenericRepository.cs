using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Base;
using Infrastructure.Helpers;
using System.Linq;

namespace Infrastructure.Repository.Generic
{
    public class GenericRepository : BaseRepository<ContextBase>, IGenericRepository
    {
        private readonly ContextBase _context;

        public GenericRepository(ContextBase context) : base(context)
        {
            _context = context;
        }

        public async Task<PageList<T>> GetAllPaginated<T>(PageParams pageParams, T type = null) where T : BaseEntity
        {

            IQueryable<T> query = _context.Set<T>().AsNoTracking()
                .Where(e => e.Name.ToLower()
                    .Contains(pageParams.Term.ToLower()))
                .OrderBy(e => e.Id);

            return await PageList<T>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
    }
}
