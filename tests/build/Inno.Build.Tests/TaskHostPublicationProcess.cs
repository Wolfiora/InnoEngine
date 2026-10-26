using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.TaskHosting;
using Inno.Build.Toolchains;

namespace Inno.Build.Tests;

internal sealed class TaskHostPublicationProcess : IAsyncDisposable
{
    private readonly string m_release;
    private readonly Task m_running;
    private readonly CancellationTokenSource m_budget;
    private int m_disposed;

    private TaskHostPublicationProcess(
        string load,
        string publisher,
        string ready,
        string release,
        Task running,
        CancellationTokenSource budget
    ) {
        loadDirectory = load;
        publisherDirectory = publisher;
        readyFile = ready;
        m_release = release;
        m_running = running;
        m_budget = budget;
    }

    internal string loadDirectory { get; }
    internal string publisherDirectory { get; }
    internal string readyFile { get; }
    internal string publishedAssembly => File.ReadAllText(readyFile).Trim();

    internal static async Task<TaskHostPublicationProcess> StartAsync(string root)
    {
        string identity = Guid.NewGuid().ToString("N");
        string worker = Path.Combine(root, "workers", identity);
        Directory.CreateDirectory(worker);
        string load = Path.Combine(root, "loads", identity, "Debug");
        string publisher = Path.Combine(root, "publishers", identity, "Debug");
        Directory.CreateDirectory(publisher);
        string ready = Path.Combine(worker, "ready");
        string release = Path.Combine(worker, "release");
        string source = Path.Combine(root, "input", "Inno.Build.Tasks.dll");
        var project = new XDocument(new XElement("Project",
            new XElement("UsingTask", new XAttribute("TaskName", "Inno.Build.TaskHosting.PublishTaskHostTask"),
                new XAttribute("AssemblyFile", typeof(PublishTaskHostTask).Assembly.Location)),
            new XElement("UsingTask", new XAttribute("TaskName", "HoldPublication"),
                new XAttribute("TaskFactory", "RoslynCodeTaskFactory"),
                new XAttribute("AssemblyFile", "$(MSBuildToolsPath)/Microsoft.Build.Tasks.Core.dll"),
                new XElement("ParameterGroup", new XElement("ReleaseFile", new XAttribute("Required", "true"))),
                new XElement("Task", new XElement("Code", new XAttribute("Type", "Fragment"),
                    new XAttribute("Language", "cs"), new XCData("""
                        var elapsed = System.Diagnostics.Stopwatch.StartNew();
                        while (!System.IO.File.Exists(ReleaseFile))
                        {
                            if (elapsed.Elapsed > System.TimeSpan.FromMinutes(2))
                                throw new System.TimeoutException("The publication fixture was not released.");
                            System.Threading.Thread.Sleep(10);
                        }
                        """)))),
            new XElement("Target", new XAttribute("Name", "Publish"),
                new XElement("ItemGroup", new XElement("RuntimeFile", new XAttribute("Include", source),
                    new XElement("RelativePath", "Inno.Build.Tasks.dll"),
                    new XElement("FileHash", Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(source)))))),
                new XElement("Inno.Build.TaskHosting.PublishTaskHostTask",
                    new XAttribute("InputFiles", "@(RuntimeFile)"),
                    new XAttribute("CacheDirectory", Path.Combine(root, "hosts")),
                    new XAttribute("LoadDirectory", load), new XAttribute("PublisherDirectory", publisher),
                    new XElement("Output", new XAttribute("TaskParameter", "PublishedAssembly"),
                        new XAttribute("PropertyName", "PublishedRuntime"))),
                new XElement("WriteLinesToFile", new XAttribute("File", ready),
                    new XAttribute("Lines", "$(PublishedRuntime)"), new XAttribute("Overwrite", "true")),
                new XElement("HoldPublication", new XAttribute("ReleaseFile", release)))));
        string path = Path.Combine(worker, "Publication.proj");
        project.Save(path);
        string dotnet = ToolchainEnvironment.ResolveExecutable(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet");
        var budget = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        Task running = ToolchainEnvironment.RunAsync(dotnet, ["msbuild", path, "-t:Publish", "-nologo", "-m:1", "-nr:false"],
            worker, budget.Token, DotNetSdkEnvironment.Create(dotnet));
        var result = new TaskHostPublicationProcess(load, publisher, ready, release, running, budget);
        try
        {
            while (!File.Exists(ready))
            {
                if (running.IsCompleted)
                {
                    await running;
                    throw new InvalidOperationException("The publication process exited without declaring its runtime.");
                }
                await Task.Delay(20, budget.Token);
            }
            return result;
        }
        catch
        {
            budget.Cancel();
            try { await running; }
            catch (Exception) { }
            budget.Dispose();
            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref m_disposed, 1) != 0)
            return;
        File.WriteAllText(m_release, "release");
        try { await m_running; }
        finally { m_budget.Dispose(); }
    }
}
