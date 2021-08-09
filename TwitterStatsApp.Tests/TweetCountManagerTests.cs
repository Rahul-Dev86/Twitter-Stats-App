using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Tweetinvi;
using TwitterStatsApp.Interfaces;
using TwitterStatsApp.Managers;
using TwitterStatsApp.Models;
using TwitterStatsApp.Sevices;
using Xunit;

namespace TwitterStatsApp.Tests
{
    public class TweetCountManagerTests
    {
        private readonly TweetCountManager _sut;
        private readonly TwitterAuthService _ta;
        private readonly Mock<IExceptionLogService> _expLogSrvMock = new Mock<IExceptionLogService>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        public TweetCountManagerTests()
        {
            _sut = new TweetCountManager(_expLogSrvMock.Object);
            _ta = new TwitterAuthService(_configMock.Object, _expLogSrvMock.Object);
        }

        [Fact]
        public async Task getTweetCount_ShouldReturn_TweetCount()
        {
            //Arrange
            TwitterClient appClient = await _ta.GetAppClientForUnitTests();
            string query = "covid&granularity=day&start_time=" + Uri.EscapeDataString(DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"));
            string endpoint = "https://api.twitter.com/2/tweets/counts/recent?query="+query;
            //Act
            List<TweetCountVM> objTweetCount = await _sut.getTweetCountAsync(appClient, endpoint);
            //Assert
            Assert.NotNull(objTweetCount);
        }
    }
}
