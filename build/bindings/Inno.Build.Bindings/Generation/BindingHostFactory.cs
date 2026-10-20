using System.IO;
using System.Threading;
using BGCS;
using BGCS.Infrastructure.Workers;

namespace Inno.Build.Bindings;

internal static class BindingHostFactory
{
    internal static BindingHostBuilder CreateBuilder(CancellationToken cancellation)
    {
        string application = Path.GetDirectoryName(typeof(BindingHost).Assembly.Location)!;
        string worker = WorkerAssetProvisioner.Prepare(application, cancellationToken: cancellation);
        return BindingHost.CreateBuilder().AddBuiltIns().WithWorkerAssetRoot(worker);
    }
}
