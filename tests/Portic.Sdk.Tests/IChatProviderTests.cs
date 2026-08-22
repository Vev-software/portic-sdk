using Portic.Sdk.Contracts;
using Portic.Sdk.Providers;
using Xunit;

namespace Portic.Sdk.Tests;

/// <summary>
/// Proves the SPI is implementable exactly as documented in the package README, using only this
/// package -- no dependency on the portic-community runtime. That is the compatibility bar for
/// external adapter authors.
/// </summary>
public sealed class IChatProviderTests
{
    private sealed class EchoProvider : IChatProvider
    {
        public string Name => "echo";

        public Task<ChatCompletion> CompleteAsync(ChatRequest request, CancellationToken cancellationToken = default)
        {
            var last = request.Messages[^1];
            return Task.FromResult(new ChatCompletion
            {
                Id = "test-completion",
                Model = request.Model,
                Provider = Name,
                Message = new ChatMessage { Role = "assistant", Content = $"echo: {last.Content}" },
                Usage = new TokenUsage { InputTokens = last.Content.Length, OutputTokens = last.Content.Length },
            });
        }
    }

    [Fact]
    public async Task Custom_provider_round_trips_a_request()
    {
        IChatProvider provider = new EchoProvider();
        var request = new ChatRequest
        {
            Model = "test-model",
            Messages = [new ChatMessage { Role = "user", Content = "hello" }],
        };

        var completion = await provider.CompleteAsync(request);

        Assert.Equal("echo", completion.Provider);
        Assert.Equal("test-model", completion.Model);
        Assert.Equal("echo: hello", completion.Message.Content);
        Assert.Equal(completion.Usage.InputTokens + completion.Usage.OutputTokens, completion.Usage.TotalTokens);
    }
}
