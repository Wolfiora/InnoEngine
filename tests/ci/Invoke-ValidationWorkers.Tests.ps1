$ErrorActionPreference = 'Stop'
$fixture = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/worker-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$scripts = Join-Path $fixture 'tests/ci'
$plan = Join-Path $fixture 'artifacts/validation/ci-plan'
$entry = 'artifacts/validation/bootstrap/outputs/bin/ValidationEntry/debug/ValidationEntry.dll'
New-Item -ItemType Directory -Force -Path $scripts, $plan, (Split-Path (Join-Path $fixture $entry)) | Out-Null
foreach ($script in @('Invoke-ValidationWorkers.ps1','Prepare-ValidationShard.ps1','Select-ValidationArtifact.ps1')) {
    Copy-Item -LiteralPath (Join-Path $PSScriptRoot $script) -Destination $scripts
}
@'
param($Stage,$BaseRevision,$ExpectedCommit,$Shard,$PlanDirectory,$OutputDirectory,$PrerequisitesDirectory,$SourceRunId)
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
if ((& git -C $root rev-parse HEAD).Trim() -cne $ExpectedCommit) { throw 'Incorrect worker checkout.' }
$plan = Get-Content -LiteralPath (Join-Path $root "$PlanDirectory/plan.json") -Raw | ConvertFrom-Json
if ($Stage -ceq 'Join') {
    $ids = @(Get-ChildItem -LiteralPath $PrerequisitesDirectory -Directory | ForEach-Object Name)
    if ($ids.Count -ne $plan.shards.Count -or @($ids | Where-Object { $_ -cnotin $plan.shards.id }).Count) { throw 'Incomplete local join.' }
    return
}
$required = @($plan.shards | Where-Object id -CEQ $Shard).prerequisites
$received = @(Get-ChildItem -LiteralPath $PrerequisitesDirectory -Recurse -File -Filter 'results.json')
if ($received.Count -ne $required.Count) { throw 'Incorrect prerequisite count.' }
foreach ($receipt in $received) {
    if ((Get-Content $receipt.FullName -Raw | ConvertFrom-Json).id -cnotin $required) { throw 'Unexpected prerequisite.' }
}
if ($Shard -ceq 'managed' -and $env:INNO_WORKER_FIXTURE_FAILURE -ceq 'failure') { throw 'Injected worker failure.' }
if ($Shard -ceq 'managed' -and $env:INNO_WORKER_FIXTURE_FAILURE -ceq 'missing') { return }
$out = Join-Path $root $OutputDirectory
New-Item -ItemType Directory -Path $out -Force | Out-Null
@{ id = $Shard; checkout = $root } | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $out 'results.json')
"compiledOutputs=$out" >> $env:GITHUB_OUTPUT
'@ | Set-Content -LiteralPath (Join-Path $scripts 'Invoke-ValidationCi.ps1')
'artifacts/' | Set-Content -LiteralPath (Join-Path $fixture '.gitignore')
'entry' | Set-Content -LiteralPath (Join-Path $fixture $entry)
@{ assembly = $entry; sha256 = (Get-FileHash -LiteralPath (Join-Path $fixture $entry)).Hash } |
    ConvertTo-Json | Set-Content -LiteralPath (Join-Path $plan 'entry.json')
& git -C $fixture init -q
& git -C $fixture add .
& git -C $fixture -c user.name=Fixture -c user.email=fixture@example.invalid commit -qm 'Worker contracts'
if ($LASTEXITCODE) { throw 'Cannot create the worker fixture.' }
$commit = (& git -C $fixture rev-parse HEAD).Trim()
$originalOutput = $env:GITHUB_OUTPUT
$originalFailure = $env:INNO_WORKER_FIXTURE_FAILURE
try {
    foreach ($case in @('managed-only','parallel','failure','missing','cycle')) {
        $shards = @(@{ id = 'architecture'; prerequisites = @() }, @{ id = 'managed'; prerequisites = @('architecture') })
        if ($case -ceq 'parallel' -or $case -ceq 'failure') { $shards += @{ id = 'native'; prerequisites = @('architecture') } }
        if ($case -ceq 'cycle') { $shards[0].prerequisites = @('managed') }
        @{ commit = $commit; shards = $shards; nativeRequests = @() } | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $plan 'plan.json')
        $env:GITHUB_OUTPUT = Join-Path $fixture "artifacts/$case.outputs"
        $env:INNO_WORKER_FIXTURE_FAILURE = $case
        $workerPath = Join-Path (Split-Path $fixture -Parent) ((Split-Path $fixture -Leaf) + "-$case")
        $failed = $false
        try { & (Join-Path $scripts 'Invoke-ValidationWorkers.ps1') -Platform fixture -ExpectedCommit $commit -WorkerDirectory $workerPath }
        catch { $failed = $true; Write-Host $_.Exception.Message }
        if ($failed -ne ($case -cin @('failure','missing','cycle'))) { throw "Unexpected outcome: $case." }
        if ($case -cin @('parallel','failure')) {
            $native = Get-Content (Join-Path $fixture 'artifacts/validation/shards/native/results.json') -Raw | ConvertFrom-Json
            if ($native.id -cne 'native' -or $native.checkout -cne $fixture) { throw 'An independent native receipt was lost.' }
        }
        if ($case -ceq 'parallel') {
            $managed = Get-Content (Join-Path $workerPath 'managed/artifacts/validation/shards/managed/results.json') -Raw | ConvertFrom-Json
            if ($managed.checkout -ceq $fixture) { throw 'Concurrent workers shared their writable checkout.' }
        }
        if ($case -ceq 'managed-only' -and (Test-Path -LiteralPath (Join-Path $workerPath 'managed'))) { throw 'An independent worker was created for a single execution path.' }
        # Preserve this case's receipts without allowing a later case to observe them as its own.
        $shardOutput = Join-Path $fixture 'artifacts/validation/shards'
        if (Test-Path -LiteralPath $shardOutput) { Rename-Item -LiteralPath $shardOutput -NewName "shards-$case" }
    }
} finally {
    $env:GITHUB_OUTPUT = $originalOutput
    $env:INNO_WORKER_FIXTURE_FAILURE = $originalFailure
}
Write-Output 'Passed five co-located planning and worker ownership contracts.'
$global:LASTEXITCODE = 0
