using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Testing.Validation;
using Inno.Tooling.Validation;

namespace Inno.Tooling.Validation.Tests;

[Collection(nameof(ValidationPlanFixture))]
public sealed class ValidationEvidenceIdentityTests : ValidationPlanFixture
{
    [Fact]
    public async Task SdkSelectedReferencePackContentInvalidatesEvidenceDespitePreservedTimestamp()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        string packs = external.PathOf("packs");
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><NetCoreTargetingPackRoot>" + SecurityElement.Escape(packs)
                + "</NetCoreTargetingPackRoot></PropertyGroup>"
            )
        );
        using JsonDocument initial = await repository.PlanAsync();
        JsonElement owner = initial.RootElement.GetProperty("contexts")
            .GetProperty("managed")
            .GetProperty("sdkGraph")
            .EnumerateArray()
            .Single(
                project => (
                    repository.BindWorkspace(
                        project.GetProperty("Path").GetString()!
                    ) == repository.PathOf("src/Example/Example.csproj")
                )
            );
        string path = Path.Combine(Assert.Single(owner.GetProperty("FrameworkPacks").EnumerateArray()).GetString()!, "reference.dll");
        external.Write(path, "first");
        using JsonDocument plan = await repository.PlanAsync();
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        external.Write(path, "other");
        File.SetLastWriteTimeUtc(path, timestamp);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(
            () => repository.EvidenceAsync(repository.PathOf("artifacts/unread-evidence.json"))
        );
        Assert.Contains("another SDK or execution environment", failure.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("tests/Example.Tests/Changed.cs", "global using System;")]
    [InlineData("docs/Example.cs.txt", "global using System;")]
    [InlineData("Shared.props", "<Project><PropertyGroup><ImplicitUsings>enable</ImplicitUsings></PropertyGroup></Project>")]
    public async Task RepositoryWideSourcePolicyInputsParticipateInEvidenceIdentity(
        string path,
        string content
    ) {
        using var repository = new ValidationRepository();
        repository.Write(path, content);
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement source = plan.RootElement
            .GetProperty("checks")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "architecture");
        Assert.Contains(source.GetProperty("inputs").EnumerateArray(), input => input.GetString() == path);
        Assert.True(plan.RootElement.GetProperty("inputs").TryGetProperty(path, out _));
    }

    [Fact]
    public async Task ExternalSourceContentInvalidatesEvidenceEvenWhenLengthAndTimestampMatch()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        string path = external.PathOf("Linked.cs");
        const string C_ORIGINAL = "namespace Example; public class Linked { public const int Value = 1; }";
        external.Write("Linked.cs", C_ORIGINAL);
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project(
                "<ItemGroup><Compile Include=\"" + SecurityElement.Escape(path) + "\" /></ItemGroup>"
            )
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare external source input");
        using JsonDocument plan = await repository.PlanAsync();
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        external.Write("Linked.cs", C_ORIGINAL.Replace("= 1", "= 2", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(path, timestamp);
        Assert.Equal(C_ORIGINAL.Length, new FileInfo(path).Length);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(
            () => repository.EvidenceAsync(repository.PathOf("artifacts/unread-evidence.json"))
        );
        Assert.Contains("another SDK or execution environment", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ExternalImportContentInvalidatesEvidenceEvenWhenLengthAndTimestampMatch()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        string path = external.PathOf("Shared.props");
        const string C_ORIGINAL = "<Project><PropertyGroup><WarningLevel>4</WarningLevel></PropertyGroup></Project>";
        external.Write("Shared.props", C_ORIGINAL);
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project("<Import Project=\"" + SecurityElement.Escape(path) + "\" />")
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare external SDK import");
        using JsonDocument plan = await repository.PlanAsync();
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        external.Write("Shared.props", C_ORIGINAL.Replace(">4<", ">3<", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(path, timestamp);
        Assert.Equal(C_ORIGINAL.Length, new FileInfo(path).Length);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(
            () => repository.EvidenceAsync(repository.PathOf("artifacts/unread-evidence.json"))
        );
        Assert.Contains("another SDK or execution environment", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ClosingFreshnessRejectsBothSourceAndExternalChangesDuringChecks()
    {
        using var repository = new ValidationRepository();
        using var external = new ValidationRepository();
        const string C_ORIGINAL = "<Project><PropertyGroup><WarningLevel>4</WarningLevel></PropertyGroup></Project>";
        external.Write("Shared.props", C_ORIGINAL);
        repository.Write("Shared.props", C_ORIGINAL);
        string dependency = external.PathOf("Shared.props");
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project("<Import Project=\"" + SecurityElement.Escape(dependency) + "\" />")
        );
        string script = string.Join(
            '\n',
            new[] { repository.PathOf("Shared.props"), dependency }.Select(path => (
                "$path = '" + path.Replace("'", "''", StringComparison.Ordinal) + "'; "
                + "$stamp = [IO.File]::GetLastWriteTimeUtc($path); "
                + "[IO.File]::WriteAllText($path, [IO.File]::ReadAllText($path).Replace('>4<', '>3<')); "
                + "[IO.File]::SetLastWriteTimeUtc($path, $stamp)"
            ))
        );
        repository.Write("tools/ci/Test-AgentRules.ps1", script);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare check-time source and environment mutations");
        using JsonDocument plan = await repository.PlanAsync();
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument evidence = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        JsonElement[] results = evidence.RootElement.GetProperty("results").EnumerateArray().ToArray();
        Assert.Contains(results, result => (
            result.GetProperty("id").GetString() == "instructions"
            && result.GetProperty("status").GetString() == "Passed"
        ));
        foreach (string id in new[] { "input-stability", "environment-stability" })
        {
            Assert.Contains(
                evidence.RootElement.GetProperty("failures").EnumerateArray(),
                fault => fault.GetProperty("stage").GetString() == id
            );
        }
    }

    [Fact]
    public async Task MissingOrStaleResultsCannotBecomeGreenEvidence()
    {
        using var repository = new ValidationRepository();
        using JsonDocument plan = await repository.PlanAsync();
        string evidence = repository.PathOf("artifacts/empty.json");
        File.WriteAllText(
            evidence,
            JsonSerializer.Serialize(new
            {
                commit = plan.RootElement.GetProperty("commit").GetString(),
                tree = plan.RootElement.GetProperty("tree").GetString(),
                environment = plan.RootElement.GetProperty("environment").GetString(),
                started = DateTimeOffset.UtcNow,
                results = Array.Empty<object>()
            })
        );
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(evidence));
        repository.Write("tests/Example.Tests/Changed.cs", "namespace Example.Tests; public sealed class Changed { }");
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(evidence));
    }

    [Fact]
    public async Task ExecutionFailureRetainsAnExactPlanReceiptAndCannotCertifySuccess()
    {
        using var repository = new ValidationRepository();
        using JsonDocument plan = await repository.PlanAsync();
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        Assert.Contains(
            results.RootElement.GetProperty("results").EnumerateArray(),
            result => result.GetProperty("status").GetString() == "Failed"
        );
        Assert.True(results.RootElement.GetProperty("completed").GetBoolean());
        string identity = Convert.ToHexStringLower(
            SHA256.HashData(File.ReadAllBytes(repository.PathOf("artifacts/run/plan.json")))
        );
        Assert.Equal(identity, results.RootElement.GetProperty("planIdentity").GetString());
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/run/results.json")));
    }

    [Fact]
    public async Task ACompetingOutputWriterCanCancelWithoutAdoptingOrReleasingAnotherOwner()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Change.cs", "namespace Example; public sealed class Change { }");
        using JsonDocument plan = await repository.PlanAsync();
        string artifacts = repository.BindWorkspace(plan.RootElement.GetProperty("artifacts").GetString()!);
        string parent = Path.GetDirectoryName(artifacts)!;
        using FileLease writer = await FileLease.AcquireAsync(artifacts + ".lock", TimeSpan.FromSeconds(5));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        Task<int> competing = repository.RunAsync(cancellationToken: cancellation.Token);
        while (!BuildCacheOwnerCatalog.ReadOwners(repository.PathOf(".")).Any(owner => owner.directory == parent))
        {
            cancellation.Token.ThrowIfCancellationRequested();
            Assert.False(competing.IsCompleted);
            await Task.Delay(20, cancellation.Token);
        }
        Assert.False(competing.IsCompleted);
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await competing);
        Assert.False(Directory.Exists(artifacts));
        Assert.False(File.Exists(repository.PathOf("artifacts/run/results.json")));
        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            using FileLease unavailable = await FileLease.AcquireAsync(artifacts + ".lock", TimeSpan.Zero);
        });
    }

    [Fact]
    public async Task ExistingUnregisteredBuildOutputCannotBeSilentlyAdopted()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Change.cs", "namespace Example; public sealed class Change { }");
        using JsonDocument plan = await repository.PlanAsync();
        string artifacts = repository.BindWorkspace(plan.RootElement.GetProperty("artifacts").GetString()!);
        Directory.CreateDirectory(artifacts);
        string unrelated = Path.Combine(artifacts, "unrelated-owner.txt");
        File.WriteAllText(unrelated, "preserve this owner");
        await Assert.ThrowsAsync<InvalidOperationException>(() => repository.RunAsync());
        Assert.Equal("preserve this owner", File.ReadAllText(unrelated));
        Assert.False(File.Exists(repository.PathOf("artifacts/run/results.json")));
    }

    [Fact]
    public async Task AnUninitializedSubmoduleDoesNotBorrowItsParentRepositoryIdentity()
    {
        using var repository = new ValidationRepository();
        using JsonDocument initial = await repository.PlanAsync();
        string revision = initial.RootElement.GetProperty("commit").GetString()!;
        repository.Git("update-index", "--add", "--cacheinfo", "160000," + revision + ",extern/Uninitialized");
        repository.Git("commit", "-m", "Declare unavailable native source");
        Directory.CreateDirectory(repository.PathOf("extern/Uninitialized"));
        repository.Write("tests/Example.Tests/Change.cs", "// Domain input does not require native source preparation.");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Equal("unavailable:" + revision, plan.RootElement.GetProperty("inputs").GetProperty("extern/Uninitialized").GetString());
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains("tests/Example.Tests/Example.Tests.csproj", Owners(plan));
    }

    [Fact]
    public async Task CachedSubmoduleStatusCannotCertifyMutatedNativeSource()
    {
        using var source = new ValidationRepository();
        using var repository = new ValidationRepository();
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/Before");
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/Native");
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/After");
        repository.Git("commit", "-am", "Pin native source fixture");
        repository.Git("-C", "extern/Native", "update-index", "--assume-unchanged", "fixtures/plugin.txt");
        string path = repository.PathOf("extern/Native/fixtures/plugin.txt");
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        long length = new FileInfo(path).Length;
        File.WriteAllText(path, "mutated input");
        File.SetLastWriteTimeUtc(path, timestamp);
        Assert.Equal(length, new FileInfo(path).Length);
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains("uncommitted inputs", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task IgnoredCompilerInputsCannotDisappearFromValidationIdentity()
    {
        using var repository = new ValidationRepository();
        File.AppendAllText(repository.PathOf(".gitignore"), "Local.cs\n");
        repository.Git("commit", "-am", "Ignore a local file");
        repository.Write("src/Example/Local.cs", "namespace Example; public class Local { }\n");
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains("untracked ignored compiler input", failure.Message, StringComparison.Ordinal);
        Assert.Contains("src/Example/Local.cs", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SubmoduleLinksAreVerifiedAsLinkTextWithoutFollowingTheirTargets()
    {
        using var source = new ValidationRepository();
        source.Write("link.txt", "../../outside-owner.txt");
        string blob = source.Git("hash-object", "-w", "link.txt");
        source.Git("update-index", "--add", "--cacheinfo", "120000," + blob + ",link.txt");
        source.Git("commit", "-m", "Record a link without importing its target");
        using var repository = new ValidationRepository();
        repository.Git("-c", "protocol.file.allow=always", "submodule", "add", source.PathOf("."), "extern/Native");
        repository.Git("commit", "-am", "Pin source with a symbolic link");
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project(
                "<ItemGroup><Compile Include=\"../../extern/Native/src/Probe.cs\" /></ItemGroup>"
            )
        );
        source.Write("src/Probe.cs", "namespace Native; public class Probe { }");
        source.Git("add", "src/Probe.cs");
        source.Git("commit", "-m", "Declare a compiler input");
        repository.Git("-C", "extern/Native", "fetch", "origin");
        repository.Git("-C", "extern/Native", "checkout", "origin/master");
        repository.Git("add", "extern/Native");
        repository.Git("commit", "-m", "Update compiler input owner");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.Contains(
            plan.RootElement.GetProperty("inputSets").GetProperty("src/Example/Example.csproj").EnumerateArray(),
            input => input.GetString() == "extern/Native"
        );
        Assert.Contains(
            plan.RootElement.GetProperty("inputSets").GetProperty("native-sources").EnumerateArray(),
            input => input.GetString() == "extern/Native"
        );
        string path = repository.PathOf("extern/Native/link.txt");
        repository.Git("-C", "extern/Native", "update-index", "--assume-unchanged", "link.txt");
        if (new FileInfo(path).LinkTarget is not null)
        {
            File.Delete(path);
            File.CreateSymbolicLink(path, "../../another-owner.txt");
        }
        else
        {
            File.WriteAllText(path, "../../another-owner.txt");
        }
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains("uncommitted inputs", failure.Message, StringComparison.Ordinal);
    }
}
