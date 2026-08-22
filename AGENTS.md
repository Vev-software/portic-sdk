# AGENTS.md — repo-local guardrails for portic-sdk

This repository is public. Read `Vev-software/engineering/AGENTS.md` for the full
policy; this file narrows it to what matters most here and adds the release ritual.
Where this file and the handbook differ, the handbook wins.

## What this repo is

`portic-sdk` is the public SDK layer for Portic: the provider SPI and normalized chat
contracts in `contracts/dotnet/Portic.Sdk/`, plus the first HTTP client in
`clients/dotnet/Portic.Client/`. It is Apache-2.0 and intentionally contains only the
public integration ergonomics — never the AGPL runtime implementation.

## Releasing this package — the git tag is the single source of truth (18 §1.1)

There is **no version number to edit** in this repo:

- The .NET package versions come from **MinVer**. A tag `vX.Y.Z` produces package
  version `X.Y.Z`; commits after a tag produce a pre-release.
- To cut a release, use the **one-button flow** — no local git/tag commands:
  Actions → **release** (`publish.yml`) → **Run workflow** → pick the bump
  (patch/minor/major) → Run. It validates that you are on `main` and that the latest
  `ci` run for `main`'s HEAD is green, then pauses on the `release` environment for a
  single **Approve** click, and on approval pushes the tag `vX.Y.Z` (which fires the
  `github release` workflow, `release.yml`) and publishes both NuGet packages to
  nuget.org via OIDC. Use the `dry_run` checkbox to rehearse without tagging or
  publishing.
- **Never** add `<VersionPrefix>`/`<Version>` to the project files.
- Keep `publish.yml` named exactly that — NuGet Trusted Publishing pins
  `workflow=publish.yml`.

## Public disclosure rules

- Public PR titles/bodies, issue bodies, README/docs, ADRs and `.github` templates
  describe only this repo's code/behaviour and its published public SDKs.
- Do **not** include: private repo/module names, proprietary deployment topology or
  control paths, licence-enforcement/entitlement detail, internal hostnames, customer
  names, security-control specifics, or secrets/credentials.
- Security vulnerabilities do not belong in a public issue/PR — follow `SECURITY.md`.

## Boundaries

- Public, Apache-2.0. Consumers compile against these packages without depending on the
  Portic runtime or a private feed.
- Breaking a public SDK contract is expensive: it needs an ADR, a migration path, a
  deprecation period and compatibility tests.
- Material or cross-cutting changes start as an issue or ADR, not a surprise PR.
