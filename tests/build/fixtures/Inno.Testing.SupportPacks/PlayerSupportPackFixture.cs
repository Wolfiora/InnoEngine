using System;
using System.IO;
using System.Linq;
using Inno.Build;
using Inno.Build.MacOSArm64;
using Inno.Build.WindowsX64;
using Inno.Core.Identity;

namespace Inno.Testing.SupportPacks;

public abstract class PlayerSupportPackFixture : IDisposable
{
    protected readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBuildTests", Guid.NewGuid().ToString("N"));
    protected readonly string m_supportPackRoot;
    private readonly string[] m_referenceAssemblies;

    protected PlayerSupportPackFixture()
        : this([typeof(IdentityAllocator).Assembly.Location]) {}

    protected PlayerSupportPackFixture(string[] referenceAssemblies)
    {
        m_referenceAssemblies = referenceAssemblies;
        m_supportPackRoot = Path.Combine(m_root, "SupportPacks");
        Directory.CreateDirectory(m_root);
        CreateSupportPack(new BuildTargetId("macos-arm64"), "Inno.Player.MacOSArm64");
        CreateSupportPack(new BuildTargetId("windows-x64"), "Inno.Player.WindowsX64.exe");
    }

    public virtual void Dispose()
    {
        if (!Path.GetFullPath(m_root)
            .StartsWith(
                Path.Combine(Path.GetTempPath(), "InnoBuildTests") + Path.DirectorySeparatorChar,
                StringComparison.OrdinalIgnoreCase
            )
        ) {
            throw new InvalidOperationException("The support pack fixture escaped its owned directory.");
        }
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
    }

    protected void CreateSupportPack(
        BuildTargetId target,
        string executable
    ) {
        string directory = Path.Combine(m_root, "pack-candidate-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        string references = Path.Combine(directory, "References");
        Directory.CreateDirectory(references);
        foreach (string assembly in m_referenceAssemblies)
        {
            File.Copy(assembly, Path.Combine(references, Path.GetFileName(assembly)));
        }
        File.WriteAllBytes(Path.Combine(directory, executable), [0x49, 0x4E, 0x4E, 0x4F]);
        string link = Path.Combine(directory, "PlayerLink");
        Directory.CreateDirectory(link);
        string platform = target == new BuildTargetId("macos-arm64") ? "MacOSArm64" : "WindowsX64";
        File.WriteAllText(
            Path.Combine(link, "Player.csproj"),
            "<Project><PropertyGroup><AssemblyName>Inno.Player." + platform + "</AssemblyName></PropertyGroup></Project>"
        );
        foreach (string source in new[]
        {
            "Program.cs",
            platform + "PlayerComposition.cs",
            platform + "PlayerContentSource.cs",
            "global.json"
        }) {
            File.WriteAllText(Path.Combine(link, source), "fixture input");
        }
        string analyzers = Path.Combine(link, "Analyzers");
        Directory.CreateDirectory(analyzers);
        File.WriteAllText(Path.Combine(analyzers, "Inno.Runtime.Generators.dll"), "fixture analyzer");
        File.WriteAllText(Path.Combine(analyzers, "Inno.Core.Serialization.Generators.dll"), "fixture analyzer");
        string linkReferences = Path.Combine(link, "References");
        Directory.CreateDirectory(linkReferences);
        foreach (string source in Directory.EnumerateFiles(references, "*.dll"))
        {
            File.Copy(source, Path.Combine(linkReferences, Path.GetFileName(source)));
        }
        File.WriteAllText(Path.Combine(linkReferences, "Inno.Player.Runtime.dll"), "fixture runtime");
        File.WriteAllText(Path.Combine(linkReferences, "BGCS.Runtime.dll"), "fixture interop");
        File.WriteAllBytes(Path.Combine(link, executable), [0x49, 0x4E, 0x4E, 0x4F]);
        string native = Path.Combine(directory, "native");
        Directory.CreateDirectory(native);
        string[] required = target == new BuildTargetId("macos-arm64")
                            ? ["libbgfx-shared-lib-release.dylib", "SDL3-release.dylib", "libminiaudio-release.dylib",
                                "libinno-text-release.dylib", "libinno-ui-release.dylib"]
                            : ["bgfx-shared-lib-release.dll", "SDL3-release.dll", "miniaudio-release.dll",
                                "inno-text-release.dll", "inno-ui-release.dll"];
        string linkNative = Path.Combine(link, "native");
        Directory.CreateDirectory(linkNative);
        foreach (string file in required)
        {
            File.WriteAllBytes(Path.Combine(native, file), CreateNativeImage(target));
            File.WriteAllBytes(Path.Combine(linkNative, file), CreateNativeImage(target));
        }
        IPlayerSupportPackValidator validator = target == new BuildTargetId("macos-arm64")
                                                ? new MacOSArm64SupportPackValidator()
                                                : new WindowsX64SupportPackValidator();
        using PlayerSupportPackLease published = new PlayerSupportPackCatalog(m_supportPackRoot).PublishAsync(
            target,
            directory,
            validator
        ).AsTask().GetAwaiter().GetResult();
    }

    protected string GetSupportPackDirectory(BuildTargetId target)
    {
        string targetRoot = Path.Combine(m_supportPackRoot, target.value);
        return Path.Combine(targetRoot, File.ReadAllText(Path.Combine(targetRoot, "current")));
    }

    protected static byte[] CreateNativeImage(BuildTargetId target)
    {
        using var output = new MemoryStream();
        using var writer = new BinaryWriter(output);
        if (target == new BuildTargetId("macos-arm64"))
        {
            writer.Write(0xFEED_FACFu);
            writer.Write(0x0100_000Cu);
            writer.Write(0u);
            writer.Write(6u);
            writer.Write(new byte[16]);
        }
        else
        {
            writer.Write((ushort)0x5A4D);
            writer.Write(new byte[58]);
            writer.Write(64);
            writer.Write(0x0000_4550u);
            writer.Write((ushort)0x8664);
            writer.Write(new byte[16]);
            writer.Write((ushort)0x2000);
            writer.Write((ushort)0x020B);
        }
        return output.ToArray();
    }

    protected static bool IsAuthoringAssembly(string name)
        => name.Contains("Inno.Editor", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Build", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Scripting.Compiler", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Assets.Pipeline", StringComparison.OrdinalIgnoreCase)
           || name.Contains("Inno.Plugins.Authoring", StringComparison.OrdinalIgnoreCase);
}
