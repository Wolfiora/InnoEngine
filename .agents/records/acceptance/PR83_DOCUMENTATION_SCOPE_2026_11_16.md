# PR 83 documentation scope reconciliation — 2026-11-16

The Windows and macOS jobs in run 43822806641 failed in seven cases from Inno.Tooling.Validation.Tests. The retained Windows TRX records show that all seven stopped at documentation indexing: synthetic pages named platform implementation owners that the canonical catalog intentionally excludes. The three incremental-compilation cases had not reached their compilation assertions.

The shared fixture now preserves the complete inventory as provenance while omitting platform reader pages. Synthetic target-matching and foreign-target projects live under src, so their existing assertions continue to exercise target selection, source proof and rejection of foreign execution within the documented scope.

A subsequent real compilation run exposed the remaining caller mismatch: ValidationRunner required project entries for every input project, including platforms. The documentation obligation now applies the same platform exclusion as the canonical catalog. A two-case regression proves that platform inventory remains present without a reader entry and that an absent required src project entry still fails. No runtime behavior, public API, branch protection or functional acceptance requirement changed.

Local evidence at commit time: the four original focused cases and both new scope cases pass on Windows/.NET SDK 10.0.402. The three original real compilation cases are running separately; complete Windows/macOS CI remains required. First failures and local TRX files are retained under artifacts/pr83-repair. The downloaded 144 MB diagnostic archive was removed after retaining its failed-case records; remote evidence remains attached to the original run.

The unrelated working-tree change in Inno.Editor.Assets/Properties/ScriptingApi.cs is not part of this repair.
