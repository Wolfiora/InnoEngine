param(
    [Parameter(Mandatory)][string]$ReceivedDirectory,
    [Parameter(Mandatory)][string]$Repository,
    [Parameter(Mandatory)][int]$Attempt,
    [Parameter(Mandatory)][long]$RunId,
    [string[]]$Hosts,
    [string]$ApiUrl = 'https://api.github.com',
    [string]$SummaryPath = $env:GITHUB_STEP_SUMMARY
)

$ErrorActionPreference = 'Stop'
$ReceivedDirectory = [IO.Path]::GetFullPath($ReceivedDirectory)
$lines = [Collections.Generic.List[string]]::new()
$lines.Add('## Validation evidence')
$lines.Add('')
$available = if (Test-Path -LiteralPath $ReceivedDirectory) {
    @(Get-ChildItem -LiteralPath $ReceivedDirectory -Directory | ForEach-Object { [pscustomobject]@{ name = $_.Name; directory = $_.FullName } })
} else { @() }
if (-not $Hosts) { $Hosts = @((Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/validation-hosts.json') -Raw | ConvertFrom-Json).runner) }
foreach ($platform in $Hosts) {
    try {
        $artifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-plan-$platform" -MaximumAttempt $Attempt
        $plan = Get-Content -LiteralPath (Join-Path $artifact.directory 'plan.json') -Raw | ConvertFrom-Json
        $lines.Add("### $platform")
        $lines.Add("")
        $lines.Add("Affected files: " + (@($plan.changes) -join ', '))
        $missing = [Collections.Generic.List[string]]::new()
        $faults = [Collections.Generic.List[object]]::new()
        $results = @(foreach ($shard in @($plan.shards)) {
            try {
                $receipt = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-result-$platform-$($shard.id)" -MaximumAttempt $Attempt
                $part = Get-Content -LiteralPath (Join-Path $receipt.directory 'results.json') -Raw | ConvertFrom-Json
                if ($part.version -ne 2 -or $part.failures -isnot [array] -or $part.commit -cne $plan.commit) { throw 'Invalid receipt identity or format.' }
                foreach ($batch in $part.batchSeconds.PSObject.Properties | Where-Object MemberType -EQ NoteProperty) {
                    $lines.Add("  - $platform/$($batch.Name) test process: $([Math]::Round($batch.Value, 2)) s.")
                }
                if ($part.closingSeconds) { $lines.Add("  - $platform/$($shard.id) final evidence verification: $([Math]::Round($part.closingSeconds, 2)) s.") }
                foreach ($fault in $part.failures) { $faults.Add($fault) }
                $part.results
            } catch { $missing.Add($shard.id) }
        })
        $failed = @($results | Where-Object { $_.status -cin @('Failed', 'Blocked') })
        if ($missing.Count) {
            $lines.Add("- ${platform}: complete results unavailable for shards " + ($missing -join ', '))
        } else {
            $lines.Add("- ${platform}: $($failed.Count) failed or blocked checks; frozen plan: $($artifact.name).")
        }
        $seenIds = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
        foreach ($result in $results) {
            if (!$seenIds.Add($result.id)) { $lines.Add("  - Duplicate check: $($result.id)") }
            if ($result.id -cnotin $plan.checks.id) { $lines.Add("  - Unplanned check: $($result.id)") }
        }
        $absent = @($plan.checks | Where-Object { !$seenIds.Contains($_.id) })
        if ($absent.Count) {
            $lines.Add("  - Missing planned checks: " + ($absent.id -join ', '))
        } else {
            $lines.Add("  - All $($plan.checks.Count) planned checks returned results.")
        }
        $required = @($plan.checks | Where-Object { !$_.exclusion }).Count
        $reused = @($results | Where-Object status -CEQ 'Reused').Count
        $excluded = @($plan.checks | Where-Object exclusion).Count
        $lines.Add("  - Required checks: $required. Reused results: $reused. Inapplicable checks: $excluded.")
        foreach ($reason in @($plan.diagnostics | Where-Object { $_ -match 'conservative|require full current coverage|selecting all current owners' })) {
            $lines.Add("  - $reason")
        }
        foreach ($reason in @($plan.diagnostics | Where-Object { $_ -clike 'Selected * entry *' } | Select-Object -First 3)) {
            $lines.Add("  - $reason")
        }
        foreach ($category in 'Engine tests','Tooling tests') {
            $projects = @($plan.checks | Where-Object {
                $_.kind -ceq 'test' -and !$_.exclusion -and ($_.owner -match '^tests/(tools|build)/') -eq ($category -ceq 'Tooling tests')
            })
            if (!$projects.Count) { continue }
            $lines.Add("#### $category")
            $lines.Add('')
            $lines.Add('| Project | Result | Executed cases | Sum of case durations |')
            $lines.Add('| --- | --- | ---: | ---: |')
            foreach ($project in $projects) {
                $result = $results | Where-Object id -CEQ $project.id | Select-Object -First 1
                $status = if ($result) { $result.status } else { 'Missing' }
                $cases = if ($status -ceq 'Reused') { 0 } elseif ($null -ne $result.testCases) { $result.testCases } else { 'unavailable' }
                $seconds = if ($status -ceq 'Reused') { '0 s' } elseif ($null -ne $result.caseSeconds) { "$([math]::Round($result.caseSeconds, 2)) s" } else { 'unavailable' }
                $lines.Add("| $($project.owner) | $status | $cases | $seconds |")
            }
            $lines.Add('')
        }
        foreach ($group in @($plan.checks | Group-Object kind)) {
            $seconds = ($results | Where-Object { $_.id -cin $group.Group.id } | Measure-Object seconds -Sum).Sum
            $lines.Add("  - $($group.Name): $([Math]::Round($seconds, 1)) check-seconds (parallel work is not wall-clock duration).")
        }
        foreach ($result in @($failed | Select-Object -First 10)) {
            $detail = ([string]$result.detail).Replace("`n", ' ').Replace("`r", ' ')
            if ($detail.Length -gt 4000) {
                $detail = $detail.Substring(0, 500) + ' [Complete first-failure detail is retained in results.json and the check log.] ' + $detail.Substring($detail.Length - 3000)
            }
            $lines.Add("  - $($result.id): $detail")
        }
        foreach ($fault in $faults) {
            $lines.Add("  - $($fault.stage) $($fault.owner): $($fault.detail) ($([Math]::Round($fault.seconds, 2)) s). Log: $($fault.log)")
        }
    } catch {
        $lines.Add("- ${platform}: complete results unavailable; inspect the retained bootstrap/planning log and required job conclusion.")
    }
}

# Measure observed execution up to the merge gate, including unrelated jobs that overlap.
# The union of busy intervals and cumulative runner time have different meanings.
if ($env:GH_TOKEN) {
    try {
        $headers = @{Authorization="Bearer $env:GH_TOKEN";Accept='application/vnd.github+json'}
        $runBase = "$ApiUrl/repos/$Repository/actions/runs/$RunId/attempts/$Attempt"
        $attemptInfo = Invoke-RestMethod -Headers $headers -Uri $runBase
        $jobs = @(for ($page = 1; ; $page++) {
            $batch = Invoke-RestMethod -Headers $headers -Uri "$runBase/jobs?per_page=100&page=$page"
            $batch.jobs
            if ($page * 100 -ge $batch.total_count) { break }
        })
        $finished = @($jobs | Where-Object { $_.started_at -and $_.completed_at -and $_.conclusion -cne 'skipped' } | Sort-Object started_at)
        if ($finished.Count) {
            $start = [datetimeoffset]$finished[0].started_at
            $end = $start
            $busy = 0.0
            $runner = 0.0
            foreach ($job in $finished) {
                $left = [datetimeoffset]$job.started_at
                $right = [datetimeoffset]$job.completed_at
                $runner += ($right - $left).TotalSeconds
                if ($right -gt $end) {
                    $busy += ($right - $(if ($left -gt $end) { $left } else { $end })).TotalSeconds
                    $end = $right
                }
            }
            $clock = ($end - [datetimeoffset]$attemptInfo.run_started_at).TotalSeconds
            $lines.Add("- Through the last completed job: $([math]::Round($clock)) seconds elapsed, $([math]::Round($busy)) seconds with active runners, $([math]::Round($runner / 60, 1)) cumulative runner-minutes.")
            $lines.Add("- Time before the first runner started: $([math]::Round(($start - [datetimeoffset]$attemptInfo.run_started_at).TotalSeconds)) seconds.")
        }
    } catch { $lines.Add('- Hosted job timing unavailable: ' + $_.Exception.Message) }
}

$lines | Add-Content -LiteralPath $SummaryPath -Encoding utf8
