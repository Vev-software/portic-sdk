using System.Net;
using System.Text;
using Portic.Sdk.Contracts;
using Xunit;

namespace Portic.Client.Tests;

public sealed class PorticClientTests
{
    [Fact]
    public async Task SendAsync_posts_request_to_v1_messages()
    {
        var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = Json("""{"id":"cmp_123","model":"stub-echo","provider":"stub","message":{"role":"assistant","content":"echo: ping"},"usage":{"inputTokens":1,"outputTokens":2,"totalTokens":3}}"""),
        });

        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://portic.example/")
        };

        var client = new PorticClient(httpClient);

        await client.SendAsync(new ChatRequest
        {
            Model = "stub-echo",
            Provider = "stub",
            MaxTokens = 64,
            Messages = [new ChatMessage { Role = "user", Content = "ping" }],
        });

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Equal("https://portic.example/v1/messages", handler.LastRequest.RequestUri!.ToString());

        var body = await handler.LastRequest.Content!.ReadAsStringAsync();
        Assert.Contains("\"model\":\"stub-echo\"", body, StringComparison.Ordinal);
        Assert.Contains("\"provider\":\"stub\"", body, StringComparison.Ordinal);
        Assert.Contains("\"maxTokens\":64", body, StringComparison.Ordinal);
        Assert.Contains("\"messages\":[{\"role\":\"user\",\"content\":\"ping\"}]", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SendAsync_returns_normalized_completion()
    {
        var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = Json("""{"id":"cmp_123","model":"gpt-4o-mini","provider":"openai","message":{"role":"assistant","content":"ready"},"usage":{"inputTokens":10,"outputTokens":4,"totalTokens":14}}"""),
        });

        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://portic.example/")
        };

        var client = new PorticClient(httpClient);
        var completion = await client.SendAsync(new ChatRequest
        {
            Model = "gpt-4o-mini",
            Messages = [new ChatMessage { Role = "user", Content = "status?" }],
        });

        Assert.Equal("cmp_123", completion.Id);
        Assert.Equal("openai", completion.Provider);
        Assert.Equal("ready", completion.Message.Content);
        Assert.Equal(14, completion.Usage.TotalTokens);
    }

    [Fact]
    public async Task SendAsync_throws_PorticClientException_for_problem_details()
    {
        var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = Json("""{"title":"messages_required","detail":"At least one message is required.","status":400}"""),
        });

        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://portic.example/")
        };

        var client = new PorticClient(httpClient);

        var exception = await Assert.ThrowsAsync<PorticClientException>(() => client.SendAsync(new ChatRequest
        {
            Model = "stub-echo",
            Messages = [],
        }));

        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal("messages_required", exception.ReasonCode);
        Assert.Equal("At least one message is required.", exception.Detail);
    }

    [Fact]
    public async Task SendAsync_requires_base_address()
    {
        var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = Json("""{"id":"cmp_123","model":"stub-echo","provider":"stub","message":{"role":"assistant","content":"ok"},"usage":{"inputTokens":1,"outputTokens":1,"totalTokens":2}}"""),
        });

        using var httpClient = new HttpClient(handler);
        var client = new PorticClient(httpClient);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => client.SendAsync(new ChatRequest
        {
            Model = "stub-echo",
            Messages = [new ChatMessage { Role = "user", Content = "ping" }],
        }));

        Assert.Equal("PorticClient requires HttpClient.BaseAddress to be set.", exception.Message);
    }

    private static StringContent Json(string json) => new(json, Encoding.UTF8, "application/json");

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder = responder;

        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(_responder(request));
        }
    }
}
