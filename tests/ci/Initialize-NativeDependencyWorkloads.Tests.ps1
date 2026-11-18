$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/workload-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$scripts = Join-Path $root 'tests/ci'
New-Item -ItemType Directory -Path $scripts, "$root/.github" -Force | Out-Null
Copy-Item "$PSScriptRoot/Initialize-NativeDependencyWorkloads.ps1" $scripts
'[{"target":"native","workloads":[]},{"target":"cross","workloads":["fixture-sdk"]}]' | Set-Content "$root/.github/native-targets.json"
$stub = Join-Path $root 'dotnet-fixture.ps1'
@'
$case = Get-Content "$PSScriptRoot/case.txt"
$global:LASTEXITCODE = 0
($args -join ' ') | Add-Content "$PSScriptRoot/calls.txt"
if ($args[0] -cne 'workload') { throw 'Workload preparation tried to compile an engine or third-party product.' }
if ($args[1] -ceq 'list') {
    if ($args -notcontains '--machine-readable') { throw 'Workload discovery must not parse localized text.' }
    @{ installed = @($(if ($case -eq 'warm') { 'fixture-sdk' })) } | ConvertTo-Json -Compress
} elseif ($args[1] -ceq 'install') {
    if ($args -notcontains '--skip-manifest-update' -or $args -notcontains 'fixture-sdk') { throw 'The declared SDK input was not selected.' }
    if ($case -eq 'failed') { $global:LASTEXITCODE = 1 }
} else { throw 'Unexpected workload operation.' }
'@ | Set-Content $stub
$passed = 0
foreach ($case in @('managed', 'native', 'warm', 'cold', 'failed', 'unknown')) {
    $case | Set-Content "$root/case.txt"
    $targets = switch ($case) { managed { @() }; native { @('native') }; unknown { @('unknown') }; default { @('cross') } }
    $countBefore = if (Test-Path "$root/calls.txt") { @(Get-Content "$root/calls.txt").Count } else { 0 }
    $accepted = $false
    try {
        $changed = & "$scripts/Initialize-NativeDependencyWorkloads.ps1" -Targets @($targets) -Dotnet $stub
        if ($changed -ne ($case -ceq 'cold')) { throw 'SDK preparation returned the wrong environment invalidation decision.' }
        $accepted = $true
    } catch { if ($case -notin @('failed', 'unknown')) { throw } }
    if ($accepted -ne ($case -notin @('failed', 'unknown'))) { throw "Unexpected workload result: $case" }
    $countAfter = if (Test-Path "$root/calls.txt") { @(Get-Content "$root/calls.txt").Count } else { 0 }
    if ($case -in @('managed', 'native', 'unknown') -and $countAfter -ne $countBefore) { throw 'Unneeded or invalid SDK work reached the tool process.' }
    $passed++
}
Write-Output "Workload preparation contracts passed: $passed."
