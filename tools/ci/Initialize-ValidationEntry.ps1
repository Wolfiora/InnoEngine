param(
    [Parameter(Mandatory)][string]$Dotnet,
    [Parameter(Mandatory)][string]$LogPath,
    [string]$WorkspaceDirectory = 'artifacts/validation/bootstrap'
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$workspace = [IO.Path]::GetFullPath($WorkspaceDirectory, $repository)
$artifactBoundary = [IO.Path]::GetFullPath('artifacts', $repository) + [IO.Path]::DirectorySeparatorChar
$pathComparison = if ($IsWindows) { [StringComparison]::OrdinalIgnoreCase } else { [StringComparison]::Ordinal }
if (-not $workspace.StartsWith($artifactBoundary, $pathComparison)) { throw 'Validation bootstrap requires an explicit repository artifact owner.' }
New-Item -ItemType Directory -Path $workspace -Force | Out-Null
$project = Join-Path $repository 'tools/Inno.Tooling.Cli/Inno.Tooling.Cli.csproj'
$outputs = Join-Path $workspace 'outputs'
& $Dotnet build $project --configuration Debug --artifacts-path $outputs --disable-build-servers "-m:$([Math]::Min([Environment]::ProcessorCount, 4))" -nr:false `
    *> $LogPath
if ($LASTEXITCODE -ne 0) { throw "Managed validation bootstrap failed; see $LogPath." }
$assembly = @(& $Dotnet msbuild $project -nologo -p:Configuration=Debug "-p:ArtifactsPath=$outputs" -getProperty:TargetPath)
if ($LASTEXITCODE -ne 0 -or $assembly.Count -ne 1 -or -not (Test-Path -LiteralPath $assembly[0])) {
    throw 'The SDK did not identify the managed validation transport.'
}
$assembly[0]
