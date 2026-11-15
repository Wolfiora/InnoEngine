param([string] $Output = 'artifacts/documentation-export/preflight.json')
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$root = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
function Git([string[]] $Arguments) {
    $value = @(& git -C $root @Arguments)
    if ($LASTEXITCODE -ne 0) { throw 'Cannot establish API export inputs.' }
    return $value
}
function Inputs([string] $Revision) {
    $entries = [ordered]@{}
    foreach ($line in (Git @('ls-tree', '-r', $Revision))) {
        if ($line -cnotmatch '^([0-9]+) (blob|commit) ([a-f0-9]{40})\t(.+)$') { throw 'Invalid Git input entry.' }
        $file = $Matches[4]; $identity = $Matches[3]
        # Only known non-executable repository guidance is excluded. Unknown inputs,
        # submodule identities and all build/source/tool additions remain conservative.
        if ($file -cmatch '^(README\.md|CONTRIBUTING\.md|LICENSE|\.agents/records/.*\.(md|png|jpg|json|csv|trx|log))$') { continue }
        $entries[$file] = $identity
    }
    if ($entries.Count -eq 0) { throw 'API input discovery is unexpectedly empty.' }
    return $entries
}
$revision = (Git @('rev-parse', 'HEAD')) -join ''
$current = Inputs $revision
$baseline = $null
if ($env:GH_TOKEN) {
    $headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
    $runs = Invoke-RestMethod 'https://api.github.com/repos/Wolfiora/InnoEngine/actions/workflows/documentation-api.yml/runs?branch=master&event=push&status=success&per_page=20' -Headers $headers
    foreach ($run in $runs.workflow_runs) {
        if ($run.id -eq $env:GITHUB_RUN_ID -or $run.head_repository.full_name -cne 'Wolfiora/InnoEngine' -or $run.path -cne '.github/workflows/documentation-api.yml') { continue }
        $jobs = Invoke-RestMethod "https://api.github.com/repos/Wolfiora/InnoEngine/actions/runs/$($run.id)/attempts/$($run.run_attempt)/jobs?per_page=100" -Headers $headers
        if (@($jobs.jobs | Where-Object { $_.name -ceq 'API export validation' -and $_.conclusion -ceq 'success' }).Count -ne 1) { continue }
        # Require an actual complete producer package, not an earlier no-change preflight.
        $artifacts = Invoke-RestMethod "https://api.github.com/repos/Wolfiora/InnoEngine/actions/runs/$($run.id)/artifacts?per_page=100" -Headers $headers
        if (@($artifacts.artifacts | Where-Object { $_.name -ceq "documentation-api-package-$($run.run_attempt)" -and !$_.expired -and $_.digest -cmatch '^sha256:[a-f0-9]{64}$' }).Count -ne 1) { continue }
        $baseline = $run.head_sha
        [void](Git @('fetch', '--no-tags', 'origin', $baseline))
        break
    }
}
$changed = @()
if ($baseline) {
    $previous = Inputs $baseline
    $changed = @(@($current.Keys) + @($previous.Keys) | Sort-Object -Unique | Where-Object { $current[$_] -cne $previous[$_] })
}
$required = !$baseline -or $changed.Count -ne 0
$result = @{ schema = 'inno-api-preflight/1'; revision = $revision; baseline = $baseline; inputs = $current;
    status = $(if ($required) { 'EvaluationRequired' } else { 'NoRelevantChanges' }); changed = $changed;
    reason = $(if (!$baseline) { 'No trusted complete export baseline; evaluate the actual graph.' } elseif ($required) { 'Input files or membership changed; evaluate owner/context dependencies.' } else { 'Complete conservative input manifest matches the trusted export.' }) }
$destination = [IO.Path]::GetFullPath($Output, $root)
[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($destination)) | Out-Null
[IO.File]::WriteAllText($destination, ($result | ConvertTo-Json -Depth 10), [Text.UTF8Encoding]::new($false))
if ($env:GITHUB_OUTPUT) { "required=$($required.ToString().ToLowerInvariant())" >> $env:GITHUB_OUTPUT }
Write-Output "$($result.status): $($result.reason)"
