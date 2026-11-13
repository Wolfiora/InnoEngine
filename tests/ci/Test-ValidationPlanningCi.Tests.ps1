$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/ci-planning-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @(
    @{ name = 'ordinary-managed'; native = $false; empty = $false; exit = 0; throws = $false },
    @{ name = 'prepared-native'; native = $true; empty = $false; exit = 0; throws = $false },
    @{ name = 'empty-discovery'; native = $true; empty = $true; exit = 0; throws = $true },
    @{ name = 'selector-failure'; native = $true; empty = $false; exit = 1; throws = $true },
    @{ name = 'native-preparation'; prepare = 'once'; native = $true; throws = $false },
    @{ name = 'native-still-blocked'; prepare = 'again'; reason = 'Validation plan generation failed; no downstream stage can certify success.'; throws = $true },
    @{ name = 'native-wrong-pin'; prepare = 'wrong-pin'; reason = 'Requested source is not the current pinned gitlink: extern/Native'; throws = $true }
)
$originalOutput = $env:GITHUB_OUTPUT
$originalDirectory = (Get-Location).Path
$originalProtocols = $env:GIT_ALLOW_PROTOCOL
$env:GIT_ALLOW_PROTOCOL = 'file'
try {
    foreach ($case in $cases) {
        $root = Join-Path $directory $case.name
        $scripts = Join-Path $root 'tests/ci'
        New-Item -ItemType Directory -Path $scripts -Force | Out-Null
        foreach ($script in @('Invoke-ValidationCi.ps1', 'Initialize-ValidationEntry.ps1', 'Prepare-ValidationSources.ps1')) {
            Copy-Item -LiteralPath (Join-Path $PSScriptRoot $script) -Destination $scripts
        }
        'fixture' | Set-Content (Join-Path $root 'source.txt')
        & git -C $root init -q
        if ($LASTEXITCODE -ne 0) { throw 'Cannot initialize the planning fixture.' }
        & git -C $root add source.txt
        & git -C $root -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit -qm 'Planning protocol fixture'
        if ($LASTEXITCODE -ne 0) { throw 'Cannot commit the planning fixture.' }
        $commit = (& git -C $root rev-parse HEAD).Trim()
        $case | ConvertTo-Json | Set-Content (Join-Path $root 'case.json')
        $stub = Join-Path $root 'dotnet-fixture.ps1'
        @'
$Arguments = @($args)
$case = Get-Content (Join-Path $PSScriptRoot 'case.json') -Raw | ConvertFrom-Json
$assembly = Join-Path $PSScriptRoot 'entry.dll'
$global:LASTEXITCODE = 0
if ($Arguments[0] -ceq 'build') { 'prepared fixture entry' | Set-Content $assembly; return }
if ($Arguments[0] -ceq 'msbuild') { Write-Output $assembly; return }
if ($Arguments[1] -ceq 'docs') { throw 'Engine CI must not prepare a paired Docs candidate.' }
if ($Arguments[1] -cne 'validate' -or $Arguments[2] -cne 'plan') { throw 'Unexpected hosted planning operation.' }
$output = $Arguments[[Array]::IndexOf($Arguments, '--output') + 1]
if ($case.prepare) {
    $attempts = Join-Path $PSScriptRoot 'artifacts/planning-attempts.txt'
    'plan' | Add-Content $attempts
    $count = @(Get-Content $attempts).Count
    if ($count -eq 1 -or $case.prepare -ceq 'again') {
        $commit = (& git -C $PSScriptRoot rev-parse HEAD).Trim()
        $pin = if ($case.prepare -ceq 'wrong-pin') { 'a' * 40 } else { $case.pin }
        @{ status = 'Blocked'; kind = 'NativeSources'; root = $PSScriptRoot; commit = $commit
            owners = @('src/Native/Native.csproj'); sources = @{ 'extern/Native' = $pin } } |
            ConvertTo-Json -Depth 5 | Set-Content (Join-Path $output 'preparation.json')
        $global:LASTEXITCODE = 2
        return
    }
    if (-not (Test-Path (Join-Path $PSScriptRoot 'extern/Native/.git'))) { throw 'Planning continued without its requested source inputs.' }
}
$checks = @()
if (-not $case.empty) {
    $checks += @{ id = 'instructions'; kind = 'instructions'; exclusion = $null }
}
$contexts = @{}
if ($case.native) { $contexts.native = @{} }
@{ checks = $checks; contexts = $contexts } | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $output 'plan.json')
$global:LASTEXITCODE = [int]$case.exit
'@ | Set-Content $stub
        if ($case.prepare) {
            $source = Join-Path $root 'artifacts/source'
            New-Item -ItemType Directory -Path $source -Force | Out-Null
            'source fixture' | Set-Content (Join-Path $source 'source.txt')
            & git -C $source init -q
            & git -C $source add source.txt
            & git -C $source -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit -qm 'Source preparation fixture'
            if ($LASTEXITCODE -ne 0) { throw 'Cannot commit the source fixture.' }
            $case.pin = (& git -C $source rev-parse HEAD).Trim()
            $case | ConvertTo-Json | Set-Content (Join-Path $root 'case.json')
            "artifacts/`noutputs.txt`nentry.dll`nfailure.log`n" | Set-Content (Join-Path $root '.gitignore')
            & git -C $root config -f .gitmodules submodule.native.path extern/Native
            & git -C $root config -f .gitmodules submodule.native.url $source.Replace('\', '/')
            & git -C $root add .
            & git -C $root update-index --add --cacheinfo "160000,$($case.pin),extern/Native"
            & git -C $root -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit -qm 'Complete hosted preparation fixture'
            if ($LASTEXITCODE -ne 0) { throw 'Cannot commit the hosted preparation fixture.' }
            New-Item -ItemType Directory -Path (Join-Path $root 'extern/Native') -Force | Out-Null
            $commit = (& git -C $root rev-parse HEAD).Trim()
        }
        $env:GITHUB_OUTPUT = Join-Path $root 'outputs.txt'
        [IO.File]::WriteAllText($env:GITHUB_OUTPUT, '')
        $threw = $false
        $failureReason = ''
        try {
            & (Join-Path $scripts 'Invoke-ValidationCi.ps1') -Stage Plan -BaseRevision $commit -ExpectedCommit $commit -Dotnet $stub
        } catch { $threw = $true; $failureReason = $_.Exception.Message; $failureReason | Set-Content (Join-Path $root 'failure.log') }
        if ($threw -ne $case.throws) { throw "Unexpected planning failure state: $($case.name)" }
        if ($case.reason -and $failureReason -cne $case.reason) { throw "Unexpected rejection for $($case.name): $failureReason" }
        $published = [IO.File]::ReadAllText($env:GITHUB_OUTPUT)
        if ($case.throws) {
            if ($published.Trim()) { throw "Blocked planning published downstream execution outputs: $($case.name)" }
        } elseif ($published.Trim() -cne "native=$($case.native.ToString().ToLowerInvariant())") {
            throw "The prepared plan did not publish its actual environment requirement: $($case.name)"
        }
        if ($case.prepare) {
            $attempts = @(Get-Content (Join-Path $root 'artifacts/planning-attempts.txt')).Count
            $expectedAttempts = if ($case.prepare -ceq 'wrong-pin') { 1 } else { 2 }
            if ($attempts -ne $expectedAttempts) { throw 'Source preparation must regenerate once and must never retry repeatedly until green.' }
            if (-not (Test-Path (Join-Path $root 'artifacts/validation/ci/initial-preparation.json'))) {
                throw 'The first blocked preparation request was not retained.'
            }
        }
    }
} finally {
    $env:GITHUB_OUTPUT = $originalOutput
    $env:GIT_ALLOW_PROTOCOL = $originalProtocols
    Set-Location -LiteralPath $originalDirectory
}
Write-Output "Passed $($cases.Count) hosted planning protocol cases. SDK and source selection are validated separately. Evidence: $directory"
$global:LASTEXITCODE = 0
