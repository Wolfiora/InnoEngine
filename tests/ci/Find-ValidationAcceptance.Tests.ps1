$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/acceptance-discovery-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$originalToken = $env:GH_TOKEN
$cases = @('match', 'no-prior-run', 'pull-request', 'foreign-repository', 'expired', 'corrupt-download', 'wrong-workflow', 'failed-result', 'changed-environment', 'changed-certificate')
$hosts = @(Get-Content (Join-Path $PSScriptRoot '../../.github/validation-hosts.json') -Raw | ConvertFrom-Json)
try {
    $env:GH_TOKEN = 'fixture-token'
    foreach ($case in $cases) {
        $root = Join-Path $owner $case
        $planRoot = Join-Path $root 'plan'
        $certificateRoot = Join-Path $root 'certificate'
        $evidenceRoot = Join-Path $root 'evidence'
        New-Item -ItemType Directory -Path $planRoot, $certificateRoot, $evidenceRoot -Force | Out-Null
        $revision = 'a' * 40
        $plan = @{ commit = $revision; tree = 'b' * 64; environment = 'c' * 64; full = $true;
            checks = @(@{ id = 'policy'; kind = 'instructions'; exclusion = $null; prerequisites = @() }) }
        $plan | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $planRoot 'plan.json')
        $planIdentity = (Get-FileHash (Join-Path $planRoot 'plan.json')).Hash.ToLowerInvariant()
        $certification = foreach ($platform in $hosts.runner) {
            $destination = Join-Path $evidenceRoot $platform
            New-Item -ItemType Directory -Path $destination | Out-Null
            Copy-Item (Join-Path $planRoot 'plan.json') $destination
            @{ commit = $revision; tree = $plan.tree; environment = $plan.environment; completed = $true; planIdentity = $planIdentity;
                results = @(@{ id = 'policy'; status = $(if ($case -ceq 'failed-result') { 'Failed' } else { 'Passed' }); inputIdentity = 'd' * 64; outputs = @{} }) } |
                ConvertTo-Json -Depth 8 | Set-Content (Join-Path $destination 'results.json')
            @{ platform = $platform; receipt = @{ full = $true; planIdentity = $planIdentity;
                resultsIdentity = $(if ($case -ceq 'changed-certificate') { 'f' * 64 } else { (Get-FileHash (Join-Path $destination 'results.json')).Hash.ToLowerInvariant() }) } }
        }
        $certification | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $certificateRoot 'certification.json')
        [IO.Compression.ZipFile]::CreateFromDirectory($certificateRoot, (Join-Path $root 'certificate.zip'))
        [IO.Compression.ZipFile]::CreateFromDirectory($evidenceRoot, (Join-Path $root 'evidence.zip'))
        $entry = Join-Path $root 'entry.ps1'
        @'
$ErrorActionPreference = 'Stop'
$selectedOutput = $args[[Array]::IndexOf($args, '--output') + 1]
if ([Array]::IndexOf($args, '--accepted-plan') -lt 0 -or [Array]::IndexOf($args, '--evidence') -lt 0) { throw 'The current engine must compare both original inputs and evidence.' }
$current = $args[[Array]::IndexOf($args, '--plan') + 1]
@{ reusable = -not $PSScriptRoot.EndsWith('changed-environment'); planIdentity = (Get-FileHash $current).Hash.ToLowerInvariant(); acceptedPlanIdentity = (Get-FileHash $current).Hash.ToLowerInvariant(); reason = 'Explicit fixture comparison' } |
    ConvertTo-Json | Set-Content (Join-Path $selectedOutput 'acceptance.json')
'@ | Set-Content $entry
        @{ assembly = $entry; sha256 = (Get-FileHash $entry).Hash } | ConvertTo-Json | Set-Content (Join-Path $planRoot 'entry.json')
        $run = @{ id = 17; workflow_id = 29; path = '.github/workflows/engine-regression.yml'; head_sha = $revision; head_branch = 'master';
            head_repository = @{ id = $(if ($case -ceq 'foreign-repository') { 33 } else { 7 }) }; status = 'completed'; conclusion = 'success';
            event = $(if ($case -ceq 'pull-request') { 'pull_request' } else { 'schedule' }); run_attempt = 1; html_url = 'https://github.com/fixture/repo/actions/runs/17' }
        $artifacts = @()
        $names = @('engine-full-certification', 'engine-acceptance-evidence') + @($hosts | ForEach-Object { "deliverable-$($_.runner)" })
        foreach ($index in 0..($names.Count - 1)) {
            $archive = if ($index -eq 1) { 'evidence.zip' } else { 'certificate.zip' }
            $artifacts += @{ id = $index + 1; name = $names[$index] + '-attempt-1'; expired = $case -ceq 'expired'; expires_at = [DateTimeOffset]::UtcNow.AddDays(2).ToString('O');
                digest = 'sha256:' + (Get-FileHash (Join-Path $root $archive)).Hash.ToLowerInvariant();
                workflow_run = @{ id = 17; repository_id = 7; head_sha = $revision } }
        }
        $calls = @{ downloads = 0 }
        function Invoke-RestMethod {
            param($Uri, $Headers)
            if ($Uri -match '/actions/runs\?') { return @{ workflow_runs = $(if ($case -ceq 'no-prior-run') { @() } else { @($run) }) } }
            if ($Uri -match '/artifacts\?') { return @{ total_count = $artifacts.Count; artifacts = $artifacts } }
            if ($Uri -match '/actions/workflows/') { return @{ id = $(if ($case -ceq 'wrong-workflow') { 99 } else { 29 }) } }
            if ($Uri -ceq 'https://api.github.com/repos/fixture/repo') { return @{ id = 7; default_branch = 'master' } }
            throw "Unexpected read request: $Uri"
        }
        function Invoke-WebRequest {
            param($Uri, $Headers, $OutFile)
            $calls.downloads++
            Copy-Item -LiteralPath (Join-Path $root $(if ($Uri -match '/artifacts/2/zip$') { 'evidence.zip' } else { 'certificate.zip' })) -Destination $OutFile
            if ($case -ceq 'corrupt-download') { 'corruption' | Add-Content $OutFile }
        }
        $failed = $false
        try {
            & "$PSScriptRoot/Find-ValidationAcceptance.ps1" -PlanDirectory $planRoot -Platform $hosts[0].runner -Repository fixture/repo -Dotnet (Get-Command pwsh).Source
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
        if ($failed -ne ($case -cin @('corrupt-download', 'wrong-workflow', 'failed-result', 'changed-certificate'))) { throw "Unexpected acceptance discovery result: $case. See $root." }
        if (-not $failed) {
            $decision = Get-Content (Join-Path $planRoot 'acceptance-reuse.json') -Raw | ConvertFrom-Json
            if ($decision.reusable -ne ($case -ceq 'match')) { throw 'Untrusted or stale evidence suppressed execution.' }
            if ($case -cin @('pull-request', 'foreign-repository', 'expired', 'no-prior-run') -and $calls.downloads -ne 0) { throw 'Ineligible producer artifacts were downloaded.' }
        }
    }
} finally { $env:GH_TOKEN = $originalToken }
Write-Output "Passed $($cases.Count) trusted acceptance discovery contracts. Network calls are simulated. Evidence: $owner"
$global:LASTEXITCODE = 0
