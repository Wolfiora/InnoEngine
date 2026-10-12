using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

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
                scoped,
                output,
                token
            ) => {
                string snapshot = Path.Combine(scoped.GetNativeBuildRoot(BgfxNativeBuild.componentDescriptor), "Sources");
                foreach (string component in new[] { "bgfx", "bx", "bimg" })
                    await CopySourcesAsync(scoped, Path.Combine(request.externalDirectory, component), Path.Combine(snapshot, component), token).ConfigureAwait(false);
                string source = Path.Combine(snapshot, "bgfx");
                await ToolchainEnvironment.RunAsync(scoped, request.genie, request.generatorArguments, source, token).ConfigureAwait(false);
                await ToolchainEnvironment.RunAsync(scoped, request.compiler, request.invocation.arguments, source, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                if (includeTools)
                    CopyTools(source, output, request.artifactToken, scoped);
                else
                    BuildArtifactCopier.CopyArtifacts(source, output, scoped.configuration,
                        new(".build", ["bgfx-shared-lib"], [".dll", ".dylib", ".so"],
                            [request.artifactToken], ToolchainEnvironment.NormalizeOutputName));
            }, cancellationToken).ConfigureAwait(false);
    }

    private static async Task CopySourcesAsync(
        NativeBuildContext context,
        string source,
        string destination,
        CancellationToken cancellationToken
    ) {
        Directory.CreateDirectory(destination);
        // Sources are exclusively owned by this workspace; generated build trees remain intact.
        foreach (string previous in Directory.EnumerateFileSystemEntries(destination))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string name = Path.GetFileName(previous);
            if (name is ".git" or ".build" or "bin" or "obj")
                continue;
            string current = Path.Combine(source, name);
            if (File.Exists(current) || Directory.Exists(current))
                continue;
            string owned = PathBoundary.RequireUnlinkedPath(destination, previous);
            if (Directory.Exists(owned))
            {
                _ = PathBoundary.EnumerateFiles(owned).ToArray();
                Directory.Delete(owned, recursive: true);
            }
            else
                File.Delete(owned);
        }
        foreach (string file in Directory.EnumerateFiles(source))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Path.GetFileName(file) == ".git")
                continue;
            string output = Path.Combine(destination, Path.GetFileName(file));
            await NativeInputMaterializer.CopyAsync(context, file, output, cancellationToken).ConfigureAwait(false);
        }
        foreach (string directory in Directory.EnumerateDirectories(source))
        {
            if (Path.GetFileName(directory) is ".git" or ".build" or "bin" or "obj")
                continue;
            await CopySourcesAsync(context, directory, Path.Combine(destination, Path.GetFileName(directory)), cancellationToken).ConfigureAwait(false);
        }
    }

    private static void CopyTools(
        string source,
        string output,
        string artifactToken,
        NativeBuildContext context
    ) {
        string configuration = context.configuration;
        string build = Path.Combine(source, ".build");
        string extension = context.RequireToolchain().host.system == "Windows" ? ".exe" : string.Empty;
        string suffix = configuration == "debug" ? "Debug" : "Release";
        foreach (string name in new[] { "shaderc", "texturec", "geometryc", "geometryv", "texturev" })
        {
            string[] candidates = Directory.EnumerateFiles(build, name + suffix + extension, SearchOption.AllDirectories)
                .Where(file => file.Replace('\\', '/').Contains(artifactToken, StringComparison.Ordinal)).ToArray();
            if (candidates.Length != 1)
                throw new FileNotFoundException($"Expected one '{name}' tool for {context.RequireToolchain().targetId}/{configuration}; found {candidates.Length}.");
            string destination = Path.Combine(output, name + "-" + configuration + extension);
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
