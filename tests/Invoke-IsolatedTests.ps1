param(
    [string]$Configuration = 'SourceDebug',
    [string]$ResultsDirectory = 'artifacts/acceptance/ci-tests',
    [string]$Dotnet = 'dotnet',
    [string]$Target,
    [string]$ProductProperties
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$resultsRoot = [System.IO.Path]::GetFullPath($ResultsDirectory, $repositoryRoot)
New-Item -ItemType Directory -Force -Path $resultsRoot | Out-Null
$productArguments = @()
if ($Target) {
    if (-not $ProductProperties) { throw 'A target test build requires explicit product properties.' }
    $productArguments = @("-p:InnoNativeTarget=$Target", "-p:InnoToolTarget=$Target",
        "-p:InnoProductBuildProperties=$ProductProperties", "-p:InnoTestNativeTarget=$Target", '-p:InnoTestNativeProduct=editor')
}

# Process-global Native state and deliberately retained Faulted owners require separate hosts.
& $Dotnet test (Join-Path $repositoryRoot 'InnoEngine.sln') --configuration $Configuration `
    --no-build --no-restore --disable-build-servers -m:1 -nr:false `
    --blame-hang-timeout 20m --blame-hang-dump-type none --filter 'ProcessIsolation!=Required' --logger 'trx' --results-directory $resultsRoot @productArguments
if ($LASTEXITCODE -ne 0) { throw 'The shared test suite failed.' }

$projects = @(Get-Content -LiteralPath (Join-Path $repositoryRoot 'InnoEngine.sln') | ForEach-Object {
    if ($_ -match '^Project\("[^"]+"\) = "[^"]+", "([^"]+\.csproj)"') {
        $relativeProject = $Matches[1].Replace([char]92, [System.IO.Path]::DirectorySeparatorChar)
        $projectPath = [System.IO.Path]::GetFullPath($relativeProject, $repositoryRoot)
        if (([xml](Get-Content -LiteralPath $projectPath -Raw)).Project.PropertyGroup.IsTestProject -contains 'true') {
            Get-Item -LiteralPath $projectPath
        }
    }
})

$projectConfiguration = switch ($Configuration) {
    'SourceDebug' { 'Debug' }
    'SourceRelease' { 'Release' }
    default { $Configuration }
}

foreach ($project in $projects) {
    $targetPath = @(& $Dotnet msbuild $project.FullName -nologo -getProperty:TargetPath `
        "-p:Configuration=$projectConfiguration" -m:1 -nr:false @productArguments)
    if ($LASTEXITCODE -ne 0 -or $targetPath.Count -ne 1 -or -not (Test-Path -LiteralPath $targetPath[0])) {
        throw "Cannot locate the compiled test assembly for $($project.FullName)."
    }
    $assembly = $targetPath[0]
    $listed = @(& $Dotnet test $assembly --list-tests --filter 'ProcessIsolation=Required' 2>&1)
    if ($LASTEXITCODE -ne 0) { throw "Cannot inventory isolated tests in $assembly." }
    $names = @($listed | ForEach-Object {
        if ($_ -match '^\s+(Inno\.[^\s]+)\s*$') { $Matches[1] }
    } | Sort-Object -Unique)
    foreach ($name in $names) {
        # The shared suite already prepared this exact target's Native deployment.
        & $Dotnet test $assembly --filter "FullyQualifiedName=$name" `
            --blame-hang-timeout 10m --blame-hang-dump-type none --logger 'trx' --results-directory $resultsRoot
        if ($LASTEXITCODE -ne 0) { throw "Isolated contract failed: $name" }
    }
}
