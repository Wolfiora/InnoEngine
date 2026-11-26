$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/shard-transport-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @('retry-managed', 'native', 'absent-native', 'missing-prerequisite', 'changed-entry', 'escaped-entry', 'future-attempt')
$originalOutput = $env:GITHUB_OUTPUT
try {
    foreach ($case in $cases) {
        $root = Join-Path $owner $case
        $scripts = Join-Path $root 'tools/ci'
        $received = Join-Path $root 'artifacts/validation/received'
        $attempt = if ($case -ceq 'future-attempt') { 3 } else { 1 }
        $planDirectory = Join-Path $received "validation-plan-fixture-attempt-$attempt"
        $entryDirectory = Join-Path $received "validation-entry-fixture-attempt-$attempt"
        New-Item -ItemType Directory -Path $scripts, $planDirectory, $entryDirectory -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/../../tools/ci/Prepare-ValidationShard.ps1", "$PSScriptRoot/../../tools/ci/Select-ValidationArtifact.ps1" -Destination $scripts
        'prepared entry bytes' | Set-Content (Join-Path $entryDirectory 'Inno.Tooling.Cli.dll')
        $entry = 'artifacts/validation/bootstrap/outputs/bin/Inno.Tooling.Cli/debug/Inno.Tooling.Cli.dll'
        if ($case -ceq 'escaped-entry') { $entry = 'src/Inno.Tooling.Cli.dll' }
        @{ assembly = $entry; sha256 = $(if ($case -ceq 'changed-entry') { '0' * 64 } else { (Get-FileHash (Join-Path $entryDirectory 'Inno.Tooling.Cli.dll')).Hash }) } |
            ConvertTo-Json | Set-Content (Join-Path $planDirectory 'entry.json')
        $shard = if ($case -cin @('native', 'absent-native')) { 'native' } else { 'managed' }
        $shards = @(if ($case -cne 'absent-native') { @{ id = $shard; checks = @('fixture'); prerequisites = @('architecture') } })
        $requests = @(if ($case -ceq 'native') { @{ product = 'editor'; target = 'fixture'; configuration = 'Debug' } })
        @{ shards = $shards; nativeRequests = $requests } | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $planDirectory 'plan.json')
        if ($case -cne 'missing-prerequisite') {
            $receipt = Join-Path $received 'validation-result-fixture-architecture-attempt-1'
            New-Item -ItemType Directory -Path $receipt | Out-Null
            '{"completed":true}' | Set-Content (Join-Path $receipt 'results.json')
        }
        $env:GITHUB_OUTPUT = Join-Path $root 'outputs.txt'
        $failed = $false
        try {
            & (Join-Path $scripts 'Prepare-ValidationShard.ps1') -ReceivedDirectory $received -Platform fixture -Shard $shard -Attempt 2
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
        if ($failed -ne ($case -cnotin @('retry-managed', 'native', 'absent-native'))) { throw "Unexpected shard transport result: $case. See $root." }
        if (-not $failed) {
            $expected = 'native=' + ($case -ceq 'native').ToString().ToLowerInvariant()
            if ((Get-Content -LiteralPath $env:GITHUB_OUTPUT) -cne $expected) { throw 'A managed-only shard requested native download caching.' }
            if (-not (Test-Path (Join-Path $root 'artifacts/validation/ci-plan/plan.json'))) { throw 'The selected prior-attempt plan was not transferred.' }
        }
    }
} finally { $env:GITHUB_OUTPUT = $originalOutput }
Write-Output "Passed $($cases.Count) shard transport contracts. Receipt execution identities are validated by the CLI. Evidence: $owner"
$global:LASTEXITCODE = 0
