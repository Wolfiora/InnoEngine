param(
    [Parameter(Mandatory)][string]$ReceivedDirectory,
    [Parameter(Mandatory)][string]$Platform,
    [Parameter(Mandatory)][string]$Shard,
    [Parameter(Mandatory)][int]$Attempt
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$received = [IO.Path]::GetFullPath($ReceivedDirectory, $repository)
$available = @(Get-ChildItem -LiteralPath $received -Directory | ForEach-Object { [pscustomobject]@{ name = $_.Name; directory = $_.FullName } })
$planArtifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-plan-$Platform" -MaximumAttempt $Attempt
$entryArtifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-entry-$Platform" -MaximumAttempt $Attempt
$plan = Get-Content -LiteralPath (Join-Path $planArtifact.directory 'plan.json') -Raw | ConvertFrom-Json
$identity = Get-Content -LiteralPath (Join-Path $planArtifact.directory 'entry.json') -Raw | ConvertFrom-Json
if ((Get-FileHash -LiteralPath (Join-Path $entryArtifact.directory 'Inno.Tooling.Cli.dll')).Hash -cne $identity.sha256) {
    throw 'The prepared entry does not match the selected frozen plan.'
}
$entryDirectory = [IO.Path]::GetDirectoryName([IO.Path]::GetFullPath($identity.assembly, $repository))
if ([IO.Path]::GetRelativePath($repository, $entryDirectory).Replace('\', '/') -cne 'artifacts/validation/bootstrap/outputs/bin/Inno.Tooling.Cli/debug') {
    throw 'The frozen entry escaped the validation bootstrap owner.'
}
$planDirectory = Join-Path $repository 'artifacts/validation/ci-plan'
$prerequisites = Join-Path $repository 'artifacts/validation/prerequisites'
foreach ($destination in @($planDirectory, $entryDirectory, $prerequisites)) {
    if (Test-Path -LiteralPath $destination) { throw 'A shard runner must start with a distinct transport owner.' }
    New-Item -ItemType Directory -Path $destination -Force | Out-Null
}
Copy-Item -LiteralPath (Join-Path $planArtifact.directory 'plan.json'), (Join-Path $planArtifact.directory 'entry.json') -Destination $planDirectory
Get-ChildItem -LiteralPath $entryArtifact.directory | Copy-Item -Destination $entryDirectory -Recurse
$selected = @($plan.shards | Where-Object id -CEQ $Shard)
if ($selected.Count -gt 1 -or ($selected.Count -eq 0 -and $Shard -cnotin @('managed', 'native'))) { throw 'The requested shard is unknown or duplicated.' }
foreach ($prerequisite in @($selected | ForEach-Object prerequisites)) {
    if ($prerequisite -cnotmatch '^[a-z][a-z0-9-]*$') { throw 'Invalid frozen prerequisite identity.' }
    $artifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-result-$Platform-$prerequisite" -MaximumAttempt $Attempt
    $destination = Join-Path $prerequisites $prerequisite
    New-Item -ItemType Directory -Path $destination | Out-Null
    Copy-Item -LiteralPath (Join-Path $artifact.directory 'results.json') -Destination $destination
}
"native=$(($Shard -ceq 'native' -and $selected.Count -eq 1 -and @($plan.nativeRequests).Count -gt 0).ToString().ToLowerInvariant())" >> $env:GITHUB_OUTPUT
