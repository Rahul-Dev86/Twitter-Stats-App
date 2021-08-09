using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;

namespace TwitterStatsApp.Interfaces
{
    public interface ITopEmojiManager
    {
        Task<KeyValuePair<string, Int64>> getTopEmojiAsync(TwitterClient appClient, string endpoint, IList<string> emojis);
    }
}
