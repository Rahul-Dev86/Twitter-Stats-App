using System.Collections.Concurrent;
using Tweetinvi.Models.V2;

namespace TwitterStatsApp.Interfaces
{
    public interface ITweetUrlManager
    {
        string[] getTweetsUrlPercent(UrlV2[] Urls, int count, ConcurrentBag<string> colUrl, ConcurrentBag<string> photoUrl);
    }
}
