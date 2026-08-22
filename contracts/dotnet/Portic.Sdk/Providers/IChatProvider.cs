using Portic.Sdk.Contracts;

namespace Portic.Sdk.Providers;

/// <summary>
/// The provider SPI (Service Provider Interface) — the permanent AI contract every model call goes
/// through. This is the ports-and-adapters "port": the Portic gateway depends only on this interface,
/// and each concrete provider (OpenAI, Anthropic, Ollama, a local stub, …) is a disposable adapter
/// that implements it.
///
/// Extracted from portic-community's local stub per ADR-0001 (provider-spi-location), so external
/// integrators can implement providers without taking an AGPL runtime dependency. The runtime's own
/// AGENTS.md guardrail still applies wherever this interface is consumed: no AI-provider SDK may be
/// referenced or called anywhere except inside a type that implements it.
/// </summary>
public interface IChatProvider
{
    /// <summary>Stable, lower-case provider name used for routing and audit, e.g. "stub".</summary>
    string Name { get; }

    /// <summary>Serve a normalized request and return a normalized completion.</summary>
    Task<ChatCompletion> CompleteAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
