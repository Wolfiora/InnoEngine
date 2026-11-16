$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/source-preparation-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $directory -Force | Out-Null
$originalProtocols = $env:GIT_ALLOW_PROTOCOL
$env:GIT_ALLOW_PROTOCOL = 'file'
function Invoke-FixtureGit([string]$Root, [string[]]$Arguments) {
    $value = @(& git -C $Root @Arguments 2>&1)
    if ($LASTEXITCODE -ne 0) { throw "Fixture git failed: $($value -join [Environment]::NewLine)" }
    $value -join "`n"
}
$cases = @('matching', 'wrong-commit', 'wrong-pin', 'unregistered-path', 'empty-pins', 'empty-owners', 'wrong-kind', 'dirty-inputs', 'stale-head')
try {
    $source = Join-Path $directory 'source'
    New-Item -ItemType Directory -Path $source | Out-Null
    'Pinned source bytes.' | Set-Content (Join-Path $source 'source.txt')
    $null = Invoke-FixtureGit $source @('init', '-q')
    $null = Invoke-FixtureGit $source @('add', '.')
    $null = Invoke-FixtureGit $source @('-c', 'user.name=Aaron Liao', '-c', 'user.email=hsuankailiao@gmail.com', 'commit', '-qm', 'Native source fixture')
    $pin = Invoke-FixtureGit $source @('rev-parse', 'HEAD')
    foreach ($case in $cases) {
        $root = Join-Path $directory $case
        $scripts = Join-Path $root 'tests/ci'
        $output = Join-Path $root 'artifacts'
        New-Item -ItemType Directory -Path $scripts, $output -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/Prepare-ValidationSources.ps1" -Destination $scripts
        "artifacts/`n" | Set-Content (Join-Path $root '.gitignore')
        'Committed engine inputs.' | Set-Content (Join-Path $root 'engine.txt')
        $null = Invoke-FixtureGit $root @('init', '-q')
        $null = Invoke-FixtureGit $root @('config', '-f', '.gitmodules', 'submodule.native.path', 'extern/Native with spaces')
        $null = Invoke-FixtureGit $root @('config', '-f', '.gitmodules', 'submodule.native.url', $source.Replace('\', '/'))
        $null = Invoke-FixtureGit $root @('add', '.')
        $null = Invoke-FixtureGit $root @('update-index', '--add', '--cacheinfo', "160000,$pin,extern/Native with spaces")
        $null = Invoke-FixtureGit $root @('-c', 'user.name=Aaron Liao', '-c', 'user.email=hsuankailiao@gmail.com', 'commit', '-qm', 'Pinned preparation fixture')
        New-Item -ItemType Directory -Path (Join-Path $root 'extern/Native with spaces') -Force | Out-Null
        $commit = Invoke-FixtureGit $root @('rev-parse', 'HEAD')
        $request = @{ status = 'Blocked'; kind = 'NativeSources'; root = $root; commit = $commit
            owners = @('src/Native/Native.csproj'); sources = @{ 'extern/Native with spaces' = $pin } }
        switch ($case) {
            'wrong-commit' { $request.commit = 'a' * 40 }
            'wrong-pin' { $request.sources['extern/Native with spaces'] = 'a' * 40 }
            'unregistered-path' { $request.sources = @{ '../outside' = $pin } }
            'empty-pins' { $request.sources = @{} }
            'empty-owners' { $request.owners = @() }
            'wrong-kind' { $request.kind = 'Documentation' }
            'dirty-inputs' { 'Uncommitted engine input.' | Set-Content (Join-Path $root 'engine.txt') }
            'stale-head' {
                $null = Invoke-FixtureGit $root @('-c', 'user.name=Aaron Liao', '-c', 'user.email=hsuankailiao@gmail.com', 'commit', '--allow-empty', '-qm', 'Candidate moved')
            }
        }
        $requirements = Join-Path $output 'preparation.json'
        $request | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $requirements
        $failed = $false
        try {
            & (Join-Path $scripts 'Prepare-ValidationSources.ps1') -RequirementsPath $requirements -ExpectedCommit $commit *> (Join-Path $output 'preparation.log')
        } catch { $failed = $true; $_.Exception.Message | Set-Content (Join-Path $output 'failure.txt') }
        if ($failed -ne ($case -cne 'matching')) { throw "Unexpected preparation outcome: $case" }
        $prepared = Join-Path $root 'extern/Native with spaces'
        if ($case -ceq 'matching') {
            if ((Invoke-FixtureGit $prepared @('rev-parse', 'HEAD')) -cne $pin -or [IO.File]::ReadAllText((Join-Path $prepared 'source.txt')).Trim() -cne 'Pinned source bytes.') {
                throw 'Prepared checkout does not contain its exact source bytes.'
            }
            if ((Invoke-FixtureGit $root @('status', '--porcelain')).Trim()) { throw 'Preparation changed committed engine inputs.' }
            if (Test-Path -LiteralPath (Join-Path $output 'plan.json')) { throw 'Source preparation must not publish executable plans.' }
        } elseif (Test-Path -LiteralPath (Join-Path $prepared '.git')) {
            throw "Invalid request performed source checkout: $case"
        }
    }
} finally { $env:GIT_ALLOW_PROTOCOL = $originalProtocols }
Write-Output "Passed $($cases.Count) source preparation contracts using local Git only. Evidence: $directory"
$global:LASTEXITCODE = 0
