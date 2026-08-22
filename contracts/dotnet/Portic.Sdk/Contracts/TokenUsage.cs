namespace Portic.Sdk.Contracts;

/// <summary>
/// Normalized token accounting. Adapters populate this from provider usage metadata (or estimate it
/// for local stubs). Used for cost/telemetry — it never carries prompt or completion content.
/// </summary>
public sealed record TokenUsage
{
    /// <summary>Tokens consumed by the request (prompt + history).</summary>
    public required int InputTokens { get; init; }

    /// <summary>Tokens produced by the completion.</summary>
    public required int OutputTokens { get; init; }

    /// <summary>Sum of <see cref="InputTokens"/> and <see cref="OutputTokens"/>.</summary>
    public int TotalTokens => InputTokens + OutputTokens;
}
