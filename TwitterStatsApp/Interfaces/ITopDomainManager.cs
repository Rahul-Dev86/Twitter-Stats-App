using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;

namespace TwitterStatsApp.Interfaces
{
    public interface ITopDomainManager
    {
        Task<KeyValuePair<string, int>> getTopDomainAsync(TwitterClient appClient, int streamLimit);
    }
}
