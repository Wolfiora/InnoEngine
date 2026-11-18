using Xunit;

// Preserve the original fixture lifetime isolation while ownership is split across assemblies.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
