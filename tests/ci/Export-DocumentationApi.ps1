param(
    [Parameter(Mandatory)][string] $Dotnet,
    [Parameter(Mandatory)][string] $Output,
    [ValidateSet('Debug', 'Release', 'SourceDebug')][string] $Configuration = 'Debug'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$outputRoot = [IO.Path]::GetFullPath($Output)
$sdk = [IO.Path]::GetFullPath($Dotnet)
function Git-Value([string[]] $Arguments) {
    $value = @(& git -C $engine @Arguments)
    if ($LASTEXITCODE -ne 0) { throw 'Cannot establish API source identity.' }
    return $value
}
function Invoke-Managed([string[]] $Arguments, [string] $Log) {
    $watch = [Diagnostics.Stopwatch]::StartNew()
    & $sdk @Arguments *> $Log
    if ($LASTEXITCODE -ne 0) { throw "API preparation failed; first failure is retained in $Log." }
    Write-Host "$([IO.Path]::GetFileName($Log)): $([Math]::Round($watch.Elapsed.TotalSeconds, 3)) seconds."
}
function Assert-Source {
    if (((Git-Value @('rev-parse', 'HEAD')) -join '') -cne $revision -or
        @(Git-Value @('status', '--porcelain', '--untracked-files=all', '--ignore-submodules=none')).Count -ne 0) {
        throw 'API export requires unchanged committed source inputs, including generated binding inputs.'
    }
}
$revision = (Git-Value @('rev-parse', 'HEAD')) -join ''
$tree = (Git-Value @('rev-parse', 'HEAD^{tree}')) -join ''
Assert-Source
if (Test-Path -LiteralPath (Join-Path $outputRoot 'producer.json')) { throw 'A producer receipt already occupies this output; select a new evidence directory.' }
[IO.Directory]::CreateDirectory($outputRoot) | Out-Null
$transport = @(& "$PSScriptRoot/Initialize-ValidationEntry.ps1" -Dotnet $sdk -LogPath (Join-Path $outputRoot 'bootstrap.log'))
if ($transport.Count -ne 1 -or !(Test-Path -LiteralPath $transport[0])) { throw 'The managed API transport was not prepared.' }
$planPath = Join-Path $outputRoot 'plan.json'
Invoke-Managed @($transport[0], 'docs', 'export-plan', '--engine-root', $engine, '--dotnet', $sdk,
    '--configuration', $Configuration, '--output', $planPath) (Join-Path $outputRoot 'plan.log')
$plan = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json
if ($plan.schema -cne 'inno-api-export-plan/1' -or @($plan.buildProjects).Count -eq 0) { throw 'API preparation has no valid owner plan.' }

# The SDK builds the union once. Normal product commands retain their Native preparation;
# this explicitly scoped export only needs real managed references and generated bindings.
$solution = [Xml.XmlDocument]::new()
$solution.AppendChild($solution.CreateElement('Solution')) | Out-Null
# The executable transport has no authored public types, but executes the real script compiler.
# Its explicit operational dependency must survive the API owner's empty-surface optimization.
foreach ($project in @(@($plan.buildProjects) + 'entry/InnoEngine.Build/InnoEngine.Build.csproj' | Sort-Object -Unique)) {
    $node = $solution.CreateElement('Project')
    $node.SetAttribute('Path', [IO.Path]::GetFullPath($project, $engine))
    $solution.DocumentElement.AppendChild($node) | Out-Null
}
$solutionPath = Join-Path $outputRoot 'managed-union.slnx'
$solution.Save($solutionPath)
$managedOutputs = Join-Path $engine "artifacts/documentation-export/managed/$Configuration/$($plan.hostTarget)"
Invoke-Managed @('build', $solutionPath, '--configuration', $Configuration, '--disable-build-servers',
    "-m:$([Math]::Min([Environment]::ProcessorCount, 4))", '-nr:false', '-p:InnoBuildPurpose=ApiReferences',
    '--artifacts-path', $managedOutputs) (Join-Path $outputRoot 'managed-union.log')
$cli = @(& $sdk msbuild (Join-Path $engine 'entry/InnoEngine.Build/InnoEngine.Build.csproj') -nologo `
    "-p:Configuration=$Configuration" "-p:ArtifactsPath=$managedOutputs" -getProperty:TargetPath)
if ($LASTEXITCODE -ne 0 -or $cli.Count -ne 1 -or !(Test-Path -LiteralPath $cli[0])) { throw 'The SDK did not identify the prepared Build entry.' }

$captures = [Collections.Generic.List[object]]::new()
foreach ($batch in $plan.captures) {
    $path = Join-Path $outputRoot ("source-$($batch.target).json")
    $arguments = @($cli[0], 'docs', 'export-api', '--engine-root', $engine, '--dotnet', $sdk,
        '--configuration', $Configuration, '--target', $batch.target, '--artifacts-path', $managedOutputs, '--output', $path)
    foreach ($project in $batch.projects) { $arguments += @('--project', $project) }
    Invoke-Managed $arguments ($path + '.log')
    $captures.Add(@{ path = [IO.Path]::GetFileName($path); sha256 = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant();
        model = 'source-clr'; configuration = $Configuration; target = $batch.target })
}

# The normal script compiler produces both logical assemblies and origin metadata.
# This empty workspace selects the engine's actual built-in exports, never host-type guesses.
$scriptRoot = Join-Path $engine "artifacts/documentation-export/scripts/$Configuration/$($plan.hostTarget)"
[IO.Directory]::CreateDirectory((Join-Path $scriptRoot 'Assets')) | Out-Null
Invoke-Managed @($cli[0], 'scripts', '--engine-root', $engine, '--dotnet', $sdk, '--project', $scriptRoot,
    '--output', (Join-Path $scriptRoot 'Export'), '--ide') (Join-Path $outputRoot 'script-projection.log')
[xml] $ide = Get-Content -LiteralPath (Join-Path $scriptRoot 'Inno.EditorScripts.csproj') -Raw
$runtime = @($ide.Project.ItemGroup.Reference | Where-Object Include -CEQ 'Inno.ScriptApi.Runtime')
$editor = @($ide.Project.ItemGroup.Reference | Where-Object Include -CEQ 'Inno.ScriptApi.Editor')
if ($runtime.Count -ne 1 -or $editor.Count -ne 1) { throw 'Actual script preparation did not expose both logical references.' }
foreach ($surface in @('runtime-script', 'editor-script')) {
    $path = Join-Path $outputRoot ($surface + '.json')
    $arguments = @($cli[0], 'docs', 'export-api', '--engine-root', $engine, '--dotnet', $sdk,
        '--configuration', $Configuration, '--target', $plan.hostTarget, '--surface', $surface,
        '--runtime-reference', $runtime[0].HintPath, '--framework-project', 'entry/InnoEngine.Build/InnoEngine.Build.csproj',
        '--artifacts-path', $managedOutputs, '--output', $path)
    if ($surface -eq 'editor-script') { $arguments += @('--editor-reference', $editor[0].HintPath) }
    foreach ($capture in $captures | Where-Object model -CEQ 'source-clr') { $arguments += @('--source-reference', (Join-Path $outputRoot $capture.path)) }
    Invoke-Managed $arguments ($path + '.log')
    $captures.Add(@{ path = [IO.Path]::GetFileName($path); sha256 = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant();
        model = $surface; configuration = $Configuration; target = $plan.hostTarget })
}
$projectsPath = Join-Path $outputRoot 'projects.json'
Invoke-Managed @($cli[0], 'docs', 'export-projects', '--engine-root', $engine, '--output', $projectsPath) (Join-Path $outputRoot 'projects.log')
Assert-Source
$receipt = @{ schema = 'inno-api-producer/1'; repository = 'https://github.com/Wolfiora/InnoEngine'; revision = $revision;
    sourceTree = $tree; toolingRevision = $revision; hostTarget = $plan.hostTarget; configuration = $Configuration;
    sdkRuntime = $plan.sdkRuntime; projectManifestIdentity = (Get-FileHash -LiteralPath $projectsPath -Algorithm SHA256).Hash.ToLowerInvariant();
    planIdentity = (Get-FileHash -LiteralPath $planPath -Algorithm SHA256).Hash.ToLowerInvariant();
    captures = $captures.ToArray(); foreignOwners = $plan.foreignOwners }
$pending = Join-Path $outputRoot 'producer.pending.json'
[IO.File]::WriteAllText($pending, ($receipt | ConvertTo-Json -Depth 30), [Text.UTF8Encoding]::new($false))
Move-Item -LiteralPath $pending -Destination (Join-Path $outputRoot 'producer.json')
Write-Host "Exported $($captures.Count) API contexts from $revision on $($plan.hostTarget). Multi-host reconciliation is still required."
