using System;
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
    public class EmojiPctManagerTests
    {
        private readonly EmojiPctManager _sut;
        private readonly TwitterAuthService _ta;
        private readonly Mock<IExceptionLogService> _expLogSrvMock = new Mock<IExceptionLogService>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        public EmojiPctManagerTests()
        {
            _ta = new TwitterAuthService(_configMock.Object, _expLogSrvMock.Object);
            _sut = new EmojiPctManager(_expLogSrvMock.Object);
        }

        [Fact]
        public async Task getEmojiProportion_ShouldReturn_Percent()
        {
            //Arrange
            TwitterClient appClient = await _ta.GetAppClientForUnitTests();
            int streamLimit = 500;
            string[] emojis = new string[] { "😀", "😍", "🤓", "💖", "😹", "💯", "👋🏻", "🚩", "🌝", "🎉", "🎯", "🏆" };
            //Act
            int per = await _sut.getEmojiProportionAsync(appClient, emojis, streamLimit);
            //Assert
            Assert.True(per > 0);
        }
    }
}
