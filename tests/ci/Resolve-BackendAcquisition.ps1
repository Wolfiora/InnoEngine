param(
    [Parameter(Mandatory)][string]$PlanPath,
    [Parameter(Mandatory)][string]$EntryAssembly,
    [Parameter(Mandatory)][string]$OutputDirectory,
    [string]$Repository = $env:GITHUB_REPOSITORY,
    [string]$ApiUrl = 'https://api.github.com',
    [string]$Dotnet = 'dotnet'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. "$PSScriptRoot/BackendReleaseCatalog.ps1"
$root = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$plan = Get-Content -LiteralPath $PlanPath -Raw | ConvertFrom-Json -AsHashtable -Depth 100
if (!$plan.backendStore -or $null -ne $plan.backendPackages) { throw 'Backend acquisition requires an unbound CI plan.' }
$entry = (Get-Item -LiteralPath $EntryAssembly).FullName
$dotnetPath = (Get-Command $Dotnet -CommandType Application | Select-Object -First 1).Source
$directory = [IO.Directory]::CreateDirectory([IO.Path]::GetFullPath($OutputDirectory)).FullName
$targets = @(Get-Content -LiteralPath (Join-Path $root '.github/native-targets.json') -Raw | ConvertFrom-Json)
function Get-CurrentInputs($Target, $Configuration, $Selection) {
    $output = Join-Path $directory "$Target-$Configuration.json"
    $arguments = @('validate','backend-inputs','--plan',$PlanPath,'--engine-root',$root,'--dotnet',$dotnetPath,
        '--target',$Target,'--configuration',$Configuration,'--output',$output)
    if ($Selection) { $arguments += @('--selection',$Selection) }
    & $dotnetPath $entry @arguments | Out-Host
    if ($LASTEXITCODE) { throw "Could not evaluate frozen backend inputs for $Target/$Configuration." }
    Get-Content -LiteralPath $output -Raw | ConvertFrom-Json -AsHashtable
}
$required = [ordered]@{}
foreach ($registration in $targets) {
    $target = $registration.target
    $requests = @($plan.nativeRequests | Where-Object target -CEQ $target)
    if ($target -cne $plan.backendHost -and !$requests.Count) { continue }
    $configurations = @($requests | ForEach-Object configuration)
    if ($target -ceq $plan.backendHost) {
        $configurations += @($plan.contexts.Values | ForEach-Object { $_.properties.Configuration })
        $configurations += $plan.configuration
    }
    $plans = [ordered]@{}
    foreach ($configuration in $configurations | Sort-Object -Unique) {
        $capture = Get-CurrentInputs $target $configuration $null
        if ($capture.required.Count -or @($requests | Where-Object configuration -CEQ $configuration).Count) {
            $plans[$configuration] = $capture
        }
    }
    if ($plans.Count) { $required[$target] = $plans }
}
if (@($plan.nativeRequests | Where-Object { $_.target -cnotin $targets.target }).Count) { throw 'A native request has no registered backend target.' }
$acquisitions = [ordered]@{}
foreach ($target in $required.Keys) {
    $changes = [ordered]@{}
    foreach ($configuration in $required[$target].Keys) { $changes[$configuration] = @($required[$target][$configuration].required) }
    $acquisitions[$target] = [ordered]@{target=$target; operation='produce'; tag=$null; assetId=$null; sha256=$null; changed=$changes}
}
if ($required.Count) {
    if ($Repository -cnotmatch '^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$') { throw 'Backend discovery requires a repository identity.' }
    $base = "$ApiUrl/repos/$Repository"
    $headers = @{Accept='application/vnd.github+json'; 'X-GitHub-Api-Version'='2022-11-28'}
    if ($env:GH_TOKEN) { $headers.Authorization = "Bearer $env:GH_TOKEN" }
    $releases = @(for ($page = 1; ; $page++) {
        $batch = @(Invoke-RestMethod -Uri "$base/releases?per_page=100&page=$page" -Headers $headers | Write-Output)
        $batch | Where-Object { !$_.draft -and $_.prerelease -and $_.tag_name -cmatch '^backend-prebuilts-\d{4}\.\d{2}\.\d{2}\.[1-9]\d*$' }
        if ($batch.Count -lt 100) { break }
    })
    foreach ($release in $releases | Sort-Object -Property published_at,id -Descending) {
        $catalog = Get-BackendCatalog $base $headers $release
        $native = [ordered]@{
            assets = @($catalog.platforms.Values | ForEach-Object { $_.native.assets } | Where-Object { $_ })
            aggregates = @($catalog.platforms.Values | ForEach-Object { $_.native.aggregates } | Where-Object { $_ })
        }
        $selection = Join-Path $directory "$($release.tag_name).json"
        [IO.File]::WriteAllText($selection, (ConvertTo-Json $native -Compress -Depth 80))
        foreach ($target in $required.Keys) {
            if ($acquisitions[$target].operation -ceq 'consume' -or !$catalog.platforms.Contains($target)) { continue }
            $platform = $catalog.platforms[$target]
            $registration = $targets | Where-Object target -CEQ $target
            if ($platform.asset -cne $registration.asset) { throw "Unexpected asset name for $target." }
            $changed = [ordered]@{}
            $cost = 0
            foreach ($configuration in $required[$target].Keys) {
                $current = Get-CurrentInputs $target $configuration $selection
                $owners = $platform.configurations[$configuration]
                $changed[$configuration] = @($current.required | Where-Object { !$owners -or $owners[$_] -cne $current.projects[$_] })
                $cost += $changed[$configuration].Count
            }
            foreach ($request in $plan.nativeRequests | Where-Object target -CEQ $target) {
                if (!@((@($platform.native.assets) + @($platform.native.aggregates)) | Where-Object {
                    $_.configuration -ieq $request.configuration -and $_.product -ceq $request.product
                }).Count) { $cost++ }
            }
            $currentBest = $acquisitions[$target]
            if ($currentBest.Contains('cost') -and $cost -ge $currentBest.cost) { continue }
            $asset = $release.assets | Where-Object name -CEQ $platform.asset
            $acquisitions[$target] = [ordered]@{target=$target; operation=$(if ($cost) { 'produce' } else { 'consume' })
                tag=$release.tag_name; assetId=$asset.id; sha256=$platform.sha256; changed=$changed; cost=$cost}
        }
        if (!@($acquisitions.Values | Where-Object operation -CEQ 'produce').Count) { break }
    }
}
$decision = [ordered]@{commit=$plan.commit; packages=@($acquisitions.Values)}
[IO.File]::WriteAllText((Join-Path $directory 'acquisition.json'), (ConvertTo-Json $decision -Depth 12))
$decision
