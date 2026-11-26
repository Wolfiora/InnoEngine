using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Inno.Testing.Processes;

namespace Inno.Testing.EditorJourney;

internal static class Program
{
    private static async Task<int> Main(string[] arguments)
    {
        try
        {
            if (arguments.Length == 0 || arguments[0] is not ("smoke" or "Workflows" or "Lifecycle"))
            {
                throw new ArgumentException("Expected smoke, Workflows or Lifecycle.");
            }
            var options = new Dictionary<string, string>();
            for (int index = 1; index < arguments.Length; index += 2)
            {
                if (index + 1 >= arguments.Length || !options.TryAdd(arguments[index], arguments[index + 1]))
                {
                    throw new ArgumentException($"Incomplete or duplicated option: {arguments[index]}");
                }
            }
            await RunAsync(arguments[0], options);
            return 0;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
    }

    private static async Task RunAsync(
        string scenario,
        Dictionary<string, string> options
    ) {
        bool smoke = scenario == "smoke";
        int frames = int.Parse(options.GetValueOrDefault("--frames", smoke ? "3" : "30000"), CultureInfo.InvariantCulture);
        int timeout = int.Parse(options.GetValueOrDefault("--timeout-seconds", smoke ? "180" : "600"), CultureInfo.InvariantCulture);
        if (frames < (smoke ? 1 : 120) || timeout < 1)
        {
            throw new ArgumentException("Invalid frame limit or timeout.");
        }
        string root = Path.GetFullPath(options.GetValueOrDefault("--engine-root", Directory.GetCurrentDirectory()));
        string editor = Path.GetFullPath(options["--editor"]);
        if (!File.Exists(editor))
        {
            throw new FileNotFoundException("Build the selected Editor before running its product checks.", editor);
        }
        string results = Path.GetFullPath(options["--output"]);
        if (!smoke && Directory.Exists(results))
        {
            throw new IOException($"Use a fresh journey output directory: {results}");
        }
        string project = smoke ? results : Path.Combine(results, "Project");
        Directory.CreateDirectory(Path.Combine(project, "Assets"));
        string logs = smoke ? Path.Combine(project, "Logs") : results;
        Directory.CreateDirectory(logs);
        string bootPath = Path.Combine(project, "Logs", "EditorBoot.log");
        if (File.Exists(bootPath))
        {
            File.Delete(bootPath);
        }
        if (!smoke)
        {
            PrepareProject(root, project, results, scenario);
        }
        var start = new ProcessStartInfo(options.GetValueOrDefault("--dotnet", "dotnet"))
        {
            UseShellExecute = false,
            WorkingDirectory = project,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (string value in new[] { editor, project, "--smoke-frames", frames.ToString(CultureInfo.InvariantCulture) })
        {
            start.ArgumentList.Add(value);
        }
        if (!smoke || options.ContainsKey("--window-visible"))
        {
            start.ArgumentList.Add("--window-visible");
            start.ArgumentList.Add(smoke ? options["--window-visible"] : "false");
        }
        foreach (string key in new[] { "--graphics-api", "--native-dependencies" })
        {
            if (options.TryGetValue(key, out string? value))
            {
                start.ArgumentList.Add(key);
                start.ArgumentList.Add(key == "--native-dependencies" ? Path.GetFullPath(value) : value);
            }
        }
        string stdout = Path.Combine(logs, smoke ? "NativeRendering.stdout.log" : "stdout.log");
        string stderr = Path.Combine(logs, smoke ? "NativeRendering.stderr.log" : "stderr.log");
        string diagnostics = Path.Combine(logs, "NativeRendering.log");
        Stopwatch elapsed = Stopwatch.StartNew();
        bool requested = false;
        string? failure = null;
        int? exitCode = null;
        try
        {
            using Process child = Process.Start(start) ?? throw new IOException("Editor did not start.");
            using (FileStream output = File.Create(stdout))
            using (FileStream errors = File.Create(stderr))
            {
                Task drain = Task.WhenAll(
                    child.StandardOutput.BaseStream.CopyToAsync(output),
                    child.StandardError.BaseStream.CopyToAsync(errors)
                );
                try
                {
                    while (!child.WaitForExit(100))
                    {
                        if (elapsed.Elapsed >= TimeSpan.FromSeconds(timeout))
                        {
                            if (OperatingSystem.IsMacOS() && smoke)
                            {
                                await SystemCommandAsync(
                                    "/usr/bin/sample",
                                    [child.Id.ToString(), "2", "-file", Path.Combine(logs, "EditorProcess.sample.txt")],
                                    root
                                );
                            }
                            throw new TimeoutException($"The owned Editor exceeded its deadline of {timeout} seconds (PID {child.Id}).");
                        }
                        if (!smoke && !requested)
                        {
                            requested = await CheckCompletionAsync(child, project, scenario);
                        }
                    }
                    exitCode = child.ExitCode;
                }
                finally
                {
                    NativeProcessTestHost.TerminateIfRunning(child);
                    await drain.WaitAsync(TimeSpan.FromSeconds(10));
                }
            }
            if (exitCode != 0)
            {
                throw new IOException($"Editor exited with code {exitCode}.");
            }
            string boot = File.ReadAllText(bootPath);
            if (!boot.Contains("Dispose end.") || boot.Contains("Teardown failure"))
            {
                throw new InvalidDataException("The completed journey did not retire its real product resources successfully.");
            }
            if (smoke)
            {
                if (!Regex.IsMatch(boot, options["--backend-pattern"], RegexOptions.IgnoreCase)
                    || !boot.Contains($"Smoke frame limit reached after {frames} frame(s).")
                    || !boot.Contains("Primary presentation encodes sRGB: True.")
                    || File.ReadLines(stdout)
                        .Concat(File.ReadLines(stderr))
                        .Any(line => line.Contains("BGFX FATAL", StringComparison.OrdinalIgnoreCase))
                ) {
                    throw new InvalidDataException(
                        "Renderer smoke is missing its required backend, frames or sRGB evidence, or reported a BGFX fatal diagnostic."
                    );
                }
            }
            else
            {
                AssertJourney(project, scenario);
            }
        }
        catch (Exception error)
        {
            failure = error.Message;
            throw;
        }
        finally
        {
            if (smoke && File.Exists(stdout) && File.Exists(stderr))
            {
                File.WriteAllText(diagnostics, File.ReadAllText(stdout) + File.ReadAllText(stderr));
            }
            if (!smoke)
            {
                var report = new
                {
                    revision = (await SystemCommandAsync("git", ["rev-parse", "HEAD"], root)).Trim(),
                    workingChanges = (await SystemCommandAsync("git", ["status", "--porcelain=v1"], root)).Split(
                        '\n',
                        StringSplitOptions.RemoveEmptyEntries
                    ),
                    editor,
                    graphicsApi = options["--graphics-api"],
                    frameLimit = frames,
                    exitCode,
                    scenario,
                    exitTrigger = requested ? "completed-journey" : "frame-limit",
                    seconds = elapsed.Elapsed.TotalSeconds,
                    cycles = scenario == "Lifecycle" ? (int?)1 : null,
                    status = failure == null ? "Passed" : "Failed",
                    diagnostic = failure ?? "Completed the product journey and orderly teardown.",
                    limitation = "Public feature commands and actual panel drawing; physical input and devices require separate acceptance."
                };
                File.WriteAllText(Path.Combine(results, "result.json"), JsonSerializer.Serialize(report));
            }
        }
    }

    private static void PrepareProject(
        string root,
        string project,
        string results,
        string scenario
    ) {
        File.Copy(
            Path.Combine(root, "src/composition/editor/hosting/Inno.Editor.Hosting/Layout/DefaultEditorLayout.ini"),
            Path.Combine(project, "editor.ini")
        );
        string directory = Directory.CreateDirectory(Path.Combine(project, "Assets/Acceptance")).FullName;
        string[] files = scenario == "Lifecycle" ? ["PanelLifecycle.editor.cs"] : ["WorkflowState.cs.txt", "WorkflowJourney.editor.cs.txt"];
        foreach (string file in files)
        {
            string destination = file.EndsWith(".txt", StringComparison.Ordinal) ? file[..^4] : file;
            File.Copy(Path.Combine(root, "tests/product/fixtures/editor-ui", file), Path.Combine(directory, destination));
        }
        var inputs = Directory.EnumerateFiles(project, "*", SearchOption.AllDirectories).Select(file => new
        {
            path = Path.GetRelativePath(project, file),
            sha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(file)))
        });
        File.WriteAllText(Path.Combine(results, "inputs.json"), JsonSerializer.Serialize(inputs));
    }

    private static async Task<bool> CheckCompletionAsync(
        Process child,
        string project,
        string scenario
    ) {
        string evidence = Path.Combine(project, "Library/Acceptance");
        string failure = Path.Combine(evidence, "workflow-failure.txt");
        if (File.Exists(failure))
        {
            throw new InvalidDataException("The source extension failed: " + File.ReadAllText(failure));
        }
        string signal = Path.Combine(evidence, "journey-completed.txt");
        if (!File.Exists(signal))
        {
            return false;
        }
        string completed = File.ReadAllText(signal);
        if (completed.Length == 0)
        {
            return false;
        }
        if (completed != scenario)
        {
            throw new InvalidDataException("Completion belongs to a different journey.");
        }
        AssertJourney(project, scenario);
        if (child.HasExited)
        {
            return false;
        }
        if (OperatingSystem.IsWindows())
        {
            return EditorJourneyExit.Request(child.Id);
        }
        if (OperatingSystem.IsMacOS())
        {
            const string C_QUIT = """
                use framework "AppKit"
                on run arguments
                    set app to current application's NSRunningApplication's runningApplicationWithProcessIdentifier:((item 1 of arguments) as integer)
                    if app is missing value then error "Editor process missing"
                    if not ((app's terminate()) as boolean) then error "Editor refused quit"
                end run
                """;
            try
            {
                await SystemCommandAsync("/usr/bin/osascript", ["-e", C_QUIT, "--", child.Id.ToString()], project);
                return true;
            }
            catch (IOException) when (child.HasExited)
            {
                return false;
            }
        }
        throw new PlatformNotSupportedException("The host has no Editor quit transport.");
    }

    private static void AssertJourney(
        string project,
        string scenario
    ) {
        string directory = Path.Combine(project, "Library/Acceptance");
        string file = Path.Combine(directory, scenario == "Workflows" ? "workflow-journey.tsv" : "panel-lifecycle.tsv");
        if (!File.Exists(file))
        {
            throw new InvalidDataException("The source extension did not complete the journey.");
        }
        if (scenario == "Workflows")
        {
            if (File.ReadAllLines(Path.Combine(directory, "workflow-journey.tsv")).Length != 8)
            {
                throw new InvalidDataException("Expected eight completed workflow stages.");
            }
            return;
        }
        string[] lines = File.ReadAllLines(Path.Combine(directory, "panel-lifecycle.tsv"));
        string[][] panels = lines.Skip(1).Select(line => line.Split('\t')).Where(values => values[0] != "protocol").ToArray();
        if (lines.FirstOrDefault() != "panel\tcompleted-open-close-cycles"
            || panels.Length < 8
            || panels.Any(values => values.Length != 2 || values[1] != "1")
        ) {
            throw new InvalidDataException("Every product panel must complete one open/close cycle.");
        }
    }

    private static async Task<string> SystemCommandAsync(
        string program,
        string[] arguments,
        string directory
    ) {
        var start = new ProcessStartInfo(program) { UseShellExecute = false, WorkingDirectory = directory, RedirectStandardOutput = true };
        foreach (string value in arguments)
        {
            start.ArgumentList.Add(value);
        }
        using Process child = Process.Start(start) ?? throw new IOException($"Cannot start {program}.");
        Task<string> output = child.StandardOutput.ReadToEndAsync();
        try
        {
            await child.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(15));
            return child.ExitCode == 0 ? await output : throw new IOException($"{program} exited with {child.ExitCode}.");
        }
        finally
        {
            NativeProcessTestHost.TerminateIfRunning(child);
        }
    }
}
