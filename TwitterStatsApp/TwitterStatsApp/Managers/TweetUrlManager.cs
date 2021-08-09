using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Managers
{
    public class TweetUrlManager : ITweetUrlManager
    {
        private readonly IExceptionLogService _exceptionLogService;
        public TweetUrlManager(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        public async Task<int> getTweetsUrlPercentAsync(TwitterClient appClient, int StreamLimit ,bool isPhotoUrl)
        {
            int per = 0;
            try
            {
                var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
                int i = 0;  string dispMsg = string.Empty;
                List<string> colUrl = new List<string>();

                sampleStreamV2.TweetReceived += (sender, args) =>
                {
                    if (args.Tweet.Entities.Urls != null)
                    {
                        if (isPhotoUrl)
                        {
                            if (args.Tweet.Entities.Urls[0].DisplayUrl != null)
                                colUrl.Add(args.Tweet.Entities.Urls[0].DisplayUrl);
                        }
                        else
                        {
                            if (args.Tweet.Entities.Urls[0].Url != null)
                                colUrl.Add(args.Tweet.Entities.Urls[0].Url);
                        }
                    }
                    ++i;
                    if (i == StreamLimit)
                    {
                        sampleStreamV2.StopStream();
                        per = colUrl.Count > 0 ? (colUrl.Count * 100) / i : 0;
                        dispMsg = isPhotoUrl ? "photo url" : "url";
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(per + "% of tweets that contain a " + dispMsg);
                        Console.ResetColor();
                    }
                };
                await sampleStreamV2.StartAsync();
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting tweet url proportion");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting tweet url proportion");
                Console.ResetColor();
            }
            return per;
        }
    }
}
