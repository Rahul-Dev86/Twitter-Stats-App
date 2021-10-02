using System;
using System.Collections.Generic;
using System.Linq;
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

        public string getEmojiProportion(string tweetText, int count, IList<string> emojis, int emojiCount, out int ecnt)
        {
            string msg = string.Empty;
            try
            {
                if (emojis.Any(tweetText.Contains))
                {
                    emojiCount++;
                }
                if (emojiCount > 0)
                {
                    msg = (emojiCount * 100) / count + "% of " + count + " tweets contain the emoji(s)!";
                }
                else
                {
                    msg = "No emojis found in " + count + " tweets";
                }
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
            ecnt = emojiCount;
            return msg;
        }
    }
}
