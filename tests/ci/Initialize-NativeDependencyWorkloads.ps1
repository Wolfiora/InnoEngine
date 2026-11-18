param(
    [Parameter(Mandatory)][AllowEmptyCollection()][string[]]$Targets,
    [string]$Dotnet = 'dotnet'
)
$ErrorActionPreference = 'Stop'
$registered = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
if (@($registered.target | Sort-Object -Unique).Count -ne $registered.Count) { throw 'Native target registration is duplicated.' }
$workloads = @($Targets | Sort-Object -Unique | ForEach-Object {
    $target = $_
    $registration = @($registered | Where-Object target -CEQ $target)
    if ($registration.Count -ne 1) { throw "The selected native target has no execution registration: $target" }
    $registration[0].workloads
} | Sort-Object -Unique)
if ($workloads.Count -eq 0) { return $false }
$inventory = @(& $Dotnet workload list --machine-readable)
if ($LASTEXITCODE -ne 0) { throw 'SDK workload inventory failed.' }
$installed = ($inventory -join "`n" | ConvertFrom-Json).installed
$missing = @($workloads | Where-Object { $_ -cnotin $installed })
if ($missing.Count -eq 0) { return $false }
& $Dotnet workload install @missing --skip-manifest-update --disable-parallel | Out-Host
if ($LASTEXITCODE -ne 0) { throw 'The selected target SDK workloads could not be prepared.' }
return $true
