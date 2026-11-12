$ErrorActionPreference = 'Stop'
$old = $env:INNO_DOCS_READ_TOKEN
$env:INNO_DOCS_READ_TOKEN = 'docs-read-fixture'
$state = @{ mode = ''; writes = @(); reads = 0 }
$docs = 'a' * 40
$tree = 'b' * 40
$call = {
    param([string]$Uri, [string]$Method, $Headers, [string]$ContentType, [string]$Body)
    if ($Headers.Authorization -cne 'Bearer docs-read-fixture' -or $Method) { throw 'Pairing used write access or the wrong credential.' }
    $prefix = 'https://api.github.com/repos/Wolfiora/InnoEngine.Docs'
    if ($Uri -ceq $prefix) { return @{ default_branch = 'main' } }
    if ($Uri -like "$prefix/compare/*") {
        return @{ status = $(if ($state.mode -ceq 'unmerged') { 'diverged' } else { 'ahead' }); merge_base_commit = @{ sha = $docs } }
    }
    if ($Uri -like "$prefix/contents/*") {
        $value = @{ repository = 'https://github.com/Wolfiora/InnoEngine'; revision = 'c' * 40; sourceTree = $tree }
        if ($state.mode -ceq 'wrong-tree') { $value.sourceTree = 'd' * 40 }
        $bytes = [Text.Encoding]::UTF8.GetBytes(($value | ConvertTo-Json))
        return @{ type = 'file'; encoding = 'base64'; size = $bytes.Length; content = [Convert]::ToBase64String($bytes) }
    }
    if ($Uri -like '*documentation.yml/runs?*') {
        $run = @{ id = 12; head_sha = $docs; path = '.github/workflows/documentation.yml'; run_attempt = 2; event = 'pull_request'
            repository = @{ full_name = 'Wolfiora/InnoEngine.Docs' }; head_repository = @{ full_name = 'Wolfiora/InnoEngine.Docs' }
            status = 'completed'; conclusion = 'success' }
        switch ($state.mode) {
            'missing-run' { return @{ workflow_runs = @() } }
            'failed' { $run.conclusion = 'failure' }
            'canceled' { $run.conclusion = 'cancelled' }
            'running' { $run.status = 'in_progress' }
            'fork' { $run.head_repository.full_name = 'Other/Docs' }
            'wrong-workflow' { $run.path = '.github/workflows/preview.yml' }
            'newer-failure' {
                $failed = @{} + $run; $failed.id = 13; $failed.conclusion = 'failure'
                return @{ workflow_runs = @($run, $failed) }
            }
        }
        return @{ workflow_runs = @($run) }
    }
    if ($Uri -like '*/attempts/2/jobs?*') {
        $job = @{ name = 'Documentation validation'; run_id = 12; status = 'completed'; conclusion = 'success' }
        switch ($state.mode) {
            'missing-job' { return @{ jobs = @() } }
            'duplicate-job' { return @{ jobs = @($job, $job) } }
            'skipped-job' { $job.conclusion = 'skipped' }
        }
        return @{ jobs = @($job) }
    }
    if ($Uri -like '*/artifacts?*') {
        $artifact = @{ id = 20; name = 'documentation-site-attempt-2'; expired = $false; size_in_bytes = 100
            digest = 'sha256:' + ('e' * 64); workflow_run = @{ id = 12; head_sha = $docs } }
        switch ($state.mode) {
            'missing-artifact' { return @{ artifacts = @() } }
            'expired' { $artifact.expired = $true }
            'wrong-attempt' { $artifact.name = 'documentation-site-attempt-1' }
            'missing-digest' { $artifact.Remove('digest') }
            'wrong-producer' { $artifact.workflow_run.id = 11 }
        }
        return @{ artifacts = @($artifact) }
    }
    throw "Unexpected pairing endpoint $Uri"
}
Set-Item Function:Invoke-RestMethod $call.GetNewClosure()
$cases = 0
try {
    foreach ($mode in @('', 'unmerged', 'wrong-tree', 'missing-run', 'failed', 'canceled', 'running', 'fork', 'wrong-workflow',
        'newer-failure', 'missing-job', 'duplicate-job', 'skipped-job', 'missing-artifact', 'expired', 'wrong-attempt', 'missing-digest', 'wrong-producer')) {
        $state.mode = $mode
        $errorValue = $null
        try { $result = & "$PSScriptRoot/Test-DocumentationPairing.ps1" -EngineTree $tree -DocumentationRevision $docs } catch { $errorValue = $_ }
        if (-not $mode -and ($errorValue -or $result.status -cne 'Passed')) { throw "Valid pair failed: $errorValue" }
        if ($mode -and -not $errorValue) { throw "Invalid pairing passed: $mode" }
        $cases++
    }
} finally {
    Remove-Item Function:Invoke-RestMethod
    $env:INNO_DOCS_READ_TOKEN = $old
}
Write-Output "Passed $cases documentation pairing evidence cases. Hosted configuration and delivery remain separate."
