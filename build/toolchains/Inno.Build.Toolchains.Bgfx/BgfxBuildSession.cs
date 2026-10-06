using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains.Bgfx.Platforms;

namespace Inno.Build.Toolchains.Bgfx;

internal static class BgfxBuildSession
{
    internal static async Task<NativeBuildProduct> PublishAsync(
        NativeBuildContext context,
        bool includeTools,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        context = await HostNativeToolchain.ResolveAsync(context, cancellationToken).ConfigureAwait(false);
        BgfxBuilder builder = BgfxBuilderFactory.CreateForCurrentPlatform();
        string external = Path.Combine(context.engineRoot, "extern");
        string bgfx = Path.Combine(external, "bgfx");
        string bx = Path.Combine(external, "bx");
        string bimg = Path.Combine(external, "bimg");
        BgfxBuildUtils.ValidateSubmodules(bgfx, bx, bimg);
        string genie = ResolveGenie(context);
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, typeof(BgfxNativeBuild).Assembly, includeTools ? "bgfx-tools" : "bgfx", builder.outputPlatform, [bgfx, bx, bimg, genie, Path.Combine(context.engineRoot, "build", "toolchains", "Inno.Build.Toolchains", "Native", "NativeInputMaterializer.cs")], []);
        return await NativeArtifactPublisher.PublishAsync(
            context,
            recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string snapshot = Path.Combine(scoped.GetNativeBuildRoot(typeof(BgfxNativeBuild).Assembly), "Sources");
                foreach (string component in new[] { "bgfx", "bx", "bimg" })
                    await CopySourcesAsync(scoped, Path.Combine(external, component), Path.Combine(snapshot, component), token).ConfigureAwait(false);
                string source = Path.Combine(snapshot, "bgfx");
                await builder.BuildAsync(source, genie, scoped, includeTools, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                if (includeTools)
                    CopyTools(source, output, builder, scoped.configuration);
                else
                    BuildArtifactCopier.CopyArtifacts(source, output, scoped.configuration,
                        new(".build", ["bgfx-shared-lib"], [".dll", ".dylib", ".so"],
                            [builder.artifactPathToken], ToolchainEnvironment.NormalizeOutputName));
            }, cancellationToken).ConfigureAwait(false);
    }

    private static string ResolveGenie(NativeBuildContext context)
    {
        string? configured = Environment.GetEnvironmentVariable("BGFX_GENIE");
        if (!string.IsNullOrWhiteSpace(configured))
            return ToolchainEnvironment.ResolveExecutable(configured);
        string platform = OperatingSystem.IsWindows() ? "windows" : OperatingSystem.IsMacOS() ? "darwin" : "linux";
        if (OperatingSystem.IsLinux() && RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
            throw new FileNotFoundException("Set BGFX_GENIE to a host-native ARM64 GENie executable.");
        return ToolchainEnvironment.ResolveExecutable(Path.Combine(context.engineRoot, "extern", "bx",
            "tools", "bin", platform, OperatingSystem.IsWindows() ? "genie.exe" : "genie"));
    }

    private static async Task CopySourcesAsync(
        NativeBuildContext context,
        string source,
        string destination,
        CancellationToken cancellationToken
    ) {
        Directory.CreateDirectory(destination);
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
        BgfxBuilder builder,
        string configuration
    ) {
        string build = Path.Combine(source, ".build");
        string extension = OperatingSystem.IsWindows() ? ".exe" : string.Empty;
        string suffix = configuration == "debug" ? "Debug" : "Release";
        foreach (string name in new[] { "shaderc", "texturec", "geometryc", "geometryv", "texturev" })
        {
            string[] candidates = Directory.EnumerateFiles(build, name + suffix + extension, SearchOption.AllDirectories)
                .Where(file => file.Replace('\\', '/').Contains(builder.artifactPathToken, StringComparison.Ordinal)).ToArray();
            if (candidates.Length != 1)
                throw new FileNotFoundException($"Expected one '{name}' tool for {builder.outputPlatform}/{configuration}; found {candidates.Length}.");
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
