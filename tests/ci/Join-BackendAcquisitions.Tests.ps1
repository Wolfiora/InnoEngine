$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$work = [IO.Directory]::CreateTempSubdirectory('InnoBackendRequests-').FullName
$hostsRegistered = @(Get-Content (Join-Path $PSScriptRoot '../../.github/validation-hosts.json') -Raw | ConvertFrom-Json)
$platforms = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$revision = 'a' * 40
$runner = $hostsRegistered[0].runner
$target = $hostsRegistered[0].target
$owner = [IO.Directory]::CreateDirectory((Join-Path $work "backend-request-$runner-attempt-1")).FullName
$file = Join-Path $owner 'acquisition.json'
function Join-Requests([array]$Packages, [string]$Commit=$revision) {
    [IO.File]::WriteAllText($file,(ConvertTo-Json @{commit=$Commit;packages=$Packages} -Depth 8))
    & "$PSScriptRoot/Join-BackendAcquisitions.ps1" -Received $work -Hosts @($runner) -Revision $revision
}
try {
    $none = Join-Requests @()
    if ($none.platforms.Count -or $none.producers.Count -or $none.deferred.Count) { throw 'An independent plan allocated a backend job.' }
    $prior = @{target=$target;operation='consume';sha256=('b'*64);assetId=12}
    $warm = Join-Requests @($prior)
    if ($warm.producers.Count -or $warm.platforms.Count -ne 1 -or $warm.platforms[0].nativeInputs) { throw 'A matching backend ZIP scheduled source production.' }
    $cross = $platforms | Where-Object { $_.target -cne $target -and $_.runner -ceq $runner } | Select-Object -First 1
    if ($cross) {
        $changed = @{target=$cross.target;operation='produce';sha256=$null;assetId=$null}
        $candidate = Join-Requests @($prior,$changed)
        if ($candidate.producers.Count -ne 1 -or $candidate.producers[0].target -cne $cross.target -or !$candidate.platforms[0].nativeInputs) {
            throw 'Cross compilation failed to acquire the host tools independently.'
        }
    }
    $blocked = $false
    try { $null = Join-Requests @($prior,$prior) } catch { $blocked = $_.Exception.Message.Contains('Invalid platform request') }
    if (!$blocked) { throw 'Duplicate target was accepted.' }
    $blocked = $false
    try { $null = Join-Requests @($prior) ('d'*40) } catch { $blocked = $_.Exception.Message.Contains('Missing backend decision') }
    if (!$blocked) { throw 'A decision from another candidate was accepted.' }
} finally {
    if ([IO.File]::Exists($file)) { [IO.File]::Delete($file) }
    [IO.Directory]::Delete($owner)
    [IO.Directory]::Delete($work)
}
