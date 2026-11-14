using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

/// <summary>
/// Isolates the process console while observing the public validation entry's live progress.
/// </summary>
[CollectionDefinition(nameof(ValidationProgressCollection), DisableParallelization = true)]
public sealed class ValidationProgressCollection { }

[Collection(nameof(ValidationProgressCollection))]
public sealed class ValidationProgressTests
{
    [Fact]
    public async Task QuietChildReportsActivityBeforeExitAndCancellationRetainsItsFailureEvidence()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/ci/Test-AgentRules.ps1", """
            [Console]::Out.WriteLine('first stdout evidence')
            [Console]::Error.WriteLine('first stderr evidence')
            [Console]::Out.Write('::error::this is child text, not a workflow command')
            while ($true) { Start-Sleep -Milliseconds 100 }
            """);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare an intentionally waiting validation child");
        using JsonDocument generated = await repository.PlanAsync();
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonNode policy = plan["checks"]!.AsArray().Single(check => check!["kind"]!.GetValue<string>() == "instructions")!.DeepClone();
        policy["prerequisites"] = new JsonArray();
        plan["checks"] = new JsonArray(policy);
        repository.Write("artifacts/plan/plan.json", plan.ToJsonString());

        using var cancellation = new CancellationTokenSource();
        using var capture = new ProgressCapture();
        TextWriter original = Console.Out;
        Console.SetOut(capture);
        Task<int>? running = null;
        try
        {
            running = repository.RunAsync(cancellation.Token);
            string activity = await capture.activity.Task.WaitAsync(TimeSpan.FromMinutes(2));
            Assert.False(running.IsCompleted);
            Assert.StartsWith("Process active: instructions.log;", activity);
            Assert.Contains("parentCpu=", activity);
            Assert.Contains("outputCharacters=", activity);
            Assert.Contains("lastOutputAge=", activity);
            Assert.Contains("lastOutputExcerpt=", activity);
            Assert.Contains("Validation check: instructions;", capture.Read());
            Assert.Contains("Process started: instructions.log;", capture.Read());
            cancellation.Cancel();
            await Assert.ThrowsAsync<InvalidDataException>(async () => await running);
            using JsonDocument evidence = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
            Assert.Equal("Blocked", evidence.RootElement.GetProperty("results")[0].GetProperty("status").GetString());
            Assert.False(evidence.RootElement.GetProperty("completed").GetBoolean());
            string log = File.ReadAllText(repository.PathOf("artifacts/run/instructions.log"));
            Assert.Contains("first stdout evidence", log);
            Assert.Contains("first stderr evidence", log);
            Assert.Contains("::error::this is child text", log);
            Assert.DoesNotContain("\n::error::", capture.Read().Replace("\r\n", "\n", StringComparison.Ordinal));
        }
        finally
        {
            cancellation.Cancel();
            if (running is not null)
            {
                try { await running; }
                catch (Exception) when (cancellation.IsCancellationRequested) { }
            }
            Console.SetOut(original);
        }
    }

    private sealed class ProgressCapture : StringWriter
    {
        private readonly object m_sync = new();

        internal TaskCompletionSource<string> activity { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public override void WriteLine(string? value)
        {
            lock (m_sync)
            {
                base.WriteLine(value);
                if (value?.StartsWith("Process active:", StringComparison.Ordinal) == true)
                    activity.TrySetResult(value);
            }
        }

        internal string Read()
        {
            lock (m_sync)
                return ToString();
        }
    }
}
