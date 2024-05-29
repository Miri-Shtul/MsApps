
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerApp.Data;
using ServerApp.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp.Services
{
    public class UserBatchService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UserBatchService> _logger;

        public UserBatchService(IServiceProvider serviceProvider, ILogger<UserBatchService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("UserBatchService running at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // Example batch processing: send emails to users
                    var users = dbContext.Users.ToList();
                    foreach (var user in users)
                    {
                        // Simulate sending email
                        _logger.LogInformation($"Sending email to {user.Email}");
                    }
                }

                await Task.Delay(TimeSpan.FromDays(7), stoppingToken); // Runs every week
            }
        }
    }
}
