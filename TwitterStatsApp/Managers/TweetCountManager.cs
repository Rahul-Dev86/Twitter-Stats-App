using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;
using TwitterStatsApp.Models;
using TwitterStatsApp.Sevices;

namespace TwitterStatsApp.Managers
{
    public class TweetCountManager : ITweetCountManager
    {
        private readonly IExceptionLogService _exceptionLogService;
        public TweetCountManager(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }
        public async Task<List<TweetCountVM>> getTweetCountAsync(TwitterClient appClient, string endpoint)
        {
            List<TweetCountVM> tweetscount = null;
            try
            {
                var getTweetResponse = await appClient.Execute.RequestAsync(request =>
                {
                    request.Url = endpoint;
                });

                JObject response = JObject.Parse(getTweetResponse.Content);
                var tweetarray = response["data"].Value<JArray>();
                tweetscount = tweetarray.ToObject<List<TweetCountVM>>();
                Console.WriteLine("Start Time:\t\t End Time:\t\t Tweet Count:");
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var tweetcnt in tweetscount)
                {
                    Console.WriteLine(tweetcnt.start + "\t" + tweetcnt.end + "\t" + tweetcnt.tweet_count);
                }
                Console.ResetColor();
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting tweet count");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting tweet count");
                Console.ResetColor();
            }
            return tweetscount;
        }
    }
}
