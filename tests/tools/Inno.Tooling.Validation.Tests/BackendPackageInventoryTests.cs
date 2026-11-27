using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class BackendPackageInventoryTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task PlatformArchiveContainsEveryRequestedPayload(bool missing)
    {
        DirectoryInfo root = Directory.CreateTempSubdirectory("InnoPackageInventory-");
        try
        {
            const string C_OWNER = "backends/Fixture/Fixture.csproj";
            const string C_TARGET = "fixture-target";
            const string C_LIBRARY = "managed/tool/lib/tool.dll";
            Directory.CreateDirectory(Path.Combine(root.FullName, "receipts"));
            Directory.CreateDirectory(Path.Combine(root.FullName, "managed/tool/lib"));
            byte[] managed = [1, 2, 3];
            byte[] component = [4, 5, 6];
            string digest = Convert.ToHexStringLower(SHA256.HashData(component));
            string compiler = Convert.ToHexStringLower(SHA256.HashData(managed));
            File.WriteAllBytes(Path.Combine(root.FullName, C_LIBRARY), managed);
            File.WriteAllText(Path.Combine(root.FullName, "receipts/files.txt"), $"{C_OWNER}|Debug|{C_LIBRARY}|{compiler}");
            File.WriteAllBytes(Path.Combine(root.FullName, "tool.zip"), component);
            string plan = Write(
                "plan.json",
                new { target = C_TARGET, configuration = "Debug", projects = new Dictionary<string, string> { [C_OWNER] = compiler } }
            );
            string plans = Write("plan-list.json", new[] { plan });
            string selection = Write("selection.json", new
            {
                assets = new[]
                {
                    new
                    {
                        product = "compiler",
                        component = "tool",
                        target = C_TARGET,
                        configuration = "release",
                        asset = "tool.zip",
                        sha256 = digest,
                        fingerprint = compiler
                    }
                },
                aggregates = Array.Empty<object>(),
                requests = new[]
                {
                    new
                    {
                        target = C_TARGET,
                        product = "compiler",
                        configuration = "Debug",
                        digests = missing ? new[] { digest, compiler } : new[] { digest }
                    }
                }
            });
            string archive = Path.Combine(root.FullName, "platform.zip");
            Task<int> assemble = ValidationCommand.ExecuteAsync([
                "assemble-backends",
                "--artifacts-path",
                root.FullName,
                "--plans",
                plans,
                "--selection",
                selection,
                "--target",
                C_TARGET,
                "--output",
                archive
            ]);
            if (missing)
            {
                InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => assemble);
                Assert.Contains("missing payload", failure.Message);
                Assert.False(File.Exists(archive));
                return;
            }
            Assert.Equal(0, await assemble);
            using ZipArchive zip = ZipFile.OpenRead(archive);
            Assert.NotNull(zip.GetEntry("native/" + digest + ".zip"));
            Assert.NotNull(zip.GetEntry(C_LIBRARY));
            Assert.NotNull(zip.GetEntry("native/selection.json"));
        }
        finally
        {
            root.Delete(recursive: true);
        }

        string Write<T>(
            string name,
            T value
        ) {
            string path = Path.Combine(root.FullName, name);
            File.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(value));
            return path;
        }
    }
}
