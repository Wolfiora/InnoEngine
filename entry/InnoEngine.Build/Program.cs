using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Cli;

namespace InnoEngine.Build;

internal static class Program
{
    private static async Task<int> Main(string[] arguments)
    {
        using var cancellation = new CancellationTokenSource();
        ConsoleCancelEventHandler onCancel = (
            _,
            options
        ) =>
        {
            options.Cancel = true;
            cancellation.Cancel();
        };
        Console.CancelKeyPress += onCancel;
        try
        {
            return await BuildApplication.Run(CreateArguments(arguments), cancellation.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return 2;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
        finally
        {
            Console.CancelKeyPress -= onCancel;
        }
    }

    private static string[] CreateArguments(string[] arguments)
    {
        if (arguments.Length > 0 && arguments[0] is "help" or "--help" or "-h")
            return arguments;
        bool implicitSupport = arguments.Length == 0 || arguments[0].StartsWith("--", StringComparison.Ordinal);
        var result = new List<string> { implicitSupport ? "support" : arguments[0] };
        result.AddRange(implicitSupport ? arguments : arguments[1..]);
        if (result[0] is not ("support" or "editor" or "clean" or "bindings" or "support-pack" or "shader"))
            return result.ToArray();
#if INNO_HOST_WINDOWS_X64
        const string C_TARGET = "windows-x64";
#elif INNO_HOST_MACOS_ARM64
        const string C_TARGET = "macos-arm64";
#else
        throw new PlatformNotSupportedException("No build host is implemented for this SDK host.");
#endif
#if INNO_HOST_WINDOWS_X64 || INNO_HOST_MACOS_ARM64
        int scope = result.IndexOf("--scope");
        bool allCaches = result[0] == "clean" && scope >= 0 && scope + 1 < result.Count && result[scope + 1] == "cache";
        if (!allCaches)
            AddDefault(result, "--target", C_TARGET);
        AddDefault(result, "--tools-target", C_TARGET);
        string configuration = typeof(Program).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        AddDefault(result, "--configuration", result[0] is "support" or "editor" or "clean" ? configuration : "Release");
        return result.ToArray();
#endif
    }

    private static void AddDefault(
        List<string> arguments,
        string name,
        string value
    ) {
        if (arguments.Contains(name))
            return;
        arguments.Add(name);
        arguments.Add(value);
    }
}
