using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterStatsApp.Models
{    
    public class HashtagVM
    {
        public string name { get; set; }
        public string url { get; set; }
        public string promoted_content { get; set; }
        public string query { get; set; }
        public string tweet_volume { get; set; }
    }
}
