$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/support-pack-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $directory -Force | Out-Null
$cases = @(
    @{ name = 'windows'; library = 'miniaudio-release.dll'; runtime = 'payload'; link = 'payload'; accept = $true },
    @{ name = 'macos'; library = 'libminiaudio-release.dylib'; runtime = 'payload'; link = 'payload'; accept = $true },
    @{ name = 'same-length-tamper'; library = 'miniaudio-release.dll'; runtime = 'payload'; link = 'changed'; accept = $false },
    @{ name = 'missing-runtime'; library = 'miniaudio-release.dll'; runtime = $null; link = 'payload'; accept = $false },
    @{ name = 'missing-link'; library = 'miniaudio-release.dll'; runtime = 'payload'; link = $null; accept = $false },
    @{ name = 'duplicate'; library = 'miniaudio-release.dll'; runtime = 'payload'; link = 'payload'; accept = $false }
)
foreach ($case in $cases) {
    $archive = Join-Path $directory ($case.name + '.zip')
    $zip = [IO.Compression.ZipFile]::Open($archive, [IO.Compression.ZipArchiveMode]::Create)
    try {
        foreach ($part in @('runtime', 'link')) {
            if ($null -eq $case.$part) { continue }
            $prefix = if ($part -eq 'runtime') { 'native' } else { 'PlayerLink/native' }
            $entry = $zip.CreateEntry("$prefix/$($case.library)")
            $writer = [IO.StreamWriter]::new($entry.Open())
            try { $writer.Write($case.$part) } finally { $writer.Dispose() }
        }
        if ($case.name -eq 'duplicate') { $null = $zip.CreateEntry("native/duplicate/$($case.library)") }
    } finally { $zip.Dispose() }
    $accepted = $false
    try { & "$PSScriptRoot/Test-SupportPackClosure.ps1" -Archive $archive; $accepted = $true }
    catch { $_.Exception.Message | Set-Content -LiteralPath (Join-Path $directory ($case.name + '.log')) }
    if ($accepted -ne $case.accept) { throw "Unexpected Support Pack closure result: $($case.name)" }
}
Write-Output "Passed $($cases.Count) Support Pack closure cases. Evidence: $directory"
