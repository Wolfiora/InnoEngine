$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/continuation-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$scripts = Join-Path $root 'scripts'
New-Item -ItemType Directory -Path $scripts -Force | Out-Null
foreach ($name in @('Get-ValidationContinuation.ps1', 'Select-ValidationArtifact.ps1', 'Expand-ValidationArtifact.ps1', 'Transfer-ValidationOutputs.ps1')) {
    Copy-Item (Join-Path $PSScriptRoot $name) $scripts
}
# Receipt semantics execute in the validation contract/integration suites. This boundary fixture
# isolates authenticated remote selection and real digest-checked archive extraction.
@'
param($PlanPath, $ResultsPath, $ExpectedCommit, $JobConclusion)
@{ planIdentity = 'a' * 64; resultsIdentity = 'b' * 64 }
'@ | Set-Content (Join-Path $scripts 'Test-ValidationReport.ps1')
$commit = 'c' * 40
$archiveFiles = @{}
$roles = @('engine-certification', 'engine-acceptance-evidence', 'validation-outputs-windows-2022-managed')
$id = 0
$artifacts = foreach ($role in $roles) {
    $id++
    $source = Join-Path $root "source/$role"
    New-Item -ItemType Directory -Path $source -Force | Out-Null
    if ($id -eq 1) {
        @(@{ platform = 'windows-2022'; receipt = @{ planIdentity = 'a' * 64; resultsIdentity = 'b' * 64 } }) |
            ConvertTo-Json -Depth 8 -AsArray | Set-Content (Join-Path $source 'certification.json')
    } elseif ($id -eq 2) {
        New-Item -ItemType Directory -Path (Join-Path $source 'windows-2022') | Out-Null
        @{ profile = 2; extended = $false } | ConvertTo-Json | Set-Content (Join-Path $source 'windows-2022/plan.json')
        '{}' | Set-Content (Join-Path $source 'windows-2022/results.json')
    } else {
        $compiled = Join-Path $root 'compiled-source'
        New-Item -ItemType Directory -Path $compiled | Out-Null
        'compiled fixture bytes' | Set-Content (Join-Path $compiled 'fixture.dll')
        & "$PSScriptRoot/Transfer-ValidationOutputs.ps1" -Operation Pack -Directory $compiled -Archive (Join-Path $source 'outputs-managed.tar')
    }
    $zip = Join-Path $root "$role.zip"
    [IO.Compression.ZipFile]::CreateFromDirectory($source, $zip)
    $archiveFiles[$id] = $zip
    @{ id = $id; name = "$role-attempt-1"; expired = $false; expires_at = [DateTimeOffset]::UtcNow.AddDays(1).ToString('O');
       digest = 'sha256:' + (Get-FileHash $zip).Hash.ToLowerInvariant(); workflow_run = @{ id = 42; head_sha = $commit; repository_id = 7 } }
}
$originalToken = $env:GH_TOKEN
$passed = 0
try {
    $env:GH_TOKEN = 'fixture-read-token'
    foreach ($case in @('valid', 'pull-request', 'foreign-repository', 'wrong-commit', 'failed', 'wrong-workflow', 'wrong-artifact-run', 'corrupt-digest', 'missing-evidence')) {
        $state = @{ run = @{ id = 42; workflow_id = 9; status = 'completed'; conclusion = 'success'; head_repository = @{ id = 7 };
            head_sha = $commit; head_branch = 'master'; event = 'push'; path = '.github/workflows/engine-ci.yml'; run_attempt = 1 };
            artifacts = @($artifacts | ConvertTo-Json -Depth 12 | ConvertFrom-Json -AsHashtable); downloads = 0 }
        switch ($case) {
            'pull-request' { $state.run.event = 'pull_request' }
            'foreign-repository' { $state.run.head_repository.id = 8 }
            'wrong-commit' { $state.run.head_sha = 'd' * 40 }
            'failed' { $state.run.conclusion = 'failure' }
            'wrong-workflow' { $state.run.workflow_id = 10 }
            'wrong-artifact-run' { $state.artifacts[0].workflow_run.id = 43 }
            'corrupt-digest' { $state.artifacts[0].digest = 'sha256:' + ('f' * 64) }
            'missing-evidence' { $state.artifacts = @($state.artifacts | Where-Object id -NE 2) }
        }
        function Invoke-RestMethod {
            param($Uri, $Headers)
            if ($Uri.EndsWith('/actions/runs/42')) { return $state.run }
            if ($Uri.Contains('/actions/workflows/')) { return @{ id = 9 } }
            if ($Uri.Contains('/artifacts?')) { return @{ total_count = $state.artifacts.Count; artifacts = $state.artifacts } }
            return @{ id = 7; default_branch = 'master' }
        }
        function Invoke-WebRequest {
            param($Uri, $Headers, $OutFile)
            $state.downloads++
            $assetId = [int]([regex]::Match($Uri, '/artifacts/(\d+)/zip$').Groups[1].Value)
            Copy-Item $archiveFiles[$assetId] $OutFile
        }
        $accepted = $false
        try {
            & (Join-Path $scripts 'Get-ValidationContinuation.ps1') -RunId 42 -Commit $commit -Platform windows-2022 -Shard managed `
                -Repository fixture/engine -ApiUrl https://example.invalid -OutputDirectory (Join-Path $root $case)
            $accepted = $true
        } catch { if ($case -eq 'valid') { throw } }
        if ($accepted -ne ($case -eq 'valid')) { throw "Unexpected continuation result: $case" }
        if ($case -notin @('valid', 'corrupt-digest') -and $state.downloads -ne 0) { throw 'Unauthenticated input reached artifact download.' }
        if ($accepted -and -not (Test-Path (Join-Path $root "$case/continuation.json"))) { throw 'Accepted continuation has no explicit identity selection.' }
        if ($accepted -and (Get-Content (Join-Path $root "$case/compiled/fixture.dll") -Raw).Trim() -cne 'compiled fixture bytes') { throw 'Compiled bytes were not restored.' }
        $passed++
    }
} finally { $env:GH_TOKEN = $originalToken }
Write-Output "Passed $passed continuation authentication and extraction cases."
