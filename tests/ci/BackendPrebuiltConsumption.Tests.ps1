param([string]$Dotnet = 'dotnet')
$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$Dotnet = (Get-Command $Dotnet -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source
$workspace = Join-Path $repository ('artifacts/validation/backend-probes/' + [guid]::NewGuid().ToString('N').Substring(0, 12))
$source = Join-Path $workspace 'source'
$staging = Join-Path $workspace 'staging'
$consumer = Join-Path $workspace 'consumer'
$fixture = Join-Path $workspace 'probe'
$project = Join-Path $repository 'backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj'
$success = $false
try {
    [IO.Directory]::CreateDirectory($fixture) | Out-Null
    & $Dotnet build $project --configuration Debug --artifacts-path $source "-p:InnoBackendExportRoot=$staging" "-p:InnoBackendExportArtifactsPath=$source" -m:2 --nologo
    if ($LASTEXITCODE) { throw 'The SDL source producer failed.' }
    $receipts = @(Get-ChildItem -LiteralPath (Join-Path $staging 'receipts') -File -Recurse -Filter files.txt)
    if ($receipts.Count -ne 2) { throw 'Expected SDL and its backend library loader in the compiled SDK closure.' }
    $reference = [Security.SecurityElement]::Escape($project)
    [IO.File]::WriteAllText((Join-Path $fixture 'Probe.csproj'), @"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup><TargetFramework>net10.0</TargetFramework><OutputType>Exe</OutputType></PropertyGroup>
  <ItemGroup><ProjectReference Include="$reference" /></ItemGroup>
</Project>
"@)
    [IO.File]::WriteAllText((Join-Path $fixture 'Program.cs'), @'
using System;
using System.Reflection;
using Inno.Native.Sdl3;
var backend = typeof(SDL).Assembly;
foreach (var dependency in backend.GetReferencedAssemblies())
    Assembly.Load(dependency);
Console.WriteLine(backend.GetName().Name);
'@)
    $arguments = @('build', (Join-Path $fixture 'Probe.csproj'), '--artifacts-path', $consumer, "-p:InnoBackendPrebuiltRoot=$staging", '-m:2', '--nologo')
    & $Dotnet @arguments
    if ($LASTEXITCODE) { throw 'A clean SDK consumer could not use the backend reference assemblies.' }
    $output = & $Dotnet (Join-Path $consumer 'bin/Probe/debug/Probe.dll')
    if ($LASTEXITCODE -or $output -cne 'Inno.Native.Sdl3') { throw 'The runtime loader did not resolve the SDL assembly closure.' }
    $lines = @($receipts | ForEach-Object { Get-Content -LiteralPath $_.FullName })
    $entry = @($lines | Where-Object { $_ -cmatch '/ref/Inno\.Native\.Sdl3\.dll\|' })
    if ($entry.Count -ne 1) { throw 'The SDL reference must have a unique compiler receipt.' }
    $fields = $entry[0].Split('|')
    $required = [IO.Path]::GetFullPath($fields[2], $staging)
    if (![IO.Path]::GetRelativePath($staging, $required).StartsWith('managed' + [IO.Path]::DirectorySeparatorChar)) { throw 'Invalid probe receipt.' }
    [IO.File]::Delete($required)
    $failure = @(& $Dotnet @arguments 2>&1)
    if ($LASTEXITCODE -eq 0 -or ($failure -join [Environment]::NewLine) -notmatch 'Missing backend prebuilt') {
        throw 'A missing reference assembly did not block consumption.'
    }
    $global:LASTEXITCODE = 0
    $success = $true
} finally {
    if ($success) { [IO.Directory]::Delete($workspace, $true) }
    else { Write-Warning "Retained backend integration probe at $workspace." }
}
