param(
    [Parameter(Mandatory)][string]$Directory,
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][string]$Platform,
    [Parameter(Mandatory)][object]$Receipt
)

$ErrorActionPreference = 'Stop'
$manifest = Get-Content -LiteralPath (Join-Path $Directory 'deliverable-manifest.json') -Raw | ConvertFrom-Json
if ($manifest.format -ne 1 -or $manifest.commit -cne $Commit -or $Receipt.commit -cne $Commit -or
    -not $Receipt.full -or $manifest.configuration -cne 'Release' -or $manifest.platform -cne $Platform -or
    $manifest.tree -cne $Receipt.tree -or $manifest.environment -cne $Receipt.environment -or
    $manifest.planIdentity -cne $Receipt.planIdentity) { throw 'Deliverable provenance does not match its complete functional certification.' }
foreach ($identity in @($manifest.tree, $manifest.environment, $manifest.planIdentity)) {
    if ($identity -cnotmatch '^[0-9a-f]{64}$') { throw 'The deliverable identity is incomplete.' }
}
$files = @($manifest.files.PSObject.Properties)
if ($files.Count -ne 2 -or @($files.Name | Where-Object { $_ -cnotin @('Editor.zip', 'PlayerSupportPack.zip') }).Count) {
    throw 'The complete Editor and Player Support Pack artifact set is required.'
}
$members = @(Get-ChildItem -LiteralPath $Directory -Force)
if ($members.Count -ne 3 -or @($members | Where-Object { $_.PSIsContainer -or ($_.Attributes -band [IO.FileAttributes]::ReparsePoint) }).Count) {
    throw 'Release input contains an unexpected member, directory or link.'
}
foreach ($file in $files) {
    $path = Join-Path $Directory $file.Name
    if ($file.Value -cnotmatch '^[0-9a-f]{64}$' -or
        (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant() -cne $file.Value) {
        throw "Deliverable content does not match the verified bytes: $($file.Name)"
    }
    [pscustomobject]@{ path = [IO.Path]::GetFullPath($path); name = "$Platform-$($file.Name)"; sha256 = $file.Value }
}
