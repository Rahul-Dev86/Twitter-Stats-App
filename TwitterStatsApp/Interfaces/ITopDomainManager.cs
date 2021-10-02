using System.Collections.Concurrent;
using Tweetinvi.Models.V2;

namespace TwitterStatsApp.Interfaces
{
    public interface ITopDomainManager
    {
        string getTopDomain(TweetContextAnnotationV2[] annotations, int count, ConcurrentDictionary<string, int> dictDomain);
    }
}
