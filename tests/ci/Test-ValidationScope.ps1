$ErrorActionPreference = 'Stop'
$policy = Join-Path $PSScriptRoot 'Select-ValidationScope.ps1'
$ui = 'src/composition/editor/presentation/Inno.Editor.ImGui/Runtime/EditorModalRenderer.cs'
$cases = @(
    @{ paths = @($ui); expected = 'editor' },
    @{ paths = @($ui, 'src/runtime/scripting/Inno.Scripting.Reload/ScriptReloadHost.cs', 'docs/editor/Inno.Editor.ImGui.md'); expected = 'editor' },
    @{ paths = @($ui, 'src/foundation/core/Inno.Core.IO/FileLease.cs'); expected = 'full' },
    @{ paths = @($ui, 'platforms/WindowsX64/build/Removed.cs'); expected = 'full' },
    @{ paths = @($ui, 'src/composition/editor/framework/Inno.Editor.Core/Inno.Editor.Core.csproj'); expected = 'full' },
    @{ paths = @($ui, '.github/actions/setup-dotnet/action.yml'); expected = 'full' },
    @{ paths = @($ui, 'tests/build/Inno.Build.Tests/BuildPipelineTests.ExportPresentation.cs'); expected = 'editor' },
    @{ paths = @('docs/README.md'); expected = 'full' },
    @{ paths = @('unknown.file'); expected = 'full' },
    @{ paths = @(); expected = 'full' }
)
foreach ($case in $cases) {
    $actual = & $policy -ChangedPath $case.paths
    if ($actual.profile -ne $case.expected) { throw "Unexpected selection for $($case.paths -join ', '): $($actual.profile)" }
}
if ((& $policy -ChangedPath @($ui) -Full).profile -ne 'full') { throw 'Manual full validation was not honored.' }
# Keep the tiny, test-owned Git fixture for inspection on failure.
$owner = Join-Path ([IO.Path]::GetTempPath()) ('InnoCiScope-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory $owner | Out-Null
$contractFilter = Join-Path $PSScriptRoot 'New-ValidationSolutionFilter.ps1'
$contractOutput = Join-Path $owner 'contracts.slnf'
& $contractFilter -OutputPath $contractOutput
$contracts = Get-Content -LiteralPath $contractOutput -Raw | ConvertFrom-Json
if ($contracts.solution.projects.Count -le 0) { throw 'The current contract solution filter is empty.' }
$solutionRoot = Split-Path $contracts.solution.path -Parent
foreach ($relative in $contracts.solution.projects) {
    $declaration = [xml](Get-Content -LiteralPath (Join-Path $solutionRoot $relative) -Raw)
    if ($declaration.Project.PropertyGroup.IsTestProject -contains 'true') {
        throw "An executable test suite entered the contract build: $relative"
    }
}
$trackedTests = @(& git -C $solutionRoot ls-files -- tests)
if ($LASTEXITCODE -ne 0) { throw 'The tracked test-contract inventory could not be read.' }
$expectedSupport = @($trackedTests | Where-Object {
    if ($_ -notlike '*.csproj') { return $false }
    $declaration = [xml](Get-Content -LiteralPath (Join-Path $solutionRoot $_) -Raw)
    return $declaration.Project.PropertyGroup.IsTestProject -notcontains 'true'
})
if ($expectedSupport.Count -eq 0) { throw 'The test contract fixture unexpectedly has no support owners.' }
foreach ($relative in $expectedSupport) {
    if ($contracts.solution.projects -notcontains $relative) { throw "A Wiki test-support contract is missing: $relative" }
}
$invalidSolution = Join-Path $owner 'invalid.sln'
'Project("{00000000-0000-0000-0000-000000000000}") = "Unknown", "unknown/Unknown.csproj", "{00000000-0000-0000-0000-000000000000}"' |
    Set-Content -LiteralPath $invalidSolution
$rejected = $false
try { & $contractFilter -Solution $invalidSolution -OutputPath (Join-Path $owner 'invalid.slnf') }
catch { $rejected = $_.Exception.Message -like '*unclassified validation owner*' }
if (-not $rejected) { throw 'An unknown production owner was silently omitted.' }
'No projects' | Set-Content -LiteralPath $invalidSolution
$rejected = $false
try { & $contractFilter -Solution $invalidSolution -OutputPath (Join-Path $owner 'empty.slnf') }
catch { $rejected = $_.Exception.Message -like '*No validation contracts*' }
if (-not $rejected) { throw 'An empty production filter was accepted.' }
Push-Location $owner
try {
    & git init --quiet
    if ($LASTEXITCODE -ne 0) { throw 'Git fixture initialization failed.' }
    New-Item -ItemType Directory -Force 'platforms/Example', 'src/composition/editor/presentation' | Out-Null
    'source' | Set-Content 'platforms/Example/Old.cs'
    & git add .
    & git -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit --quiet -m 'test(ci): seed owner'
    if ($LASTEXITCODE -ne 0) { throw 'Git fixture commit failed.' }
    $baseline = (& git rev-parse HEAD).Trim()
    Move-Item -LiteralPath 'platforms/Example/Old.cs' -Destination 'src/composition/editor/presentation/New.cs'
    & git add -A
    & git -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit --quiet -m 'test(ci): move owner'
    if ($LASTEXITCODE -ne 0) { throw 'Git fixture move failed.' }
    $moved = & $policy -BaseRevision $baseline
    if ($moved.profile -ne 'full' -or $moved.changedFiles -notcontains 'platforms/Example/Old.cs') { throw 'A moved owner escaped validation.' }
    & git checkout --quiet -b ui-only $baseline
    if ($LASTEXITCODE -ne 0) { throw 'The UI branch fixture could not be created.' }
    New-Item -ItemType Directory -Force 'src/composition/editor/presentation' | Out-Null
    'UI' | Set-Content 'src/composition/editor/presentation/Ui.cs'
    & git add .
    & git -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit --quiet -m 'test(ci): add editor change'
    if ($LASTEXITCODE -ne 0) { throw 'The UI fixture commit failed.' }
    $uiHead = (& git rev-parse HEAD).Trim()
    & git checkout --quiet -b base-advanced $baseline
    if ($LASTEXITCODE -ne 0) { throw 'The advanced base fixture could not be created.' }
    New-Item -ItemType Directory -Force 'src/foundation' | Out-Null
    'Core' | Set-Content 'src/foundation/Core.cs'
    & git add .
    & git -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit --quiet -m 'test(ci): advance shared base'
    if ($LASTEXITCODE -ne 0) { throw 'The advanced base fixture commit failed.' }
    $baseHead = (& git rev-parse HEAD).Trim()
    if ((& $policy -BaseRevision $baseHead -HeadRevision $uiHead -PullRequest).profile -ne 'editor') {
        throw 'Unrelated base commits broadened pull request validation.'
    }
    if ((& $policy -BaseRevision $baseHead -HeadRevision $uiHead).profile -ne 'full') {
        throw 'A direct push comparison lost its removed shared owner.'
    }
    & git checkout --quiet ui-only
    if ($LASTEXITCODE -ne 0) { throw 'The reference fixture branch is unavailable.' }
    $project = 'src/composition/editor/presentation/Ui.csproj'
    '<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup><ItemGroup>' +
        [Environment]::NewLine + '  <ProjectReference Include="../Dependency/Dependency.csproj" />' +
        [Environment]::NewLine + '</ItemGroup></Project>' | Set-Content $project
    & git add .
    & git -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit --quiet -m 'test(ci): seed reference boundary'
    if ($LASTEXITCODE -ne 0) { throw 'The reference baseline commit failed.' }
    $referenceBase = (& git rev-parse HEAD).Trim()
    (Get-Content $project -Raw).Replace('Dependency.csproj" />', 'Dependency.csproj" PrivateAssets="compile" />') | Set-Content $project -NoNewline
    & git add .
    & git -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit --quiet -m 'test(ci): tighten editor visibility'
    if ($LASTEXITCODE -ne 0) { throw 'The visibility fixture commit failed.' }
    if ((& $policy -BaseRevision $referenceBase).profile -ne 'editor') { throw 'A proved visibility-only correction broadened validation.' }
    (Get-Content $project -Raw).Replace('net10.0', 'net11.0') | Set-Content $project -NoNewline
    & git add .
    & git -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit --quiet -m 'test(ci): change framework'
    if ($LASTEXITCODE -ne 0) { throw 'The framework fixture commit failed.' }
    if ((& $policy -BaseRevision $referenceBase).profile -ne 'full') { throw 'A framework change escaped full validation.' }
} finally {
    Pop-Location
}
$resolved = [IO.Path]::GetFullPath($owner)
$parent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
if (-not $resolved.StartsWith($parent, [StringComparison]::OrdinalIgnoreCase) -or
    [IO.Path]::GetFileName($resolved) -notlike 'InnoCiScope-*') { throw 'Invalid fixture cleanup owner.' }
Remove-Item -LiteralPath $resolved -Recurse -Force
Write-Output 'Validation scope policy: 20 cases passed, including test-support contracts, visibility proofs, real moves and PR bases.'
