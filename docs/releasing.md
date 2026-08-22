# Releasing & publishing

How a version of the Portic SDK is cut and published. Two workflows do the work, and
they are deliberately separate so building a release and pushing it to the public
registry remain distinct, gated steps.

| Workflow | Trigger | What it does |
|---|---|---|
| [`release.yml`](../.github/workflows/release.yml) | push a `v*.*.*` tag | Builds + tests from public feeds, packs both .NET SDKs, attaches a **Sigstore build-provenance** attestation, and creates a **GitHub Release** with the versioned artifacts. Does not touch nuget.org. |
| [`publish.yml`](../.github/workflows/publish.yml) | manual (`workflow_dispatch`) | Validates `main`, computes the next version tag, waits for approval on the `release` environment, pushes the tag, then publishes `Portic.Sdk` and `Portic.Client` to **nuget.org**. |

## What publishes where

- **NuGet**: `Portic.Sdk` and `Portic.Client` publish to **nuget.org**.
- **GitHub Releases**: every pushed `vX.Y.Z` tag produces a signed release with the built
  `.nupkg` artifacts and SBOM.
- **npm**: nothing today. This repo currently has no TypeScript SDK, so there is no npm
  publishing step.

## Versioning — the tag is the single source of truth

There is **no hand-maintained version number** in this repo. The git tag drives the
package version:

- **.NET** — [MinVer](https://github.com/adamralph/minver) derives the package version
  from the tag at build time (`MinVerTagPrefix` is `v`, so tag `vX.Y.Z` → package
  `X.Y.Z`; commits after a tag produce a pre-release).

So bumping a release is a **single action: run the gated `publish.yml` workflow**. It
computes the next tag, validates CI, and pushes the tag only after approval.

> MinVer needs the full git history + tags, so every job that builds/packs checks out
> with `fetch-depth: 0`.

## Cutting a release

1. Land the change on `main` and wait for green `ci`.
2. Run **Actions → release (`publish.yml`) → Run workflow**, choose the bump
   (`patch`/`minor`/`major`), and optionally use `dry_run` to rehearse.
3. Approve the `release` environment when the workflow pauses.
4. The workflow pushes `vX.Y.Z`, which fires `release.yml` to create the GitHub Release.
5. The same approved `publish.yml` run publishes both NuGet packages to nuget.org.

## Publishing without API keys — Trusted Publishing

Publishing uses **OIDC Trusted Publishing**, not stored API keys. nuget.org exchanges
the GitHub OIDC token for a short-lived credential via `NuGet/login@v1`.

The one-time trusted-publisher setup lives with the maintainers (it is registry account
configuration, not repo configuration). For this repo, the setup must cover:

- repository: `Vev-software/portic-sdk`
- workflow file: `publish.yml`
- environment: `release`
- repo variable: `NUGET_USER` set to the nuget.org username that created the trusted
  publishing policy

Because Trusted Publishing attaches to an **existing** nuget.org package, the very first
publish of each brand-new package may still need the one-time maintainer bootstrap noted
in the internal runbook.

## Current status

Live as of August 22, 2026:

- `Portic.Sdk` `0.1.0` on **nuget.org**
- `Portic.Client` `0.1.0` on **nuget.org**
- `v0.1.0` GitHub Release and tag-driven release path working from `publish.yml`

The one-time bootstrap and repo configuration are now in place. Future releases should use the same
gated `publish.yml` flow from `main`.
