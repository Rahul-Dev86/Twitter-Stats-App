using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json.Linq;
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
        public void getEmojiProportion_ShouldReturn_Percent()
        {
            //Arrange
            string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\MockObj\DummyStream.json";
            JObject o1 = JObject.Parse(File.ReadAllText(filepath));
            var tweetarray = o1["data"]["entities"]["urls"].Value<JArray>();
            string[] emojis = new string[] { "😀", "😍", "🤓", "💖", "😹", "💯", "👋🏻", "🚩", "🌝", "🎉", "🎯", "🏆" };
            string strDummyTweetText = "@Pedcanuto a janela do meu quarto está até fechada no momento, se abrir eu pulo😍😍😍😍";
            
            //Act
            string msg = _sut.getEmojiProportion(strDummyTweetText, 1, emojis, 0, out int ecnt);

            //Assert
            Assert.True(ecnt > 0);
            Assert.NotEmpty(msg);
        }
    }
}
