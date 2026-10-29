param(
    [Parameter(Mandatory)][string]$EditorAssembly,
    [Parameter(Mandatory)][string]$ProjectDirectory,
    [Parameter(Mandatory)][string]$ResultsDirectory,
    [string]$Dotnet = 'dotnet',
    [Parameter(Mandatory)][string]$GraphicsApi,
    [ValidateSet('Workflows', 'Lifecycle')][string]$Scenario = 'Workflows',
    [int]$Frames = 30000,
    [int]$TimeoutSeconds = 600
)

$ErrorActionPreference = 'Stop'
if ($Frames -lt 120 -or $TimeoutSeconds -le 0) { throw 'The journey needs at least 120 frames and a positive deadline.' }
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
$fixtureNames = if ($Scenario -eq 'Lifecycle') { @('PanelLifecycle.editor.cs') } else { @('WorkflowState.cs.txt', 'WorkflowJourney.editor.cs.txt') }
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
foreach ($argument in @($editor, $project, '--window-visible', 'false', '--smoke-frames', [string]$Frames,
                        '--graphics-api', $GraphicsApi)) { $start.ArgumentList.Add($argument) }
$process = [Diagnostics.Process]::new()
$process.StartInfo = $start
$code = $null
$failure = $null
$started = $false
try {
    if (-not $process.Start()) { throw 'The owned Editor process did not start.' }
    $started = $true
    $output = $process.StandardOutput.ReadToEndAsync()
    $errors = $process.StandardError.ReadToEndAsync()
    if (-not $process.WaitForExit($TimeoutSeconds * 1000)) { throw 'The owned Editor journey exceeded its deadline.' }
    $code = $process.ExitCode
    if ($code -ne 0) { throw "The owned Editor exited with code $code." }
    $evidence = Join-Path $project $(if ($Scenario -eq 'Lifecycle') { 'Library/Acceptance/panel-lifecycle.tsv' } else { 'Library/Acceptance/workflow-journey.tsv' })
    if (-not (Test-Path -LiteralPath $evidence)) { throw 'The source extension did not complete the journey.' }
    $verified = @(Get-Content -LiteralPath $evidence)
    if ($Scenario -eq 'Workflows') {
        if ($verified.Count -ne 8) { throw "Expected eight completed workflow stages, got $($verified.Count)." }
    } else {
        $panels = @(Import-Csv -LiteralPath $evidence -Delimiter "`t" | Where-Object panel -ne 'protocol')
        if ($panels.Count -lt 9 -or @($panels | Where-Object 'completed-open-close-cycles' -ne '100').Count -ne 0) {
            throw 'Every actual product panel must complete 100 open/close cycles.'
        }
        $resources = @(Import-Csv -LiteralPath (Join-Path $project 'Library/Acceptance/panel-lifecycle-resources.tsv') -Delimiter "`t")
        if ($resources.Count -ne 11 -or [int]$resources[0].'completed-cycles' -ne 0 -or [int]$resources[-1].'completed-cycles' -ne 100) {
            throw 'The lifecycle run did not record its complete full-GC resource series.'
        }
        $growth = [long]$resources[-1].'managed-bytes-after-full-gc' - [long]$resources[0].'managed-bytes-after-full-gc'
        if ($growth -gt 4MB) { throw "Panel lifecycle retained $growth bytes beyond its 4 MiB final live-memory budget." }
    }
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
    [pscustomobject]@{
        revision = (& git -C $repository rev-parse HEAD).Trim()
        workingChanges = @(& git -C $repository status --porcelain=v1)
        editor = $editor; graphicsApi = $GraphicsApi; frames = $Frames; exitCode = $code; scenario = $Scenario
        status = $(if ($failure) { 'Failed' } else { 'Passed' })
        diagnostic = $(if ($failure) { $failure.Exception.Message } elseif ($Scenario -eq 'Lifecycle') { 'Every actual product panel completed 100 cycles and eleven full-GC resource observations.' } else { 'Eight actual-product feature workflows completed.' })
        limitation = 'Public feature commands with real panel drawing; native pointer/keyboard contracts and physical-device experience are separate evidence.'
    } | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $results 'result.json') -Encoding utf8
}
if ($failure) { throw $failure }
Write-Output "Verified product ${Scenario}: $results"
