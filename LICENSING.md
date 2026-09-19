# Licensing

This repository — the Portic **provider SPI, client SDK and normalized contracts** — is
licensed under the **Apache License 2.0** (see [`LICENSE`](./LICENSE)). It is the permissive
**integration surface** of Portic: you can depend on `Portic.Sdk` and `Portic.Client` from
proprietary or open-source code without taking on any copyleft obligation.

This is deliberate and load-bearing for the whole product family. Portic is open-core, and
the three repositories carry different licenses on purpose:

| Component | Repository | License |
| --- | --- | --- |
| Provider SPI, client SDK, normalized contracts | **`portic-sdk`** (public) | **Apache-2.0** |
| Community runtime (`Portic.Core`, gateway host, stub adapter) | `portic-community` (public) | AGPL-3.0 **or** commercial |
| Enterprise modules, hosted management, Portic Cloud | `portic-enterprise` (private) | Proprietary (commercial) |

- **Integrate against this SDK freely.** Building a client or a provider adapter against
  `Portic.Sdk` never makes your code AGPL — that copyleft lives only in the runtime, and only
  the runtime is dual-licensed.
- **Dependency direction is one-way.** The runtime and the enterprise modules depend on this
  SDK; this SDK depends only on public standards and never on the runtime or on private
  modules ([ADR-0001](./docs/adr/0001-repository-visibility-and-license.md)).
- **Do not move runtime code here.** Keeping the SPI/contracts permissive only works if the
  AGPL runtime stays out of this repository.

For the runtime's dual-license terms (when you self-host or embed the gateway itself), see
`portic-community`'s [`LICENSING.md`](https://github.com/Vev-software/portic-community/blob/main/LICENSING.md).

## Contributions

Contributions to this repository are accepted under the Apache-2.0 inbound=outbound rule
(Apache-2.0 §5): by submitting a contribution you license it under Apache-2.0. Sign off your
commits with `git commit -s` (Developer Certificate of Origin). See
[`CONTRIBUTING.md`](./CONTRIBUTING.md).

> This document explains the licensing model; it is not itself a license or legal advice. The
> binding terms are in [`LICENSE`](./LICENSE).
