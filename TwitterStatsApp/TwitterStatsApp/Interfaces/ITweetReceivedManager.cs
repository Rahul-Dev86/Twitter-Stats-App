using System.Threading.Tasks;
using Tweetinvi;

namespace TwitterStatsApp.Interfaces
{
    public interface ITweetReceivedManager
    {
        Task<int> showTweetReceivedAsync(TwitterClient appClient, int streamLimit);
    }
}
