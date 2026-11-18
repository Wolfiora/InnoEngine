$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$work = [IO.Directory]::CreateDirectory((Join-Path $root 'artifacts/validation/backend-variant-contract')).FullName
$probe = Join-Path $work 'compiler.targets'
@'
<Project>
  <Target Name="RunBindingCompiler" Condition="'$(SourceCompilation)' == 'true'">
    <PropertyGroup>
      <DefineConstants>$(DefineConstants);INNO_STATIC_NATIVE</DefineConstants>
    </PropertyGroup>
  </Target>
  <Target Name="ReadBackendVariant" DependsOnTargets="PrepareForBuild;RunBindingCompiler;GetInnoBackendCompilerOutputs" />
</Project>
'@ | Set-Content -LiteralPath $probe
$project = Join-Path $root 'backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj'
function Get-Variant([string]$Compile, [string]$Kind, [string]$Fingerprint) {
    $variant = @(& dotnet msbuild $project -nologo -verbosity:quiet -target:ReadBackendVariant -getProperty:_InnoBackendVariant `
        "-p:CustomAfterMicrosoftCommonTargets=$probe" "-p:ArtifactsPath=$work/sdk" "-p:InnoBackendExportRoot=$work/export" `
        "-p:SourceCompilation=$Compile" "-p:InnoNativeLibraryKind=$Kind" "-p:BindGenExpectedFingerprint=$Fingerprint")
    if ($LASTEXITCODE -or $variant.Count -ne 1 -or $variant[0] -cnotmatch '/[a-f0-9]{64}$') { throw 'Cannot evaluate the backend compiler variant.' }
    $variant[0]
}
$source = Get-Variant true Static ('a' * 64)
$consumer = Get-Variant false Static ('a' * 64)
if ($source -cne $consumer) { throw 'Skipping the binding compiler changed the identity of the same static binding.' }
if ($consumer -ceq (Get-Variant false Shared ('a' * 64)) -or $consumer -ceq (Get-Variant false Static ('b' * 64))) {
    throw 'Distinct binding fingerprints and linkage must retain distinct SDK variants.'
}
Write-Output 'Verified source and consumption use the same SDK variant for matching bindings.'
