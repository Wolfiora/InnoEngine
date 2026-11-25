using Xunit;

// Producer regressions own one native workspace; concurrency contracts have separate fixtures.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
