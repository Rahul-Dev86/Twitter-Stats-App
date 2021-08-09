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
    public class TweetReceivedManagerTests
    {
        private readonly TweetReceivedManager _sut;
        private readonly TwitterAuthService _ta;
        private readonly Mock<IExceptionLogService> _expLogSrvMock = new Mock<IExceptionLogService>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        public TweetReceivedManagerTests()
        {
            _ta = new TwitterAuthService(_configMock.Object, _expLogSrvMock.Object);
            _sut = new TweetReceivedManager(_expLogSrvMock.Object);
        }

        [Fact]
        public async Task showTweetReceived_ShouldReturn_Count()
        {
            //Arrange
            TwitterClient appClient = await _ta.GetAppClientForUnitTests();
            int streamLimit = 500;
            //Act
            int tweetCount = await _sut.showTweetReceivedAsync(appClient, streamLimit);
            //Assert 
            Assert.True(tweetCount > 0);
        }
    }
}
