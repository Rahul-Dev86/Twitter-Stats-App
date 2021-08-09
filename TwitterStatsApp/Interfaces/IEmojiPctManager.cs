using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace TwitterStatsApp.Interfaces
{
    public interface IEmojiPctManager
    {
        Task<int> getEmojiProportionAsync(TwitterClient appClient, IList<string> emojis, int streamLimit);
    }
}
