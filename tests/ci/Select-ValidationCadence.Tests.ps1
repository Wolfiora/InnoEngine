$ErrorActionPreference = 'Stop'
$commit = 'a' * 40
$script = Join-Path $PSScriptRoot 'Select-ValidationCadence.ps1'
$daily = @{ Event = 'schedule'; Commit = $commit; Schedule = '17 19 * * *' }
$fresh = & $script @daily
if (-not $fresh.run -or -not $fresh.full -or $fresh.extended -or $fresh.profile -cne 'full') { throw 'Nightly requires complete functional validation.' }
if (-not (& $script @daily).run) { throw 'Repeated event routing cannot certify SDK and dependency inputs before planning.' }
$weekly = & $script -Event schedule -Commit $commit -Schedule '17 20 * * 6'
if (-not $weekly.run -or $weekly.full -or $weekly.extended -or $weekly.profile -cne 'performance') { throw 'Weekly sampling must execute without cold dependency production.' }
foreach ($event in @('pull_request', 'merge_group')) {
    $selected = & $script -Event $event -Commit $commit -HasOpenPullRequest $true
    if (-not $selected.run -or $selected.full -or $selected.profile -cne 'pull-request') { throw 'Candidate and merge-queue checks must remain present.' }
}
$branch = & $script -Event push -Commit $commit -IsDefaultBranch $false
if (-not $branch.run -or $branch.profile -cne 'fast') { throw 'Branches need the fast profile.' }
if ((& $script -Event push -Commit $commit -IsDefaultBranch $false -HasOpenPullRequest $true).run) { throw 'PR-owned pushes must not duplicate candidate execution.' }
$master = & $script -Event push -Commit $commit -IsDefaultBranch $true -HasOpenPullRequest $true
if (-not $master.run -or $master.profile -cne 'master') { throw 'Default-branch integration must not be skipped by PR routing.' }
if (-not (& $script -Event workflow_dispatch -Commit $commit).full) { throw 'Manual complete acceptance was lost.' }
$rejected = $false
try { $null = & $script -Event schedule -Commit $commit -Schedule invalid } catch { $rejected = $true }
if (-not $rejected) { throw 'Unknown schedules must fail closed.' }
Write-Output 'Cadence contracts passed. Exact input and environment reuse is covered by validation acceptance contracts.'
