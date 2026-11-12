$ErrorActionPreference = 'Stop'
$oldReadToken = $env:GH_TOKEN
$oldWriteToken = $env:INNO_DOCS_DISPATCH_TOKEN
$env:GH_TOKEN = 'fixture-read'
$env:INNO_DOCS_DISPATCH_TOKEN = 'fixture-dispatch'
$cases = 0
$transport = @{ failure = ''; dispatched = 0 }
$call = {
    param([string]$Uri, [string]$Method, [object]$Headers, [string]$ContentType, [string]$Body)
    $sha = 'a' * 40
    if ($Uri -ceq 'https://api.github.com/repos/Wolfiora/InnoEngine.Docs/actions/workflows/source-evidence.yml/dispatches') {
        if ($Headers.Authorization -cne 'Bearer fixture-dispatch' -or $Method -cne 'Post') { throw 'Dispatch used the wrong credential or method.' }
        $payload = $Body | ConvertFrom-Json
        if ($payload.ref -cne 'main' -or $payload.inputs.engine_commit -cne $sha -or $payload.inputs.engine_run -cne '11' -or $payload.inputs.bootstrap) {
            throw 'Dispatch lost immutable producer identity.'
        }
        $transport.dispatched++
        return
    }
    if ($Headers.Authorization -cne 'Bearer fixture-read') { throw 'Engine metadata used a cross-repository write credential.' }
    if ($Uri -ceq 'https://api.github.com/repos/Wolfiora/InnoEngine') { return @{ default_branch = 'master' } }
    if ($Uri -ceq 'https://api.github.com/repos/Wolfiora/InnoEngine/actions/runs/11') {
        $run = @{ id = 11; repository = @{ full_name = 'Wolfiora/InnoEngine' }; head_repository = @{ full_name = 'Wolfiora/InnoEngine' }
            path = '.github/workflows/rendering-ci.yml'; status = 'completed'; conclusion = 'success'; head_sha = $sha; run_attempt = 2; head_branch = 'master'; event = 'push' }
        switch ($transport.failure) {
            'fork' { $run.head_repository.full_name = 'Other/InnoEngine' }
            'wrong-workflow' { $run.path = '.github/workflows/other.yml' }
            'failed' { $run.conclusion = 'failure' }
            'unfinished' { $run.status = 'in_progress' }
            'wrong-branch' { $run.head_branch = 'feature' }
            'internal-pr' { $run.head_branch = 'feature'; $run.event = 'pull_request'; $run.pull_requests = @(@{ head = @{ sha = $sha } }) }
            'stale-pr' { $run.head_branch = 'feature'; $run.event = 'pull_request'; $run.pull_requests = @(@{ head = @{ sha = 'b' * 40 } }) }
        }
        return $run
    }
    if ($Uri -like '*/attempts/2/jobs?*') {
        if ($transport.failure -ceq 'missing-aggregate') { return @{ jobs = @() } }
        return @{ jobs = @(@{ name = 'Engine validation'; status = 'completed'; conclusion = 'success' }) }
    }
    if ($Uri -like '*/artifacts?*') {
        $artifact = @{ name = 'documentation-source-certification-attempt-2'; expired = $false; size_in_bytes = 100
            digest = 'sha256:' + ('b' * 64); workflow_run = @{ id = 11; head_sha = $sha } }
        switch ($transport.failure) {
            'missing-certificate' { return @{ artifacts = @() } }
            'full-missing-certificate' { return @{ artifacts = @(@{ name = 'engine-full-certification-attempt-2' }) } }
            'expired' { $artifact.expired = $true }
            'missing-digest' { $artifact.Remove('digest') }
            'wrong-attempt' { $artifact.name = 'documentation-source-certification-attempt-1' }
            'wrong-producer' { $artifact.workflow_run.id = 12 }
        }
        return @{ artifacts = @($artifact) }
    }
    throw "Unexpected relay endpoint: $Uri"
}
Set-Item Function:Invoke-RestMethod $call.GetNewClosure()
try {
    foreach ($mode in @('', 'internal-pr', 'fork', 'wrong-workflow', 'failed', 'unfinished', 'wrong-branch', 'stale-pr', 'missing-aggregate',
        'missing-certificate', 'expired', 'missing-digest', 'wrong-attempt', 'wrong-producer')) {
        $transport.failure = $mode
        $transport.dispatched = 0
        $failure = $null
        try { & "$PSScriptRoot/Send-DocumentationEvidence.ps1" -RunId 11 | Out-Null } catch { $failure = $_ }
        $valid = $mode -cin @('', 'internal-pr')
        if ($valid -and ($failure -or $transport.dispatched -ne 1)) { throw "Valid relay failed: $failure" }
        if (-not $valid -and (-not $failure -or $transport.dispatched -ne 0)) { throw "Invalid relay dispatched: $mode" }
        $cases++
    }
    $env:INNO_DOCS_DISPATCH_TOKEN = ''
    foreach ($mode in @('', 'missing-certificate', 'full-missing-certificate', 'expired', 'wrong-producer')) {
        $transport.failure = $mode
        $transport.dispatched = 0
        $failure = $null
        $selection = $null
        try { $selection = & "$PSScriptRoot/Send-DocumentationEvidence.ps1" -RunId 11 -SelectOnly } catch { $failure = $_ }
        $valid = $mode -cin @('', 'missing-certificate')
        if ($valid -and ($failure -or $selection.status -cne $(if ($mode) { 'NotApplicable' } else { 'Ready' }))) {
            throw "Read-only certificate selection failed: $failure"
        }
        if (-not $valid -and -not $failure) { throw "Invalid read-only certificate selection passed: $mode" }
        if ($transport.dispatched -ne 0) { throw 'Read-only selection requested a cross-repository dispatch.' }
        $cases++
    }
} finally {
    Remove-Item Function:Invoke-RestMethod
    $env:GH_TOKEN = $oldReadToken
    $env:INNO_DOCS_DISPATCH_TOKEN = $oldWriteToken
}
Write-Output "Passed $cases source relay protocol cases. No hosted dispatch or installation provisioning is inferred."
