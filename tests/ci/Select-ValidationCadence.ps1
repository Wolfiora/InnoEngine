param(
    [Parameter(Mandatory)][ValidateSet('pull_request', 'push', 'merge_group', 'workflow_dispatch', 'schedule')][string]$Event,
    [Parameter(Mandatory)][string]$Commit,
    [string]$Schedule = '',
    [string]$LastCertifiedCommit = '',
    [bool]$ManualFull = $true,
    [DateTimeOffset]$Now = [DateTimeOffset]::UtcNow
)

$ErrorActionPreference = 'Stop'
if ($Commit -cnotmatch '^[0-9a-f]{40}$') { throw 'Cadence selection requires an exact candidate commit.' }
if ($LastCertifiedCommit -and $LastCertifiedCommit -cnotmatch '^[0-9a-f]{40}$') { throw 'The previous certification is not an exact commit.' }
$run = $true
$full = $false
$extended = $false
$reason = 'Evaluate affected owners on the actual candidate.'
if ($Event -eq 'workflow_dispatch') {
    $full = $ManualFull
    $reason = 'Explicit manual validation request.'
} elseif ($Event -eq 'schedule') {
    if ($Schedule -ceq '17 20 * * 6') {
        $full = $true
        $extended = $true
        $reason = 'Sunday 04:17 Taipei: complete functional, cold, ten warm and long-running evidence.'
    } elseif ($Schedule -ceq '17 19 * * *') {
        $full = $true
        if ($Now.ToUniversalTime().AddHours(8).DayOfWeek -eq [DayOfWeek]::Sunday) {
            $run = $false
            $reason = 'Sunday is owned by the 04:17 weekly validation; no duplicate daily run.'
        } elseif ($LastCertifiedCommit -ceq $Commit) {
            $run = $false
            $reason = 'No new default-branch commit since the last successful full certification.'
        } else {
            $reason = '03:17 Taipei: new default-branch input requires full functional certification.'
        }
    } else { throw 'Unknown scheduled cadence; required validation cannot be silently skipped.' }
}
[pscustomobject]@{ run = $run; full = $full; extended = $extended; reason = $reason; commit = $Commit }
