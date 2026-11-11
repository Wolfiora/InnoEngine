using System;

namespace EditorUiAcceptance;

internal readonly record struct ManagedMemorySample(
    long liveBytes,
    long estimatedBytes,
    long heapBytes,
    long fragmentedBytes,
    long collectionIndex,
    int generation,
    bool compacted,
    bool concurrent
) {
    internal static ManagedMemorySample Capture()
    {
        long previousIndex = GC.GetGCMemoryInfo(GCKind.FullBlocking).Index;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
        GCMemoryInfo memory = GC.GetGCMemoryInfo(GCKind.FullBlocking);
        if (memory.Index <= previousIndex || memory.Generation != GC.MaxGeneration || memory.Concurrent || !memory.Compacted ||
            memory.FragmentedBytes < 0 || memory.HeapSizeBytes < memory.FragmentedBytes)
            throw new InvalidOperationException("The lifecycle sample requires a fresh compacting full-GC snapshot.");

        // Keep the approximate API as diagnostic data; pinned fragmentation can make it negative.
        return new ManagedMemorySample(
            checked(memory.HeapSizeBytes - memory.FragmentedBytes), GC.GetTotalMemory(forceFullCollection: false),
            memory.HeapSizeBytes, memory.FragmentedBytes, memory.Index, memory.Generation, memory.Compacted, memory.Concurrent);
    }
}
