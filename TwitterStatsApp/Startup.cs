using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitterStatsApp.Interfaces;
using TwitterStatsApp.Managers;
using TwitterStatsApp.Sevices;

namespace TwitterStatsApp
{
    public static class Startup
    {
        public static IConfiguration Configuration { get; }
        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            IConfiguration configuration = builder.Build();

            services.AddSingleton(configuration);            
            
            services.AddSingleton<EntryPoint>();
            services.AddSingleton<ITwitterAuthService, TwitterAuthService>();
            services.AddSingleton<ITweetCountManager, TweetCountManager>();
            services.AddSingleton<ITweetUrlManager, TweetUrlManager>();
            services.AddSingleton<ITopHashtagManager, TopHashtagManager>();
            services.AddSingleton<ITopEmojiManager, TopEmojiManager>();
            services.AddSingleton<IEmojiPctManager, EmojiPctManager>();
            services.AddSingleton<ITweetReceivedManager, TweetReceivedManager>();
            services.AddSingleton<ITopDomainManager, TopDomainManager>();
            services.AddSingleton<IExceptionLogService, ExceptionLogService>();
            return services;
        }
    }
}
