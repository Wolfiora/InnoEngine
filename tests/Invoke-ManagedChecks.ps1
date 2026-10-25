param(
    [string]$Dotnet = 'dotnet',
    [string]$Configuration = 'SourceDebug',
    [string]$ResultsDirectory = 'artifacts/acceptance/managed',
    [switch]$InventoryOnly
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$resultsRoot = [IO.Path]::GetFullPath($ResultsDirectory, $repositoryRoot)
New-Item -ItemType Directory -Force -Path $resultsRoot | Out-Null
$projects = @{}
$deferred = @{}
foreach ($line in Get-Content (Join-Path $repositoryRoot 'InnoEngine.sln')) {
    if ($line -notmatch '^Project\("[^"]+"\) = "[^"]+", "([^"]+\.csproj)"') { continue }
    $relative = $Matches[1]
    $path = [IO.Path]::GetFullPath($relative.Replace([char]92, [IO.Path]::DirectorySeparatorChar), $repositoryRoot)
    $output = & $Dotnet msbuild $path -nologo -p:DesignTimeBuild=true -p:Configuration=Debug `
        -getProperty:BindGenGeneratedBindings,InnoProductId,NETCoreSdkVersion,RoslynTargetsPath,MSBuildVersion -getItem:ProjectReference
    if ($LASTEXITCODE -ne 0) { throw "Project inventory failed: $relative" }
    $evaluated = ($output -join [Environment]::NewLine) | ConvertFrom-Json
    if ($projects.Count -eq 0) {
        $evaluated.Properties | ConvertTo-Json | Set-Content (Join-Path $resultsRoot 'compiler-selection.json') -Encoding utf8
        & $Dotnet (Join-Path $evaluated.Properties.RoslynTargetsPath 'bincore/csc.dll') -version |
            Set-Content (Join-Path $resultsRoot 'compiler-version.txt')
        if ($LASTEXITCODE -ne 0) { throw 'Cannot identify the selected SDK compiler.' }
    }
    $references = @($evaluated.Items.ProjectReference | ForEach-Object { $_.FullPath })
    $projects[$path] = [pscustomobject]@{ path = $path; relative = $relative; references = $references }
    if ($evaluated.Properties.BindGenGeneratedBindings -eq 'true') {
        $deferred[$path] = "Requires generated binding: $relative"
    } elseif ($evaluated.Properties.InnoProductId) {
        $deferred[$path] = "Requires native product preparation: $relative"
    }
}

# Follow evaluated project references so consumers cannot accidentally bypass an excluded producer.
do {
    $changed = $false
    foreach ($project in $projects.Values) {
        if ($deferred.ContainsKey($project.path)) { continue }
        foreach ($reference in $project.references) {
            if ($deferred.ContainsKey($reference)) {
                $deferred[$project.path] = "$($project.relative) -> $($deferred[$reference])"
                $changed = $true
                break
            }
        }
    }
} while ($changed)

$selected = @($projects.Values | Where-Object { -not $deferred.ContainsKey($_.path) } | Sort-Object relative)
if ($selected.Count -eq 0) { throw 'The evaluated graph contains no independent managed projects.' }
$filterPath = Join-Path $resultsRoot 'Managed.slnf'
@{ solution = @{ path = (Join-Path $repositoryRoot 'InnoEngine.sln'); projects = @($selected.relative) } } |
    ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $filterPath -Encoding utf8
@($deferred.Keys | Sort-Object | ForEach-Object {
    [pscustomobject]@{ project = $projects[$_].relative; status = 'Deferred'; reason = $deferred[$_] }
}) | ConvertTo-Json -Depth 4 | Set-Content (Join-Path $resultsRoot 'deferred-projects.json') -Encoding utf8
@{
    trait = 'BuildGraph=Complete'
    status = 'Deferred'
    reason = 'Requires the complete SourceDebug solution outputs; executed by the native product CI test suite.'
} | ConvertTo-Json | Set-Content (Join-Path $resultsRoot 'deferred-test-context.json') -Encoding utf8
& $Dotnet --info | Set-Content (Join-Path $resultsRoot 'dotnet-info.txt')
Write-Host "Selected $($selected.Count) managed projects; deferred $($deferred.Count) native-dependent projects."
Write-Host 'Tests requiring BuildGraph=Complete run after the full SourceDebug build in product CI.'
if ($InventoryOnly) { return }
& $Dotnet build $filterPath --configuration $Configuration --disable-build-servers -m:1 -nr:false -p:UseSharedCompilation=false 2>&1 |
    Tee-Object -FilePath (Join-Path $resultsRoot 'build.log')
if ($LASTEXITCODE -ne 0) { throw 'The independent managed build failed.' }
& (Join-Path $PSScriptRoot 'Invoke-IsolatedTests.ps1') -Dotnet $Dotnet -Configuration $Configuration `
    -Solution $filterPath -Filter 'BuildGraph!=Complete' -ResultsDirectory $resultsRoot *>&1 |
    Tee-Object -FilePath (Join-Path $resultsRoot 'tests.log')
