using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Integration.Tests;

public sealed class EditorJourneyProtocolTests
{
    [Fact]
    public async Task ReportsTheOutcomeOfEveryInteractionAndWindowLifecycle()
    {
        string engine = TestRepository.GetRoot(typeof(EditorJourneyProtocolTests).Assembly);
        using var repository = new ValidationRepository();
        string root = repository.PathOf(".");
        string fixture = Path.Combine(engine, "tests/tools/Inno.Tooling.Validation.Integration.Tests/fixtures/editor-journey");
        repository.Write(
            "Fixture/Fixture.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><OutputType>Exe</OutputType><ImplicitUsings>enable</ImplicitUsings></PropertyGroup>"
            )
        );
        repository.Write("Fixture/Program.cs", File.ReadAllText(Path.Combine(fixture, "Program.cs.txt")));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(6));
        await repository.SdkAsync(["build", "Fixture/Fixture.csproj", "--disable-build-servers"]);
        string app = repository.PathOf("Fixture/bin/Debug/net10.0/Fixture.dll");
        string project = Path.Combine(engine, "tests/product/Inno.Testing.EditorJourney/Inno.Testing.EditorJourney.csproj");
        string products = repository.PathOf("artifacts/journey");
        await repository.SdkAsync(["build", project, "--artifacts-path", products, "--disable-build-servers"]);
        string journey = (await ToolchainEnvironment.CaptureOutputAsync(repository.dotnet, [
            "msbuild",
            project,
            "-nologo",
            "-p:ArtifactsPath="
            + products,
            "-p:Configuration=Debug",
            "-getProperty:TargetPath"
        ], engine, cancellationToken: cancellation.Token)).Trim();
        string cocoa = repository.PathOf("CocoaFixture");
        if (OperatingSystem.IsMacOS())
        {
            await ToolchainEnvironment.RunAsync("/usr/bin/xcrun", [
                "clang",
                "-fobjc-arc",
                "-framework",
                "AppKit",
                Path.Combine(fixture, "CocoaFixture.m"),
                "-o",
                cocoa
            ], root, cancellationToken: cancellation.Token);
        }
        (string name, string scenario, string? diagnostic)[] cases =
        [
            ("valid", "Workflows", null),
            ("missing-evidence", "Workflows", "did not complete the journey"),
            ("missing-stage", "Workflows", "Expected eight completed workflow stages"),
            ("failed-process", "Workflows", "exited with code 3"),
            ("canceled-process", "Workflows", "exited with code 2"),
            ("missing-retirement", "Workflows", "did not retire its real product resources"),
            ("teardown-failure", "Workflows", "did not retire its real product resources"),
            ("wrong-notification", "Workflows", "different journey"),
            ("timeout", "Workflows", "exceeded its deadline"),
            ("extension-failure", "Workflows", "The source extension failed: Original source failure"),
            ("valid", "Lifecycle", null),
            ("missing-cycle", "Lifecycle", "must complete one open/close cycle"),
            ("appkit-exit", "Workflows", null),
            ("appkit-retirement-failure", "Workflows", "did not retire its real product resources"),
            ("appkit-failed-process", "Workflows", "exited with code 2")
        ];
        foreach ((string name, string scenario, string? diagnostic) in cases)
        {
            bool appKit = name.StartsWith("appkit-", StringComparison.Ordinal);
            if (appKit && !OperatingSystem.IsMacOS())
            {
                continue;
            }
            string output = repository.PathOf("artifacts/" + scenario + "-" + name);
            var env = new System.Collections.Generic.Dictionary<string, string?>
            {
                ["INNO_JOURNEY_PROTOCOL_CASE"] = name,
                ["INNO_JOURNEY_PROTOCOL_SCENARIO"] = scenario
            };
            Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(
                repository.dotnet,
                [
                    journey,
                    scenario,
                    "--editor",
                    app,
                    "--engine-root",
                    engine,
                    "--output",
                    output,
                    "--dotnet",
                    appKit
                    ? cocoa
                    : repository.dotnet,
                    "--graphics-api",
                    "ProtocolFixture",
                    "--timeout-seconds",
                    appKit ? "30" : name == "timeout" ? "1" : "8"
                ],
                engine,
                env,
                cancellation.Token
            ));
            using JsonDocument result = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(output, "result.json")));
            Assert.Equal(diagnostic == null ? "Passed" : "Failed", result.RootElement.GetProperty("status").GetString());
            if (diagnostic == null)
            {
                Assert.Null(failure);
            }
            else
            {
                Assert.NotNull(failure);
                Assert.Contains(diagnostic, result.RootElement.GetProperty("diagnostic").GetString());
            }
        }
    }
}
