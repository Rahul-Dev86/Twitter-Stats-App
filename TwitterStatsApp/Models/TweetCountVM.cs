using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterStatsApp.Models
{
    public class TweetCountVM
    {
        public DateTime end { get; set; }
        public DateTime start { get; set; }
        public int tweet_count { get; set; }
    }
}
