using System.Collections.Generic;

namespace TwitterStatsApp.Interfaces
{
    public interface IEmojiPctManager
    {
        string getEmojiProportion(string tweetText, int count, IList<string> emojis, int emojiCnt, out int ecnt);
    }
}
