param(
    [Parameter(Mandatory)][string] $EngineRoot,
    [Parameter(Mandatory)][string[]] $Projects,
    [Parameter(Mandatory)][ValidateSet('Debug', 'Release', 'SourceDebug')][string] $Configuration,
    [Parameter(Mandatory)][string] $Dotnet,
    [Parameter(Mandatory)][string] $ArtifactsPath,
    [Parameter(Mandatory)][string] $Output,
    [Parameter(Mandatory)][string] $Log
)
$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath($EngineRoot)
$solution = [Xml.XmlDocument]::new()
$solution.AppendChild($solution.CreateElement('Solution')) | Out-Null
# Declare the selected solution configuration and preserve it for transitive projects
# omitted from this bounded union. The SDK otherwise unsets their parent configuration.
$configurations = $solution.CreateElement('Configurations')
$buildType = $solution.CreateElement('BuildType')
$buildType.SetAttribute('Name', $Configuration)
$configurations.AppendChild($buildType) | Out-Null
$solution.DocumentElement.AppendChild($configurations) | Out-Null
if ($Projects.Count -eq 0) { throw 'Managed preparation requires actual project owners.' }
foreach ($project in $Projects | Sort-Object -Unique) {
    $absolute = [IO.Path]::GetFullPath($project, $root)
    $relative = [IO.Path]::GetRelativePath($root, $absolute)
    if ([IO.Path]::IsPathRooted($relative) -or $relative -eq '..' -or $relative.StartsWith('..' + [IO.Path]::DirectorySeparatorChar) -or
        [IO.Path]::GetExtension($absolute) -cne '.csproj' -or !(Test-Path -LiteralPath $absolute -PathType Leaf)) {
        throw "Managed preparation requires a real project within its selected source owner: $project"
    }
    $node = $solution.CreateElement('Project')
    $node.SetAttribute('Path', $absolute)
    $solution.DocumentElement.AppendChild($node) | Out-Null
}
$destination = [IO.Path]::GetFullPath($Output)
[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($destination)) | Out-Null
$pending = $destination + '.' + [Guid]::NewGuid().ToString('N') + '.pending'
try {
    $solution.Save($pending)
    [IO.File]::Move($pending, $destination, $true)
} finally {
    if ([IO.File]::Exists($pending)) { [IO.File]::Delete($pending) }
}
$logPath = [IO.Path]::GetFullPath($Log)
if ([IO.File]::Exists($logPath)) { throw 'Retain the previous preparation log and select a new evidence path.' }
[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($logPath)) | Out-Null
$watch = [Diagnostics.Stopwatch]::StartNew()
# Docs-owned solutions live outside the Engine's Directory.Solution.targets scope.
# One explicit invocation identity must reach every root and transitive reference.
$bootstrapOwner = [Guid]::NewGuid().ToString('N')
Write-Host "Preparing $($Projects.Count) managed roots in $Configuration; output owner: $ArtifactsPath."
& $Dotnet build $destination --configuration $Configuration --artifacts-path ([IO.Path]::GetFullPath($ArtifactsPath)) `
    '-p:InnoBuildPurpose=ApiReferences' '-p:ShouldUnsetParentConfigurationAndPlatform=false' `
    "-p:InnoTaskBootstrapOwner=$bootstrapOwner" `
    "-m:$([Math]::Min([Environment]::ProcessorCount, 4))" -nr:false --disable-build-servers 2>&1 |
    Tee-Object -FilePath $logPath | ForEach-Object { Write-Host $_ }
if ($LASTEXITCODE -ne 0) { throw "Managed API preparation failed; its first failure is retained in $logPath." }
Write-Host "$([IO.Path]::GetFileName($logPath)): $([Math]::Round($watch.Elapsed.TotalSeconds, 3)) seconds."
