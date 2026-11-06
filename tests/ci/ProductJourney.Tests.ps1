$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$owner = [IO.Path]::GetFullPath((Join-Path ([IO.Path]::GetTempPath()) 'InnoJourneyContracts'))
$root = Join-Path $owner ([Guid]::NewGuid().ToString('N'))
$dotnet = if ($env:DOTNET_HOST_PATH) { $env:DOTNET_HOST_PATH } else { (Get-Command dotnet).Source }
$pwsh = (Get-Command pwsh).Source
$null = New-Item -ItemType Directory -Path $root
try {
    [IO.File]::WriteAllText((Join-Path $root 'Fixture.csproj'), '<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net10.0</TargetFramework><OutputType>Exe</OutputType><ImplicitUsings>enable</ImplicitUsings></PropertyGroup></Project>')
    [IO.File]::WriteAllText((Join-Path $root 'Program.cs'), @'
string project = args[0];
string mode = Environment.GetEnvironmentVariable("INNO_JOURNEY_PROTOCOL_CASE")!;
string scenario = Environment.GetEnvironmentVariable("INNO_JOURNEY_PROTOCOL_SCENARIO")!;
int cycles = int.Parse(Environment.GetEnvironmentVariable("INNO_ACCEPTANCE_PANEL_CYCLES")!);
string evidence = Path.Combine(project, "Library", "Acceptance");
Directory.CreateDirectory(evidence);
Directory.CreateDirectory(Path.Combine(project, "Logs"));
File.WriteAllText(Path.Combine(project, "Logs", "EditorBoot.log"), mode == "missing-retirement" ? "Starting" :
    mode == "teardown-failure" ? "Dispose end.\nTeardown failure" : "Dispose end.");
if (mode == "timeout") { Thread.Sleep(Timeout.Infinite); return 0; }
if (mode == "extension-failure")
{
    File.WriteAllText(Path.Combine(evidence, "workflow-failure.txt"), "Original source failure");
    Thread.Sleep(Timeout.Infinite);
}
if (mode != "missing-evidence")
{
    if (scenario == "Workflows")
        File.WriteAllLines(Path.Combine(evidence, "workflow-journey.tsv"),
            Enumerable.Range(0, mode == "missing-stage" ? 7 : 8).Select(index => "Verified stage " + index));
    else
    {
        File.WriteAllLines(Path.Combine(evidence, "panel-lifecycle.tsv"),
            new[] { "panel\tcompleted-open-close-cycles", "protocol\tfixture" }.Concat(
                Enumerable.Range(0, 9).Select(index => $"panel-{index}\t{(mode == "missing-cycle" && index == 0 ? cycles - 1 : cycles)}")));
        File.WriteAllLines(Path.Combine(evidence, "panel-lifecycle-preparation.tsv"),
            new[] { "phase", "before-first-panel-draw", mode == "missing-preparation" ? "wrong" : "after-120-drawn-frames" });
        int count = mode == "missing-sample" ? cycles / 10 : cycles / 10 + 1;
        File.WriteAllLines(Path.Combine(evidence, "panel-lifecycle-resources.tsv"),
            new[] { "completed-cycles\tmanaged-bytes-after-full-gc\tpending-events" }.Concat(
                Enumerable.Range(0, count).Select(index => $"{index * 10}\t{(mode == "growth" && index == count - 1 ? 5 * 1024 * 1024 : 0)}\t{(mode == "pending-events" ? 1 : 0)}")));
    }
}
if (mode == "wrong-notification")
{
    File.WriteAllText(Path.Combine(evidence, "journey-completed.txt"), "Lifecycle");
    Thread.Sleep(30000);
}
return mode == "failed-process" ? 3 : mode == "canceled-process" ? 2 : 0;
'@)
    & $dotnet build (Join-Path $root 'Fixture.csproj') --configuration Debug --disable-build-servers -m:1 -nr:false *> (Join-Path $root 'build.log')
    if ($LASTEXITCODE -ne 0) { throw "Protocol fixture build failed:`n$(Get-Content (Join-Path $root 'build.log') -Raw)" }
    $cocoa = Join-Path $root 'CocoaFixture'
    if ($IsMacOS) {
        # Exercise the real AppKit quit transport without building an engine or installing a fake signal handler.
        [IO.File]::WriteAllText((Join-Path $root 'CocoaFixture.m'), @'
#import <AppKit/AppKit.h>
#include <stdio.h>
#include <stdlib.h>

static BOOL quitRequested = NO;

@interface JourneyApplication : NSApplication
@end

@implementation JourneyApplication
- (void)terminate:(id)sender
{
    quitRequested = YES;
}
@end

static void Write(NSString *path, NSString *text)
{
    NSError *failure = nil;
    if (![text writeToFile:path atomically:YES encoding:NSUTF8StringEncoding error:&failure]) {
        fprintf(stderr, "%s\n", failure.localizedDescription.UTF8String);
        exit(4);
    }
}

int main(int argc, const char *argv[])
{
    @autoreleasepool {
        if (argc < 3) return 4;
        NSString *project = [NSString stringWithUTF8String:argv[2]];
        NSString *mode = [NSString stringWithUTF8String:getenv("INNO_JOURNEY_PROTOCOL_CASE")];
        NSString *evidence = [project stringByAppendingPathComponent:@"Library/Acceptance"];
        NSString *logs = [project stringByAppendingPathComponent:@"Logs"];
        for (NSString *directory in @[evidence, logs]) {
            NSError *failure = nil;
            if (![[NSFileManager defaultManager] createDirectoryAtPath:directory withIntermediateDirectories:YES attributes:nil error:&failure]) {
                fprintf(stderr, "%s\n", failure.localizedDescription.UTF8String);
                return 4;
            }
        }
        Write([logs stringByAppendingPathComponent:@"EditorBoot.log"], @"Starting");
        NSMutableString *stages = [NSMutableString string];
        for (int index = 0; index < 8; index++) [stages appendFormat:@"Verified stage %d\n", index];
        Write([evidence stringByAppendingPathComponent:@"workflow-journey.tsv"], stages);
        [JourneyApplication sharedApplication];
        [NSApp setActivationPolicy:NSApplicationActivationPolicyAccessory];
        [NSApp finishLaunching];
        Write([evidence stringByAppendingPathComponent:@"journey-completed.txt"], @"Workflows");
        while (!quitRequested) {
            @autoreleasepool {
                NSEvent *event = [NSApp nextEventMatchingMask:NSEventMaskAny untilDate:[NSDate dateWithTimeIntervalSinceNow:0.1]
                                                     inMode:NSDefaultRunLoopMode dequeue:YES];
                if (event) [NSApp sendEvent:event];
            }
        }
        Write([logs stringByAppendingPathComponent:@"EditorBoot.log"],
              [mode isEqualToString:@"appkit-retirement-failure"] ? @"Dispose end.\nTeardown failure" : @"Dispose end.");
        return [mode isEqualToString:@"appkit-failed-process"] ? 2 : 0;
    }
}
'@)
        & /usr/bin/xcrun clang -fobjc-arc -framework AppKit (Join-Path $root 'CocoaFixture.m') -o $cocoa *> (Join-Path $root 'cocoa-build.log')
        if ($LASTEXITCODE -ne 0) { throw "AppKit protocol fixture build failed:`n$(Get-Content (Join-Path $root 'cocoa-build.log') -Raw)" }
    }
    $source = Join-Path $root 'Source'
    $null = New-Item -ItemType Directory -Path (Join-Path $source 'Assets')
    $cases = @(
        @{ name='valid'; scenario='Workflows'; cycles=100; success=$true },
        @{ name='missing-evidence'; scenario='Workflows'; cycles=100; reason='did not complete the journey' },
        @{ name='missing-stage'; scenario='Workflows'; cycles=100; reason='Expected eight completed workflow stages' },
        @{ name='failed-process'; scenario='Workflows'; cycles=100; reason='exited with code 3' },
        @{ name='canceled-process'; scenario='Workflows'; cycles=100; reason='exited with code 2' },
        @{ name='missing-retirement'; scenario='Workflows'; cycles=100; reason='did not retire its real product resources' },
        @{ name='teardown-failure'; scenario='Workflows'; cycles=100; reason='did not retire its real product resources' },
        @{ name='wrong-notification'; scenario='Workflows'; cycles=100; reason='different journey' },
        @{ name='timeout'; scenario='Workflows'; cycles=100; reason='exceeded its deadline' },
        @{ name='extension-failure'; scenario='Workflows'; cycles=100; reason='The source extension failed: Original source failure' },
        @{ name='valid'; scenario='Lifecycle'; cycles=100; success=$true },
        @{ name='valid'; scenario='Lifecycle'; cycles=1000; success=$true },
        @{ name='missing-cycle'; scenario='Lifecycle'; cycles=100; reason='must complete 100 open/close cycles' },
        @{ name='missing-preparation'; scenario='Lifecycle'; cycles=100; reason='record cold resource preparation separately' },
        @{ name='missing-sample'; scenario='Lifecycle'; cycles=100; reason='complete full-GC resource series' },
        @{ name='growth'; scenario='Lifecycle'; cycles=100; reason='beyond its 4 MiB final live-memory budget' },
        @{ name='pending-events'; scenario='Lifecycle'; cycles=100; reason='retained pending events' }
    )
    if ($IsMacOS) {
        $cases += @(
            @{ name='appkit-exit'; scenario='Workflows'; cycles=100; success=$true },
            @{ name='appkit-retirement-failure'; scenario='Workflows'; cycles=100; reason='did not retire its real product resources' },
            @{ name='appkit-failed-process'; scenario='Workflows'; cycles=100; reason='exited with code 2' }
        )
    }
    $oldCase = $env:INNO_JOURNEY_PROTOCOL_CASE
    $oldScenario = $env:INNO_JOURNEY_PROTOCOL_SCENARIO
    try {
        foreach ($case in $cases) {
            $env:INNO_JOURNEY_PROTOCOL_CASE = $case.name
            $env:INNO_JOURNEY_PROTOCOL_SCENARIO = $case.scenario
            $output = Join-Path $root "$($case.scenario)-$($case.name)-$($case.cycles)"
            $hostExecutable = if ($case.name.StartsWith('appkit-', [StringComparison]::Ordinal)) { $cocoa } else { $dotnet }
            $deadline = if ($hostExecutable -ceq $cocoa) { 30 } else { 5 }
            & $pwsh -NoProfile -File (Join-Path $repository 'tests/Invoke-EditorUiProductJourney.ps1') `
                -EditorAssembly (Join-Path $root 'bin/Debug/net10.0/Fixture.dll') -ProjectDirectory $source `
                -ResultsDirectory $output -Dotnet $hostExecutable -GraphicsApi ProtocolFixture -Scenario $case.scenario `
                -Cycles $case.cycles -TimeoutSeconds $deadline *> "$output.log"
            $code = $LASTEXITCODE
            $result = Get-Content -LiteralPath (Join-Path $output 'result.json') -Raw | ConvertFrom-Json
            if ($case.success) {
                if ($code -ne 0 -or $result.status -ne 'Passed') { throw "Required protocol acceptance failed: $output`n$(Get-Content "$output.log" -Raw)" }
            } elseif ($code -eq 0 -or $result.status -ne 'Failed' -or $result.diagnostic -notmatch [regex]::Escape($case.reason)) {
                throw "The protocol case did not fail for its actual reason: $output`n$(Get-Content "$output.log" -Raw)"
            }
        }
    } finally {
        $env:INNO_JOURNEY_PROTOCOL_CASE = $oldCase
        $env:INNO_JOURNEY_PROTOCOL_SCENARIO = $oldScenario
    }
    Write-Output "Product journey protocol: $($cases.Count) cases passed; real product execution remains separate evidence."
} finally {
    $resolved = [IO.Path]::GetFullPath($root)
    if (-not $resolved.StartsWith($owner + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) { throw 'The protocol fixture escaped its cleanup owner.' }
    Remove-Item -LiteralPath $resolved -Recurse -Force
}
exit 0
