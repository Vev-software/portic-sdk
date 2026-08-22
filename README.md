# Portic SDK

Client SDKs and the provider SPI for **Portic** — the stable, versioned
integration surface that applications, adapters and ecosystem tooling build
against without depending on the Portic runtime internals.

## Status

The provider SPI and normalized chat contracts are extracted from `portic-community`
(per its `docs/adr/0001-provider-spi-location.md`) and build/pack as a repo-local
package baseline — see [`contracts/dotnet/Portic.Sdk`](contracts/dotnet/Portic.Sdk/README.md).
Not yet published to nuget.org (that needs the one-time Trusted Publishing bootstrap
used by the org's other public packages); the `portic-community` runtime still
defines these types locally until a published version exists to switch to. A first
client SDK package is tracked separately (`portic-sdk#3`).

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
```

See [`contracts/dotnet/Portic.Sdk/README.md`](contracts/dotnet/Portic.Sdk/README.md)
for how to implement a provider adapter against `IChatProvider`.

## Contributing

- One logical change per pull request.
- Public API changes stay versioned and reviewable.
- Breaking changes to a published contract require an ADR and a migration path.

Repository decision records live under [`docs/adr/`](docs/adr/).

## Security

Please report vulnerabilities privately — see [SECURITY.md](SECURITY.md). Do not
open a public issue for a security report.

## License

Licensed under the [Apache License 2.0](LICENSE).
