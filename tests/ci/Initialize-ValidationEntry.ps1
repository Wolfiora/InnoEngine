param(
    [Parameter(Mandatory)][string]$Dotnet,
    [Parameter(Mandatory)][string]$LogPath
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$workspace = Join-Path $repository 'artifacts/validation/bootstrap'
New-Item -ItemType Directory -Path $workspace -Force | Out-Null
# This generated transport owns no validation policy and is not another daily executable project.
# Bootstrap only the managed libraries; compiling the product Build CLI would first require native binding inputs.
$source = @'
using System;
using System.Threading;
using Inno.Tooling.Documentation;
using Inno.Tooling.Validation;

using var cancellation = new CancellationTokenSource();
Console.CancelKeyPress += (_, options) => { options.Cancel = true; cancellation.Cancel(); };
try
{
    if (args.Length == 0) throw new ArgumentException("Expected validate or docs followed by its operation.");
    return args[0] switch
    {
        "validate" => await ValidationCommand.ExecuteAsync(args[1..], cancellation.Token),
        "docs" => await DocumentationCommand.ExecuteAsync(args[1..], cancellation.Token),
        _ => throw new ArgumentException("Unsupported CI transport command: " + args[0])
    };
}
catch (Exception failure)
{
    Console.Error.WriteLine(failure);
    return 1;
}
'@
$validation = [Security.SecurityElement]::Escape((Join-Path $repository 'tools/Inno.Tooling.Validation/Inno.Tooling.Validation.csproj').Replace('\', '/'))
$documentation = [Security.SecurityElement]::Escape((Join-Path $repository 'tools/Inno.Tooling.Documentation/Inno.Tooling.Documentation.csproj').Replace('\', '/'))
$projectText = @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>disable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="ValidationEntry.cs" />
    <ProjectReference Include="$validation" />
    <ProjectReference Include="$documentation" />
  </ItemGroup>
</Project>
"@
$project = Join-Path $workspace 'ValidationEntry.csproj'
foreach ($item in @(@{ path = $project; content = $projectText }, @{ path = (Join-Path $workspace 'ValidationEntry.cs'); content = $source })) {
    if (-not (Test-Path -LiteralPath $item.path) -or [IO.File]::ReadAllText($item.path) -cne $item.content) {
        [IO.File]::WriteAllText($item.path, $item.content, [Text.UTF8Encoding]::new($false))
    }
}
$outputs = Join-Path $workspace 'outputs'
& $Dotnet build $project --configuration Debug --artifacts-path $outputs --disable-build-servers "-m:$([Math]::Min([Environment]::ProcessorCount, 4))" -nr:false `
    *> $LogPath
if ($LASTEXITCODE -ne 0) { throw "Managed validation bootstrap failed; see $LogPath." }
$assembly = @(& $Dotnet msbuild $project -nologo -p:Configuration=Debug "-p:ArtifactsPath=$outputs" -getProperty:TargetPath)
if ($LASTEXITCODE -ne 0 -or $assembly.Count -ne 1 -or -not (Test-Path -LiteralPath $assembly[0])) {
    throw 'The SDK did not identify the managed validation transport.'
}
$assembly[0]
