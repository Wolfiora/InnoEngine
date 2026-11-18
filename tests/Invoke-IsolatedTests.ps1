param(
    [string]$Configuration = 'SourceDebug',
    [string]$ResultsDirectory = 'artifacts/acceptance/ci-tests',
    [string]$Dotnet = 'dotnet',
    [string]$Target,
    [string]$ToolTarget,
    [string]$ProductProperties,
    [string]$ArtifactsPath,
    [string]$NativeDependencies,
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
$executionEnvironment = @('--environment', "INNO_TESTING_REPOSITORY_ROOT=$repositoryRoot")
if ($ArtifactsPath) {
    $executionEnvironment += @('--environment', "INNO_TESTING_ARTIFACTS_PATH=$([IO.Path]::GetFullPath($ArtifactsPath, $repositoryRoot))")
}
if ($NativeDependencies) {
    $executionEnvironment += @('--environment', "INNO_TESTING_NATIVE_DEPENDENCIES=$([IO.Path]::GetFullPath($NativeDependencies, $repositoryRoot))")
}
$sdkEnvironment += $executionEnvironment
$assemblyEnvironment += $executionEnvironment
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

$loggerProject = Join-Path $PSScriptRoot 'process/Inno.Testing.Discovery.TestLogger/Inno.Testing.Discovery.TestLogger.csproj'
$logger = @($snapshots | Where-Object { [IO.Path]::GetFullPath($_.Path) -eq $loggerProject })
$assemblies = [Collections.Generic.Dictionary[string,object]]::new($(if ($IsWindows) { [StringComparer]::OrdinalIgnoreCase } else { [StringComparer]::Ordinal }))
$contracts = @{}
Start-Check 'Discovery' $solutionPath
try {
    if ($logger.Count -ne 1 -or -not (Test-Path -LiteralPath $logger[0].Properties.TargetPath -PathType Leaf)) {
        throw 'The selected SDK graph must provide the compiled discovery logger.'
    }
    foreach ($project in $projects) {
        $snapshot = @($snapshots | Where-Object { [IO.Path]::GetFullPath($_.Path) -eq $project.FullName })
        $assembly = $snapshot[0].Properties.TargetPath
        if (-not (Test-Path -LiteralPath $assembly -PathType Leaf)) { throw "Compiled test assembly is missing: $assembly" }
        $assemblies.Add([IO.Path]::GetFullPath($assembly), $project)
    }
    if ($assemblies.Count -eq 0) { throw 'No test assemblies were selected for discovery.' }

    # VSTest receives the same testhost environment as the selected executions.
    $settings = [xml]'<RunSettings><RunConfiguration><EnvironmentVariables /></RunConfiguration></RunSettings>'
    $variables = $settings.SelectSingleNode('/RunSettings/RunConfiguration/EnvironmentVariables')
    for ($i = 1; $i -lt $assemblyEnvironment.Count; $i += 2) {
        $key, $value = $assemblyEnvironment[$i].Split('=', 2)
        $variable = $settings.CreateElement($key)
        $variable.InnerText = $value
        $null = $variables.AppendChild($variable)
    }
    $settingsPath = Join-Path $resultsRoot 'discovery.runsettings'
    $settings.Save($settingsPath)
    $receipt = Join-Path $resultsRoot ('discovery-' + [Guid]::NewGuid().ToString('N') + '.json')
    $loggerDirectory = Split-Path $logger[0].Properties.TargetPath -Parent
    $sources = @($assemblies.Keys)
    $options = @('--ListTests', "--Settings:$settingsPath", "--TestAdapterPath:$loggerDirectory", "--Logger:InnoDiscovery;Receipt=$receipt")
    if ($Filter) { $options += "--TestCaseFilter:$Filter" }
    $log = Join-Path $resultsRoot 'discovery.log'
    & $Dotnet vstest @sources @options *> $log
    if ($LASTEXITCODE -ne 0) { throw "VSTest discovery exited $LASTEXITCODE. See $log." }
    $discovery = Get-Content -LiteralPath $receipt -Raw | ConvertFrom-Json
    $count = 0
    foreach ($owner in $discovery.owners) {
        if (-not $assemblies.ContainsKey($owner.source) -or $owner.count -lt 1) { throw "Unexpected discovery source or count: $($owner.source)" }
        $contracts.Add($assemblies[$owner.source].FullName, @($owner.contracts))
        $count += $owner.count
    }
    if (-not $discovery.completed -or $count -ne $discovery.discovered) {
        throw "VSTest did not complete the reported discovery inventory. See $log. $($discovery.errors -join [Environment]::NewLine)"
    }
} catch {
    foreach ($project in $projects) {
        Add-CheckResult 'Discovery' $project.FullName 'Failed' -1 $_.Exception.Message -Owner $project.FullName
    }
    throw
}

foreach ($project in $projects) {
    if (-not $contracts.ContainsKey($project.FullName) -and -not $Filter) {
        Add-CheckResult 'Discovery' $project.FullName 'Failed' -1 'The compiled owner discovered zero test cases.' -Owner $project.FullName
        continue
    }
    $names = [string[]]@()
    if ($contracts.ContainsKey($project.FullName)) { $names = $contracts[$project.FullName] }
    Add-CheckResult 'Discovery' $project.FullName 'Passed' 0 "$($names.Count) isolated contracts selected." -Owner $project.FullName -Contracts $names
}

foreach ($project in $projects) {
    if (-not $contracts.ContainsKey($project.FullName)) { continue }
    $names = $contracts[$project.FullName]
    $assembly = ($snapshots | Where-Object { [IO.Path]::GetFullPath($_.Path) -eq $project.FullName }).Properties.TargetPath
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
