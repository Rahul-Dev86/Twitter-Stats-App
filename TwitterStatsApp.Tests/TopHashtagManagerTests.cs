using System.Collections.Generic;
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
    public class TopHashtagManagerTests
    {
        private readonly TopHashtagManager _sut;
        private readonly TwitterAuthService _ta;
        private readonly Mock<IExceptionLogService> _expLogSrvMock = new Mock<IExceptionLogService>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        public TopHashtagManagerTests()
        {
            _ta = new TwitterAuthService(_configMock.Object, _expLogSrvMock.Object);
            _sut = new TopHashtagManager(_expLogSrvMock.Object);
        }

        [Fact]
        public async Task getTopHashtags_ShouldReturn_TopHashtags()
        {
            //Arrange
            TwitterClient appClient = await _ta.GetAppClientForUnitTests();
            string endpoint = "https://api.twitter.com/1.1/trends/place.json?id=23424977";
            //Act
            List<HashtagVM> topHashtags = await _sut.getTopHashtagsAsync(appClient, endpoint);
            //Assert
            Assert.True(topHashtags.Count > 0);
        }
    }
}
