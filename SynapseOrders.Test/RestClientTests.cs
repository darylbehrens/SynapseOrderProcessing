using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynapseOrders;
using SynapseOrders.Services;
using System.Net;

namespace SynapseOrders.Test
{
    // I would Moq out the HttpClient here and test the RestClient
    // which I was honestly struggling with and ran out of time
    // Here is the article I was working with : https://www.code4it.dev/blog/testing-httpclientfactory-moq/
    public class RestClientTests
    {
    }

    //public class HttpMessageHandlerStub : HttpMessageHandler
    //{
    //    private Func<object, Task<HttpResponseMessage>> value;

    //    public HttpMessageHandlerStub(Func<object, Task<HttpResponseMessage>> value)
    //    {
    //        this.value = value;
    //    }

    //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

    //        HttpResponseMessage result = new HttpResponseMessage();

    //        handlerMock
    //            .Protected()
    //            .Setup<Task<HttpResponseMessage>>(
    //                "SendAsync",
    //                ItExpr.IsAny<HttpRequestMessage>(),
    //                ItExpr.IsAny<CancellationToken>()
    //            )
    //            .ReturnsAsync(result)
    //            .Verifiable();

    //        var httpClient = new HttpClient(handlerMock.Object)
    //        {
    //            BaseAddress = new Uri("https://www.code4it.dev/")
    //        };

    //        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

    //        mockHttpClientFactory.Setup(_ => _.CreateClient("ext_service")).Returns(httpClient);

    //        var services = new RestClient(null, mockHttpClientFactory.Object);
    //    }
    //}
}
