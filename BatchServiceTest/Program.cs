
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerApp.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<UserBatchService>();
                services.AddDbContext<ServerApp.Data.AppDbContext>(options =>
                    options.UseMySql("Server=localhost;Database=MyDatabase;User=root;Password=Ms176726;", 
                    new MySqlServerVersion(new Version(8, 0, 25))));
            })
            .Build();

        await host.StartAsync();

        // Keep the application running to observe the batch service
        Console.WriteLine("Press any key to stop the application...");
        Console.ReadKey();

        await host.StopAsync();
    }
}
