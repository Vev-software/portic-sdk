# Portic SDK

Client SDKs and the provider SPI for **Portic** — the stable, versioned
integration surface that applications, adapters and ecosystem tooling build
against without depending on the Portic runtime internals.

## Status

- [`Portic.Sdk`](contracts/dotnet/Portic.Sdk/README.md): provider SPI + normalized chat contracts,
  extracted from `portic-community` per its ADR-0001.
- [`Portic.Client`](clients/dotnet/Portic.Client/README.md): a thin .NET `HttpClient` wrapper for the
  stable `POST /v1/messages` gateway surface.

The repo now includes the release/publish workflow pattern used by the org's other package repos:
tagged releases produce GitHub Release artifacts, and the gated `publish.yml` workflow publishes the
two NuGet packages to nuget.org. Public registry publishing still depends on the one-time Trusted
Publishing bootstrap used by the org's other public packages. Until a published `Portic.Sdk`
exists, `portic-community` continues to define the same shapes locally.

## What it is

- The public integration surface for Portic: client SDKs plus the provider
  extension points (SPI) used by adapters and third-party tooling.
- A stable contract you can build against — changes are versioned and
  compatibility-aware.

## What it is not

- The Portic gateway runtime.
- The hosted control plane.
- Enterprise-only or commercial integrations.

## Getting started

Not published yet — see Status above. Once published:

```sh
dotnet add package Portic.Sdk
dotnet add package Portic.Client
```

See [`contracts/dotnet/Portic.Sdk/README.md`](contracts/dotnet/Portic.Sdk/README.md)
for how to implement a provider adapter against `IChatProvider`, and
[`clients/dotnet/Portic.Client/README.md`](clients/dotnet/Portic.Client/README.md) for calling a
running Portic gateway over HTTP.

## Contributing

- One logical change per pull request.
- Public API changes stay versioned and reviewable.
- Breaking changes to a published contract require an ADR and a migration path.

Repository decision records live under [`docs/adr/`](docs/adr/).
Release/publish mechanics are documented in [`docs/releasing.md`](docs/releasing.md).

## Security

Please report vulnerabilities privately — see [SECURITY.md](SECURITY.md). Do not
open a public issue for a security report.

## License

Licensed under the [Apache License 2.0](LICENSE).
