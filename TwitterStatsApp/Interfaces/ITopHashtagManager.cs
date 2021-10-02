using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using TwitterStatsApp.Models;

namespace TwitterStatsApp.Interfaces
{
   public interface ITopHashtagManager
    {
        Task<List<HashtagVM>> getTopHashtagsAsync(TwitterClient appClient, string endpoint);
    }
}
