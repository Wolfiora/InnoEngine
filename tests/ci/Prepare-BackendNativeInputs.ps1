param(
    [Parameter(Mandatory)][string]$Target,
    [Parameter(Mandatory)][string]$HostTarget,
    [Parameter(Mandatory)][string]$BuildEntry,
    [Parameter(Mandatory)][string]$Directory,
    [string]$PreviousDirectory,
    [string]$Dotnet = 'dotnet'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$root = [IO.Path]::GetFullPath('../..',$PSScriptRoot)
$revision = (& git -C $root rev-parse HEAD).Trim()
if ($LASTEXITCODE -or $revision -cnotmatch '^[a-f0-9]{40}$') { throw 'Native production requires an exact source revision.' }
$registered = @(Get-Content -LiteralPath (Join-Path $root '.github/native-targets.json') -Raw | ConvertFrom-Json | Where-Object target -CEQ $Target)
if ($registered.Count -ne 1) { throw "Unknown producer platform $Target." }
$output = [IO.Path]::GetFullPath($Directory,$root)
if (Test-Path -LiteralPath $output) { throw 'Native production needs a fresh staging directory.' }
$null = [IO.Directory]::CreateDirectory($output)
function Invoke-Native([string[]]$Arguments) {
    & $Dotnet $BuildEntry native-dependencies --engine-root $root --tools-target $HostTarget --target $Target @Arguments | Out-Host
    if ($LASTEXITCODE) { throw "Native producer failed for $Target at $($Arguments -join ' ')." }
}
$prior = $null
if ($PreviousDirectory) {
    $installed = [IO.Path]::GetFullPath($PreviousDirectory)
    $metadata = Get-Content -LiteralPath (Join-Path $installed 'manifest.json') -Raw | ConvertFrom-Json
    if ($metadata.target -cne $Target) { throw 'The native seed belongs to a different target.' }
    $priorPath = Join-Path $installed 'native/selection.json'
    $prior = Get-Content -LiteralPath $priorPath -Raw | ConvertFrom-Json -AsHashtable -Depth 80
}
$productsPath = Join-Path $output 'products.json'
Invoke-Native @('--operation','describe','--output',$productsPath)
$products = @( (Get-Content -LiteralPath $productsPath -Raw | ConvertFrom-Json).products )
if (!$products.Count) { throw 'The platform declares no native product closures.' }
$selected = [ordered]@{assets=[ordered]@{};aggregates=[ordered]@{}}
# Capture logical product requests independently of per-component configuration overrides.
$imported = $false
$requests = [Collections.Generic.List[object]]::new()
foreach ($configuration in $registered[0].configurations) {
    foreach ($product in $products) {
        $arguments = @('--configuration',$configuration,'--product',$product)
        $reuse = $false
        if ($prior) {
            $report = Join-Path $output "inputs-$product-$configuration.json"
            Invoke-Native ($arguments + @('--operation','evaluate','--native-dependencies',$priorPath,'--output',$report))
            $evaluation = Get-Content -LiteralPath $report -Raw | ConvertFrom-Json
            $reuse = $evaluation.matched
        }
        if ($reuse) {
            $source = $prior
            $digests = @($evaluation.assets | ForEach-Object sha256)
        } else {
            # Rebuild permission exists only in the explicit producer. Seed its existing publisher
            # with verified generations so unchanged steps remain eligible for exact recipe reuse.
            if ($prior -and !$imported) {
                $payloads = @(@($prior.assets) + @($prior.aggregates) | ForEach-Object sha256 | Sort-Object -Unique)
                foreach ($digest in $payloads) {
                    if ($digest -cnotmatch '^[a-f0-9]{64}$') { throw 'Malformed native seed digest.' }
                    Invoke-Native @('--operation','import-cache','--input',(Join-Path $installed "native/$digest.zip"))
                }
                $imported = $true
            }
            Invoke-Native ($arguments + @('--operation','produce','--release',$revision,'--output',$output))
            Invoke-Native ($arguments + @('--operation','verify-bindings'))
            $fragment = Join-Path $output "$product-$Target-$($configuration.ToLowerInvariant()).json"
            $source = Get-Content -LiteralPath $fragment -Raw | ConvertFrom-Json -AsHashtable -Depth 80
            $digests = @(@($source.assets) + @($source.aggregates) | ForEach-Object sha256)
        }
        $closure = @($digests | Sort-Object -Unique -CaseSensitive)
        if (!$closure.Count) { throw "The producer selected an empty native closure for $product/$configuration." }
        foreach ($family in @('assets','aggregates')) {
            foreach ($item in $source[$family] | Where-Object { $_.target -ceq $Target -and $_.product -ceq $product -and $_.sha256 -cin $digests }) {
                if ($item.asset -cnotmatch '^[A-Za-z0-9._-]+\.zip$' -or $item.sha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'Invalid native producer asset.' }
                $key = ConvertTo-Json -InputObject @($product,$item.component,$item.configuration,$item.options) -Compress
                if ($selected[$family].Contains($key) -and $selected[$family][$key].sha256 -cne $item.sha256) {
                    throw "One native product closure selected conflicting bytes for $key."
                }
                $selected[$family][$key] = $item
                $file = Join-Path $output $item.asset
                if ($reuse -and !(Test-Path -LiteralPath $file)) {
                    [IO.File]::Copy((Join-Path $installed "native/$($item.sha256).zip"),$file,$false)
                }
                if ((Get-FileHash -LiteralPath $file).Hash.ToLowerInvariant() -cne $item.sha256) { throw 'Native bytes changed during assembly.' }
            }
        }
        $requests.Add([ordered]@{target=$Target; product=$product; configuration=$configuration; digests=$closure})
    }
}
$inventory = [ordered]@{requests=$requests.ToArray()}
foreach ($family in $selected.Keys) { $inventory[$family] = @($selected[$family].Keys | Sort-Object -CaseSensitive | ForEach-Object { $selected[$family][$_] }) }
[IO.File]::WriteAllText((Join-Path $output 'selection.json'), (ConvertTo-Json $inventory -Depth 80 -Compress))
