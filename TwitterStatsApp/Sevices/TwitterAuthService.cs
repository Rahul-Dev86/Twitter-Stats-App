using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Microsoft.Extensions.Configuration;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Sevices
{
    public class TwitterAuthService : ITwitterAuthService
    {
        private readonly IConfiguration _config;
        private readonly IExceptionLogService _exceptionLogService;
        public TwitterClient appClient { get; set; }
        public TwitterAuthService(IConfiguration configuration, IExceptionLogService exceptionLogService)
        {
            _config = configuration;
            _exceptionLogService = exceptionLogService;
        }
        public async Task GetAppClient()
        {
            try
            {
                // get the bearer token and create a client
                var consumerOnlyCredentials = new ConsumerOnlyCredentials(_config["Credentials:ConsumerKey"], _config["Credentials:ConsumerSecret"]);
                var appClientWithoutBearer = new TwitterClient(consumerOnlyCredentials);

                var bearerToken = await appClientWithoutBearer.Auth.CreateBearerTokenAsync();
                var appCredentials = new ConsumerOnlyCredentials(_config["Credentials:ConsumerKey"], _config["Credentials:ConsumerSecret"])
                {
                    BearerToken = bearerToken
                };
                appClient = new TwitterClient(appCredentials);
               
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while authenticating with Twitter");
                Console.ResetColor();
            }
            catch(Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while authenticating with Twitter");
                Console.ResetColor();
            }
        }

        public async Task<TwitterClient> GetAppClientForUnitTests()
        {
            try
            {
                var consumerOnlyCredentials = new ConsumerOnlyCredentials("Your-CounsumerKey", "Your-CounsumerSecretKey");
                var appClientWithoutBearer = new TwitterClient(consumerOnlyCredentials);

                var bearerToken = await appClientWithoutBearer.Auth.CreateBearerTokenAsync();
                var appCredentials = new ConsumerOnlyCredentials("Your-CounsumerKey", "Your-CounsumerSecretKey")
                {
                    BearerToken = bearerToken
                };
                appClient = new TwitterClient(appCredentials);

            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while authenticating with Twitter");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while authenticating with Twitter");
                Console.ResetColor();
            }
            return appClient;
        }
    }
}
