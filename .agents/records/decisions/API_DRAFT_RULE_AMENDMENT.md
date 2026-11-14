# API draft/main rule amendment

The user-approved API automatic synchronization plan supersedes the previous paired-PR documentation delivery policy. This is an explicit policy amendment, not a claim that a translation preserves a changed obligation verbatim.

## Changed obligations

- API scope excludes all backends, extern, non-C# projects, tests and fixtures. Useful integration guidance, legal provenance and actual engine validation remain required. This supersedes the narrower backend-native exception and its constant-catalog exclusions.
- Engine API changes maintain source XML. Complete actual declarations and script projections synchronize from master into Docs draft. Manual changes are maintained independently on draft; an engine PR no longer requires a paired Docs candidate.
- Each type/member has independent XML/manual prose ownership. Missing manual explanation and changed manual contracts receive editorial warnings. This does not weaken declaration/XML validity, ownership contracts, required engine tests, executable example checks or website integrity.
- Only a human draft-to-main release merge authorizes automatic site deployment. No automated merge, force push, draft deletion or formal engine release is authorized.

## Preserved obligations

Original rule IDs, immutable source locations, source hashes and clause order remain in coverage.json. Exact signatures, external protected extension points, overloads, nullability, defaults, generic constraints, inherited contracts, API/source distinctions, project ownership, navigation/link integrity and source/example verification remain applicable within the approved scope. Historical evidence is unchanged.

The accompanying [clause mapping](API_DRAFT_RULE_CHANGES.json) records each amended destination clause before and after this change. The mapping updates the authority hashes and cites this semantic review; a hash update alone is not the review. Scope and publication changes are authorized by the user's explicit API draft/main plan.

Implementation and hosted activation remain in progress; these rules do not certify that the synchronization workflow or deployment has already run successfully.

## Final reader scope amendment

The user subsequently restricted Engine API to C# projects under src/ and build/, excluding tests and fixtures. Platform implementation guides and API are retired; shared contracts and truthful availability limits remain. Tooling and entry usage guides do not create tool API obligations. Original rule IDs and source hashes are preserved; [the exact clause amendment](API_SCOPE_RULE_CHANGES.json) records this explicitly authorized scope change.
