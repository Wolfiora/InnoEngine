using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using BGCS;
using BGCS.Core.Hosting;
using BGCS.Core.Ports;
using BGCS.Core.Plugins;
using BGCS.Frontend.Clang.Workers;
using BGCS.Hosting.Standard;

namespace Inno.Build.Bindings;

internal static class BindingHostFactory
{
    internal static HostServices CreateHostServices(string baseDirectory) => new HostServicesProvider().Create(
        new(baseDirectory, JsonSerializer.SerializeToElement(new { })));

    internal static BindingHostBuilder CreateBuilder(
        NativeBindingProfile profile,
        CancellationToken cancellation
    ) {
        cancellation.ThrowIfCancellationRequested();
        string application = Path.GetDirectoryName(typeof(BindingHost).Assembly.Location)!;
        // MSBuild's application directory is not the privately loaded SDK's deployment root.
        BindingHostBuilder builder = BindingGenerator.CreateBuilder().ReplaceFactory<IFrontend>(
            "bgcs.frontend.clang.worker",
            new("inno.bindings.frontend.clang.worker", ExtensionPort.Frontend),
            context =>
            {
                HostServices host = context.RequireHostServices();
                return new FrontendWorkerClient(WorkerAssetLocator.Locate(host, applicationBase: application), host,
                    TimeSpan.FromSeconds(300));
            });
        profile.Configure(builder);
        return builder;
    }
}
