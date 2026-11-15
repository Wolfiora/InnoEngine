# Incremental documentation CI implementation

This change implements the approved November 16 documentation CI/API-export plan. Engine functional, Native, device and product policies are unchanged. Formal merges remain human-controlled.

## Input and ownership boundaries

- Preflight compares complete conservative committed file membership and identities against a trusted complete export, before SDK/submodule/matrix preparation. Records and root prose are excluded; unknown build relationships expand conservatively.
- Actual evaluated projects, imported files, Compile/generator/reference inputs, SDK and dependency closure determine owner keys. Cached captures remain immutable, are hash checked and acquire fresh local ownership. Active leases/obj are never transported.
- Required owner builds form one managed union. Script contexts retain actual projections; without a sufficiently narrow dependency proof, their complete evaluated input closure invalidates conservatively.
- Final multi-host/configuration reconciliation still requires a complete inventory. Cached owner provenance remains distinct from current input equivalence.
- Equal complete fact identity returns before main ancestry merge or any Docs commit/push. Manual ownership and concurrent fast-forward publication protection remain intact.

## Finite evidence

- Documentation catalog reference-link export regression: passed.
- Owner planning, repeated identity, immutable payload reuse, original producer provenance, new Compile item invalidation and cancellation: passed.
- Publisher fixture: first publication, identical no-op despite advanced main, concurrent manual takeover, outdated metadata after XML changes and stale source rejection: passed. The first run exposed an outdated fixture expectation that incorrectly forbade the intentional outdated metadata update; original diagnostic retained.
- Engine Architecture managed build: passed, zero warnings/errors.
- Remote API export run [43758842003](https://github.com/Wolfiora/InnoEngine/actions/runs/43758842003): Windows/macOS Debug/Release and complete package reconciliation passed. The preceding first preflight failure was command-name recursion; it was corrected before retry.
- Preflight fixture: unchanged and README-only inputs skip extraction; new and deleted source membership require evaluation.
- Restored immutable snapshot regression: passed. Restored payloads acquire fresh local generation ownership; confirming a protected snapshot does not wait on its existing reader.
- Docs CI and publication candidate: tracked in the Docs implementation record. Human merge and actual deployment remain separate actions.

No stress tests, repeated performance matrix or formal release was run. Source changes from another session in Inno.Editor.Assets/Properties/ScriptingApi.cs are excluded.
