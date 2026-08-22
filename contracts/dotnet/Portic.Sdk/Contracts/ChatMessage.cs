namespace Portic.Sdk.Contracts;

/// <summary>
/// A single normalized message in a conversation. Provider-neutral: adapters translate this to and
/// from their wire format. Roles are lower-case strings ("system", "user", "assistant") so the
/// contract does not bake in any one provider's enum.
/// </summary>
public sealed record ChatMessage
{
    /// <summary>Lower-case role: "system", "user" or "assistant".</summary>
    public required string Role { get; init; }

    /// <summary>The message text.</summary>
    public required string Content { get; init; }
}
