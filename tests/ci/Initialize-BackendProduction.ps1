param(
    [Parameter(Mandatory)][string]$Target,
    [Parameter(Mandatory)][string]$HostTarget,
    [Parameter(Mandatory)][ValidateSet('consume','produce')][string]$Operation,
    [Parameter(Mandatory)][string]$OutputDirectory,
    [Parameter(Mandatory)][string]$NativeDirectory,
    [string]$PreviousZip,
    [string]$PreviousDigest,
    [switch]$PrepareNative,
    [string]$Dotnet = 'dotnet'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$root = [IO.Path]::GetFullPath('../..',$PSScriptRoot)
$platform = @(Get-Content -LiteralPath (Join-Path $root '.github/native-targets.json') -Raw | ConvertFrom-Json | Where-Object target -CEQ $Target)
if ($platform.Count -ne 1) { throw "Unknown platform $Target." }
$revision = (& git -C $root rev-parse HEAD).Trim()
$output = [IO.Directory]::CreateDirectory([IO.Path]::GetFullPath($OutputDirectory)).FullName
if ($PreviousZip) {
    if ($PreviousDigest -cnotmatch '^[a-f0-9]{64}$' -or (Get-FileHash -LiteralPath $PreviousZip).Hash.ToLowerInvariant() -cne $PreviousDigest) {
        throw 'Backend producer seed differs from the catalog.'
    }
    [IO.File]::Copy([IO.Path]::GetFullPath($PreviousZip),(Join-Path $output $platform[0].asset),$false)
}
if ($Operation -ceq 'consume') {
    if (!$PreviousZip) { throw 'A consumption job needs a certified archive.' }
    [IO.File]::WriteAllText((Join-Path $output 'candidate.json'), (ConvertTo-Json ([ordered]@{
        target=$Target;asset=$platform[0].asset;commit=$revision;sha256=$PreviousDigest;reused=$true
    })))
    if (!$PrepareNative) { return }
}
$dotnetPath = (Get-Command $Dotnet -CommandType Application | Select-Object -First 1).Source
$work = [IO.Directory]::CreateDirectory((Join-Path $root 'artifacts/bn')).FullName
$entry = @(& "$PSScriptRoot/Initialize-ValidationEntry.ps1" -Dotnet $dotnetPath -LogPath "$work/bootstrap.log")
$previous = $null
if ($PreviousZip) {
    $previous = Join-Path $work 'prior'
    & $dotnetPath $entry[0] validate import-backends --archive $PreviousZip --sha256 $PreviousDigest --target $Target --output $previous
    if ($LASTEXITCODE) { throw 'The native producer seed could not be verified.' }
}
if ($Operation -ceq 'consume') {
    $directory = [IO.Directory]::CreateDirectory([IO.Path]::GetFullPath($NativeDirectory)).FullName
    $selection = Join-Path $previous 'native/selection.json'
    [IO.File]::Copy($selection,(Join-Path $directory 'selection.json'),$false)
    $catalog = Get-Content -LiteralPath $selection -Raw | ConvertFrom-Json
    foreach ($item in @($catalog.assets) + @($catalog.aggregates)) {
        if ($item.asset -cnotmatch '^[A-Za-z0-9._-]+\.zip$' -or $item.sha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'Invalid host native tool inventory.' }
        $destination = Join-Path $directory $item.asset
        if (!(Test-Path -LiteralPath $destination)) {
            [IO.File]::Copy((Join-Path $previous "native/$($item.sha256).zip"),$destination,$false)
        }
    }
    return
}
$null = & "$PSScriptRoot/Initialize-NativeDependencyWorkloads.ps1" -Targets @($Target) -Dotnet $dotnetPath
$project = Join-Path $root 'entry/InnoEngine.Build/InnoEngine.Build.csproj'
$sdk = Join-Path $work 'sdk'
$properties = @()
if ($previous -and $HostTarget -ceq $Target) {
    $plan = Join-Path $work 'identity'
    & $dotnetPath $entry[0] validate plan --engine-root $root --dotnet $dotnetPath --base HEAD --profile fast --backend-store artifacts/bn/store --output $plan
    if ($LASTEXITCODE) { throw 'Could not evaluate producer bootstrap owners.' }
    $capture = Join-Path $work 'managed.json'
    & $dotnetPath $entry[0] validate backend-inputs --engine-root $root --dotnet $dotnetPath --target $Target --plan "$plan/plan.json" `
        --selection "$previous/native/selection.json" --output $capture
    if ($LASTEXITCODE) { throw 'Could not evaluate the managed producer bootstrap.' }
    $current = (Get-Content -LiteralPath $capture -Raw | ConvertFrom-Json -AsHashtable).projects
    $matching = @((Get-Content -LiteralPath "$previous/manifest.json" -Raw | ConvertFrom-Json).files | Where-Object {
        $_.configuration -ceq 'Debug' -and $_.path -clike 'managed/*' -and $current[$_.owner] -ceq $_.input
    } | ForEach-Object owner | Sort-Object -Unique)
    $changed = @($current.Keys | Where-Object { $_ -cnotin $matching })
    $properties = @("-p:InnoBackendPrebuiltRoot=$previous","-p:InnoBackendExportRoot=$work/bootstrap-outputs",
        "-p:InnoBackendExportArtifactsPath=$sdk",("-p:InnoBackendCompileOwners=|" + ($changed -join '|') + '|'))
}
& $dotnetPath build $project --configuration Debug --artifacts-path $sdk --disable-build-servers -m:2 -nr:false @properties
if ($LASTEXITCODE) { throw 'The producer Build entry failed.' }
$buildEntry = @(& $dotnetPath msbuild $project -nologo -p:Configuration=Debug "-p:ArtifactsPath=$sdk" -getProperty:TargetPath)
if ($LASTEXITCODE -or $buildEntry.Count -ne 1) { throw 'The producer Build entry was not unique.' }
& "$PSScriptRoot/Prepare-BackendNativeInputs.ps1" -Target $Target -HostTarget $HostTarget -BuildEntry $buildEntry[0] -Dotnet $dotnetPath `
    -Directory $NativeDirectory -PreviousDirectory $previous
