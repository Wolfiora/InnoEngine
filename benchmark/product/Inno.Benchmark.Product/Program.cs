using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Benchmark.Product;

internal static class Program
{
    private static readonly string[] CASES = ["editor", "player-short", "player-long", "game-cold", "game-warm", "support-pack"];

    private static async Task<int> Main(string[] arguments)
    {
        try
        {
            if (arguments is [] or ["list"])
            {
                Console.WriteLine(string.Join(Environment.NewLine, CASES));
                return 0;
            }
            if (!CASES.Contains(arguments[0]))
            {
                throw new ArgumentException($"Unknown product scenario: {arguments[0]}");
            }
            if (!(OperatingSystem.IsWindows() || OperatingSystem.IsMacOS()))
            {
                Console.Error.WriteLine("Unavailable: product scenarios require a registered desktop host.");
                return 3;
            }
            return await MeasureAsync(arguments).ConfigureAwait(false);
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure.Message);
            return 1;
        }
    }

    private static async Task<int> MeasureAsync(string[] arguments)
    {
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 1; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (index + 1 >= arguments.Length || key is not ("--executable" or "--project" or "--input-revision" or "--target"
                or "--tools-target" or "--support-packs" or "--startup-scene" or "--graphics-api" or "--hardware" or "--expected-artifact"
                or "--artifacts" or "--dotnet" or "--timeout-seconds" or "--metrics-hook") || !options.TryAdd(key, arguments[index + 1])
            ) {
                throw new ArgumentException($"Unknown, incomplete or duplicate option: {key}");
            }
        }
        string Required(string key)
            => options.TryGetValue(key, out string? value) && !string.IsNullOrWhiteSpace(value)
               ? value
               : throw new ArgumentException($"Required option: {key}");
        string scenario = arguments[0];
        bool frame = scenario is "editor" or "player-short" or "player-long";
        string entry = Path.GetFullPath(Required("--executable"));
        string revision = Required("--input-revision");
        int timeout = int.Parse(options.GetValueOrDefault("--timeout-seconds", "3600"), CultureInfo.InvariantCulture);
        if (timeout is < 1 or > 86_400)
        {
            throw new ArgumentOutOfRangeException("--timeout-seconds");
        }
        string root = RepositoryRoot();
        string sdk = options.GetValueOrDefault("--dotnet", "dotnet");
        string run = Path.Combine(
            Path.GetFullPath(options.GetValueOrDefault("--artifacts", "artifacts/benchmarks/products")),
            $"{scenario}-{Guid.NewGuid():N}"
        );
        string output = Path.Combine(run, "product");
        int frames = scenario == "player-long" ? 6000 : 600;
        List<string> launch = [];
        string? expected = null;
        string hook = Path.GetFullPath(
            options.GetValueOrDefault("--metrics-hook", Path.Combine(AppContext.BaseDirectory, "Inno.Benchmark.ProcessMetrics.dll"))
        );
        if (frame)
        {
            Required("--hardware");
            if (!File.Exists(hook))
            {
                Console.Error.WriteLine($"Unavailable: missing process metrics hook {hook}");
                return 3;
            }
            if (scenario == "editor")
            {
                launch.Add(ExistingDirectory(Required("--project")));
            }
            launch.AddRange(
                [
                    "--graphics-api",
                    Required("--graphics-api"),
                    "--window-visible",
                    "true",
                    "--smoke-frames",
                    frames.ToString(CultureInfo.InvariantCulture)
                ]
            );
        }
        else
        {
            string relative = Required("--expected-artifact");
            expected = Path.GetFullPath(relative, output);
            if (Path.IsPathRooted(relative) || relative.Replace('\\', '/').Split('/').Any(piece => piece is "" or "." or ".."))
            {
                throw new ArgumentException("Expected artifact must be a relative file under product/.");
            }
            bool game = scenario != "support-pack";
            launch.Add(game ? "game" : "support-pack");
            launch.AddRange(["--tools-target", Required("--tools-target"), "--target", Required("--target")]);
            if (game)
            {
                launch.AddRange(
                    [
                        "--support-packs",
                        ExistingDirectory(Required("--support-packs")),
                        "--project",
                        ExistingDirectory(Required("--project")),
                        "--startup-scene",
                        Required("--startup-scene")
                    ]
                );
            }
            else
            {
                launch.AddRange(["--engine-root", root, "--configuration", "Release"]);
            }
            launch.AddRange(["--output", output]);
        }
        var report = new Dictionary<string, object?>
        {
            ["case"] = scenario,
            ["engineCommit"] = await IdentifyAsync("git", ["rev-parse", "HEAD"], root),
            ["inputRevision"] = revision,
            ["executable"] = entry,
            ["executableSha256"] = Hash(entry),
            ["arguments"] = launch,
            ["host"] = RuntimeInformation.OSDescription,
            ["architecture"] = RuntimeInformation.ProcessArchitecture.ToString(),
            ["sdk"] = await IdentifyAsync(sdk, ["--version"], root),
            ["graphicsApi"] = options.GetValueOrDefault("--graphics-api"),
            ["hardware"] = options.GetValueOrDefault("--hardware"),
            ["target"] = options.GetValueOrDefault("--target"),
            ["configuration"] = "Release",
            ["gpuMilliseconds"] = null
        };
        Directory.CreateDirectory(run);
        try
        {
            if (scenario == "game-warm")
            {
                await RunAsync("prepare");
                if (!File.Exists(expected))
                {
                    throw new IOException("Warm preparation did not produce its expected artifact.");
                }
            }
            Dictionary<string, object?> process = await RunAsync("measure");
            report["process"] = process;
            if (frame)
            {
                report["frames"] = frames;
                report["windowVisible"] = true;
                report["vsync"] = scenario == "editor" ? "editor configuration" : "enabled by Player";
                string log = File.ReadAllText(Path.Combine(run, "measure.stdout"));
                if (scenario == "editor")
                {
                    if (!log.Contains($"Smoke frame limit reached after {frames} frame(s).")
                        || !log.Contains("Dispose end.")
                        || log.Contains("Teardown failure")
                    ) {
                        throw new InvalidDataException("Editor frame or shutdown evidence missing.");
                    }
                }
                else
                {
                    Match? last = Regex
                        .Matches(
                            log,
                            @"^INNO-SMOKE frames=(\d+) views=(\d+) draws=(\d+) dispatches=(\d+)",
                            RegexOptions.Multiline
                        )
                        .LastOrDefault();
                    if (last == null || int.Parse(last.Groups[1].Value, CultureInfo.InvariantCulture) != frames)
                    {
                        throw new InvalidDataException("Player did not report the requested frame count.");
                    }
                    int Count(int group) => int.Parse(last.Groups[group].Value, CultureInfo.InvariantCulture);
                    report["finalFrame"] = new { views = Count(2), draws = Count(3), dispatches = Count(4) };
                }
                string metrics = Path.Combine(run, "managed-metrics.txt");
                if (!File.Exists(metrics))
                {
                    report["status"] = "unavailable";
                    report["reason"] = "The selected runtime did not execute the metrics hook.";
                    return 3;
                }
                Dictionary<string, string> managed = File.ReadLines(metrics).Select(line => line.Split('=', 2)).ToDictionary(pair => pair[0], pair => pair[1]);
                if (int.Parse(managed["pid"], CultureInfo.InvariantCulture) != (int)process["pid"]!)
                {
                    throw new InvalidDataException("Metrics belong to another process.");
                }
                report["managed"] = managed;
                process["cpuMilliseconds"] = double.Parse(managed["cpuMilliseconds"], CultureInfo.InvariantCulture);
            }
            else
            {
                report["artifactSha256"] = Hash(expected!);
            }
            report["status"] = "completed";
            return 0;
        }
        catch (Exception failure)
        {
            report["status"] = "failed";
            report["reason"] = failure.Message;
            throw;
        }
        finally
        {
            File.WriteAllText(
                Path.Combine(run, "result.json"),
                JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true })
            );
            Console.WriteLine($"Benchmark output: {run}");
        }

        async Task<Dictionary<string, object?>> RunAsync(string phase)
        {
            bool dll = string.Equals(Path.GetExtension(entry), ".dll", StringComparison.OrdinalIgnoreCase);
            var start = new ProcessStartInfo(dll ? sdk : entry)
            {
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = root
            };
            if (dll)
            {
                start.ArgumentList.Add(entry);
            }
            foreach (string argument in launch)
            {
                start.ArgumentList.Add(argument);
            }
            if (frame)
            {
                start.Environment.TryGetValue("DOTNET_STARTUP_HOOKS", out string? existing);
                start.Environment["DOTNET_STARTUP_HOOKS"] = string.IsNullOrEmpty(existing) ? hook : existing + Path.PathSeparator + hook;
                start.Environment["INNO_BENCHMARK_PROCESS_METRICS"] = Path.Combine(run, "managed-metrics.txt");
            }
            using FileStream stdout = File.Create(Path.Combine(run, phase + ".stdout"));
            using FileStream stderr = File.Create(Path.Combine(run, phase + ".stderr"));
            Stopwatch watch = Stopwatch.StartNew();
            using Process child = Process.Start(start) ?? throw new IOException("Cannot start product.");
            using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            Task streams = Task.WhenAll(
                child.StandardOutput.BaseStream.CopyToAsync(stdout),
                child.StandardError.BaseStream.CopyToAsync(stderr)
            );
            try
            {
                await child.WaitForExitAsync(deadline.Token);
                watch.Stop();
            }
            catch (OperationCanceledException)
            {
                if (!child.HasExited)
                {
                    child.Kill(entireProcessTree: true);
                }
                await child.WaitForExitAsync();
                throw new TimeoutException($"{phase} exceeded {timeout} seconds.");
            }
            finally
            {
                await streams.WaitAsync(TimeSpan.FromSeconds(10));
            }
            if (child.ExitCode != 0)
            {
                throw new IOException($"{phase} exited with {child.ExitCode}. See {run}.");
            }
            double? cpu = null;
            try
            {
                cpu = child.TotalProcessorTime.TotalMilliseconds;
            }
            catch (InvalidOperationException)
            {
                // Some hosts retire process accounting immediately on exit.
            }
            return new Dictionary<string, object?>
            {
                ["pid"] = child.Id,
                ["wallMilliseconds"] = watch.Elapsed.TotalMilliseconds,
                ["cpuMilliseconds"] = cpu
            };
        }
    }

    private static string ExistingDirectory(string path)
        => Directory.Exists(path) ? Path.GetFullPath(path) : throw new DirectoryNotFoundException(path);

    private static string Hash(string path)
    {
        using FileStream input = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(input));
    }

    private static string RepositoryRoot()
    {
        for (DirectoryInfo? directory = new(Directory.GetCurrentDirectory()); directory != null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "InnoEngine.sln")))
            {
                return directory.FullName;
            }
        }
        throw new DirectoryNotFoundException("Run from the InnoEngine checkout.");
    }

    private static async Task<string> IdentifyAsync(
        string executable,
        string[] arguments,
        string directory
    ) {
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = directory,
            RedirectStandardOutput = true,
            UseShellExecute = false
        };
        foreach (string argument in arguments)
        {
            start.ArgumentList.Add(argument);
        }
        using Process process = Process.Start(start) ?? throw new IOException($"Cannot start {executable}.");
        string value = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return process.ExitCode == 0 ? value.Trim() : throw new IOException($"Cannot identify {executable} input.");
    }
}
