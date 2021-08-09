using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace TwitterStatsApp.Interfaces
{
    public interface ITweetUrlManager
    {
        Task<int> getTweetsUrlPercentAsync(TwitterClient appClient, int StreamLimit, bool isPhotoUrl = false);
    }
}
