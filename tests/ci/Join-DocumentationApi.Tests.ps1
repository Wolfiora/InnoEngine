param([Parameter(Mandatory)][string] $Dotnet, [Parameter(Mandatory)][string] $Transport)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$root = Join-Path $engine ('artifacts/validation/api-aggregation/' + [Guid]::NewGuid().ToString('N'))
function Write-Text([string] $Path, [string] $Text) {
    [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($Path)) | Out-Null
    [IO.File]::WriteAllText($Path, $Text, [Text.UTF8Encoding]::new($false))
}
function Write-Json([string] $Path, [object] $Data) { Write-Text $Path (ConvertTo-Json -InputObject $Data -Depth 40) }
function Hash([string] $Path) { (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash.ToLowerInvariant() }
[IO.Directory]::CreateDirectory($root) | Out-Null
& git -C $root init -q -b master
& git -C $root config user.name 'API aggregation fixture'
& git -C $root config user.email fixture@example.invalid
Write-Text (Join-Path $root 'src/Inno.Probe/Inno.Probe.csproj') '<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>'
Write-Text (Join-Path $root 'src/Inno.Probe/Probe.cs') 'internal sealed class Probe { }'
Write-Text (Join-Path $root '.gitignore') "artifacts/`n"
[IO.Directory]::CreateDirectory((Join-Path $root 'tests/ci')) | Out-Null
Copy-Item -LiteralPath "$PSScriptRoot/Join-DocumentationApi.ps1" -Destination (Join-Path $root 'tests/ci/Join-DocumentationApi.ps1')
& git -C $root add -- .
& git -C $root -c core.hooksPath= commit -q -m 'Fixture source'
if ($LASTEXITCODE -ne 0) { throw 'Could not prepare the aggregation fixture source.' }
$revision = (& git -C $root rev-parse HEAD).Trim()
$tree = (& git -C $root rev-parse 'HEAD^{tree}').Trim()
$manifest = Join-Path $root 'artifacts/projects.json'
& $Dotnet $Transport docs export-projects --engine-root $root --output $manifest | Out-Null
if ($LASTEXITCODE -ne 0) { throw 'Fixture actual inventory export failed.' }
$cases = [Collections.Generic.List[object]]::new()
function Case([string] $Name, [scriptblock] $Change, [bool] $Accept) {
    $directory = Join-Path $root "artifacts/$Name/inputs/host"
    [IO.Directory]::CreateDirectory($directory) | Out-Null
    Copy-Item -LiteralPath $manifest -Destination (Join-Path $directory 'projects.json')
    $plan = @{ schema = 'inno-api-export-plan/1'; configuration = 'Debug'; hostTarget = 'fixture-host'; sdkRuntime = 'fixture-sdk';
        projects = @(@{ name = 'Inno.Probe'; path = 'src/Inno.Probe/Inno.Probe.csproj'; target = ''; authoredSurfaceEmpty = $true }) }
    Write-Json (Join-Path $directory 'plan.json') $plan
    $captures = @()
    foreach ($model in @('source-clr', 'runtime-script', 'editor-script')) {
        $facts = @{ model = $model; configuration = 'Debug'; target = 'fixture-host'; sourceApiStatus = 'Unverified'; projects = @() }
        if ($model -eq 'source-clr') {
            $facts.projects = @(@{ name = 'Inno.Probe'; project = 'src/Inno.Probe/Inno.Probe.csproj'; assembly = 'Inno.Probe'; symbols = @(); scriptExports = @() })
        } else {
            $facts.projectionIdentities = @{ 'Inno.ScriptApi.Runtime' = 'a' * 64 }
            if ($model -eq 'editor-script') { $facts.projectionIdentities['Inno.ScriptApi.Editor'] = 'b' * 64 }
        }
        $file = Join-Path $directory "$model.json"
        Write-Json $file $facts
        $captures += @{ path = "$model.json"; sha256 = Hash $file; model = $model; configuration = 'Debug'; target = 'fixture-host' }
    }
    $data = @{ schema = 'inno-api-producer/1'; repository = 'https://github.com/Wolfiora/InnoEngine'; revision = $revision;
        sourceTree = $tree; toolingRevision = $revision; hostTarget = 'fixture-host'; configuration = 'Debug'; sdkRuntime = 'fixture-sdk';
        projectManifestIdentity = Hash (Join-Path $directory 'projects.json'); planIdentity = Hash (Join-Path $directory 'plan.json'); captures = $captures }
    $state = @{ data = $data; directory = $directory; configurations = @('Debug') }
    & $Change $state
    Write-Json (Join-Path $directory 'producer.json') $data
    $accepted = $false
    $failure = $null
    try {
        & (Join-Path $root 'tests/ci/Join-DocumentationApi.ps1') -Dotnet $Dotnet -Transport $Transport `
            -InputDirectory (Split-Path $directory -Parent) -OutputDirectory (Join-Path $root "artifacts/$Name/output") `
            -ExpectedCommit $revision -Configurations $state.configurations | Out-Null
        $accepted = $true
    } catch { $failure = $_.Exception.Message }
    $cases.Add(@{ name = $Name; accepted = $accepted; expected = $Accept; diagnostic = $failure })
    Write-Json (Join-Path $root 'artifacts/results.json') $cases.ToArray()
    if ($accepted -ne $Accept) { throw "Aggregation case $Name expected $Accept, got $accepted. $failure" }
}
Case 'complete-real-inventory' {} $true
Case 'stale-source' { param($s) $s.data.revision = 'f' * 40 } $false
Case 'wrong-source-tree' { param($s) $s.data.sourceTree = 'f' * 40 } $false
Case 'foreign-repository' { param($s) $s.data.repository = 'https://example.invalid/foreign' } $false
Case 'missing-configuration' { param($s) $s.configurations += 'Release' } $false
Case 'missing-script-projection' { param($s) $s.data.captures = @($s.data.captures | Where-Object model -CNE 'editor-script') } $false
Case 'missing-source-owner' { param($s)
    $path = Join-Path $s.directory 'source-clr.json'
    $facts = Get-Content -LiteralPath $path -Raw | ConvertFrom-Json
    $facts.projects = @()
    Write-Json $path $facts
    $s.data.captures[0].sha256 = Hash $path
} $false
Case 'corrupt-facts' { param($s) Add-Content -LiteralPath (Join-Path $s.directory 'source-clr.json') -Value ' ' } $false
Case 'path-escape' { param($s) $s.data.captures[0].path = '../source-clr.json' } $false
Case 'invented-inventory' { param($s)
    Write-Text (Join-Path $s.directory 'projects.json') '[]'
    $s.data.projectManifestIdentity = Hash (Join-Path $s.directory 'projects.json')
} $false
Write-Output "Passed $($cases.Count) actual package aggregation cases. Evidence: $root/artifacts/results.json"
