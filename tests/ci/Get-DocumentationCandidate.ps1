param(
    [Parameter(Mandatory)][string]$EventPath,
    [Parameter(Mandatory)][ValidateSet('pull_request', 'push', 'merge_group', 'schedule', 'workflow_dispatch')][string]$Event,
    [Parameter(Mandatory)][string]$ExpectedCommit,
    [string]$ExplicitRevision
)

$ErrorActionPreference = 'Stop'
if ($ExpectedCommit -cnotmatch '^[a-f0-9]{40}$') { throw 'Documentation selection requires an exact engine candidate.' }
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$actual = (& git -C $repository rev-parse HEAD).Trim()
if ($LASTEXITCODE -ne 0 -or $actual -cne $ExpectedCommit) { throw 'Engine checkout does not match the requested candidate.' }
$tree = (& git -C $repository rev-parse 'HEAD^{tree}').Trim()
if ($LASTEXITCODE -ne 0) { throw 'Cannot resolve the candidate source tree.' }
$eventData = Get-Content -LiteralPath $EventPath -Raw | ConvertFrom-Json
if ($eventData.repository.full_name -cne 'Wolfiora/InnoEngine') { throw 'Unexpected engine repository in documentation selection.' }
if ($ExplicitRevision -and $Event -cne 'workflow_dispatch') { throw 'Only manual dispatch accepts an explicit documentation revision.' }
$pulls = [Collections.Generic.List[object]]::new()
$headers = @{ Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
if ($env:GH_TOKEN) { $headers.Authorization = "Bearer $env:GH_TOKEN" }
if ($Event -ceq 'pull_request') {
    if ($eventData.pull_request.base.repo.full_name -cne 'Wolfiora/InnoEngine') { throw 'Pull request metadata belongs to another base repository.' }
    $number = [int]$eventData.pull_request.number
    if ($number -le 0) { throw 'Pull request metadata has no valid request number.' }
    # A metadata correction must work on a rerun without an empty source commit. Refuse a changed base/head instead of certifying an obsolete merge.
    $current = Invoke-RestMethod -Uri "https://api.github.com/repos/Wolfiora/InnoEngine/pulls/$number" -Headers $headers
    if ($current.base.repo.full_name -cne 'Wolfiora/InnoEngine' -or $current.head.sha -cne $eventData.pull_request.head.sha `
        -or $current.base.sha -cne $eventData.pull_request.base.sha -or $current.merge_commit_sha -cne $ExpectedCommit) {
        throw 'The pull request base, head or merge candidate changed; validate the current candidate before selecting documentation.'
    }
    $pulls.Add($current)
} elseif ($Event -ceq 'merge_group') {
    if ($eventData.merge_group.head_sha -cne $ExpectedCommit -or $eventData.merge_group.base_sha -cnotmatch '^[a-f0-9]{40}$') {
        throw 'Merge-group metadata does not match the checked candidate.'
    }
    $branch = [string]$eventData.merge_group.base_ref
    if (-not $branch.StartsWith('refs/heads/', [StringComparison]::Ordinal)) { throw 'Merge-group base is not a branch.' }
    $branch = $branch.Substring('refs/heads/'.Length)
    $cursor = $null
    do {
        # Queue entry head commits describe prepared queue inputs, including rebased groups; do not guess PR numbers from ref names.
        $query = 'query($branch:String!,$after:String){repository(owner:"Wolfiora",name:"InnoEngine"){mergeQueue(branch:$branch){entries(first:100,after:$after){nodes{headCommit{oid} pullRequest{number body}} pageInfo{hasNextPage endCursor}}}}}'
        $payload = @{ query = $query; variables = @{ branch = $branch; after = $cursor } } | ConvertTo-Json -Depth 5
        $response = Invoke-RestMethod -Method Post -Uri 'https://api.github.com/graphql' -Headers $headers -ContentType 'application/json' -Body $payload
        if ($response.errors -or $null -eq $response.data.repository.mergeQueue) { throw 'The active merge queue could not be read; no candidate can be inferred.' }
        $entries = $response.data.repository.mergeQueue.entries
        foreach ($entry in $entries.nodes) {
            $head = [string]$entry.headCommit.oid
            if ($head -cnotmatch '^[a-f0-9]{40}$' -or $null -eq $entry.pullRequest) { continue }
            & git -C $repository cat-file -e "$head^{commit}" 2>$null
            if ($LASTEXITCODE -ne 0) { continue }
            & git -C $repository merge-base --is-ancestor $head $ExpectedCommit
            if ($LASTEXITCODE -eq 1) { continue }
            if ($LASTEXITCODE -ne 0) { throw 'Cannot establish queue input ancestry.' }
            & git -C $repository merge-base --is-ancestor $head $eventData.merge_group.base_sha
            if ($LASTEXITCODE -eq 0) { continue }
            if ($LASTEXITCODE -ne 1) { throw 'Cannot establish queue base ancestry.' }
            $pulls.Add($entry.pullRequest)
        }
        if ($entries.pageInfo.hasNextPage -and (-not $entries.pageInfo.endCursor -or $entries.pageInfo.endCursor -ceq $cursor)) {
            throw 'Merge-queue pagination did not advance.'
        }
        $cursor = $entries.pageInfo.endCursor
    } while ($entries.pageInfo.hasNextPage)
} elseif (-not $ExplicitRevision) {
    $page = 1
    do {
        $batch = Invoke-RestMethod -Uri "https://api.github.com/repos/Wolfiora/InnoEngine/commits/$ExpectedCommit/pulls?per_page=100&page=$page" -Headers $headers
        foreach ($pull in $batch) {
            if ($pull.base.repo.full_name -ceq 'Wolfiora/InnoEngine') { $pulls.Add($pull) }
        }
        $page++
    } while ($batch.Count -eq 100)
}
$candidate = & "$PSScriptRoot/Select-DocumentationCandidate.ps1" -EngineTree $tree -PullRequests $pulls.ToArray() -ExplicitRevision $ExplicitRevision
$global:LASTEXITCODE = 0
$candidate
