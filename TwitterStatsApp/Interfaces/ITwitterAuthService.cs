using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using System.Threading.Tasks;

namespace TwitterStatsApp.Sevices
{
    public interface ITwitterAuthService
    {
        TwitterClient appClient { get; set; }
        Task GetAppClient();
    }
}
