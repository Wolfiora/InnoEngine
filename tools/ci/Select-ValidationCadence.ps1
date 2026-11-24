param(
    [Parameter(Mandatory)][ValidateSet('pull_request', 'push', 'merge_group', 'workflow_dispatch')][string]$Event,
    [Parameter(Mandatory)][string]$Commit,
    [bool]$IsDefaultBranch = $true,
    [bool]$HasOpenPullRequest = $false,
    [bool]$ManualFull = $true
)

$ErrorActionPreference = 'Stop'
if ($Commit -cnotmatch '^[0-9a-f]{40}$') { throw 'Cadence selection requires an exact candidate commit.' }
$run = $true
$full = $false
$profile = 'pull-request'
$reason = 'Evaluate affected owners on the actual candidate.'
if ($Event -ceq 'workflow_dispatch') {
    $full = $ManualFull
    $profile = if ($full) { 'full' } else { 'pull-request' }
    $reason = 'Explicit manual validation request.'
} elseif ($Event -ceq 'push') {
    if ($IsDefaultBranch) {
        $profile = 'master'
        $reason = 'Expand affected product and publication checks on the default branch.'
    } elseif ($HasOpenPullRequest) {
        $run = $false
        $profile = 'fast'
        $reason = 'The open pull request owns candidate validation; this push only routes the event.'
    } else {
        $profile = 'fast'
        $reason = 'An ordinary branch push runs affected managed compilation and fast contracts.'
    }
}
[pscustomobject]@{ run = $run; full = $full; extended = $false; profile = $profile; reason = $reason; commit = $Commit }
