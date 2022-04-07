using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using Infrastructure.Repository.Generic;

namespace Application.Services.BackgroundServices
{
    public class ExampleHostedService : CronJobService
    {
        private readonly IHostedRepository _hostedRepository;

        public ExampleHostedService(IScheduleConfig<ExampleHostedService> config,
                                 IHostedRepository hostedRepository)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _hostedRepository = hostedRepository;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cron is starting.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            int countUsers = await _hostedRepository.CountAll<User>();
            Console.WriteLine("Cron is running. " + countUsers);

            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cron is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
