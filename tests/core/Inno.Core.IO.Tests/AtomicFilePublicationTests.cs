using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;

namespace Inno.Core.IO.Tests;

public sealed class AtomicFilePublicationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoAtomicPublication", Guid.NewGuid().ToString("N"));

    public AtomicFilePublicationTests() => Directory.CreateDirectory(m_root);

    [Theory]
    [InlineData("document.imeta.staging-0123456789abcdef0123456789ABCDEF", true)]
    [InlineData("document.staging-0123456789abcdef0123456789abcdef.more", false)]
    [InlineData("document.staging-0123456789abcdef0123456789abcdeg", false)]
    [InlineData("document.staging-", false)]
    [InlineData(".staging-0123456789abcdef0123456789abcdef", false)]
    [InlineData("ordinary.imeta", false)]
    public void PublicationRecognitionRequiresTheCompleteReservedName(
        string name,
        bool expected
    ) {
        Assert.Equal(expected, AtomicFile.IsCandidatePath(name));
        Assert.Equal(expected, AtomicFile.IsCandidatePath(Path.Combine(m_root, name)));
    }

    [Theory]
    [InlineData("")]
    [InlineData("external document")]
    public void CreateOnlyPreservesAnExistingFileAndTheLosingCandidate(string document)
    {
        string destination = Path.Combine(m_root, "document.ini");
        string candidate = Path.Combine(m_root, "candidate");
        File.WriteAllText(destination, document);
        File.WriteAllText(candidate, "candidate");
        byte[] original = File.ReadAllBytes(destination);

        Assert.ThrowsAny<IOException>(() => AtomicFile.Install(candidate, destination, overwrite: false));
        Assert.Equal(original, File.ReadAllBytes(destination));
        Assert.Equal("candidate", File.ReadAllText(candidate));
        Assert.ThrowsAny<IOException>(() => AtomicFile.WriteAllBytes(destination, "replacement"u8, overwrite: false));
        Assert.Equal(original, File.ReadAllBytes(destination));
        Assert.Equal(2, Directory.GetFiles(m_root).Length);
    }

    [Fact]
    public void CreateOnlyRejectsAnExistingDirectoryWithoutConsumingTheCandidate()
    {
        string destination = Path.Combine(m_root, "directory");
        string candidate = Path.Combine(m_root, "candidate");
        Directory.CreateDirectory(destination);
        File.WriteAllText(candidate, "complete");
        Exception? failure = Record.Exception(() => AtomicFile.Install(candidate, destination, overwrite: false));
        Assert.True(failure is IOException or UnauthorizedAccessException, failure?.ToString());
        Assert.Equal("complete", File.ReadAllText(candidate));
        Assert.Empty(Directory.GetFileSystemEntries(destination));
    }

    [Fact]
    public async Task ConcurrentCandidatesPublishExactlyOneCompleteFile()
    {
        const int count = 16;
        for (int round = 0; round < 12; round++)
        {
            string destination = Path.Combine(m_root, "document-" + round);
            string[] candidates = Enumerable.Range(0, count).Select(index => destination + ".candidate-" + index).ToArray();
            foreach (string candidate in candidates)
                File.WriteAllBytes(candidate, Enumerable.Repeat((byte)Array.IndexOf(candidates, candidate), 64 * 1024).ToArray());
            int winner = -1;
            int successes = 0;
            using var ready = new CountdownEvent(count);
            using var start = new ManualResetEventSlim();
            Task[] writers = Enumerable.Range(0, count).Select(index => Task.Factory.StartNew(() =>
            {
                ready.Signal();
                start.Wait();
                try
                {
                    AtomicFile.Install(candidates[index], destination, overwrite: false);
                    Interlocked.Exchange(ref winner, index);
                    Interlocked.Increment(ref successes);
                }
                catch (IOException) when (File.Exists(destination))
                {
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)).ToArray();
            bool prepared = ready.Wait(TimeSpan.FromSeconds(20));
            start.Set();
            await Task.WhenAll(writers).WaitAsync(TimeSpan.FromSeconds(20));
            Assert.True(prepared, "All candidate publishers must reach the start barrier.");
            Assert.Equal(1, successes);
            byte[] bytes = File.ReadAllBytes(destination);
            Assert.Equal(64 * 1024, bytes.Length);
            Assert.All(bytes, value => Assert.Equal((byte)winner, value));
            for (int index = 0; index < count; index++)
                Assert.Equal(index != winner, File.Exists(candidates[index]));
        }
    }

    [Fact]
    public async Task IndependentCreatorsAndAReaderObserveOnlyTheCompleteWinningPayload()
    {
        string project = Path.Combine(m_root, "Publisher.csproj");
        new XDocument(new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
            new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"), new XElement("OutputType", "Exe"),
                new XElement("ImplicitUsings", "disable")),
            new XElement("ItemGroup", new XElement("Reference", new XAttribute("Include", "Inno.Core.IO"),
                new XElement("HintPath", typeof(AtomicFile).Assembly.Location))))).Save(project);
        File.WriteAllText(Path.Combine(m_root, "Program.cs"), """
            using System;
            using System.IO;
            using System.Threading;
            using Inno.Core.IO;
            byte[] payload = new byte[1024 * 1024];
            Array.Fill(payload, byte.Parse(args[2]));
            File.WriteAllText(args[1] + ".ready-" + args[2], "ready");
            long deadline = Environment.TickCount64 + 20000;
            while (!File.Exists(args[1]))
            {
                if (Environment.TickCount64 > deadline) return 2;
                Thread.Sleep(1);
            }
            try { AtomicFile.WriteAllBytes(args[0], payload, overwrite: false); return 0; }
            catch (IOException) when (File.Exists(args[0])) { return 17; }
            """);
        using (Process build = Start("build", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "-v:quiet"))
        {
            Task<string> output = build.StandardOutput.ReadToEndAsync();
            Task<string> error = build.StandardError.ReadToEndAsync();
            try
            {
                await build.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(2));
            }
            finally
            {
                if (!build.HasExited)
                {
                    build.Kill(entireProcessTree: true);
                    await build.WaitForExitAsync();
                }
            }
            Assert.True(build.ExitCode == 0, await output + await error);
        }
        string destination = Path.Combine(m_root, "concurrent.ini");
        string gate = Path.Combine(m_root, "start");
        string assembly = Path.Combine(m_root, "bin", "Debug", "net10.0", "Publisher.dll");
        Process[] creators = Enumerable.Range(1, 8).Select(index => Start(assembly, destination, gate, index.ToString())).ToArray();
        try
        {
            await WaitUntil(() => Directory.GetFiles(m_root, "start.ready-*").Length == creators.Length);
            File.WriteAllText(gate, "start");
            await WaitUntil(() => File.Exists(destination));
            byte[] winner = File.ReadAllBytes(destination);
            Assert.Equal(1024 * 1024, winner.Length);
            Assert.InRange(winner[0], (byte)1, (byte)8);
            Assert.All(winner, value => Assert.Equal(winner[0], value));
            for (int read = 0; read < 16; read++)
                Assert.Equal(winner, File.ReadAllBytes(destination));
            await Task.WhenAll(creators.Select(process => process.WaitForExitAsync())).WaitAsync(TimeSpan.FromSeconds(30));
            Assert.Single(creators.Where(process => process.ExitCode == 0));
            Assert.All(creators, process => Assert.True(process.ExitCode is 0 or 17));
            Assert.Empty(Directory.GetFiles(m_root, "*.staging-*"));
        }
        finally
        {
            foreach (Process process in creators)
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
                process.Dispose();
            }
        }
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    private Process Start(params string[] arguments)
    {
        string sdk = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var start = new ProcessStartInfo(sdk)
        {
            WorkingDirectory = m_root,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (string argument in arguments)
            start.ArgumentList.Add(argument);
        return Process.Start(start) ?? throw new InvalidOperationException("The SDK fixture did not start.");
    }

    private static async Task WaitUntil(Func<bool> condition)
    {
        long deadline = Environment.TickCount64 + 20000;
        while (!condition())
        {
            Assert.True(Environment.TickCount64 < deadline, "The independent publishers did not reach their owned barrier.");
            await Task.Delay(5);
        }
    }
}
