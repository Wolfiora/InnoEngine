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
