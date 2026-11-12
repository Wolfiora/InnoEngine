$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/documentation-baseline-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
[IO.Directory]::CreateDirectory($root) | Out-Null
$originalToken = $env:GH_TOKEN
$env:GH_TOKEN = 'fixture-repository-read'
$commit = 'a' * 40
$state = @{ mode = ''; downloads = 0; archive = ''; digest = '' }
$results = [Collections.Generic.List[object]]::new()
$metadata = {
    param([string]$Uri, [object]$Headers)
    if ($Headers.Authorization -cne 'Bearer fixture-repository-read') { throw 'Baseline lookup used a different credential.' }
    if ($Uri -ceq 'https://api.github.com/repos/Wolfiora/InnoEngine') {
        return @{ id = 7; full_name = 'Wolfiora/InnoEngine'; default_branch = 'master' }
    }
    if ($Uri -like '*/workflows/rendering-ci.yml/runs?*') {
        $run = @{ id = 11; head_sha = $commit; repository = @{ id = 7 }; head_repository = @{ id = 7 }
            head_branch = 'master'; path = '.github/workflows/rendering-ci.yml'; event = 'push'; status = 'completed'; conclusion = 'success'; run_attempt = 2 }
        switch ($state.mode) {
            'no-runs' { return @{ workflow_runs = @() } }
            'fork' { $run.head_repository.id = 8 }
            'wrong-workflow' { $run.path = '.github/workflows/untrusted.yml' }
            'wrong-commit' { $run.head_sha = 'b' * 40 }
            'wrong-branch' { $run.head_branch = 'feature' }
            'pr-producer' { $run.event = 'pull_request' }
            'unfinished' { $run.status = 'in_progress' }
            'failed' { $run.conclusion = 'failure' }
        }
        return @{ workflow_runs = @($run) }
    }
    if ($Uri -like '*/runs/11/artifacts?*') {
        $artifact = @{ id = 22; name = 'documentation-source-certification-attempt-2'; size_in_bytes = (Get-Item $state.archive).Length
            expired = $false; digest = $state.digest; workflow_run = @{ id = 11; head_sha = $commit; repository_id = 7; head_repository_id = 7 } }
        switch ($state.mode) {
            'no-certificate' { return @{ artifacts = @() } }
            'wrong-attempt' { $artifact.name = 'documentation-source-certification-attempt-1' }
            'expired' { $artifact.expired = $true }
            'missing-digest' { $artifact.digest = '' }
            'tampered-archive' { $artifact.digest = 'sha256:' + ('c' * 64) }
            'wrong-producer' { $artifact.workflow_run.id = 12 }
            'foreign-artifact' { $artifact.workflow_run.head_repository_id = 8 }
            'duplicate-artifact' { return @{ artifacts = @($artifact, $artifact) } }
        }
        return @{ artifacts = @($artifact) }
    }
    if ($Uri -like '*/attempts/2/jobs?*') {
        $job = @{ name = 'Engine validation'; status = 'completed'; conclusion = 'success' }
        switch ($state.mode) {
            'missing-aggregate' { return @{ jobs = @() } }
            'failed-aggregate' { $job.conclusion = 'failure' }
            'duplicate-aggregate' { return @{ jobs = @($job, $job) } }
        }
        return @{ jobs = @($job) }
    }
    throw "Unexpected baseline metadata endpoint: $Uri"
}
$download = {
    param([string]$Uri, [object]$Headers, [string]$OutFile)
    if ($Uri -cne 'https://api.github.com/repos/Wolfiora/InnoEngine/actions/artifacts/22/zip' `
        -or $Headers.Authorization -cne 'Bearer fixture-repository-read') { throw 'Unexpected artifact endpoint or credential.' }
    Copy-Item -LiteralPath $state.archive -Destination $OutFile
    $state.downloads++
}
Set-Item Function:Invoke-RestMethod $metadata.GetNewClosure()
Set-Item Function:Invoke-WebRequest $download.GetNewClosure()
try {
    $unavailable = @('no-runs', 'fork', 'wrong-workflow', 'wrong-commit', 'wrong-branch', 'pr-producer', 'unfinished', 'failed', 'no-certificate', 'wrong-attempt', 'expired')
    foreach ($mode in @('complete') + $unavailable + @('missing-digest', 'tampered-archive', 'wrong-producer', 'foreign-artifact', 'duplicate-artifact',
        'missing-aggregate', 'failed-aggregate', 'duplicate-aggregate', 'escaping-entry', 'extra-entry', 'wrong-payload')) {
        $state.mode = $mode; $state.downloads = 0
        $state.archive = Join-Path $root ($mode + '.zip')
        $zip = [IO.Compression.ZipFile]::Open($state.archive, [IO.Compression.ZipArchiveMode]::Create)
        try {
            $name = if ($mode -ceq 'escaping-entry') { '../source-certificate.json' } else { 'source-certificate.json' }
            $writer = [IO.StreamWriter]::new($zip.CreateEntry($name).Open())
            try { $writer.Write((@{ format = 'inno-documentation-source/1'; repository = 'https://github.com/Wolfiora/InnoEngine'
                sourceCommit = $(if ($mode -ceq 'wrong-payload') { 'b' * 40 } else { $commit }); sourceTree = 'd' * 40 } | ConvertTo-Json)) }
            finally { $writer.Dispose() }
            if ($mode -ceq 'extra-entry') { [void]$zip.CreateEntry('unexpected.txt') }
        } finally { $zip.Dispose() }
        $state.digest = 'sha256:' + (Get-FileHash $state.archive).Hash.ToLowerInvariant()
        $actual = 'Rejected'; $diagnostic = ''
        try {
            $selected = & "$PSScriptRoot/Get-DocumentationBaseline.ps1" -Commit $commit -OutputDirectory (Join-Path $root $mode)
            $actual = $selected.status
            if ($actual -ceq 'Ready' -and ($selected.commit -cne $commit -or $selected.run -ne 11 -or $selected.attempt -ne 2 `
                -or $selected.digest -cne $state.digest -or $state.downloads -ne 1)) { throw 'Selected baseline lost exact authenticated provenance.' }
            if ($actual -ceq 'Unavailable' -and $state.downloads -ne 0) { throw 'Untrusted metadata downloaded a payload.' }
        } catch { $actual = 'Rejected'; $diagnostic = $_.Exception.Message }
        $expected = if ($mode -ceq 'complete') { 'Ready' } elseif ($mode -cin $unavailable) { 'Unavailable' } else { 'Rejected' }
        $results.Add(@{ name = $mode; actual = $actual; expected = $expected; diagnostic = $diagnostic })
        $results | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $root 'tests.json') -Encoding utf8NoBOM
        if ($actual -cne $expected) { throw "Unexpected baseline import for ${mode}: $actual $diagnostic" }
    }
} finally {
    Remove-Item Function:Invoke-RestMethod
    Remove-Item Function:Invoke-WebRequest
    $env:GH_TOKEN = $originalToken
}
Write-Output "Passed $($results.Count) authenticated baseline cases. Evidence: $root"
