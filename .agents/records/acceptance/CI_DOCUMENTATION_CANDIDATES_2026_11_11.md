# Immutable CI documentation candidates — 2026-11-11

Engine CI selects one immutable Docs revision before starting its platform matrix. PR coordination metadata carries the Docs commit and engine source-tree identity without adding circular tracked-file commit dependencies.

## Implemented paths

- PR validation reads current metadata for the same base, head and merge candidate. Correcting metadata can therefore take effect on a rerun without an empty source commit. A changed candidate is rejected.
- Push, schedule and manual fallback lookup paginate the PRs associated with the exact engine commit.
- Merge-group lookup reads active queue entries, follows pagination and selects prepared queue commits between the base and the checked candidate. It does not extract PR numbers from generated branch names. The GitHub schema exposes [queue entries and their prepared commits](https://docs.github.com/en/graphql/reference/pulls) and [branch-specific queue lookup](https://docs.github.com/en/graphql/reference/repos).
- Manual dispatch can explicitly select a complete Docs commit. Branch names, tags, missing metadata, conflicting revisions and malformed matching metadata are rejected.
- Both platform jobs check out the same public Docs revision under an ignored output directory. They pass the exact revision and checkout to the protected snapshot preparation added in the previous stage. No Docs script executes during this selection.
- Candidate selection and its first failure are retained for 30 days. Input pairing is separate from API verification, publication provenance and review.

## Local evidence and first failures

`ci-documentation-candidate-selection-current-pr.log` records nineteen passing protocol cases using isolated Git inputs and simulated GitHub responses. They cover actual Git ancestry and both REST/queue pagination contracts, but do not certify live GitHub queue execution or authentication. YAML parsing with duplicate-key rejection confirms the stable `Engine validation` name and PR/merge-group triggers remain present.

The initial fixture accidentally named its helper `Git`, shadowing the native command in case-insensitive PowerShell and recursing before repository initialization. Only the identified fixture process was stopped after its command line and activity were inspected. The helper was renamed. A subsequent fixture attempt exposed mock state crossing script scope; the transport now captures its own closure. Both unsuccessful attempts remain alongside the passing nineteen-case result. No production assertion or selection requirement was removed.

No engine compilation or Native run was repeated for these shell/workflow changes. The prior compiled implementation and source comparisons remain separately recorded in [external validation acceptance](EXTERNAL_DOCUMENTATION_VALIDATION_2026_11_11.md).

## Not yet certified

This stage does not establish a trusted Docs source certificate, App installation, actual hosted success, site promotion or draft release. Current binding requires the same engine source tree; trusted compatibility carry-forward for unchanged documentation across other engine trees remains unfinished. Initial Native source checkout still precedes detailed platform planning; preparation classification must be improved separately without reusing a plan whose inputs changed after checkout.

The three outstanding platform source captures, old standalone Wiki callers and legacy tree removal remain open. The candidate-selection stage is not an overall completion certificate or permission to merge.
