# Bounded documentation page preparation

The UI migration exposed a writer bottleneck while prepared queries remained independent: a 5,399-page publication spent 103.09 seconds in page preparation, including 40.43 seconds in immutable owner registration, but only 0.50 seconds parsing changed Markdown. Comparable unchanged-source samples are retained before judging improvement.

## Change and ownership review

The writer prepares independently content-addressed pages with at most `min(processor count, 4)` workers. Each worker owns one output slot, path-derived cache key and exclusive page lease. The existing publisher is immutable configuration; page registrations write distinct entry files and do not update a shared current root. The outer writer/activity protection remains held until all workers drain.

Content hashes, path checks, ownership registration, file inventory, cancellation and final freshness checks remain in place. Results are collected in deterministic source order. Catalog validation, dependency registration and atomic generation selection remain serial. A failure or cancellation cannot proceed to generation publication. No query reader, engine product or Native preparation path is changed.

Accumulated parsing/registration measurements are worker time and can overlap. The enclosing PagePreparation value is wall time. Do not add worker totals to obtain elapsed time.

## Regression evidence

All 35 documentation tests passed locally under SDK 10.0.402, with no skips. The concurrent writer/reader test now includes 24 independent pages. A new test holds a real Core IO page lease, observes independent payload preparation, cancels the writer, verifies that the prior pointer and reader remain valid, drains workers, then resumes and reads the complete new generation. Existing corruption, preserved-timestamp edits, moves, boundaries, includes and ambiguous queries remain covered.

The initial compile rejected use of Core IO without an explicit test-project reference; the test now declares that real public dependency rather than widening production visibility. The first test execution also correctly returned Partial for an API page with unread required contracts. Its assertion now checks Partial, the original snapshot and the ownership continuation instead of treating the excerpt as complete. Both first-failure logs and the failed TRX remain under the owned acceptance artifacts.

Passing TRX: `artifacts/acceptance/english-documentation/bounded-writer/bounded-writer-complete.trx`. This certifies the local concurrency/cancellation scope. Comparative writer performance and real remote execution remain separate work.

## Comparable writer samples

Five sequential warm confirmation invocations used the same 5,399-page content, Windows machine, .NET 10.0.402 SDK/10.0.14 runtime and owned C-drive cache. Every invocation reused all 5,399 parsed pages. The previous writer was revision `84d72179ccc5ee2fa27ff25b956ec10e9cf5f2d5`; the bounded writer was `c48b7d11fd48d8de21aaf94e416c73c37d70df26`. No builds or tests ran concurrently. Tool compilation and the new query-schema publication are excluded from warm confirmation samples.

| Writer | Total milliseconds, in execution order | Median | Page preparation median |
| --- | --- | ---: | ---: |
| Sequential | 76450.341, 11091.3491, 11390.4072, 13782.5289, 12144.5334 | 12144.5334 | 6871.6181 |
| Bounded | 22124.6487, 8185.4288, 8205.3828, 8263.3709, 7714.162 | 8205.3828 | 2483.3147 |

The median writer total decreased 32.44%; page preparation decreased 63.86%. These measurements retain both slow first samples. They do not establish the cause of that variance, cold-cache performance, query latency, or whole-CI acceleration. Total measures the index operation, excluding process startup and catalog serialization. Ordinary prepared-snapshot reads do not perform this confirmation operation.

Raw phase measurements, log hashes, content identity and snapshots are recorded in the paired Docs repository at `.agents/records/acceptance/DOCUMENTATION_WRITER_2026_11_11.json`. The implementation preserves full page validation and owner registration rather than replacing integrity checks with timestamps.
