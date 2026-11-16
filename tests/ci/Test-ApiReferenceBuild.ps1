param([Parameter(Mandatory)][string] $Dotnet, [Parameter(Mandatory)][string] $Output)
$ErrorActionPreference = 'Stop'
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$outputRoot = [IO.Path]::GetFullPath($Output)
[IO.Directory]::CreateDirectory($outputRoot) | Out-Null
$cases = @(
    @{ project = 'backends/Bgfx/runtime/Inno.Adapter.Rendering.Bgfx/Inno.Adapter.Rendering.Bgfx.csproj'; target = 'CompileCompositionShaderGraphs'; property = 'InnoBgfxShaderApis' },
    @{ project = 'backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Bgfx/Inno.Adapter.Presentation.ImGui.Bgfx.csproj'; target = 'CompileImGuiShaderGraphs'; property = 'InnoImGuiShaderApis' }
)
foreach ($case in $cases) {
    $arguments = @('msbuild', (Join-Path $engine $case.project), '-nologo', '-m:4', '-nr:false', "-t:$($case.target)",
        '-p:InnoNativeTarget=api-contract-fixture', "-p:$($case.property)=", "-p:ArtifactsPath=$outputRoot/managed")
    $log = Join-Path $outputRoot ($case.target + '-reference.log')
    & $Dotnet @arguments '-p:InnoBuildPurpose=ApiReferences' *> $log
    if ($LASTEXITCODE -ne 0) { throw "Managed API preparation unexpectedly entered shader preparation: $log" }
    $log = Join-Path $outputRoot ($case.target + '-product.log')
    & $Dotnet @arguments *> $log
    if ($LASTEXITCODE -eq 0 -or !([IO.File]::ReadAllText($log).Contains('explicit shader platform'))) {
        throw "Ordinary products no longer require their shader prerequisites: $log"
    }
}
$project = Join-Path $engine 'src/foundation/core/Inno.Core.IO/Inno.Core.IO.csproj'
foreach ($target in @('ValidateApiReferenceOutputOwner', 'RejectApiReferenceProductPublication')) {
    $log = Join-Path $outputRoot ($target + '.log')
    & $Dotnet msbuild $project -nologo "-t:$target" '-p:InnoBuildPurpose=ApiReferences' *> $log
    $expected = if ($target -eq 'ValidateApiReferenceOutputOwner') { 'explicit ArtifactsPath' } else { 'cannot publish a runnable product' }
    if ($LASTEXITCODE -eq 0 -or !([IO.File]::ReadAllText($log).Contains($expected))) { throw "API preparation boundary failed: $log" }
}
'Passed six actual SDK preparation/publication boundary cases.'
