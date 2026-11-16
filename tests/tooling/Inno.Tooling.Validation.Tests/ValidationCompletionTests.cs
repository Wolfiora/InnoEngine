using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Testing.Source;
using Inno.Tooling.Documentation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationCompletionTests
{
    [Fact]
    public Task ChangedUnionRootsRecompileWithoutCleaningUnchangedDependencies() => ChangedUnionRootsAsync(variant: false);

    [Fact]
    public Task ChangedPropertyVariantsCannotReuseAnUncleanedOutput() => ChangedUnionRootsAsync(variant: true);

    [Fact]
    public Task ChangedRootsCompileReferencesSelectedDuringBuild() => ChangedUnionRootsAsync(variant: false, lateSelection: true);

    private static async Task ChangedUnionRootsAsync(bool variant, bool lateSelection = false)
    {
        using var repository = new ValidationRepository(TestRepository.GetRoot(typeof(ValidationCompletionTests).Assembly));
        // Source capture and three real compilation cycles prove content correctness, not a short latency budget.
        using var cancellation = new CancellationTokenSource(TimeSpan.FromHours(1));
        const string C_FIXTURE = "tests/input/fixtures/Inno.Input.CompilationFixture";
        const string C_PROJECT = "tests/input/Inno.Input.Tests/Inno.Input.Tests.csproj";
        const string C_TEST = "tests/input/Inno.Input.Tests/UnionCompilationProbe.cs";
        repository.Write(C_FIXTURE + "/Inno.Input.CompilationFixture.csproj", ValidationRepository.Project(
            "<ItemGroup><ProjectReference Include=\"../../../../src/foundation/extensibility/Inno.Extensibility.Catalogs/Inno.Extensibility.Catalogs.csproj\" /></ItemGroup>"));
        repository.Write(C_FIXTURE + "/Probe.cs", """
            namespace Inno.Input.CompilationFixture;
            /// <summary>Exposes a real fixture compilation value.</summary>
            public static class Probe
            {
                /// <summary>Reads the value compiled from this source.</summary>
                /// <returns>The current fixture value.</returns>
                public static int Read() => 1;
            }
            """);
        repository.Write(C_PROJECT, File.ReadAllText(repository.PathOf(C_PROJECT)).Replace("</Project>",
            "<ItemGroup><ProjectReference Include=\"../fixtures/Inno.Input.CompilationFixture/Inno.Input.CompilationFixture.csproj\""
            + " /></ItemGroup><PropertyGroup><BuildProjectReferences>true</BuildProjectReferences></PropertyGroup></Project>", StringComparison.Ordinal)
            .Replace("<Project Sdk=", "<Project TreatAsLocalProperty=\"BuildProjectReferences\" Sdk=", StringComparison.Ordinal));
        repository.Write(C_TEST, """
            using Xunit;
            using Inno.Input.CompilationFixture;
            namespace Inno.Input.Tests;
            public sealed class UnionCompilationProbe
            {
                [Fact]
                public void ReadsTheChangedDependency() { Assert.Equal(1, Probe.Read()); }
            }
            """);
        if (lateSelection)
        {
            repository.Write(C_PROJECT, File.ReadAllText(repository.PathOf(C_PROJECT))
                .Replace("<ItemGroup><ProjectReference Include=\"../fixtures/Inno.Input.CompilationFixture/Inno.Input.CompilationFixture.csproj\" /></ItemGroup>",
                    string.Empty, StringComparison.Ordinal)
                .Replace("</Project>", """
                <ItemGroup>
                  <InnoValidationInput Include="../fixtures/Inno.Input.CompilationFixture/*.cs;../fixtures/Inno.Input.CompilationFixture/*.csproj" />
                </ItemGroup>
                <Target Name="SelectCompilationReference" BeforeTargets="AssignProjectConfiguration">
                  <ItemGroup>
                    <ProjectReference Include="../fixtures/Inno.Input.CompilationFixture/Inno.Input.CompilationFixture.csproj" />
                  </ItemGroup>
                </Target>
                </Project>
                """, StringComparison.Ordinal));
        }
        repository.Write("Directory.Build.targets", File.ReadAllText(repository.PathOf("Directory.Build.targets")).Replace("</Project>", """
            <Target Name="RecordDependencyBuild" AfterTargets="Build" Condition="'$(MSBuildProjectName)' == 'Inno.Core.IO'">
              <WriteLinesToFile File="$(ArtifactsPath)/dependency-builds.log" Lines="$(MSBuildProjectFullPath)" Overwrite="false" />
            </Target>
            </Project>
            """, StringComparison.Ordinal));
        await repository.SdkAsync("sln", "InnoEngine.sln", "add", C_FIXTURE + "/Inno.Input.CompilationFixture.csproj",
            "--solution-folder", "tests/input/fixtures");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the compilation dependency and its real assertion");
        var documentation = new ValidationDocumentationFixture(repository);
        documentation.Initialize();
        await documentation.CaptureAsync(C_FIXTURE + "/Inno.Input.CompilationFixture.csproj");
        DocumentationResult documentationSnapshot = await documentation.IndexAsync();

        string dependencyOutput = string.Empty;
        string dependencyBuildLog = string.Empty;
        int dependencyBuilds = 0;
        string restoredEnvironment = string.Empty;
        string variantArtifacts = string.Empty;
        string workspace = string.Empty;
        DateTime outputTimestamp = default;
        byte[] dependencyBytes = [];
        // The first build restores previously absent SDK imports. Establish that real environment before comparing warm compilations.
        for (int value = 2; value <= 4; value++)
        {
            if (value == 3 && variant)
            {
                repository.Write(C_PROJECT, File.ReadAllText(repository.PathOf(C_PROJECT)).Replace(
                    "Inno.Input.CompilationFixture.csproj\"", "Inno.Input.CompilationFixture.csproj\" AdditionalProperties=\"ArtifactsPath=$(ArtifactsPath)/variant\"",
                    StringComparison.Ordinal));
                repository.Git("add", C_PROJECT);
                repository.Git("commit", "-m", "Consume a second effective property instance");
                // The first real run has established workspace ownership. Restore the extra NuGet output instance before measuring it.
                using (await Inno.Core.IO.CoordinatedFileLease.AcquireAsync(Path.Combine(Path.GetDirectoryName(workspace)!, "owner.lock"),
                    workspace + ".lock", Timeout.InfiniteTimeSpan, cancellationToken: cancellation.Token))
                    await repository.SdkAsync("restore", C_FIXTURE + "/Inno.Input.CompilationFixture.csproj",
                        "-p:ArtifactsPath=" + variantArtifacts, "-p:Configuration=Debug", "--disable-parallel", "-nr:false");
            }
            foreach (string relative in new[] { C_FIXTURE + "/Probe.cs", C_TEST })
            {
                string path = repository.PathOf(relative);
                DateTime timestamp = File.GetLastWriteTimeUtc(path);
                repository.Write(relative, File.ReadAllText(path)
                    .Replace("=> " + (value - 1), "=> " + value, StringComparison.Ordinal)
                    .Replace("Assert.Equal(" + (value - 1), "Assert.Equal(" + value, StringComparison.Ordinal));
                File.SetLastWriteTimeUtc(path, timestamp);
            }
            using (JsonDocument plan = await documentation.PlanAsync(documentationSnapshot))
            {
                Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
                Assert.Equal(2, plan.RootElement.GetProperty("owners").GetArrayLength());
                if (lateSelection)
                    Assert.DoesNotContain(plan.RootElement.GetProperty("contexts").GetProperty("managed").GetProperty("sdkGraph").EnumerateArray()
                        .Single(snapshot => snapshot.GetProperty("Path").GetString()!.EndsWith("Inno.Input.Tests.csproj", StringComparison.Ordinal))
                        .GetProperty("Items").GetProperty("ProjectReference").EnumerateArray(), reference =>
                            reference.GetProperty("FullPath").GetString()!.EndsWith("Inno.Input.CompilationFixture.csproj", StringComparison.Ordinal));
                JsonElement fixture = plan.RootElement.GetProperty("contexts").GetProperty("managed").GetProperty("sdkGraph").EnumerateArray()
                    .Single(snapshot => snapshot.GetProperty("Path").GetString()!.EndsWith("Inno.Input.CompilationFixture.csproj", StringComparison.Ordinal));
                if (variant && value >= 3)
                    Assert.Contains(fixture.GetProperty("Variants").EnumerateArray(), instance =>
                        instance.GetProperty("Properties").GetProperty("TargetPath").GetString()
                        != fixture.GetProperty("Properties").GetProperty("TargetPath").GetString());
                else
                    Assert.Empty(fixture.GetProperty("Variants").EnumerateArray());
                variantArtifacts = Path.Combine(plan.RootElement.GetProperty("contexts").GetProperty("managed").GetProperty("properties")
                    .GetProperty("ArtifactsPath").GetString()!, "variant");
                workspace = plan.RootElement.GetProperty("artifacts").GetString()!;
                string environment = plan.RootElement.GetProperty("environment").GetString()!;
                if (value == 3)
                    restoredEnvironment = environment;
                else if (value == 4)
                    Assert.Equal(restoredEnvironment, environment);
                dependencyOutput = plan.RootElement.GetProperty("contexts").GetProperty("managed").GetProperty("sdkGraph").EnumerateArray()
                    .Single(snapshot => snapshot.GetProperty("Path").GetString()!.EndsWith("Inno.Core.IO.csproj", StringComparison.Ordinal))
                    .GetProperty("Properties").GetProperty("TargetPath").GetString()!;
                dependencyBuildLog = Path.Combine(plan.RootElement.GetProperty("contexts").GetProperty("managed")
                    .GetProperty("properties").GetProperty("ArtifactsPath").GetString()!, "dependency-builds.log");
            }
            string output = "artifacts/union-" + value;
            try
            {
                Assert.Equal(0, await repository.RunAsync(cancellation.Token, output));
            }
            catch (Exception failure)
            {
                string runDirectory = repository.PathOf(output);
                string diagnostics = string.Join("\n", Directory.Exists(runDirectory)
                    ? Directory.EnumerateFiles(runDirectory, "*.log").Select(path => Path.GetFileName(path) + ":\n" + File.ReadAllText(path))
                    : new[] { "The run did not start." });
                string results = Path.Combine(runDirectory, "results.json");
                if (File.Exists(results))
                    diagnostics += "\nResults:\n" + File.ReadAllText(results);
                throw new InvalidOperationException("Union compilation failed for value " + value + ":\n"
                    + diagnostics, failure);
            }
            if (variant && value >= 3)
            {
                using JsonDocument resolved = JsonDocument.Parse(File.ReadAllText(repository.PathOf(output + "/managed-resolved-sdk-graph.json")));
                JsonElement owner = resolved.RootElement.EnumerateArray().Single(snapshot => snapshot.GetProperty("Path").GetString()!
                    .EndsWith("Inno.Input.CompilationFixture.csproj", StringComparison.Ordinal));
                Assert.Contains(owner.GetProperty("Variants").EnumerateArray(), instance =>
                    instance.GetProperty("Properties").GetProperty("TargetPath").GetString() != owner.GetProperty("Properties").GetProperty("TargetPath").GetString());
            }
            if (value == 3)
            {
                outputTimestamp = File.GetLastWriteTimeUtc(dependencyOutput);
                dependencyBytes = File.ReadAllBytes(dependencyOutput);
                dependencyBuilds = File.ReadAllLines(dependencyBuildLog).Length;
            }
            else if (value == 4)
            {
                Assert.Equal(dependencyBytes, File.ReadAllBytes(dependencyOutput));
                if (!variant)
                {
                    Assert.True(outputTimestamp == File.GetLastWriteTimeUtc(dependencyOutput),
                        "An unchanged dependency was rebuilt. Clean log:\n" + File.ReadAllText(repository.PathOf(output + "/managed-clean.log")));
                    Assert.Equal(dependencyBuilds, File.ReadAllLines(dependencyBuildLog).Length);
                }
                Assert.Equal(0, await repository.EvidenceAsync(repository.PathOf(output + "/results.json")));
                if (variant)
                {
                    using DocumentationSnapshot reader = await DocumentationStore.OpenAsync(documentation.cache, documentationSnapshot.snapshot);
                    DocumentationCatalogReport catalog = await reader.ExportCatalogAsync();
                    string reference = catalog.references.Single(item => item.model == "source-clr").identity;
                    File.WriteAllText(Path.Combine(documentation.cache, "references", reference, "reference.json"), "damaged immutable source facts");
                    InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf(output + "/results.json")));
                    Assert.Contains("integrity mismatch", failure.Message);
                }
            }
        }
    }

    [Fact]
    public async Task ChangedCompilationInputsCannotPublishReusableBuildIdentity()
    {
        using var repository = new ValidationRepository(TestRepository.GetRoot(typeof(ValidationCompletionTests).Assembly));
        // Five real SDK validation cycles exercise identity correctness; their aggregate duration is not a latency contract.
        // Keep a generous runaway safeguard without canceling a healthy final build on a slower hosted runner.
        using var cancellation = new CancellationTokenSource(TimeSpan.FromHours(1));
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
        Assert.Equal(0, await repository.RunCurrentAsync(cancellation.Token, "artifacts/warmup"));
        repository.Write(C_PROBE, File.ReadAllText(source) + "\n// Change this owner's compilation input.\n");
        byte[] original = File.ReadAllBytes(source);
        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        File.WriteAllText(gate, "mutate after planning");
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunCurrentAsync(cancellation.Token, "artifacts/run"));
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
        Assert.Equal(0, await repository.RunCurrentAsync(cancellation.Token, "artifacts/repaired"));
        Assert.Equal(0, await repository.EvidenceAsync(repository.PathOf("artifacts/repaired/results.json"), "artifacts/repaired/plan.json"));

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
        repository.Write("README.md", File.ReadAllText(repository.PathOf("README.md")) + "\nClosing evidence acceptance.\n");
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
