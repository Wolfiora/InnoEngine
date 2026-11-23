$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/output-transport-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$source = Join-Path $root 'source'
New-Item -ItemType Directory -Path "$source/bin" -Force | Out-Null
$bytes = [byte[]](0, 255, 13, 10, 65)
[IO.File]::WriteAllBytes("$source/bin/app", $bytes)
if (-not $IsWindows) { [IO.File]::SetUnixFileMode("$source/bin/app", 493) }
$archive = Join-Path $root 'outputs.tar'
& "$PSScriptRoot/../../tools/ci/Transfer-ValidationOutputs.ps1" -Operation Pack -Directory $source -Archive $archive
& "$PSScriptRoot/../../tools/ci/Transfer-ValidationOutputs.ps1" -Operation Unpack -Directory "$root/restored" -Archive $archive
if ((Get-FileHash "$source/bin/app").Hash -cne (Get-FileHash "$root/restored/bin/app").Hash) { throw 'Output bytes changed in transport.' }
if (-not $IsWindows -and [int][IO.File]::GetUnixFileMode("$root/restored/bin/app") -ne 493) { throw 'Executable permissions changed in transport.' }
$passed = 1
foreach ($kind in @('traversal', 'absolute', 'drive', 'duplicate', 'symlink', 'hardlink', 'existing')) {
    $bad = Join-Path $root "$kind.tar"
    $stream = [IO.File]::Create($bad)
    $writer = [System.Formats.Tar.TarWriter]::new($stream)
    try {
        $name = switch ($kind) { traversal { '../escape' }; absolute { '/escape' }; drive { 'C:/escape' }; default { 'app' } }
        $type = switch ($kind) { symlink { [System.Formats.Tar.TarEntryType]::SymbolicLink }; hardlink { [System.Formats.Tar.TarEntryType]::HardLink }; default { [System.Formats.Tar.TarEntryType]::RegularFile } }
        $entry = [System.Formats.Tar.PaxTarEntry]::new($type, $name)
        if ($kind -in @('symlink', 'hardlink')) { $entry.LinkName = '../escape' }
        else { $entry.DataStream = [IO.MemoryStream]::new($bytes) }
        $writer.WriteEntry($entry)
        if ($kind -eq 'duplicate') {
            $entry.DataStream = [IO.MemoryStream]::new($bytes)
            $writer.WriteEntry($entry)
        }
    } finally { $writer.Dispose(); $stream.Dispose() }
    $destination = Join-Path $root "rejected-$kind"
    if ($kind -eq 'existing') { New-Item -ItemType Directory -Path $destination | Out-Null }
    $rejected = $false
    try { & "$PSScriptRoot/../../tools/ci/Transfer-ValidationOutputs.ps1" -Operation Unpack -Directory $destination -Archive $bad }
    catch { $rejected = $true }
    if (-not $rejected) { throw "Unsafe output accepted: $kind" }
    if ($kind -ne 'existing' -and (Test-Path $destination)) { throw 'Unsafe archive was partially materialized.' }
    $passed++
}
Write-Output "Output transport contracts passed: $passed."
