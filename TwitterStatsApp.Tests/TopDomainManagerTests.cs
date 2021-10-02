using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json.Linq;
using Tweetinvi;
using Tweetinvi.Models.V2;
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
        public void getTopDomain_ShouldReturn_TopDomain()
        {
            //Arrange
            string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\MockObj\DummyStream.json";
            JObject o1 = JObject.Parse(File.ReadAllText(filepath));
            var tweetarray = o1["data"]["context_annotations"].Value<JArray>();
            TweetContextAnnotationV2[] annotations = tweetarray.ToObject<TweetContextAnnotationV2[]>();
            ConcurrentDictionary<string, int> dictDomain = new ConcurrentDictionary<string, int>();

            //Act
            string topDomain = _sut.getTopDomain(annotations, 1, dictDomain);

            //Assert 
            Assert.True(dictDomain.Count > 0);
            Assert.NotEmpty(topDomain);
        }
    }
}
