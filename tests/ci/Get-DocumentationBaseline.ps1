param(
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'
if ($Commit -cnotmatch '^[a-f0-9]{40}$' -or -not $env:GH_TOKEN) {
    throw 'Documentation baseline lookup requires an exact commit and the repository read token.'
}
$prefix = 'https://api.github.com/repos/Wolfiora/InnoEngine'
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
function Read-Collection([string]$Path, [string]$Field) {
    $values = [Collections.Generic.List[object]]::new()
    for ($page = 1; ; $page++) {
        $result = Invoke-RestMethod -Uri "$prefix$($Path)?per_page=100&page=$page" -Headers $headers
        if ($null -eq $result.$Field) { throw 'Malformed documentation baseline collection.' }
        $rows = @($result.$Field)
        foreach ($row in $rows) { $values.Add($row) }
        if ($rows.Count -lt 100) { return $values.ToArray() }
    }
}
$repository = Invoke-RestMethod -Uri $prefix -Headers $headers
if ($repository.full_name -cne 'Wolfiora/InnoEngine' -or $repository.id -le 0 -or [string]::IsNullOrWhiteSpace($repository.default_branch)) {
    throw 'Documentation baseline repository identity is unavailable.'
}
$branch = [Uri]::EscapeDataString($repository.default_branch)
# A bounded exact-commit lookup may miss old evidence; that yields no compatibility, never a pass.
$runs = Invoke-RestMethod -Uri "$prefix/actions/workflows/rendering-ci.yml/runs?head_sha=$Commit&branch=$branch&status=success&per_page=100" -Headers $headers
if ($null -eq $runs.workflow_runs) { throw 'Malformed documentation baseline producer collection.' }
foreach ($run in @($runs.workflow_runs)) {
    if ($run.head_sha -cne $Commit -or $run.head_branch -cne $repository.default_branch -or $run.repository.id -ne $repository.id `
        -or $run.head_repository.id -ne $repository.id -or $run.path -cne '.github/workflows/rendering-ci.yml' `
        -or $run.event -cnotin @('push', 'schedule', 'workflow_dispatch') -or $run.status -cne 'completed' `
        -or $run.conclusion -cne 'success' -or $run.run_attempt -le 0 -or $run.id -le 0) { continue }
    $artifacts = @(Read-Collection "/actions/runs/$($run.id)/artifacts" 'artifacts')
    $certificates = @($artifacts | Where-Object name -CEQ "documentation-source-certification-attempt-$($run.run_attempt)")
    if ($certificates.Count -eq 0) { continue }
    if ($certificates.Count -ne 1) { throw 'Documentation baseline has ambiguous certificate artifacts.' }
    $artifact = $certificates[0]
    if ($artifact.expired) { continue }
    if ($artifact.id -le 0 -or $artifact.size_in_bytes -le 0 -or $artifact.size_in_bytes -gt 256MB `
        -or $artifact.digest -cnotmatch '^sha256:[a-f0-9]{64}$' -or $artifact.workflow_run.id -ne $run.id `
        -or $artifact.workflow_run.head_sha -cne $Commit -or $artifact.workflow_run.repository_id -ne $repository.id `
        -or $artifact.workflow_run.head_repository_id -ne $repository.id) { throw 'Documentation baseline artifact has invalid producer provenance.' }
    $jobs = @(Read-Collection "/actions/runs/$($run.id)/attempts/$($run.run_attempt)/jobs" 'jobs')
    $aggregate = @($jobs | Where-Object name -CEQ 'Engine validation')
    if ($aggregate.Count -ne 1 -or $aggregate[0].status -cne 'completed' -or $aggregate[0].conclusion -cne 'success') {
        throw 'Documentation baseline aggregate did not pass exactly once.'
    }
    $output = [IO.Path]::GetFullPath($OutputDirectory)
    if (Test-Path -LiteralPath $output) { throw 'Documentation baseline import must retain a distinct owned destination.' }
    [IO.Directory]::CreateDirectory($output) | Out-Null
    $archive = Join-Path $output 'source-certificate.zip'
    Invoke-WebRequest -Uri "$prefix/actions/artifacts/$($artifact.id)/zip" -Headers $headers -OutFile $archive
    $zip = [IO.Compression.ZipFile]::OpenRead($archive)
    try {
        if ($zip.Entries.Count -ne 1 -or $zip.Entries[0].FullName -cne 'source-certificate.json' `
            -or $zip.Entries[0].Length -le 0 -or $zip.Entries[0].Length -gt 16MB) {
            throw 'Documentation baseline archive exceeds its single-certificate content boundary.'
        }
    } finally { $zip.Dispose() }
    & "$PSScriptRoot/Expand-ValidationArtifact.ps1" -ArchivePath $archive -Destination (Join-Path $output 'payload') -ExpectedDigest $artifact.digest
    $files = @(Get-ChildItem -LiteralPath (Join-Path $output 'payload') -File -Recurse)
    if ($files.Count -ne 1 -or $files[0].Name -cne 'source-certificate.json' -or $files[0].DirectoryName -cne (Join-Path $output 'payload')) {
        throw 'Documentation baseline must contain exactly its root certificate.'
    }
    $certificate = Get-Content -LiteralPath $files[0].FullName -Raw | ConvertFrom-Json
    if ($certificate.format -cne 'inno-documentation-source/2' -or $certificate.sourceCommit -cne $Commit `
        -or $certificate.repository -cne 'https://github.com/Wolfiora/InnoEngine' -or $certificate.sourceTree -cnotmatch '^[a-f0-9]{40}$') {
        throw 'Documentation baseline payload differs from its authenticated producer.'
    }
    $selection = [pscustomobject]@{ status = 'Ready'; commit = $Commit; run = $run.id; attempt = $run.run_attempt
        path = $files[0].FullName; artifact = $artifact.id; digest = $artifact.digest
        origin = "https://github.com/Wolfiora/InnoEngine/actions/runs/$($run.id)/attempts/$($run.run_attempt)" }
    $selection | ConvertTo-Json -Depth 4 | Set-Content (Join-Path $output 'provenance.json') -Encoding utf8NoBOM
    return $selection
}
[pscustomobject]@{ status = 'Unavailable'; commit = $Commit; reason = 'No retained trusted default-branch certificate matches the exact impact baseline.' }
