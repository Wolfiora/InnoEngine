param(
    [string]$Configuration = 'SourceDebug',
    [string]$ResultsDirectory = 'artifacts/acceptance/ci-tests',
    [string]$Dotnet = 'dotnet',
    [string]$Target,
    [string]$ProductProperties,
    [string]$ArtifactsPath,
    [string]$Filter,
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
$checks = [System.Collections.Generic.List[object]]::new()

function Add-CheckResult(
    [string]$Stage,
    [string]$Identity,
    [string]$Status,
    [int]$ExitCode,
    [string]$Diagnostic
) {
    $checks.Add([pscustomobject]@{
        stage = $Stage
        identity = $Identity
        status = $Status
        exitCode = $ExitCode
        diagnostic = $Diagnostic
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
if ($Filter) {
    $sharedFilter = "($sharedFilter)&($Filter)"
    $isolatedFilter = "($isolatedFilter)&($Filter)"
}
if ($Target) {
    if (-not $ProductProperties) { throw 'A target test build requires explicit product properties.' }
    $productArguments += @("-p:InnoNativeTarget=$Target", "-p:InnoToolTarget=$Target",
        "-p:InnoProductBuildProperties=$ProductProperties", "-p:InnoTestNativeTarget=$Target", '-p:InnoTestNativeProduct=editor')
}

# Process-global Native state and deliberately retained Faulted owners require separate hosts.
try {
    & $Dotnet test $solutionPath --configuration $Configuration `
        --no-build --no-restore --disable-build-servers -m:1 -nr:false `
        --blame-hang-timeout 20m --blame-hang-dump-type none --filter $sharedFilter --logger 'trx' --results-directory $resultsRoot @productArguments @sdkEnvironment
    $sharedExit = $LASTEXITCODE
    Add-CheckResult 'Shared' $solutionPath $(if ($sharedExit -eq 0) { 'Passed' } else { 'Failed' }) $sharedExit ''
} catch {
    Add-CheckResult 'Shared' $solutionPath 'Failed' -1 $_.Exception.Message
}

$selectedProjects = if ([System.IO.Path]::GetExtension($solutionPath) -eq '.slnf') {
    (Get-Content -LiteralPath $solutionPath -Raw | ConvertFrom-Json).solution.projects
} else {
    Get-Content -LiteralPath $solutionPath | ForEach-Object {
        if ($_ -match '^Project\("[^"]+"\) = "[^"]+", "([^"]+\.csproj)"') { $Matches[1] }
    }
}
$projects = @($selectedProjects | ForEach-Object {
        $relativeProject = $_.Replace([char]92, [System.IO.Path]::DirectorySeparatorChar)
        $projectPath = [System.IO.Path]::GetFullPath($relativeProject, $repositoryRoot)
        if (([xml](Get-Content -LiteralPath $projectPath -Raw)).Project.PropertyGroup.IsTestProject -contains 'true') {
            Get-Item -LiteralPath $projectPath
        }
})

$projectConfiguration = switch ($Configuration) {
    'SourceDebug' { 'Debug' }
    'SourceRelease' { 'Release' }
    default { $Configuration }
}

foreach ($project in $projects) {
    try {
        $targetPath = @(& $Dotnet msbuild $project.FullName -nologo -getProperty:TargetPath `
            "-p:Configuration=$projectConfiguration" -m:1 -nr:false @productArguments)
        if ($LASTEXITCODE -ne 0 -or $targetPath.Count -ne 1 -or -not (Test-Path -LiteralPath $targetPath[0])) {
            Add-CheckResult 'Discovery' $project.FullName 'Blocked' -1 'The compiled test assembly is unavailable.'
            continue
        }
        $assembly = $targetPath[0]
        $listed = @(& $Dotnet test $assembly --list-tests --filter $isolatedFilter @assemblyEnvironment 2>&1)
        $discoveryExit = $LASTEXITCODE
        $listed | Set-Content -LiteralPath (Join-Path $resultsRoot ($project.BaseName + '.discovery.log')) -Encoding utf8
        $discoveryErrors = @($listed | Where-Object { $_ -is [System.Management.Automation.ErrorRecord] })
        if ($discoveryExit -ne 0 -or $discoveryErrors.Count -ne 0) {
            Add-CheckResult 'Discovery' $project.FullName 'Failed' $discoveryExit ($listed -join [Environment]::NewLine)
            continue
        }
        $names = @($listed | ForEach-Object {
            if ($_ -match '^\s+(Inno\.[^\s]+)\s*$') { $Matches[1] }
        } | Sort-Object -Unique)
        Add-CheckResult 'Discovery' $project.FullName 'Passed' 0 "$($names.Count) isolated contracts selected."
    } catch {
        Add-CheckResult 'Discovery' $project.FullName 'Failed' -1 $_.Exception.Message
        continue
    }
    foreach ($name in $names) {
        # A failed independent contract does not prevent the remaining isolated hosts from running.
        $caseFilter = "($isolatedFilter)&(FullyQualifiedName=$name)"
        try {
            & $Dotnet test $assembly --filter $caseFilter `
                --blame-hang-timeout 10m --blame-hang-dump-type none --logger 'trx' --results-directory $resultsRoot @assemblyEnvironment
            $caseExit = $LASTEXITCODE
            Add-CheckResult 'Isolated' $name $(if ($caseExit -eq 0) { 'Passed' } else { 'Failed' }) $caseExit ''
        } catch {
            Add-CheckResult 'Isolated' $name 'Failed' -1 $_.Exception.Message
        }
    }
}

$unsuccessful = @($checks | Where-Object { $_.status -ne 'Passed' })
if ($unsuccessful.Count -ne 0) {
    throw "$($unsuccessful.Count) test execution stages failed or were blocked. See $resultsRoot/execution-summary.json."
}
