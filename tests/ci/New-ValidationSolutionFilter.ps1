param(
    [Parameter(Mandatory)][string]$OutputPath,
    [string]$Solution = (Join-Path (Split-Path $PSScriptRoot -Parent) '../InnoEngine.sln')
)

$ErrorActionPreference = 'Stop'
$solutionPath = [IO.Path]::GetFullPath($Solution)
$root = Split-Path $solutionPath -Parent
$projects = [Collections.Generic.List[string]]::new()
foreach ($line in Get-Content -LiteralPath $solutionPath) {
    if ($line -notmatch '^Project\("[^"]+"\) = "[^"]+", "([^"]+\.csproj)"') { continue }
    $relative = $Matches[1].Replace('\', '/')
    if ($relative -notmatch '^(entry|src|backends|platforms|build|tools|tests)/') {
        throw "The solution contains an unclassified validation owner: $relative"
    }
    $projectPath = [IO.Path]::GetFullPath($relative, $root)
    $declaration = [xml](Get-Content -LiteralPath $projectPath -Raw)
    if ($declaration.Project.PropertyGroup.IsTestProject -contains 'true') { continue }
    $projects.Add($relative)
}
if ($projects.Count -eq 0) { throw 'No validation contracts were selected from the current solution.' }
$output = [IO.Path]::GetFullPath($OutputPath)
if (Test-Path -LiteralPath $output) { throw 'Use a new solution-filter output owner.' }
New-Item -ItemType Directory -Force (Split-Path $output -Parent) | Out-Null
@{ solution = @{ path = $solutionPath; projects = @($projects.ToArray()) } } |
    ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $output -Encoding utf8
Write-Output "Selected $($projects.Count) production and test-support contracts; executable test suites are excluded. MSBuild retains responsibility for the reference closure."
