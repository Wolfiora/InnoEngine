$ErrorActionPreference = 'Stop'
$commit = 'a' * 40
$script = Join-Path $PSScriptRoot '../../tools/ci/Select-ValidationCadence.ps1'
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
try { $null = & $script -Event schedule -Commit $commit } catch { $rejected = $true }
if (-not $rejected) { throw 'Timed events must not select an engine validation profile.' }
Write-Output 'Event routing and manual full validation contracts passed.'
