using System;
using System.Threading;
using Inno.Core.Execution;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("9f9f5f9f-4d86-414f-9ff9-78ad8ec60606")]
internal sealed class HookAsset : AssetObject
{
    internal int payloadChangeCount;
    internal int unloadingCount;
    internal int pendingUnloads;
    internal bool failUnloading;
    internal bool retirementExpired;

    protected override void OnRuntimePayloadChanged(
        ReadOnlyMemory<byte> previousPayload,
        ReadOnlyMemory<byte> currentPayload
    ) => Interlocked.Increment(ref payloadChangeCount);

    protected override void OnUnloading()
    {
        Interlocked.Increment(ref unloadingCount);
        if (retirementExpired)
        {
            throw new RetirementTimeoutException("The test asset work exceeded its retirement deadline.");
        }
        if (pendingUnloads > 0)
        {
            pendingUnloads--;
            throw new RetirementPendingException("The test asset still owns unfinished work.");
        }
        if (failUnloading)
        {
            throw new InvalidOperationException("The test asset release failed.");
        }
    }
}
