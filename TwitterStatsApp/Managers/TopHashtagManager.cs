using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;
using TwitterStatsApp.Models;

namespace TwitterStatsApp.Managers
{
    public class TopHashtagManager : ITopHashtagManager
    {
        private readonly IExceptionLogService _exceptionLogService;
        public TopHashtagManager(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        public async Task<List<HashtagVM>> getTopHashtagsAsync(TwitterClient appClient, string endpoint)
        {
            List<HashtagVM> topHashtags = null;
            try
            {
                var getHashtags = await appClient.Execute.RequestAsync(request =>
                    {
                        request.Url = endpoint;
                    });
                JObject response = JObject.Parse(getHashtags.Content.Substring(1, getHashtags.Content.Length - 2));
                var tweetarray = response["trends"].Value<JArray>();
                topHashtags = tweetarray.ToObject<List<HashtagVM>>();
                Console.WriteLine("Top Hashtags/Trends in USA");
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var tophashtag in topHashtags)
                {
                    Console.WriteLine(tophashtag.name);
                }
                Console.ResetColor();
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting top hashtag");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting top hashtag");
                Console.ResetColor();
            }
            return topHashtags;
        }
    }
}
