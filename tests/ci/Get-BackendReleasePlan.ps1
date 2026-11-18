param(
    [Parameter(Mandatory)][AllowEmptyCollection()][object[]]$Releases,
    [Parameter(Mandatory)][hashtable]$Digests,
    [Parameter(Mandatory)][string]$Revision,
    [string[]]$ActiveTags = @(),
    [datetime]$UtcDate = [datetime]::UtcNow
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$registry = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
if ($Revision -cnotmatch '^[a-f0-9]{40}$' -or !$registry.Count -or $Digests.Count -ne $registry.Count -or
    @($registry.asset | Sort-Object -Unique).Count -ne $registry.Count) { throw 'The backend release needs an exact source revision and platform matrix.' }
foreach ($asset in $registry.asset) {
    if ($asset -cnotmatch '^[a-z0-9]+-prebuilt-backends\.zip$' -or $Digests[$asset] -cnotmatch '^[0-9a-f]{64}$') {
        throw "Incomplete platform ZIP certification for $asset."
    }
}
$pattern = '^backend-prebuilts-(\d{4}\.\d{2}\.\d{2})\.([1-9]\d*)$'
$owned = @($Releases | Where-Object { $_.tag_name -cmatch $pattern })
if (@($owned | Where-Object { !$_.prerelease }).Count -or @($owned | ForEach-Object tag_name | Sort-Object -Unique).Count -ne $owned.Count) {
    throw 'A backend cache tag has conflicting release ownership.'
}
$published = @($owned | Where-Object { !$_.draft } | Sort-Object -Property published_at,id -Descending)
$drafts = @($owned | Where-Object draft)
if ($published.Count -gt 3 -or $drafts.Count -gt 1) { throw 'Resolve the existing backend release inventory before creating another version.' }
foreach ($release in $owned) {
    $seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($asset in $release.assets) {
        if (!$seen.Add($asset.name) -or $asset.name -cnotmatch '^[a-z0-9]+-prebuilt-backends\.zip$' -or ($release.draft -and $asset.name -cnotin $registry.asset) -or $asset.state -cne 'uploaded' -or $asset.digest -cnotmatch '^sha256:[0-9a-f]{64}$') {
            throw "Unexpected or incomplete asset on $($release.tag_name)."
        }
    }
    if (!$release.draft -and !$seen.Count) { throw "Published backend release $($release.tag_name) is incomplete." }
}
foreach ($release in $published) {
    if ($release.assets.Count -eq $Digests.Count -and !@($release.assets | Where-Object { $_.digest -cne ('sha256:' + $Digests[$_.name]) }).Count) {
        return [pscustomobject]@{ operation='reuse'; tag=$release.tag_name; id=$release.id; retire=$null }
    }
}
$date = $UtcDate.ToUniversalTime().ToString('yyyy.MM.dd', [Globalization.CultureInfo]::InvariantCulture)
$version = 1
foreach ($release in $owned) {
    if ($release.tag_name -cmatch $pattern -and $Matches[1] -ceq $date) {
        $version = [Math]::Max($version, [int]$Matches[2] + 1)
    }
}
$tag = "backend-prebuilts-$date.$version"
$operation = 'create'
$id = $null
if ($drafts.Count) {
    $draft = $drafts[0]
    if ($draft.target_commitish -cne $Revision -or @($draft.assets | Where-Object { $_.digest -cne ('sha256:' + $Digests[$_.name]) }).Count) {
        throw "An interrupted backend draft belongs to a different producer or asset set: $($draft.tag_name)."
    }
    $operation = 'resume'
    $tag = $draft.tag_name
    $id = $draft.id
}
$retire = $null
if ($published.Count -eq 3) {
    $oldest = $published[-1]
    if ($oldest.tag_name -cin $ActiveTags) { throw "Backend release $($oldest.tag_name) still has a reader or rollback reference." }
    $retire = [pscustomobject]@{ id=$oldest.id; tag=$oldest.tag_name }
}
[pscustomobject]@{ operation=$operation; tag=$tag; title=('Backend Prebuilts ' + $tag.Substring('backend-prebuilts-'.Length)); id=$id; retire=$retire }
