param(
    [string]$Dotnet = 'dotnet',
    [string]$Project = 'InnoEngine.sln',
    [string]$Configuration = 'SourceDebug',
    [string]$Target = '',
    [string]$RuntimeIdentifier = '',
    [string]$ProductProperties = '',
    [string]$ArtifactsPath = '',
    [switch]$PublishAot,
    [switch]$PublishTrimmed,
    [string]$ResultsDirectory = 'artifacts/acceptance/nuget-locks'
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path $PSScriptRoot -Parent
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$results = [IO.Path]::GetFullPath($ResultsDirectory, $repository)
New-Item -ItemType Directory -Force -Path $results | Out-Null
$selectedProject = [IO.Path]::GetFullPath($Project, $repository)
$properties = @('-p:Configuration=' + $Configuration)
if ($Target) { $properties += '-p:InnoNativeTarget=' + $Target }
if ($RuntimeIdentifier) { $properties += '-p:RuntimeIdentifier=' + $RuntimeIdentifier }
if ($ProductProperties) { $properties += '-p:InnoProductBuildProperties=' + [IO.Path]::GetFullPath($ProductProperties, $repository) }
if ($ArtifactsPath) { $properties += '-p:ArtifactsPath=' + [IO.Path]::GetFullPath($ArtifactsPath, $repository) }
if ($PublishAot) { $properties += '-p:PublishAot=true' }
if ($PublishTrimmed) { $properties += '-p:PublishTrimmed=true' }

function Invoke-SelectedSdk([string[]]$SdkArguments, [string]$LogName) {
    & $Dotnet @SdkArguments 2>&1 | Tee-Object -FilePath (Join-Path $results $LogName) | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "SDK operation failed: $LogName (exit $LASTEXITCODE)." }
}

function Get-SourceLockState {
    $state = @{}
    $paths = @(& git -C $repository -c core.quotepath=false ls-files --cached --others --exclude-standard -- '**/packages*.lock.json' 'packages*.lock.json')
    if ($LASTEXITCODE -ne 0) { throw 'Cannot enumerate source lock ownership.' }
    foreach ($path in $paths | Sort-Object -Unique) {
        $file = Join-Path $repository $path
        if (-not (Test-Path -LiteralPath $file -PathType Leaf)) { throw "A tracked lock file is missing: $path" }
        $state[$path] = @((Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash, (Get-Item -LiteralPath $file).LastWriteTimeUtc.Ticks)
    }
    return $state
}

$before = Get-SourceLockState
$graphPath = Join-Path $results 'restore.dg.json'
Push-Location $repository
try {
    Invoke-SelectedSdk (@('msbuild', $selectedProject, '-t:GenerateRestoreGraphFile', "-p:RestoreGraphOutputPath=$graphPath", '-m:1', '-nr:false', '-v:minimal') + $properties) 'graph.log'
    $graph = Get-Content -LiteralPath $graphPath -Raw | ConvertFrom-Json
    $projects = @($graph.projects.PSObject.Properties)
    if ($projects.Count -eq 0) { throw 'The SDK returned an empty restore graph.' }
    $versionLines = @(& $Dotnet msbuild $projects[0].Name -getProperty:InnoBgcsPackageVersion -nologo -nr:false @properties)
    if ($LASTEXITCODE -ne 0) { throw 'Cannot evaluate the declared BGCS package version.' }
    $expectedVersion = ($versionLines -join [Environment]::NewLine).Trim()
    if (-not $expectedVersion) { throw 'The source graph has no declared BGCS package identity.' }
    Invoke-SelectedSdk (@('restore', $selectedProject, '--locked-mode', '--disable-parallel', '-nr:false', '-v:minimal') + $properties) 'restore.log'

    $inventory = [Collections.Generic.List[object]]::new()
    foreach ($projectEntry in $projects) {
        $specification = $projectEntry.Value
        $restore = $specification.restore
        $lockPath = [IO.Path]::GetFullPath($restore.restoreLockProperties.nuGetLockFilePath)
        $owner = [IO.Path]::GetDirectoryName($projectEntry.Name)
        if ([IO.Path]::GetDirectoryName($lockPath) -ne $owner) { throw "The lock file escapes its project owner: $lockPath" }
        if ($restore.restoreLockProperties.restorePackagesWithLockFile -ne 'true') { throw "Lock generation is disabled: $($projectEntry.Name)" }
        if (-not (Test-Path -LiteralPath $lockPath -PathType Leaf)) { throw "The selected lock file is missing: $lockPath" }
        # NuGet's restore graph declares the actual assets output directory, including isolated layouts.
        $assetsPath = Join-Path $restore.outputPath 'project.assets.json'
        $assets = Get-Content -LiteralPath $assetsPath -Raw | ConvertFrom-Json
        $locked = Get-Content -LiteralPath $lockPath -Raw | ConvertFrom-Json
        $identities = @($assets.libraries.PSObject.Properties | Where-Object { $_.Name -match '^BGCS(?:\.[^/]+)?/' })
        foreach ($identity in $identities) {
            $packageName, $version = $identity.Name -split '/', 2
            if ($version -ne $expectedVersion) { throw "Unexpected package identity: $($identity.Name) in $assetsPath" }
            $entries = @($locked.dependencies.PSObject.Properties | ForEach-Object { $_.Value.PSObject.Properties } | Where-Object { $_.Name -eq $packageName })
            if ($entries.Count -eq 0) { throw "The assets package is absent from its selected lock: $($identity.Name)" }
            foreach ($entry in $entries) {
                if ($entry.Value.resolved -ne $version -or $entry.Value.contentHash -ne $identity.Value.sha512) {
                    throw "Package content does not match the selected lock: $($identity.Name)"
                }
            }
        }
        $inventory.Add([pscustomobject]@{
            project = $projectEntry.Name
            configuration = $Configuration
            target = $Target
            runtimes = @($specification.runtimes.PSObject.Properties.Name | Where-Object { $_ })
            frameworks = @($specification.frameworks.PSObject.Properties.Name)
            lockFile = $lockPath
            assetsFile = $assetsPath
            sha256 = (Get-FileHash -LiteralPath $lockPath -Algorithm SHA256).Hash
            bgcs = @($identities.Name)
        })
    }

    $after = Get-SourceLockState
    if ($before.Count -ne $after.Count) { throw 'Restore changed the source lock-file set.' }
    foreach ($path in $before.Keys) {
        if (-not $after.ContainsKey($path) -or ($before[$path] -join '|') -cne ($after[$path] -join '|')) {
            throw "Restore rewrote a source lock file: $path"
        }
        if ([IO.Path]::GetFileName($path) -notmatch '^packages(?:\.host-[a-zA-Z0-9][a-zA-Z0-9.-]*)?(?:\.rid-[a-zA-Z0-9][a-zA-Z0-9.-]*)?(?:\.(?:aot|link))?\.lock\.json$') {
            throw "A source lock uses an unowned naming scheme: $path"
        }
    }
    $untracked = @(& git -C $repository ls-files --others --exclude-standard -- '**/packages*.lock.json' 'packages*.lock.json')
    if ($LASTEXITCODE -ne 0 -or $untracked.Count -ne 0) { throw "Source locks must be intentionally tracked: $($untracked -join ', ')" }
    # Argument batches avoid Windows pipeline CRLF becoming part of stdin path records.
    $paths = @($after.Keys)
    for ($offset = 0; $offset -lt $paths.Count; $offset += 16) {
        $end = [Math]::Min($offset + 15, $paths.Count - 1)
        $batch = @($paths[$offset..$end])
        $ignored = @(& git -C $repository check-ignore --no-index -- @batch)
        if ($LASTEXITCODE -gt 1 -or $ignored.Count -ne 0) { throw 'Source lock files must not be hidden by ignore rules.' }
    }
    [pscustomobject]@{
        passed = $true
        project = $selectedProject
        bgcsVersion = $expectedVersion
        sourceLockCount = $after.Count
        requests = @($inventory.ToArray())
    } | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath (Join-Path $results 'inventory.json') -Encoding utf8
    Write-Host "Verified $($projects.Count) SDK projects; source locks unchanged; BGCS $expectedVersion."
}
finally {
    Pop-Location
}

exit 0
