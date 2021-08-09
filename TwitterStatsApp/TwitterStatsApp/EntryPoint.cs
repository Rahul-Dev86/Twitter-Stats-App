using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;
using TwitterStatsApp.Interfaces;
using TwitterStatsApp.Sevices;

namespace TwitterStatsApp
{
    public class EntryPoint
    {
        private readonly IConfiguration _config;
        private readonly ITwitterAuthService _twitterAuthService;
        private readonly ITweetCountManager _tweetCountManager;
        private readonly ITweetUrlManager _tweetUrlManager;
        private readonly ITopHashtagManager _topHashtagManager;
        private readonly ITopEmojiManager _topEmojiManager;
        private readonly IEmojiPctManager _emojiPctManager;
        private readonly ITweetReceivedManager _tweetReceivedManager;
        private readonly ITopDomainManager _topDomainManager;
        public EntryPoint(
            IConfiguration configuration,
            ITwitterAuthService twitterAuthService,
            ITweetCountManager tweetCountManager,
            ITweetUrlManager tweetUrlManager,
            ITopHashtagManager topHashtagManager,
            ITopEmojiManager topEmojiManager,
            IEmojiPctManager emojiPctManager,
            ITweetReceivedManager tweetReceivedManager,
            ITopDomainManager topDomainManager
            )
        {
            _config = configuration;
            _twitterAuthService = twitterAuthService;
            _tweetCountManager = tweetCountManager;
            _tweetUrlManager = tweetUrlManager;
            _topHashtagManager = topHashtagManager;
            _topEmojiManager = topEmojiManager;
            _emojiPctManager = emojiPctManager;
            _tweetReceivedManager = tweetReceivedManager;
            _topDomainManager = topDomainManager;
        }
        public void Run(String[] args)
        {
            getAppClient();
            displayMenu();
        }

        private void getAppClient()
        {
            _= _twitterAuthService.GetAppClient();
        }
        private void displayMenu()
        {
            Console.WriteLine("\n");
            Console.WriteLine("**********************Twitter Stats App**********************");
            Console.WriteLine("\n");
            Console.WriteLine("1: Display number of tweets received");
            Console.WriteLine("2: Display tweet count by Minute");
            Console.WriteLine("3: Display tweet count by Hour");
            Console.WriteLine("4: Display tweet count by Day");
            Console.WriteLine("5: Display top Emoji");
            Console.WriteLine("6: Display Emoji proportion in tweets");
            Console.WriteLine("7: Display top Hashtag/Trends");
            Console.WriteLine("8: Display URL proportion in tweets");
            Console.WriteLine("9: Display Photo URL proportion in tweets");
            Console.WriteLine("10: Display top Domain");
            Console.WriteLine("11: For quit");
            Console.WriteLine("Please make a selection from above options");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.WriteLine("You choose option 1 and working on 'Display number of tweets received'...");
                    showTweetReceived();
                    break;
                case "2":
                    Console.WriteLine("You choose option 2 and working on 'Display tweet count by Minute'...");
                    getTweetCountByMinute();
                    break;
                case "3":
                    Console.WriteLine("You choose option 3 and working on 'Display tweet count by Hour'...");
                    getTweetCountByHour();
                    break;
                case "4":
                    Console.WriteLine("You choose option 4 and working on 'Display tweet count by Day'...");
                    getTweetCountByDay();
                    break;
                case "5":
                    Console.WriteLine("You choose option 5 and working on 'Display top Emoji'...");
                    getTopEmoji();
                    break;
                case "6":
                    Console.WriteLine("You choose option 6 and working on 'Display Emoji proportion in tweets'...");
                    getEmojiProportion();
                    break;
                case "7":
                    Console.WriteLine("You choose option 7 and working on 'Display top Hashtag/Trends'...");
                    getTopHashtags();
                    break;
                case "8":
                    Console.WriteLine("You choose option 8 and working on 'Display URL proportion in tweets'...");
                    getTweetUrlPercent(false);
                    break;
                case "9":
                    Console.WriteLine("You choose option 9 and working on 'Display Photo URL proportion in tweets'...");
                    getTweetUrlPercent(true);
                    break;
                case "10":
                    Console.WriteLine("You choose option 10 and working on 'Display top Domain'...");
                    getTopDomain();
                    break;
                case "11":
                    Console.WriteLine("You choose option For quit");
                    dispose();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please make appropriate selection from below menu");
                    Console.ResetColor();
                    displayMenu();
                    break;
            }


        }
        private void showTweetReceived()
        {
            _ = _tweetReceivedManager.showTweetReceivedAsync(_twitterAuthService.appClient, Convert.ToInt16(_config.GetSection("StreamLimit").Value)).Result;
            displayMenu();
        }
        private void getTweetCountByMinute()
        {
            string query = "covid&granularity=minute&start_time=" + Uri.EscapeDataString(DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"));
            string endpoint = _config["EndpointUrl:TweetCount"] + query;
            _ = _tweetCountManager.getTweetCountAsync(_twitterAuthService.appClient, endpoint).Result;
            displayMenu();
        }
        private void getTweetCountByHour()
        {
            string query = "covid&granularity=hour&start_time=" + Uri.EscapeDataString(DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"));
            string endpoint = _config["EndpointUrl:TweetCount"] + query;
            _ = _tweetCountManager.getTweetCountAsync(_twitterAuthService.appClient, endpoint).Result;
            displayMenu();
        }
        private void getTweetCountByDay()
        {
            string query = "covid&granularity=day&start_time=" + Uri.EscapeDataString(DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"));
            string endpoint = _config["EndpointUrl:TweetCount"] + query;
            _ = _tweetCountManager.getTweetCountAsync(_twitterAuthService.appClient, endpoint).Result;
            displayMenu();
        }
        private void getTweetUrlPercent(bool isPhotoUrl)
        {
            _ = _tweetUrlManager.getTweetsUrlPercentAsync(_twitterAuthService.appClient, Convert.ToInt16(_config.GetSection("StreamLimit").Value), isPhotoUrl).Result;
            displayMenu();
        }
        private void getTopHashtags()
        {
            _ = _topHashtagManager.getTopHashtagsAsync(_twitterAuthService.appClient, _config["EndpointUrl:TopHashtag"]).Result;
            displayMenu();
        }
        private void getTopEmoji()
        {
            string endpoint = _config["EndpointUrl:TweetCount"];
            IList<string> emojis = _config.GetSection("DictEmojis")?.GetChildren()?.Select(x => x.Value)?.ToList();
            _ = _topEmojiManager.getTopEmojiAsync(_twitterAuthService.appClient, endpoint, emojis).Result;
            displayMenu();
        }
        private void getEmojiProportion()
        {
            IList<string> emojis = _config.GetSection("Emojis")?.GetChildren()?.Select(x => x.Value)?.ToList();
            _ = _emojiPctManager.getEmojiProportionAsync(_twitterAuthService.appClient, emojis, Convert.ToInt16(_config.GetSection("StreamLimit").Value)).Result;
            displayMenu();
        }
        private void getTopDomain()
        {
            _ = _topDomainManager.getTopDomainAsync(_twitterAuthService.appClient, Convert.ToInt16(_config.GetSection("StreamLimit").Value)).Result;
            displayMenu();
        }
        private void dispose()
        {
            _twitterAuthService.appClient = null;
        }
    }
}