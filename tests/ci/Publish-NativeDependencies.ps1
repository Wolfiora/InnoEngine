param(
    [Parameter(Mandatory)][string]$Directory,
    [Parameter(Mandatory)][string]$Repository,
    [Parameter(Mandatory)][string]$ProducerRevision,
    [Parameter(Mandatory)][string]$Release,
    [Parameter(Mandatory)][string[]]$Targets,
    [string]$ApiUrl = 'https://api.github.com',
    [string]$AcceptanceRevision = $ProducerRevision,
    [ValidateRange(1, [long]::MaxValue)][long]$DraftReleaseId
)

$ErrorActionPreference = 'Stop'
if ($ProducerRevision -cnotmatch '^[a-f0-9]{40}$' -or $Release -cnotmatch '^native-deps-[a-zA-Z0-9._-]+$' `
    -or $Repository -cnotmatch '^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$' -or -not $env:GH_TOKEN) { throw 'Dependency publication lacks exact authorized provenance.' }
if ($Targets.Count -eq 0 -or @($Targets | Sort-Object -Unique).Count -ne $Targets.Count -or
    @($Targets | Where-Object { $_ -cnotmatch '^[a-z0-9][a-z0-9-]*$' }).Count -ne 0) { throw 'The producer target matrix is empty, invalid or duplicated.' }
$root = [IO.Path]::GetFullPath($Directory)
$fragments = @()
$archives = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
foreach ($target in $Targets) {
    $owner = Join-Path $root "native-dependency-$target"
    $manifest = Join-Path $owner 'native-artifacts.json'
    $certificates = @(Get-ChildItem -LiteralPath $root -Directory -Filter 'native-certification-*' |
        ForEach-Object { Join-Path $_.FullName $target } | Where-Object { Test-Path (Join-Path $_ 'native-producer.json') })
    if ($certificates.Count -ne 1) { throw "The produced target requires exactly one host certification: $target" }
    $evidenceDirectory = $certificates[0]
    $receipt = Get-Content -LiteralPath (Join-Path $evidenceDirectory 'native-producer.json') -Raw | ConvertFrom-Json
    if ($receipt.completed -isnot [bool] -or -not $receipt.completed -or $receipt.commit -cne $ProducerRevision `
        -or $receipt.target -cne $target -or $receipt.release -cne $Release -or $receipt.planIdentity -cnotmatch '^[a-f0-9]{64}$' `
        -or $receipt.evidenceDigest -cnotmatch '^[a-f0-9]{64}$' -or (Get-FileHash -LiteralPath $manifest).Hash.ToLowerInvariant() -cne $receipt.selectionDigest) {
        throw "Dependency producer acceptance is absent or mismatched: $target."
    }
    $selection = Get-Content -LiteralPath $manifest -Raw | ConvertFrom-Json
    $resultsPath = Join-Path $evidenceDirectory 'results.json'
    if ((Get-FileHash -LiteralPath $resultsPath).Hash.ToLowerInvariant() -cne $receipt.evidenceDigest) {
        throw 'The accepted producer evidence was modified or is absent.'
    }
    $certification = & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath (Join-Path $evidenceDirectory 'plan.json') `
        -ResultsPath $resultsPath -ExpectedCommit $AcceptanceRevision -JobConclusion success
    if (-not $certification.full -or $certification.planIdentity -cne $receipt.planIdentity) { throw 'The dependency producer lacks complete functional acceptance.' }
    foreach ($asset in @($selection.assets) + @($selection.aggregates)) {
        if ($asset.release -cne $Release -or $asset.producerRevision -cne $ProducerRevision -or $asset.target -cne $target `
            -or $asset.asset -cnotmatch '^[A-Za-z0-9._-]+\.zip$' -or $asset.sha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'A produced archive belongs to another selection.' }
        $archive = Join-Path $owner $asset.asset
        if ((Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant() -cne $asset.sha256) { throw 'A produced archive was modified after acceptance.' }
        if ($archives.ContainsKey($asset.asset) -and $archives[$asset.asset].digest -cne $asset.sha256) { throw 'Two producers published conflicting bytes under one asset name.' }
        $archives[$asset.asset] = @{ path = $archive; digest = $asset.sha256 }
    }
    $fragments += $manifest
}
if ($archives.Count -eq 0) { throw 'The native publication has no complete dependency archives.' }
$selectionPath = Join-Path $root 'native-artifacts.json'
& "$PSScriptRoot/Merge-NativeDependencySelection.ps1" -Fragments $fragments -BaselinePath build/dependencies/native-artifacts.json `
    -OutputPath $selectionPath -ProducerRevision $ProducerRevision -Release $Release
# All receipts and bytes are verified before the first remote write. Interrupted drafts are never consumable.
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
$base = "$ApiUrl/repos/$Repository"
$existing = @{}
if ($PSBoundParameters.ContainsKey('DraftReleaseId')) {
    $created = Invoke-RestMethod -Uri "$base/releases/$DraftReleaseId" -Headers $headers
    if ($created.id -ne $DraftReleaseId -or $created.draft -ne $true -or $created.prerelease -ne $true -or
        $created.tag_name -cne $Release -or $created.target_commitish -cne $ProducerRevision) {
        throw 'Only the exact unpublished producer draft can resume.'
    }
    $expected = @{'native-artifacts.json' = (Get-FileHash -LiteralPath $selectionPath).Hash.ToLowerInvariant()}
    foreach ($entry in $archives.GetEnumerator()) { $expected[$entry.Key] = $entry.Value.digest }
    for ($page = 1; ; $page++) {
        $batch = @(Invoke-RestMethod -Uri "$base/releases/$DraftReleaseId/assets?per_page=100&page=$page" -Headers $headers | Write-Output)
        foreach ($asset in $batch) {
            if ($asset.state -cne 'uploaded' -or $asset.name -cnotin $expected.Keys -or $existing.ContainsKey($asset.name) -or
                $asset.digest -cne ('sha256:' + $expected[$asset.name])) { throw 'Existing draft assets differ from accepted dependency bytes.' }
            $existing[$asset.name] = $asset.digest
        }
        if ($batch.Count -lt 100) { break }
    }
} else {
    try {
        $null = Invoke-RestMethod -Uri "$base/releases/tags/$Release" -Headers $headers
        throw 'The dependency release already exists; published identities cannot be overwritten.'
    } catch {
        if ($_.Exception.Response.StatusCode -ne [Net.HttpStatusCode]::NotFound) { throw }
    }
    $body = @{ tag_name = $Release; target_commitish = $ProducerRevision; name = $Release; draft = $true; prerelease = $true
        body = 'Verified native dependency assets. Adopt native-artifacts.json explicitly; ordinary engine CI never rebuilds these dependencies.' } | ConvertTo-Json
    try {
        $created = Invoke-RestMethod -Method Post -Uri "$base/releases" -Headers $headers -ContentType 'application/json' -Body $body
    } catch [Microsoft.PowerShell.Commands.HttpResponseException] {
        if ($_.Exception.Response.StatusCode -ne [Net.HttpStatusCode]::Forbidden) { throw }
        $reason = "GitHub denied creation of release '$Release' at '$ProducerRevision'. Check release permissions. A producer that changes workflows relative to the default branch requires a credential authorized to modify workflows."
        $recovery = 'To resume publication, download the retained native-dependency and native-certification artifacts into their named directories and rerun Publish-NativeDependencies.ps1 with the same revisions, targets and release. The publisher verifies the original receipts and archive hashes before uploading.'
        throw [InvalidOperationException]::new("$reason $recovery", $_.Exception)
    }
    Write-Host "Created dependency draft $($created.id). A later publication attempt can resume with -DraftReleaseId $($created.id)."
}
$upload = ([string]$created.upload_url).Split('{')[0]
$files = @($archives.Values | ForEach-Object { $_.path }) + @($selectionPath)
foreach ($file in $files) {
    if ($existing.ContainsKey([IO.Path]::GetFileName($file))) { continue }
    $name = [Uri]::EscapeDataString([IO.Path]::GetFileName($file))
    $uploaded = Invoke-RestMethod -Method Post -Uri "${upload}?name=$name" -Headers $headers -ContentType 'application/octet-stream' -InFile $file
    $expectedDigest = 'sha256:' + (Get-FileHash -LiteralPath $file).Hash.ToLowerInvariant()
    if ($uploaded.state -cne 'uploaded' -or $uploaded.name -cne [IO.Path]::GetFileName($file) -or $uploaded.digest -cne $expectedDigest) {
        throw 'The uploaded release asset does not match the accepted bytes; the release remains a draft.'
    }
}
$null = Invoke-RestMethod -Method Patch -Uri "$base/releases/$($created.id)" -Headers $headers -ContentType 'application/json' -Body '{"draft":false}'
"Published dependency release: $Release. Adopt its native-artifacts.json in the intended branch." >> $env:GITHUB_STEP_SUMMARY
