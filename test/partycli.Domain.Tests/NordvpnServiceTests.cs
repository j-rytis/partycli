using System.Net.Mime;
using System.Text;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;

namespace partycli.Domain.Tests
{
    public class Tests
    {
        //private NordvpnService _nordvpnService;

        //private Mock<HttpMessageHandler> _httpMessageHandler;

        //[SetUp]
        //public void Setup()
        //{
        //    _httpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        //    var httpClient = new HttpClient(_httpMessageHandler.Object);

        //    _nordvpnService = new NordvpnService();
        //}

        //[Test]
        //public void GetAllServersList_ReturnsCorrectList()
        //{
        //    var uri = "http://localhost";

        //    _httpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //        "SendAync",
        //        It.IsAny<HttpResponseMessage>(),
        //        It.IsAny<CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage()
        //        {
        //            StatusCode = System.Net.HttpStatusCode.OK,
        //            Content = new StringContent("",
        //            UTF8Encoding.UTF8,
        //            MediaTypeNames.Application.Json)
        //        }).Verifiable();
        //}
    }
}