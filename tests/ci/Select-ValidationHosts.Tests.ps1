$ErrorActionPreference = 'Stop'
$baseline = 'a'*40
$candidate = 'b'*40
$hostsToRun = @([pscustomobject]@{runner='required';fast=$true},[pscustomobject]@{runner='conditional';fast=$false})
$change = "M`ttests/Feature/Test.cs"
$routingFixture = [IO.Path]::GetTempFileName()
$originalId = $env:GITHUB_REPOSITORY_ID
$failingGit = ''
function git {
    $global:LASTEXITCODE = if ($failingGit -and $args -contains $failingGit) { 128 } else { 0 }
    if ($args -contains 'diff' -and !$LASTEXITCODE) { $change }
}
function Invoke-RestMethod($Headers,$Uri) {
    if ($Uri.Contains('/workflows/')) {
        return @{workflow_runs=@(@{head_sha=$baseline;head_branch='master';head_repository=@{id=7};event='push';conclusion='success'
            path='.github/workflows/engine-ci.yml';id=12;run_attempt=1})}
    }
    return @{artifacts=@(@{name='validation-host-routing-attempt-1';expired=$false;digest=$digest;id=14;size_in_bytes=512
        workflow_run=@{id=12;head_sha=$baseline;repository_id=7}})}
}
function Invoke-WebRequest($Headers,$Uri,$OutFile) { [IO.File]::Copy($routingFixture,$OutFile,$true) }
function Check([int]$Expected) {
    $selected = @(& "$PSScriptRoot/../../tools/ci/Select-ValidationHosts.ps1" -Baseline $baseline -Candidate $candidate -Hosts $hostsToRun -DefaultBranch master)
    if ($selected.Count -ne $Expected -or $selected[0].runner -cne 'required') { throw "Unexpected route for $change." }
}
try {
    $env:GITHUB_REPOSITORY_ID = '7'
    $stream = [IO.File]::Open($routingFixture,'Create')
    $zip = [IO.Compression.ZipArchive]::new($stream,[IO.Compression.ZipArchiveMode]::Create)
    try {
        $writer = [IO.StreamWriter]::new($zip.CreateEntry('host-routing.json').Open())
        try { $writer.Write((ConvertTo-Json @{commit=$baseline;hosts=@{conditional=@('tests/Feature/Test.cs','tools/ci/Invoke-ValidationGate.ps1')}} -Depth 5)) }
        finally { $writer.Dispose() }
    } finally { $zip.Dispose(); $stream.Dispose() }
    $digest = 'sha256:'+(Get-FileHash -LiteralPath $routingFixture).Hash.ToLowerInvariant()
    Check 1
    $change = "M`ttools/ci/Invoke-ValidationGate.ps1"
    Check 1
    $change = "M`t.github/workflows/engine-ci.yml"
    Check 2
    $change = "A`ttests/Feature/Test.cs"
    Check 2
    $change = "M`ttests/Shared.cs"
    Check 2
    $change = "M`ttests/Feature/Test.cs"
    $digest = 'sha256:'+('0'*64)
    Check 2
    $env:GITHUB_REPOSITORY_ID = '8'
    Check 2
    # A stale pull-request base is absent from the shallow checkout; the step must still succeed with every host.
    foreach ($failingGit in @('merge-base', 'diff')) {
        Check 2
        if ($LASTEXITCODE) { throw "A handled git $failingGit failure leaked exit code $LASTEXITCODE." }
    }
} finally { [IO.File]::Delete($routingFixture); $env:GITHUB_REPOSITORY_ID = $originalId }
