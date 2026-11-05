using Xunit;

// Cost samples own one native workspace; concurrency contracts have separate fixtures.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
