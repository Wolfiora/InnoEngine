using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class DevelopmentBuildProvenanceTests
{
    [Theory]
    [InlineData("", false)]
    [InlineData("", true)]
    [InlineData("Debug", false)]
    [InlineData("Debug", true)]
    [InlineData("SourceDebug", false)]
    [InlineData("Release", false)]
    [InlineData("Release", true)]
    public async Task ReportOnlyCommitsPreserveDevelopmentOutputsAndReleaseKeepsExactProvenance(
        string configuration,
        bool continuousIntegration
    ) {
        string parent = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "InnoDevelopmentBuildTests"));
        string root = Path.Combine(parent, Guid.NewGuid().ToString("N"));
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT") ?? throw new InvalidOperationException("An explicit SDK host is required."),
                OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        DirectoryInfo? repository = new(Directory.GetCurrentDirectory());
        while (repository is not null && !File.Exists(Path.Combine(repository.FullName, "InnoEngine.sln")))
            repository = repository.Parent;
        Assert.NotNull(repository);
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "build", "msbuild"));
            File.Copy(Path.Combine(repository.FullName, "Directory.Build.props"), Path.Combine(root, "Directory.Build.props"));
            File.Copy(Path.Combine(repository.FullName, "Directory.Build.targets"), Path.Combine(root, "Directory.Build.targets"));
            File.Copy(Path.Combine(repository.FullName, "build", "msbuild", "Inno.Packages.props"), Path.Combine(root, "build", "msbuild", "Inno.Packages.props"));
            File.Copy(Path.Combine(repository.FullName, "build", "msbuild", "Inno.Build.Tasks.targets"), Path.Combine(root, "build", "msbuild", "Inno.Build.Tasks.targets"));
            File.Copy(Path.Combine(repository.FullName, "global.json"), Path.Combine(root, "global.json"));
            await File.WriteAllTextAsync(Path.Combine(root, ".gitignore"), "bin/\nobj/\n");
            await File.WriteAllTextAsync(Path.Combine(root, "Fixture.csproj"), """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework><DebugType>portable</DebugType></PropertyGroup>
                </Project>
                """);
            const string C_SOURCE = "namespace Fixture; public static class Contract { public static int Read() => 1; }\n";
            await File.WriteAllTextAsync(Path.Combine(root, "Contract.cs"), C_SOURCE);
            await RunAsync("git", "init", "-b", "master");
            await RunAsync("git", "remote", "add", "origin", "https://github.com/Wolfiora/InnoEngine.git");
            await CommitAsync("Initial source snapshot");
            await BuildAsync();
            string effectiveConfiguration = configuration.Length == 0 ? "Debug" : configuration;
            string assembly = Path.Combine(root, "bin", effectiveConfiguration, "net10.0", "Fixture.dll");
            string symbols = Path.ChangeExtension(assembly, ".pdb");
            byte[] beforeAssembly = SHA256.HashData(await File.ReadAllBytesAsync(assembly));
            byte[] beforeSymbols = SHA256.HashData(await File.ReadAllBytesAsync(symbols));
            DateTime beforeTimestamp = File.GetLastWriteTimeUtc(assembly);
            await File.WriteAllTextAsync(Path.Combine(root, "Report.md"), "# Validation report\nNo compiler input changed.\n");
            await CommitAsync("Record completed validation without source changes");
            string commit = (await RunAsync("git", "rev-parse", "HEAD")).Trim();
            await BuildAsync();
            byte[] afterAssembly = SHA256.HashData(await File.ReadAllBytesAsync(assembly));
            byte[] afterSymbols = SHA256.HashData(await File.ReadAllBytesAsync(symbols));
            if (effectiveConfiguration is "Debug" or "SourceDebug")
            {
                Assert.Equal(beforeAssembly, afterAssembly);
                Assert.Equal(beforeSymbols, afterSymbols);
                Assert.Equal(beforeTimestamp, File.GetLastWriteTimeUtc(assembly));
                Assert.Equal(C_SOURCE, ReadEmbeddedSource(symbols, "Contract.cs"));
                Assert.DoesNotContain(commit, await File.ReadAllTextAsync(Path.Combine(root, "obj", effectiveConfiguration, "net10.0", "Fixture.AssemblyInfo.cs")), StringComparison.Ordinal);
            }
            else
            {
                Assert.False(beforeAssembly.SequenceEqual(afterAssembly));
                Assert.False(beforeSymbols.SequenceEqual(afterSymbols));
                Assert.Contains(commit, await File.ReadAllTextAsync(Path.Combine(root, "obj", effectiveConfiguration, "net10.0", "Fixture.AssemblyInfo.cs")), StringComparison.Ordinal);
                Assert.Contains(commit, ReadSourceLink(symbols), StringComparison.Ordinal);
            }

            async Task CommitAsync(string message)
            {
                await RunAsync("git", "add", ".");
                await RunAsync("git", "-c", "user.name=Aaron Liao", "-c", "user.email=hsuankailiao@gmail.com", "commit", "-m", message);
            }

            Task<string> BuildAsync() => RunAsync(dotnet, ["build", "Fixture.csproj",
                ..configuration.Length == 0 ? Array.Empty<string>() : ["-c", configuration],
                "-p:ContinuousIntegrationBuild=" + continuousIntegration, "-m:1", "-nr:false", "--disable-build-servers"]);

            async Task<string> RunAsync(
                string executable,
                params string[] arguments
            ) {
                var start = new ProcessStartInfo(executable)
                {
                    WorkingDirectory = root, UseShellExecute = false, CreateNoWindow = true,
                    RedirectStandardOutput = true, RedirectStandardError = true
                };
                foreach (string argument in arguments)
                    start.ArgumentList.Add(argument);
                using Process process = Process.Start(start)!;
                Task<string> output = process.StandardOutput.ReadToEndAsync();
                Task<string> errors = process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                string text = await output + await errors;
                Assert.True(process.ExitCode == 0, text);
                return text;
            }
        }
        finally
        {
            if (!Path.GetFullPath(root).StartsWith(parent + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The development-build fixture escaped its temporary owner.");
            if (Directory.Exists(root))
            {
                foreach (string file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
                    File.SetAttributes(file, FileAttributes.Normal);
                Directory.Delete(root, recursive: true);
            }
        }
    }

    private static string ReadEmbeddedSource(
        string symbols,
        string fileName
    ) {
        using var stream = File.OpenRead(symbols);
        using MetadataReaderProvider provider = MetadataReaderProvider.FromPortablePdbStream(stream);
        MetadataReader reader = provider.GetMetadataReader();
        DocumentHandle document = Assert.Single(reader.Documents.Where(handle => reader.GetString(reader.GetDocument(handle).Name).EndsWith(fileName, StringComparison.Ordinal)));
        CustomDebugInformation source = Assert.Single(reader.GetCustomDebugInformation(document).Select(reader.GetCustomDebugInformation)
            .Where(info => reader.GetGuid(info.Kind) == new Guid("0E8A571B-6926-466E-B4AD-8AB04611F5FE")));
        byte[] data = reader.GetBlobBytes(source.Value);
        if (BitConverter.ToInt32(data, 0) == 0)
            return Encoding.UTF8.GetString(data, 4, data.Length - 4);
        using var compressed = new MemoryStream(data, 4, data.Length - 4, writable: false);
        using var decoder = new DeflateStream(compressed, CompressionMode.Decompress);
        using var decoded = new MemoryStream();
        decoder.CopyTo(decoded);
        Assert.Equal(BitConverter.ToInt32(data, 0), decoded.Length);
        return Encoding.UTF8.GetString(decoded.ToArray());
    }

    private static string ReadSourceLink(string symbols)
    {
        using var stream = File.OpenRead(symbols);
        using MetadataReaderProvider provider = MetadataReaderProvider.FromPortablePdbStream(stream);
        MetadataReader reader = provider.GetMetadataReader();
        CustomDebugInformation source = Assert.Single(reader.CustomDebugInformation.Select(reader.GetCustomDebugInformation)
            .Where(info => reader.GetGuid(info.Kind) == new Guid("CC110556-A091-4D38-9FEC-25AB9A351A6A")));
        return Encoding.UTF8.GetString(reader.GetBlobBytes(source.Value));
    }
}
