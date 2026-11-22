$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/acceptance-join-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$originalOutput = $env:GITHUB_OUTPUT
$originalSummary = $env:GITHUB_STEP_SUMMARY
$cases = @('complete', 'missing-decision', 'different-environment', 'wrong-commit', 'wrong-host', 'changed-plan', 'missing-source', 'duplicate-host', 'incomplete-original')
try {
    foreach ($case in $cases) {
        $root = Join-Path $owner $case
        $received = Join-Path $root 'received'
        foreach ($platform in @('host-a', 'host-b')) {
            $directory = Join-Path $received "validation-preflight-$platform-attempt-1"
            New-Item -ItemType Directory -Path $directory -Force | Out-Null
            '{}' | Set-Content (Join-Path $directory 'plan.json')
            $decision = @{ reusable = $true; platform = $platform; commit = 'a' * 40;
                planIdentity = (Get-FileHash (Join-Path $directory 'plan.json')).Hash.ToLowerInvariant(); acceptedPlanIdentity = 'b' * 64;
                sourceRun = 17; sourceUrl = 'https://github.com/fixture/repo/actions/runs/17'; evidenceDigest = 'sha256:' + ('c' * 64);
                receipt = @{ full = $true; planIdentity = 'b' * 64 } }
            if ($platform -ceq 'host-b') {
                switch ($case) {
                    'missing-decision' { continue }
                    'different-environment' { $decision.reusable = $false; $decision.continuationRun = 19 }
                    'wrong-commit' { $decision.commit = 'f' * 40 }
                    'wrong-host' { $decision.platform = 'host-a' }
                    'changed-plan' { $decision.planIdentity = 'f' * 64 }
                    'missing-source' { $decision.sourceRun = 0 }
                    'incomplete-original' { $decision.receipt.full = $false }
                }
            }
            if ($case -cne 'missing-decision' -or $platform -cne 'host-b') {
                $decision | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $directory 'acceptance-reuse.json')
            }
        }
        $env:GITHUB_OUTPUT = Join-Path $root 'outputs.txt'
        $env:GITHUB_STEP_SUMMARY = Join-Path $root 'summary.md'
        $hosts = if ($case -ceq 'duplicate-host') { @('host-a', 'host-a') } else { @('host-a', 'host-b') }
        $failed = $false
        try {
            & "$PSScriptRoot/../../tools/ci/Join-ValidationAcceptance.ps1" -ReceivedDirectory $received -Hosts $hosts -ExpectedCommit ('a' * 40) `
                -Attempt 1 -OutputPath (Join-Path $root 'decision.json')
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
        if ($failed -ne ($case -cnotin @('complete', 'missing-decision', 'different-environment'))) { throw "Unexpected acceptance matrix result: $case. See $root." }
        if (-not $failed) {
            $actual = Get-Content (Join-Path $root 'decision.json') -Raw | ConvertFrom-Json
            if ($case -ceq 'different-environment' -and !(Select-String -LiteralPath $env:GITHUB_OUTPUT -Pattern '^continuations=\{"host-b":19\}$' -Quiet)) { throw 'Missing per-host continuation routing.' }
            if ($actual.reusable -ne ($case -ceq 'complete')) { throw 'Incomplete or stale acceptance suppressed required execution.' }
        }
    }
} finally {
    $env:GITHUB_OUTPUT = $originalOutput
    $env:GITHUB_STEP_SUMMARY = $originalSummary
}
Write-Output "Passed $($cases.Count) complete-acceptance matrix contracts. Evidence: $owner"
$global:LASTEXITCODE = 0
