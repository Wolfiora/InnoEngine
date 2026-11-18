param(
    [Parameter(Mandatory)][string]$Received,
    [Parameter(Mandatory)][string[]]$Hosts,
    [Parameter(Mandatory)][string]$Revision,
    [int]$Attempt = 1,
    [string]$RebuildTargets
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$platforms = @(Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$hostsRegistry = @(Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/validation-hosts.json') -Raw | ConvertFrom-Json)
$targets = [ordered]@{}
$deferred = @()
foreach ($hostName in $Hosts) {
    $hostRegistration = @($hostsRegistry | Where-Object runner -CEQ $hostName)
    if ($hostRegistration.Count -ne 1) { throw "Unknown validation runner $hostName." }
    $decision = Get-Content -LiteralPath (Join-Path $Received "backend-request-$hostName-attempt-$Attempt/acquisition.json") -Raw | ConvertFrom-Json
    if ($decision.commit -cne $Revision -or $decision.packages -isnot [array]) { throw "Missing backend decision for $hostName/$Revision." }
    $seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($package in $decision.packages) {
        $target = $package.target
        $registration = @($platforms | Where-Object target -CEQ $target)
        if ($registration.Count -ne 1 -or !$seen.Add($target) -or $package.operation -cnotin @('produce','consume')) {
            throw "Invalid platform request from $hostName."
        }
        if ($package.operation -ceq 'consume' -and ($package.sha256 -cnotmatch '^[0-9a-f]{64}$' -or [string]$package.assetId -cnotmatch '^[1-9][0-9]*$')) {
            throw "Missing exact archived platform for $target."
        }
        if (!$targets.Contains($target)) {
            $hostTarget = @($hostsRegistry | Where-Object runner -CEQ $registration[0].runner)
            if ($hostTarget.Count -ne 1) { throw "No unique SDK host for $target." }
            $targets[$target] = [ordered]@{target=$target; asset=$registration[0].asset; runner=$registration[0].runner; hostTarget=$hostTarget[0].target
                operation=$package.operation; sha256=$package.sha256; assetId=$package.assetId}
        } else {
            $prior = $targets[$target]
            if ($package.operation -ceq 'produce' -or $prior.sha256 -cne $package.sha256) { $prior.operation = 'produce' }
        }
    }
    if ($seen.Count) {
        $names = @($seen | Sort-Object -CaseSensitive)
        $pattern = if ($names.Count -eq 1) { "backend-candidate-$($names[0])" } else { "backend-candidate-{$($names -join ',')}" }
        $deferred += [pscustomobject]@{runner=$hostName;target=$hostRegistration[0].target;pattern=$pattern}
    }
}
if ($RebuildTargets) {
    $requested = @($RebuildTargets.Split(',') | ForEach-Object { $_.Trim() })
    if (@($requested | Sort-Object -CaseSensitive -Unique).Count -ne $requested.Count) { throw 'Duplicate manual rebuild target.' }
    foreach ($name in $requested) {
        if ($name -cnotin $platforms.target -or !$targets.Contains($name)) { throw "No frozen backend platform to rebuild: $name." }
        $targets[$name].operation = 'produce'
        $targets[$name].sha256 = $null
        $targets[$name].assetId = $null
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
[pscustomobject]@{ platforms=@($targets.Values); producers=@($targets.Values | Where-Object operation -CEQ 'produce'); deferred=$deferred }
