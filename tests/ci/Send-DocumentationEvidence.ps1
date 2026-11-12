param(
    [Parameter(Mandatory)][long]$RunId,
    [switch]$Bootstrap
)

$ErrorActionPreference = 'Stop'
if ($RunId -le 0 -or -not $env:GH_TOKEN -or -not $env:INNO_DOCS_DISPATCH_TOKEN) {
    throw 'Evidence relay requires an exact run and independently scoped read/dispatch credentials.'
}
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
$prefix = 'https://api.github.com/repos/Wolfiora/InnoEngine'
function Read-Collection([string]$Path, [string]$Field) {
    $values = [Collections.Generic.List[object]]::new()
    for ($page = 1; ; $page++) {
        $result = Invoke-RestMethod -Uri "$prefix$($Path)?per_page=100&page=$page" -Headers $headers
        if ($null -eq $result.$Field) { throw 'Malformed evidence collection.' }
        $rows = @($result.$Field)
        foreach ($row in $rows) { $values.Add($row) }
        if ($rows.Count -lt 100) { return $values.ToArray() }
    }
}
$run = Invoke-RestMethod -Uri "$prefix/actions/runs/$RunId" -Headers $headers
$repository = Invoke-RestMethod -Uri $prefix -Headers $headers
if ($run.id -ne $RunId -or $run.repository.full_name -cne 'Wolfiora/InnoEngine' -or $run.head_repository.full_name -cne 'Wolfiora/InnoEngine' `
    -or $run.path -cne '.github/workflows/rendering-ci.yml' -or $run.status -cne 'completed' -or $run.conclusion -cne 'success' `
    -or $run.head_sha -cnotmatch '^[a-f0-9]{40}$' -or $run.run_attempt -le 0) { throw 'Only the successful exact Engine CI producer can relay source evidence.' }
$expectedBranch = if ($Bootstrap) { 'ai-pipeline' } else { $repository.default_branch }
$internalCandidate = -not $Bootstrap -and $run.event -ceq 'pull_request' -and @($run.pull_requests).Count -gt 0 `
    -and @($run.pull_requests | Where-Object { $_.head.sha -cne $run.head_sha }).Count -eq 0
if ((-not $internalCandidate -and $run.head_branch -cne $expectedBranch) -or $run.event -cnotin @('push', 'workflow_dispatch', 'pull_request', 'merge_group', 'schedule')) {
    throw 'The engine evidence producer is outside the selected trusted branch.'
}
$jobs = @(Read-Collection "/actions/runs/$RunId/attempts/$($run.run_attempt)/jobs" 'jobs')
$aggregate = @($jobs | Where-Object name -CEQ 'Engine validation')
if ($aggregate.Count -ne 1 -or $aggregate[0].conclusion -cne 'success' -or $aggregate[0].status -cne 'completed') { throw 'The Engine validation aggregate did not pass exactly once.' }
$artifacts = @(Read-Collection "/actions/runs/$RunId/artifacts" 'artifacts')
$certificate = @($artifacts | Where-Object name -CEQ "documentation-source-certification-attempt-$($run.run_attempt)")
if ($certificate.Count -ne 1 -or $certificate[0].expired -or $certificate[0].size_in_bytes -le 0 `
    -or $certificate[0].digest -cnotmatch '^sha256:[a-f0-9]{64}$' -or $certificate[0].workflow_run.id -ne $RunId `
    -or $certificate[0].workflow_run.head_sha -cne $run.head_sha) { throw 'No retained complete source certificate exists for this successful attempt.' }
$destination = if ($Bootstrap) { 'docs-modernization' } else { 'main' }
$body = @{ ref = $destination; inputs = @{ engine_run = [string]$RunId; engine_commit = $run.head_sha; bootstrap = [bool]$Bootstrap } } | ConvertTo-Json -Depth 4
$dispatchHeaders = @{ Authorization = "Bearer $env:INNO_DOCS_DISPATCH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
Invoke-RestMethod -Uri 'https://api.github.com/repos/Wolfiora/InnoEngine.Docs/actions/workflows/source-evidence.yml/dispatches' `
    -Method Post -Headers $dispatchHeaders -ContentType 'application/json' -Body $body | Out-Null
Write-Output "Requested independent source-certificate import for engine $($run.head_sha), run $RunId, attempt $($run.run_attempt). The receiver must authenticate it again; dispatch is not certification."
