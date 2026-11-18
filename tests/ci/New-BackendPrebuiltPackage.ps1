param(
    [Parameter(Mandatory)][string]$Staging,
    [Parameter(Mandatory)][string[]]$InputPlans,
    [Parameter(Mandatory)][string]$NativeSelection,
    [Parameter(Mandatory)][string]$EntryAssembly,
    [Parameter(Mandatory)][string]$Target,
    [Parameter(Mandatory)][string]$OutputDirectory,
    [string]$Dotnet = 'dotnet'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$stagingRoot = [IO.Path]::GetFullPath($Staging)
$destination = [IO.Path]::GetFullPath($OutputDirectory)
$registration = @(Get-Content (Join-Path $repository '.github/native-targets.json') -Raw | ConvertFrom-Json | Where-Object target -CEQ $Target)
if ($registration.Count -ne 1 -or $registration[0].asset -cnotmatch '^[a-z0-9]+-prebuilt-backends\.zip$') { throw 'Expected one registered backend platform asset.' }
$plans = @{}
foreach ($path in $InputPlans) {
    $plan = Get-Content -LiteralPath $path -Raw | ConvertFrom-Json -AsHashtable
    if ($plan.target -cne $Target -or $plans.ContainsKey($plan.configuration)) { throw 'Input plans must describe distinct configurations for the requested target.' }
    $plans.Add($plan.configuration, $plan)
}
$files = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
function Add-File([string]$Owner, [string]$Configuration, [string]$InputIdentity, [string]$Name, [string]$Digest) {
    if ($InputIdentity -cnotmatch '^[0-9a-f]{64}$' -or $Digest -cnotmatch '^[0-9a-f]{64}$' -or $files.ContainsKey($Name)) {
        throw "Invalid or duplicated platform file receipt for $Name."
    }
    $files.Add($Name, [ordered]@{ owner=$Owner; configuration=$Configuration; input=$InputIdentity; path=$Name; sha256=$Digest })
}
foreach ($receipt in Get-ChildItem -LiteralPath (Join-Path $stagingRoot 'receipts') -Recurse -Filter files.txt -File) {
    foreach ($line in Get-Content -LiteralPath $receipt.FullName) {
        $fields = $line.Split('|')
        if ($fields.Length -ne 4 -or !$plans.ContainsKey($fields[1]) -or !$plans[$fields[1]].projects.Contains($fields[0]) -or !$fields[2].StartsWith('managed/')) {
            throw "An SDK receipt has no matching project or configuration in the frozen plan: $line"
        }
        Add-File $fields[0] $fields[1] $plans[$fields[1]].projects[$fields[0]] $fields[2] $fields[3].ToLowerInvariant()
    }
}
if (!$files.Count) { throw 'The backend producer emitted no managed compiler outputs.' }
foreach ($configuration in $plans.Keys) {
    $built = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($file in $files.Values) {
        if ($file.configuration -ceq $configuration -and $file.path -cmatch '/lib/[^/]+\.dll$') { $null = $built.Add($file.owner) }
    }
    $missing = @($plans[$configuration].projects.Keys | Where-Object { !$built.Contains($_) })
    if ($missing.Count) { throw "Backend production omitted $configuration owners: $($missing -join ', ')." }
}
$native = Get-Content -LiteralPath $NativeSelection -Raw | ConvertFrom-Json -AsHashtable
$selection = [ordered]@{requests=@($native.requests | Where-Object target -CEQ $Target)}
if (!$selection.requests.Count) { throw 'The platform package lacks verified product requests.' }
$nativeRoot = Split-Path ([IO.Path]::GetFullPath($NativeSelection)) -Parent
$nativePayload = Join-Path $stagingRoot 'native'
[IO.Directory]::CreateDirectory($nativePayload) | Out-Null
foreach ($family in @('assets', 'aggregates')) {
    if (!$native.Contains($family)) { throw "Missing native closure inventory $family." }
    $selection[$family] = @($native[$family] | Where-Object target -CEQ $Target)
    foreach ($item in $selection[$family]) {
        if (!$plans.ContainsKey($item.configuration) -or $item.asset -cnotmatch '^[a-zA-Z0-9._-]+\.zip$' -or $item.sha256 -cnotmatch '^[a-f0-9]{64}$') {
            throw 'The native component receipt does not belong to a requested build context.'
        }
        $name = "native/$($item.sha256).zip"
        if ($files.ContainsKey($name)) { continue }
        $source = Join-Path $nativeRoot $item.asset
        $component = Join-Path $stagingRoot $name
        [IO.File]::Copy($source, $component, $true)
        Add-File $item.component $item.configuration $item.fingerprint $name $item.sha256
    }
}
if (($selection.assets.Count + $selection.aggregates.Count) -eq 0) { throw "No native payloads were selected for $Target." }
$selectionFile = Join-Path $nativePayload 'selection.json'
[IO.File]::WriteAllText($selectionFile, (ConvertTo-Json $selection -Depth 70 -Compress))
$digest = (Get-FileHash -LiteralPath $selectionFile).Hash.ToLowerInvariant()
Add-File $Target 'all' $digest 'native/selection.json' $digest
$inventory = Join-Path $stagingRoot 'package-inventory.json'
[IO.File]::WriteAllText($inventory, (ConvertTo-Json ([ordered]@{version=1; target=$Target; files=@($files.Values)}) -Depth 8 -Compress))
[IO.Directory]::CreateDirectory($destination) | Out-Null
$archive = Join-Path $destination $registration[0].asset
& $Dotnet $EntryAssembly validate pack-backends --inventory $inventory --artifacts-path $stagingRoot --output $archive
if ($LASTEXITCODE) { throw "Failed to verify and pack backend files for $Target." }
Write-Output $archive
