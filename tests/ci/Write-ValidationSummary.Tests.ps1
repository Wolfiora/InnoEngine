$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/summary-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$token = $env:GH_TOKEN
try {
    $env:GH_TOKEN = $null
    foreach ($case in @('complete', 'failed', 'missing')) {
        $received = Join-Path $directory "$case/received"
        $planDirectory = Join-Path $received 'validation-plan-fixture-host-attempt-1'
        $resultDirectory = Join-Path $received 'validation-result-fixture-host-managed-attempt-1'
        New-Item -ItemType Directory -Path $planDirectory, $resultDirectory -Force | Out-Null
        @{ checks = @(@{ id = 'contract'; kind = 'test' }); shards = @(@{ id = 'managed' }) } |
            ConvertTo-Json -Depth 5 | Set-Content (Join-Path $planDirectory 'plan.json')
        if ($case -cne 'missing') {
            @{ results = @(@{ id = 'contract'; seconds = 12.5; status = $(if ($case -ceq 'failed') { 'Failed' } else { 'Passed' }); detail = 'Retained first failure' }) } |
                ConvertTo-Json -Depth 5 | Set-Content (Join-Path $resultDirectory 'results.json')
        }
        $summary = Join-Path $directory "$case/summary.md"
        & "$PSScriptRoot/Write-ValidationSummary.ps1" -ReceivedDirectory $received -Hosts @('fixture-host') `
            -Repository fixture/engine -DefaultBranch master -Attempt 1 -RunId 1 -SummaryPath $summary
        $text = Get-Content -LiteralPath $summary -Raw
        if ($text -match 'windows-|macos-') { throw 'The summary invented an unselected host requirement.' }
        if ($case -ceq 'missing') {
            if ($text -notmatch 'complete results unavailable' -or $text -match '0 failed') { throw 'Missing receipts were presented as success.' }
        } elseif ($text -notmatch '12.5 check-seconds') {
            throw 'The summary lost check timing or confused parallel work with wall-clock time.'
        }
        if ($case -cne 'missing' -and $text -notmatch 'Required checks: 1. Reused results: 0. Inapplicable checks: 0.') {
            throw 'The summary lost the selection and reuse counts.'
        }
        if ($case -ceq 'failed' -and ($text -notmatch '1 failed or blocked' -or $text -notmatch 'Retained first failure')) {
            throw 'The summary lost the first failure.'
        }
    }
} finally { $env:GH_TOKEN = $token }
Write-Output "Passed 3 summary presentation contracts. Evidence: $directory"
