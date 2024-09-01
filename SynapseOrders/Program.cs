using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SynapseOrders.Services;

namespace SynapseOrders
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add Logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                // We could add other logging options here
                // For example writing to a log file
                // Which We would store the location in a config file
                // Would have implemented Serilog if I had time
            });

            // Start of adding Serilog
            //Log.Logger = new LoggerConfiguration()
            //   .WriteToConsole()  // Optional: Logs to console
            //   .WriteTo.File("logs/app-log-.txt", rollingInterval: RollingInterval.Day)  // Logs to file, new file each day
            //   .CreateLogger();

            services.AddHttpClient();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<SynapseOrderProcessing>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<SynapseOrderProcessing>();
            var result = await app.ProcessOrders();

            // We could do something here with the result;
        }
    }
}