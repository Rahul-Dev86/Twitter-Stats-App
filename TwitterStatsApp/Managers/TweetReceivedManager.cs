using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Managers
{
    public class TweetReceivedManager : ITweetReceivedManager
    {
        private readonly IExceptionLogService _exceptionLogService;
        public TweetReceivedManager(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        public async Task<int> showTweetReceivedAsync(TwitterClient appClient, int streamLimit)
        {
            int tweetCount = 1;
            try
            {
                var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
                sampleStreamV2.TweetReceived += (sender, args) =>
                {
                    if (tweetCount == streamLimit)
                        sampleStreamV2.StopStream();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("No. " + tweetCount + " Tweet received with Id: " + args.Tweet.Id);
                    Console.ResetColor();
                    tweetCount++;
                };

                await sampleStreamV2.StartAsync();
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while receiving tweets");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while receiving tweet");
                Console.ResetColor();
            }
            return tweetCount;
        }
    }
}
