param(
    [Parameter(Mandatory)][string]$ReceivedDirectory,
    [Parameter(Mandatory)][string]$Repository,
    [Parameter(Mandatory)][string]$DefaultBranch,
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
        $results = @(foreach ($shard in @($plan.shards)) {
            $receipt = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-result-$platform-$($shard.id)" -MaximumAttempt $Attempt
            (Get-Content -LiteralPath (Join-Path $receipt.directory 'results.json') -Raw | ConvertFrom-Json).results
        })
        $failed = @($results | Where-Object { $_.status -cin @('Failed', 'Blocked') })
        $lines.Add("- ${platform}: $($failed.Count) failed or blocked checks; frozen plan: $($artifact.name).")
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

# Timing comparisons are informational; they never transfer validation status across runs.
$certification = [IO.Path]::GetFullPath('../certification.json', $ReceivedDirectory)
if ($env:GH_TOKEN -and (Test-Path -LiteralPath $certification)) {
    try {
        $current = @(Get-Content -LiteralPath $certification -Raw | ConvertFrom-Json)
        $headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
        $branch = [Uri]::EscapeDataString($DefaultBranch)
        $runs = Invoke-RestMethod -Uri "$ApiUrl/repos/$Repository/actions/workflows/engine-ci.yml/runs?branch=$branch&status=success&per_page=20" -Headers $headers
        foreach ($run in $runs.workflow_runs) {
            if ($run.id -eq $RunId -or $run.head_repository.full_name -cne $Repository -or $run.head_branch -cne $DefaultBranch -or
                $run.event -cnotin @('push', 'schedule', 'workflow_dispatch')) { continue }
            $inventory = Invoke-RestMethod -Uri "$ApiUrl/repos/$Repository/actions/runs/$($run.id)/artifacts?per_page=100" -Headers $headers
            $matches = @($inventory.artifacts | Where-Object { $_.name -ceq "engine-certification-attempt-$($run.run_attempt)" -and -not $_.expired })
            if ($matches.Count -ne 1) { continue }
            $comparison = Join-Path (Split-Path ([IO.Path]::GetFullPath($ReceivedDirectory)) -Parent) ('comparison-' + [Guid]::NewGuid().ToString('N'))
            New-Item -ItemType Directory -Path $comparison | Out-Null
            $archive = Join-Path $comparison 'certification.zip'
            Invoke-WebRequest -Uri "$ApiUrl/repos/$Repository/actions/artifacts/$($matches[0].id)/zip" -Headers $headers -OutFile $archive
            & "$PSScriptRoot/Expand-ValidationArtifact.ps1" -ArchivePath $archive -Destination (Join-Path $comparison 'verified') -ExpectedDigest $matches[0].digest
            $previous = @(Get-Content -LiteralPath (Join-Path $comparison 'verified/certification.json') -Raw | ConvertFrom-Json)
            $changes = 0
            foreach ($platform in $current) {
                $before = @($previous | Where-Object { $_.platform -ceq $platform.platform -and $_.receipt.environment -ceq $platform.receipt.environment })
                if ($before.Count -ne 1) { continue }
                foreach ($timing in @($platform.receipt.timings)) {
                    $baseline = @($before[0].receipt.timings | Where-Object { $_.id -ceq $timing.id -and $_.kind -ceq $timing.kind -and $_.owner -ceq $timing.owner })
                    if ($baseline.Count -ne 1 -or $baseline[0].seconds -le 0 -or $timing.seconds -lt $baseline[0].seconds * 1.5 -or
                        $timing.seconds - $baseline[0].seconds -lt 15) { continue }
                    $message = "$($platform.platform) / $($timing.id): $([Math]::Round($baseline[0].seconds, 1))s to $([Math]::Round($timing.seconds, 1))s. Inspect workload changes and compare repeated samples."
                    $lines.Add("- Duration increase against [run $($run.id)]($($run.html_url)): $message")
                    Write-Host ('::warning::' + $message.Replace('%', '%25').Replace("`r", '%0D').Replace("`n", '%0A'))
                    $changes++
                }
            }
            if ($changes -eq 0) { $lines.Add('- No actionable duration increase in comparable retained checks; no performance notification emitted.') }
            break
        }
    } catch { $lines.Add('- Timing comparison unavailable; required validation status is unchanged. ' + $_.Exception.Message) }
}
$lines | Add-Content -LiteralPath $SummaryPath -Encoding utf8
