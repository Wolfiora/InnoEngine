param(
    [Parameter(Mandatory)][ValidateSet('Download', 'Produce')][string]$Operation,
    [Parameter(Mandatory)][string]$EntryAssembly,
    [string]$Dotnet = 'dotnet',
    [string]$PlanPath,
    [string]$Repository = $env:GITHUB_REPOSITORY,
    [string]$ApiUrl = 'https://api.github.com',
    [string]$Target,
    [string]$ToolsTarget,
    [string[]]$Products,
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Release',
    [string]$Release,
    [string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'
$root = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$EntryAssembly = [IO.Path]::GetFullPath($EntryAssembly, $root)
if (-not (Test-Path -LiteralPath $EntryAssembly -PathType Leaf)) { throw 'The prepared Build entry is missing.' }
if ($Operation -ceq 'Produce') {
    if (-not $Target -or -not $ToolsTarget -or -not $Products -or -not $OutputDirectory -or $Release -cnotmatch '^native-deps-[a-zA-Z0-9._-]+$') {
        throw 'Production requires explicit target, products, output owner and unique native-deps release tag.'
    }
    # The workflow owns the fresh checkout for cold production. This adapter never cleans existing native owners.
    foreach ($product in $Products) {
        & $Dotnet $EntryAssembly native-dependencies --operation produce --engine-root $root --dotnet $Dotnet `
            --tools-target $ToolsTarget --target $Target --product $product --configuration $Configuration --release $Release --output $OutputDirectory
        if ($LASTEXITCODE -ne 0) { throw "Native dependency production failed: $product/$Target/$Configuration." }
    }
    return
}
if (-not $PlanPath) { throw 'Dependency consumption requires the frozen validation plan.' }
$plan = Get-Content -LiteralPath $PlanPath -Raw | ConvertFrom-Json
if ($plan.root -cne '${workspace}' -and [IO.Path]::GetFullPath($plan.root) -ne [IO.Path]::GetFullPath($root)) { throw 'Dependency requests belong to another workspace.' }
$requests = @($plan.nativeRequests)
if ($requests.Count -eq 0) { return }
$toolTargets = @($plan.contexts.psobject.Properties | ForEach-Object { $_.Value.properties.InnoToolTarget } | Where-Object { $_ } | Sort-Object -Unique)
if ($toolTargets.Count -ne 1) { throw 'Native consumption requires one explicit host tools target in the frozen plan.' }
$ToolsTarget = $toolTargets[0]
if (-not $plan.nativeDependencies) { throw 'Native requests require an explicit consume-only selection.' }
$selection = [IO.Path]::GetFullPath($plan.nativeDependencies, $root)
$relative = [IO.Path]::GetRelativePath($root, $selection)
if ([IO.Path]::IsPathRooted($relative) -or $relative -match '^\.\.([\\/]|$)') { throw 'Dependency selection escaped the checkout.' }
$downloads = Join-Path $root 'artifacts/dependencies/downloads'
$reports = Join-Path (Split-Path ([IO.Path]::GetFullPath($PlanPath)) -Parent) 'dependency-selection'
New-Item -ItemType Directory -Path $downloads, $reports -Force | Out-Null
$assets = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::Ordinal)
$index = 0
foreach ($request in $requests) {
    $report = Join-Path $reports "request-$index.json"
    $index++
    & $Dotnet $EntryAssembly native-dependencies --operation select --engine-root $root --dotnet $Dotnet `
        --tools-target $ToolsTarget --target $request.target --configuration $request.configuration --product $request.product `
        --native-dependencies $selection --output $report
    if ($LASTEXITCODE -ne 0) { throw "Native dependency selection failed: $($request.product)/$($request.target). No producer fallback is permitted." }
    foreach ($asset in @(Get-Content -LiteralPath $report -Raw | ConvertFrom-Json)) {
        if ($asset.sha256 -cnotmatch '^[a-f0-9]{64}$' -or $asset.release -cnotmatch '^native-deps-[a-zA-Z0-9._-]+$' `
            -or $asset.asset -cnotmatch '^[a-zA-Z0-9._-]+\.zip$') { throw 'Native dependency transport received an invalid exact asset identity.' }
        if ($assets.ContainsKey($asset.sha256)) {
            $previous = $assets[$asset.sha256]
            if ($previous.asset -cne $asset.asset -or $previous.release -cne $asset.release) { throw 'One digest has conflicting release provenance.' }
        } else { $assets.Add($asset.sha256, $asset) }
    }
}
# All composition and source checks finish before downloading any archive.
$headers = @{ Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
if ($env:GH_TOKEN) { $headers.Authorization = "Bearer $env:GH_TOKEN" }
$releases = @{}
foreach ($asset in $assets.Values) {
    $destination = Join-Path $downloads ($asset.sha256 + '.zip')
    if (Test-Path -LiteralPath $destination) {
        if ((Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash.ToLowerInvariant() -cne $asset.sha256) {
            throw "The cached dependency archive is corrupt: $destination. Retained for diagnosis."
        }
        continue
    }
    if (-not $Repository -or $Repository -notmatch '^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$') { throw 'Dependency download requires the current repository identity.' }
    if (-not $releases.ContainsKey($asset.release)) {
        $tag = [Uri]::EscapeDataString($asset.release)
        $releaseData = Invoke-RestMethod -Uri "$ApiUrl/repos/$Repository/releases/tags/$tag" -Headers $headers
        if ($releaseData.draft -or $releaseData.tag_name -cne $asset.release) { throw 'A draft or different dependency release cannot be consumed.' }
        $releases[$asset.release] = $releaseData
    }
    $matches = @($releases[$asset.release].assets | Where-Object name -CEQ $asset.asset)
    if ($matches.Count -ne 1 -or $matches[0].state -cne 'uploaded') { throw "Missing published dependency asset: $($asset.release)/$($asset.asset)." }
    $partial = Join-Path $downloads ($asset.sha256 + '.' + [Guid]::NewGuid().ToString('N') + '.partial')
    $binaryHeaders = $headers.Clone()
    $binaryHeaders.Accept = 'application/octet-stream'
    Invoke-WebRequest -Uri "$ApiUrl/repos/$Repository/releases/assets/$($matches[0].id)" -Headers $binaryHeaders -OutFile $partial
    if ((Get-FileHash -LiteralPath $partial -Algorithm SHA256).Hash.ToLowerInvariant() -cne $asset.sha256) {
        throw "Downloaded dependency digest mismatch; partial archive retained at $partial."
    }
    [IO.File]::Move($partial, $destination, $false)
}
Write-Host "Selected $($assets.Count) exact native dependency archives. Third-party compilation is disabled."
