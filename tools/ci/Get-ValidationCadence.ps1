param(
    [Parameter(Mandatory)][string]$EventPath,
    [Parameter(Mandatory)][string]$Event,
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][string]$Repository,
    [string]$ApiUrl = 'https://api.github.com'
)

$ErrorActionPreference = 'Stop'
$eventData = Get-Content -LiteralPath $EventPath -Raw | ConvertFrom-Json
$default = $Event -ceq 'push' -and $eventData.ref -ceq "refs/heads/$($eventData.repository.default_branch)"
$open = $false
if ($Event -ceq 'push' -and -not $default) {
    if (-not $env:GH_TOKEN) { throw 'Push routing requires the pull-request read token.' }
    $headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
    $pulls = @(Invoke-RestMethod -Uri "$ApiUrl/repos/$Repository/commits/$Commit/pulls?per_page=100" -Headers $headers)
    $open = @($pulls | Where-Object { $_.state -ceq 'open' -and $_.head.sha -ceq $Commit `
        -and $_.head.repo.full_name -ceq $Repository -and "refs/heads/$($_.head.ref)" -ceq $eventData.ref }).Count -gt 0
}
$manualFull = [string]$eventData.inputs.full -ceq 'true'
$selected = & "$PSScriptRoot/Select-ValidationCadence.ps1" -Event $Event -Commit $Commit `
    -IsDefaultBranch $default -HasOpenPullRequest $open -ManualFull $manualFull
# Keep the required PR/queue check visible while bypassing validation of local measurements.
$base = switch ($Event) {
    'pull_request' { $eventData.pull_request.base.sha }
    'merge_group' { $eventData.merge_group.base_sha }
}
if ($base -cmatch '^[0-9a-f]{40}$') {
    $outside = git diff --shortstat --no-renames $base $Commit -- . ':(top,exclude)benchmark/**'
    if ($LASTEXITCODE) { throw 'Could not compare the merge candidate with its base.' }
    if (!$outside) {
        $benchmark = git diff --shortstat --no-renames $base $Commit -- benchmark/
        if ($LASTEXITCODE) { throw 'Could not compare benchmark inputs.' }
        if ($benchmark) {
            $selected.run = $false
            $selected.reason = 'Only local benchmark inputs changed.'
        }
    }
}
foreach ($name in @('run', 'full', 'extended')) {
    "$name=$($selected.$name.ToString().ToLowerInvariant())" >> $env:GITHUB_OUTPUT
}
"profile=$($selected.profile)" >> $env:GITHUB_OUTPUT
$selected | ConvertTo-Json -Depth 4
