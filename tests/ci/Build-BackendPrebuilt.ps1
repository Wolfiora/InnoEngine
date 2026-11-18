param(
    [Parameter(Mandatory)][string]$Target,
    [Parameter(Mandatory)][string]$HostTarget,
    [Parameter(Mandatory)][string]$EntryAssembly,
    [Parameter(Mandatory)][string]$NativeSelection,
    [Parameter(Mandatory)][string]$ConsumptionSelection,
    [Parameter(Mandatory)][string]$WorkspaceDirectory,
    [Parameter(Mandatory)][string]$OutputDirectory,
    [string]$PreviousPackage,
    [string]$PreviousSha256,
    [string]$Dotnet = 'dotnet'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$root = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$dotnetPath = (Get-Command $Dotnet -CommandType Application | Select-Object -First 1).Source
$entry = (Get-Item -LiteralPath $EntryAssembly).FullName
$native = (Get-Item -LiteralPath $NativeSelection).FullName
$consumption = (Get-Item -LiteralPath $ConsumptionSelection).FullName
$work = [IO.Path]::GetFullPath($WorkspaceDirectory, $root)
$boundary = [IO.Path]::GetFullPath('artifacts', $root) + [IO.Path]::DirectorySeparatorChar
if (!$work.StartsWith($boundary, [StringComparison]::Ordinal) -or (Test-Path -LiteralPath $work)) {
    throw 'Backend production requires a new staging owner beneath repository artifacts.'
}
$null = [IO.Directory]::CreateDirectory($work)
$registry = @(Get-Content -LiteralPath (Join-Path $root '.github/native-targets.json') -Raw | ConvertFrom-Json)
$platform = @($registry | Where-Object target -CEQ $Target)
if ($platform.Count -ne 1 -or $HostTarget -cnotin $registry.target) { throw 'Unknown backend producer target or host.' }
function Invoke-Sdk([string[]]$Arguments) {
    & $dotnetPath @Arguments | Out-Host
    if ($LASTEXITCODE) { throw "Backend production failed at $($Arguments[0]) $($Arguments[1])." }
}
$previous = $null
$manifest = $null
if ($PreviousPackage -or $PreviousSha256) {
    if (!$PreviousPackage -or $PreviousSha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'Prior production must have an exact ZIP and digest.' }
    $previous = Join-Path $work 'previous'
    Invoke-Sdk @($entry,'validate','import-backends','--archive',$PreviousPackage,'--sha256',$PreviousSha256,'--target',$Target,'--output',$previous)
    $manifest = Get-Content -LiteralPath (Join-Path $previous 'manifest.json') -Raw | ConvertFrom-Json
}
$staging = Join-Path $work 'staging'
$inputPlans = @(foreach ($configuration in $platform[0].configurations) {
    $planDirectory = Join-Path $work "plan-$configuration"
    Invoke-Sdk @($entry,'validate','plan','--engine-root',$root,'--base','HEAD','--profile','fast','--dotnet',$dotnetPath,
        '--configuration',$configuration,'--backend-store',(Join-Path $work 'identity-store'),'--output',$planDirectory)
    $planPath = Join-Path $planDirectory 'plan.json'
    $inputs = Join-Path $planDirectory 'inputs.json'
    Invoke-Sdk @($entry,'validate','backend-inputs','--engine-root',$root,'--dotnet',$dotnetPath,'--plan',$planPath,'--target',$Target,
        '--selection',$consumption,'--output',$inputs)
    $identity = Get-Content -LiteralPath $inputs -Raw | ConvertFrom-Json -AsHashtable
    $current = $identity.projects
    if (!$current.Count) { throw "Empty backend project discovery for $Target/$configuration." }
    $matching = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    if ($manifest) {
        foreach ($file in $manifest.files) {
            if ($file.configuration -ceq $configuration -and $file.path -clike 'managed/*' -and $current[$file.owner] -ceq $file.input) {
                $null = $matching.Add($file.owner)
            }
        }
    }
    $changed = @($current.Keys | Where-Object { !$matching.Contains($_) } | Sort-Object -CaseSensitive)
    $sdk = Join-Path $work 'sdk'
    $options = @('--configuration',$configuration,'--artifacts-path',$sdk,'--disable-build-servers','-m:2','-nr:false',
        "-p:InnoNativeDependencyManifest=$consumption","-p:InnoBackendExportRoot=$staging","-p:InnoBackendExportArtifactsPath=$sdk")
    if ($previous) {
        $options += @("-p:InnoBackendPrebuiltRoot=$previous",("-p:InnoBackendCompileOwners=|" + ($changed -join '|') + '|'))
    }
    $solution = Join-Path $planDirectory 'Backends.slnx'
    $owners = foreach ($owner in $current.Keys | Sort-Object -CaseSensitive) {
        $path = [Security.SecurityElement]::Escape([IO.Path]::GetFullPath($owner,$root))
        "  <Project Path=`"$path`" />"
    }
    [IO.File]::WriteAllLines($solution, [string[]](@('<Solution>') + $owners + @('</Solution>')))
    Invoke-Sdk (@('build',$solution) + $options)
    $frozen = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json -AsHashtable -Depth 100
    $products = @($frozen.contexts.managed.sdkGraph | Where-Object { $_.Properties.InnoProductTarget -ceq $Target -and $_.Properties.InnoProductId })
    if (!$products.Count) { throw "The SDK graph declares no products for $Target." }
    foreach ($product in $products) {
        $path = $product.Path.Replace('${workspace}', $root)
        $properties = @(& $dotnetPath msbuild $path -nologo "-p:Configuration=$configuration" -getProperty:InnoProductBuildProperties)
        if ($LASTEXITCODE -or $properties.Count -ne 1 -or !(Test-Path -LiteralPath $properties[0] -PathType Leaf)) {
            throw "Missing declared product build properties for $path."
        }
        Invoke-Sdk (@('build',$path) + $options + @("-p:InnoNativeTarget=$Target","-p:InnoToolTarget=$HostTarget",
            "-p:InnoProductBuildProperties=$($properties[0])"))
    }
    Write-Host "$Target/$configuration compiled $($changed.Count) backend owners and used $($matching.Count) certified owners."
    $inputs
})
& "$PSScriptRoot/New-BackendPrebuiltPackage.ps1" -Staging $staging -InputPlans $inputPlans -NativeSelection $native -Target $Target `
    -EntryAssembly $entry -Dotnet $dotnetPath -OutputDirectory $OutputDirectory
