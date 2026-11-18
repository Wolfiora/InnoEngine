param(
    [Parameter(Mandatory)][string]$Directory,
    [Parameter(Mandatory)][string]$Repository,
    [Parameter(Mandatory)][string]$Revision,
    [string]$ApiUrl = 'https://api.github.com',
    [switch]$DryRun
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. "$PSScriptRoot/BackendReleaseCatalog.ps1"
if ($Repository -cnotmatch '^[a-zA-Z0-9_.-]+/[a-zA-Z0-9_.-]+$' -or $Revision -cnotmatch '^[a-f0-9]{40}$' -or !$env:GH_TOKEN) {
    throw 'Publication requires a repository, exact certified source and release credential.'
}
$base = "$ApiUrl/repos/$Repository"
$headers = @{ Authorization="Bearer $env:GH_TOKEN"; Accept='application/vnd.github+json'; 'X-GitHub-Api-Version'='2022-11-28' }
$targets = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$digests = @{}
$archives = @{}
foreach ($platform in $targets) {
    $owner = Join-Path $Directory $platform.target
    $archive = Join-Path $owner $platform.asset
    $planPath = Join-Path $owner 'plan.json'
    $resultsPath = Join-Path $owner 'results.json'
    $receipt = & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath $planPath -ResultsPath $resultsPath -ExpectedCommit $Revision -JobConclusion success
    $plan = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json
    $bound = @($plan.backendPackages | Where-Object target -CEQ $platform.target)
    $digest = (Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant()
    if (!$receipt.full -or $platform.target -cnotin @($plan.nativeRequests.target) -or $bound.Count -ne 1 -or $bound[0].sha256 -cne $digest -or $digests.ContainsKey($platform.asset)) {
        throw "No matching certified platform ZIP for $($platform.target)."
    }
    $digests.Add($platform.asset, $digest)
    $archives.Add($platform.asset, (Get-Item -LiteralPath $archive).FullName)
}
function Get-Releases {
    for ($page = 1; ; $page++) {
        $rows = @(Invoke-RestMethod -Uri "$base/releases?per_page=100&page=$page" -Headers $headers | Write-Output)
        $rows
        if ($rows.Count -lt 100) { break }
    }
}
function Get-OtherReaders {
    foreach ($status in 'queued','in_progress','requested','waiting','pending') {
        $runs = Invoke-RestMethod -Headers $headers -Uri "$base/actions/runs?status=$status&per_page=100"
        if ([int]$runs.total_count -gt 100) { throw 'Too many active runs to certify backend release retirement.' }
        foreach ($run in $runs.workflow_runs) {
            if ([string]$run.id -cne [string]$env:GITHUB_RUN_ID -and
                $run.path -cmatch '^\.github/workflows/(engine-ci|engine-regression|engine-performance|draft-release|backend-prebuilts)\.yml') {
                $run.id
            }
        }
    }
}
$releases = @(Get-Releases)
$publication = & "$PSScriptRoot/Get-BackendReleasePlan.ps1" -Releases $releases -Digests $digests -Revision $Revision
if ($publication.operation -ceq 'reuse') {
    $published = $releases | Where-Object id -EQ $publication.id
    $null = Get-BackendCatalog $base $headers $published
    Write-Output "Reused $($publication.tag)"
    return
}
if ($publication.retire) {
    $readers = @(Get-OtherReaders)
    if ($readers.Count) { throw "Release $($publication.retire.tag) is protected while runs $($readers -join ', ') can acquire its files." }
}
if ($DryRun) { return $publication }
if ($publication.operation -ceq 'resume') {
    $draft = $releases | Where-Object id -EQ $publication.id
} else {
    $body = @{tag_name=$publication.tag; target_commitish=$Revision; name=$publication.title; draft=$true; prerelease=$true
        body="Certified backend assemblies, native libraries and build tools for engine CI. Each platform ZIP includes its file inventory. Source revision: $Revision." }
    $draft = Invoke-RestMethod -Method Post -Uri "$base/releases" -Headers $headers -ContentType application/json -Body ($body | ConvertTo-Json)
}
if (!$draft.draft -or !$draft.prerelease -or $draft.tag_name -cne $publication.tag -or $draft.target_commitish -cne $Revision) {
    throw 'The server returned a different draft or source revision.'
}
$upload = ([string]$draft.upload_url).Split('{')[0]
foreach ($name in $archives.Keys | Sort-Object) {
    if (@($draft.assets | Where-Object name -CEQ $name).Count) { continue }
    $uploaded = Invoke-RestMethod -Method Post -Uri "${upload}?name=$name" -Headers $headers -ContentType application/octet-stream -InFile $archives[$name]
    if ($uploaded.name -cne $name -or $uploaded.digest -cne ('sha256:' + $digests[$name]) -or $uploaded.state -cne 'uploaded') {
        throw "GitHub failed to verify $name. Retained draft $($draft.id)."
    }
}
$ready = Invoke-RestMethod -Uri "$base/releases/$($draft.id)" -Headers $headers
if (!$ready.draft -or !$ready.prerelease -or $ready.tag_name -cne $publication.tag -or $ready.target_commitish -cne $Revision -or
    @($ready.assets).Count -ne $digests.Count -or @($ready.assets.name | Sort-Object -Unique).Count -ne $digests.Count -or
    @($ready.assets | Where-Object { $_.state -cne 'uploaded' -or $_.digest -cne ('sha256:' + $digests[$_.name]) }).Count) {
    throw 'The complete uploaded draft differs from the certified platform matrix.'
}
# Keep discovery metadata on the immutable tag. No branch commit or fourth release asset is needed.
$platforms = [ordered]@{}
foreach ($target in $targets) {
    $platforms[$target.target] = Get-BackendCatalogPlatform $archives[$target.asset] $target.asset $target.target $digests[$target.asset]
}
$message = ConvertTo-BackendTagMessage ([ordered]@{version=1; revision=$Revision; platforms=$platforms})
$tagName = $publication.tag
$catalogRef = $null
try { $catalogRef = Invoke-RestMethod -Uri "$base/git/ref/tags/$tagName" -Headers $headers }
catch { if ($_.Exception.Response.StatusCode -ne [Net.HttpStatusCode]::NotFound) { throw } }
if ($catalogRef) {
    if ($catalogRef.object.type -cne 'tag') { throw "Unexpected existing tag $tagName." }
    $existing = Invoke-RestMethod -Uri "$base/git/tags/$($catalogRef.object.sha)" -Headers $headers
    if ($existing.object.type -cne 'commit' -or $existing.object.sha -cne $Revision -or $existing.message.TrimEnd() -cne $message) {
        throw 'The pending tag no longer describes the same certified platform archives.'
    }
} else {
    $object = Invoke-RestMethod -Method Post -Uri "$base/git/tags" -Headers $headers -ContentType application/json -Body (
        @{tag=$tagName; message=$message; object=$Revision; type='commit'} | ConvertTo-Json -Compress)
    if ($object.object.sha -cne $Revision -or $object.sha -cnotmatch '^[a-f0-9]{40}$') { throw 'GitHub returned a different tag object.' }
    $null = Invoke-RestMethod -Method Post -Uri "$base/git/refs" -Headers $headers -ContentType application/json -Body (
        @{ref="refs/tags/$tagName"; sha=$object.sha} | ConvertTo-Json -Compress)
}
if ($publication.retire) {
    $readers = @(Get-OtherReaders)
    if ($readers.Count) { throw "Retained draft $($draft.id) while runs $($readers -join ', ') can acquire the rollback release." }
    $old = Invoke-RestMethod -Uri "$base/releases/$($publication.retire.id)" -Headers $headers
    $tag = $publication.retire.tag
    $previousCatalog = Get-BackendCatalog $base $headers $old
    if ($old.tag_name -cne $tag -or $old.draft -or !$old.prerelease -or $old.target_commitish -cnotmatch '^[0-9a-f]{40}$' -or
        $previousCatalog.revision -cne $old.target_commitish) { throw "Release or dedicated tag ownership changed for $tag." }
    Invoke-RestMethod -Method Delete -Uri "$base/releases/$($old.id)" -Headers $headers | Out-Null
    Invoke-RestMethod -Method Delete -Uri "$base/git/refs/tags/$tag" -Headers $headers | Out-Null
}
$published = Invoke-RestMethod -Method Patch -Uri "$base/releases/$($draft.id)" -Headers $headers -ContentType application/json -Body '{"draft":false,"prerelease":true,"make_latest":"false"}'
if ($published.draft -or !$published.prerelease -or $published.tag_name -cne $publication.tag) { throw 'The backend release was not published as certified.' }
Write-Output "Published $($published.name)."
