param(
    [Parameter(Mandatory)][string]$Received,
    [Parameter(Mandatory)][string[]]$Hosts,
    [Parameter(Mandatory)][string]$Revision,
    [string]$DefaultBranch,
    [string]$RebuildTargets
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$platforms = @(Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$hostsRegistry = @(Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/validation-hosts.json') -Raw | ConvertFrom-Json)
$targets = [ordered]@{}
$deferred = @()
$requirements = [ordered]@{}
$readerTags = [Collections.Generic.SortedSet[string]]::new([StringComparer]::Ordinal)
$independent = @()
foreach ($hostName in $Hosts) {
    $hostRegistration = @($hostsRegistry | Where-Object runner -CEQ $hostName)
    if ($hostRegistration.Count -ne 1) { throw "Unknown validation runner $hostName." }
    $decision = Get-Content -LiteralPath (Join-Path $Received "backend-request-$hostName/acquisition.json") -Raw | ConvertFrom-Json
    if ($decision.commit -cne $Revision -or $decision.packages -isnot [array] -or $decision.deferred -isnot [bool] -or $decision.executedLocally -isnot [bool]) { throw "Missing backend decision for $hostName/$Revision." }
    if ($decision.executedLocally -and ($RebuildTargets -or $decision.deferred -or ($Hosts.Count -gt 1 -and ($decision.packages.Count -or !$decision.portableTestChanges)))) {
        throw "An inline host cannot hide a required execution or production chain: $hostName."
    }
    $seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($package in $decision.packages) {
        if ($package.PSObject.Properties['tag'] -and $package.tag) { $null = $readerTags.Add($package.tag) }
        $target = $package.target
        $registration = @($platforms | Where-Object target -CEQ $target)
        if ($registration.Count -ne 1 -or !$seen.Add($target) -or $package.operation -cnotin @('produce','consume')) {
            throw "Invalid platform request from $hostName."
        }
        if ($package.operation -ceq 'consume' -and ($package.sha256 -cnotmatch '^[0-9a-f]{64}$' -or [string]$package.assetId -cnotmatch '^[1-9][0-9]*$')) {
            throw "Missing exact archived platform for $target."
        }
        if (!$decision.deferred) {
            if ($target -cin @($RebuildTargets -split ',') -or $package.operation -cne 'consume') {
                throw "The local plan did not own this acquisition: $hostName/$target."
            }
            continue
        }
        if (!$targets.Contains($target)) {
            $hostTarget = @($hostsRegistry | Where-Object runner -CEQ $registration[0].runner)
            if ($hostTarget.Count -ne 1) { throw "No unique SDK host for $target." }
            $targets[$target] = [ordered]@{target=$target; asset=$registration[0].asset; runner=$registration[0].runner; hostTarget=$hostTarget[0].target
                operation=$package.operation; sha256=$package.sha256; assetId=$package.assetId; tag=$(if ($package.PSObject.Properties['tag']) { $package.tag })}
        } else {
            $prior = $targets[$target]
            if ($package.operation -ceq 'produce' -or $prior.sha256 -cne $package.sha256) { $prior.operation = 'produce' }
        }
    }
    if (!$decision.deferred -and !$decision.executedLocally) {
        $digest = if ($decision.packages.Count -eq 1) { $decision.packages[0].sha256 } else { '' }
        $independent += @{runner=$hostName;digest=$digest}
    }
    if ($decision.deferred) {
        if (!$seen.Count) { throw "An empty acquisition cannot defer execution: $hostName." }
        $names = @($seen | Sort-Object -CaseSensitive)
        $requirements[$hostName] = $names
        $deferred += [pscustomobject]@{runner=$hostName;target=$hostRegistration[0].target;targets=$names;pattern='';destination='artifacts/candidates'}
    }
}
# The initial catalog must publish all registered targets together. Check before a cold rebuild clears seed identities.
$bootstrap = $targets.Count -eq $platforms.Count -and !@($targets.Values | Where-Object sha256).Count
$requested = @()
if ($RebuildTargets) {
    $requested = @($RebuildTargets.Split(',') | ForEach-Object { $_.Trim() })
    if (@($requested | Sort-Object -CaseSensitive -Unique).Count -ne $requested.Count) { throw 'Duplicate manual rebuild target.' }
    foreach ($name in $requested) {
        if ($name -cnotin $platforms.target -or !$targets.Contains($name)) { throw "No frozen backend platform to rebuild: $name." }
        $targets[$name].operation = 'produce'
        $targets[$name].tag = $null
        $targets[$name].sha256 = $null
        $targets[$name].assetId = $null
    }
}
$eligible = @($targets.Values | Where-Object { $_.operation -ceq 'produce' -and $_.target -cnotin $requested })
if ($DefaultBranch -and $eligible.Count) {
    $matchesByTarget = & "$PSScriptRoot/Get-MergedBackendCandidates.ps1" -Revision $Revision -Targets $eligible -DefaultBranch $DefaultBranch
    foreach ($item in $eligible) {
        if ($matchesByTarget.ContainsKey($item.target)) {
            $item.operation = 'promote'
            $item.promotion = $matchesByTarget[$item.target]
        }
    }
}
# Cross-target builds need an independently acquired native tools host. The planner names that
# host through its explicit product requests; it must have an input in the same producer wave.
foreach ($target in $targets.Values) {
    if ($target.operation -ceq 'produce' -and $target.hostTarget -cne $target.target -and !$targets.Contains($target.hostTarget)) {
        throw "The frozen requests for $($target.target) omitted its required build host $($target.hostTarget)."
    }
}
foreach ($target in $targets.Values) {
    $target.hostSeed = $null
    if ($target.operation -ceq 'produce' -and $target.hostTarget -cne $target.target) {
        $hostPackage = $targets[$target.hostTarget]
        if ($hostPackage.sha256) { $target.hostSeed = @{sha256=$hostPackage.sha256;assetId=$hostPackage.assetId} }
    }
    $target.nativeInputs = @($targets.Values | Where-Object { $_.operation -ceq 'produce' -and ($_.target -ceq $target.target -or $_.hostTarget -ceq $target.target) }).Count -gt 0
}
foreach ($consumer in $deferred) {
    $built = @($consumer.targets | Where-Object { $targets[$_].operation -ceq 'produce' })
    if ($built.Count -eq 1) {
        $consumer.pattern = "backend-candidate-$($built[0])"
        $consumer.destination = "artifacts/candidates/$($consumer.pattern)"
    } elseif ($built.Count -gt 1) {
        $consumer.pattern = "backend-candidate-{$($built -join ',')}"
    }
}
# Each target is produced once. Consumers that share a target or its build host
# travel in the same workflow call, independently of unrelated producer groups.
$chains = @()
$pending = [Collections.Generic.List[string]]::new()
foreach ($runner in $requirements.Keys) { $pending.Add($runner) }
while ($pending.Count) {
    $members = @($pending[0])
    if ($bootstrap) { $members = $pending.ToArray() }
    $closure = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    for ($i = 0; $i -lt $members.Count; $i++) {
        $null = $pending.Remove($members[$i])
        foreach ($name in $requirements[$members[$i]]) {
            $null = $closure.Add($name)
            if ($targets[$name].operation -ceq 'produce') { $null = $closure.Add($targets[$name].hostTarget) }
        }
        foreach ($runner in $pending) {
            if ($runner -cin $members) { continue }
            foreach ($name in $requirements[$runner]) {
                if ($closure.Contains($name) -or ($targets[$name].operation -ceq 'produce' -and $closure.Contains($targets[$name].hostTarget))) {
                    $members += $runner
                    break
                }
            }
        }
    }
    $platformSet = @($targets.Values | Where-Object { $closure.Contains($_.target) })
    $chains += @{platforms=@{include=$platformSet}; builders=@{include=@($platformSet | Where-Object operation -CEQ 'produce')};
        producers=@{include=@($platformSet | Where-Object operation -CNE 'consume')};
        consumers=@{include=@($deferred | Where-Object { $_.runner -cin $members })}}
}
[pscustomobject]@{platforms=@($targets.Values); producers=@($targets.Values | Where-Object operation -CEQ 'produce');
    deferred=$deferred; chains=$chains; independent=$independent; readerTags=@($readerTags)}
