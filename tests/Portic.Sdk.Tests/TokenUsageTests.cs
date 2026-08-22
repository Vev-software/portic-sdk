using Portic.Sdk.Contracts;
using Xunit;

namespace Portic.Sdk.Tests;

public sealed class TokenUsageTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(3, 5, 8)]
    [InlineData(100, 0, 100)]
    public void TotalTokens_sums_input_and_output(int input, int output, int expectedTotal)
    {
        var usage = new TokenUsage { InputTokens = input, OutputTokens = output };

        Assert.Equal(expectedTotal, usage.TotalTokens);
    }
}
