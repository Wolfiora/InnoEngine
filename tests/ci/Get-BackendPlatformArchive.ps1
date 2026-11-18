param(
    [Parameter(Mandatory)][string]$Sha256,
    [Parameter(Mandatory)][long]$AssetId,
    [Parameter(Mandatory)][string]$Directory,
    [string]$Repository = $env:GITHUB_REPOSITORY,
    [string]$ApiUrl = 'https://api.github.com'
)
$ErrorActionPreference = 'Stop'
if ($Sha256 -cnotmatch '^[a-f0-9]{64}$' -or $AssetId -le 0) { throw 'Invalid backend asset digest or ID.' }
$owner = [IO.Directory]::CreateDirectory([IO.Path]::GetFullPath($Directory)).FullName
$archive = Join-Path $owner "$Sha256.zip"
if (Test-Path -LiteralPath $archive -PathType Leaf) {
    if ((Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant() -cne $Sha256) { throw 'Cached backend ZIP differs from its catalog digest.' }
} else {
    if ($Repository -cnotmatch '^[a-zA-Z0-9_.-]+/[a-zA-Z0-9_.-]+$') { throw 'Cold acquisition requires a repository identity.' }
    $partial = Join-Path $owner ([Guid]::NewGuid().ToString('N') + '.partial')
    $headers = @{Accept='application/octet-stream'; 'X-GitHub-Api-Version'='2022-11-28'}
    if ($env:GH_TOKEN) { $headers.Authorization = "Bearer $env:GH_TOKEN" }
    Invoke-WebRequest -Uri "$ApiUrl/repos/$Repository/releases/assets/$AssetId" -Headers $headers -OutFile $partial
    if ((Get-FileHash -LiteralPath $partial).Hash.ToLowerInvariant() -cne $Sha256) { throw 'Cold backend ZIP differs from its catalog digest.' }
    [IO.File]::Move($partial,$archive,$false)
}
$archive
