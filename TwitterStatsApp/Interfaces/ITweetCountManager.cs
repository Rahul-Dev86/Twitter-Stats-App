using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using TwitterStatsApp.Models;

namespace TwitterStatsApp.Interfaces
{
    public interface ITweetCountManager
    {
        Task<List<TweetCountVM>> getTweetCountAsync(TwitterClient appClient, string endpoint);
    }
}
