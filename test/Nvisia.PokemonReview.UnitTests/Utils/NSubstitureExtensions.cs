using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;

namespace Nvisia.PokemonReview.UnitTests.Utils;

public static class NSubstituteExtensions
{
    public static HttpMessageHandler SetupRequest(this HttpMessageHandler handler, HttpMethod method, string requestUri)
    {
        handler
            .GetType()
            .GetMethod("SendAsync", BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(handler, [
                Arg.Is<HttpRequestMessage>(x =>
                    x.Method == method &&
                    x.RequestUri != null &&
                    x.RequestUri.ToString() == requestUri),
                Arg.Any<CancellationToken>()
            ]);

        return handler;
    }

    public static ConfiguredCall ReturnsResponse(this HttpMessageHandler handler, HttpStatusCode statusCode,
        object? responseContent = null)
    {
        return ((object)handler).Returns(
            Task.FromResult(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = JsonContent.Create(responseContent)
            })
        );
    }

    public static void ShouldHaveReceived(this HttpMessageHandler handler, HttpMethod requestMethod, string requestUri,
        int timesCalled = 1)
    {
        if (timesCalled == 0)
        {
            handler.ShouldNotHaveReceived(requestMethod, requestUri);
            return;
        }

        var calls = handler.ReceivedCalls()
            .Where(call => call.GetMethodInfo().Name == "SendAsync")
            .Select(call => call.GetOriginalArguments().First())
            .Cast<HttpRequestMessage>()
            .Where(request =>
                request.Method == requestMethod &&
                request.RequestUri != null &&
                request.RequestUri.ToString() == requestUri
            );

        calls.Should().HaveCount(timesCalled,
            $"HttpMessageHandler was expected to make the following call {timesCalled} times: {requestMethod} {requestUri}");
    }


    public static void ShouldNotHaveReceived(this HttpMessageHandler handler, HttpMethod requestMethod,
        string requestUri)
    {
        var calls = handler.ReceivedCalls()
            .Where(call => call.GetMethodInfo().Name == "SendAsync")
            .Select(call => call.GetOriginalArguments().First())
            .Cast<HttpRequestMessage>()
            .Where(request =>
                request.Method == requestMethod &&
                request.RequestUri != null &&
                request.RequestUri.ToString() == requestUri
            );

        calls.Should().HaveCount(0,
            $"HttpMessageHandler was expected to not the following call {0} times: {requestMethod} {requestUri}");
    }
}