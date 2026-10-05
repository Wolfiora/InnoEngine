using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Inno.Build.Cli;

internal static partial class NativeBindingsVerification
{
    internal static async Task<int> RunAsync(
        string[] args,
        CancellationToken cancellationToken
    ) {
        if (args.Any(static argument => argument is "help" or "--help" or "-h"))
        {
            Console.WriteLine(NativeVerificationOptions.usage);
            return 0;
        }

        try
        {
            NativeVerificationOptions options = NativeVerificationOptions.Parse(args);
            string bindGenRoot = Path.GetFullPath(options.bindGenRoot);
            string repositoryRoot = options.engineRoot;
            IReadOnlyList<string> bindingProjects = DiscoverBindingProjects(repositoryRoot);
            string uiBridgeConfig = Path.Combine(
                repositoryRoot,
                "native",
                "Inno.Native.UI",
                "Bindings",
                "rmlui.bridge.json");
            string bindGenProject = Path.Combine(bindGenRoot, "src", "BGCS.Tool", "BGCS.Tool.csproj");
            string bindGenRuntimeProject = Path.Combine(
                bindGenRoot, "src", "BGCS.Runtime", "BGCS.Runtime.csproj");
            ValidateInputs(bindingProjects, uiBridgeConfig, bindGenProject, bindGenRuntimeProject);

            string target = DetectTarget();
            string nativeConfiguration = options.configuration.ToLowerInvariant();
            string cliOutput = Path.Combine(repositoryRoot, "artifacts", "build-tools", "verification", Guid.NewGuid().ToString("N"));
            Console.WriteLine($"[inno-bindings] Verify deterministic generation for {target}.");
            await VerifyCppBridgeAsync(
                repositoryRoot,
                options,
                bindGenProject,
                uiBridgeConfig, cancellationToken);
            foreach (string project in bindingProjects)
            {
                await ToolchainEnvironment.RunAsync(options.dotnet,
                    ["build", project,
                        "--configuration", options.configuration, "-t:CheckBindings",
                        $"-p:BindGenRoot={bindGenRoot}", $"-p:BindGenDotNetHost={options.dotnet}", "--nologo"],
                    repositoryRoot, cancellationToken);
            }

            AuditNativeImports(repositoryRoot);

            Console.WriteLine("[inno-bindings] Restore the complete InnoEngine solution.");
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "restore", Path.Combine(repositoryRoot, "InnoEngine.sln"),
                    $"-p:BGCSRuntimeProject={bindGenRuntimeProject}",
                    $"-p:InnoBuildCliOutputPath={cliOutput}",
                    "-p:BuildInParallel=false",
                    "/m:1",
                    "/nodeReuse:false",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken);

            var nativeProducts = await BuildNativeDependenciesAsync(repositoryRoot, nativeConfiguration, cancellationToken);

            Console.WriteLine("[inno-bindings] Build the complete InnoEngine solution.");
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "build", Path.Combine(repositoryRoot, "InnoEngine.sln"),
                    "--configuration", options.configuration,
                    "--no-restore",
                    "-m:1", "-nodeReuse:false", "--disable-build-servers",
                    $"-p:BGCSRuntimeProject={bindGenRuntimeProject}",
                    $"-p:InnoBuildCliOutputPath={cliOutput}",
                    "-p:TreatWarningsAsErrors=true",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken);

            IReadOnlyList<string> testProjects = DiscoverTestProjects(repositoryRoot);
            await RunTestsAsync(repositoryRoot, options, testProjects, nativeProducts, cancellationToken);
            string reportPath = await WriteReportAsync(
                repositoryRoot,
                bindGenRoot,
                target,
                bindingProjects,
                testProjects.Count, cancellationToken);
            Console.WriteLine(
                $"[inno-bindings] Project binding diffs, native builds, solution build, import audit, "
                + $"and {testProjects.Count} test projects passed.");
            Console.WriteLine($"[inno-bindings] Acceptance report: {reportPath}");
            return 0;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            Console.Error.WriteLine($"[inno-bindings] {exception.Message}");
            return 1;
        }
    }

    private static void ValidateInputs(
        IReadOnlyList<string> bindingProjects,
        string uiBridgeConfig,
        string bindGenProject,
        string bindGenRuntimeProject
    ) {
        foreach (string project in bindingProjects)
        {
            string config = Path.Combine(Path.GetDirectoryName(project)!, "Bindings", "bindgen.json");
            if (!File.Exists(config))
                throw new FileNotFoundException("Native binding configuration was not found.", config);
        }
        if (!File.Exists(uiBridgeConfig))
            throw new FileNotFoundException("RmlUi Cpp2C bridge configuration was not found.", uiBridgeConfig);
        if (!File.Exists(bindGenProject))
            throw new FileNotFoundException("The BindGen-CS CLI project was not found.", bindGenProject);
        if (!File.Exists(bindGenRuntimeProject))
            throw new FileNotFoundException("The BindGen-CS runtime project was not found.", bindGenRuntimeProject);

    }

    private static IReadOnlyList<string> DiscoverBindingProjects(string repositoryRoot)
    {
        string nativeRoot = Path.Combine(repositoryRoot, "native");
        string[] projects = Directory.EnumerateDirectories(nativeRoot, "Inno.Native.*", SearchOption.TopDirectoryOnly)
            .Select(directory => Path.Combine(directory, Path.GetFileName(directory) + ".csproj"))
            .Where(File.Exists)
            .Where(project => XDocument.Load(project).Descendants("BindGenGeneratedBindings")
                .Any(property => string.Equals(property.Value.Trim(), "true", StringComparison.OrdinalIgnoreCase)))
            .Order(StringComparer.Ordinal)
            .ToArray();
        if (projects.Length == 0)
            throw new InvalidOperationException("No Native projects declare BindGenGeneratedBindings=true.");
        return projects;
    }

    private static async Task VerifyCppBridgeAsync(
        string repositoryRoot,
        NativeVerificationOptions options,
        string bindGenProject,
        string uiBridgeConfig,
        CancellationToken cancellationToken
    ) {
        using JsonDocument bridgeConfig = JsonDocument.Parse(File.ReadAllText(uiBridgeConfig));
        string outputPath = bridgeConfig.RootElement.GetProperty("outputPath").GetString()
            ?? throw new InvalidOperationException("RmlUi Cpp2C outputPath must be a directory.");
        string outputRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(uiBridgeConfig)!, outputPath));
        IReadOnlyDictionary<string, string> before = SnapshotDirectory(outputRoot);
        await ToolchainEnvironment.RunAsync(
            options.dotnet,
            [
                "run",
                "--project", bindGenProject,
                "--configuration", options.configuration,
                "--",
                "bridge", uiBridgeConfig
            ],
            repositoryRoot, cancellationToken);
        IReadOnlyDictionary<string, string> after = SnapshotDirectory(outputRoot);
        if (!before.OrderBy(static pair => pair.Key, StringComparer.Ordinal)
            .SequenceEqual(after.OrderBy(static pair => pair.Key, StringComparer.Ordinal)))
        {
            throw new InvalidOperationException(
                "The checked-in RmlUi Cpp2C bridge is stale. Regenerate it with the current BindGen-CS tool.");
        }
        Console.WriteLine("[inno-bindings] RmlUi Cpp2C bridge is deterministic and current.");
    }

    private static IReadOnlyDictionary<string, string> SnapshotDirectory(string root)
    {
        if (!Directory.Exists(root))
            return new Dictionary<string, string>(StringComparer.Ordinal);
        return Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .ToDictionary(
                path => Path.GetRelativePath(root, path).Replace('\\', '/'),
                path => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))),
                StringComparer.Ordinal);
    }

    private static void AuditNativeImports(string repositoryRoot)
    {
        const int C_MAX_REPORTED_VIOLATIONS = 50;
        string nativeRoot = Path.Combine(repositoryRoot, "native");
        var violations = new List<string>();
        foreach (string path in Directory.EnumerateFiles(nativeRoot, "*.cs", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(repositoryRoot, path).Replace('\\', '/');
            if (relativePath.Contains("/Generated/", StringComparison.OrdinalIgnoreCase)
                || relativePath.Contains("/generated/", StringComparison.OrdinalIgnoreCase)
                || relativePath.Contains("/.bindgen-cache/", StringComparison.Ordinal)
                || relativePath.Contains("/bin/", StringComparison.OrdinalIgnoreCase)
                || relativePath.Contains("/obj/", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            int lineNumber = 0;
            foreach (string line in File.ReadLines(path))
            {
                lineNumber++;
                if (NativeImportPattern().IsMatch(line))
                    violations.Add($"{relativePath}:{lineNumber}:{line.Trim()}");
            }
        }

        if (violations.Count != 0)
        {
            string[] reported = violations.Take(C_MAX_REPORTED_VIOLATIONS).ToArray();
            string remainder = violations.Count > reported.Length
                ? $"{Environment.NewLine}... and {violations.Count - reported.Length} more violation(s)."
                : string.Empty;
            throw new InvalidOperationException(
                "Hand-authored native imports remain outside generated binding roots:"
                + Environment.NewLine
                + string.Join(Environment.NewLine, reported)
                + remainder);
        }

        Console.WriteLine("[inno-bindings] No hand-authored native imports exist outside generated binding roots.");
    }

    private static async Task<IReadOnlyList<NativeBuildProduct>> BuildNativeDependenciesAsync(
        string repositoryRoot,
        string nativeConfiguration,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine("[inno-bindings] Build every native dependency required by generated bindings.");
        return await HostNativeBuild.BuildEditorAsync(
            new NativeBuildContext(repositoryRoot, nativeConfiguration), cancellationToken);
    }

    private static IReadOnlyList<string> DiscoverTestProjects(string repositoryRoot)
    {
        string nativeTestsRoot = Path.Combine(repositoryRoot, "tests", "native");
        var projects = Directory.EnumerateFiles(nativeTestsRoot, "*.csproj", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .ToList();
        projects.Add(Path.Combine(repositoryRoot, "tests", "text", "Inno.Text.Tests", "Inno.Text.Tests.csproj"));
        projects.Add(Path.Combine(repositoryRoot, "tests", "ui", "Inno.UI.Tests", "Inno.UI.Tests.csproj"));
        if (projects.Count == 0)
            throw new InvalidOperationException("No native binding test projects were discovered.");
        if (projects.Any(static project => !File.Exists(project)))
        {
            throw new FileNotFoundException(
                "A required native binding test project is missing.",
                projects.First(static project => !File.Exists(project)));
        }
        return projects;
    }

    private static async Task RunTestsAsync(
        string repositoryRoot,
        NativeVerificationOptions options,
        IReadOnlyList<string> testProjects,
        IReadOnlyList<NativeBuildProduct> nativeProducts,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine("[inno-bindings] Run every native binding, Text, and UI test project.");
        foreach (string project in testProjects)
        {
            await HostNativeDeployment.InstallAsync(nativeProducts,
                Path.Combine(Path.GetDirectoryName(project)!, "bin", options.configuration, "net9.0"), cancellationToken)
                .ConfigureAwait(false);
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "test", project,
                    "--configuration", options.configuration,
                    "--no-build",
                    "--no-restore",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken);
        }
    }

    private static async Task<string> WriteReportAsync(
        string repositoryRoot,
        string bindGenRoot,
        string target,
        IReadOnlyList<string> bindingProjects,
        int testProjectCount,
        CancellationToken cancellationToken
    ) {
        string innoRevision = (await ToolchainEnvironment.CaptureOutputAsync(
            "git", ["-C", repositoryRoot, "rev-parse", "HEAD"], repositoryRoot, cancellationToken)).Trim();
        string bindGenRevision = (await ToolchainEnvironment.CaptureOutputAsync(
            "git", ["-C", bindGenRoot, "rev-parse", "HEAD"], bindGenRoot, cancellationToken)).Trim();
        bool innoDirty = !string.IsNullOrWhiteSpace(await ToolchainEnvironment.CaptureOutputAsync(
            "git", ["-C", repositoryRoot, "status", "--porcelain", "--untracked-files=all"], repositoryRoot, cancellationToken));
        bool bindGenDirty = !string.IsNullOrWhiteSpace(await ToolchainEnvironment.CaptureOutputAsync(
            "git", ["-C", bindGenRoot, "status", "--porcelain", "--untracked-files=all"], bindGenRoot, cancellationToken));
        DateTimeOffset generatedAt = DateTimeOffset.UtcNow;
        string reportDirectory = Path.Combine(
            repositoryRoot, "artifacts", "bindings", "acceptance", target);
        Directory.CreateDirectory(reportDirectory);

        var report = new
        {
            target,
            status = "passed",
            generatedAtUtc = generatedAt,
            source = new
            {
                innoEngine = new { revision = innoRevision, workingTreeDirty = innoDirty },
                bindGenCs = new { revision = bindGenRevision, workingTreeDirty = bindGenDirty }
            },
            bindingProjects = bindingProjects.Select(Path.GetFileNameWithoutExtension).ToArray(),
            nativeBuilds = new[] { "SDL3", "MiniAudio", "cimgui", "cimguizmo", "bgfx", "bgfx-tools", "inno-text", "inno-ui" },
            nativeTestProjects = testProjectCount,
            gates = new[]
            {
                "deterministic-project-binding-diff",
                "deterministic-cpp2c-bridge",
                "no-handwritten-native-imports",
                "native-dependency-builds",
                "full-solution-build",
                "all-native-binding-tests"
            }
        };
        string jsonPath = Path.Combine(reportDirectory, "report.json");
        File.WriteAllText(
            jsonPath,
            JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }) + Environment.NewLine,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

        string markdown = $"""
            # InnoEngine generated-native-binding acceptance

            - Target: `{target}`
            - Status: **passed**
            - Generated at: `{generatedAt:O}`
            - InnoEngine: `{innoRevision}` (working tree dirty: `{innoDirty.ToString().ToLowerInvariant()}`)
            - BindGen-CS: `{bindGenRevision}` (working tree dirty: `{bindGenDirty.ToString().ToLowerInvariant()}`)
            - Binding projects: {string.Join(", ", bindingProjects.Select(Path.GetFileNameWithoutExtension))}
            - Native builds: SDL3, MiniAudio, cimgui, cimguizmo, bgfx, bgfx tools, inno-text, inno-ui
            - Native test projects: {testProjectCount}

            Deterministic Cpp2C bridge and project binding diffs, handwritten-import audit, all native dependency builds, the complete
            InnoEngine solution build, and every native-binding test project passed.
            """;
        File.WriteAllText(
            Path.Combine(reportDirectory, "report.md"),
            markdown + Environment.NewLine,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        return jsonPath;
    }

    private static string DetectTarget()
    {
        string operatingSystem = OperatingSystem.IsWindows()
            ? "windows"
            : OperatingSystem.IsMacOS()
                ? "macos"
                : OperatingSystem.IsLinux()
                    ? "linux"
                    : throw new PlatformNotSupportedException("The current operating system is not supported.");
        string architecture = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.Arm64 => "arm64",
            _ => throw new PlatformNotSupportedException(
                $"Architecture '{RuntimeInformation.ProcessArchitecture}' is not supported.")
        };
        string abi = operatingSystem switch
        {
            "windows" => "msvc",
            "macos" => "darwin",
            "linux" => "gnu",
            _ => throw new UnreachableException()
        };
        return $"{operatingSystem}-{architecture}-{abi}";
    }

    [GeneratedRegex(@"\[(DllImport|LibraryImport)\b|partial\s+.*\bextern\b|static\s+extern\b")]
    private static partial Regex NativeImportPattern();

}
