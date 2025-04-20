using Microsoft.EntityFrameworkCore;
using SimpleLinkShrink.Data.Entity;

namespace SimpleLinkShrink.BackgroundServices
{
    public class InitDbService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InitDbService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LinkDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
