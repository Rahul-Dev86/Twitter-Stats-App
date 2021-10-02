using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events.V2;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Managers
{
    public class ConsolidatedStatsManager : IConsolidatedStats
    {
        private readonly IExceptionLogService _exceptionLogService;
        private readonly ITweetReceivedManager _tweetReceivedManager;
        private readonly ITweetUrlManager _tweetUrlManager;
        private readonly ITopDomainManager _topDomainManager;
        private readonly IEmojiPctManager _emojiPctManager;
        private int iEmojiCnt = 0;
        public ConsolidatedStatsManager(
            IExceptionLogService exceptionLogService,
            ITweetReceivedManager tweetReceivedManager,
            ITweetUrlManager tweetUrlManager,
            ITopDomainManager topDomainManager,
            IEmojiPctManager emojiPctManager
            )
        {
            _exceptionLogService = exceptionLogService;
            _tweetReceivedManager = tweetReceivedManager;
            _tweetUrlManager = tweetUrlManager;
            _topDomainManager = topDomainManager;
            _emojiPctManager = emojiPctManager;
        }

        public async Task getConsolidatedStatsAsync(TwitterClient appClient, IList<string> emojis)
        {
            int cnt = 0; int topCurPos = Console.CursorTop;
            ConcurrentBag<string> colUrl = new ConcurrentBag<string>();
            ConcurrentBag<string> photoUrl = new ConcurrentBag<string>();            
            ConcurrentDictionary<string, int> dictDomain = new ConcurrentDictionary<string, int>();
            try
            {
                var sampleStreamV2 = appClient.StreamsV2.CreateSampleStream();
                sampleStreamV2.TweetReceived += (sender, args) =>
                {
                    cnt++;
                    processData(args, cnt, colUrl, photoUrl, dictDomain, topCurPos, emojis, iEmojiCnt);
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
        }

        private void processData(TweetV2EventArgs args, int count, ConcurrentBag<string> colUrl, ConcurrentBag<string> photoUrl, ConcurrentDictionary<string, int> dictDomain, int topCurPos, IList<string> emojis, int emojiCount)
        {
            try
            {
                var tTweet = Task.Factory.StartNew(() =>
                {
                    return _tweetReceivedManager.showTweetReceived(args.Tweet.Id, count);
                });
                tTweet.Wait();

                var tURL = Task.Factory.StartNew(() =>
                {
                    return _tweetUrlManager.getTweetsUrlPercent(args.Tweet.Entities.Urls, count, colUrl, photoUrl);
                });
                tURL.Wait();

                var tDomain = Task.Factory.StartNew(() =>
                {
                    return _topDomainManager.getTopDomain(args.Tweet.ContextAnnotations, count, dictDomain);
                });
                tDomain.Wait();

                var tEmoji = Task.Factory.StartNew(() =>
                {
                    string msg = _emojiPctManager.getEmojiProportion(args.Tweet.Text, count, emojis, emojiCount, out int ecnt);
                    iEmojiCnt = ecnt;
                    return msg;
                });
                tEmoji.Wait();

                Console.SetCursorPosition(0, topCurPos);

                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine("Tweet Received:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}", tTweet.Result);
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------------------------------");

                Console.WriteLine("URL & Photo URL proportion:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}", tURL.Result[0]);
                Console.WriteLine("{0}", tURL.Result[1]);
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------------------------------");

                Console.WriteLine("Top Domain:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}", tDomain.Result);
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------------------------------");

                Console.WriteLine("Emoji proportion:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}", tEmoji.Result);
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------------------------------");

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
        }

    }
}
