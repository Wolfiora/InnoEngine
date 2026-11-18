$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/ci-gate-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$savedSummary = $env:GITHUB_STEP_SUMMARY
$savedEnvironment = $env:GITHUB_ENV
$cases = @('complete', 'missing-shard', 'changed-entry', 'wrong-candidate', 'failed-join', 'changed-shard', 'duplicate-shards', 'changed-result', 'wrong-platform', 'empty-hosts', 'duplicate-hosts')
try {
    foreach ($case in $cases) {
        $root = Join-Path $owner $case
        $scripts = Join-Path $root 'tests/ci'
        $received = Join-Path $root 'artifacts/validation/received'
        $planRoot = Join-Path $received 'validation-plan-fixture-attempt-1'
        $joinRoot = Join-Path $received 'validation-joined-fixture-attempt-1'
        New-Item -ItemType Directory -Path $scripts, $planRoot, $joinRoot -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/Join-ValidationCi.ps1", "$PSScriptRoot/Select-ValidationArtifact.ps1", "$PSScriptRoot/Test-ValidationReport.ps1" -Destination $scripts
        $executor = 'E' * 64
        @{ sha256 = $executor } | ConvertTo-Json | Set-Content (Join-Path $planRoot 'entry.json')
        $commit = if ($case -ceq 'wrong-candidate') { 'f' * 40 } else { 'a' * 40 }
        @{ commit = $commit; tree = 'b' * 64; environment = 'c' * 64; full = $true;
            checks = @(@{ id = 'policy'; kind = 'instructions'; exclusion = $null; prerequisites = @() });
            shards = @(@{ id = 'architecture'; checks = @('policy'); prerequisites = @() }) } |
            ConvertTo-Json -Depth 8 | Set-Content (Join-Path $planRoot 'plan.json')
        if ($case -cne 'missing-shard') {
            $receipt = Join-Path $received 'validation-result-fixture-architecture-attempt-1'
            New-Item -ItemType Directory -Path $receipt | Out-Null
            '{}' | Set-Content (Join-Path $receipt 'results.json')
        }
        $planHash = (Get-FileHash (Join-Path $planRoot 'plan.json')).Hash.ToLowerInvariant()
        $result = Join-Path $joinRoot 'results.json'
        @{ commit = $commit; tree = 'b' * 64; environment = 'c' * 64; completed = $case -cne 'failed-join'; planIdentity = $planHash;
            results = @(@{ id = 'policy'; status = 'Passed'; inputIdentity = 'd' * 64; outputs = @{} }) } |
            ConvertTo-Json -Depth 8 | Set-Content $result
        $shardHash = if ($case -cin @('missing-shard','changed-shard')) { '0' * 64 } else { (Get-FileHash (Join-Path $receipt 'results.json')).Hash.ToLowerInvariant() }
        $shards = @(@{ id = 'architecture'; sha256 = $shardHash })
        if ($case -ceq 'duplicate-shards') { $shards += $shards[0] }
        @{ planIdentity = $planHash; resultsIdentity = (Get-FileHash $result).Hash.ToLowerInvariant(); shards = $shards;
            platform = $(if ($case -ceq 'wrong-platform') { 'another-host' } else { 'fixture' });
            executor = $(if ($case -ceq 'changed-entry') { 'F' * 64 } else { $executor }) } |
            ConvertTo-Json -Depth 8 | Set-Content (Join-Path $joinRoot 'receipt.json')
        if ($case -ceq 'changed-result') { Add-Content $result ' ' }
        $env:GITHUB_STEP_SUMMARY = Join-Path $root 'summary.md'
        $env:GITHUB_ENV = Join-Path $root 'environment.txt'
        $hosts = if ($case -ceq 'duplicate-hosts') { @('fixture', 'fixture') } elseif ($case -ceq 'empty-hosts') { @() } else { @('fixture') }
        $failed = $false
        try {
            & (Join-Path $scripts 'Join-ValidationCi.ps1') -ReceivedDirectory $received -Hosts $hosts -ExpectedCommit ('a' * 40) -Attempt 1
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
        if ($failed -ne ($case -cne 'complete')) { throw "Unexpected final gate result: $case. See $root." }
        $certificate = Join-Path $root 'artifacts/validation/certification.json'
        if ((Test-Path $certificate) -ne ($case -ceq 'complete')) { throw 'An incomplete candidate received a final certification.' }
        if ($case -ceq 'complete') {
            if (-not (Test-Path (Join-Path $root 'artifacts/validation/joined/fixture/plan.json'))) { throw 'The original plan must accompany retained acceptance evidence.' }
            if ((Get-Content $env:GITHUB_ENV) -cne 'INNO_FULL_CERTIFICATION=true') { throw 'Complete acceptance was not identified explicitly.' }
        }
    }
} finally {
    $env:GITHUB_STEP_SUMMARY = $savedSummary
    $env:GITHUB_ENV = $savedEnvironment
}
Write-Output "Passed $($cases.Count) final gate transport contracts. Exact receipt semantics are tested through the real CLI. Evidence: $owner"
$global:LASTEXITCODE = 0
