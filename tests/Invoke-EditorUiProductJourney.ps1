param(
    [Parameter(Mandatory)][string]$EditorAssembly,
    [Parameter(Mandatory)][string]$ProjectDirectory,
    [Parameter(Mandatory)][string]$ResultsDirectory,
    [string]$Dotnet = 'dotnet',
    [Parameter(Mandatory)][string]$GraphicsApi,
    [ValidateSet('Workflows', 'Lifecycle')][string]$Scenario = 'Workflows',
    [int]$Frames = 30000,
    [int]$TimeoutSeconds = 600,
    [ValidateRange(100, 10000)][int]$Cycles = 100
)

$ErrorActionPreference = 'Stop'

function Request-EditorJourneyExit([Diagnostics.Process]$OwnedProcess) {
    if ($OwnedProcess.HasExited) { return $false }
    if ($IsMacOS) {
        # Request the owned Cocoa application's normal quit event; process signals can bypass SDL and retirement.
        $quit = @'
use framework "AppKit"
on run arguments
    set ownedApplication to current application's NSRunningApplication's runningApplicationWithProcessIdentifier:((item 1 of arguments) as integer)
    if ownedApplication is missing value then error "The owned Editor has no running Cocoa application."
    if not ((ownedApplication's terminate()) as boolean) then error "The owned Editor refused its normal quit request."
end run
'@
        & /usr/bin/osascript -e $quit -- ([string]$OwnedProcess.Id)
        if ($LASTEXITCODE -ne 0) {
            if ($OwnedProcess.HasExited) { return $false }
            throw 'The owned Editor did not accept its orderly quit request.'
        }
        return $true
    }
    if (-not $IsWindows) { throw 'This product journey has no orderly exit transport for the current host.' }
    if (-not ('Inno.Validation.EditorJourneyExit' -as [type])) {
        Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
namespace Inno.Validation;
public static class EditorJourneyExit
{
    private delegate bool WindowCallback(IntPtr window, IntPtr state);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EnumWindows(WindowCallback callback, IntPtr state);
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr window, out uint process);
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetWindowText(IntPtr window, StringBuilder text, int maximum);
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool PostMessage(IntPtr window, uint message, IntPtr first, IntPtr second);
    public static bool Request(int process)
    {
        var windows = new List<IntPtr>();
        if (!EnumWindows((window, state) =>
        {
            GetWindowThreadProcessId(window, out uint owner);
            if (owner == (uint)process)
            {
                var title = new StringBuilder(256);
                GetWindowText(window, title, title.Capacity);
                if (title.ToString() == "Inno Editor")
                    windows.Add(window);
            }
            return true;
        }, IntPtr.Zero))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        if (windows.Count == 0)
            return false;
        if (windows.Count != 1)
            throw new InvalidOperationException("The owned Editor has an ambiguous primary window.");
        GetWindowThreadProcessId(windows[0], out uint currentOwner);
        if (currentOwner != (uint)process)
            throw new InvalidOperationException("The Editor window ownership changed before close.");
        if (!PostMessage(windows[0], 0x0010, IntPtr.Zero, IntPtr.Zero))
            throw new Win32Exception(Marshal.GetLastWin32Error());
        return true;
    }
}
'@
    }
    return [Inno.Validation.EditorJourneyExit]::Request($OwnedProcess.Id)
}

function Assert-JourneyEvidence([string]$Project, [string]$Scenario, [int]$Cycles) {
    $evidence = Join-Path $Project $(if ($Scenario -eq 'Lifecycle') { 'Library/Acceptance/panel-lifecycle.tsv' } else { 'Library/Acceptance/workflow-journey.tsv' })
    if (-not (Test-Path -LiteralPath $evidence)) { throw 'The source extension did not complete the journey.' }
    $verified = @(Get-Content -LiteralPath $evidence)
    if ($Scenario -eq 'Workflows') {
        if ($verified.Count -ne 8) { throw "Expected eight completed workflow stages, got $($verified.Count)." }
    } else {
        $panels = @(Import-Csv -LiteralPath $evidence -Delimiter "`t" | Where-Object panel -ne 'protocol')
        if ($panels.Count -lt 9 -or @($panels | Where-Object 'completed-open-close-cycles' -ne ([string]$Cycles)).Count -ne 0) {
            throw "Every actual product panel must complete $Cycles open/close cycles."
        }
        $resources = @(Import-Csv -LiteralPath (Join-Path $Project 'Library/Acceptance/panel-lifecycle-resources.tsv') -Delimiter "`t")
        $preparation = @(Import-Csv -LiteralPath (Join-Path $Project 'Library/Acceptance/panel-lifecycle-preparation.tsv') -Delimiter "`t")
        if ($preparation.Count -ne 2 -or $preparation[0].phase -ne 'before-first-panel-draw' -or $preparation[1].phase -ne 'after-120-drawn-frames') {
            throw 'The lifecycle run must record cold resource preparation separately from its repeated open/close baseline.'
        }
        if ($resources.Count -ne ($Cycles / 10 + 1) -or [int]$resources[0].'completed-cycles' -ne 0 -or [int]$resources[-1].'completed-cycles' -ne $Cycles) {
            throw 'The lifecycle run did not record its complete full-GC resource series.'
        }
        $previousCollection = 0L
        foreach ($sample in $resources) {
            $collection = [long]$sample.'gc-index'
            $heap = [long]$sample.'gc-heap-bytes'
            $fragmented = [long]$sample.'gc-fragmented-bytes'
            if ($collection -le $previousCollection -or [int]$sample.'gc-generation' -ne [GC]::MaxGeneration -or
                $sample.'gc-compacted' -cne 'True' -or $sample.'gc-concurrent' -cne 'False' -or
                $fragmented -lt 0 -or $heap -lt $fragmented -or [long]$sample.'managed-bytes-after-full-gc' -ne ($heap - $fragmented)) {
                throw 'The lifecycle run did not record a fresh compacting full-GC snapshot with consistent live-memory bytes.'
            }
            $previousCollection = $collection
        }
        $growth = [long]$resources[-1].'managed-bytes-after-full-gc' - [long]$resources[0].'managed-bytes-after-full-gc'
        if ($growth -gt 4MB) { throw "Panel lifecycle retained $growth bytes beyond its 4 MiB final live-memory budget." }
        if (@($resources | Where-Object { [long]$_.'pending-events' -ne 0 }).Count -ne 0) {
            throw 'Panel lifecycle retained pending events after closed-frame reconciliation.'
        }
    }
}

if ($Frames -lt 120 -or $TimeoutSeconds -le 0) { throw 'The journey needs at least 120 frames and a positive deadline.' }
if ($Cycles % 10 -ne 0) { throw 'Lifecycle sampling requires a whole number of ten-cycle intervals.' }
$repository = Split-Path $PSScriptRoot -Parent
$source = [IO.Path]::GetFullPath($ProjectDirectory)
$editor = [IO.Path]::GetFullPath($EditorAssembly)
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
if (-not (Test-Path -LiteralPath $editor -PathType Leaf)) { throw 'Build the selected official Editor output first.' }
if (-not (Test-Path -LiteralPath (Join-Path $source 'Assets') -PathType Container)) { throw 'A complete project input is required.' }
$results = [IO.Path]::GetFullPath($ResultsDirectory)
if (Test-Path -LiteralPath $results) { throw 'Use a new result owner; prior evidence must not be overwritten.' }
New-Item -ItemType Directory -Path $results | Out-Null
$project = Join-Path $results 'Project'
New-Item -ItemType Directory -Path $project | Out-Null
foreach ($name in @('Assets', 'Plugins')) {
    $directory = Join-Path $source $name
    if (Test-Path -LiteralPath $directory) { Copy-Item -LiteralPath $directory -Destination $project -Recurse }
}
foreach ($file in Get-ChildItem -LiteralPath $source -File | Where-Object { $_.Extension -eq '.inno' }) {
    Copy-Item -LiteralPath $file.FullName -Destination $project
}
Copy-Item -LiteralPath (Join-Path $repository 'src/composition/editor/hosting/Inno.Editor.Hosting/Layout/DefaultEditorLayout.ini') `
    -Destination (Join-Path $project 'editor.ini')
$fixtures = Join-Path $project 'Assets/Acceptance'
New-Item -ItemType Directory -Force -Path $fixtures | Out-Null
$fixtureNames = if ($Scenario -eq 'Lifecycle') { @('ManagedMemorySample.editor.cs', 'PanelLifecycle.editor.cs') } else { @('WorkflowState.cs.txt', 'WorkflowJourney.editor.cs.txt') }
foreach ($name in $fixtureNames) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot "fixtures/editor-ui/$name") -Destination (Join-Path $fixtures ($name -replace '\.txt$', ''))
}
Get-ChildItem -LiteralPath $project -File -Recurse | ForEach-Object {
    [pscustomobject]@{ path = [IO.Path]::GetRelativePath($project, $_.FullName); sha256 = (Get-FileHash -LiteralPath $_.FullName).Hash }
} | ConvertTo-Json -Depth 3 | Set-Content -LiteralPath (Join-Path $results 'inputs.json') -Encoding utf8

$start = [Diagnostics.ProcessStartInfo]::new($Dotnet)
$start.UseShellExecute = $false
$start.CreateNoWindow = $true
$start.WindowStyle = [Diagnostics.ProcessWindowStyle]::Hidden
$start.RedirectStandardOutput = $true
$start.RedirectStandardError = $true
$start.WorkingDirectory = $project
$start.Environment['INNO_ACCEPTANCE_PANEL_CYCLES'] = [string]$Cycles
foreach ($argument in @($editor, $project, '--window-visible', 'false', '--smoke-frames', [string]$Frames,
                        '--graphics-api', $GraphicsApi)) { $start.ArgumentList.Add($argument) }
$process = [Diagnostics.Process]::new()
$process.StartInfo = $start
$code = $null
$failure = $null
$started = $false
$exitRequested = $false
$elapsed = [Diagnostics.Stopwatch]::StartNew()
try {
    if (-not $process.Start()) { throw 'The owned Editor process did not start.' }
    $started = $true
    $output = $process.StandardOutput.ReadToEndAsync()
    $errors = $process.StandardError.ReadToEndAsync()
    $completion = Join-Path $project 'Library/Acceptance/journey-completed.txt'
    $extensionFailure = Join-Path $project 'Library/Acceptance/workflow-failure.txt'
    while (-not $process.WaitForExit(100)) {
        if ($elapsed.Elapsed.TotalSeconds -ge $TimeoutSeconds) { throw 'The owned Editor journey exceeded its deadline.' }
        if (Test-Path -LiteralPath $extensionFailure) { throw "The source extension failed: $([IO.File]::ReadAllText($extensionFailure))" }
        if (-not $exitRequested -and (Test-Path -LiteralPath $completion)) {
            $completedScenario = [IO.File]::ReadAllText($completion)
            if ($completedScenario.Length -eq 0) { continue }
            if ($completedScenario -cne $Scenario) { throw 'The completion notification belongs to a different journey.' }
            Assert-JourneyEvidence -Project $project -Scenario $Scenario -Cycles $Cycles
            $exitRequested = Request-EditorJourneyExit -OwnedProcess $process
        }
    }
    $code = $process.ExitCode
    if ($code -ne 0) { throw "The owned Editor exited with code $code." }
    Assert-JourneyEvidence -Project $project -Scenario $Scenario -Cycles $Cycles
    $boot = Get-Content -LiteralPath (Join-Path $project 'Logs/EditorBoot.log') -Raw
    if ($boot -notmatch 'Dispose end\.' -or $boot -match 'Teardown failure') { throw 'The completed journey did not retire its real product resources successfully.' }
}
catch { $failure = $_ }
finally {
    if ($started -and -not $process.HasExited) {
        $process.Kill($true)
        if (-not $process.WaitForExit(10000)) { throw 'The owned Editor process did not terminate.' }
    }
    if ($output) {
        $output.WaitAsync([TimeSpan]::FromSeconds(10)).GetAwaiter().GetResult() |
            Set-Content -LiteralPath (Join-Path $results 'stdout.log') -Encoding utf8
    }
    if ($errors) {
        $errors.WaitAsync([TimeSpan]::FromSeconds(10)).GetAwaiter().GetResult() |
            Set-Content -LiteralPath (Join-Path $results 'stderr.log') -Encoding utf8
    }
    $process.Dispose()
    $elapsed.Stop()
    [pscustomobject]@{
        revision = (& git -C $repository rev-parse HEAD).Trim()
        workingChanges = @(& git -C $repository status --porcelain=v1)
        editor = $editor; graphicsApi = $GraphicsApi; frameLimit = $Frames; exitCode = $code; scenario = $Scenario
        exitTrigger = $(if ($exitRequested) { 'completed-journey' } else { 'frame-limit' })
        seconds = $elapsed.Elapsed.TotalSeconds; cycles = $(if ($Scenario -eq 'Lifecycle') { $Cycles } else { $null })
        status = $(if ($failure) { 'Failed' } else { 'Passed' })
        diagnostic = $(if ($failure) { $failure.Exception.Message } elseif ($Scenario -eq 'Lifecycle') { "Every actual product panel completed $Cycles cycles and $($Cycles / 10 + 1) full-GC resource observations." } else { 'Eight actual-product feature workflows completed.' })
        limitation = 'Public feature commands with real panel drawing; native pointer/keyboard contracts and physical-device experience are separate evidence.'
    } | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $results 'result.json') -Encoding utf8
}
if ($failure) { throw $failure }
Write-Output "Verified product ${Scenario}: $results"
