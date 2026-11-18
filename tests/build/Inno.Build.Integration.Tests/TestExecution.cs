using Xunit;

// SDK fixtures share checkout outputs; concurrency is exercised explicitly within each ownership contract.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
