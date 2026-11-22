param(
    [Parameter(Mandatory)][ValidateSet('Pack', 'Unpack')][string]$Operation,
    [Parameter(Mandatory)][string]$Directory,
    [Parameter(Mandatory)][string]$Archive
)
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath($Directory)
$archivePath = [IO.Path]::GetFullPath($Archive)
if ($Operation -ceq 'Pack') {
    if (-not [IO.Directory]::Exists($root) -or [IO.File]::Exists($archivePath)) { throw 'Output transport requires an existing owner and a new archive.' }
    if ($archivePath.StartsWith($root.TrimEnd([IO.Path]::DirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'The transport archive must be outside the compiled owner.'
    }
    $pending = [Collections.Generic.Stack[string]]::new()
    $pending.Push($root)
    while ($pending.Count -gt 0) {
        $path = $pending.Pop()
        $attributes = [IO.File]::GetAttributes($path)
        if ($attributes.HasFlag([IO.FileAttributes]::ReparsePoint)) { throw 'Compiled transport cannot contain links.' }
        if ($attributes.HasFlag([IO.FileAttributes]::Directory)) {
            foreach ($child in [IO.Directory]::EnumerateFileSystemEntries($path)) { $pending.Push($child) }
        }
    }
    [System.Formats.Tar.TarFile]::CreateFromDirectory($root, $archivePath, $false)
    return
}
if ([IO.Directory]::Exists($root) -or [IO.File]::Exists($root)) { throw 'Retained output staging must be new.' }
$stream = [IO.File]::OpenRead($archivePath)
try {
    # Validate every entry before materializing any content. Only files and directories belong to this transport.
    $reader = [System.Formats.Tar.TarReader]::new($stream, $true)
    $paths = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
    try {
        while ($null -ne ($entry = $reader.GetNextEntry())) {
            $name = $entry.Name.Replace('\', '/').TrimEnd('/')
            if ($name -ceq '.' -and $entry.EntryType -eq [System.Formats.Tar.TarEntryType]::Directory) { continue }
            if ($name.StartsWith('./')) { $name = $name.Substring(2) }
            if ([string]::IsNullOrWhiteSpace($name) -or $name.StartsWith('/') -or $name.Contains(':') -or
                @($name.Split('/') | Where-Object { $_ -cin @('', '.', '..') }).Count -gt 0 -or -not $paths.Add($name)) {
                throw 'Retained output contains an unsafe or duplicate path.'
            }
            if ($entry.EntryType -notin @([System.Formats.Tar.TarEntryType]::RegularFile, [System.Formats.Tar.TarEntryType]::V7RegularFile,
                [System.Formats.Tar.TarEntryType]::Directory)) { throw 'Retained output contains a link or unsupported entry.' }
        }
    } finally { $reader.Dispose() }
    if ($paths.Count -eq 0) { throw 'Retained output archive is empty.' }
    $stream.Position = 0
    $reader = [System.Formats.Tar.TarReader]::new($stream, $true)
    try {
        [IO.Directory]::CreateDirectory($root) | Out-Null
        while ($null -ne ($entry = $reader.GetNextEntry())) {
            $name = $entry.Name.Replace('\', '/').TrimEnd('/')
            if ($name -ceq '.') { continue }
            if ($name.StartsWith('./')) { $name = $name.Substring(2) }
            $destination = Join-Path $root $name
            if ($entry.EntryType -eq [System.Formats.Tar.TarEntryType]::Directory) {
                [IO.Directory]::CreateDirectory($destination) | Out-Null
            } else {
                [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($destination)) | Out-Null
                $file = [IO.File]::Open($destination, [IO.FileMode]::CreateNew)
                try { $entry.DataStream.CopyTo($file) } finally { $file.Dispose() }
                if (-not $IsWindows) { [IO.File]::SetUnixFileMode($destination, ([int]$entry.Mode -band 511)) }
            }
        }
    } finally { $reader.Dispose() }
} finally { $stream.Dispose() }
