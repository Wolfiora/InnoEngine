$ErrorActionPreference = 'Stop'
$fixture = [IO.Path]::GetTempFileName()
$identity = 'a'*64
$sha = 'b'*40
$name = "backend-tests-fixture-$identity"
$runData = @{id=3;status='completed';conclusion='success';head_sha=$sha;head_branch='master';head_repository=@{id=2}
    event='push';path='.github/workflows/engine-ci.yml';run_attempt=1}
function Invoke-RestMethod($Uri,$Headers) {
    if ($Uri.Contains('/actions/artifacts?')) { return @{artifacts=@($advertised)} }
    if ($Uri.EndsWith('/actions/runs/3')) { return $runData }
    throw 'Unexpected evidence request.'
}
function Invoke-WebRequest($Uri,$Headers,$OutFile) {
    if (!$Uri.EndsWith('/actions/artifacts/4/zip')) { throw 'Unexpected proof download.' }
    [IO.File]::Copy($fixture,$OutFile,$true)
}
function Read-Proof { & "$PSScriptRoot/../../tools/ci/Get-BackendTestEvidence.ps1" -ApiBase https://fixture.invalid -Headers @{} -HostTarget fixture -EnvironmentIdentity $identity -DefaultBranch master -RepositoryId 2 }
try {
    $stream = [IO.File]::Open($fixture,'Create')
    $zip = [IO.Compression.ZipArchive]::new($stream,[IO.Compression.ZipArchiveMode]::Create)
    try {
        $writer = [IO.StreamWriter]::new($zip.CreateEntry('backend-tests.json').Open())
        try { $writer.Write((ConvertTo-Json @{version=2;commit=$sha;environment=$identity;hostTarget='fixture';attempt=1;tests=@(@{id='test:fixture'})} -Depth 5)) }
        finally { $writer.Dispose() }
    } finally { $zip.Dispose(); $stream.Dispose() }
    $advertised = @{id=4;name=$name;expired=$false;size_in_bytes=384;digest='sha256:'+(Get-FileHash -LiteralPath $fixture).Hash.ToLowerInvariant()
        workflow_run=@{id=3;head_branch='master';head_sha=$sha;repository_id=2;head_repository_id=2}}
    if (@(Read-Proof).Count -ne 1) { throw 'Certified backend test evidence was lost.' }
    $runData.event = 'pull_request'
    if (@(Read-Proof).Count) { throw 'PR backend evidence escaped its trust boundary.' }
    $runData.event = 'push'
    $runData.run_attempt = 2
    if (@(Read-Proof).Count) { throw 'Evidence from another attempt was accepted.' }
    $runData.run_attempt = 1
    $advertised.workflow_run.head_repository_id = 7
    if (@(Read-Proof).Count) { throw 'Fork backend evidence was accepted.' }
    $advertised.workflow_run.head_repository_id = 2
    $advertised.digest = 'sha256:'+('0'*64)
    try { $null = Read-Proof; throw 'Damaged test evidence passed.' }
    catch { if ($_.Exception.Message -cne 'Backend test evidence changed during transfer.') { throw } }
} finally { [IO.File]::Delete($fixture) }
