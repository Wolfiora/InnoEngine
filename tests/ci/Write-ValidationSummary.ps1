param(
    [Parameter(Mandatory)][string]$ReceivedDirectory,
    [Parameter(Mandatory)][string]$Repository,
    [Parameter(Mandatory)][string]$DefaultBranch,
    [Parameter(Mandatory)][int]$Attempt,
    [Parameter(Mandatory)][long]$RunId,
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
foreach ($platform in @('windows-2022', 'macos-15')) {
    try {
        $artifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-$platform" -MaximumAttempt $Attempt
        $results = Get-Content -LiteralPath (Join-Path $artifact.directory 'results.json') -Raw | ConvertFrom-Json
        $failed = @($results.results | Where-Object { $_.status -cin @('Failed', 'Blocked') })
        $lines.Add("- ${platform}: $($failed.Count) failed or blocked checks; retained evidence: $($artifact.name).")
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

# Timing comparisons are informational; they never transfer validation status across runs.
$certification = [IO.Path]::GetFullPath('../certification.json', $ReceivedDirectory)
if ($env:GH_TOKEN -and (Test-Path -LiteralPath $certification)) {
    try {
        $current = @(Get-Content -LiteralPath $certification -Raw | ConvertFrom-Json)
        $headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
        $branch = [Uri]::EscapeDataString($DefaultBranch)
        $runs = Invoke-RestMethod -Uri "$ApiUrl/repos/$Repository/actions/workflows/rendering-ci.yml/runs?branch=$branch&status=success&per_page=20" -Headers $headers
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
