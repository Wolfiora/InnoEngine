$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$fixture = Join-Path $engine ('artifacts/documentation-preflight-test-' + [Guid]::NewGuid().ToString('N'))
$gitApplication = (Get-Command git -CommandType Application | Select-Object -First 1).Source
[IO.Directory]::CreateDirectory((Join-Path $fixture 'tests/ci')) | Out-Null
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'Get-DocumentationExportPreflight.ps1') -Destination (Join-Path $fixture 'tests/ci')
function Commit-Fixture {
    & $gitApplication -C $fixture add .
    & $gitApplication -C $fixture -c user.name=Fixture -c user.email=fixture@example.invalid commit -qm fixture
    if ($LASTEXITCODE -ne 0) { throw 'Cannot commit input fixture.' }
}
function git {
    # The exact authenticated baseline already exists locally; no fixture network access.
    if ('fetch' -cin $args) { $global:LASTEXITCODE = 0; return }
    & $gitApplication @args
}
function Invoke-RestMethod {
    param([string] $Uri, $Headers)
    if ($Uri -match '/jobs\?') { return @{ jobs = @(@{ name='API export validation'; conclusion='success' }) } }
    if ($Uri -match '/artifacts\?') { return @{ artifacts = @(@{ name='documentation-api-package-1'; expired=$false; digest=('sha256:' + ('a' * 64)) }) } }
    return @{ workflow_runs = @(@{ id=1; run_attempt=1; head_sha=$fixtureBaseline; head_repository=@{full_name='Wolfiora/InnoEngine'}; path='.github/workflows/documentation-api.yml' }) }
}
function Assert-Plan([string] $Expected) {
    & (Join-Path $fixture 'tests/ci/Get-DocumentationExportPreflight.ps1')
    $result = Get-Content (Join-Path $fixture 'artifacts/documentation-export/preflight.json') -Raw | ConvertFrom-Json
    if ($result.status -cne $Expected) { throw "Expected $Expected, received $($result.status)." }
}
$previousToken = $env:GH_TOKEN
$previousOutput = $env:GITHUB_OUTPUT
try {
    $env:GITHUB_OUTPUT = $null
    & $gitApplication -C $fixture init -q
    [IO.File]::WriteAllText((Join-Path $fixture '.gitignore'), "artifacts/`n")
    [IO.File]::WriteAllText((Join-Path $fixture 'README.md'), 'Original guidance')
    [IO.Directory]::CreateDirectory((Join-Path $fixture 'src')) | Out-Null
    [IO.File]::WriteAllText((Join-Path $fixture 'src/Owner.cs'), 'public class Owner {}')
    Commit-Fixture
    $fixtureBaseline = (& $gitApplication -C $fixture rev-parse HEAD).Trim()
    $env:GH_TOKEN = 'fixture-only'
    Assert-Plan 'NoRelevantChanges'
    [IO.File]::AppendAllText((Join-Path $fixture 'README.md'), ' More guidance')
    Commit-Fixture
    Assert-Plan 'NoRelevantChanges'
    [IO.File]::WriteAllText((Join-Path $fixture 'src/Added.cs'), 'public class Added {}')
    Commit-Fixture
    Assert-Plan 'EvaluationRequired'
    $fixtureBaseline = (& $gitApplication -C $fixture rev-parse HEAD).Trim()
    Remove-Item -LiteralPath (Join-Path $fixture 'src/Added.cs')
    Commit-Fixture
    Assert-Plan 'EvaluationRequired'
    Write-Host 'Preflight passed: unchanged input, README-only, added and removed Compile membership.'
} finally {
    $env:GH_TOKEN = $previousToken
    $env:GITHUB_OUTPUT = $previousOutput
    $boundary = [IO.Path]::GetFullPath((Join-Path $engine 'artifacts')) + [IO.Path]::DirectorySeparatorChar
    if (![IO.Path]::GetFullPath($fixture).StartsWith($boundary, [StringComparison]::OrdinalIgnoreCase)) { throw 'Fixture cleanup escaped its owner.' }
    Remove-Item -LiteralPath $fixture -Recurse -Force
}
