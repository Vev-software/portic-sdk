# Contributing to the Portic SDK

Please read [`AGENTS.md`](./AGENTS.md) first for the repo-local release, disclosure and
boundary guardrails.

## Licensing of contributions

This repository is **Apache-2.0** (see [`LICENSING.md`](./LICENSING.md)). Under Apache-2.0
§5, contributions are inbound=outbound: by submitting a contribution you license it under
Apache-2.0. No separate CLA is required here.

- Sign off every commit with `git commit -s` (Developer Certificate of Origin,
  https://developercertificate.org/).

> Note: the **runtime** in `portic-community` is dual-licensed and *does* require a CLA. This
> SDK does not — Apache-2.0 already lets VEV redistribute and sublicense contributions, so a
> CLA would add nothing here.

## Scope

Keep changes inside this repo's boundary: **public client SDKs and the provider SPI /
contracts only** — never the Portic runtime, never a dependency on private modules or feeds
([ADR-0001](./docs/adr/0001-repository-visibility-and-license.md)).

## Before you open a PR

- Keep PRs small and single-purpose; use Conventional Commits.
- Build and test with the commands documented in [`AGENTS.md`](./AGENTS.md) and
  [`docs/releasing.md`](./docs/releasing.md); do not weaken a fitness/architecture test to
  make a change pass.

## Security

Report vulnerabilities privately — see [`SECURITY.md`](./SECURITY.md). Do not open a public
issue for a security report.
