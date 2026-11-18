param(
    [Parameter(Mandatory)][long]$SourceRunId,
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][string]$Repository,
    [Parameter(Mandatory)][string]$DefaultBranch,
    [string]$ApiUrl = 'https://api.github.com',
    [int]$TimeoutMinutes = 240
)
$ErrorActionPreference = 'Stop'
if ($SourceRunId -le 0 -or $Commit -cnotmatch '^[a-f0-9]{40}$' -or -not $env:GH_TOKEN -or $TimeoutMinutes -le 0) {
    throw 'Release completion requires an authenticated exact candidate and finite timeout.'
}
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
$base = "$ApiUrl/repos/$Repository"
$current = Invoke-RestMethod -Uri "$base/commits/$([Uri]::EscapeDataString($DefaultBranch))" -Headers $headers
if ($current.sha -cne $Commit) { throw 'Release completion must dispatch the exact current default-branch candidate; select its matching source run.' }
$request = [Guid]::NewGuid().ToString('N')
$body = @{ ref = $DefaultBranch; inputs = @{ full = 'true'; source_run = "$SourceRunId"; request_id = $request } }
$null = Invoke-RestMethod -Method Post -Uri "$base/actions/workflows/engine-ci.yml/dispatches" -Headers $headers `
    -ContentType application/json -Body ($body | ConvertTo-Json -Depth 5)
$deadline = [DateTimeOffset]::UtcNow.AddMinutes($TimeoutMinutes)
while ([DateTimeOffset]::UtcNow -lt $deadline) {
    $runs = Invoke-RestMethod -Uri "$base/actions/workflows/engine-ci.yml/runs?event=workflow_dispatch&per_page=100" -Headers $headers
    $matches = @($runs.workflow_runs | Where-Object display_title -CEQ "Engine validation $request")
    if ($matches.Count -gt 1) { throw 'Release completion correlation is ambiguous.' }
    if ($matches.Count -eq 1) {
        $run = $matches[0]
        if ($run.head_sha -cne $Commit -or $run.head_branch -cne $DefaultBranch -or $run.path -cne '.github/workflows/engine-ci.yml') {
            throw 'The default branch moved before the completion dispatch; this run cannot certify the selected release.'
        }
        if ($run.status -ceq 'completed') {
            if ($run.conclusion -cne 'success') { throw "Release completion failed; retain the source evidence and inspect $($run.html_url)." }
            return [long]$run.id
        }
    }
    Start-Sleep -Seconds 30
}
throw "Release completion did not finish within $TimeoutMinutes minutes. Request: $request. No draft was created."
