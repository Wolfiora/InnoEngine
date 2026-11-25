$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$scripts = Join-Path $PSScriptRoot '../../tools/ci'
$platform = (Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)[0]
$revision = (git rev-parse HEAD).Trim()
$landedTree = (git rev-parse 'HEAD^{tree}').Trim()
$tested = 'a' * 40
$head = 'b' * 40
$repo = @{full_name='fixture/engine'}
$pull = @{merged_at='2026-01-01'; merge_commit_sha=$revision; base=@{ref='master';repo=$repo}; head=@{sha=$head;repo=$repo}}
$workflow = @{id=12;run_attempt=2;repository=$repo;head_repository=$repo;head_sha=$head;status='completed';conclusion='failure';
    path='.github/workflows/engine-ci.yml';event='pull_request';display_title="Engine validation $tested"}
$verify = @{name='Verify new backend publication inputs';conclusion='success';started_at='2026-01-01T00:00:03Z'}
$artifact = @{name="backend-candidate-$($platform.target)";id=345;expired=$false;created_at='2026-01-01T00:00:02Z';digest=''}
$commitData = @{tree=@{sha=$landedTree};parents=@(@{sha=$head})}
function Invoke-RestMethod($Uri,$Headers,[switch]$SkipHttpErrorCheck,$StatusCodeVariable) {
    switch -Wildcard ($Uri) {
        '*/pulls?per_page=100' { return ,@($pull) }
        '*event=pull_request*' { return @{workflow_runs=@($workflow)} }
        '*event=merge_group*' { return @{workflow_runs=@()} }
        '*/git/commits/*' { Set-Variable -Scope 1 -Name $StatusCodeVariable -Value 200; return $commitData }
        '*/12/attempts/2/jobs*' { return @{jobs=@(
            @{name="platform / Backend build ($($platform.target))";conclusion='success';started_at='2026-01-01T00:00:01Z';completed_at='2026-01-01T00:00:02Z'},
            @{name="platform / Engine tests ($($platform.runner))";conclusion='success';steps=@($verify)})} }
        '*/12/artifacts*' { return @{artifacts=@($artifact)} }
        default { throw "Unexpected fixture request $Uri" }
    }
}
function Get-Candidate {
    & "$scripts/Get-MergedBackendCandidates.ps1" -Targets @($platform) -Repository $repo.full_name -Revision $revision -DefaultBranch master
}
$work = [IO.Directory]::CreateTempSubdirectory('InnoPromote-').FullName
$bundle = Join-Path $work 'artifact.zip'
function Invoke-WebRequest($Uri,$Headers,$OutFile) {
    if (!$Uri.EndsWith('/actions/artifacts/345/zip')) { throw "Unexpected fixture download $Uri" }
    [IO.File]::Copy($bundle,$OutFile,$true)
}
try {
    $inputDir = [IO.Directory]::CreateDirectory((Join-Path $work 'input')).FullName
    $asset = Join-Path $inputDir $platform.asset
    [IO.File]::WriteAllText($asset,'original backend bytes')
    $digest = (Get-FileHash $asset).Hash.ToLowerInvariant()
    @{commit=$tested;target=$platform.target;version=2;sha256=$digest} | ConvertTo-Json | Set-Content (Join-Path $inputDir 'consumption.json')
    [IO.Compression.ZipFile]::CreateFromDirectory($inputDir,$bundle)
    $artifact.digest = 'sha256:' + (Get-FileHash $bundle).Hash.ToLowerInvariant()
    $accepted = Get-Candidate
    if ($accepted[$platform.target].artifact -ne 345) { throw 'A certified target was lost after a sibling failed.' }
    $request = [IO.Directory]::CreateDirectory((Join-Path $work "backend-request-$($platform.runner)"))
    $package = @{target=$platform.target;operation='produce';sha256=$null;assetId=$null}
    @{commit=$revision;packages=@($package);executedLocally=$false;deferred=$true} | ConvertTo-Json -Depth 4 |
        Set-Content (Join-Path $request 'acquisition.json')
    $envRepo = $env:GITHUB_REPOSITORY
    $env:GITHUB_REPOSITORY = $repo.full_name
    try {
        $joined = & "$scripts/Join-BackendAcquisitions.ps1" -Received $work -Hosts $platform.runner -Revision $revision -DefaultBranch master
    } finally { $env:GITHUB_REPOSITORY = $envRepo }
    $chain = $joined.chains[0]
    if ($chain.builders.include.Count -or $chain.producers.include[0].operation -cne 'promote' -or $chain.consumers.include[0].pattern) {
        throw 'A verified PR candidate must reach publication without native preparation, production or a second artifact upload.'
    }
    $platform | Add-Member promotion $accepted[$platform.target]
    $received = & "$scripts/Receive-BackendCandidate.ps1" -Platform $platform -Directory (Join-Path $work 'output') -ApiUrl 'https://fixture.invalid' -Repository $repo.full_name
    if ((Get-FileHash $received).Hash.ToLowerInvariant() -cne $digest) { throw 'Promotion changed the ZIP.' }
    [IO.File]::AppendAllText($bundle,'changed')
    $rejected = $false
    try { & "$scripts/Receive-BackendCandidate.ps1" -Platform $platform -Directory (Join-Path $work 'corrupt') -ApiUrl 'https://fixture.invalid' -Repository $repo.full_name }
    catch { $rejected = $_.Exception.Message.Contains('artifact changed') }
    if (!$rejected) { throw 'Changed artifact accepted.' }
    $verify.conclusion = 'skipped'
    if ((Get-Candidate).Count) { throw 'Skipped host verification accepted.' }
    $verify.conclusion = 'success'
    $artifact.expired = $true
    if ((Get-Candidate).Count) { throw 'Expired artifact accepted.' }
    $artifact.expired = $false
    $commitData.tree.sha = 'f' * 40
    if ((Get-Candidate).Count) { throw 'Different landing tree accepted.' }
    $commitData.tree.sha = $landedTree
    $pull.head.repo = @{full_name='fork/engine'}
    if ((Get-Candidate).Count) { throw 'A fork supplied a release candidate.' }
} finally {
    [IO.Directory]::Delete($work,$true)
}
