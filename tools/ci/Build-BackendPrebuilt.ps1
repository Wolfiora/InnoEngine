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
    [string]$PreviousTag,
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
    if ($Arguments[0] -ceq 'build') {
        # Each SDK invocation owns one task bootstrap shared by its project roots.
        $Arguments += "-p:InnoTaskBootstrapOwner=$([Guid]::NewGuid().ToString('N'))"
        Write-Host "Building $($Arguments[1]) for $Target."
    }
    & $dotnetPath @Arguments | Out-Host
    if ($LASTEXITCODE) { throw "Backend production failed at $($Arguments[0]) $($Arguments[1])." }
}
function Invoke-BackendSolution([string]$Solution, [Collections.IDictionary]$Context, [string[]]$Options) {
    # Keep dependencies outside the partial solution in the same configuration as the planned SDK graph.
    $policy = $Context.properties.ShouldUnsetParentConfigurationAndPlatform
    Invoke-Sdk (@('build',$Solution,'-m:1','-p:Platform=Any CPU',"-p:ShouldUnsetParentConfigurationAndPlatform=$policy") + $Options)
}
$previous = $null
$manifest = $null
if ($PreviousPackage -or $PreviousSha256) {
    if (!$PreviousPackage -or $PreviousSha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'Prior production must have an exact ZIP and digest.' }
    $previous = Join-Path $work 'previous'
    Invoke-Sdk @($entry,'validate','import-backends','--archive',$PreviousPackage,'--sha256',$PreviousSha256,'--target',$Target,'--output',$previous)
    $manifest = Get-Content -LiteralPath (Join-Path $previous 'manifest.json') -Raw | ConvertFrom-Json
}
$trustedChecks = @()
if ($PreviousTag) {
    if (!$previous -or $PreviousTag -cnotmatch '^backend-prebuilts-\d{4}\.\d{2}\.\d{2}\.[1-9]\d*$') { throw 'Invalid producer seed catalog.' }
    . "$PSScriptRoot/BackendReleaseCatalog.ps1"
    $base = "$env:GITHUB_API_URL/repos/$env:GITHUB_REPOSITORY"
    $headers = @{Authorization="Bearer $env:GH_TOKEN";Accept='application/vnd.github+json'}
    $release = Invoke-RestMethod -Headers $headers -Uri "$base/releases/tags/$PreviousTag"
    $prior = (Get-BackendCatalog $base $headers $release).platforms[$Target]
    if ($prior.sha256 -cne $PreviousSha256) { throw 'The seed proof belongs to another ZIP.' }
    if ($prior.Contains('consumers')) { $trustedChecks = @($prior.consumers) }
}
$staging = Join-Path $work 'staging'
$nativeInventory = Get-Content -LiteralPath $native -Raw | ConvertFrom-Json
function Invoke-PlayerSupportPack([string]$Phase, [string]$Package, [string[]]$ChangedOwners, [string]$Selection) {
    # Use the platform's real publication path so static linkage, RID, binding selection and
    # strip-symbol options are those used by engine releases. Its publisher owns the read leases.
    $project = Join-Path $root 'entry/InnoEngine.Build/InnoEngine.Build.csproj'
    $bootstrap = Join-Path $work 'cli'
    if ($Phase -ceq 'produce') {
        Invoke-Sdk @('build',$project,'--configuration','Release','--artifacts-path',$bootstrap,'--disable-build-servers','-m:2','-nr:false',
            "-p:InnoBackendPrebuiltRoot=$staging")
    }
    $cli = @(& $dotnetPath msbuild $project -nologo -p:Configuration=Release "-p:ArtifactsPath=$bootstrap" -getProperty:TargetPath)
    if ($LASTEXITCODE -or $cli.Count -ne 1) { throw 'The Support Pack producer entry could not be located.' }
    $info = [Diagnostics.ProcessStartInfo]::new($dotnetPath)
    $info.WorkingDirectory = $root
    $info.UseShellExecute = $false
    foreach ($argument in @($cli[0],'support-pack','--engine-root',$root,'--configuration','Release','--target',$Target,'--tools-target',$HostTarget,
        '--native-dependencies',$Selection,'--dotnet',$dotnetPath,'--output',(Join-Path $work "pack-$Phase"))) { $info.ArgumentList.Add($argument) }
    if ($Package) { $info.Environment['InnoBackendPrebuiltRoot'] = $Package }
    if ($Phase -ceq 'produce') {
        # The Support Pack owns a managed source snapshot. Its generator and composition
        # outputs use SDK project directories inside that snapshot.
        $info.Environment['InnoBackendExportProductTarget'] = $Target
        $info.Environment['InnoBackendExportRoot'] = $staging
        $info.Environment['InnoBackendCompileOwners'] = '|' + ($ChangedOwners -join '|') + '|'
    }
    $process = [Diagnostics.Process]::Start($info)
    try {
        $process.WaitForExit()
        if ($process.ExitCode) { throw "The platform's $Phase Support Pack failed for $Target." }
    } finally { $process.Dispose() }
}
$production = [ordered]@{}
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
    $sdkOptions = @('--artifacts-path',$sdk,"-p:InnoBackendExportArtifactsPath=$sdk")
    $options = @('--configuration',$configuration,'--disable-build-servers','-nr:false',
        "-p:InnoNativeDependencyManifest=$consumption","-p:InnoBackendExportRoot=$staging")
    if ($previous) {
        $options += @("-p:InnoBackendPrebuiltRoot=$previous",("-p:InnoBackendCompileOwners=|" + ($changed -join '|') + '|'))
    }
    $frozen = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json -AsHashtable -Depth 100
    $solution = Join-Path $planDirectory 'Backends.slnx'
    $owners = foreach ($owner in $current.Keys | Sort-Object -CaseSensitive) {
        $path = [Security.SecurityElement]::Escape([IO.Path]::GetFullPath($owner,$root))
        "  <Project Path=`"$path`" />"
    }
    [IO.File]::WriteAllLines($solution, [string[]](@('<Solution>') + $owners + @('</Solution>')))
    Invoke-BackendSolution $solution $frozen.contexts.managed ($options + $sdkOptions)
    # Validation builds standalone SDK roots under the development product properties as well
    # as the product entry point, whose references receive additional binding selections.
    if ($identity.developmentProperties) {
        $development = [IO.Path]::GetFullPath($identity.developmentProperties,$root)
        Invoke-BackendSolution $solution $frozen.contexts.managed ($options + $sdkOptions + @(
            "-p:InnoNativeTarget=$Target","-p:InnoToolTarget=$HostTarget","-p:InnoProductBuildProperties=$development"))
    }
    $products = @($frozen.contexts.managed.sdkGraph | Where-Object { $_.Properties.InnoProductTarget -ceq $Target -and $_.Properties.InnoProductId })
    if (!$products.Count) { throw "The SDK graph declares no products for $Target." }
    foreach ($product in $products) {
        $path = $product.Path.Replace('${workspace}', $root)
        $properties = @(& $dotnetPath msbuild $path -nologo "-p:Configuration=$configuration" -getProperty:InnoProductBuildProperties)
        if ($LASTEXITCODE -or $properties.Count -ne 1 -or !(Test-Path -LiteralPath $properties[0] -PathType Leaf)) {
            throw "Missing declared product build properties for $path."
        }
        $id = $product.Properties.InnoProductId
        $aggregate = @($nativeInventory.aggregates | Where-Object { $_.product -ceq $id -and $_.target -ceq $Target -and $_.configuration -ieq $configuration })
        if (!$aggregate.Count) {
            Invoke-Sdk (@('build',$path,'-m:2') + $options + $sdkOptions + @("-p:InnoNativeTarget=$Target","-p:InnoToolTarget=$HostTarget",
                "-p:InnoProductBuildProperties=$($properties[0])"))
        }
        if ($id -ceq 'player' -and $configuration -ceq 'Release') { Invoke-PlayerSupportPack 'produce' $previous $changed $consumption }
        elseif ($aggregate.Count) { throw "Missing declared aggregate SDK entry for $id/$configuration." }
    }
    $production[$configuration] = [ordered]@{sourceOwners=$changed;prebuiltOwners=@($matching | Sort-Object -CaseSensitive)}
    Write-Host "$Target/$configuration selected $($changed.Count) backend owners for compilation and $($matching.Count) for prebuilt consumption."
    $inputs
})
$planList = Join-Path $work 'input-plans.json'
[IO.File]::WriteAllText($planList, (ConvertTo-Json -InputObject $inputPlans -Compress))
$archive = Join-Path $OutputDirectory $platform[0].asset
Invoke-Sdk @($entry,'validate','assemble-backends','--artifacts-path',$staging,'--plans',$planList,'--selection',$native,'--target',$Target,'--output',$archive)
$digest = (Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant()
$coldStore = [IO.Directory]::CreateDirectory((Join-Path $work 'certified')).FullName
$installed = Join-Path $coldStore $Target
Invoke-Sdk @($entry,'validate','import-backends','--archive',$archive,'--sha256',$digest,'--target',$Target,'--output',$installed)
$packInputs = Get-Content -LiteralPath (Join-Path $installed 'native/selection.json') -Raw | ConvertFrom-Json -AsHashtable -Depth 80
$coldInputs = [ordered]@{layout='platform-packages';assets=@($packInputs.assets);aggregates=@($packInputs.aggregates)}
if ($HostTarget -cne $Target) {
    $source = Get-Content -LiteralPath $consumption -Raw | ConvertFrom-Json -AsHashtable -Depth 80
    $hostDirectory = [IO.Directory]::CreateDirectory((Join-Path $coldStore "$HostTarget/native")).FullName
    foreach ($family in @('assets','aggregates')) {
        foreach ($item in $source[$family] | Where-Object { $_.target -ceq $HostTarget }) {
            if ($item.sha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'Invalid host tool digest.' }
            $file = "$($item.sha256).zip"
            $origin = Join-Path (Split-Path $consumption) "$HostTarget/native/$file"
            $destination = Join-Path $hostDirectory $file
            if (!(Test-Path -LiteralPath $destination)) {
                if ((Get-FileHash -LiteralPath $origin).Hash.ToLowerInvariant() -cne $item.sha256) { throw 'Host tool archive changed.' }
                New-Item -ItemType HardLink -Path $destination -Target $origin | Out-Null
            }
            $coldInputs[$family] += $item
        }
    }
}
$coldSelection = Join-Path $coldStore 'selection.json'
[IO.File]::WriteAllText($coldSelection,(ConvertTo-Json $coldInputs -Depth 80 -Compress))
$payload = Get-Content -LiteralPath (Join-Path $installed 'manifest.json') -Raw | ConvertFrom-Json -AsHashtable
$checks = @(foreach ($configuration in $platform[0].configurations) {
    $plan = Get-Content -LiteralPath (Join-Path $work "plan-$configuration/plan.json") -Raw | ConvertFrom-Json -AsHashtable -Depth 100
    $sdk = Join-Path $work 'consume'
    $compilerPlan = Get-Content -LiteralPath (Join-Path $work "plan-$configuration/inputs.json") -Raw | ConvertFrom-Json -AsHashtable
    $checkedDevelopmentOwners = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($product in $plan.contexts.managed.sdkGraph | Where-Object { $_.Properties.InnoProductTarget -ceq $Target -and $_.Properties.InnoProductId }) {
        $project = $product.Path.Replace('${workspace}', $root)
        $properties = @(& $dotnetPath msbuild $project -nologo "-p:Configuration=$configuration" -getProperty:InnoProductBuildProperties)
        if ($LASTEXITCODE -or $properties.Count -ne 1) { throw 'Consumer product properties could not be evaluated.' }
        $clock = [Diagnostics.Stopwatch]::StartNew()
        $id = $product.Properties.InnoProductId
        $aggregate = @($nativeInventory.aggregates | Where-Object { $_.product -ceq $id -and $_.target -ceq $Target -and $_.configuration -ieq $configuration })
        $relative = [IO.Path]::GetRelativePath($root,$project).Replace('\','/')
        $consumer = $compilerPlan.consumers[$relative]
        if (!$consumer -or $consumer.identity -cnotmatch '^[a-f0-9]{64}$') { throw "Missing SDK consumption plan for $relative." }
        $facts = [Collections.Generic.SortedDictionary[string,string]]::new([StringComparer]::Ordinal)
        $facts['entry'] = $consumer.identity
        $facts['host'] = $HostTarget
        $facts['registration'] = ConvertTo-Json $platform[0] -Compress
        foreach ($file in $payload.files | Where-Object { $_.configuration -ceq $configuration -and $_.owner -cin $consumer.backendOwners }) {
            $facts["file/"+$file.path] = $file.sha256
        }
        $request = @($packInputs.requests | Where-Object { $_.product -ceq $id -and $_.target -ceq $Target -and $_.configuration -ieq $configuration })
        if ($request.Count -gt 1) { throw 'Duplicate cold-consumption product declaration.' }
        foreach ($family in @('assets','aggregates')) {
            foreach ($item in $coldInputs[$family]) {
                if ($request.Count -eq 1 -and $item.target -ceq $Target -and $item.sha256 -cnotin $request[0].digests) { continue }
                $facts["native/$family/"+$item.sha256] = $item.sha256
            }
        }
        $pack = $id -ceq 'player' -and $configuration -ceq 'Release'
        if (!$aggregate.Count) {
            $sourceArgs = @('-nologo',"-p:Configuration=$configuration","-p:ArtifactsPath=$(Join-Path $work 'sdk')",
                "-p:InnoNativeTarget=$Target","-p:InnoToolTarget=$HostTarget","-p:InnoProductBuildProperties=$($properties[0])")
            $out = @(& $dotnetPath msbuild $project @sourceArgs -getProperty:TargetDir)
            if ($LASTEXITCODE -or $out.Count -ne 1 -or !(Test-Path -LiteralPath $out[0] -PathType Container)) { throw 'Missing built SDK product outputs.' }
            $outputs = @(Get-ChildItem -LiteralPath $out[0] -File -Recurse)
            if (!$outputs.Count) { throw 'The SDK produced no product files.' }
            foreach ($file in $outputs) {
                $facts['output/'+[IO.Path]::GetRelativePath($out[0],$file.FullName).Replace('\','/')] = (Get-FileHash -LiteralPath $file.FullName).Hash
            }
        }
        $identityBytes = [Text.Encoding]::UTF8.GetBytes((ConvertTo-Json $facts -Compress))
        $contract = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($identityBytes)).ToLowerInvariant()
        $priorChecks = @($trustedChecks | Where-Object { $_.configuration -ceq $configuration -and $_.project -ceq $relative -and $_.product -ceq $id -and $_.identity -ceq $contract })
        if ($priorChecks.Count -gt 1) { throw 'Duplicate trusted product evidence.' }
        $reuse = $priorChecks.Count -eq 1
        if (!$reuse -and $compilerPlan.developmentProperties) {
            $roots = @($consumer.backendOwners | Where-Object { $checkedDevelopmentOwners.Add($_) })
            if ($roots.Count) {
                $solution = Join-Path $work "plan-$configuration/Consume-$id.slnx"
                $entries = foreach ($owner in $roots) {
                    $path = [Security.SecurityElement]::Escape([IO.Path]::GetFullPath($owner,$root))
                    "  <Project Path=`"$path`" />"
                }
                [IO.File]::WriteAllLines($solution,[string[]](@('<Solution>') + @($entries) + @('</Solution>')))
                $development = [IO.Path]::GetFullPath($compilerPlan.developmentProperties,$root)
                Invoke-BackendSolution $solution $plan.contexts.managed @(
                    '--configuration',$configuration,'--artifacts-path',(Join-Path $work 'consume-development'),'--disable-build-servers','-nr:false',
                    "-p:InnoNativeDependencyManifest=$coldSelection","-p:InnoBackendPrebuiltRoot=$installed","-p:InnoNativeTarget=$Target",
                    "-p:InnoToolTarget=$HostTarget","-p:InnoProductBuildProperties=$development")
            }
        }
        if (!$reuse -and !$aggregate.Count) {
            Invoke-Sdk @('build',$project,'--configuration',$configuration,'--artifacts-path',$sdk,'--disable-build-servers','-m:2','-nr:false',
                "-p:InnoNativeDependencyManifest=$coldSelection","-p:InnoBackendPrebuiltRoot=$installed","-p:InnoNativeTarget=$Target",
                "-p:InnoToolTarget=$HostTarget","-p:InnoProductBuildProperties=$($properties[0])")
        }
        if (!$reuse -and $pack) { Invoke-PlayerSupportPack 'consume' $installed @() $coldSelection }
        [ordered]@{configuration=$configuration;product=$id;project=$relative;seconds=$clock.Elapsed.TotalSeconds;identity=$contract;
            source=$(if ($reuse) { $PreviousTag } else { 'executed' })}
    }
})
$receipt = [ordered]@{version=2;commit=$identity.commit;target=$Target;sha256=$digest;bytes=[IO.FileInfo]::new($archive).Length;production=$production;checks=$checks}
[IO.File]::WriteAllText((Join-Path $OutputDirectory 'consumption.json'),(ConvertTo-Json $receipt -Depth 8))
$archive
