param([string]$DocumentationRevision, [string]$MergeCommit)

$ErrorActionPreference = 'Stop'
if (-not $env:GH_TOKEN) { throw 'Pairing status requires the engine workflow credential.' }
if ($DocumentationRevision -and $DocumentationRevision -cnotmatch '^[a-f0-9]{40}$') { throw 'An immutable Docs callback is required.' }
if ($MergeCommit -and $MergeCommit -cnotmatch '^[a-f0-9]{40}$') { throw 'An immutable merge-group candidate is required.' }
$prefix = 'https://api.github.com/repos/Wolfiora/InnoEngine'
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
function Read-Pulls {
    for ($page = 1; ; $page++) {
        $rows = @(Invoke-RestMethod -Uri "$prefix/pulls?state=open&per_page=100&page=$page" -Headers $headers)
        foreach ($row in $rows) { $row }
        if ($rows.Count -lt 100) { break }
    }
}
function Write-Check([string]$Method, [string]$Path, $Body) {
    Invoke-RestMethod -Method $Method -Uri "$prefix/$Path" -Headers $headers -ContentType 'application/json' -Body ($Body | ConvertTo-Json -Depth 8)
}
function Assert-Current($Before, $After) {
    if ($After.state -cne 'open' -or $After.head.sha -cne $Before.head.sha -or $After.base.sha -cne $Before.base.sha -or
        $After.merge_commit_sha -cne $Before.merge_commit_sha -or $After.body -cne $Before.body) {
        throw 'The PR head, base, merge candidate or pairing metadata changed during reconciliation.'
    }
}
$pulls = @(Read-Pulls | Where-Object { $_.state -ceq 'open' -and $_.base.repo.full_name -ceq 'Wolfiora/InnoEngine' })
$targets = if ($MergeCommit) { @([pscustomobject]@{ commit = $MergeCommit; pulls = $pulls }) } else {
    # Refresh all open candidates: GitHub can coalesce pending notifications, and merging Docs
    # can make an earlier exact candidate eligible without changing its original revision.
    @($pulls | ForEach-Object {
            # List endpoints do not guarantee that a test merge has been calculated. Fetch the actual PR.
            $current = Invoke-RestMethod -Uri "$prefix/pulls/$($_.number)" -Headers $headers
            $commit = if ($current.merge_commit_sha -cmatch '^[a-f0-9]{40}$') { $current.merge_commit_sha } else { $current.head.sha }
            [pscustomobject]@{ commit = $commit; pulls = @($current) }
        })
}
foreach ($target in $targets) {
    if ($target.commit -cnotmatch '^[a-f0-9]{40}$') { throw 'GitHub did not identify a checkable engine candidate.' }
    $check = Write-Check Post 'check-runs' @{ name = 'Documentation pairing'; head_sha = $target.commit; status = 'in_progress'
        output = @{ title = 'Checking the exact documentation candidate'; summary = 'No build is performed. Missing evidence and unmerged documentation remain blocked.' } }
    $failure = $null
    $result = $null
    try {
        if (-not $MergeCommit -and $target.pulls[0].merge_commit_sha -cnotmatch '^[a-f0-9]{40}$') { throw 'The PR has no test merge candidate yet.' }
        $commit = Invoke-RestMethod -Uri "$prefix/git/commits/$($target.commit)" -Headers $headers
        $candidate = & "$PSScriptRoot/Select-DocumentationCandidate.ps1" -EngineTree $commit.tree.sha -PullRequests $target.pulls
        $result = & "$PSScriptRoot/Test-DocumentationPairing.ps1" -EngineTree $commit.tree.sha -DocumentationRevision $candidate.revision
        foreach ($before in $target.pulls) {
            $after = Invoke-RestMethod -Uri "$prefix/pulls/$($before.number)" -Headers $headers
            Assert-Current $before $after
        }
    } catch { $failure = $_.Exception.Message }
    $summary = if ($failure) { $failure } else { $result | ConvertTo-Json -Depth 4 }
    Write-Check Patch "check-runs/$($check.id)" @{ status = 'completed'; conclusion = $(if ($failure) { 'failure' } else { 'success' })
        output = @{ title = $(if ($failure) { 'Documentation pairing is blocked' } else { 'Verified prepared documentation candidate' }); summary = $summary } } | Out-Null
    [pscustomobject]@{ commit = $target.commit; status = $(if ($failure) { 'Blocked' } else { 'Passed' }); reason = $summary }
}
