param(
    [Parameter(Mandatory)][long]$RunId,
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][string]$Tag,
    [Parameter(Mandatory)][string]$Repository,
    [string]$ApiUrl = 'https://api.github.com',
    [string]$Output = 'artifacts/release-rehearsal',
    [switch]$VerifyOnly
)

$ErrorActionPreference = 'Stop'
if (-not $env:GH_TOKEN) { throw 'The release workflow token is unavailable.' }
if ($Tag -cnotmatch '^[A-Za-z0-9][A-Za-z0-9._-]{0,127}$') { throw 'Select an explicit release tag containing only letters, digits, dots, underscores or hyphens.' }
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
$base = "$ApiUrl/repos/$Repository"
$repositoryData = Invoke-RestMethod -Uri $base -Headers $headers
$run = Invoke-RestMethod -Uri "$base/actions/runs/$RunId" -Headers $headers
$workflow = Invoke-RestMethod -Uri "$base/actions/workflows/rendering-ci.yml" -Headers $headers
$artifacts = Invoke-RestMethod -Uri "$base/actions/runs/$RunId/artifacts?per_page=100" -Headers $headers
if ($artifacts.total_count -gt 100) { throw 'Unexpected artifact inventory size; refuse a partial provenance inventory.' }
$selected = @(& "$PSScriptRoot/Test-ReleaseSource.ps1" -Repository $repositoryData -Run $run -Artifacts $artifacts.artifacts `
    -Commit $Commit -WorkflowId $workflow.id -RunId $RunId)
$root = [IO.Path]::GetFullPath($Output)
if (Test-Path -LiteralPath $root) { throw 'Release evidence must use a new directory for each attempt.' }
New-Item -ItemType Directory -Path $root | Out-Null
foreach ($artifact in $selected) {
    $zip = Join-Path $root ($artifact.name + '.zip')
    Invoke-WebRequest -Uri "$base/actions/artifacts/$($artifact.id)/zip" -Headers $headers -OutFile $zip
    & "$PSScriptRoot/Expand-ValidationArtifact.ps1" -ArchivePath $zip -Destination (Join-Path $root $artifact.name) -ExpectedDigest $artifact.digest
}
$certificate = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $selected -Name 'engine-full-certification' -MaximumAttempt $run.run_attempt -RequireCurrentAttempt
$certification = @(Get-Content -LiteralPath (Join-Path $root ($certificate.name + '/certification.json')) -Raw | ConvertFrom-Json)
if ($certification.Count -ne 2) { throw 'Both hosted platform certifications are required.' }
$uploads = foreach ($platform in @('windows-2022', 'macos-15')) {
    $receipt = @($certification | Where-Object platform -CEQ $platform)
    if ($receipt.Count -ne 1) { throw "Missing or duplicate platform certification: $platform" }
    $bundle = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $selected -Name "deliverable-$platform" -MaximumAttempt $run.run_attempt
    & "$PSScriptRoot/Test-ReleaseBundle.ps1" -Directory (Join-Path $root $bundle.name) -Commit $Commit -Platform $platform -Receipt $receipt[0].receipt
}
$report = [pscustomobject]@{ repository = $Repository; runId = $RunId; commit = $Commit; tag = $Tag; artifacts = $uploads; verifiedAt = [DateTimeOffset]::UtcNow }
$report | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $root 'verified-release.json')
if ($VerifyOnly) { Write-Host 'Release rehearsal verified all source and artifact identities; no release was created.'; return }
# Complete all provenance and byte checks before the first remote write. Existing releases are never modified.
try {
    $null = Invoke-RestMethod -Uri "$base/releases/tags/$Tag" -Headers $headers
    throw 'A release already exists for the requested tag.'
} catch {
    if ($_.Exception.Response.StatusCode -ne [Net.HttpStatusCode]::NotFound) { throw }
}
try {
    $null = Invoke-RestMethod -Uri "$base/git/ref/tags/$Tag" -Headers $headers
    $tagCommit = Invoke-RestMethod -Uri "$base/commits/$Tag" -Headers $headers
    if ($tagCommit.sha -cne $Commit) { throw 'The existing tag points to another source commit.' }
} catch {
    if ($_.Exception.Response.StatusCode -ne [Net.HttpStatusCode]::NotFound) { throw }
}
$body = @{ tag_name = $Tag; target_commitish = $Commit; name = $Tag; draft = $true; prerelease = $false;
    body = "Validated commit: $Commit`n`nSource run: $($run.html_url)`n`nDraft only. Assets are the exact verified Editor and Player Support Pack archives; this workflow does not rebuild or publish the release." }
$release = Invoke-RestMethod -Method Post -Uri "$base/releases" -Headers $headers -ContentType 'application/json' -Body ($body | ConvertTo-Json)
$uploadUrl = $release.upload_url -replace '\{.*$', ''
foreach ($upload in $uploads) {
    if ((Get-FileHash -LiteralPath $upload.path -Algorithm SHA256).Hash.ToLowerInvariant() -cne $upload.sha256) { throw 'A verified upload changed before transmission.' }
    $asset = Invoke-RestMethod -Method Post -Uri ($uploadUrl + '?name=' + [Uri]::EscapeDataString($upload.name)) `
        -Headers $headers -ContentType 'application/zip' -InFile $upload.path
    if ($asset.state -cne 'uploaded' -or $asset.digest -cne ('sha256:' + $upload.sha256)) {
        throw 'The draft asset server digest differs from the verified upload; retain the draft for diagnosis.'
    }
}
Write-Host "Created draft release: $($release.html_url)"
"Draft release: $($release.html_url)" >> $env:GITHUB_STEP_SUMMARY
