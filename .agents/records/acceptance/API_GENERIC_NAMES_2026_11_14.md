# Readable generic API names

API type labels now retain the actual parameter names from captured declarations. Navigation qualifies genuinely ambiguous labels with their namespace instead of exposing XML documentation arity markers as `Value1` or `Value2`. Escaped Markdown displays angle brackets as text, and composed page metadata uses the same source-derived title even when the author owns the prose.

Storage paths remain derived from the existing unparameterized identity policy. This change does not rename API fragments, change symbol identities, regenerate source facts, or alter runtime behavior. Manual fragments are preserved byte-for-byte.

All 48 synchronization regressions passed, including generic delegates, nested generic/non-generic types, escaped identifiers, same-name types, renamed parameters, query output and manual prose. The first local compile exposed a test assertion against a nonexistent hit-title property; it was corrected to assert the actual hit text and catalog title. Its original log remains in the ignored acceptance directory.

The corrected tool synchronized the complete 17,971-symbol Docs reference with 73 changed generated files. Site projection and browser evidence are recorded in the Docs repository. No performance, stress, Native product or full engine matrix was run for this presentation correction.
