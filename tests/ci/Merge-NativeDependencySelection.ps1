param(
    [Parameter(Mandatory)][string[]]$Fragments,
    [Parameter(Mandatory)][string]$OutputPath,
    [string]$BaselinePath,
    [Parameter(Mandatory)][string]$ProducerRevision,
    [Parameter(Mandatory)][string]$Release
)

$ErrorActionPreference = 'Stop'
if ($ProducerRevision -cnotmatch '^[a-f0-9]{40}$' -or $Release -cnotmatch '^native-deps-[a-zA-Z0-9._-]+$') { throw 'Dependency selection requires exact producer provenance.' }
$families = @{ assets = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal); aggregates = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal) }
$newKeys = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$documents = @()
if ($BaselinePath -and (Test-Path -LiteralPath $BaselinePath)) { $documents += @{ path = $BaselinePath; baseline = $true } }
$documents += @($Fragments | Sort-Object -Unique | ForEach-Object { @{ path = $_; baseline = $false } })
if ($Fragments.Count -eq 0) { throw 'Dependency publication has no produced selections.' }
foreach ($document in $documents) {
    $selection = Get-Content -LiteralPath $document.path -Raw | ConvertFrom-Json
    foreach ($family in @('assets', 'aggregates')) {
        if ($null -eq $selection.$family) { throw 'Dependency selection must explicitly declare both inventories.' }
        foreach ($asset in @($selection.$family)) {
            if ($asset.sha256 -cnotmatch '^[a-f0-9]{64}$' -or $asset.fingerprint -cnotmatch '^[a-f0-9]{64}$' `
                -or $asset.producerRevision -cnotmatch '^[a-f0-9]{40}$' -or [string]::IsNullOrWhiteSpace($asset.product) `
                -or [string]::IsNullOrWhiteSpace($asset.target) -or [string]::IsNullOrWhiteSpace($asset.configuration) `
                -or $asset.asset -cnotmatch '^[A-Za-z0-9._-]+\.zip$') { throw 'A dependency fragment lacks complete immutable provenance.' }
            if (-not $document.baseline -and ($asset.producerRevision -cne $ProducerRevision -or $asset.release -cne $Release)) {
                throw 'A produced dependency fragment belongs to another source revision or release.'
            }
            $key = @($asset.product, $asset.component, $asset.target, $asset.configuration, $asset.options) | ConvertTo-Json -Compress
            $owner = "$family/$key"
            if (-not $document.baseline -and -not $newKeys.Add($owner)) {
                if (($families[$family][$key] | ConvertTo-Json -Depth 50 -Compress) -cne ($asset | ConvertTo-Json -Depth 50 -Compress)) {
                    throw 'Two producers selected incompatible artifacts for the same product component.'
                }
            }
            $families[$family][$key] = $asset
        }
    }
}
if ($newKeys.Count -eq 0) { throw 'No native dependency artifacts were produced.' }
$result = [ordered]@{}
foreach ($family in @('assets', 'aggregates')) {
    $result[$family] = @($families[$family].Keys | Sort-Object -CaseSensitive | ForEach-Object { $families[$family][$_] })
}
$destination = [IO.Path]::GetFullPath($OutputPath)
New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
$json = ($result | ConvertTo-Json -Depth 50).Replace("`r`n", "`n")
[IO.File]::WriteAllText($destination, $json + "`n", [Text.UTF8Encoding]::new($false))
