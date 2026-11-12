param(
    [Parameter(Mandatory)][string]$EngineTree,
    [Parameter(Mandatory)][string]$DocumentationRevision
)

$ErrorActionPreference = 'Stop'
if ($EngineTree -cnotmatch '^[a-f0-9]{40}$' -or $DocumentationRevision -cnotmatch '^[a-f0-9]{40}$' -or -not $env:INNO_DOCS_READ_TOKEN) {
    throw 'Pairing requires exact engine/documentation identities and the scoped Docs read credential.'
}
$prefix = 'https://api.github.com/repos/Wolfiora/InnoEngine.Docs'
$headers = @{ Authorization = "Bearer $env:INNO_DOCS_READ_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
function Read-Collection([string]$Path, [string]$Field) {
    $rows = [Collections.Generic.List[object]]::new()
    for ($page = 1; ; $page++) {
        $separator = if ($Path.Contains('?')) { '&' } else { '?' }
        $result = Invoke-RestMethod -Uri "$prefix$Path$($separator)per_page=100&page=$page" -Headers $headers
        if ($null -eq $result.$Field) { throw 'Malformed documentation evidence collection.' }
        foreach ($row in @($result.$Field)) { $rows.Add($row) }
        if (@($result.$Field).Count -lt 100) { return $rows.ToArray() }
    }
}
$repository = Invoke-RestMethod -Uri $prefix -Headers $headers
$branch = [Uri]::EscapeDataString([string]$repository.default_branch)
$ancestry = Invoke-RestMethod -Uri "$prefix/compare/$DocumentationRevision...$branch" -Headers $headers
if ($ancestry.status -cnotin @('identical', 'ahead') -or $ancestry.merge_base_commit.sha -cne $DocumentationRevision) {
    throw 'The exact verified Docs candidate must merge into its protected default branch before the engine PR can merge.'
}
$file = Invoke-RestMethod -Uri "$prefix/contents/reference/engine.lock.json?ref=$DocumentationRevision" -Headers $headers
if ($file.type -cne 'file' -or $file.encoding -cne 'base64' -or $file.size -le 0 -or $file.size -gt 262144) { throw 'The paired engine binding is not a bounded ordinary file.' }
$bytes = [Convert]::FromBase64String($file.content)
if ($bytes.Length -ne $file.size) { throw 'The paired engine binding has inconsistent length.' }
$binding = [Text.Encoding]::UTF8.GetString($bytes) | ConvertFrom-Json
if ($binding.repository -cne 'https://github.com/Wolfiora/InnoEngine' -or $binding.sourceTree -cne $EngineTree -or $binding.revision -cnotmatch '^[a-f0-9]{40}$') {
    throw 'The Docs candidate binds a different engine source tree.'
}
$runs = @(Read-Collection "/actions/workflows/documentation.yml/runs?head_sha=$DocumentationRevision" 'workflow_runs' |
    Where-Object { $_.path -ceq '.github/workflows/documentation.yml' -and $_.head_sha -ceq $DocumentationRevision } | Sort-Object id -Descending)
if ($runs.Count -eq 0) { throw 'No documentation validation exists for the exact paired candidate.' }
$run = $runs[0]
if ($run.repository.full_name -cne 'Wolfiora/InnoEngine.Docs' -or $run.head_repository.full_name -cne 'Wolfiora/InnoEngine.Docs' -or
    $run.status -cne 'completed' -or $run.conclusion -cne 'success' -or $run.id -le 0 -or $run.run_attempt -le 0 -or
    $run.event -cnotin @('pull_request', 'push', 'workflow_dispatch', 'merge_group')) {
    throw 'The latest matching documentation producer is not a successful trusted candidate.'
}
$jobs = @(Read-Collection "/actions/runs/$($run.id)/attempts/$($run.run_attempt)/jobs" 'jobs')
$aggregate = @($jobs | Where-Object name -CEQ 'Documentation validation')
if ($aggregate.Count -ne 1 -or $aggregate[0].run_id -ne $run.id -or $aggregate[0].status -cne 'completed' -or $aggregate[0].conclusion -cne 'success') {
    throw 'The exact Docs attempt has no unique successful Documentation validation aggregate.'
}
$artifacts = @(Read-Collection "/actions/runs/$($run.id)/artifacts" 'artifacts')
$site = @($artifacts | Where-Object name -CEQ "documentation-site-attempt-$($run.run_attempt)")
if ($site.Count -ne 1 -or $site[0].expired -or $site[0].size_in_bytes -le 0 -or $site[0].digest -cnotmatch '^sha256:[a-f0-9]{64}$' -or
    $site[0].workflow_run.id -ne $run.id -or $site[0].workflow_run.head_sha -cne $DocumentationRevision) {
    throw 'The successful Docs attempt has no retained verified site artifact.'
}
[pscustomobject]@{ status = 'Passed'; engineTree = $EngineTree; documentationRevision = $DocumentationRevision; run = $run.id
    attempt = $run.run_attempt; artifact = $site[0].id; digest = $site[0].digest }
