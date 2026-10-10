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
        ArgumentNullException.ThrowIfNull(context);
        context.RequireToolchain();
        ArgumentNullException.ThrowIfNull(profile);
        if (profile.targetId != context.RequireToolchain().targetId)
            throw new ArgumentException("The BGFX profile does not match the frozen toolchain target.", nameof(profile));
        NativeComponentBuildOptions options = context.RequireComponentOptions(BgfxNativeBuild.componentDescriptor);
        if (options.libraryKind != NativeLibraryKind.Shared || options.cmakeArguments.Count != 0)
            throw new NotSupportedException("The standalone graphics recipe requires shared linkage and its explicit generation profile.");
        string artifactToken = profile.artifactPathToken;
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactToken);
        string[] generatorArguments = profile.generatorArguments?.ToArray()
            ?? throw new ArgumentException("The BGFX profile has no generator arguments.", nameof(profile));
        if (generatorArguments.Length == 0 || generatorArguments.Any(static value => value is null))
            throw new ArgumentException("The BGFX profile requires assigned generator arguments.", nameof(profile));
        string[] generate = includeTools
            ? ["--with-shared-lib", "--with-tools", .. generatorArguments]
            : ["--with-shared-lib", .. generatorArguments];
        BgfxBuildInvocation invocation = profile.CreateBuildInvocation(context, includeTools)
            ?? throw new ArgumentException("The BGFX profile returned no SDK invocation.", nameof(profile));
        string compiler = context.RequireToolchain().ResolveExecutable(invocation.tool);
        string[] declarations = ["artifact=" + artifactToken,
            .. generate.Select(static value => "generate=" + value),
            "compiler=" + compiler, .. invocation.arguments.Select(static value => "compile=" + value)];
        string external = Path.Combine(context.engineRoot, "extern");
        string bgfx = Path.Combine(external, "bgfx");
        string bx = Path.Combine(external, "bx");
        string bimg = Path.Combine(external, "bimg");
        BgfxBuildUtils.ValidateSubmodules(bgfx, bx, bimg);
        string genie = ResolveGenie(context);
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, BgfxNativeBuild.componentDescriptor, includeTools ? "bgfx-tools" : "bgfx", context.RequireToolchain().targetId, [bgfx, bx, bimg, genie, Path.Combine(context.engineRoot, "build", "toolchains", "Inno.Build.Toolchains", "Native", "NativeInputMaterializer.cs")], declarations);
        return await NativeArtifactPublisher.PublishAsync(
            context,
            recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string snapshot = Path.Combine(scoped.GetNativeBuildRoot(BgfxNativeBuild.componentDescriptor), "Sources");
                foreach (string component in new[] { "bgfx", "bx", "bimg" })
                    await CopySourcesAsync(scoped, Path.Combine(external, component), Path.Combine(snapshot, component), token).ConfigureAwait(false);
                string source = Path.Combine(snapshot, "bgfx");
                await ToolchainEnvironment.RunAsync(scoped, genie, generate, source, token).ConfigureAwait(false);
                await ToolchainEnvironment.RunAsync(scoped, compiler, invocation.arguments, source, token).ConfigureAwait(false);
                token.ThrowIfCancellationRequested();
                if (includeTools)
                    CopyTools(source, output, artifactToken, scoped);
                else
                    BuildArtifactCopier.CopyArtifacts(source, output, scoped.configuration,
                        new(".build", ["bgfx-shared-lib"], [".dll", ".dylib", ".so"],
                            [artifactToken], ToolchainEnvironment.NormalizeOutputName));
            }, cancellationToken).ConfigureAwait(false);
    }

    private static string ResolveGenie(NativeBuildContext context)
    {
        if (context.RequireToolchain().TryResolveExecutable("genie", out string configured))
            return configured;
        BuildHostDescriptor host = context.RequireToolchain().host;
        string platform = host.system == "Windows" ? "windows" : host.system == "MacOS" ? "darwin" : "linux";
        if (host.system == "Linux" && host.architecture == "arm64")
            throw new FileNotFoundException("The selected toolchain must declare a host-native ARM64 GENie executable.");
        return ToolchainEnvironment.ResolveExecutable(Path.Combine(context.engineRoot, "extern", "bx",
            "tools", "bin", platform, host.system == "Windows" ? "genie.exe" : "genie"));
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
