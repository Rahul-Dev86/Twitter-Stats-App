using System;
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
        public string showTweetReceived(string tweetId, int count)
        {
            string msg = string.Empty;
            try
            {
                msg = "Number " + count + " Tweet received with Id: " + tweetId;
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
            return msg;
        }
    }
}
