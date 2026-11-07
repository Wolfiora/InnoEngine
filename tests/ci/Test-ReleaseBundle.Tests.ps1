$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$root = Join-Path $repository ('artifacts/validation/release-bundle-tests/' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $root | Out-Null
$commit = 'a' * 40
$passed = 0
function Invoke-BundleCase([string]$Name, [scriptblock]$Change, [bool]$Accept) {
    $directory = Join-Path $root $Name
    New-Item -ItemType Directory -Path $directory | Out-Null
    $files = @{}
    foreach ($name in @('Editor.zip', 'PlayerSupportPack.zip')) {
        [IO.File]::WriteAllText((Join-Path $directory $name), 'fixture payload')
        $files[$name] = (Get-FileHash -LiteralPath (Join-Path $directory $name)).Hash.ToLowerInvariant()
    }
    $receipt = @{ commit = $commit; full = $true; tree = 'b' * 64; environment = 'c' * 64; planIdentity = 'd' * 64 }
    $manifest = @{ format = 1; commit = $commit; tree = $receipt.tree; environment = $receipt.environment;
        planIdentity = $receipt.planIdentity; configuration = 'Release'; platform = 'windows-2022'; files = $files }
    & $Change $directory $manifest $receipt
    $manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $directory 'deliverable-manifest.json')
    $accepted = $false
    try {
        $null = & "$PSScriptRoot/Test-ReleaseBundle.ps1" -Directory $directory -Commit $commit -Platform windows-2022 -Receipt $receipt
        $accepted = $true
    } catch { if ($Accept) { throw } }
    if ($accepted -ne $Accept) { throw "Unexpected artifact content result: $Name" }
    $script:passed++
}
Invoke-BundleCase 'verified' { param($d, $m, $r) } $true
Invoke-BundleCase 'same-length-tamper' { param($d, $m, $r) [IO.File]::WriteAllText((Join-Path $d 'Editor.zip'), 'changed payload') } $false
Invoke-BundleCase 'missing-pack' { param($d, $m, $r) $m.files.Remove('PlayerSupportPack.zip') } $false
Invoke-BundleCase 'wrong-commit' { param($d, $m, $r) $m.commit = 'e' * 40 } $false
Invoke-BundleCase 'wrong-tree' { param($d, $m, $r) $m.tree = 'e' * 64 } $false
Invoke-BundleCase 'wrong-environment' { param($d, $m, $r) $m.environment = 'e' * 64 } $false
Invoke-BundleCase 'wrong-plan' { param($d, $m, $r) $m.planIdentity = 'e' * 64 } $false
Invoke-BundleCase 'wrong-platform' { param($d, $m, $r) $m.platform = 'macos-15' } $false
Invoke-BundleCase 'debug-output' { param($d, $m, $r) $m.configuration = 'Debug' } $false
Invoke-BundleCase 'bounded-evidence' { param($d, $m, $r) $r.full = $false } $false
Invoke-BundleCase 'undeclared-member' { param($d, $m, $r) [IO.File]::WriteAllText((Join-Path $d 'unexpected.dll'), 'unexpected') } $false
function Invoke-ZipCase([string]$Name, [string[]]$Members, [bool]$Accept, [bool]$Corrupt = $false, [bool]$Link = $false) {
    $path = Join-Path $root ($Name + '.zip')
    $stream = [IO.File]::Create($path)
    $archive = [IO.Compression.ZipArchive]::new($stream, [IO.Compression.ZipArchiveMode]::Create)
    try {
        foreach ($member in $Members) {
            $entry = $archive.CreateEntry($member)
            if ($Link) { $entry.ExternalAttributes = -1610612736 }
            $writer = [IO.StreamWriter]::new($entry.Open())
            try { $writer.Write('payload') } finally { $writer.Dispose() }
        }
    } finally { $archive.Dispose(); $stream.Dispose() }
    $digest = 'sha256:' + (Get-FileHash -LiteralPath $path).Hash.ToLowerInvariant()
    if ($Corrupt) { $digest = 'sha256:' + ('0' * 64) }
    $accepted = $false
    try {
        & "$PSScriptRoot/Expand-ValidationArtifact.ps1" -ArchivePath $path -Destination (Join-Path $root ($Name + '-extracted')) -ExpectedDigest $digest
        $accepted = $true
    } catch { if ($Accept) { throw } }
    if ($accepted -ne $Accept) { throw "Unexpected archive boundary result: $Name" }
    $script:passed++
}
Invoke-ZipCase 'valid-archive' @('manifest.json', 'Editor.zip') $true
Invoke-ZipCase 'digest-mismatch' @('Editor.zip') $false -Corrupt $true
Invoke-ZipCase 'path-escape' @('../escaped.txt') $false
Invoke-ZipCase 'absolute-member' @('/escaped.txt') $false
Invoke-ZipCase 'windows-escape' @('..\escaped.txt') $false
Invoke-ZipCase 'case-collision' @('Editor.zip', 'editor.zip') $false
Invoke-ZipCase 'linked-member' @('linked.txt') $false -Link $true
Invoke-ZipCase 'empty-archive' @() $false
Write-Output "Release bundle and archive contracts passed: $passed. Evidence: $root"
