using Inno.Build.Bindings;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Distribution.Standard;
using Inno.Build.Composition;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            IReadOnlyList<string> bindingProjects = DiscoverBindingProjects(repositoryRoot, options.target);
            string bindGenRuntimeProject = Path.Combine(
                bindGenRoot, "src", "BGCS.Runtime", "BGCS.Runtime.csproj");
            ValidateInputs(bindingProjects, bindGenRuntimeProject);

            string target = options.target;
            string nativeConfiguration = options.configuration.ToLowerInvariant();
            string cliOutput = Path.Combine(repositoryRoot, "artifacts", "build-tools", "verification", Guid.NewGuid().ToString("N"));
            Console.WriteLine($"[inno-bindings] Verify deterministic generation for {target}.");
            foreach (string project in bindingProjects)
            {
                await ToolchainEnvironment.RunAsync(options.dotnet,
                    ["build", project,
                        "--configuration", options.configuration, "-t:CheckBindings",
                        $"-p:BindGenRoot={bindGenRoot}", $"-p:BindGenDotNetHost={options.dotnet}",
                        $"-p:InnoNativeTarget={target}", $"-p:InnoToolTarget={target}", "--nologo"],
                    repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));
            }

            AuditNativeImports(repositoryRoot);

            Console.WriteLine("[inno-bindings] Restore the complete InnoEngine solution.");
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "restore", Path.Combine(repositoryRoot, "InnoEngine.sln"),
                    $"-p:BGCSRuntimeProject={bindGenRuntimeProject}",
                    $"-p:InnoToolTarget={target}",
                    $"-p:InnoBuildCliOutputPath={cliOutput}",
                    "-p:BuildInParallel=false",
                    "/m:1",
                    "/nodeReuse:false",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));

            var native = await BuildNativeDependenciesAsync(repositoryRoot, options, nativeConfiguration, cancellationToken);

            Console.WriteLine("[inno-bindings] Build the complete InnoEngine solution.");
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "build", Path.Combine(repositoryRoot, "InnoEngine.sln"),
                    "--configuration", options.configuration,
                    "--no-restore",
                    "-m:1", "-nodeReuse:false", "--disable-build-servers",
                    $"-p:BGCSRuntimeProject={bindGenRuntimeProject}",
                    $"-p:InnoToolTarget={target}",
                    $"-p:InnoBuildCliOutputPath={cliOutput}",
                    "-p:TreatWarningsAsErrors=true",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));

            IReadOnlyList<string> testProjects = DiscoverTestProjects(repositoryRoot);
            await RunTestsAsync(repositoryRoot, options, testProjects, native.products, native.plan, cancellationToken);
            string reportPath = await WriteReportAsync(
                repositoryRoot,
                bindGenRoot,
                target,
                bindingProjects,
                testProjects.Count,
                native.plan,
                cancellationToken);
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
        string bindGenRuntimeProject
    ) {
        foreach (string project in bindingProjects)
        {
            string definition = Path.Combine(Path.GetDirectoryName(project)!, "Bindings", "bindings.props");
            if (!File.Exists(definition))
                throw new FileNotFoundException("The sole component binding definition was not found.", definition);
        }
        if (!File.Exists(bindGenRuntimeProject))
            throw new FileNotFoundException("The BindGen-CS runtime project was not found.", bindGenRuntimeProject);

    }

    private static IReadOnlyList<string> DiscoverBindingProjects(
        string repositoryRoot,
        string targetId
    ) {
        var execution = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId(targetId));
        string[] projects = StandardBuildDistribution.Create(execution).build.ResolveNativeProduct(targetId, "editor").steps
            .Select(step => Path.GetFullPath(step.component.nativeProject, repositoryRoot))
            .Distinct(StringComparer.Ordinal).Where(File.Exists)
            .Where(project => XDocument.Load(project).Descendants("BindGenGeneratedBindings")
                .Any(static property => property.Value.Trim() == "true"))
            .Order(StringComparer.Ordinal).ToArray();
        if (projects.Length == 0)
            throw new InvalidOperationException("No Native projects declare BindGenGeneratedBindings=true.");
        return projects;
    }

    private static void AuditNativeImports(string repositoryRoot)
    {
        const int C_MAX_REPORTED_VIOLATIONS = 50;
        string nativeRoot = Path.Combine(repositoryRoot, "backends");
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

    private static async Task<(ProductNativeBuildPlan plan, IReadOnlyList<NativeBuildProduct> products)> BuildNativeDependenciesAsync(
        string repositoryRoot,
        NativeVerificationOptions options,
        string nativeConfiguration,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine("[inno-bindings] Build every native dependency required by generated bindings.");
        BuildCompositionContext captured = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new(options.target));
        var host = new BuildCompositionContext(options.dotnet, captured.applicationDirectory, captured.host, captured.toolsTarget, new NativeBindingGenerator(options.dotnet));
        BuildDistribution distribution = StandardBuildDistribution.Create(host).build;
        ProductNativeBuildPlan plan = distribution.ResolveNativeProduct(options.target, "editor");
        var context = new NativeBuildContext(repositoryRoot, nativeConfiguration).WithBindingGenerator(host.bindingGenerator);
        context = context.WithToolchain(await distribution.ResolveNativeToolchain(options.target)
            .ResolveAsync(context, host.host, options.target, cancellationToken).ConfigureAwait(false));
        return (plan, await plan.BuildAsync(context, cancellationToken).ConfigureAwait(false));
    }

    private static IReadOnlyList<string> DiscoverTestProjects(string repositoryRoot)
    {
        string nativeTestsRoot = Path.Combine(repositoryRoot, "backends");
        var projects = Directory.EnumerateFiles(nativeTestsRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(static path => Path.GetFileNameWithoutExtension(path).StartsWith("Inno.Native.", StringComparison.Ordinal)
                && path.Contains(Path.DirectorySeparatorChar + "tests" + Path.DirectorySeparatorChar, StringComparison.Ordinal)
                && !path.Split(Path.DirectorySeparatorChar).Any(static part => part is "bin" or "obj"))
            .Order(StringComparer.Ordinal).ToList();
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
        ProductNativeBuildPlan nativePlan,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine("[inno-bindings] Run every native binding, Text, and UI test project.");
        foreach (string project in testProjects)
        {
            string output = (await ToolchainEnvironment.CaptureOutputAsync(options.dotnet,
                ["msbuild", project, "-getProperty:TargetDir", $"-p:Configuration={options.configuration}",
                    $"-p:InnoToolTarget={options.target}"],
                repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet))).Trim();
            if (!Path.IsPathFullyQualified(output))
                throw new InvalidDataException("The test project did not declare an absolute target output directory.");
            await ProductNativeDeployment.InstallAsync(nativeProducts, nativePlan,
                output, cancellationToken)
                .ConfigureAwait(false);
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "test", project,
                    "--configuration", options.configuration,
                    "--no-build",
                    "--no-restore",
                    $"-p:InnoToolTarget={options.target}",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));
        }
    }

    private static async Task<string> WriteReportAsync(
        string repositoryRoot,
        string bindGenRoot,
        string target,
        IReadOnlyList<string> bindingProjects,
        int testProjectCount,
        ProductNativeBuildPlan nativePlan,
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
            nativeBuilds = nativePlan.steps.Select(static step => step.component.id).ToArray(),
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
            - Native builds: {string.Join(", ", nativePlan.steps.Select(static step => step.component.id))}
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

    [GeneratedRegex(@"\[(DllImport|LibraryImport)\b|partial\s+.*\bextern\b|static\s+extern\b")]
    private static partial Regex NativeImportPattern();

}
