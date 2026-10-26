param(
    [Parameter(Mandatory)]
    [string]$ProjectDirectory,
    [Parameter(Mandatory)]
    [string]$Target,
    [string]$Dotnet = 'dotnet',
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',
    [string]$ResultsDirectory = 'artifacts/acceptance/wiki-gate'
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path $PSScriptRoot -Parent
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$project = [IO.Path]::GetFullPath($ProjectDirectory, $repository)
$results = [IO.Path]::GetFullPath($ResultsDirectory, $repository)
New-Item -ItemType Directory -Force -Path $results | Out-Null
$entryProject = Join-Path $repository 'entry/InnoEngine.Build/InnoEngine.Build.csproj'
$entry = @(& $Dotnet msbuild $entryProject -nologo -m:1 -nr:false "-p:Configuration=$Configuration" -getProperty:TargetPath)
if ($LASTEXITCODE -ne 0 -or $entry.Count -ne 1 -or -not (Test-Path -LiteralPath $entry[0])) {
    throw 'Build the selected Build entry and product references before running the Wiki gate.'
}

# Generate the current projection through the same workflow used by project authors.
& $Dotnet $entry[0] scripts --engine-root $repository --tools-target $Target --dotnet $Dotnet `
    --project $project --output (Join-Path $results 'Scripts') --ide 2>&1 |
    Tee-Object -FilePath (Join-Path $results 'projection.log')
if ($LASTEXITCODE -ne 0) { throw 'Current project script projection failed.' }
$ide = [xml](Get-Content -LiteralPath (Join-Path $project 'Inno.EditorScripts.csproj') -Raw)
function Read-Reference([string]$Name) {
    $references = @($ide.Project.ItemGroup.Reference | Where-Object { $_.Include -ceq $Name })
    if ($references.Count -ne 1) { throw "The current IDE projection must declare exactly one '$Name' reference." }
    $path = [IO.Path]::GetFullPath($references[0].HintPath, $project)
    if (-not (Test-Path -LiteralPath $path)) { throw "The current reference is unavailable: $path" }
    return $path
}
$runtimeReference = Read-Reference 'Inno.ScriptApi.Runtime'
$editorReference = Read-Reference 'Inno.ScriptApi.Editor'
@{ runtime = $runtimeReference; editor = $editorReference } | ConvertTo-Json |
    Set-Content -LiteralPath (Join-Path $results 'script-references.json') -Encoding utf8
& $Dotnet $entry[0] verify $repository --dotnet $Dotnet --configuration $Configuration --wiki --wiki-target $Target `
    --wiki-runtime-reference $runtimeReference --wiki-editor-reference $editorReference `
    --wiki-api (Join-Path $results 'api.json') --wiki-examples (Join-Path $results 'examples.json') 2>&1 |
    Tee-Object -FilePath (Join-Path $results 'wiki.log')
if ($LASTEXITCODE -ne 0) { throw "The Wiki gate failed; see $results/wiki.log." }
