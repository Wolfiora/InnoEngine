$ErrorActionPreference = 'Stop'
$commit = 'a' * 40
$passed = 0
function Invoke-Case([string]$Name, [scriptblock]$Change, [bool]$Accept) {
    $state = @{
        repository = @{ id = 123; default_branch = 'master' }
        run = @{ id = 45; run_attempt = 1; workflow_id = 67; path = '.github/workflows/rendering-ci.yml'; status = 'completed'; conclusion = 'success';
            head_repository = @{ id = 123 }; head_sha = $commit; head_branch = 'master'; event = 'push' }
        artifacts = @('engine-full-certification', 'deliverable-windows-2022', 'deliverable-macos-15') | ForEach-Object {
            @{ name = $_ + '-attempt-1'; expired = $false; expires_at = [DateTimeOffset]::UtcNow.AddDays(1).ToString('O'); digest = 'sha256:' + ('b' * 64);
                workflow_run = @{ id = 45; head_sha = $commit; repository_id = 123 } }
        }
    }
    & $Change $state
    $accepted = $false
    try {
        $null = & "$PSScriptRoot/Test-ReleaseSource.ps1" -Repository $state.repository -Run $state.run -Artifacts $state.artifacts `
            -Commit $commit -WorkflowId 67 -RunId 45
        $accepted = $true
    } catch { if ($Accept) { throw } }
    if ($accepted -ne $Accept) { throw "Unexpected release provenance result: $Name" }
    $script:passed++
}
Invoke-Case 'trusted-default-branch' { param($s) } $true
Invoke-Case 'wrong-repository' { param($s) $s.run.head_repository.id = 999 } $false
Invoke-Case 'pull-request' { param($s) $s.run.event = 'pull_request' } $false
Invoke-Case 'feature-branch' { param($s) $s.run.head_branch = 'feature' } $false
Invoke-Case 'wrong-commit' { param($s) $s.run.head_sha = 'c' * 40 } $false
Invoke-Case 'failed-run' { param($s) $s.run.conclusion = 'failure' } $false
Invoke-Case 'canceled-run' { param($s) $s.run.conclusion = 'cancelled' } $false
Invoke-Case 'different-workflow' { param($s) $s.run.workflow_id = 100 } $false
Invoke-Case 'different-run' { param($s) $s.run.id = 100 } $false
Invoke-Case 'expired' { param($s) $s.artifacts[0].expired = $true } $false
Invoke-Case 'past-retention' { param($s) $s.artifacts[0].expires_at = [DateTimeOffset]::UtcNow.AddDays(-1).ToString('O') } $false
Invoke-Case 'missing-platform' { param($s) $s.artifacts = $s.artifacts[0..1] } $false
Invoke-Case 'missing-digest' { param($s) $s.artifacts[1].digest = '' } $false
Invoke-Case 'wrong-artifact-commit' { param($s) $s.artifacts[1].workflow_run.head_sha = 'c' * 40 } $false
Invoke-Case 'wrong-artifact-run' { param($s) $s.artifacts[1].workflow_run.id = 999 } $false
Invoke-Case 'future-attempt' { param($s) $s.artifacts[1].name = 'deliverable-windows-2022-attempt-2' } $false
Invoke-Case 'old-certification' { param($s) $s.run.run_attempt = 2 } $false
Invoke-Case 'duplicate-current-artifact' { param($s) $s.artifacts += $s.artifacts[0] } $false
Invoke-Case 'retry-reconciles-previous-successful-platform' {
    param($s)
    $s.run.run_attempt = 2
    $s.artifacts[0].name = 'engine-full-certification-attempt-2'
    $s.artifacts[2].name = 'deliverable-macos-15-attempt-2'
} $true
Write-Output "Release provenance contracts passed: $passed."
