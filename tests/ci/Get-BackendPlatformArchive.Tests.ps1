$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/backend-cache', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$bytes = [byte[]](1,2,3,5)
$digest = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
$requests = [Collections.Generic.List[string]]::new()
function Invoke-WebRequest($Uri,$Headers,$OutFile) {
    if ($Uri -cne 'https://fixture.invalid/repos/fixture/engine/releases/assets/17') { throw 'Requested a different release asset.' }
    $requests.Add($Uri)
    [IO.File]::WriteAllBytes($OutFile,$bytes)
}
function Acquire { & "$PSScriptRoot/Get-BackendPlatformArchive.ps1" -Directory $directory -Repository 'fixture/engine' -ApiUrl 'https://fixture.invalid' -Sha256 $digest -AssetId 17 }
try {
    $cold = Acquire
    $warm = Acquire
    if ($warm -cne $cold -or $requests.Count -ne 1 -or @(Get-ChildItem -LiteralPath $directory -File).Count -ne 1) {
        throw 'A matching platform digest did not reuse the cold download.'
    }
    [IO.File]::WriteAllBytes($warm,[byte[]](1,2,3))
    try { $null = Acquire; throw 'A corrupted cache passed.' }
    catch { if ($_.Exception.Message -cne 'Cached backend ZIP differs from its catalog digest.') { throw } }
    if ($requests.Count -ne 1) { throw 'A corrupt local entry initiated another transfer.' }
} finally {
    if ([IO.Directory]::Exists($directory)) { [IO.Directory]::Delete($directory,$true) }
}
