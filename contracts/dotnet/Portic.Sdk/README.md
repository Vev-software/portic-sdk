# Portic.Sdk

Public, versioned **provider SPI and normalized chat contracts** for [Portic](https://github.com/Vev-software/portic-community) — VEV's AI gateway. The stable integration surface adapter
authors build against, without taking a dependency on the AGPL-3.0 gateway runtime.

```sh
dotnet add package Portic.Sdk
```

## What's inside

- **`IChatProvider`** — the provider SPI. Every model call the gateway makes goes through this one
  interface; each concrete provider (OpenAI, Anthropic, Ollama, a local stub, …) is a disposable
  adapter that implements it.
- **Normalized contracts** — `ChatRequest`, `ChatMessage`, `ChatCompletion`, `TokenUsage`: the
  provider-neutral shapes the gateway's `POST /v1/messages` accepts and returns. Adapters translate
  these to and from their own provider's wire format.

The types are plain records/an interface with no runtime dependencies beyond the base class library.

## Implementing a provider adapter

```csharp
using Portic.Sdk.Contracts;
using Portic.Sdk.Providers;

public sealed class MyProvider : IChatProvider
{
    public string Name => "my-provider";

    public async Task<ChatCompletion> CompleteAsync(
        ChatRequest request, CancellationToken cancellationToken = default)
    {
        // Map `request` onto your provider's SDK/wire call, then map the response back
        // onto a normalized ChatCompletion. No provider SDK call may happen outside a
        // type like this one that implements IChatProvider (Portic's AGENTS.md guardrail).
        var reply = new ChatMessage { Role = "assistant", Content = "…" };
        return new ChatCompletion
        {
            Id = Guid.NewGuid().ToString("n"),
            Model = request.Model,
            Provider = Name,
            Message = reply,
            Usage = new TokenUsage { InputTokens = 0, OutputTokens = 0 },
        };
    }
}
```

## Versioning

[SemVer](https://semver.org), derived from the git tag (`vX.Y.Z`) at release. Pre-1.0, the surface
evolves additively across minor/patch versions; a breaking change to `IChatProvider` or any of the
normalized contracts requires a major version bump and an ADR (see `docs/adr/` in this repo).

## Links

- **NuGet:** https://www.nuget.org/packages/Portic.Sdk
- **Source & issues:** https://github.com/Vev-software/portic-sdk
- **Gateway runtime (community edition):** https://github.com/Vev-software/portic-community

## Licence

[Apache-2.0](https://github.com/Vev-software/portic-sdk/blob/main/LICENSE).
