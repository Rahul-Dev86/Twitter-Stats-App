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
    public class TopDomainManagerTests
    {
        private readonly TopDomainManager _sut;
        private readonly TwitterAuthService _ta;
        private readonly Mock<IExceptionLogService> _expLogSrvMock = new Mock<IExceptionLogService>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        public TopDomainManagerTests()
        {
            _ta = new TwitterAuthService(_configMock.Object, _expLogSrvMock.Object);
            _sut = new TopDomainManager(_expLogSrvMock.Object);
        }

        [Fact]
        public async Task getTopDomain_ShouldReturn_TopDomain()
        {
            //Arrange
            TwitterClient appClient = await _ta.GetAppClientForUnitTests();
            int streamLimit = 500;
            //Act
            KeyValuePair<string, int> topDomain = await _sut.getTopDomainAsync(appClient, streamLimit);
            //Assert 
            Assert.NotEmpty(topDomain.Key);
        }
    }
}
