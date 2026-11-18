param(
    [Parameter(Mandatory)][string]$Directory,
    [Parameter(Mandatory)][object[]]$Targets,
    [Parameter(Mandatory)][string]$Platform,
    [Parameter(Mandatory)][string]$ProducerRevision,
    [Parameter(Mandatory)][string]$Release,
    [string]$Dotnet = 'dotnet',
    [string]$AcceptanceRevision = $ProducerRevision
)
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$received = [IO.Path]::GetFullPath($Directory, $root)
$registrations = @(Get-Content (Join-Path $root '.github/native-targets.json') -Raw | ConvertFrom-Json)
if ($ProducerRevision -cnotmatch '^[a-f0-9]{40}$' -or $Release -cnotmatch '^native-deps-[A-Za-z0-9._-]+$' -or
    $Targets.Count -eq 0 -or @($Targets.target | Sort-Object -Unique).Count -ne $Targets.Count) { throw 'Native certification requires exact unique source and target identities.' }
foreach ($target in $Targets) {
    if ($target.target -cnotmatch '^[a-z0-9][a-z0-9-]*$' -or @($registrations | Where-Object { $_.target -ceq $target.target -and $_.runner -ceq $target.runner }).Count -ne 1) {
        throw 'Native certification target and execution host do not match their registration.'
    }
}
$owned = @($Targets | Where-Object runner -CEQ $Platform)
if ($owned.Count -eq 0) { throw 'The acceptance host has no selected producer target.' }
$fragments = @($Targets | ForEach-Object { Join-Path $received "native-dependency-$($_.target)/native-artifacts.json" })
$selectionPath = Join-Path $root 'build/dependencies/native-artifacts.json'
& "$PSScriptRoot/Merge-NativeDependencySelection.ps1" -Fragments $fragments -BaselinePath $selectionPath -OutputPath $selectionPath `
    -ProducerRevision $ProducerRevision -Release $Release
$downloads = Join-Path $root 'artifacts/dependencies/downloads'
New-Item -ItemType Directory -Path $downloads -Force | Out-Null
foreach ($fragment in $fragments) {
    $selection = Get-Content $fragment -Raw | ConvertFrom-Json
    foreach ($asset in @($selection.assets) + @($selection.aggregates)) {
        if ($asset.asset -cnotmatch '^[A-Za-z0-9._-]+\.zip$' -or $asset.sha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'Produced dependency identity is invalid.' }
        $archive = Join-Path (Split-Path $fragment -Parent) $asset.asset
        if ((Get-FileHash $archive).Hash.ToLowerInvariant() -cne $asset.sha256) { throw 'Produced dependency bytes changed before acceptance.' }
        $destination = Join-Path $downloads ($asset.sha256 + '.zip')
        if (-not (Test-Path $destination)) { Copy-Item -LiteralPath $archive -Destination $destination }
    }
}
$registered = @($registrations | Where-Object runner -CEQ $Platform)
$null = & "$PSScriptRoot/Initialize-NativeDependencyWorkloads.ps1" -Targets @($registered.target) -Dotnet $Dotnet
$entryOwner = Join-Path $root 'artifacts/validation/native-entry'
& $Dotnet build (Join-Path $root 'entry/InnoEngine.Build/InnoEngine.Build.csproj') --configuration Debug --artifacts-path $entryOwner --disable-build-servers -m:2 -nr:false
if ($LASTEXITCODE -ne 0) { throw 'Native consumer entry preparation failed.' }
$entry = @(& $Dotnet msbuild (Join-Path $root 'entry/InnoEngine.Build/InnoEngine.Build.csproj') -nologo -p:Configuration=Debug "-p:ArtifactsPath=$entryOwner" -getProperty:TargetPath)
if ($LASTEXITCODE -ne 0 -or $entry.Count -ne 1) { throw 'Native consumer entry discovery was not exact.' }
$evidence = Join-Path $root 'artifacts/validation/native-consumption'
& $Dotnet $entry[0] validate plan --engine-root $root --dotnet (Get-Command $Dotnet).Source --profile full --native-dependencies $selectionPath --output $evidence
if ($LASTEXITCODE -ne 0) { throw 'Native consumption planning failed.' }
$planPath = Join-Path $evidence 'plan.json'
$plan = Get-Content $planPath -Raw | ConvertFrom-Json
foreach ($target in $owned.target) {
    if ($target -cnotin $plan.nativeRequests.target) { throw "The complete host plan does not exercise the produced target: $target" }
}
& "$PSScriptRoot/Invoke-NativeDependenciesCi.ps1" -Operation Download -EntryAssembly $entry[0] -Dotnet $Dotnet -PlanPath $planPath
& $Dotnet $entry[0] validate run --engine-root $root --dotnet (Get-Command $Dotnet).Source --plan $planPath --output $evidence
if ($LASTEXITCODE -ne 0) { throw 'Native package consumption failed; no release is publishable.' }
$resultsPath = Join-Path $evidence 'results.json'
$receipt = & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath $planPath -ResultsPath $resultsPath -ExpectedCommit $AcceptanceRevision -JobConclusion success
if (-not $receipt.full) { throw 'Native package consumption did not certify a full host plan.' }
foreach ($target in $owned.target) {
    $destination = Join-Path $root "artifacts/dependencies/certification/$target"
    New-Item -ItemType Directory -Path $destination -Force | Out-Null
    Copy-Item $planPath, $resultsPath $destination
    @{ commit = $ProducerRevision; target = $target; release = $Release; completed = $true
       selectionDigest = (Get-FileHash (Join-Path $received "native-dependency-$target/native-artifacts.json")).Hash.ToLowerInvariant()
       planIdentity = $receipt.planIdentity; evidenceDigest = (Get-FileHash $resultsPath).Hash.ToLowerInvariant() } |
        ConvertTo-Json | Set-Content (Join-Path $destination 'native-producer.json')
}
