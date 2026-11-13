$ErrorActionPreference = 'Stop'
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$root = Join-Path $engine ('artifacts/validation/preparation-solution/' + [Guid]::NewGuid().ToString('N'))
$dotnet = (Get-Command dotnet -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source
$passed = $false
function Write-Text([string] $Path, [string] $Text) {
    [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($Path)) | Out-Null
    [IO.File]::WriteAllText($Path, $Text, [Text.UTF8Encoding]::new($false))
}
try {
    Write-Text (Join-Path $root 'Directory.Build.props') '<Project />'
    Write-Text (Join-Path $root 'Directory.Build.targets') '<Project />'
    Write-Text (Join-Path $root 'Directory.Solution.targets') '<Project />'
    $project = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
  <Target Name="RequireSelectedConfiguration" BeforeTargets="CoreCompile">
    <Error Condition="'$(Configuration)' != 'EXPECTED_CONFIGURATION'" Text="The transitive project lost the selected preparation configuration." />
  </Target>
  REFERENCE
</Project>
'@
    Write-Text (Join-Path $root 'entry/Entry.cs') 'public sealed class Entry { }'
    Write-Text (Join-Path $root 'dependency/Dependency.cs') 'public sealed class Dependency { }'
    $solution = Join-Path $root 'preparation.slnx'
    $writer = Join-Path $PSScriptRoot 'Invoke-ManagedApiPreparation.ps1'
    foreach ($configuration in @('Release', 'SourceDebug')) {
        $selected = $project.Replace('EXPECTED_CONFIGURATION', $configuration)
        Write-Text (Join-Path $root 'dependency/Dependency.csproj') $selected.Replace('REFERENCE', '')
        Write-Text (Join-Path $root 'entry/Entry.csproj') $selected.Replace('REFERENCE', '<ItemGroup><ProjectReference Include="../dependency/Dependency.csproj" /></ItemGroup>')
        & $writer -EngineRoot $root -Projects 'entry/Entry.csproj' -Configuration $configuration -Output $solution `
            -Dotnet $dotnet -ArtifactsPath (Join-Path $root ('outputs/' + $configuration)) -Log (Join-Path $root ($configuration + '.log'))
    }
    $previous = [IO.File]::ReadAllBytes($solution)
    $rejected = $false
    try { & $writer -EngineRoot $root -Projects '../outside.csproj' -Configuration Release -Output $solution `
        -Dotnet $dotnet -ArtifactsPath (Join-Path $root 'outputs/Release') -Log (Join-Path $root 'invalid.log') }
    catch { $rejected = $true }
    if (!$rejected -or ![Linq.Enumerable]::SequenceEqual[byte]($previous, [IO.File]::ReadAllBytes($solution))) {
        throw 'Invalid preparation input replaced the previous complete solution.'
    }
    $passed = $true
    'Passed actual Release and SourceDebug transitive preparation, invalid owner rejection and atomic previous-output preservation.'
} finally {
    if ($passed) {
        $owned = [IO.Path]::GetFullPath($root)
        $parent = [IO.Path]::GetFullPath('artifacts/validation/preparation-solution', $engine) + [IO.Path]::DirectorySeparatorChar
        if (!$owned.StartsWith($parent, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unexpected preparation fixture cleanup boundary.' }
        Remove-Item -LiteralPath $owned -Recurse -Force
    }
}
