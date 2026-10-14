using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentBuildWorkerProcess
{
    internal static string Execute(
        string operation,
        string assembly,
        string root,
        string configuration,
        string dotnet,
        string tool,
        CancellationToken cancellation
    ) {
        using var bounded = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
        bounded.CancelAfter(TimeSpan.FromMinutes(10));
        ProcessOwnedDirectory working = ProcessOwnedDirectory.CreateAsync(
            Path.Combine(root, "artifacts", "entry", "workers"), bounded.Token).AsTask().GetAwaiter().GetResult();
        try
        {
            string nonce = Guid.NewGuid().ToString("N");
            var request = new DevelopmentBuildWorkerRequest(root, configuration, dotnet, assembly, operation, nonce, tool);
            string requestPath = Path.Combine(working.directory, "request.json");
            AtomicFile.WriteAllBytes(requestPath, JsonSerializer.SerializeToUtf8Bytes(request));
            string entry = Path.Combine(AppContext.BaseDirectory, "InnoEngine.Build.dll");
            if (!File.Exists(entry))
                throw new FileNotFoundException("The current Build entry cannot start its own worker.", entry);
            DevelopmentProcess.RunAsync(dotnet, [entry, DevelopmentBuildWorker.C_ARGUMENT, requestPath, nonce],
                root, capture: false, bounded.Token).GetAwaiter().GetResult();
            DevelopmentBuildWorkerResponse response = JsonSerializer.Deserialize<DevelopmentBuildWorkerResponse>(
                File.ReadAllBytes(Path.Combine(working.directory, "response.json")))
                ?? throw new InvalidDataException("The build worker response is empty.");
            if (response.nonce != nonce || string.IsNullOrWhiteSpace(response.result))
                throw new InvalidDataException("The build worker response does not belong to this operation.");
            return response.result;
        }
        catch (OperationCanceledException) when (!cancellation.IsCancellationRequested)
        {
            throw new TimeoutException("Build tool inspection exceeded its ten-minute process budget.");
        }
        finally
        {
            try { working.Dispose(); }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
            {
                Console.Error.WriteLine($"Build worker cleanup was deferred: {failure.Message}");
            }
        }
    }
}
