using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Cli;

internal static class EditorBuildCommand
{
    internal static async Task RunAsync(
        BuildCommandContext context,
        BuildCommandLine options,
        CancellationToken cancellationToken
    ) {
        string project = await context.PreflightEditorAsync(cancellationToken).ConfigureAwait(false);
        using var ownership = await ProductCleanExecutor.AcquireAsync(context, cancellationToken).ConfigureAwait(false);
        if (options.Has("clean"))
        {
            ProductCleanPlan plan = await ProductCleanPlan.CreateAsync(context, project, cancellationToken).ConfigureAwait(false);
            ProductCleanExecutor.Execute(plan, false, cancellationToken);
        }
        await ToolchainEnvironment.RunAsync(context.composition.dotnetHost, ["build", ..context.ProductArguments(project)],
            context.root, cancellationToken, DotNetSdkEnvironment.Create(context.composition.dotnetHost)).ConfigureAwait(false);
    }
}
