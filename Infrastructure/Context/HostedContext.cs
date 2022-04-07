using Microsoft.EntityFrameworkCore;
using Domain.Model;
using System;
using Domain.Model.Base;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace Infrastructure.Context
{
    public class HostedContext : DbContext
    {
        public HostedContext(DbContextOptions<HostedContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }

}
