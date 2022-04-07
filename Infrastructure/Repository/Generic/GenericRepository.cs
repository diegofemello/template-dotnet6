using Microsoft.EntityFrameworkCore;
using Domain.Model.Base;
using Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Infrastructure.Repository.Generic
{
    public class GenericRepository : BaseRepository<ContextBase>, IGenericRepository
    {
        public GenericRepository(ContextBase context) : base(context) { }
    }
}
