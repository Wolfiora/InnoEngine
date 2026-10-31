param(
    [Alias('Profile')][ValidateSet('Contracts', 'Workflows', 'Visual', 'Lifecycle', 'Performance', 'Products', 'All')]
    [string]$AcceptanceProfile = 'Contracts',
    [ValidateSet('Debug', 'Release')][string]$Configuration = 'Debug',
    [string]$Dotnet = 'dotnet',
    [Parameter(Mandatory = $true)][string]$ResultsDirectory,
    [Parameter(Mandatory = $true)][string]$ArtifactsPath,
    [string]$TemporaryDirectory,
    [string]$ProductProject,
    [string[]]$GraphicsApis = @(),
    [string]$BackendPattern,
    [switch]$EditorOnly,
    [switch]$NoBuild
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path $PSScriptRoot -Parent
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$results = Join-Path ([IO.Path]::GetFullPath($ResultsDirectory, $repository)) ([Guid]::NewGuid().ToString('N'))
$artifacts = [IO.Path]::GetFullPath($ArtifactsPath, $repository)
New-Item -ItemType Directory -Force -Path $results | Out-Null
$records = [Collections.Generic.List[object]]::new()
$revision = (& git -C $repository rev-parse HEAD).Trim()
if ($LASTEXITCODE -ne 0) { throw 'The candidate revision could not be recorded.' }
$dirty = @(& git -C $repository status --porcelain=v1)
if ($LASTEXITCODE -ne 0) { throw 'The candidate working state could not be recorded.' }
$sdk = @(& $Dotnet --info)
if ($LASTEXITCODE -ne 0) { throw 'The selected SDK could not be inspected.' }
$sdk | Set-Content -LiteralPath (Join-Path $results 'sdk.txt') -Encoding utf8

function Save-Result(
    [string]$Stage,
    [string]$Status,
    [string]$Diagnostic
) {
    $records.Add([pscustomobject]@{ stage = $Stage; status = $Status; diagnostic = $Diagnostic })
    [pscustomobject]@{
        revision = $revision
        workingChanges = $dirty
        profile = $AcceptanceProfile
        configuration = $Configuration
        buildRequested = -not $NoBuild
        scope = 'Selected automated suites; issue closure additionally requires the complete tracked acceptance matrix.'
        results = @($records.ToArray())
    } | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $results 'summary.json') -Encoding utf8
}

$entry = Join-Path $repository 'entry/InnoEngine.Editor/InnoEngine.Editor.csproj'
$evaluated = @(& $Dotnet msbuild $entry -nologo "-p:Configuration=$Configuration" "-p:ArtifactsPath=$artifacts" `
    '-getProperty:InnoNativeTarget,InnoToolTarget,InnoProductBuildProperties,TargetPath')
if ($LASTEXITCODE -ne 0) { throw 'The official Editor entry could not select its product owner.' }
$product = ($evaluated -join [Environment]::NewLine | ConvertFrom-Json).Properties
if (-not $product.InnoNativeTarget -or -not $product.InnoToolTarget -or
    -not (Test-Path -LiteralPath $product.InnoProductBuildProperties)) {
    throw 'The selected development host does not provide an Editor product.'
}
$buildArguments = @("-p:ArtifactsPath=$artifacts", "-p:InnoNativeTarget=$($product.InnoNativeTarget)",
    "-p:InnoToolTarget=$($product.InnoToolTarget)",
    "-p:InnoProductBuildProperties=$($product.InnoProductBuildProperties)")
$profiles = if ($AcceptanceProfile -eq 'All') { @('Contracts', 'Workflows', 'Visual', 'Lifecycle', 'Performance', 'Products') } else { @($AcceptanceProfile) }

foreach ($selected in $profiles) {
    $stageRoot = Join-Path $results $selected
    New-Item -ItemType Directory -Force -Path $stageRoot | Out-Null
    try {
        if ($selected -eq 'Products') {
            if (-not $ProductProject -or -not (Test-Path -LiteralPath (Join-Path $ProductProject 'Assets'))) {
                Save-Result $selected 'Blocked' 'Supply an independent FlappyBird acceptance copy using -ProductProject.'
                continue
            }
            if ($GraphicsApis.Count -eq 0) {
                Save-Result $selected 'Blocked' 'Supply the explicit graphics API matrix for the selected product using -GraphicsApis.'
                continue
            }
            if (-not $BackendPattern) {
                Save-Result $selected 'Blocked' 'Supply the product initialization expectation using -BackendPattern.'
                continue
            }
            if (-not $NoBuild) {
                & $Dotnet build $entry -c $Configuration @buildArguments *> (Join-Path $stageRoot 'build.log')
                if ($LASTEXITCODE -ne 0) { throw 'The official Editor build failed.' }
            }
            if (-not (Test-Path -LiteralPath $product.TargetPath)) { throw 'The SDK-selected Editor output is unavailable.' }
            foreach ($api in $GraphicsApis) {
                & (Join-Path $PSScriptRoot 'Invoke-EditorSmoke.ps1') -Dotnet $Dotnet -EditorAssembly $product.TargetPath `
                    -ProjectDirectory $ProductProject -GraphicsApi $api -BackendPattern $BackendPattern -Frames 300 -HiddenWindow `
                    *> (Join-Path $stageRoot "$api.log")
                & (Join-Path $PSScriptRoot 'Invoke-EditorUiProductJourney.ps1') -Dotnet $Dotnet -EditorAssembly $product.TargetPath `
                    -ProjectDirectory $ProductProject -GraphicsApi $api -ResultsDirectory (Join-Path $stageRoot "journey-$api") `
                    *> (Join-Path $stageRoot "journey-$api.log")
                & (Join-Path $PSScriptRoot 'Invoke-EditorUiProductJourney.ps1') -Dotnet $Dotnet -EditorAssembly $product.TargetPath `
                    -ProjectDirectory $ProductProject -GraphicsApi $api -Scenario Lifecycle -ResultsDirectory (Join-Path $stageRoot "lifecycle-$api") `
                    *> (Join-Path $stageRoot "lifecycle-$api.log")
            }
            Save-Result $selected 'Passed' 'Hidden renderer smoke, eight product workflows and 100 cycles per actual panel completed; physical-device experience remains separately tracked.'
            continue
        }
        $names = switch ($selected) {
            'Contracts' { @('Inno.Editor.Interactions.Tests', 'Inno.Editor.ImGui.Tests', 'Inno.Editor.Graph.Tests', 'Inno.Editor.Inspection.Tests', 'Inno.Scripting.Compiler.Tests', 'Inno.Build.Tests', 'Inno.Tooling.Architecture.Tests') }
            'Workflows' { @('Inno.Editor.PlayMode.Tests', 'Inno.Editor.Scripting.Tests', 'Inno.Editor.Hosting.Tests') }
            'Visual' { @('Inno.Editor.ImGui.Tests', 'Inno.Editor.PlayMode.Tests', 'Inno.Editor.Scripting.Tests') }
            'Lifecycle' { @('Inno.Editor.ImGui.Tests', 'Inno.Editor.Graph.Tests', 'Inno.Editor.PlayMode.Tests', 'Inno.Editor.Scripting.Tests') }
            'Performance' { @('Inno.Editor.Interactions.Tests', 'Inno.Editor.PlayMode.Tests', 'Inno.Editor.Scripting.Tests') }
        }
        if ($EditorOnly) { $names = @($names | Where-Object { $_ -notin @('Inno.Build.Tests', 'Inno.Tooling.Architecture.Tests') }) }
        if ($EditorOnly -and $selected -eq 'Workflows') { $names += 'Inno.Build.Tests' }
        $filter = switch ($selected) {
            'Visual' { 'FullyQualifiedName~WidgetLayoutTests|FullyQualifiedName~ViewportMetricsTests|Acceptance=Visual' }
            'Lifecycle' { 'Acceptance=Lifecycle' }
            'Performance' { 'Acceptance=Performance|Category=Performance' }
            default { '(Acceptance!=Performance)&(Category!=Performance)' }
        }
        if ($EditorOnly -and $selected -eq 'Workflows') {
            $filter = "($filter)&(FullyQualifiedName!~Inno.Build.Tests|FullyQualifiedName~BuildPipelineTests.Export|FullyQualifiedName~BuildPipelineTests.EditorShutdownDrainsSupportPackCleanupBeforeReleasingExportServices)"
        }
        $projects = foreach ($name in $names) {
            $projectFiles = @(Get-ChildItem -LiteralPath (Join-Path $repository 'tests') -Filter "$name.csproj" -Recurse)
            if ($projectFiles.Count -ne 1) { throw "The suite '$name' does not resolve to one test project." }
            [IO.Path]::GetRelativePath($repository, $projectFiles[0].FullName)
        }
        $selection = Join-Path $stageRoot 'tests.slnf'
        @{ solution = @{ path = (Join-Path $repository 'InnoEngine.sln'); projects = @($projects) } } |
            ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $selection -Encoding utf8
        if (-not $NoBuild) {
            if ($selected -eq 'Contracts' -and -not $EditorOnly) {
                $sourceConfiguration = "Source$Configuration"
                & $Dotnet build (Join-Path $repository 'InnoEngine.sln') -c $sourceConfiguration "-p:ArtifactsPath=$artifacts" `
                    *> (Join-Path $stageRoot 'source-build.log')
                if ($LASTEXITCODE -ne 0) { throw 'The complete neutral source graph required by repository architecture and Wiki checks did not build.' }
                $contracts = @()
                foreach ($candidate in Get-ChildItem -LiteralPath (Join-Path $repository 'platforms') -Filter '*.csproj' -Recurse) {
                    $declaration = [xml](Get-Content -LiteralPath $candidate.FullName -Raw)
                    if (-not $declaration.SelectSingleNode('//InnoProductTarget')) { continue }
                    $productEvaluation = @(& $Dotnet msbuild $candidate.FullName -nologo "-p:Configuration=$Configuration" "-p:ArtifactsPath=$artifacts" `
                        '-getProperty:InnoProductTarget,TargetPath,MSBuildProjectName')
                    if ($LASTEXITCODE -ne 0) { throw "The SDK could not evaluate the declared product '$($candidate.FullName)'." }
                    $contract = ($productEvaluation -join [Environment]::NewLine | ConvertFrom-Json).Properties
                    if ($contract.InnoProductTarget -ne $product.InnoNativeTarget) { continue }
                    $contracts += [pscustomobject]@{ project = $candidate.FullName; target = $contract.InnoProductTarget; output = $contract.TargetPath }
                    & $Dotnet build $candidate.FullName -c $Configuration "-p:ArtifactsPath=$artifacts" `
                        *> (Join-Path $stageRoot "source-product-$($contract.MSBuildProjectName).log")
                    if ($LASTEXITCODE -ne 0) { throw "The selected product contract '$($candidate.FullName)' did not build." }
                }
                $contracts | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $stageRoot 'product-contracts.json') -Encoding utf8
                if ($contracts.Count -eq 0) { throw 'No declared product contract matches the selected target.' }
            }
            & $Dotnet build $selection -c $Configuration @buildArguments *> (Join-Path $stageRoot 'build.log')
            if ($LASTEXITCODE -ne 0) { throw 'The selected test graph did not build.' }
        }
        & (Join-Path $PSScriptRoot 'Invoke-IsolatedTests.ps1') -Dotnet $Dotnet -Solution $selection `
            -Configuration $Configuration -ResultsDirectory $stageRoot -ArtifactsPath $artifacts `
            -Target $product.InnoNativeTarget -ToolTarget $product.InnoToolTarget -ProductProperties $product.InnoProductBuildProperties -Filter $filter `
            -TemporaryDirectory $TemporaryDirectory `
            *> (Join-Path $stageRoot 'execution.log')
        $executed = 0
        $observedSuites = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        foreach ($trx in Get-ChildItem -LiteralPath $stageRoot -Filter '*.trx') {
            $report = [xml](Get-Content -LiteralPath $trx.FullName -Raw)
            $executed += [int]$report.TestRun.ResultSummary.Counters.executed
            if ([int]$report.TestRun.ResultSummary.Counters.executed -gt 0) {
                foreach ($method in $report.TestRun.TestDefinitions.UnitTest.TestMethod) {
                    [void]$observedSuites.Add([IO.Path]::GetFileNameWithoutExtension($method.codeBase))
                }
            }
            if ([int]$report.TestRun.ResultSummary.Counters.failed -ne 0 -or $report.TestRun.ResultSummary.outcome -eq 'Aborted') {
                throw "The test result '$($trx.Name)' did not pass."
            }
        }
        if ($executed -le 0) { throw 'The selected suite executed no tests.' }
        foreach ($name in $names) {
            if (-not $observedSuites.Contains($name)) { throw "The selected project '$name' executed no tests." }
        }
        Save-Result $selected 'Passed' "$executed test cases executed; consult TRX and isolation logs."
    } catch {
        Save-Result $selected 'Failed' $_.Exception.Message
    }
}
if (@($records | Where-Object status -ne 'Passed').Count -ne 0) {
    throw "Acceptance has failed or blocked stages. See $results/summary.json."
}
