param(
    [string]$BaseRevision,
    [string]$HeadRevision = 'HEAD',
    [string[]]$ChangedPath,
    [switch]$PullRequest,
    [switch]$Full
)

$ErrorActionPreference = 'Stop'

function Test-EditorReferenceVisibilityOnly(
    [string]$Path,
    [string]$Before,
    [string]$After
) {
    if (-not $Before -or $Path -notmatch '^src/composition/editor/.*\.csproj$') { return $false }
    $diff = @(& git diff --unified=0 --no-color --no-ext-diff $Before $After -- $Path)
    if ($LASTEXITCODE -ne 0) { throw 'The project visibility comparison failed.' }
    $removed = [Collections.Generic.List[string]]::new()
    $added = [Collections.Generic.List[string]]::new()
    foreach ($line in $diff) {
        if ($line -match '^(---|\+\+\+)' -or $line -notmatch '^[+-]') { continue }
        if ($line -match '^\-\s*<ProjectReference Include="([^"]+)"\s*/>\s*$') { $removed.Add($Matches[1]) }
        elseif ($line -match '^\+\s*<ProjectReference Include="([^"]+)" PrivateAssets="compile"\s*/>\s*$') { $added.Add($Matches[1]) }
        else { return $false }
    }
    if ($added.Count -eq 0 -or $added.Count -ne $removed.Count -or
        (Compare-Object @($removed | Sort-Object) @($added | Sort-Object))) { return $false }
    # Prove unchanged, unconditional edges; do not interpret or resolve an MSBuild dependency graph.
    foreach ($revision in @($Before, $After)) {
        $text = @(& git show "${revision}:$Path")
        if ($LASTEXITCODE -ne 0) { return $false }
        try { $xml = [xml]($text -join [Environment]::NewLine) } catch { return $false }
        if ($xml.Project.Sdk -ne 'Microsoft.NET.Sdk' -or
            @($xml.Project.ChildNodes | Where-Object { $_.NodeType -eq 'Element' -and $_.Name -notin @('PropertyGroup', 'ItemGroup') }).Count -ne 0) { return $false }
        foreach ($include in $added) {
            $references = @($xml.SelectNodes('//ProjectReference') | Where-Object { $_.Include -ceq $include })
            if ($references.Count -ne 1 -or $references[0].ParentNode.Name -ne 'ItemGroup' -or
                $references[0].ParentNode.Attributes.Count -ne 0 -or
                $references[0].ParentNode.ParentNode.Name -ne 'Project') { return $false }
        }
    }
    return $true
}

[string[]]$paths = @()
if ($ChangedPath) { $paths = $ChangedPath }
if (-not $PSBoundParameters.ContainsKey('ChangedPath') -and -not $Full) {
    if (-not $BaseRevision) { throw 'A complete comparison base is required for automatic validation.' }
    $comparisonBase = $BaseRevision
    if ($PullRequest) {
        $ancestor = @(& git merge-base $BaseRevision $HeadRevision)
        if ($LASTEXITCODE -ne 0 -or $ancestor.Count -ne 1) { throw 'The pull request merge base is unavailable.' }
        $comparisonBase = $ancestor[0]
    }
    # Include both removed and added owners when a file is moved.
    $paths = @(& git diff --name-only --no-renames $comparisonBase $HeadRevision --)
    if ($LASTEXITCODE -ne 0) { throw 'The complete changed-file set could not be read.' }
}
$editor = $false
$reasons = [Collections.Generic.List[string]]::new()
foreach ($raw in $paths) {
    $path = $raw.Replace('\', '/')
    if ($path -match '\.(csproj|props|targets|sln|slnf)$') {
        if (Test-EditorReferenceVisibilityOnly $path $comparisonBase $HeadRevision) { $editor = $true }
        else { $reasons.Add("Build graph: $path") }
    } elseif ($path -match '^src/composition/editor/' -or
        $path -match '^src/runtime/scripting/Inno\.Scripting\.Reload/') {
        $editor = $true
    } elseif ($path -match '^tests/editor/' -or
        $path -match '^tests/scripting/Inno\.Editor\.Scripting\.Tests/' -or
        $path -match '^(docs|records)/' -or $path -eq 'AGENTS.md' -or
        $path -eq '.github/workflows/rendering-ci.yml' -or
        $path -eq 'tests/build/Inno.Build.Tests/BuildPipelineTests.ExportPresentation.cs' -or
        $path -match '^tests/ci/(Select-ValidationScope|Test-ValidationScope|New-ProductionSolutionFilter)\.ps1$' -or
        $path -eq 'tests/Invoke-EditorUiAcceptance.ps1') {
        # Companions do not broaden a bounded Editor production change.
    } else {
        $reasons.Add("Shared or unclassified owner: $path")
    }
}
if ($Full) { $reasons.Add('Full validation explicitly requested.') }
if (-not $editor) { $reasons.Add('No bounded Editor production change was identified.') }
$profile = if ($reasons.Count -eq 0) { 'editor' } else { 'full' }
[string[]]$scopeReasons = @('Only Editor authoring owners and their verification/documentation changed.')
if ($reasons.Count) { $scopeReasons = $reasons.ToArray() }
[string[]]$omitted = @()
if ($profile -eq 'editor') {
    $omitted = @('Editor Release', 'NativeAOT restore', 'ten hot builds', 'unrelated complete test suites', 'Player Support Pack generation')
}
[pscustomobject]@{
    profile = $profile
    changedFiles = @($paths)
    reasons = $scopeReasons
    omitted = $omitted
}
