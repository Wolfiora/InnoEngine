using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

/// <summary>
/// Executes a compiled VSTest union and gives each discovered process-isolated contract its own host.
/// </summary>
public static class ValidationTestExecution
{
    /// <summary>
    /// Runs the selected solution and records per-owner discovery, execution receipts and TRX files.
    /// </summary>
    public static async Task RunAsync(
        string root,
        string dotnet,
        string solution,
        string results,
        string sharedLog,
        IReadOnlyDictionary<string, string> assemblies,
        string logger,
        IReadOnlyDictionary<string, string> properties,
        string? nativeDependencies,
        CancellationToken cancellationToken
    ) {
        if (assemblies.Count == 0 || assemblies.Any(pair => !File.Exists(pair.Value)) || !File.Exists(logger))
        {
            throw new InvalidDataException("Execution requires compiled test assemblies and their VSTest discovery logger.");
        }
        Directory.CreateDirectory(results);
        string sdk = Path.GetDirectoryName(dotnet)!;
        var environment = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["PATH"] = sdk + Path.PathSeparator + Environment.GetEnvironmentVariable("PATH"),
            ["INNO_TESTING_REPOSITORY_ROOT"] = root,
            ["INNO_TESTING_ARTIFACTS_PATH"] = properties["ArtifactsPath"]
        };
        if (nativeDependencies != null)
        {
            environment.Add("INNO_TESTING_NATIVE_DEPENDENCIES", nativeDependencies);
        }
        string[] sharedEnvironment =
        [
            "--environment",
            "DOTNET_ROOT=" + sdk,
            ..environment.SelectMany(item => new[] { "--environment", item.Key + "=" + item.Value })
        ];
        environment.Add("DOTNET_HOST_PATH", dotnet);
        string[] assemblyEnvironment = environment.SelectMany(item => new[] { "--environment", item.Key + "=" + item.Value }).ToArray();
        string[] settings =
        [
            "--",
            "xUnit.DiagnosticMessages=true",
            "xUnit.LongRunningTestSeconds=60",
            "xUnit.MaxParallelThreads=" + Math.Min(Environment.ProcessorCount, 4)
        ];
        List<TestStage> stages = [];
        DateTimeOffset started = DateTimeOffset.UtcNow;
        long tick = Stopwatch.GetTimestamp();
        try
        {
            await ValidationProcess.RunAsync(dotnet, [
                "test",
                solution,
                "--configuration",
                properties["Configuration"],
                "--no-build",
                "--no-restore",
                "--disable-build-servers",
                "-m:1",
                "-nr:false",
                "--blame",
                "--filter",
                "ProcessIsolation!=Required",
                "--logger",
                "trx",
                "--logger",
                "console;verbosity=normal",
                "--results-directory",
                results,
                ..properties
                    .Where(pair => pair.Key is "ArtifactsPath" or "InnoNativeTarget" or "InnoToolTarget" or "InnoProductBuildProperties")
                    .Select(pair => "-p:" + pair.Key + "=" + pair.Value),
                ..sharedEnvironment,
                ..settings
            ], root, sharedLog, cancellationToken: cancellationToken).ConfigureAwait(false);
            Record("Shared", solution);
        }
        catch (Exception failure) when (failure is not OperationCanceledException)
        {
            Record("Shared", solution, error: failure.Message);
        }

        started = DateTimeOffset.UtcNow;
        tick = Stopwatch.GetTimestamp();
        var contracts = new Dictionary<string, string[]>(StringComparer.Ordinal);
        try
        {
            string runSettings = Path.Combine(results, "discovery.runsettings");
            new XDocument(
                new XElement(
                    "RunSettings",
                    new XElement(
                        "RunConfiguration",
                        new XElement(
                            "EnvironmentVariables",
                            environment.Select(item => new XElement(item.Key, item.Value))
                        )
                    )
                )
            ).Save(runSettings);
            string receipt = Path.Combine(results, "discovery-" + Guid.NewGuid().ToString("N") + ".json");
            await ValidationProcess.RunAsync(
                dotnet,
                [
                    "vstest",
                    ..assemblies.Values,
                    "--ListTests",
                    "--Settings:" + runSettings,
                    "--TestAdapterPath:" + Path.GetDirectoryName(logger),
                    "--Logger:InnoDiscovery;Receipt=" + receipt
                ],
                root,
                Path.Combine(results, "discovery.log"),
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);
            using JsonDocument discovery = JsonDocument.Parse(File.ReadAllBytes(receipt));
            Dictionary<string, string> owners = assemblies.ToDictionary(
                pair => Path.GetFullPath(pair.Value),
                pair => pair.Key,
                OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal
            );
            int count = 0;
            foreach (JsonElement item in discovery.RootElement.GetProperty("owners").EnumerateArray())
            {
                int discovered = item.GetProperty("count").GetInt32();
                if (discovered < 1)
                {
                    throw new InvalidDataException("Empty VSTest discovery owner.");
                }
                string owner = owners[Path.GetFullPath(item.GetProperty("source").GetString()!)];
                contracts.Add(owner, item.GetProperty("contracts").Deserialize<string[]>()!);
                count += discovered;
            }
            if (!discovery.RootElement.GetProperty("completed").GetBoolean()
                || count != discovery.RootElement.GetProperty("discovered").GetInt32()
            ) {
                throw new InvalidDataException("VSTest did not complete its discovery inventory. See " + receipt);
            }
        }
        catch (Exception failure) when (failure is not OperationCanceledException)
        {
            foreach (string owner in assemblies.Keys)
            {
                Record("Discovery", owner, owner, failure.Message);
            }
            throw;
        }
        foreach (string owner in assemblies.Keys)
        {
            if (contracts.TryGetValue(owner, out string[]? names))
            {
                Record("Discovery", owner, owner, names: names);
            }
            else
            {
                Record("Discovery", owner, owner, error: "The compiled owner discovered zero test cases.");
            }
        }
        foreach ((string owner, string[] names) in contracts)
        {
            foreach (string name in names)
            {
                started = DateTimeOffset.UtcNow;
                tick = Stopwatch.GetTimestamp();
                string id = "isolated-" + Guid.NewGuid().ToString("N");
                try
                {
                    await ValidationProcess.RunAsync(
                        dotnet,
                        [
                            "test",
                            assemblies[owner],
                            "--filter",
                            "(ProcessIsolation=Required)&(FullyQualifiedName=" + name + ")",
                            "--blame",
                            "--logger",
                            "trx;LogFileName=" + id + ".trx",
                            "--logger",
                            "console;verbosity=normal",
                            "--results-directory",
                            results,
                            ..assemblyEnvironment,
                            ..settings
                        ],
                        root,
                        Path.Combine(results, id + ".log"),
                        cancellationToken: cancellationToken
                    ).ConfigureAwait(false);
                    Verify(Path.Combine(results, id + ".trx"), name);
                    Record("Isolated", name, owner);
                }
                catch (Exception failure) when (failure is not OperationCanceledException)
                {
                    Record("Isolated", name, owner, failure.Message);
                }
            }
        }
        TestStage? first = stages.FirstOrDefault(stage => stage.status != "Passed");
        if (first != null)
        {
            throw new InvalidDataException(first.stage + ": " + first.diagnostic + " See " + results);
        }

        void Record(
            string stage,
            string identity,
            string owner = "",
            string? error = null,
            string[]? names = null
        ) {
            stages.Add(
                new TestStage(
                    stage,
                    stage == "Discovery"
                    ? Path.GetFullPath(identity, root)
                    : identity,
                    error == null ? "Passed" : "Failed",
                    error == null ? 0 : -1,
                    error ?? string.Empty,
                    owner.Length == 0 ? owner : Path.GetFullPath(owner, root),
                    names ?? [],
                    started,
                    Stopwatch.GetElapsedTime(tick).TotalSeconds
                )
            );
            AtomicFile.WriteAllBytes(Path.Combine(results, "execution-summary.json"), JsonSerializer.SerializeToUtf8Bytes(stages));
        }
    }

    /// <summary>
    /// Requires nonempty successful TRX evidence for the exact isolated contract.
    /// </summary>
    public static void Verify(
        string path,
        string name
    ) {
        XDocument document = XDocument.Load(path);
        IEnumerable<XElement> Elements(string local) => document.Descendants().Where(element => element.Name.LocalName == local);
        XElement counter = Elements("Counters").Single();
        int executed = (int?)counter.Attribute("executed") ?? 0;
        XElement[] records = Elements("UnitTestResult").ToArray();
        XElement[] methods = Elements("TestMethod").ToArray();
        if (executed < 1 || (int?)counter.Attribute("passed") != executed || (int?)counter.Attribute("failed") != 0
            || (int?)counter.Attribute("error") != 0
            || records.Length != executed
            || records.Any(item => (string?)item.Attribute("outcome") != "Passed")
            || methods.Length == 0
            || methods.Any(item => (string?)item.Attribute("className") + "." + (string?)item.Attribute("name") != name)
        ) {
            throw new InvalidDataException("Isolated execution did not certify " + name + ": " + path);
        }
    }

    private sealed record TestStage(
        string stage,
        string identity,
        string status,
        int exitCode,
        string diagnostic,
        string owner,
        string[] contracts,
        DateTimeOffset startedUtc,
        double seconds
    );
}
