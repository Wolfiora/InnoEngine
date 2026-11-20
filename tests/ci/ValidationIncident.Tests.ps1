$ErrorActionPreference = 'Stop'
$incident = Get-Content "$PSScriptRoot/fixtures/pr88-managed-failure.json" -Raw | ConvertFrom-Json
$commit = 'a' * 40
$hash = 'b' * 64
$owner = [IO.Directory]::CreateTempSubdirectory('InnoValidationIncident-').FullName
$token = $env:GH_TOKEN
try {
    $env:GH_TOKEN = $null
    $planned = @(1..$incident.passed | ForEach-Object { "unchanged-$_" }) + $incident.failedOwner
    $checks = @($planned | ForEach-Object { @{id=$_;kind='instructions';prerequisites=@()} })
    $plan = @{commit=$commit;tree=$hash;environment=$hash;checks=$checks;shards=@(@{id='managed';checks=$planned})}
    $planRoot = New-Item -ItemType Directory -Path (Join-Path $owner 'validation-plan-windows-attempt-1')
    $resultRoot = New-Item -ItemType Directory -Path (Join-Path $owner 'validation-result-windows-managed-attempt-1')
    $planPath = Join-Path $planRoot.FullName 'plan.json'
    $path = Join-Path $resultRoot.FullName 'results.json'
    $plan | ConvertTo-Json -Depth 8 | Set-Content $planPath
    $results = @(foreach ($id in $planned) {
        if ($id -ceq $incident.failedOwner) {
            @{id=$id;status='Failed';inputIdentity=$hash;seconds=0;detail="$($incident.test): expected $($incident.expected), observed $($incident.actual); TRX duration $($incident.trxDuration)"}
        } else { @{id=$id;status='Passed';inputIdentity=$hash;seconds=0;outputs=@{}} }
    })
    $legacyIds = @($results.id) + $incident.legacyExtraId
    if ($legacyIds.Count -ne 301 -or $checks.Count -ne 300 -or $incident.gate -cne 'failure') { throw 'The incident fixture lost the original count mismatch.' }
    foreach ($case in @('failed-owner','missing-owner')) {
        $evidence = @{version=2;commit=$commit;environment=$hash;tree=$hash;completed=$true;shard='managed'
            planIdentity=(Get-FileHash $planPath).Hash.ToLowerInvariant();results=$results
            failures=@(@{stage='test-execution';owner='managed';log='managed-tests.log';seconds=$incident.batchSeconds;detail="Shared tests exited with $($incident.exit)"})}
        if ($case -ceq 'missing-owner') { $evidence.results = @($results | Where-Object id -CNE 'unchanged-1') }
        $evidence | ConvertTo-Json -Depth 8 | Set-Content $path
        $summary = Join-Path $owner "$case.md"
        & "$PSScriptRoot/Write-ValidationSummary.ps1" -ReceivedDirectory $owner -Repository fixture/engine -DefaultBranch master -Attempt 1 -RunId 1 -Hosts windows -SummaryPath $summary
        $message = Get-Content $summary -Raw
        if (!$message.Contains($incident.test) -or !$message.Contains($incident.trxDuration) -or !$message.Contains('Shared tests exited with 1')) { throw 'The first failed test or independent process failure was lost.' }
        if ($case -ceq 'failed-owner' -and !$message.Contains('All 300 planned checks returned results.')) { throw 'A fully accounted failed run was reported as incomplete.' }
        if ($case -ceq 'missing-owner' -and !$message.Contains('Missing planned checks: unchanged-1')) { throw 'The missing check was not identified.' }
        $rejected = $false
        try { & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath $planPath -ResultsPath $path -ExpectedCommit $commit -JobConclusion success } catch { $rejected = $true }
        if (!$rejected) { throw 'A failed incident was certified.' }
    }
} finally {
    $env:GH_TOKEN = $token
    $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
    if ([IO.Path]::GetDirectoryName($owner) -eq $temp.TrimEnd([IO.Path]::DirectorySeparatorChar) -and [IO.Path]::GetFileName($owner).StartsWith('InnoValidationIncident-')) {
        Remove-Item -LiteralPath $owner -Recurse -Force
    }
}
Write-Output 'Passed complete and missing PR 88 incident receipts.'
$global:LASTEXITCODE = 0
