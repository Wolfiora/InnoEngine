$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/ci-planning-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @(
    @{ name = 'missing-snapshot'; missing = 'documentation'; native = $true; empty = $false; exit = 0; throws = $true },
    @{ name = 'missing-api-capture'; missing = 'documentation-api-missing:owner'; native = $true; empty = $false; exit = 0; throws = $false },
    @{ name = 'managed-prose'; missing = ''; native = $false; empty = $false; exit = 0; throws = $false },
    @{ name = 'prepared-native'; missing = ''; native = $true; empty = $false; exit = 0; throws = $false },
    @{ name = 'empty-discovery'; missing = ''; native = $true; empty = $true; exit = 0; throws = $true },
    @{ name = 'selector-failure'; missing = ''; native = $true; empty = $false; exit = 1; throws = $true },
    @{ name = 'matching-docs'; docs = 'matching'; missing = ''; native = $false; empty = $false; exit = 0; throws = $false },
    @{ name = 'stale-docs'; docs = 'stale'; reason = 'Documentation does not bind the actual engine merge-candidate tree.'; throws = $true },
    @{ name = 'wrong-docs-revision'; docs = 'wrong-revision'; reason = 'Documentation checkout is not the requested candidate.'; throws = $true },
    @{ name = 'dirty-docs'; docs = 'dirty'; reason = 'Hosted documentation candidate contains uncommitted inputs.'; throws = $true },
    @{ name = 'wrong-docs-source'; docs = 'wrong-source'; reason = 'Canonical documentation binding does not identify this engine candidate.'; throws = $true },
    @{ name = 'native-preparation'; docs = 'matching'; prepare = 'once'; native = $true; throws = $false },
    @{ name = 'native-still-blocked'; docs = 'matching'; prepare = 'again'; reason = 'Validation plan generation failed; no downstream stage can certify success.'; throws = $true },
    @{ name = 'native-wrong-pin'; docs = 'matching'; prepare = 'wrong-pin'; reason = 'Requested source is not the current pinned gitlink: extern/Native'; throws = $true },
    @{ name = 'native-before-docs'; prepare = 'once'; reason = 'Hosted preparation requires the explicitly bound documentation candidate.'; throws = $true }
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
        $documentationArguments = @{}
        if ($case.docs) {
            $documentation = Join-Path $root 'documentation'
            New-Item -ItemType Directory -Path $documentation | Out-Null
            'Fixture documentation.' | Set-Content (Join-Path $documentation 'page.md')
            & git -C $documentation init -q
            if ($LASTEXITCODE -ne 0) { throw 'Cannot initialize the documentation fixture.' }
            & git -C $documentation add page.md
            & git -C $documentation -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit -qm 'Documentation protocol fixture'
            if ($LASTEXITCODE -ne 0) { throw 'Cannot commit the documentation fixture.' }
            $documentationRevision = (& git -C $documentation rev-parse HEAD).Trim()
            $documentationArguments = @{ DocumentationRoot = $documentation; DocumentationRevision = $documentationRevision }
            if ($case.docs -eq 'wrong-revision') { $documentationArguments.DocumentationRevision = '0' * 40 }
            if ($case.docs -eq 'dirty') { 'Uncommitted edit.' | Add-Content (Join-Path $documentation 'page.md') }
        }
        $case | ConvertTo-Json | Set-Content (Join-Path $root 'case.json')
        $stub = Join-Path $root 'dotnet-fixture.ps1'
        @'
$Arguments = @($args)
$case = Get-Content (Join-Path $PSScriptRoot 'case.json') -Raw | ConvertFrom-Json
$assembly = Join-Path $PSScriptRoot 'entry.dll'
$global:LASTEXITCODE = 0
if ($Arguments[0] -ceq 'build') { 'prepared fixture entry' | Set-Content $assembly; return }
if ($Arguments[0] -ceq 'msbuild') { Write-Output $assembly; return }
if ($Arguments[1] -ceq 'docs') {
    if ($Arguments[2] -ceq 'index') { @{ snapshot = 'a' * 64 } | ConvertTo-Json; return }
    if ($Arguments[2] -cne 'catalog') { throw 'Unexpected documentation operation.' }
    if ($case.docs -ceq 'stale') { $global:LASTEXITCODE = 2; return }
    $sourceTree = $Arguments[[Array]::IndexOf($Arguments, '--source-tree') + 1]
    $repository = if ($case.docs -ceq 'wrong-source') { 'https://github.com/example/other' } else { 'https://github.com/Wolfiora/InnoEngine' }
    @{ snapshot = 'a' * 64; contentIdentity = 'b' * 64; sourceTree = $sourceTree; repository = $repository
        toolingRevision = 'c' * 40; sourceApiStatus = 'Unverified' } | ConvertTo-Json
    return
}
if ($Arguments[1] -cne 'validate' -or $Arguments[2] -cne 'plan') { throw 'Unexpected hosted planning operation.' }
if ($case.docs -and ($Arguments[[Array]::IndexOf($Arguments, '--docs-snapshot') + 1] -cne ('a' * 64))) {
    throw 'The frozen documentation selection was not forwarded to planning.'
}
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
    if ($case.missing) { $checks += @{ id = $case.missing; kind = 'documentation-missing'; exclusion = $null } }
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
            "artifacts/`ndocumentation/`noutputs.txt`nentry.dll`nfailure.log`n" | Set-Content (Join-Path $root '.gitignore')
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
            & (Join-Path $scripts 'Invoke-ValidationCi.ps1') -Stage Plan -BaseRevision $commit -ExpectedCommit $commit -Dotnet $stub @documentationArguments
        } catch { $threw = $true; $failureReason = $_.Exception.Message; $failureReason | Set-Content (Join-Path $root 'failure.log') }
        if ($threw -ne $case.throws) { throw "Unexpected planning failure state: $($case.name)" }
        if ($case.reason -and $failureReason -cne $case.reason) { throw "Unexpected rejection for $($case.name): $failureReason" }
        if ($case.missing -ceq 'documentation' -and $failureReason -notlike 'Required documentation inputs are unavailable;*') {
            throw "The fixture did not fail for its missing documentation requirement: $($case.name)"
        }
        $published = [IO.File]::ReadAllText($env:GITHUB_OUTPUT)
        if ($case.missing -and -not $case.throws) {
            $planned = Get-Content (Join-Path $root 'artifacts/validation/ci/plan.json') -Raw | ConvertFrom-Json
            $required = @($planned.checks | Where-Object { $_.id -ceq $case.missing -and $_.kind -ceq 'documentation-missing' -and $null -eq $_.exclusion })
            if ($required.Count -ne 1) { throw 'Independent execution removed or excluded the required missing API result.' }
        }
        if ($case.throws) {
            if ($published.Trim()) { throw "Blocked planning published downstream execution outputs: $($case.name)" }
        } elseif ($published.Trim() -cne "native=$($case.native.ToString().ToLowerInvariant())") {
            throw "The prepared plan did not publish its actual environment requirement: $($case.name)"
        }
        if ($case.docs -eq 'matching') {
            $pairing = Get-Content (Join-Path $root 'artifacts/validation/ci/documentation-pairing.json') -Raw | ConvertFrom-Json
            if ($pairing.engineCommit -cne $commit -or $pairing.documentationCommit -cne $documentationRevision `
                -or $pairing.sourceApiStatus -cne 'Unverified') { throw 'Candidate binding was lost or incorrectly promoted to source certification.' }
        }
        if ($case.prepare) {
            $attempts = @(Get-Content (Join-Path $root 'artifacts/planning-attempts.txt')).Count
            $expectedAttempts = if ($case.prepare -ceq 'wrong-pin' -or -not $case.docs) { 1 } else { 2 }
            if ($attempts -ne $expectedAttempts) { throw 'Source preparation must regenerate once and must never retry repeatedly until green.' }
            if ($case.docs -and -not (Test-Path (Join-Path $root 'artifacts/validation/ci/initial-preparation.json'))) {
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
