#requires -Version 7.2
[CmdletBinding()]
param(
    [ValidateSet('list','editor','player-short','player-long','game-cold','game-warm','support-pack')]
    [string]$Case = 'list',
    [string]$Executable,
    [string]$Project,
    [string]$InputRevision,
    [string]$Target,
    [string]$ToolsTarget,
    [string]$SupportPacks,
    [string]$StartupScene,
    [string]$GraphicsApi,
    [string]$Hardware,
    [string]$ExpectedArtifact,
    [string]$Artifacts = 'artifacts/benchmarks/products',
    [string]$Dotnet = 'dotnet',
    [ValidateRange(1,86400)][int]$TimeoutSeconds = 3600,
    [string]$MetricsHook = "$PSScriptRoot/../support/Inno.Benchmark.ProcessMetrics/bin/Release/net10.0/Inno.Benchmark.ProcessMetrics.dll"
)
$ErrorActionPreference = 'Stop'
if ($Case -eq 'list') {
    'editor','player-short','player-long','game-cold','game-warm','support-pack'
    return
}
$engine = (Resolve-Path "$PSScriptRoot/../..").Path
if (!($IsWindows -or $IsMacOS)) {
    [Console]::Error.WriteLine('Unavailable: native product scenarios require a registered desktop host.')
    exit 3
}
$isFrame = $Case -in 'editor','player-short','player-long'
$isGame = $Case -in 'game-cold','game-warm'
if (!$Executable -or !$InputRevision) { throw 'Supply -Executable and -InputRevision (input project or product commit/hash).' }
$entry = (Get-Item -LiteralPath $Executable -ErrorAction Stop).FullName
$dotnetHost = (Get-Command $Dotnet -CommandType Application).Source
if ($isFrame -and (!$Hardware -or !$GraphicsApi)) { throw 'Frame measurements require -Hardware and -GraphicsApi.' }
if ($isGame -or $Case -eq 'editor') { $Project = (Resolve-Path -LiteralPath $Project).Path }
if (!$isFrame -and (!$Target -or !$ToolsTarget -or !$ExpectedArtifact)) {
    throw 'Build measurements require -Target, -ToolsTarget and the relative path of -ExpectedArtifact.'
}
if ($isGame) {
    $SupportPacks = (Resolve-Path -LiteralPath $SupportPacks).Path
    if (!$StartupScene) { throw 'Supply -StartupScene relative to Assets.' }
}
$run = Join-Path ([IO.Path]::GetFullPath($Artifacts)) "$Case-$([guid]::NewGuid().ToString('N'))"
$output = Join-Path $run 'product'
if (!$isFrame) {
    $expected = [IO.Path]::GetFullPath($ExpectedArtifact,$output)
    if (![IO.Path]::GetRelativePath($output,$expected).Equals($ExpectedArtifact.Replace('/',[IO.Path]::DirectorySeparatorChar)) -or
        $ExpectedArtifact -match '(^|[/\\])\.\.([/\\]|$)' -or [IO.Path]::IsPathRooted($ExpectedArtifact)) {
        throw 'ExpectedArtifact must be a relative artifact path inside product/.'
    }
}
if ($isFrame -and !(Test-Path -LiteralPath $MetricsHook -PathType Leaf)) {
    [Console]::Error.WriteLine("Unavailable: build the Release process metrics hook at $MetricsHook.")
    exit 3
}
New-Item -ItemType Directory -Path $run | Out-Null
$frames = if ($Case -eq 'player-long') { 6000 } else { 600 }
$launch = switch ($Case) {
    'editor' { $Project; '--graphics-api'; $GraphicsApi; '--window-visible'; 'true'; '--smoke-frames'; "$frames" }
    { $_ -in 'player-short','player-long' } { '--graphics-api'; $GraphicsApi; '--window-visible'; 'true'; '--smoke-frames'; "$frames" }
    { $isGame } { 'game'; '--tools-target'; $ToolsTarget; '--target'; $Target; '--support-packs'; $SupportPacks; '--project'; $Project; '--startup-scene'; $StartupScene; '--output'; $output }
    'support-pack' { 'support-pack'; '--engine-root'; $engine; '--tools-target'; $ToolsTarget; '--target'; $Target; '--configuration'; 'Release'; '--output'; $output }
}
$commit = git -C $engine rev-parse HEAD
if ($LASTEXITCODE) { throw 'Engine revision unavailable.' }
Push-Location -LiteralPath $engine
try {
    $sdk = & $dotnetHost --version
    if ($LASTEXITCODE) { throw 'Cannot identify the selected .NET SDK.' }
} finally {
    Pop-Location
}
$report = [ordered]@{ case=$Case; engineCommit=$commit; inputRevision=$InputRevision; executable=$entry;
    executableSha256=(Get-FileHash -LiteralPath $entry).Hash; arguments=@($launch); host=[Runtime.InteropServices.RuntimeInformation]::OSDescription;
    architecture=[Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture.ToString(); sdk=$sdk; graphicsApi=$GraphicsApi;
    hardware=$Hardware; target=$Target; configuration='Release'; status='started'; gpuMilliseconds=$null }
function Invoke-Product([string]$phase) {
    $start = [Diagnostics.ProcessStartInfo]::new()
    $start.UseShellExecute = $false
    $start.WorkingDirectory = $engine
    $start.FileName = $entry
    if ([IO.Path]::GetExtension($entry) -eq '.dll') {
        $start.FileName = $dotnetHost
        $start.ArgumentList.Add($entry)
    }
    foreach ($argValue in $launch) { $start.ArgumentList.Add($argValue) }
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    if ($isFrame) {
        $hook = (Resolve-Path -LiteralPath $MetricsHook).Path
        $start.Environment['DOTNET_STARTUP_HOOKS'] = (@($start.Environment['DOTNET_STARTUP_HOOKS'],$hook) | Where-Object {$_}) -join [IO.Path]::PathSeparator
        $start.Environment['INNO_BENCHMARK_PROCESS_METRICS'] = Join-Path $run 'managed-metrics.txt'
    }
    $stdout = [IO.File]::Create((Join-Path $run "$phase.stdout"))
    $stderr = [IO.File]::Create((Join-Path $run "$phase.stderr"))
    $process = [Diagnostics.Process]::new()
    try {
        $process.StartInfo = $start
        $watch = [Diagnostics.Stopwatch]::StartNew()
        if (!$process.Start()) { throw 'Product process failed to start.' }
        $pidMeasured = $process.Id
        $outTask = $process.StandardOutput.BaseStream.CopyToAsync($stdout)
        $errTask = $process.StandardError.BaseStream.CopyToAsync($stderr)
        if (!$process.WaitForExit($TimeoutSeconds * 1000)) {
            $process.Kill($true)
            $process.WaitForExit()
            throw "$phase exceeded $TimeoutSeconds seconds."
        }
        $watch.Stop()
        $outTask.GetAwaiter().GetResult() | Out-Null
        $errTask.GetAwaiter().GetResult() | Out-Null
        if ($process.ExitCode) { throw "$phase exited with $($process.ExitCode). See $run." }
        $cpu = $null
        try { $cpu = $process.TotalProcessorTime.TotalMilliseconds } catch [InvalidOperationException] { }
        return @{ pid=$pidMeasured; wallMilliseconds=$watch.Elapsed.TotalMilliseconds; cpuMilliseconds=$cpu }
    } finally {
        $process.Dispose()
        $stdout.Dispose()
        $stderr.Dispose()
    }
}
try {
    if ($Case -eq 'game-warm') {
        $null = Invoke-Product 'prepare'
        if (!(Test-Path -LiteralPath $expected -PathType Leaf)) { throw 'Warm preparation did not produce the requested artifact.' }
    }
    $report.process = Invoke-Product 'measure'
    if ($isFrame) {
        $report.frames = $frames
        $report.windowVisible = $true
        $report.vsync = if ($Case -eq 'editor') { 'editor configuration' } else { 'enabled by Player' }
        if ($Case -like 'player-*') {
            $smoke = Select-String -LiteralPath (Join-Path $run 'measure.stdout') -Pattern '^INNO-SMOKE frames=(\d+) views=(\d+) draws=(\d+) dispatches=(\d+)' | Select-Object -Last 1
            if (!$smoke -or [int]$smoke.Matches[0].Groups[1].Value -ne $frames) { throw 'Player did not report the requested frame count.' }
            $report.finalFrame = @{views=[int]$smoke.Matches[0].Groups[2].Value;draws=[int]$smoke.Matches[0].Groups[3].Value;dispatches=[int]$smoke.Matches[0].Groups[4].Value}
        }
        if ($Case -eq 'editor') {
            $boot = Get-Content -Raw -LiteralPath (Join-Path $run 'measure.stdout')
            if ($boot -notmatch "Smoke frame limit reached after $frames frame\(s\)\." -or
                $boot -notmatch 'Dispose end\.' -or $boot -match 'Teardown failure') { throw 'Editor frame/shutdown evidence missing.' }
        }
        $metricsPath = Join-Path $run 'managed-metrics.txt'
        if (!(Test-Path -LiteralPath $metricsPath)) {
            $report.status = 'unavailable'
            $report.reason = 'The selected runtime did not execute managed process metrics hooks.'
            exit 3
        }
        $report.managed = Get-Content -Raw -LiteralPath $metricsPath | ConvertFrom-StringData
        if ([int]$report.managed.pid -ne $report.process.pid) { throw 'Metrics belong to another process.' }
        $report.process.cpuMilliseconds = [double]::Parse($report.managed.cpuMilliseconds,[CultureInfo]::InvariantCulture)
    } else {
        if (!(Test-Path -LiteralPath $expected -PathType Leaf)) { throw "Missing product artifact: $expected" }
        $report.artifactSha256 = (Get-FileHash -LiteralPath $expected).Hash
    }
    $report.status = 'completed'
} catch {
    $report.status = 'failed'
    $report.reason = $_.Exception.Message
    throw
} finally {
    $report | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $run 'result.json') -Encoding utf8
    Write-Host "Benchmark output: $run"
}
