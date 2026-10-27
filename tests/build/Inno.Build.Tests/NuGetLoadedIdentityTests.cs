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
    public Task IsolatedHostRejectsARealLoadedPayloadMismatch() => VerifyScope(true, true);

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
            File.Copy(runtime, mismatch);
            await using (FileStream altered = new(mismatch, FileMode.Append, FileAccess.Write))
                await altered.WriteAsync(new byte[] { 0x49, 0x4E, 0x4E, 0x4F }, cancellation.Token);
            WeakReference retired = LoadMismatchedContent(runtime, mismatch, loaded);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.False(retired.IsAlive);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference LoadMismatchedContent(
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
            return new WeakReference(context);
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
