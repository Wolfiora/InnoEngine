using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Entry.Bootstrap;

/// <summary>
/// Provides the two checkout development commands without selecting platforms in shared engine code.
/// </summary>
public static class DevelopmentEntry
{
    private const string C_BUILD_PROJECT = "build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj";

    /// <summary>
    /// Runs the validated prepared Editor on the original main thread, building it when preparation is absent.
    /// </summary>
    /// <param name="arguments">
    /// An optional project directory followed by Editor options; no directory opens a native folder dialog.
    /// Use --help or --status to inspect the entry without building a product.
    /// Engine source changes are applied by the Build entry; opening a prepared Editor does not invoke build tools.
    /// </param>
    /// <returns>
    /// Zero on normal exit or dialog cancellation, two on cancellation, or one on failure.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The argument array is absent.
    /// </exception>
    public static int RunEditor(string[] arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        try
        {
            if (ShowInformation(arguments, editor: true))
                return 0;
            DevelopmentHost host = DevelopmentHost.Resolve();
            string editorEntryType = host.RequireEditor();
            string root = FindRoot();
            using var cancellation = CreateCancellation(out ConsoleCancelEventHandler onCancel);
            try
            {
                InitializeLease(LockPath(root), cancellation.Token);
                FileLease? lease = FileLease.AcquireSharedAsync(LockPath(root), TimeSpan.FromMinutes(10), cancellation.Token)
                    .GetAwaiter().GetResult();
                FileLease? productLease = null;
                string productLeasePath = Path.Combine(root, "artifacts", "products", "editor", host.target,
                    Configuration(), "product.lock");
                try
                {
                    InitializeLease(productLeasePath, cancellation.Token);
                    productLease = FileLease.AcquireSharedAsync(productLeasePath,
                        TimeSpan.FromSeconds(10), cancellation.Token).GetAwaiter().GetResult();
                    string? path = DevelopmentProduct.Open(root, host.target, Configuration(), cancellation.Token);
                    if (path is null)
                    {
                        productLease.Dispose();
                        productLease = null;
                        lease.Dispose();
                        lease = null;
                        using (FileLease.AcquireAsync(LockPath(root), TimeSpan.FromMinutes(10), cancellation.Token)
                            .GetAwaiter().GetResult())
                        {
                            using FileLease productPreparation = FileLease.AcquireAsync(productLeasePath,
                                TimeSpan.FromSeconds(10), cancellation.Token).GetAwaiter().GetResult();
                            // Another preparation may have completed while this caller waited for ownership.
                            if (DevelopmentProduct.Open(root, host.target, Configuration(), cancellation.Token) is null)
                            {
                                string dotnet = DevelopmentProcess.ResolveDotnet();
                                DevelopmentDependencies.PrepareAsync(root, cancellation.Token).GetAwaiter().GetResult();
                                BuildAsync(root, dotnet, host.editorProject!, cancellation.Token).GetAwaiter().GetResult();
                                path = GetAssemblyPathAsync(root, dotnet, host.editorProject!, Configuration(), cancellation.Token)
                                    .GetAwaiter().GetResult();
                                DevelopmentProduct.Record(root, host.target, Configuration(), path, cancellation.Token);
                            }
                        }
                        lease = FileLease.AcquireSharedAsync(LockPath(root), TimeSpan.FromMinutes(10), cancellation.Token)
                            .GetAwaiter().GetResult();
                        productLease = FileLease.AcquireSharedAsync(productLeasePath,
                            TimeSpan.FromSeconds(10), cancellation.Token).GetAwaiter().GetResult();
                        path = DevelopmentProduct.Open(root, host.target, Configuration(), cancellation.Token)
                            ?? throw new IOException("The Editor deployment changed before startup. Run InnoEngine.Build.");
                    }
                    Console.WriteLine($"Starting prepared Editor ({host.target}/{Configuration()}). Engine source changes require InnoEngine.Build.");
                    using var application = new DevelopmentApplication(path);
                    Func<string[], CancellationToken, int> run = application.GetEntry<Func<string[], CancellationToken, int>>(editorEntryType);
                    return run(arguments, cancellation.Token);
                }
                finally
                {
                    try
                    {
                        productLease?.Dispose();
                    }
                    finally
                    {
                        lease?.Dispose();
                    }
                }
            }
            finally
            {
                Console.CancelKeyPress -= onCancel;
            }
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Editor preparation canceled.");
            return 2;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure.Message);
            return 1;
        }
    }

    /// <summary>
    /// Prepares and invokes the shared build application for the current development host.
    /// </summary>
    /// <param name="arguments">
    /// No arguments selects an incremental Editor build. Use --clean to clean the selected product first.
    /// </param>
    /// <returns>
    /// Zero on success, two on cancellation, or one on preparation or build failure.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The argument array is absent.
    /// </exception>
    public static async Task<int> RunBuildAsync(string[] arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        try
        {
            if (ShowInformation(arguments, editor: false))
                return 0;
            DevelopmentHost host = DevelopmentHost.Resolve();
            string root = FindRoot();
            string dotnet = DevelopmentProcess.ResolveDotnet();
            string[] command = DevelopmentBuildRequest.Create(arguments, host, root, dotnet, Configuration());
            using var cancellation = CreateCancellation(out ConsoleCancelEventHandler onCancel);
            try
            {
                using var lease = await FileLease.AcquireAsync(LockPath(root), TimeSpan.FromMinutes(10), cancellation.Token)
                    .ConfigureAwait(false);
                await DevelopmentDependencies.PrepareAsync(root, cancellation.Token).ConfigureAwait(false);
                await BuildAsync(root, dotnet, C_BUILD_PROJECT, cancellation.Token).ConfigureAwait(false);
                string path = await GetAssemblyPathAsync(root, dotnet, C_BUILD_PROJECT, Configuration(), cancellation.Token).ConfigureAwait(false);
                using var application = new DevelopmentApplication(path);
                Func<string[], CancellationToken, Task<int>> run = application
                    .GetEntry<Func<string[], CancellationToken, Task<int>>>("Inno.Build.Cli.BuildApplication");
                int result = await run(command, cancellation.Token).ConfigureAwait(false);
                if (result == 0 && host.editorProject is not null
                    && command[0] == "editor" && command[Array.IndexOf(command, "--target") + 1] == host.target)
                {
                    string configuration = command[Array.IndexOf(command, "--configuration") + 1];
                    string editor = await GetAssemblyPathAsync(root, dotnet, host.editorProject, configuration,
                        cancellation.Token).ConfigureAwait(false);
                    DevelopmentProduct.Record(root, host.target, configuration, editor, cancellation.Token);
                    Console.WriteLine($"Editor prepared for direct startup ({host.target}/{configuration}).");
                }
                return result;
            }
            finally
            {
                Console.CancelKeyPress -= onCancel;
            }
        }
        catch (OperationCanceledException)
        {
            Console.Error.WriteLine("Build canceled; incomplete preparations were not published.");
            return 2;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure.Message);
            return 1;
        }
    }

    private static Task<string> BuildAsync(
        string root,
        string dotnet,
        string project,
        CancellationToken cancellationToken
    ) {
        Console.WriteLine($"Preparing {Path.GetFileNameWithoutExtension(project)} ({Configuration()})...");
        return DevelopmentProcess.RunAsync(dotnet,
            ["build", Path.GetFullPath(project, root), "--configuration", Configuration(),
                "--disable-build-servers", "-m:1", "-nodeReuse:false"], root, capture: false, cancellationToken);
    }

    private static async Task<string> GetAssemblyPathAsync(
        string root,
        string dotnet,
        string project,
        string configuration,
        CancellationToken cancellationToken
    ) {
        string path = await DevelopmentProcess.RunAsync(dotnet,
            ["msbuild", Path.GetFullPath(project, root), "-nologo", "-getProperty:TargetPath",
                "-p:Configuration=" + configuration], root, capture: true, cancellationToken).ConfigureAwait(false);
        return Path.IsPathFullyQualified(path) && File.Exists(path) ? path
            : throw new FileNotFoundException("The selected product did not provide its built assembly.", path);
    }

    private static string FindRoot()
    {
        for (DirectoryInfo? directory = new(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Combine(directory.FullName, "InnoEngine.sln")))
                return directory.FullName;
        throw new DirectoryNotFoundException("The development entry must run inside an InnoEngine checkout.");
    }

    private static string LockPath(string root) => Path.Combine(root, "artifacts", "entry", "checkout.lock");

    private static void InitializeLease(
        string path,
        CancellationToken cancellationToken
    ) {
        if (!File.Exists(path))
            FileLease.AcquireAsync(path, TimeSpan.FromSeconds(10), cancellationToken).GetAwaiter().GetResult().Dispose();
    }

    private static string Configuration() => typeof(DevelopmentEntry).Assembly
        .GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration == "Release" ? "Release" : "Debug";

    private static CancellationTokenSource CreateCancellation(out ConsoleCancelEventHandler onCancel)
    {
        var cancellation = new CancellationTokenSource();
        onCancel = (
            _,
            arguments
        ) =>
        {
            arguments.Cancel = true;
            cancellation.Cancel();
        };
        Console.CancelKeyPress += onCancel;
        return cancellation;
    }

    private static bool ShowInformation(
        string[] arguments,
        bool editor
    ) {
        if (arguments.Length != 1 || arguments[0] is not ("--help" or "-h" or "--status"))
            return false;
        if (arguments[0] == "--status")
        {
            DevelopmentHost host = DevelopmentHost.Resolve();
            Console.WriteLine($"Host: {host.target}; Editor: {host.editorAssembly ?? "not implemented"}; Configuration: {Configuration()}");
            Console.WriteLine($"Checkout: {FindRoot()}");
            return true;
        }
        Console.WriteLine(editor ? "InnoEngine.Editor [project-directory] [--graphics-api API] [--smoke-frames COUNT]"
            : "InnoEngine.Build [editor] [--configuration Debug|Release] [--clean]\n"
                + "InnoEngine.Build clean --scope product [--dry-run]\n"
                + "InnoEngine.Build <game|plugin|scripts|bindings|support-pack|verify|verify-native|shader> [options]");
        return true;
    }
}
