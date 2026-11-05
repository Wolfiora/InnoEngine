using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationCompletionTests
{
    [Fact]
    public async Task FailedClosingReadCannotCertifyOtherwisePassingIntermediateResults()
    {
        using var repository = new ValidationRepository(TestRepository.GetRoot(typeof(ValidationCompletionTests).Assembly));
        using var external = new ValidationRepository();
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        string dependency = external.PathOf("Shared.props");
        const string C_PROJECT = "tests/input/Inno.Input.Tests/Inno.Input.Tests.csproj";
        repository.Write(C_PROJECT, File.ReadAllText(repository.PathOf(C_PROJECT)).Replace("</Project>",
            "<Import Project=\"" + System.Security.SecurityElement.Escape(dependency) + "\" /></Project>", StringComparison.Ordinal));
        string signal = repository.PathOf("artifacts/check-ready");
        string release = repository.PathOf("artifacts/check-release");
        const string C_RULES = "tests/ci/Test-AgentRules.ps1";
        repository.Write(C_RULES, File.ReadAllText(repository.PathOf(C_RULES)) + "\n"
            + "[IO.File]::WriteAllText('" + signal.Replace("'", "''", StringComparison.Ordinal) + "', 'ready')\n"
            + "while (-not (Test-Path -LiteralPath '" + release.Replace("'", "''", StringComparison.Ordinal) + "')) { Start-Sleep -Milliseconds 10 }\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare a coordinated closing-read failure fixture");
        repository.Write("docs/README.md", File.ReadAllText(repository.PathOf("docs/README.md")) + "\n收尾证据验收。\n");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Empty(plan.RootElement.GetProperty("owners").EnumerateArray());
        Task<int> execution = repository.RunAsync(cancellation.Token);
        try
        {
            while (!File.Exists(signal))
            {
                if (execution.IsCompleted)
                    Assert.Fail("Validation ended before the real instruction check reached its coordination point.");
                await Task.Delay(20, cancellation.Token);
            }
            using (var reader = new FileStream(dependency, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                File.WriteAllText(release, "continue");
                Exception? failure = await Record.ExceptionAsync(async () => await execution);
                Assert.NotNull(failure);
                Exception[] failures = failure is AggregateException aggregate ? aggregate.Flatten().InnerExceptions.ToArray() : [failure];
                Assert.Contains(failures, static error => error is IOException);
            }
            string evidence = repository.PathOf("artifacts/run/results.json");
            using JsonDocument results = JsonDocument.Parse(File.ReadAllText(evidence));
            Assert.Equal(plan.RootElement.GetProperty("checks").GetArrayLength(), results.RootElement.GetProperty("results").GetArrayLength());
            Assert.All(results.RootElement.GetProperty("results").EnumerateArray(), result =>
                Assert.Contains(result.GetProperty("status").GetString(), new[] { "Passed", "Reused", "NotApplicable" }));
            Assert.False(results.RootElement.GetProperty("completed").GetBoolean());
            await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(evidence));
            Assert.Equal(0, await repository.RunAsync(cancellation.Token, "artifacts/completed"));
            Assert.Equal(0, await repository.EvidenceAsync(repository.PathOf("artifacts/completed/results.json")));
        }
        finally
        {
            cancellation.Cancel();
            File.WriteAllText(release, "drain");
            try { await execution; }
            catch (Exception) { /* The assertion above owns the original failure; drain before retiring the fixture. */ }
        }
    }
}
