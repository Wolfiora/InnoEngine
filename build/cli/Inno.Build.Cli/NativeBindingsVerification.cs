using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Core.IO;

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

        string? successfulRoot = null;
        try
        {
            NativeVerificationOptions options = NativeVerificationOptions.Parse(args);
            string repositoryRoot = options.engineRoot;
            BuildCompositionContext execution = StandardBuildEnvironment.Capture(AppContext.BaseDirectory,
                new BuildTargetId(options.target), options.dotnet);
            BuildDistribution distribution = StandardBuildDistribution.Create(execution).build;
            BuildPlatformContribution platform = distribution.platforms.SingleOrDefault(platform => platform.descriptor.id.value == options.target)
                ?? throw new NotSupportedException($"No publication target is registered for '{options.target}'.");
            if (platform.editorProject is null)
                throw new NotSupportedException($"Native verification requires a registered Editor test host; '{options.target}' contributes no such runner.");
            NativeProductContribution contribution = distribution.ResolveNativeProduct(options.target, "editor");
            var nativeContext = new NativeBuildContext(repositoryRoot, options.configuration.ToLowerInvariant())
                .WithBindingGenerator(distribution.ResolveBindingGenerator(options.target));
            NativeToolchainSelection sdk = await distribution.ResolveNativeToolchain(options.target)
                .ResolveAsync(nativeContext, execution.host, options.target, cancellationToken).ConfigureAwait(false);
            nativeContext = nativeContext.WithToolchain(sdk);
            string testHost = ToolchainEnvironment.ResolveExecutable("pwsh");
            string testEntry = Path.Combine(repositoryRoot, "tests", "Invoke-IsolatedTests.ps1");
            if (!File.Exists(testEntry))
                throw new FileNotFoundException("The common test execution entry is unavailable.", testEntry);
            IReadOnlyList<string> bindingProjects = DiscoverBindingProjects(repositoryRoot, contribution.plan);
            ValidateInputs(bindingProjects);

            string target = options.target;
            string operationRoot = Path.Combine(repositoryRoot, "artifacts", "build-tools", "verification");
            BuildCacheOwnerCatalog.Register(repositoryRoot, operationRoot, "native-verification", BuildCacheArtifactRole.ProcessWorkingDirectory, cancellationToken);
            using ProcessOwnedDirectory operation = await ProcessOwnedDirectory.CreateAsync(operationRoot, cancellationToken).ConfigureAwait(false);
            string artifactsPath = Path.Combine(operation.directory, "managed");
            Console.WriteLine("[inno-bindings] Verify deterministic generation against the checked-in host sources.");
            foreach (string project in bindingProjects)
            {
                await ToolchainEnvironment.RunAsync(options.dotnet,
                    ["build", project,
                        "--configuration", options.configuration, "-t:CheckBindings",
                        $"-p:InnoBindingDotnetHost={options.dotnet}",
                        "-p:InnoBindingTargetId=" + target, "-p:InnoBindingOutputMode=HostSource",
                        $"-p:InnoNativeTarget={target}", $"-p:InnoToolTarget={target}",
                        "--disable-build-servers", "-m:1", "-nr:false", "-p:UseSharedCompilation=false", "--nologo"],
                    repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));
            }

            AuditNativeImports(repositoryRoot);

            Console.WriteLine("[inno-bindings] Restore the complete InnoEngine solution.");
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "restore", Path.Combine(repositoryRoot, "InnoEngine.sln"),
                    "-p:Configuration=Source" + options.configuration,
                    $"-p:InnoToolTarget={target}",
                    $"-p:ArtifactsPath={artifactsPath}",
                    "-p:BuildInParallel=false",
                    "/m:1",
                    "/nodeReuse:false",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));

            var native = await BuildNativeDependenciesAsync(nativeContext, contribution, cancellationToken);
            using ProductNativeBuildResult nativeOwnership = native.products;

            Console.WriteLine("[inno-bindings] Build the complete InnoEngine solution.");
            await ToolchainEnvironment.RunAsync(
                options.dotnet,
                [
                    "build", Path.Combine(repositoryRoot, "InnoEngine.sln"),
                    "--configuration", "Source" + options.configuration,
                    "--no-restore",
                    "-m:1", "-nodeReuse:false", "--disable-build-servers",
                    $"-p:InnoToolTarget={target}",
                    $"-p:ArtifactsPath={artifactsPath}",
                    "-p:TreatWarningsAsErrors=true",
                    "--nologo"
                ],
                repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));

            IReadOnlyList<string> testProjects = DiscoverTestProjects(repositoryRoot);
            string testResults = await RunTestsAsync(repositoryRoot, options, testProjects,
                native.products, native.plan, testHost, testEntry, artifactsPath, cancellationToken);
            string reportPath = await WriteReportAsync(
                repositoryRoot,
                target,
                bindingProjects,
                testProjects.Count,
                native.plan,
                testResults,
                cancellationToken);
            Console.WriteLine(
                $"[inno-bindings] Project binding diffs, native builds, solution build, import audit, "
                + $"and {testProjects.Count} test projects passed.");
            Console.WriteLine($"[inno-bindings] Acceptance report: {reportPath}");
            successfulRoot = repositoryRoot;
            return 0;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            Console.Error.WriteLine($"[inno-bindings] {exception.Message}");
            return 1;
        }
        finally
        {
            if (successfulRoot is not null)
            {
                try
                {
                    await BuildMaintenanceApplication.CleanAsync(successfulRoot, new BuildCachePolicy(),
                        false, false, null, CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception failure)
                {
                    Console.Error.WriteLine($"Native verification passed; automatic cache maintenance was deferred: {failure.Message}");
                }
            }
        }
    }

    private static void ValidateInputs(IReadOnlyList<string> bindingProjects)
    {
        foreach (string project in bindingProjects)
        {
            string definition = Path.Combine(Path.GetDirectoryName(project)!, "Bindings", "bindings.props");
            if (!File.Exists(definition))
                throw new FileNotFoundException("The sole component binding definition was not found.", definition);
        }
    }

    private static IReadOnlyList<string> DiscoverBindingProjects(
        string repositoryRoot,
        ProductNativeBuildPlan plan
    ) {
        string[] projects = plan.steps
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

    private static async Task<(ProductNativeBuildPlan plan, ProductNativeBuildResult products)> BuildNativeDependenciesAsync(
        NativeBuildContext context,
        NativeProductContribution contribution,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine("[inno-bindings] Build the preflighted Editor Native closure with the frozen SDK.");
        return (contribution.plan, await contribution.BuildAsync(context, cancellationToken).ConfigureAwait(false));
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

    private static async Task<string> RunTestsAsync(
        string repositoryRoot,
        NativeVerificationOptions options,
        IReadOnlyList<string> testProjects,
        IReadOnlyList<NativeBuildProduct> nativeProducts,
        ProductNativeBuildPlan nativePlan,
        string testHost,
        string testEntry,
        string artifactsPath,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine("[inno-bindings] Run every native binding, Text, and UI test project.");
        foreach (string project in testProjects)
        {
            string output = (await ToolchainEnvironment.CaptureOutputAsync(options.dotnet,
                ["msbuild", project, "-getProperty:TargetDir", $"-p:Configuration={options.configuration}",
                    $"-p:InnoToolTarget={options.target}", $"-p:ArtifactsPath={artifactsPath}"],
                repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet))).Trim();
            if (!Path.IsPathFullyQualified(output))
                throw new InvalidDataException("The test project did not declare an absolute target output directory.");
            await ProductNativeDeployment.InstallAsync(nativeProducts, nativePlan,
                output, cancellationToken)
                .ConfigureAwait(false);
        }
        string results = Path.Combine(repositoryRoot, "artifacts", "acceptance", "native-bindings",
            options.target, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(results);
        string selection = Path.Combine(results, "Native.slnf");
        File.WriteAllText(selection, JsonSerializer.Serialize(new
        {
            solution = new
            {
                path = Path.Combine(repositoryRoot, "InnoEngine.sln"),
                projects = testProjects.Select(project => Path.GetRelativePath(repositoryRoot, project)).ToArray()
            }
        }));
        await ToolchainEnvironment.RunAsync(testHost,
            ["-NoLogo", "-NoProfile", "-NonInteractive", "-File", testEntry,
                "-Configuration", "Source" + options.configuration, "-Dotnet", options.dotnet,
                "-Solution", selection, "-ResultsDirectory", results, "-ArtifactsPath", artifactsPath],
            repositoryRoot, cancellationToken, DotNetSdkEnvironment.Create(options.dotnet));
        return Path.GetRelativePath(repositoryRoot, results).Replace('\\', '/');
    }

    private static async Task<string> WriteReportAsync(
        string repositoryRoot,
        string target,
        IReadOnlyList<string> bindingProjects,
        int testProjectCount,
        ProductNativeBuildPlan nativePlan,
        string testResults,
        CancellationToken cancellationToken
    ) {
        string innoRevision = (await ToolchainEnvironment.CaptureOutputAsync(
            "git", ["-C", repositoryRoot, "rev-parse", "HEAD"], repositoryRoot, cancellationToken)).Trim();
        Assembly bindingImplementation = typeof(NativeBindingGenerator).Assembly;
        string packageVersion = bindingImplementation.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(static metadata => metadata.Key == "Inno.BindingPackageVersion").Value
            ?? throw new InvalidDataException("The binding implementation has no frozen package version.");
        string implementationHash = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(bindingImplementation.Location)));
        bool innoDirty = !string.IsNullOrWhiteSpace(await ToolchainEnvironment.CaptureOutputAsync(
            "git", ["-C", repositoryRoot, "status", "--porcelain", "--untracked-files=all"], repositoryRoot, cancellationToken));
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
                bindingPackages = new { version = packageVersion, implementationHash }
            },
            bindingProjects = bindingProjects.Select(Path.GetFileNameWithoutExtension).ToArray(),
            nativeBuilds = nativePlan.steps.Select(static step => step.component.id).ToArray(),
            nativeTestProjects = testProjectCount,
            testResults,
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
            - BGCS NuGet packages: `{packageVersion}` (binding implementation SHA-256: `{implementationHash}`)
            - Binding projects: {string.Join(", ", bindingProjects.Select(Path.GetFileNameWithoutExtension))}
            - Native builds: {string.Join(", ", nativePlan.steps.Select(static step => step.component.id))}
            - Native test projects: {testProjectCount}
            - Shared and isolated test results: `{testResults}`

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
