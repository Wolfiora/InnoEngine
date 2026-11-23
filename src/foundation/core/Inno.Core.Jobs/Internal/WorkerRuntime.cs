using System.Threading;

namespace Inno.Core.Jobs.Internal;

internal sealed class WorkerRuntime
{
    internal WorkerRuntime(
        int workerID,
        ThreadStart loopBody
    ) {
        this.id = workerID;
        this.localQueue = new WorkStealingDeque<int>();
        this.thread = new Thread(loopBody)
        {
            IsBackground = true,
            Name = $"Inno.JobWorker.{workerID}"
        };
    }

    internal int id { get; }
    internal Thread thread { get; }
    internal WorkStealingDeque<int> localQueue { get; }
}
