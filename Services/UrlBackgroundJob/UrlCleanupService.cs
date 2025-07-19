using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using URLShortenerApiApplication.Data;

namespace URLShortenerApiApplication.Services.UrlBackgroundJob
{
    public class UrlCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UrlCleanupService(IServiceScopeFactory serviceScope)
        {
            _scopeFactory = serviceScope;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var tt = _context.Urls.Where(u => u.ExpireAt <= DateTime.UtcNow).ToList();
                    if (tt.Any())
                    {
                        _context.Urls.RemoveRange(tt);
                        await _context.SaveChangesAsync();
                    }
                }

                await Task.Delay(5000);

            }
        }
    }

}
