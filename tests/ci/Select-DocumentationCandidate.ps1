param(
    [Parameter(Mandatory)][string]$EngineTree,
    [AllowEmptyCollection()][object[]]$PullRequests = @(),
    [string]$ExplicitRevision
)

$ErrorActionPreference = 'Stop'
if ($EngineTree -cnotmatch '^[a-f0-9]{40}$') { throw 'Documentation selection requires the actual engine source tree.' }
if ($ExplicitRevision) {
    if ($ExplicitRevision -cnotmatch '^[a-f0-9]{40}$') { throw 'Manual documentation selection requires a full commit identity, not a branch or tag.' }
    return [pscustomobject]@{ repository = 'Wolfiora/InnoEngine.Docs'; revision = $ExplicitRevision; engineTree = $EngineTree; pullRequests = @() }
}
$candidates = [Collections.Generic.List[object]]::new()
foreach ($pull in $PullRequests) {
    $lines = [regex]::Matches([string]$pull.body, '(?m)^Documentation-Candidate:[^\r\n]*')
    foreach ($line in $lines) {
        $match = [regex]::Match($line.Value, '^Documentation-Candidate: Wolfiora/InnoEngine\.Docs@(?<revision>[a-f0-9]{40}) engine-tree=(?<tree>[a-f0-9]{40})$')
        if (-not $match.Success) {
            if ($line.Value.Contains($EngineTree, [StringComparison]::Ordinal)) { throw "Malformed documentation metadata on pull request #$($pull.number)." }
            continue
        }
        if ($match.Groups['tree'].Value -ceq $EngineTree) {
            $candidates.Add([pscustomobject]@{ revision = $match.Groups['revision'].Value; pullRequest = [int]$pull.number })
        }
    }
}
$revisions = @($candidates.revision | Sort-Object -Unique)
if ($revisions.Count -eq 0) { throw "No immutable documentation candidate binds engine tree $EngineTree. Add Documentation-Candidate metadata to the corresponding engine PR." }
if ($revisions.Count -ne 1) { throw 'Conflicting documentation revisions bind the same engine candidate; resolve the pairing before validation.' }
[pscustomobject]@{
    repository = 'Wolfiora/InnoEngine.Docs'
    revision = $revisions[0]
    engineTree = $EngineTree
    pullRequests = @($candidates.pullRequest | Sort-Object -Unique)
}
