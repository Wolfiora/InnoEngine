$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/native-publication-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @('publish', 'corrupt-archive', 'wrong-revision', 'wrong-acceptance-revision', 'failed-acceptance', 'changed-evidence', 'missing-certification', 'duplicate-certification', 'existing-release', 'wrong-upload-digest', 'denied-release', 'resume-draft', 'resume-wrong-revision', 'resume-corrupt-asset', 'resume-published')
$originalToken = $env:GH_TOKEN
$originalSummary = $env:GITHUB_STEP_SUMMARY
try {
    $env:GH_TOKEN = 'fixture-token'
    foreach ($case in $cases) {
        $root = Join-Path $owner $case
        $package = Join-Path $root 'native-dependency-fixture'
        $evidence = Join-Path $root 'native-certification-fixture/fixture'
        New-Item -ItemType Directory -Path $package, $evidence -Force | Out-Null
        $env:GITHUB_STEP_SUMMARY = Join-Path $root 'summary.md'
        $revision = 'a' * 40
        $release = 'native-deps-fixture-1'
        $archive = Join-Path $package 'fixture.zip'
        'fixture archive bytes' | Set-Content $archive
        $asset = @{ product = 'editor'; component = 'fixture'; target = 'fixture'; configuration = 'debug'; options = @()
            release = $release; asset = 'fixture.zip'; sha256 = (Get-FileHash $archive).Hash.ToLowerInvariant(); fingerprint = 'b' * 64; producerRevision = $revision }
        @{ assets = @($asset); aggregates = @() } | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $package 'native-artifacts.json')
        $candidate = '9' * 40
        $plan = @{ commit = $(if ($case -ceq 'wrong-acceptance-revision') { $revision } else { $candidate }); tree = 'c' * 64; environment = 'd' * 64; full = $true
            checks = @(@{ id = 'producer-contract'; kind = 'instructions'; exclusion = $null; prerequisites = @() }) }
        $plan | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $evidence 'plan.json')
        $planIdentity = (Get-FileHash (Join-Path $evidence 'plan.json')).Hash.ToLowerInvariant()
        @{ commit = $plan.commit; tree = $plan.tree; environment = $plan.environment; completed = $true; planIdentity = $planIdentity
            results = @(@{ id = 'producer-contract'; status = $(if ($case -ceq 'failed-acceptance') { 'Failed' } else { 'Passed' }); inputIdentity = 'e' * 64; outputs = @{} }) } |
            ConvertTo-Json -Depth 8 | Set-Content (Join-Path $evidence 'results.json')
        @{ commit = $(if ($case -ceq 'wrong-revision') { 'f' * 40 } else { $revision }); target = 'fixture'; release = $release; completed = $true
            selectionDigest = (Get-FileHash (Join-Path $package 'native-artifacts.json')).Hash.ToLowerInvariant(); planIdentity = $planIdentity
            evidenceDigest = (Get-FileHash (Join-Path $evidence 'results.json')).Hash.ToLowerInvariant() } |
            ConvertTo-Json | Set-Content (Join-Path $evidence 'native-producer.json')
        if ($case -ceq 'corrupt-archive') { 'corrupt' | Add-Content $archive }
        if ($case -ceq 'changed-evidence') { ' ' | Add-Content (Join-Path $evidence 'results.json') }
        if ($case -ceq 'missing-certification') { Remove-Item -LiteralPath (Join-Path $evidence 'native-producer.json') }
        if ($case -ceq 'duplicate-certification') {
            $duplicate = Join-Path $root 'native-certification-other/fixture'
            New-Item -ItemType Directory -Path $duplicate -Force | Out-Null
            Copy-Item -LiteralPath (Join-Path $evidence 'native-producer.json') -Destination $duplicate
        }
        $calls = @{ writes = 0; published = $false }
        function Invoke-RestMethod {
            param($Uri, $Headers, $Method = 'Get', $ContentType, $Body, $InFile)
            if ($Method -ceq 'Get') {
                if ($case.StartsWith('resume-')) {
                    if ($Uri -like '*/assets?*') {
                        return @(@{ state = 'uploaded'; name = [IO.Path]::GetFileName($archive); digest = $(if ($case -ceq 'resume-corrupt-asset') { 'sha256:wrong' } else { 'sha256:' + (Get-FileHash $archive).Hash.ToLowerInvariant() }) })
                    }
                    return @{ id = 12; draft = $case -cne 'resume-published'; prerelease = $true; tag_name = $release
                        target_commitish = $(if ($case -ceq 'resume-wrong-revision') { '0' * 40 } else { $revision }); upload_url = 'https://uploads.github.com/fixture{?name}' }
                }
                if ($case -ceq 'existing-release') { return @{ id = 99 } }
                throw [Microsoft.PowerShell.Commands.HttpResponseException]::new('Fixture not found', [Net.Http.HttpResponseMessage]::new([Net.HttpStatusCode]::NotFound))
            }
            if ($case -ceq 'denied-release') {
                throw [Microsoft.PowerShell.Commands.HttpResponseException]::new('Release permission denied', [Net.Http.HttpResponseMessage]::new([Net.HttpStatusCode]::Forbidden))
            }
            $calls.writes++
            if ($InFile) {
                return @{ name = [IO.Path]::GetFileName($InFile); state = 'uploaded'; digest = $(if ($case -ceq 'wrong-upload-digest') { 'sha256:' + ('0' * 64) } else { 'sha256:' + (Get-FileHash $InFile).Hash.ToLowerInvariant() }) }
            }
            if ($Method -ceq 'Patch') { $calls.published = $true; return @{} }
            if (-not ($Body | ConvertFrom-Json).draft) { throw 'An incomplete release became consumable.' }
            return @{ id = 12; upload_url = 'https://uploads.github.com/fixture{?name}' }
        }
        $failed = $false
        $resume = if ($case.StartsWith('resume-')) { @{ DraftReleaseId = 12 } } else { @{} }
        try {
            & "$PSScriptRoot/Publish-NativeDependencies.ps1" -Directory $root -Repository fixture/repository -ProducerRevision $revision -AcceptanceRevision $candidate -Release $release -Targets fixture @resume
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
        if ($failed -ne ($case -cnotin @('publish','resume-draft')) -or $calls.published -ne ($case -cin @('publish','resume-draft'))) { throw "Unexpected native publication result: $case. See $root." }
        if ($case -ceq 'denied-release' -and (Get-Content (Join-Path $root 'failure.log') -Raw) -notmatch 'resume publication') { throw 'Denied release lost the retained-artifact recovery instructions.' }
        if ($case -ceq 'resume-draft' -and $calls.writes -ne 2) { throw 'A resumed upload rewrote an accepted archive or skipped the selector.' }
        if ($case -cnotin @('publish', 'resume-draft', 'wrong-upload-digest') -and $calls.writes -ne 0) { throw 'Invalid provenance caused a remote write.' }
    }
} finally {
    $env:GH_TOKEN = $originalToken
    $env:GITHUB_STEP_SUMMARY = $originalSummary
}
Write-Output "Passed $($cases.Count) native publication contracts. Network calls are simulated; no release was created. Evidence: $owner"
$global:LASTEXITCODE = 0
