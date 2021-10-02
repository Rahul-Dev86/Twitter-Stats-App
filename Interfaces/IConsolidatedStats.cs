
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;

namespace TwitterStatsApp.Interfaces
{
    public interface IConsolidatedStats
    {
        Task getConsolidatedStatsAsync(TwitterClient appClient, IList<string> emojis);
    }
}
