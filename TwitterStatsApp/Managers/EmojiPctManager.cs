using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Managers
{
    public class EmojiPctManager : IEmojiPctManager
    {
        private readonly IExceptionLogService _exceptionLogService;
        public EmojiPctManager(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        public async Task<int> getEmojiProportionAsync(TwitterClient appClient, IList<string> emojis, int streamLimit)
        {
            int per = 0;
            try
            {
                var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();

                int i = 0; int emojiCnt = 0; 
                sampleStreamV2.TweetReceived += (sender, args) =>
                {
                    i++;
                    if (emojis.Any(args.Tweet.Text.Contains))
                        emojiCnt++;

                    if (i == streamLimit)
                    {
                        sampleStreamV2.StopStream();
                        per = emojiCnt > 0 ? (emojiCnt * 100) / i : 0;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(per + "% of tweets contain the emoji(s)!");
                        Console.ResetColor();
                    }
                };

                await sampleStreamV2.StartAsync();
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting emoji proportion");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting emoji proportion");
                Console.ResetColor();
            }
            return per;
        }
    }
}
