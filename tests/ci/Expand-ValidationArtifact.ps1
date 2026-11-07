param(
    [Parameter(Mandatory)][string]$ArchivePath,
    [Parameter(Mandatory)][string]$Destination,
    [Parameter(Mandatory)][string]$ExpectedDigest
)

$ErrorActionPreference = 'Stop'
if ($ExpectedDigest -cnotmatch '^sha256:[0-9a-f]{64}$' -or
    'sha256:' + (Get-FileHash -LiteralPath $ArchivePath -Algorithm SHA256).Hash.ToLowerInvariant() -cne $ExpectedDigest) {
    throw 'Downloaded archive does not match its authenticated server digest.'
}
$root = [IO.Path]::GetFullPath($Destination)
if (Test-Path -LiteralPath $root) { throw 'Artifact extraction requires a new, explicitly owned destination.' }
$archive = [IO.Compression.ZipFile]::OpenRead([IO.Path]::GetFullPath($ArchivePath))
try {
    $seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    [long]$total = 0
    foreach ($entry in $archive.Entries) {
        $name = $entry.FullName
        $kind = ($entry.ExternalAttributes -shr 16) -band 0xF000
        if (-not $name -or $name.Contains('\') -or $name.Contains(':') -or $name.StartsWith('/') -or
            $name -match '(^|/)\.{1,2}(/|$)' -or $kind -notin @(0, 0x4000, 0x8000) -or
            -not $seen.Add($name.TrimEnd('/'))) { throw "Unsafe or ambiguous artifact member: $name" }
        $path = [IO.Path]::GetFullPath($name, $root)
        if (-not $path.StartsWith($root + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
            throw "Artifact member escapes its extraction owner: $name"
        }
        $total += $entry.Length
        if ($total -gt 32GB -or $seen.Count -gt 200000) { throw 'Artifact exceeds its explicit extraction budget.' }
    }
    if ($seen.Count -eq 0) { throw 'A required artifact archive cannot be empty.' }
    [IO.Compression.ZipFileExtensions]::ExtractToDirectory($archive, $root, $false)
} finally { $archive.Dispose() }
