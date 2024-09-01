using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SynapseOrders.Services;
using System.Threading.Tasks;
using SynapseOrders.Models;

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
            });

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