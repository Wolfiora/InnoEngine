param(
    [Parameter(Mandatory)][string]$Revision,
    [Parameter(Mandatory)][array]$Targets,
    [Parameter(Mandatory)][string]$DefaultBranch,
    [string]$Repository = $env:GITHUB_REPOSITORY,
    [string]$ApiUrl = 'https://api.github.com'
)
$ErrorActionPreference = 'Stop'
$headers = @{Authorization="Bearer $env:GH_TOKEN"; Accept='application/vnd.github+json'}
$base = "$ApiUrl/repos/$Repository"
$tree = (& git rev-parse --verify "${Revision}^{tree}").Trim()
if ($LASTEXITCODE -or $tree -cnotmatch '^[0-9a-f]{40}$') { throw 'Missing landed source tree.' }
$pulls = @(Invoke-RestMethod -Headers $headers -Uri "$base/commits/$Revision/pulls?per_page=100" | Write-Output)
$heads = @($pulls | Where-Object {
    $_.merged_at -and $_.merge_commit_sha -ceq $Revision -and $_.base.ref -ceq $DefaultBranch -and
    $_.base.repo.full_name -ceq $Repository -and $_.head.repo.full_name -ceq $Repository
} | ForEach-Object { $_.head.sha } | Sort-Object -Unique)
$runs = @(foreach ($head in $heads) {
    if ($head -cnotmatch '^[0-9a-f]{40}$') { continue }
    (Invoke-RestMethod -Headers $headers -Uri "$base/actions/workflows/engine-ci.yml/runs?event=pull_request&head_sha=$head&per_page=100").workflow_runs
})
# A merge queue can validate the final tree across several PRs.
$queue = Invoke-RestMethod -Headers $headers -Uri "$base/actions/workflows/engine-ci.yml/runs?event=merge_group&per_page=100"
$runs += @($queue.workflow_runs | Where-Object { $_.head_branch -like "gh-readonly-queue/$DefaultBranch/*" })
$found = @{}
foreach ($run in $runs | Sort-Object id -Descending) {
    if ($run.path -cne '.github/workflows/engine-ci.yml' -or $run.repository.full_name -cne $Repository -or
        $run.head_repository.full_name -cne $Repository -or $run.status -cne 'completed' -or
        $run.display_title -cnotmatch '^Engine validation ([0-9a-f]{40})$') { continue }
    $source = $Matches[1]
    if ($run.event -ceq 'merge_group') {
        if ($source -cne $run.head_sha) { continue }
    } elseif ($run.event -cne 'pull_request' -or $run.head_sha -cnotin $heads) { continue }
    $commit = Invoke-RestMethod -Headers $headers -Uri "$base/git/commits/$source" -SkipHttpErrorCheck -StatusCodeVariable commitStatus
    if ($commitStatus -eq 404) { continue }
    if ($commitStatus -ne 200) { throw "Unable to verify candidate source (HTTP $commitStatus)." }
    if ($commit.tree.sha -cne $tree -or ($run.event -ceq 'pull_request' -and $run.head_sha -cnotin $commit.parents.sha)) { continue }
    $jobs = (Invoke-RestMethod -Headers $headers -Uri "$base/actions/runs/$($run.id)/attempts/$($run.run_attempt)/jobs?per_page=100").jobs
    $artifacts = (Invoke-RestMethod -Headers $headers -Uri "$base/actions/runs/$($run.id)/artifacts?per_page=100").artifacts
    foreach ($target in $Targets) {
        if ($found.ContainsKey($target.target)) { continue }
        $build = @($jobs | Where-Object { $_.name.EndsWith(" / Backend build ($($target.target))") -and $_.conclusion -ceq 'success' })
        $verify = @($jobs | Where-Object { $_.name.EndsWith(" / Engine tests ($($target.runner))") -and $_.conclusion -ceq 'success' } |
            ForEach-Object steps | Where-Object { $_.name -ceq 'Verify new backend publication inputs' -and $_.conclusion -ceq 'success' })
        $asset = @($artifacts | Where-Object { $_.name -ceq "backend-candidate-$($target.target)" -and !$_.expired })
        if ($build.Count -ne 1 -or $verify.Count -ne 1 -or $asset.Count -ne 1) { continue }
        if ($asset[0].digest -cnotmatch '^sha256:[0-9a-f]{64}$' -or $asset[0].created_at -lt $build[0].started_at -or
            $asset[0].created_at -gt $build[0].completed_at -or $asset[0].created_at -gt $verify[0].started_at) { continue }
        $found[$target.target] = @{commit=$source; tree=$tree; run=$run.id; attempt=$run.run_attempt;
            artifact=$asset[0].id; digest=$asset[0].digest}
    }
    if ($found.Count -eq $Targets.Count) { break }
}
$found
