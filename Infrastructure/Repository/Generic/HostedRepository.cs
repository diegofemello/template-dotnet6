using Infrastructure.Context;

namespace Infrastructure.Repository.Generic
{
    public class HostedRepository : BaseRepository<HostedContext>, IHostedRepository
    {
        public HostedRepository(HostedContext context) : base(context) { }
    }
}
