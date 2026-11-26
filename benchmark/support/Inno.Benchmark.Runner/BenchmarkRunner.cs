using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Filters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Inno.Benchmark.Runner;

public static class BenchmarkRunner
{
    public static int Run(
        Assembly assembly,
        string[] args,
        IReadOnlyDictionary<string, Action<ProbeReport>>? probes = null,
        Func<bool>? available = null
    ) {
        try
        {
            return Dispatch(assembly, args, probes ?? new Dictionary<string, Action<ProbeReport>>(), available);
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
    }

    private static string CaseID(BenchmarkCase item)
        => item.Descriptor.Type.Name + "." + item.Descriptor.WorkloadMethod.Name
           + string.Concat(item.Parameters.Items.Select(p => FormattableString.Invariant($"/{p.Name}={p.Value}")));

    private static int Dispatch(
        Assembly assembly,
        string[] args,
        IReadOnlyDictionary<string, Action<ProbeReport>> probes,
        Func<bool>? available
    ) {
        if (args is ["--probe", string probe, string output])
        {
            if (!probes.TryGetValue(probe, out var measure))
            {
                Console.Error.WriteLine("Unknown probe: " + probe);
                return 2;
            }
            BenchmarkEnvironment.Write(assembly, Path.GetFullPath(output) + ".json", args);
            var result = new ProbeReport();
            measure(result);
            result.Write(output);
            return 0;
        }
        if (args is ["--probes"] or ["--probes", _])
        {
            if (probes.Count == 0) return 2;
            string report = Path.GetFullPath(args.Length == 2 ? args[1] : "probes.md");
            foreach (string name in probes.Keys)
            {
                var start = new ProcessStartInfo(Environment.ProcessPath!) { UseShellExecute = false };
                if (Path.GetFileNameWithoutExtension(start.FileName).Equals("dotnet", StringComparison.OrdinalIgnoreCase))
                {
                    start.ArgumentList.Add(assembly.Location);
                }
                string part = report + "." + name + ".md";
                foreach (string argument in new[] { "--probe", name, part })
                {
                    start.ArgumentList.Add(argument);
                }
                using Process child = Process.Start(start)!;
                child.WaitForExit();
                if (child.ExitCode != 0) return child.ExitCode;
            }
            File.WriteAllText(
                report,
                string.Join(
                    Environment.NewLine,
                    probes.Keys.Select(name => File.ReadAllText(report + "." + name + ".md"))
                )
            );
            return 0;
        }
        Type[] types = assembly.GetExportedTypes().Where(type =>
            type.GetMethods().Any(method => method.IsDefined(typeof(BenchmarkAttribute)))).ToArray();
        string[] cases = types.SelectMany(type => BenchmarkConverter.TypeToBenchmarks(type).BenchmarksCases)
            .Select(CaseID)
            .Order(StringComparer.Ordinal)
            .ToArray();
        if (cases.Distinct(StringComparer.Ordinal).Count() != cases.Length)
        {
            throw new InvalidOperationException("Duplicate benchmark case ID.");
        }
        if (args is [] or ["--list"])
        {
            foreach (string id in cases)
            {
                Console.WriteLine(id);
            }
            foreach (string name in probes.Keys)
            {
                Console.WriteLine($"probe/{name}");
            }
            return 0;
        }
        if (args.Contains("--list") || args.Contains("--help") || args.Contains("--info"))
        {
            if (args is not (["--list", "flat" or "tree"] or ["--help"] or ["--info"])) return 2;
            BenchmarkSwitcher.FromTypes(types).Run(args);
            return 0;
        }
        ManualConfig config = ManualConfig.Create(DefaultConfig.Instance).AddExporter(JsonExporter.Full);
        if (args[0] is "--case" or "--check-case")
        {
            if (args[0] == "--check-case")
            {
                HashSet<string> inventory = cases.Concat(probes.Keys.Select(name => "probe/" + name)).ToHashSet(StringComparer.Ordinal);
                if (args.Length == 1) return 2;
                foreach (string requested in args.Skip(1))
                {
                    if (!inventory.Contains(requested))
                    {
                        Console.Error.WriteLine("Unknown case: " + requested);
                        return 2;
                    }
                }
                return 0;
            }
            if (args.Length < 2 || !cases.Contains(args[1], StringComparer.Ordinal))
            {
                Console.Error.WriteLine("Unknown case. Use --list to see exact IDs.");
                return 2;
            }
            string id = args[1];
            if (available?.Invoke() == false) return 3;
            config.AddFilter(new SimpleFilter(item => CaseID(item) == id));
            args = ["--filter", "*", .. args.Skip(2)];
        }
        int pathIndex = Array.IndexOf(args, "--artifacts");
        if (pathIndex == args.Length - 1) throw new ArgumentException("Expected artifacts path.");
        string artifacts = pathIndex >= 0
                           ? args[pathIndex + 1]
                           : Path.Combine("artifacts", "benchmarks", Guid.NewGuid().ToString("N"));
        if (pathIndex < 0) args = [.. args, "--artifacts", artifacts];
        BenchmarkEnvironment.Write(assembly, Path.Combine(artifacts, "environment.json"), Environment.GetCommandLineArgs()[1..]);
        Summary[] summaries = BenchmarkSwitcher.FromTypes(types).Run(args, config).ToArray();
        return summaries.Length > 0 && summaries.All(summary => (
            !summary.HasCriticalValidationErrors && summary.Reports.Any()
            && summary.Reports.All(report => report.Success && report.ResultStatistics is not null)
        )) ? 0 : 1;
    }
}
