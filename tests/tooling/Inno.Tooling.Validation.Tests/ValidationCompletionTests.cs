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
    public async Task ChangedCompilationInputsCannotPublishReusableBuildIdentity()
    {
        using var repository = new ValidationRepository(TestRepository.GetRoot(typeof(ValidationCompletionTests).Assembly));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(20));
        const string C_PROBE = "tests/input/Inno.Input.Tests/CompilationInputProbe.cs";
        const string C_DEPENDENCY = "src/foundation/core/Inno.Core.IO/CompilationDependencyProbe.cs";
        const string C_RULES = "tests/ci/Test-AgentRules.ps1";
        string gate = repository.PathOf("artifacts/change-compilation-input");
        string source = repository.PathOf(C_PROBE);
        repository.Write(C_RULES, File.ReadAllText(repository.PathOf(C_RULES)) + "\n"
            + "$gate = '" + gate.Replace("'", "''", StringComparison.Ordinal) + "'\n"
            + "if (Test-Path -LiteralPath $gate) {\n"
            + "  $source = '" + source.Replace("'", "''", StringComparison.Ordinal) + "'\n"
            + "  [IO.File]::WriteAllText($source, [IO.File]::ReadAllText($source).Replace('Assert.Equal(2, CompilationDependencyProbe.value)', 'Assert.Equal(3, CompilationDependencyProbe.value)'))\n"
            + "  Remove-Item -LiteralPath $gate\n}\n");
        repository.Write(C_DEPENDENCY, """
            namespace Inno.Core.IO;
            /// <summary>
            /// Supplies an observable compiled dependency value for the validation fixture.
            /// </summary>
            public static class CompilationDependencyProbe
            {
                /// <summary>
                /// Gets the current compiled fixture input.
                /// </summary>
                public static int value => 2;
            }
            """);
        repository.Git("add", C_RULES, C_DEPENDENCY);
        repository.Git("commit", "-m", "Declare a coordinated compilation input mutation");
        repository.Write(C_PROBE, """
            using Xunit;
            using Inno.Core.IO;
            namespace Inno.Input.Tests;
            public sealed class CompilationInputProbe
            {
                [Fact]
                public void CompiledInputMatchesItsDeclaredSource() { Assert.Equal(2, CompilationDependencyProbe.value); }
            }
            """);
        using (await repository.PlanAsync()) { }
        Assert.Equal(0, await repository.RunAsync(cancellation.Token, "artifacts/warmup"));
        repository.Write(C_PROBE, File.ReadAllText(source) + "\n// Change this owner's compilation input.\n");
        byte[] original = File.ReadAllBytes(source);
        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        File.WriteAllText(gate, "mutate after planning");
        using (await repository.PlanAsync()) { }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(cancellation.Token));
        using (JsonDocument failed = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json"))))
        {
            JsonElement[] results = failed.RootElement.GetProperty("results").EnumerateArray().ToArray();
            Assert.Contains(results, result => result.GetProperty("id").GetString() == "input-stability"
                && result.GetProperty("status").GetString() == "Failed");
            Assert.Contains(results, result => result.GetProperty("id").GetString() == "test:tests/input/Inno.Input.Tests/Inno.Input.Tests.csproj"
                && result.GetProperty("status").GetString() == "Failed"
                && result.GetProperty("detail").GetString()!.Contains("CompiledInputMatchesItsDeclaredSource", StringComparison.Ordinal));
        }
        File.WriteAllBytes(source, original);
        File.SetLastWriteTimeUtc(source, timestamp);
        using (await repository.PlanAsync()) { }
        Assert.Equal(0, await repository.RunAsync(cancellation.Token, "artifacts/repaired"));
        Assert.Equal(0, await repository.EvidenceAsync(repository.PathOf("artifacts/repaired/results.json")));

        repository.Write(C_PROBE, File.ReadAllText(source) + "\n// Recompile only this owner's changed source.\n");
        string dependencyOutput;
        using (JsonDocument plan = await repository.PlanAsync())
            dependencyOutput = plan.RootElement.GetProperty("contexts").GetProperty("managed").GetProperty("sdkGraph").EnumerateArray()
                .Single(snapshot => snapshot.GetProperty("Path").GetString()!.EndsWith("Inno.Core.IO.csproj", StringComparison.Ordinal))
                .GetProperty("Properties").GetProperty("TargetPath").GetString()!;
        DateTime outputTimestamp = File.GetLastWriteTimeUtc(dependencyOutput);
        byte[] dependencyBytes = File.ReadAllBytes(dependencyOutput);
        Assert.Equal(0, await repository.RunAsync(cancellation.Token, "artifacts/owner-only"));
        Assert.Equal(dependencyBytes, File.ReadAllBytes(dependencyOutput));
        Assert.Equal(outputTimestamp, File.GetLastWriteTimeUtc(dependencyOutput));

        string dependency = repository.PathOf(C_DEPENDENCY);
        DateTime dependencyTimestamp = File.GetLastWriteTimeUtc(dependency);
        repository.Write(C_DEPENDENCY, File.ReadAllText(dependency).Replace("value => 2", "value => 3", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(dependency, dependencyTimestamp);
        // Force a content read: this fixture deliberately preserves the Git stat-cache size and timestamp.
        repository.Git("add", "--renormalize", "--", C_DEPENDENCY);
        repository.Git("commit", "-m", "Change a previously compiled dependency without changing its timestamp");
        using (JsonDocument plan = await repository.PlanAsync())
            Assert.Single(plan.RootElement.GetProperty("owners").EnumerateArray());
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(cancellation.Token, "artifacts/dependency-changed"));
        using JsonDocument changed = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/dependency-changed/results.json")));
        Assert.Contains(changed.RootElement.GetProperty("results").EnumerateArray(), result =>
            result.GetProperty("id").GetString() == "test:tests/input/Inno.Input.Tests/Inno.Input.Tests.csproj"
            && result.GetProperty("status").GetString() == "Failed"
            && result.GetProperty("detail").GetString()!.Contains("CompiledInputMatchesItsDeclaredSource", StringComparison.Ordinal));
    }

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
