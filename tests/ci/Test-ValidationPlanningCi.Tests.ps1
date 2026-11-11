$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/ci-planning-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @(
    @{ name = 'missing-snapshot'; missing = 'documentation'; native = $true; empty = $false; exit = 0; throws = $true },
    @{ name = 'missing-api-capture'; missing = 'documentation-api-missing:owner'; native = $true; empty = $false; exit = 0; throws = $true },
    @{ name = 'managed-prose'; missing = ''; native = $false; empty = $false; exit = 0; throws = $false },
    @{ name = 'prepared-native'; missing = ''; native = $true; empty = $false; exit = 0; throws = $false },
    @{ name = 'empty-discovery'; missing = ''; native = $true; empty = $true; exit = 0; throws = $true },
    @{ name = 'selector-failure'; missing = ''; native = $true; empty = $false; exit = 1; throws = $true },
    @{ name = 'matching-docs'; docs = 'matching'; missing = ''; native = $false; empty = $false; exit = 0; throws = $false },
    @{ name = 'stale-docs'; docs = 'stale'; reason = 'Documentation does not bind the actual engine merge-candidate tree.'; throws = $true },
    @{ name = 'wrong-docs-revision'; docs = 'wrong-revision'; reason = 'Documentation checkout is not the requested candidate.'; throws = $true },
    @{ name = 'dirty-docs'; docs = 'dirty'; reason = 'Hosted documentation candidate contains uncommitted inputs.'; throws = $true },
    @{ name = 'wrong-docs-source'; docs = 'wrong-source'; reason = 'Canonical documentation binding does not identify this engine candidate.'; throws = $true }
)
$originalOutput = $env:GITHUB_OUTPUT
$originalDirectory = (Get-Location).Path
try {
    foreach ($case in $cases) {
        $root = Join-Path $directory $case.name
        $scripts = Join-Path $root 'tests/ci'
        New-Item -ItemType Directory -Path $scripts -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/Invoke-ValidationCi.ps1" -Destination $scripts
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
        $env:GITHUB_OUTPUT = Join-Path $root 'outputs.txt'
        [IO.File]::WriteAllText($env:GITHUB_OUTPUT, '')
        $threw = $false
        $failureReason = ''
        try {
            & (Join-Path $scripts 'Invoke-ValidationCi.ps1') -Stage Plan -BaseRevision $commit -ExpectedCommit $commit -Dotnet $stub @documentationArguments
        } catch { $threw = $true; $failureReason = $_.Exception.Message; $failureReason | Set-Content (Join-Path $root 'failure.log') }
        if ($threw -ne $case.throws) { throw "Unexpected planning failure state: $($case.name)" }
        if ($case.reason -and $failureReason -cne $case.reason) { throw "Unexpected rejection for $($case.name): $failureReason" }
        if ($case.missing -and $failureReason -notlike 'Required documentation inputs are unavailable;*') {
            throw "The fixture did not fail for its missing documentation requirement: $($case.name)"
        }
        $published = [IO.File]::ReadAllText($env:GITHUB_OUTPUT)
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
    }
} finally {
    $env:GITHUB_OUTPUT = $originalOutput
    Set-Location -LiteralPath $originalDirectory
}
Write-Output "Passed $($cases.Count) hosted planning protocol cases. SDK and source selection are validated separately. Evidence: $directory"
$global:LASTEXITCODE = 0
