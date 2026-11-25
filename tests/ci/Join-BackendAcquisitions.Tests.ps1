$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$work = [IO.Directory]::CreateTempSubdirectory('InnoBackendRequests-').FullName
$hostsRegistered = @(Get-Content (Join-Path $PSScriptRoot '../../.github/validation-hosts.json') -Raw | ConvertFrom-Json)
$platforms = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$revision = 'a' * 40
$runner = $hostsRegistered[0].runner
$target = $hostsRegistered[0].target
$owner = [IO.Directory]::CreateDirectory((Join-Path $work "backend-request-$runner")).FullName
$file = Join-Path $owner 'acquisition.json'
function Join-Requests([array]$Packages, [string]$Commit=$revision, [string]$RebuildTargets, [bool]$Deferred=($Packages.Count -gt 0)) {
    [IO.File]::WriteAllText($file,(ConvertTo-Json @{commit=$Commit;packages=$Packages;deferred=$Deferred;executedLocally=$false} -Depth 8))
    & "$PSScriptRoot/../../tools/ci/Join-BackendAcquisitions.ps1" -Received $work -Hosts @($runner) -Revision $revision -RebuildTargets $RebuildTargets
}
try {
    $none = Join-Requests @()
    if ($none.platforms.Count -or $none.producers.Count -or $none.deferred.Count) { throw 'An independent plan allocated a backend job.' }
    $prior = @{target=$target;operation='consume';sha256=('b'*64);assetId=12;tag='backend-prebuilts-2026.11.19.1'}
    $warm = Join-Requests @($prior)
    if ($warm.producers.Count -or $warm.platforms.Count -ne 1 -or $warm.platforms[0].nativeInputs) { throw 'A matching backend ZIP scheduled source production.' }
    if ($warm.deferred[0].pattern) { throw 'Release consumption requested a candidate artifact.' }
    $peer = $hostsRegistered | Where-Object runner -CNE $runner | Select-Object -First 1
    if ($peer) {
        $peerOwner = [IO.Directory]::CreateDirectory((Join-Path $work "backend-request-$($peer.runner)")).FullName
        $peerFile = Join-Path $peerOwner 'acquisition.json'
        try {
            $pair = @($runner,$peer.runner)
            $peerPackage = @{target=$peer.target;operation='produce';sha256=$null;assetId=$null}
            $peerDecision = @{commit=$revision;packages=@($peerPackage);deferred=$true;executedLocally=$false;portableTestChanges=$false}
            [IO.File]::WriteAllText($peerFile,(ConvertTo-Json $peerDecision -Depth 8))
            $split = & "$PSScriptRoot/../../tools/ci/Join-BackendAcquisitions.ps1" -Received $work -Hosts $pair -Revision $revision
            if ($split.chains.Count -ne 2) { throw 'Disjoint target requests must start independent chains.' }
            if ($split.readerTags.Count -ne 1 -or $split.readerTags[0] -cne $prior.tag) { throw 'Publication lost a sibling reader.' }
            $coldHost = @($platforms | Where-Object runner -CEQ $runner | ForEach-Object {
                @{target=$_.target;operation='produce';sha256=$null;assetId=$null}
            })
            [IO.File]::WriteAllText($file,(ConvertTo-Json @{commit=$revision;packages=$coldHost;deferred=$true;executedLocally=$false} -Depth 8))
            $initial = & "$PSScriptRoot/../../tools/ci/Join-BackendAcquisitions.ps1" -Received $work -Hosts $pair -Revision $revision
            if ($initial.chains.Count -ne 1 -or $initial.chains[0].producers.include.Count -ne $platforms.Count) { throw 'The initial catalog requires the complete producer set.' }
            $null = Join-Requests @($prior)
            $peerDecision.packages += $prior
            [IO.File]::WriteAllText($peerFile,(ConvertTo-Json $peerDecision -Depth 8))
            $shared = & "$PSScriptRoot/../../tools/ci/Join-BackendAcquisitions.ps1" -Received $work -Hosts $pair -Revision $revision
            if ($shared.chains.Count -ne 1 -or $shared.chains[0].consumers.include.Count -ne 2 -or $shared.chains[0].platforms.include.Count -ne 2) {
                throw 'A shared target requires one acquisition for both consumers.'
            }
            $peerDecision.packages = @()
            $peerDecision.deferred = $false
            [IO.File]::WriteAllText($peerFile,(ConvertTo-Json $peerDecision -Depth 8))
            $ready = & "$PSScriptRoot/../../tools/ci/Join-BackendAcquisitions.ps1" -Received $work -Hosts $pair -Revision $revision
            if ($ready.independent.Count -ne 1 -or $ready.independent[0].runner -cne $peer.runner -or $ready.chains.Count -ne 1) {
                throw 'A host with no pending producer must not wait for other hosts.'
            }
            $peerDecision.executedLocally = $true
            [IO.File]::WriteAllText($peerFile,(ConvertTo-Json $peerDecision -Depth 8))
            $rejected = $false
            try { $null = & "$PSScriptRoot/../../tools/ci/Join-BackendAcquisitions.ps1" -Received $work -Hosts $pair -Revision $revision }
            catch { $rejected = $_.Exception.Message.Contains('inline host') }
            if (!$rejected) { throw 'A product host was allowed to suppress its independent worker.' }
            $peerDecision.portableTestChanges = $true
            [IO.File]::WriteAllText($peerFile,(ConvertTo-Json $peerDecision -Depth 8))
            $mixed = & "$PSScriptRoot/../../tools/ci/Join-BackendAcquisitions.ps1" -Received $work -Hosts $pair -Revision $revision
            if ($mixed.independent.Count -or $mixed.chains.Count -ne 1) { throw 'A completed domain suite was dispatched again.' }
        } finally {
            if ([IO.File]::Exists($peerFile)) { [IO.File]::Delete($peerFile) }
            [IO.Directory]::Delete($peerOwner)
        }
    }
    $local = Join-Requests @($prior) -Deferred $false
    if ($local.platforms.Count -or $local.deferred.Count) { throw 'A host that consumes its own archive was scheduled a second time.' }
    $rejected = $false
    try { $null = Join-Requests @($prior) -RebuildTargets $target -Deferred $false } catch { $rejected = $_.Exception.Message.Contains('did not own') }
    if (!$rejected) { throw 'An inline decision hid a manual producer.' }
    $cold = Join-Requests @($prior) $revision $target
    if ($cold.producers.Count -ne 1 -or $cold.producers[0].sha256 -or $cold.producers[0].assetId) { throw 'A manual cold rebuild restored an old producer seed.' }
    $blocked = $false
    try { $null = Join-Requests @($prior) $revision 'unregistered-target' } catch { $blocked = $_.Exception.Message.Contains('No frozen backend platform') }
    if (!$blocked) { throw 'An unknown manual rebuild target was accepted.' }
    $cross = $platforms | Where-Object { $_.target -cne $target -and $_.runner -ceq $runner } | Select-Object -First 1
    if ($cross) {
        $changed = @{target=$cross.target;operation='produce';sha256=$null;assetId=$null}
        $candidate = Join-Requests @($prior,$changed)
        $name = "backend-candidate-$($cross.target)"
        if ($candidate.deferred[0].pattern -cne $name -or $candidate.deferred[0].destination -cne "artifacts/candidates/$name") {
            throw 'Only a newly produced cross target should transfer through the current workflow.'
        }
        if ($candidate.producers[0].hostSeed.sha256 -cne $prior.sha256 -or $candidate.producers[0].hostSeed.assetId -ne $prior.assetId) {
            throw 'The cross-target managed SDK bootstrap lost the independently selected host package.'
        }
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
