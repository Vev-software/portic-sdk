# Portic.Client

Thin .NET client for [Portic](https://github.com/Vev-software/portic-community)'s stable
`POST /v1/messages` HTTP surface. It builds on [`Portic.Sdk`](../../../contracts/dotnet/Portic.Sdk/README.md)'s normalized
`ChatRequest` / `ChatCompletion` contracts, so callers get one provider-neutral shape whether the
gateway routes to OpenAI, Anthropic, Ollama or a local adapter.

```sh
dotnet add package Portic.Client
```

## What it does

- Wraps `HttpClient` for `POST /v1/messages`.
- Serializes/deserializes the normalized contracts with web-default JSON (`camelCase`).
- Surfaces non-success responses as `PorticClientException`, including Portic's `ProblemDetails`
  reason code when available.

## Basic usage

```csharp
using Portic.Client;
using Portic.Sdk.Contracts;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://portic.example/")
};

var portic = new PorticClient(httpClient);

var completion = await portic.SendAsync(new ChatRequest
{
    Model = "gpt-4o-mini",
    Messages =
    [
        new ChatMessage { Role = "system", Content = "Answer tersely." },
        new ChatMessage { Role = "user", Content = "Write a haiku about gateways." },
    ],
});

Console.WriteLine(completion.Message.Content);
```

## OpenAI-style migration shape

If you already assemble a messages array for an OpenAI-compatible call, the Portic adoption path is
deliberately small: keep the conversation payload, point `HttpClient.BaseAddress` at Portic, and send
the normalized `ChatRequest` instead of a provider-specific request type.

```csharp
var completion = await portic.SendAsync(new ChatRequest
{
    Model = "gpt-4o-mini",
    Provider = "openai",
    Messages =
    [
        new ChatMessage { Role = "user", Content = "Summarize this repository strategy." },
    ],
});
```

## Versioning and compatibility

`Portic.Client` tracks the gateway's stable `/v1/messages` surface. Within `v1`, compatibility is
additive: clients must ignore unknown response fields, and new optional request fields may appear.
Breaking transport changes require a new path version in the gateway and a corresponding major version
bump here.

## Links

- **NuGet:** https://www.nuget.org/packages/Portic.Client
- Source and issues: https://github.com/Vev-software/portic-sdk
- Gateway runtime: https://github.com/Vev-software/portic-community

## Licence

[Apache-2.0](https://github.com/Vev-software/portic-sdk/blob/main/LICENSE)
