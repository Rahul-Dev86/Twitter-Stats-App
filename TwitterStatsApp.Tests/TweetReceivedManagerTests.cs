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
        public void showTweetReceived_ShouldReturn_Count()
        {
            //Arrange
            string tweetId = "1443056477820780547";

            //Act
            string msg = _sut.showTweetReceived(tweetId, 1);

            //Assert 
            Assert.NotEmpty(msg);
        }
    }
}
