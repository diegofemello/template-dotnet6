using Microsoft.EntityFrameworkCore;
using Domain.Model;

namespace Infrastructure.Context
{
    public class HostedContext : DbContext
    {
        public HostedContext(DbContextOptions<HostedContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }

}
