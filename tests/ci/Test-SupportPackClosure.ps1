param([Parameter(Mandatory)][string]$Archive)

$ErrorActionPreference = 'Stop'
$zip = [IO.Compression.ZipFile]::OpenRead([IO.Path]::GetFullPath($Archive))
try {
    $runtime = @($zip.Entries | Where-Object { $_.FullName -cmatch '^native/' -and $_.Name -match 'miniaudio.*\.(dll|dylib)$' })
    $link = @($zip.Entries | Where-Object { $_.FullName -cmatch '^PlayerLink/native/' -and $_.Name -match 'miniaudio.*\.(dll|dylib)$' })
    if ($runtime.Count -ne 1 -or $link.Count -ne 1 -or $runtime[0].Name -cne $link[0].Name) {
        throw 'The runtime and PlayerLink closures must each contain the same single MiniAudio runtime library.'
    }
    $hashes = foreach ($entry in @($runtime[0], $link[0])) {
        $stream = $entry.Open()
        try { [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($stream)) }
        finally { $stream.Dispose() }
    }
    if ($hashes[0] -cne $hashes[1]) { throw 'The runtime and PlayerLink native library contents differ.' }
} finally { $zip.Dispose() }
