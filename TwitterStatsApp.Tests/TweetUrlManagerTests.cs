using System;
using System.Collections.Generic;
using System.Text;
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
    public class TweetUrlManagerTests
    {
        private readonly TweetUrlManager _sut;
        private readonly TwitterAuthService _ta;
        private readonly Mock<IExceptionLogService> _expLogSrvMock = new Mock<IExceptionLogService>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        public TweetUrlManagerTests()
        {
            _ta = new TwitterAuthService(_configMock.Object, _expLogSrvMock.Object);
            _sut = new TweetUrlManager(_expLogSrvMock.Object);
        }

        [Fact]
        public async Task getTweetsUrlPercentAsync_ShouldReturn_Percent()
        {
            //Arrange
            TwitterClient appClient = await _ta.GetAppClientForUnitTests();
            int streamLimit = 500; bool isPhotoUrl = false;
            //Act
            int per = await _sut.getTweetsUrlPercentAsync(appClient, streamLimit, isPhotoUrl);
            //Assert
            Assert.True(per > 0);
        }
    }
}
