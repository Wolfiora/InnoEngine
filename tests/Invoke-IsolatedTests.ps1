param(
    [string]$Configuration = 'SourceDebug',
    [string]$ResultsDirectory = 'artifacts/acceptance/ci-tests',
    [string]$Dotnet = 'dotnet',
    [string]$Target,
    [string]$ToolTarget,
    [string]$ProductProperties,
    [string]$ArtifactsPath,
    [string]$Filter,
    [string]$TemporaryDirectory,
    [Parameter(Mandatory)][string]$SdkSnapshot,
    [string]$Solution = 'InnoEngine.sln'
)

$ErrorActionPreference = 'Stop'
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$sdkDirectory = Split-Path $Dotnet -Parent
$sdkEnvironment = @('--environment', "DOTNET_ROOT=$sdkDirectory",
    '--environment', "PATH=$sdkDirectory$([IO.Path]::PathSeparator)$env:PATH")
# Assembly-mode VSTest derives DOTNET_ROOT from the explicit host; passing both duplicates its environment key.
$assemblyEnvironment = @('--environment', "DOTNET_HOST_PATH=$Dotnet",
    '--environment', "PATH=$sdkDirectory$([IO.Path]::PathSeparator)$env:PATH")

$repositoryRoot = Split-Path $PSScriptRoot -Parent
$solutionPath = [System.IO.Path]::GetFullPath($Solution, $repositoryRoot)
$resultsRoot = [System.IO.Path]::GetFullPath($ResultsDirectory, $repositoryRoot)
New-Item -ItemType Directory -Force -Path $resultsRoot | Out-Null
if ($TemporaryDirectory) {
    $temporaryOwner = [IO.Path]::GetFullPath($TemporaryDirectory, $repositoryRoot)
    New-Item -ItemType Directory -Force -Path $temporaryOwner | Out-Null
    $processTemp = Join-Path $temporaryOwner ([Guid]::NewGuid().ToString('N').Substring(0, 12))
    New-Item -ItemType Directory -Path $processTemp | Out-Null
    @{ directory = $processTemp; purpose = 'Owned temporary files for the selected test processes.' } |
        ConvertTo-Json | Set-Content -LiteralPath (Join-Path $resultsRoot 'temporary-owner.json') -Encoding utf8
    $tempEnvironment = @('--environment', "TMP=$processTemp", '--environment', "TEMP=$processTemp",
        '--environment', "TMPDIR=$processTemp")
    $sdkEnvironment += $tempEnvironment
    $assemblyEnvironment += $tempEnvironment
}
$checks = [System.Collections.Generic.List[object]]::new()
$stageClock = [Diagnostics.Stopwatch]::new()
$stageStarted = [DateTimeOffset]::MinValue

function Start-Check([string]$Stage, [string]$Identity) {
    $script:stageStarted = [DateTimeOffset]::UtcNow
    $script:stageClock.Restart()
    Write-Host "Test stage starting: $Stage | $Identity"
}

function Add-CheckResult(
    [string]$Stage,
    [string]$Identity,
    [string]$Status,
    [int]$ExitCode,
    [string]$Diagnostic,
    [string]$Owner = '',
    [string[]]$Contracts = @()
) {
    $checks.Add([pscustomobject]@{
        stage = $Stage
        identity = $Identity
        status = $Status
        exitCode = $ExitCode
        diagnostic = $Diagnostic
        owner = $Owner
        contracts = @($Contracts)
        startedUtc = $stageStarted.ToString('o')
        seconds = $stageClock.Elapsed.TotalSeconds
    })
    ConvertTo-Json -InputObject @($checks.ToArray()) -Depth 4 |
        Set-Content -LiteralPath (Join-Path $resultsRoot 'execution-summary.json') -Encoding utf8
}
$productArguments = @()
if ($ArtifactsPath) {
    $selectedArtifacts = [System.IO.Path]::GetFullPath($ArtifactsPath, $repositoryRoot)
    $productArguments += "-p:ArtifactsPath=$selectedArtifacts"
}
$sharedFilter = 'ProcessIsolation!=Required'
$isolatedFilter = 'ProcessIsolation=Required'
# Emit named long-running test diagnostics without treating legitimate compilation as a hang.
# This interval reports activity; it neither cancels a test nor changes its outcome.
$testParallelism = [Math]::Min([Environment]::ProcessorCount, 4)
$runnerSettings = @('--', 'xUnit.DiagnosticMessages=true', 'xUnit.LongRunningTestSeconds=60', "xUnit.MaxParallelThreads=$testParallelism")
if ($Filter) {
    $sharedFilter = "($sharedFilter)&($Filter)"
    $isolatedFilter = "($isolatedFilter)&($Filter)"
}
if ($Target) {
    if (-not $ProductProperties) { throw 'A target test build requires explicit product properties.' }
    if (-not $ToolTarget) {
        throw 'A planned target test requires its explicitly evaluated tool target.'
    }
    $productArguments += @("-p:InnoNativeTarget=$Target", "-p:InnoToolTarget=$ToolTarget",
        "-p:InnoProductBuildProperties=$ProductProperties")
}

# Process-global Native state and deliberately retained Faulted owners require separate hosts.
Start-Check 'Shared' $solutionPath
try {
    & $Dotnet test $solutionPath --configuration $Configuration `
        --no-build --no-restore --disable-build-servers -m:1 -nr:false `
        --blame --filter $sharedFilter --logger 'trx' --logger 'console;verbosity=normal' --results-directory $resultsRoot @productArguments @sdkEnvironment @runnerSettings
    $sharedExit = $LASTEXITCODE
    Add-CheckResult 'Shared' $solutionPath $(if ($sharedExit -eq 0) { 'Passed' } else { 'Failed' }) $sharedExit ''
} catch {
    Add-CheckResult 'Shared' $solutionPath 'Failed' -1 $_.Exception.Message
}

$selectedProjects = if ([System.IO.Path]::GetExtension($solutionPath) -eq '.slnx') {
    ([xml](Get-Content -LiteralPath $solutionPath -Raw)).SelectNodes('/Solution//Project') | ForEach-Object {
        [IO.Path]::GetFullPath($_.GetAttribute('Path'), (Split-Path $solutionPath -Parent))
    }
} elseif ([System.IO.Path]::GetExtension($solutionPath) -eq '.slnf') {
    (Get-Content -LiteralPath $solutionPath -Raw | ConvertFrom-Json).solution.projects
} else {
    Get-Content -LiteralPath $solutionPath | ForEach-Object {
        if ($_ -match '^Project\("[^"]+"\) = "[^"]+", "([^"]+\.csproj)"') { $Matches[1] }
    }
}
$snapshots = @(Get-Content -LiteralPath $SdkSnapshot -Raw | ConvertFrom-Json)
$projects = @($selectedProjects | ForEach-Object {
        $relativeProject = $_.Replace([char]92, [System.IO.Path]::DirectorySeparatorChar)
        $projectPath = [System.IO.Path]::GetFullPath($relativeProject, $repositoryRoot)
        $evaluated = @($snapshots | Where-Object { [IO.Path]::GetFullPath($_.Path) -eq $projectPath })
        if ($evaluated.Count -ne 1) { throw "The selected SDK graph does not uniquely declare this project: $projectPath" }
        if ($evaluated[0].Properties.IsTestProject -eq 'true') {
            Get-Item -LiteralPath $projectPath
        }
})

foreach ($project in $projects) {
    Start-Check 'Discovery' $project.FullName
    try {
        $matches = @($snapshots | Where-Object { [IO.Path]::GetFullPath($_.Path) -eq $project.FullName })
        $targetPath = @($matches[0].Properties.TargetPath)
        if ($targetPath.Count -ne 1 -or -not (Test-Path -LiteralPath $targetPath[0])) {
            Add-CheckResult 'Discovery' $project.FullName 'Blocked' -1 'The compiled test assembly is unavailable.' -Owner $project.FullName
            continue
        }
        $assembly = $targetPath[0]
        $listed = @(& $Dotnet test $assembly --list-tests --filter $isolatedFilter @assemblyEnvironment 2>&1)
        $discoveryExit = $LASTEXITCODE
        $listed | Set-Content -LiteralPath (Join-Path $resultsRoot ($project.BaseName + '.discovery.log')) -Encoding utf8
        $discoveryErrors = @($listed | Where-Object { $_ -is [System.Management.Automation.ErrorRecord] })
        if ($discoveryExit -ne 0 -or $discoveryErrors.Count -ne 0) {
            Add-CheckResult 'Discovery' $project.FullName 'Failed' $discoveryExit ($listed -join [Environment]::NewLine) -Owner $project.FullName
            continue
        }
        $names = @($listed | ForEach-Object {
            if ($_ -match '^\s+(Inno\.[^\s]+)\s*$') { $Matches[1] }
        } | Sort-Object -Unique)
        Add-CheckResult 'Discovery' $project.FullName 'Passed' 0 "$($names.Count) isolated contracts selected." -Owner $project.FullName -Contracts $names
    } catch {
        Add-CheckResult 'Discovery' $project.FullName 'Failed' -1 $_.Exception.Message -Owner $project.FullName
        continue
    }
    foreach ($name in $names) {
        # A failed independent contract does not prevent the remaining isolated hosts from running.
        $caseFilter = "($isolatedFilter)&(FullyQualifiedName=$name)"
        Start-Check 'Isolated' $name
        try {
            $reportName = 'isolated-' + [Guid]::NewGuid().ToString('N') + '.trx'
            & $Dotnet test $assembly --filter $caseFilter `
                --blame --logger "trx;LogFileName=$reportName" --logger 'console;verbosity=normal' --results-directory $resultsRoot @assemblyEnvironment @runnerSettings
            $caseExit = $LASTEXITCODE
            if ($caseExit -eq 0) {
                & (Join-Path $PSScriptRoot 'ci/Test-IsolatedExecution.ps1') -Report (Join-Path $resultsRoot $reportName) -Contract $name
            }
            Add-CheckResult 'Isolated' $name $(if ($caseExit -eq 0) { 'Passed' } else { 'Failed' }) $caseExit '' -Owner $project.FullName
        } catch {
            Add-CheckResult 'Isolated' $name 'Failed' -1 $_.Exception.Message -Owner $project.FullName
        }
    }
}

$unsuccessful = @($checks | Where-Object { $_.status -ne 'Passed' })
if ($unsuccessful.Count -ne 0) {
    throw "$($unsuccessful.Count) test execution stages failed or were blocked. See $resultsRoot/execution-summary.json."
}
