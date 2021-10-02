using System;
using System.Collections.Concurrent;
using Tweetinvi.Exceptions;
using Tweetinvi.Models.V2;
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

        public string[] getTweetsUrlPercent(UrlV2[] Urls, int count, ConcurrentBag<string> colUrl, ConcurrentBag<string> photoUrl)
        {
            string[] msg = new string[2];
            try
            {
                if (Urls != null)
                {
                    if (Urls[0].DisplayUrl != null)
                        photoUrl.Add(Urls[0].DisplayUrl);

                    if (Urls[0].Url != null)
                        colUrl.Add(Urls[0].Url);
                }
                if (colUrl.Count > 0)
                {
                    msg[0] = (colUrl.Count * 100) / count + "% of " + count + " tweets contain a url";
                }
                if (photoUrl.Count > 0)
                {
                    msg[1] = (photoUrl.Count * 100) / count + "% of " + count + " tweets contain a photo url";
                }

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
            return msg;
        }
    }
}
