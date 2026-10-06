using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Inno.Content.Testing;
using Xunit;

namespace Inno.Adapter.Content.FileSystem.Tests;

public sealed class ContentCacheProcessTests
{
    [Fact]
    public async Task IndependentProcessesPublishOneGenerationAndPinItUntilBothReadersRetire()
    {
        string root = Path.Combine(Path.GetTempPath(), "ContentProcesses-" + Guid.NewGuid().ToString("N"));
        string sourceRoot = Path.Combine(root, "source");
        string cacheRoot = Path.Combine(root, "cache");
        Directory.CreateDirectory(sourceRoot);
        File.WriteAllBytes(Path.Combine(sourceRoot, "pinned.bin"), [1, 2, 3]);
        File.WriteAllBytes(Path.Combine(sourceRoot, "repair.bin"), [4, 5, 6]);
        try
        {
            using ContentTestStore source = ContentTestStore.FromDirectory(sourceRoot);
            await using Probe first = Start(sourceRoot, cacheRoot);
            await using Probe second = Start(sourceRoot, cacheRoot);
            Assert.Equal("ready", await first.ReadLineAsync());
            Assert.Equal("ready", await second.ReadLineAsync());
            await first.process.StandardInput.WriteLineAsync("prepare");
            await second.process.StandardInput.WriteLineAsync("prepare");
            string generation = await first.ReadLineAsync();
            Assert.Equal(generation, await second.ReadLineAsync());
            string cache = Path.Combine(cacheRoot, "Content", source.descriptor.contentHash);
            string oldDirectory = Path.Combine(cache, "generations", generation);
            Assert.Single(Directory.GetDirectories(Path.Combine(cache, "generations")));

            File.WriteAllBytes(Path.Combine(oldDirectory, "repair.bin"), [6, 5, 4]);
            using (FileContentStore repaired = await FileContentPreparation.PrepareAsync(source,
                       new FileContentCacheOptions(cacheRoot)))
            {
                Assert.NotEqual(generation, File.ReadAllText(Path.Combine(cache, "current")));
                Assert.True(Directory.Exists(oldDirectory));
            }
            await first.ReleaseAsync();
            using (FileContentStore next = await FileContentPreparation.PrepareAsync(source,
                       new FileContentCacheOptions(cacheRoot)))
            {
                Assert.True(Directory.Exists(oldDirectory));
            }
            await second.ReleaseAsync();
            using FileContentStore final = await FileContentPreparation.PrepareAsync(source,
                new FileContentCacheOptions(cacheRoot));
            Assert.False(Directory.Exists(oldDirectory));
            Assert.Single(Directory.GetDirectories(Path.Combine(cache, "generations")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static Probe Start(
        string sourceRoot,
        string cacheRoot
    ) {
        var start = new ProcessStartInfo
        {
            FileName = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        start.ArgumentList.Add(Path.Combine(AppContext.BaseDirectory, "CacheProbe", "Inno.Content.CacheProbe.dll"));
        start.ArgumentList.Add(sourceRoot);
        start.ArgumentList.Add(cacheRoot);
        return new Probe(Process.Start(start) ?? throw new InvalidOperationException("The cache probe did not start."));
    }

    private sealed class Probe(Process process) : IAsyncDisposable
    {
        internal Process process { get; } = process;
        private readonly Task<string> m_error = process.StandardError.ReadToEndAsync();

        internal async Task<string> ReadLineAsync()
            => await process.StandardOutput.ReadLineAsync().WaitAsync(TimeSpan.FromSeconds(30))
                ?? throw new InvalidOperationException("The cache probe exited before reporting its state.");

        internal async Task ReleaseAsync()
        {
            await process.StandardInput.WriteLineAsync("release");
            Assert.Equal("010203", await ReadLineAsync());
            await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(30));
            Assert.True(process.ExitCode == 0, await m_error);
        }

        public async ValueTask DisposeAsync()
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
            }
            process.Dispose();
        }
    }
}
