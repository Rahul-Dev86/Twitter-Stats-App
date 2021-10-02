using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TwitterStatsApp.Configure;
using TwitterStatsApp.Interfaces;
using TwitterStatsApp.Sevices;

namespace TwitterStatsApp
{
    public class EntryPoint
    {
        private readonly IConfiguration _config;
        private readonly ITwitterAuthService _twitterAuthService;
        private readonly ITweetCountManager _tweetCountManager;
        private readonly ITopHashtagManager _topHashtagManager;
        private readonly ITopEmojiManager _topEmojiManager;
        
        private readonly IConsolidatedStats _consolidatedStats;
        public EntryPoint(
            IConfiguration configuration,
            ITwitterAuthService twitterAuthService,
            ITweetCountManager tweetCountManager,
            ITopHashtagManager topHashtagManager,
            ITopEmojiManager topEmojiManager,
            IConsolidatedStats consolidatedStats
            )
        {
            _config = configuration;
            _twitterAuthService = twitterAuthService;
            _tweetCountManager = tweetCountManager;
            _topHashtagManager = topHashtagManager;
            _topEmojiManager = topEmojiManager;            
            _consolidatedStats = consolidatedStats;
        }
        public void Run(String[] args)
        {
            DisableQuickEdit.Go();
            getAppClient();
            displayMenu();
        }

        private void getAppClient()
        {
            _ = _twitterAuthService.GetAppClient();
        }
        private void displayMenu()
        {
            Console.WriteLine("\n");
            Console.WriteLine("**********************Twitter Stats App**********************");
            Console.WriteLine("\n");
            
            Console.WriteLine("1: Display tweet count by Minute");
            Console.WriteLine("2: Display tweet count by Hour");
            Console.WriteLine("3: Display tweet count by Day");
            Console.WriteLine("4: Display top Emoji");           
            Console.WriteLine("5: Display top Hashtag/Trends");
            Console.WriteLine("6: Display consolidated stats for 'Tweet Received', 'URL & Photo URL proportion', 'Top Domain' and 'Emoji proportion'");
            Console.WriteLine("Press '7' to quit for option 1 to 5 and 'CTRL C' for option 6");            
            Console.WriteLine("Please make a selection from above options");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.WriteLine("You choose option 2 and working on 'Display tweet count by Minute'...");
                    getTweetCountByMinute();
                    break;
                case "2":
                    Console.WriteLine("You choose option 3 and working on 'Display tweet count by Hour'...");
                    getTweetCountByHour();
                    break;
                case "3":
                    Console.WriteLine("You choose option 4 and working on 'Display tweet count by Day'...");
                    getTweetCountByDay();
                    break;
                case "4":
                    Console.WriteLine("You choose option 5 and working on 'Display top Emoji'...");
                    getTopEmoji();
                    break;
                case "5":
                    Console.WriteLine("You choose option 7 and working on 'Display top Hashtag/Trends'...");
                    getTopHashtags();
                    break;
                case "6":
                    Console.WriteLine("You choose option 6 and working on 'Display consolidated stats'");
                    getConsolidatedStats();
                    break;
                case "7":
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
        private void getConsolidatedStats()
        {
            IList<string> emojis = _config.GetSection("Emojis")?.GetChildren()?.Select(x => x.Value)?.ToList();
             _consolidatedStats.getConsolidatedStatsAsync(_twitterAuthService.appClient, emojis).Wait();
        }
        private void dispose()
        {
            _twitterAuthService.appClient = null;
        }
    }
}