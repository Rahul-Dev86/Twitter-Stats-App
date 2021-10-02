using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Tweetinvi;
using TwitterStatsApp.Interfaces;
using TwitterStatsApp.Managers;
using TwitterStatsApp.Sevices;
using Xunit;

namespace TwitterStatsApp.Tests
{
    public class TopEmojiManagerTests
    {
        private readonly TopEmojiManager _sut;
        private readonly TwitterAuthService _ta;
        private readonly Mock<IExceptionLogService> _expLogSrvMock = new Mock<IExceptionLogService>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        public TopEmojiManagerTests()
        {
            _ta = new TwitterAuthService(_configMock.Object, _expLogSrvMock.Object);
            _sut = new TopEmojiManager(_expLogSrvMock.Object);
        }

        [Fact]
        public async Task getTopEmoji_ShouldReturn_EmojiCount()
        {
            //Arrange
            TwitterClient appClient = await _ta.GetAppClientForUnitTests();
            string[] emojis = new string[] { "😀:Happy Face", "😍:Heart Face", "🤓:Nerdy", "💖:Sparkle Heart", "😹:Happy Tears Cat", "💯:Hundred Points Symbol", "👋🏻:Waving Hand", "🚩:Triangular Flag", "🌝:Moonface", "🎉:Celebration", "🎯:Archery", "🏆:Trophy" };
            string endpoint = "https://api.twitter.com/2/tweets/counts/recent?query=";
            //Act
            KeyValuePair<string, Int64> topEmoji = await _sut.getTopEmojiAsync(appClient, endpoint, emojis);
            //Assert 
            Assert.True(topEmoji.Value > 0);            
        }
    }
}
