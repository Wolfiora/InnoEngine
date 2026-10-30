using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using BGCS.Runtime;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NuGetLoadedIdentityTests
{
    private const string C_EXPECTED_RUNTIME = "INNO_PACKAGE_IDENTITY_EXPECTED_RUNTIME";
    private const string C_MISMATCH_RUNTIME = "INNO_PACKAGE_IDENTITY_MISMATCH_RUNTIME";
    private const string C_PROBE_RESULT = "INNO_PACKAGE_IDENTITY_PROBE_RESULT";
    private const string C_VERIFIED_PROBE = "loaded-content-mismatch-rejected;context-collected;assembly-collected";

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public Task SdkOutputScopesMatchTheActuallyLoadedRuntime(
        bool isolated,
        bool staleDefault
    ) => VerifyScope(isolated, staleDefault);

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public Task IsolatedHostRejectsARealLoadedPayloadMismatch()
    {
        string? expected = Environment.GetEnvironmentVariable(C_EXPECTED_RUNTIME);
        if (expected is null)
            return VerifyScope(true, true);
        string mismatch = Environment.GetEnvironmentVariable(C_MISMATCH_RUNTIME)
            ?? throw new InvalidOperationException("The loaded-package probe has no owned mismatch input.");
        string result = Environment.GetEnvironmentVariable(C_PROBE_RESULT)
            ?? throw new InvalidOperationException("The loaded-package probe has no result owner.");
        (WeakReference retired, WeakReference retiredAssembly) = LoadMismatchedContent(expected, mismatch, typeof(Utf8Codec).Assembly);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(retired.IsAlive);
        Assert.False(retiredAssembly.IsAlive);
        File.WriteAllText(result, C_VERIFIED_PROBE);
        return Task.CompletedTask;
    }

    private static async Task VerifyScope(
        bool isolated,
        bool staleDefault
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoLoadedPackageIdentity", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string repository = TestRepository.GetRoot(typeof(NuGetLoadedIdentityTests).Assembly);
            string sdk = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            string version = (await ToolchainEnvironment.CaptureOutputAsync(sdk,
                ["msbuild", Path.Combine(repository, "backends/ImGui/native/Inno.Native.ImGui/Inno.Native.ImGui.csproj"),
                    "-nologo", "-nr:false", "-getProperty:InnoBgcsPackageVersion"],
                repository, cancellation.Token, DotNetSdkEnvironment.Create(sdk))).Trim();
            string project = Path.Combine(root, "Consumer.csproj");
            new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"),
                    new XElement("LangVersion", "14.0"), new XElement("ImplicitUsings", "disable")),
                new XElement("ItemGroup", new XElement("PackageReference", new XAttribute("Include", "BGCS.Runtime"),
                    new XAttribute("Version", "[" + version + "]"))))).Save(project);
            string defaultAssets = Path.Combine(root, "obj", "project.assets.json");
            const string stale = "unrelated stale default assets must never be consumed";
            if (staleDefault)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(defaultAssets)!);
                File.WriteAllText(defaultAssets, stale);
            }
            string[] owner = isolated ? ["-p:ArtifactsPath=" + Path.Combine(root, "owned-output")] : [];
            string evaluated = await ToolchainEnvironment.CaptureOutputAsync(sdk,
                ["msbuild", project, "-t:Restore", "-nologo", "-nr:false", "-v:quiet", "-getProperty:ProjectAssetsFile", .. owner],
                root, cancellation.Token, DotNetSdkEnvironment.Create(sdk));
            string assetsPath = evaluated.Trim();
            Assert.True(File.Exists(assetsPath), evaluated);
            if (isolated)
            {
                Assert.StartsWith(Path.Combine(root, "owned-output"), assetsPath, StringComparison.OrdinalIgnoreCase);
                if (staleDefault)
                    Assert.Equal(stale, File.ReadAllText(defaultAssets));
                else
                    Assert.False(File.Exists(defaultAssets));
            }
            else
                Assert.Equal(defaultAssets, assetsPath);
            using JsonDocument assets = JsonDocument.Parse(File.ReadAllBytes(assetsPath));
            string library = "BGCS.Runtime/" + version;
            JsonElement package = assets.RootElement.GetProperty("libraries").GetProperty(library);
            string packageRoot = assets.RootElement.GetProperty("packageFolders").EnumerateObject()
                .Select(folder => Path.Combine(folder.Name, package.GetProperty("path").GetString()!)).Single(Directory.Exists);
            JsonElement target = assets.RootElement.GetProperty("targets").EnumerateObject().Single().Value.GetProperty(library);
            string relative = target.GetProperty("runtime").EnumerateObject().Single(item => item.Name.EndsWith(".dll", StringComparison.Ordinal)).Name;
            string runtime = Path.Combine(packageRoot, relative);
            Assembly loaded = typeof(Utf8Codec).Assembly;
            AssertLoadedContent(runtime, loaded);

            string mismatch = Path.Combine(root, "mismatched", "BGCS.Runtime.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(mismatch)!);
            File.WriteAllBytes(mismatch, File.ReadAllBytes(runtime));
            await using (FileStream altered = new(mismatch, FileMode.Append, FileAccess.Write))
                await altered.WriteAsync(new byte[] { 0x49, 0x4E, 0x4E, 0x4F }, cancellation.Token);
            await RunLoadedMismatchProbeAsync(sdk, runtime, mismatch, root, cancellation.Token);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static async Task RunLoadedMismatchProbeAsync(
        string sdk,
        string expected,
        string mismatch,
        string root,
        CancellationToken cancellationToken
    ) {
        string result = Path.Combine(root, "loaded-package-probe.txt");
        string output = await ToolchainEnvironment.CaptureOutputAsync(sdk,
            ["test", typeof(NuGetLoadedIdentityTests).Assembly.Location,
                "--filter", "FullyQualifiedName=" + typeof(NuGetLoadedIdentityTests).FullName + "." + nameof(IsolatedHostRejectsARealLoadedPayloadMismatch),
                "--results-directory", Path.Combine(root, "probe-results"),
                "--logger", "trx;LogFileName=loaded-package.trx",
                "--environment", C_EXPECTED_RUNTIME + "=" + expected,
                "--environment", C_MISMATCH_RUNTIME + "=" + mismatch,
                "--environment", C_PROBE_RESULT + "=" + result],
            root, cancellationToken, DotNetSdkEnvironment.Create(sdk));
        Assert.True(File.Exists(result), output);
        Assert.Equal(C_VERIFIED_PROBE, File.ReadAllText(result));
        // The physical image belongs to the probe process; normal cleanup follows its complete exit.
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference context, WeakReference assembly) LoadMismatchedContent(
        string expected,
        string mismatch,
        Assembly loaded
    ) {
        var context = new AssemblyLoadContext("package-content-mismatch", isCollectible: true);
        try
        {
            Assembly different = context.LoadFromAssemblyPath(mismatch);
            Assert.Equal(loaded.GetName().FullName, different.GetName().FullName);
            Assert.Equal(loaded.ManifestModule.ModuleVersionId, different.ManifestModule.ModuleVersionId);
            Assert.Throws<Xunit.Sdk.EqualException>(() => AssertLoadedContent(expected, different));
            return (new WeakReference(context), new WeakReference(different));
        }
        finally
        {
            context.Unload();
        }
    }

    private static void AssertLoadedContent(
        string expected,
        Assembly loaded
    ) => Assert.Equal(Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(expected))),
        Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(loaded.Location))));
}
