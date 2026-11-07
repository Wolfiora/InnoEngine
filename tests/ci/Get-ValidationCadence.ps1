param(
    [Parameter(Mandatory)][string]$EventPath,
    [Parameter(Mandatory)][string]$Event,
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][string]$Repository,
    [string]$ApiUrl = 'https://api.github.com'
)

$ErrorActionPreference = 'Stop'
$eventData = Get-Content -LiteralPath $EventPath -Raw | ConvertFrom-Json
$lastCertified = ''
if ($Event -eq 'schedule') {
    if (-not $env:GH_TOKEN) { throw 'Scheduled certification lookup requires the workflow read token.' }
    $headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
    $branch = [Uri]::EscapeDataString($eventData.repository.default_branch)
    # Only a successful same-repository default-branch run with a full-certification artifact can suppress a daily run.
    # A bounded lookup that finds no certification conservatively performs full validation.
    $runs = Invoke-RestMethod -Uri "$ApiUrl/repos/$Repository/actions/workflows/rendering-ci.yml/runs?branch=$branch&status=success&per_page=100" -Headers $headers
    foreach ($run in $runs.workflow_runs) {
        if ($run.head_sha -cne $Commit -or $run.head_repository.id -ne $eventData.repository.id -or
            $run.head_branch -cne $eventData.repository.default_branch -or $run.event -cnotin @('push', 'schedule', 'workflow_dispatch')) { continue }
        $artifacts = Invoke-RestMethod -Uri "$ApiUrl/repos/$Repository/actions/runs/$($run.id)/artifacts?per_page=100" -Headers $headers
        $certificates = @($artifacts.artifacts | Where-Object { $_.name -ceq "engine-full-certification-attempt-$($run.run_attempt)" -and -not $_.expired })
        if ($certificates.Count -eq 1) { $lastCertified = $Commit; break }
    }
}
$manualFull = if ($null -eq $eventData.inputs.full) { $true } else { [string]$eventData.inputs.full -ceq 'true' }
$selected = & "$PSScriptRoot/Select-ValidationCadence.ps1" -Event $Event -Commit $Commit -Schedule ([string]$eventData.schedule) `
    -LastCertifiedCommit $lastCertified -ManualFull $manualFull
if ($Event -eq 'workflow_dispatch' -and [string]$eventData.inputs.extended -ceq 'true') {
    $selected.full = $true
    $selected.extended = $true
}
foreach ($name in @('run', 'full', 'extended')) {
    "$name=$($selected.$name.ToString().ToLowerInvariant())" >> $env:GITHUB_OUTPUT
}
$selected | ConvertTo-Json -Depth 4
