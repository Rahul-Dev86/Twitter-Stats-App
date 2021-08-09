using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;

namespace TwitterStatsApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            var services = Startup.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

             serviceProvider.GetService<EntryPoint>().Run(args);
        }
    }
}
