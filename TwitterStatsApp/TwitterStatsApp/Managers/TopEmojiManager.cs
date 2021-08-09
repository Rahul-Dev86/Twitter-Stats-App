using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tweetinvi;
using Tweetinvi.Exceptions;
using TwitterStatsApp.Interfaces;

namespace TwitterStatsApp.Managers
{
    public class TopEmojiManager : ITopEmojiManager
    {
        private readonly IExceptionLogService _exceptionLogService;
        public TopEmojiManager(IExceptionLogService exceptionLogService)
        {
            _exceptionLogService = exceptionLogService;
        }

        public async Task<KeyValuePair<string, Int64>> getTopEmojiAsync(TwitterClient appClient, string endpoint, IList<string> emojis)
        {
            KeyValuePair<string, Int64> topEmoji = new KeyValuePair<string, long>();
            try
            {                
                Dictionary<string, Int64> topEmojiDict = new Dictionary<string, Int64>();
                foreach (string emoji in emojis)
                {
                    string[] arrEmoji = emoji.Split(':');
                    var getTweetResponse = await appClient.Execute.RequestAsync(request =>
                    {
                        request.Url = endpoint + arrEmoji[0] + "&granularity=day&start_time=" + Uri.EscapeDataString(DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"));                        
                    });
                    JObject response = JObject.Parse(getTweetResponse.Content);
                    Int64 totalCnt = response.Count > 0 ? Convert.ToInt64(response["meta"]["total_tweet_count"].Value<JValue>().Value) : 0;
                    topEmojiDict.Add(arrEmoji[1], totalCnt);
                }
                topEmoji = topEmojiDict.Aggregate((l, r) => l.Value > r.Value ? l : r);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("'" + topEmoji.Key + "'" + " is the top emoji (found in " + String.Format("{0:#,##0}", topEmoji.Value) + " tweets in a day)");
                Console.ResetColor();
            }
            catch (TwitterException ex)
            {
                _exceptionLogService.LogException(ex);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting top emoji");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                _exceptionLogService.LogException(e);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error occured while getting top emoji");
                Console.ResetColor();
            }
            return topEmoji;
        }
    }
}
