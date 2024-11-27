using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Service.Admin.IService;

namespace vigo.Service.Admin
{
    public class VigoBackGroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public VigoBackGroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }
                var delay = nextRun - now;
                await Task.Delay(delay, stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var backupService = scope.ServiceProvider.GetRequiredService<ICheckOutService>();

                    try
                    {
                        await backupService.CheckOutEveryday(nextRun);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
