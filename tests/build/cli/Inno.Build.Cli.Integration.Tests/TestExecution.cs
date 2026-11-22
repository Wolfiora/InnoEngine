using Xunit;

// Command diagnostics use process-wide console streams. Real concurrency belongs to owner-level fixtures.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
