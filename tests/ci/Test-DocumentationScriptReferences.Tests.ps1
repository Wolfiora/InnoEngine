$ErrorActionPreference = 'Stop'
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$root = Join-Path $engine ('artifacts/validation/script-reference-selection/' + [Guid]::NewGuid().ToString('N'))
[IO.Directory]::CreateDirectory($root) | Out-Null
$passed = $false
try {
    # Artifact selection does not certify PE contents; the real API extractor validates those.
    [IO.File]::WriteAllText((Join-Path $root 'runtime.dll'), 'selection fixture')
    [IO.File]::WriteAllText((Join-Path $root 'editor.dll'), 'selection fixture')
    $path = Join-Path $root 'Inno.EditorScripts.csproj'
    $reader = Join-Path $PSScriptRoot 'Get-DocumentationScriptReferences.ps1'
    $xml = '<Project><ItemGroup><Compile Include="Script.cs" /></ItemGroup><ItemGroup><Reference Include="Inno.ScriptApi.Runtime"><HintPath>runtime.dll</HintPath></Reference><Reference Include="Inno.ScriptApi.Editor"><HintPath>editor.dll</HintPath></Reference></ItemGroup><ItemGroup><None Include="project.json" /></ItemGroup></Project>'
    [IO.File]::WriteAllText($path, $xml)
    $actual = & $reader -Project $path
    if ($actual.runtime -cne (Join-Path $root 'runtime.dll') -or $actual.editor -cne (Join-Path $root 'editor.dll')) { throw 'Mixed item groups lost an actual logical reference.' }
    foreach ($invalid in @(
        $xml.Replace('Inno.ScriptApi.Editor', 'Unrelated'),
        $xml.Replace('Inno.ScriptApi.Editor', 'Inno.ScriptApi.Runtime'),
        $xml.Replace('editor.dll', 'missing.dll'),
        $xml.Replace('<HintPath>editor.dll</HintPath>', '')
    )) {
        [IO.File]::WriteAllText($path, $invalid)
        $rejected = $false
        try { $null = & $reader -Project $path } catch { $rejected = $true }
        if (!$rejected) { throw 'Incomplete or ambiguous logical references were accepted.' }
    }
    $passed = $true
    'Passed mixed IDE item groups and four missing/ambiguous artifact rejection cases.'
} finally {
    if ($passed) {
        $owned = [IO.Path]::GetFullPath($root)
        $parent = [IO.Path]::GetFullPath('artifacts/validation/script-reference-selection', $engine) + [IO.Path]::DirectorySeparatorChar
        if (!$owned.StartsWith($parent, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unexpected script fixture cleanup boundary.' }
        Remove-Item -LiteralPath $owned -Recurse -Force
    }
}
