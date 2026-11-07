$ErrorActionPreference = 'Stop'
$commit = 'a' * 40
$script = Join-Path $PSScriptRoot 'Select-ValidationCadence.ps1'
$daily = @{ Event = 'schedule'; Commit = $commit; Schedule = '17 19 * * *'; Now = [DateTimeOffset]::Parse('2026-11-04T19:17:00Z') }
$fresh = & $script @daily
if (-not $fresh.run -or -not $fresh.full -or $fresh.extended) { throw 'New daily input must run the complete functional matrix.' }
$unchanged = & $script @daily -LastCertifiedCommit $commit
if ($unchanged.run) { throw 'Unchanged certified input must not trigger another daily run.' }
$daily.Now = [DateTimeOffset]::Parse('2026-11-07T19:17:00Z')
if ((& $script @daily).run) { throw 'Sunday must not duplicate the weekly run.' }
$weekly = & $script -Event schedule -Commit $commit -Schedule '17 20 * * 6' -LastCertifiedCommit $commit
if (-not $weekly.run -or -not $weekly.full -or -not $weekly.extended) { throw 'Weekly depth must execute even without source changes.' }
foreach ($event in @('pull_request', 'push', 'merge_group')) {
    $selected = & $script -Event $event -Commit $commit -LastCertifiedCommit $commit
    if (-not $selected.run -or $selected.full) { throw 'Candidate checks must remain present and use actual impact analysis.' }
}
if (-not (& $script -Event workflow_dispatch -Commit $commit).full) { throw 'Manual full validation was lost.' }
$rejected = $false
try { $null = & $script -Event schedule -Commit $commit -Schedule invalid } catch { $rejected = $true }
if (-not $rejected) { throw 'Unknown schedules must fail closed.' }
Write-Output 'Cadence contracts passed: 9.'
