using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentBuildWorker
{
    internal const string C_ARGUMENT = "--internal-build-worker";

    internal static async Task<int> RunAsync(string[] arguments)
    {
        try
        {
            if (arguments.Length != 3)
                throw new ArgumentException("A build worker requires its request path and nonce.");
            string requestPath = Path.GetFullPath(arguments[1]);
            DevelopmentBuildWorkerRequest request = JsonSerializer.Deserialize<DevelopmentBuildWorkerRequest>(File.ReadAllBytes(requestPath))
                ?? throw new InvalidDataException("The build worker request is empty.");
            Validate(requestPath, request, arguments[2]);
            string workingRoot = Path.Combine(request.root, "artifacts", "entry", "workers");
            string directory = Path.GetDirectoryName(requestPath)!;
            ProcessOwnedDirectory.RegisterCurrentProcess(workingRoot, directory);
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            using DevelopmentBuildToolLease? reading = request.operation == "inspect"
                ? new DevelopmentBuildToolLease(request.assembly, cancellation.Token) : null;
            using var application = new DevelopmentApplication(request.assembly);
            string? result;
            if (request.operation == "inspect")
            {
                var inspect = application.GetEntry<Func<string, string, string, CancellationToken, Task<string>>>(
                    "Inno.Build.Cli.BuildApplication", "ReadInputIdentityAsync");
                result = await inspect(request.root, request.configuration, request.dotnet, cancellation.Token).ConfigureAwait(false);
            }
            else
            {
                var publish = application.GetEntry<Func<string, string, string, string, CancellationToken, Task<string>>>(
                    "Inno.Build.Cli.BuildApplication", "PublishRuntimeAsync");
                result = await publish(request.root, request.configuration, request.dotnet, request.assembly, cancellation.Token).ConfigureAwait(false);
            }
            AtomicFile.WriteAllBytes(Path.Combine(directory, "response.json"),
                JsonSerializer.SerializeToUtf8Bytes(new DevelopmentBuildWorkerResponse(request.nonce, result)));
            return 0;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
    }

    private static void Validate(
        string requestPath,
        DevelopmentBuildWorkerRequest request,
        string nonce
    ) {
        if (request.nonce != nonce || nonce.Length != 32 || !Guid.TryParseExact(nonce, "N", out _)
            || request.operation is not ("inspect" or "publish") || request.configuration is not ("Debug" or "Release"))
            throw new InvalidDataException("The build worker operation, configuration or nonce is invalid.");
        if (!Path.IsPathFullyQualified(request.root) || !File.Exists(Path.Combine(request.root, "InnoEngine.sln"))
            || !Path.IsPathFullyQualified(request.dotnet) || !File.Exists(request.dotnet))
            throw new InvalidDataException("The worker requires an explicit checkout and managed host.");
        string workers = Path.Combine(request.root, "artifacts", "entry", "workers");
        PathBoundary.RequireUnlinkedPath(workers, requestPath);
        string directory = Path.GetDirectoryName(requestPath)!;
        if (Path.GetDirectoryName(directory) != workers || Path.GetFileName(requestPath) != "request.json")
            throw new InvalidDataException("The request is outside the claimed worker directory.");
        string hosts = Path.Combine(request.root, "artifacts", "build-tools", "cli", "hosts");
        string outputs = Path.Combine(request.root, "artifacts", "build-tools", "cli", "workspaces");
        string assembly = Path.GetFullPath(request.assembly);
        if (Path.GetFileName(assembly) != "Inno.Build.Cli.dll" || !File.Exists(assembly)
            || !(IsChild(hosts, assembly) || IsChild(outputs, assembly)))
            throw new InvalidDataException("The worker assembly is outside the Build tool's publication or bootstrap output.");
        PathBoundary.RequireUnlinkedPath(request.root, assembly);
    }

    private static bool IsChild(
        string root,
        string path
    ) {
        string relative = Path.GetRelativePath(root, path);
        return !Path.IsPathRooted(relative) && relative != ".." && !relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal);
    }
}

internal sealed record DevelopmentBuildWorkerResponse(
    string nonce,
    string? result
);
