using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Bgfx;

internal static class BgfxBuildSession
{
    internal static async Task<NativeBuildProduct> PublishAsync(
        NativeBuildContext context,
        BgfxNativeBuildProfile profile,
        bool includeTools,
        CancellationToken cancellationToken
    ) {
        var request = new BgfxBuildRequest(context, profile, includeTools);
        return await NativeArtifactPublisher.PublishAsync(
            request.context,
            request.recipe,
            async (
                @scoped,
                output,
                token
            ) => {
                NativeCMakeSource snapshot = await NativeCMakeExecutor.PrepareSourceAsync(scoped, request.component, token).ConfigureAwait(false);
                string source = snapshot.ResolvePath(Path.Combine(request.externalDirectory, "bgfx"));
                string[] generatorArguments = request.generatorArguments;
                if (!includeTools)
                {
                    string script = await BgfxBindingProject.PrepareAsync(scoped, request, profile, source, token).ConfigureAwait(false);
                    generatorArguments = ["--file=" + script, .. generatorArguments];
                }
                await ToolchainEnvironment.RunAsync(scoped, request.genie, generatorArguments, source, token).ConfigureAwait(false);
                await ToolchainEnvironment.RunAsync(scoped, request.compiler, request.invocation.arguments, source, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                if (includeTools)
                    CopyTools(source, output, request.artifactToken, scoped);
                else
                    BuildArtifactCopier.CopyArtifacts(source, output, scoped.configuration,
                        new(".build", ["bgfx-shared-lib"], [scoped.RequireToolchain().sharedLibraryExtension],
                            [request.artifactToken], ToolchainEnvironment.NormalizeOutputName));
            }, cancellationToken, describeOutputs: (
                @scoped,
                output
            ) => Directory.EnumerateFiles(output, "*", SearchOption.AllDirectories)
                .Select(file => new NativeArtifact(Path.GetRelativePath(output, file), file,
                    includeTools ? NativeArtifactRole.SupportFile : NativeArtifactRole.SharedLibrary)).ToArray()).ConfigureAwait(false);
    }

    private static void CopyTools(
        string source,
        string output,
        string artifactToken,
        NativeBuildContext context
    ) {
        string configuration = context.configuration;
        string build = Path.Combine(source, ".build");
        string extension = context.RequireToolchain().host.services.executableExtension;
        string suffix = configuration == "debug" ? "Debug" : "Release";
        foreach (string name in new[] { "shaderc", "texturec", "geometryc", "geometryv", "texturev" })
        {
            string[] candidates = Directory.EnumerateFiles(build, name + suffix + extension, SearchOption.AllDirectories)
                .Where(file => file.Replace('\\', '/').Contains(artifactToken, StringComparison.Ordinal)).ToArray();
            if (candidates.Length != 1)
                throw new FileNotFoundException($"Expected one '{name}' tool for {context.RequireToolchain().targetId}/{configuration}; found {candidates.Length}.");
            string destination = Path.Combine(output, name + extension);
            File.Copy(candidates[0], destination);
            if (!OperatingSystem.IsWindows())
                File.SetUnixFileMode(destination, File.GetUnixFileMode(candidates[0]));
        }
        string includes = Path.Combine(output, "includes");
        Directory.CreateDirectory(includes);
        foreach (string name in new[] { "bgfx_shader.sh", "bgfx_compute.sh" })
            File.Copy(Path.Combine(source, "src", name), Path.Combine(includes, name));
    }
}
