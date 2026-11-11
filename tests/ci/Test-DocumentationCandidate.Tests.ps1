$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/documentation-candidate-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$scripts = Join-Path $root 'tests/ci'
New-Item -ItemType Directory -Path $scripts -Force | Out-Null
foreach ($file in @('Get-DocumentationCandidate.ps1', 'Select-DocumentationCandidate.ps1')) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot $file) -Destination $scripts
}
function Invoke-FixtureGit([string[]]$Arguments) {
    $result = @(& git -C $root @Arguments)
    if ($LASTEXITCODE -ne 0) { throw "Fixture Git operation failed: $($Arguments[0])" }
    return ($result -join "`n").Trim()
}
$null = Invoke-FixtureGit @('init', '-q')
'base' | Set-Content (Join-Path $root 'source.txt')
$null = Invoke-FixtureGit @('add', 'source.txt')
$null = Invoke-FixtureGit @('-c', 'user.name=Aaron Liao', '-c', 'user.email=hsuankailiao@gmail.com', 'commit', '-qm', 'Candidate base')
$base = Invoke-FixtureGit @('rev-parse', 'HEAD')
'candidate' | Set-Content (Join-Path $root 'source.txt')
$null = Invoke-FixtureGit @('add', 'source.txt')
$null = Invoke-FixtureGit @('-c', 'user.name=Aaron Liao', '-c', 'user.email=hsuankailiao@gmail.com', 'commit', '-qm', 'Candidate input')
$commit = Invoke-FixtureGit @('rev-parse', 'HEAD')
$tree = Invoke-FixtureGit @('rev-parse', 'HEAD^{tree}')
$revision = 'a' * 40
$metadata = "Documentation-Candidate: Wolfiora/InnoEngine.Docs@$revision engine-tree=$tree"
$pull = @{ number = 83; body = $metadata; head = @{ sha = $commit }; merge_commit_sha = $commit
    base = @{ sha = $base; repo = @{ full_name = 'Wolfiora/InnoEngine' } } }
$selector = Join-Path $scripts 'Select-DocumentationCandidate.ps1'
$cases = 0
function Expect-Rejection([scriptblock]$Action, [string]$Reason) {
    $message = ''
    try { $null = & $Action } catch { $message = $_.Exception.Message }
    if (-not $message.StartsWith($Reason, [StringComparison]::Ordinal)) { throw "Expected '$Reason'; received '$message'." }
    $script:cases++
}
$selected = & $selector -EngineTree $tree -PullRequests @($pull)
if ($selected.revision -cne $revision -or $selected.repository -cne 'Wolfiora/InnoEngine.Docs') { throw 'Exact metadata selection failed.' }
$cases++
$selected = & $selector -EngineTree $tree -PullRequests @($pull, @{ number = 84; body = $metadata })
if ($selected.revision -cne $revision -or $selected.pullRequests.Count -ne 2) { throw 'Consistent grouped metadata did not retain its owners.' }
$cases++
Expect-Rejection { & $selector -EngineTree $tree -PullRequests @() } 'No immutable documentation candidate'
Expect-Rejection { & $selector -EngineTree ('b' * 40) -PullRequests @($pull) } 'No immutable documentation candidate'
Expect-Rejection { & $selector -EngineTree $tree -PullRequests @($pull, @{ number = 84; body = $metadata.Replace($revision, ('c' * 40)) }) } 'Conflicting documentation revisions'
Expect-Rejection { & $selector -EngineTree $tree -PullRequests @(@{ number = 83; body = $metadata.Replace('Wolfiora/InnoEngine.Docs', 'other/repository') }) } 'Malformed documentation metadata'
Expect-Rejection { & $selector -EngineTree $tree -PullRequests @(@{ number = 83; body = $metadata.Replace($revision, 'main') }) } 'Malformed documentation metadata'
Expect-Rejection { & $selector -EngineTree $tree -ExplicitRevision 'main' } 'Manual documentation selection requires'
$selected = & $selector -EngineTree $tree -ExplicitRevision $revision
if ($selected.revision -cne $revision -or $selected.pullRequests.Count -ne 0) { throw 'Manual immutable selection failed.' }
$cases++

# Exercise the public orchestration entry with isolated Git inputs and simulated GitHub responses.
# No network, credential provisioning or hosted success is inferred from these protocol fixtures.
$transport = @{ requests = [Collections.Generic.List[string]]::new(); mode = 'associated' }
$transportCall = {
    param([string]$Uri, [string]$Method, [object]$Headers, [string]$ContentType, [string]$Body)
    $transport.requests.Add($Uri)
    if ($Uri -ceq 'https://api.github.com/repos/Wolfiora/InnoEngine/pulls/83') {
        if ($transport.mode -ceq 'advanced-pr') {
            return @{ number = 83; body = $metadata; head = @{ sha = $commit }; merge_commit_sha = $commit
                base = @{ sha = '0' * 40; repo = @{ full_name = 'Wolfiora/InnoEngine' } } }
        }
        return $pull
    }
    if ($Uri -ceq 'https://api.github.com/graphql') {
        if ($transport.mode -ceq 'queue-error') { return @{ errors = @(@{ message = 'Unavailable queue' }) } }
        $request = $Body | ConvertFrom-Json
        if ($request.variables.branch -cne 'master') { throw 'Queue lookup used the wrong base branch.' }
        if (-not $request.variables.after) {
            return @{ data = @{ repository = @{ mergeQueue = @{ entries = @{
                nodes = @(@{ headCommit = @{ oid = $base }; pullRequest = $pull })
                pageInfo = @{ hasNextPage = $true; endCursor = 'second-page' }
            } } } } }
        }
        if ($request.variables.after -cne 'second-page') { throw 'Queue pagination lost its cursor.' }
        return @{ data = @{ repository = @{ mergeQueue = @{ entries = @{
            nodes = @(@{ headCommit = @{ oid = $commit }; pullRequest = $pull })
            pageInfo = @{ hasNextPage = $false; endCursor = 'finished' }
        } } } } }
    }
    if (-not $Uri.StartsWith("https://api.github.com/repos/Wolfiora/InnoEngine/commits/$commit/pulls?", [StringComparison]::Ordinal)) {
        throw 'Unexpected candidate discovery endpoint.'
    }
    if ($Uri.EndsWith('page=1', [StringComparison]::Ordinal)) { return @(1..100 | ForEach-Object { $pull }) }
    if ($Uri.EndsWith('page=2', [StringComparison]::Ordinal)) { return @($pull) }
    throw 'Associated PR pagination did not stop.'
}
Set-Item -Path Function:Invoke-RestMethod -Value $transportCall.GetNewClosure()
$getter = Join-Path $scripts 'Get-DocumentationCandidate.ps1'
$eventFile = Join-Path $root 'event.json'
function Write-Event([object]$Extra) {
    $value = @{ repository = @{ full_name = 'Wolfiora/InnoEngine' } }
    foreach ($key in $Extra.Keys) { $value[$key] = $Extra[$key] }
    $value | ConvertTo-Json -Depth 8 | Set-Content $eventFile
}
Write-Event @{ pull_request = $pull }
# Retrying an unchanged merge candidate may read corrected pairing metadata from the PR API.
$oldEvent = Get-Content $eventFile -Raw | ConvertFrom-Json
$oldEvent.pull_request.body = 'Pairing metadata has not been added yet.'
$oldEvent | ConvertTo-Json -Depth 8 | Set-Content $eventFile
$selected = & $getter -EventPath $eventFile -Event pull_request -ExpectedCommit $commit
if ($selected.revision -cne $revision -or $transport.requests.Count -ne 1) { throw 'PR selection did not refresh metadata for its unchanged candidate.' }
$cases++
$transport.mode = 'advanced-pr'
Expect-Rejection { & $getter -EventPath $eventFile -Event pull_request -ExpectedCommit $commit } 'The pull request base, head or merge candidate changed'
$transport.mode = 'associated'
$transport.requests.Clear()
$selected = & $getter -EventPath $eventFile -Event workflow_dispatch -ExpectedCommit $commit -ExplicitRevision $revision
if ($selected.revision -cne $revision -or $transport.requests.Count -ne 0) { throw 'Explicit dispatch must not silently search other candidates.' }
$cases++
Expect-Rejection { & $getter -EventPath $eventFile -Event pull_request -ExpectedCommit $commit -ExplicitRevision $revision } 'Only manual dispatch accepts'
Expect-Rejection { & $getter -EventPath $eventFile -Event pull_request -ExpectedCommit ('0' * 40) } 'Engine checkout does not match'
foreach ($eventName in @('push', 'schedule', 'workflow_dispatch')) {
    $transport.requests.Clear()
    $selected = & $getter -EventPath $eventFile -Event $eventName -ExpectedCommit $commit
    if ($selected.revision -cne $revision -or $transport.requests.Count -ne 2) { throw 'Associated PR discovery did not paginate completely.' }
    $cases++
}
Write-Event @{ merge_group = @{ head_sha = $commit; base_sha = $base; base_ref = 'refs/heads/master' } }
$transport.requests.Clear()
$selected = & $getter -EventPath $eventFile -Event merge_group -ExpectedCommit $commit
if ($selected.revision -cne $revision -or $transport.requests.Count -ne 2 -or $LASTEXITCODE -ne 0) { throw 'Queue entry selection or its success exit code is incorrect.' }
$cases++
$transport.mode = 'queue-error'
Expect-Rejection { & $getter -EventPath $eventFile -Event merge_group -ExpectedCommit $commit } 'The active merge queue could not be read'
Write-Output "Passed $cases candidate selection and discovery protocol cases. Evidence: $root"
$global:LASTEXITCODE = 0
