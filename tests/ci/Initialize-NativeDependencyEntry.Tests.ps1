$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$workflow = Get-Content (Join-Path $repository '.github/workflows/native-dependencies.yml') -Raw
$match = [regex]::Match($workflow, '(?s)- name: Prepare the managed Build entry.*?        run: \|\r?\n(?<script>.*?)(?=      - uses:)')
if (-not $match.Success) { throw 'The native producer has no executable entry preparation step.' }
$script = [regex]::Replace($match.Groups['script'].Value, '(?m)^          ', '')
$owner = Join-Path $repository ('artifacts/validation/native-entry-contracts/' + [Guid]::NewGuid().ToString('N'))
$root = Join-Path $owner 'workspace with spaces'
$project = Join-Path $root 'entry/InnoEngine.Build'
New-Item -ItemType Directory -Path $project, "$root/tests/ci", "$root/.github" -Force | Out-Null
'<Project />' | Set-Content "$root/Directory.Build.props"
'<Project />' | Set-Content "$root/Directory.Build.targets"
'<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>' |
    Set-Content "$project/InnoEngine.Build.csproj"
'System.Console.WriteLine("native-entry-path-contract");' | Set-Content "$project/Program.cs"
'return $false' | Set-Content "$root/tests/ci/Initialize-NativeDependencyWorkloads.ps1"
'[{"runner":"fixture-runner","target":"fixture-host"}]' | Set-Content "$root/.github/validation-hosts.json"
$original = (Get-Location).Path
$environment = @{}
foreach ($name in @('GITHUB_WORKSPACE', 'GITHUB_ENV', 'TARGET', 'VALIDATION_PLATFORM')) {
    $environment[$name] = [Environment]::GetEnvironmentVariable($name)
}
try {
    Set-Location -LiteralPath $root
    $env:GITHUB_WORKSPACE = $root
    $env:GITHUB_ENV = Join-Path $owner 'github-env.txt'
    $env:TARGET = 'fixture-target'
    $env:VALIDATION_PLATFORM = 'fixture-runner'
    & ([scriptblock]::Create($script)) *> (Join-Path $owner 'entry-preparation.log')
    $entry = @(Get-Content $env:GITHUB_ENV | Where-Object { $_.StartsWith('NATIVE_ENTRY=', [StringComparison]::Ordinal) })
    if ($entry.Count -ne 1) { throw 'Entry preparation did not export one exact entry.' }
    $path = $entry[0].Substring('NATIVE_ENTRY='.Length)
    $outputOwner = [IO.Path]::GetFullPath('artifacts/validation/native-entry', $root)
    if (-not [IO.Path]::IsPathRooted($path) -or -not $path.StartsWith($outputOwner + [IO.Path]::DirectorySeparatorChar, [StringComparison]::Ordinal) -or
        -not (Test-Path -LiteralPath $path -PathType Leaf)) { throw 'Entry discovery does not identify the artifact that the SDK actually built.' }
    $result = @(dotnet $path)
    if ($LASTEXITCODE -ne 0 -or $result.Count -ne 1 -or $result[0] -cne 'native-entry-path-contract') {
        throw 'The discovered producer entry is not executable.'
    }
} finally {
    Set-Location -LiteralPath $original
    foreach ($name in $environment.Keys) { [Environment]::SetEnvironmentVariable($name, $environment[$name]) }
}
Write-Output "Passed actual SDK producer entry discovery, including a workspace path with spaces. Evidence: $owner"
$global:LASTEXITCODE = 0
