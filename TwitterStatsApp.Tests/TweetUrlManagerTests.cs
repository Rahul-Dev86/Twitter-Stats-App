using System;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json.Linq;
using Tweetinvi.Models.V2;
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
        public void getTweetsUrlPercent_ShouldReturn_Message()
        {
            //Arrange
            string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\MockObj\DummyStream.json";
            JObject o1 = JObject.Parse(File.ReadAllText(filepath));
            var tweetarray = o1["data"]["entities"]["urls"].Value<JArray>();
            UrlV2[] urls = tweetarray.ToObject<UrlV2[]>();
            ConcurrentBag<string> colUrl = new ConcurrentBag<string>();
            ConcurrentBag<string> photoUrl = new ConcurrentBag<string>();

            //Act
            string[] msg = _sut.getTweetsUrlPercent(urls, 1, colUrl, photoUrl);

            //Assert
            Assert.NotNull(urls);
            Assert.True(colUrl.Count > 0);
            Assert.True(photoUrl.Count > 0);
            Assert.NotEmpty(msg);
        }
    }
}
