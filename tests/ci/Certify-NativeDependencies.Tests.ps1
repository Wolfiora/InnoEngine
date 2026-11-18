$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/native-certification-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @('valid', 'corrupt', 'unregistered', 'missing-target', 'failed-consumer', 'failed-plan')
foreach ($case in $cases) {
    $root = Join-Path $owner $case
    $scripts = Join-Path $root 'tests/ci'
    $package = Join-Path $root 'received/native-dependency-fixture'
    New-Item -ItemType Directory -Path $scripts, $package, "$root/.github", "$root/build/dependencies" -Force | Out-Null
    foreach ($name in @('Certify-NativeDependencies.ps1', 'Merge-NativeDependencySelection.ps1', 'Initialize-NativeDependencyWorkloads.ps1', 'Test-ValidationReport.ps1')) {
        Copy-Item (Join-Path $PSScriptRoot $name) $scripts
    }
    '[{"target":"fixture","runner":"host","workloads":[]}]' | Set-Content "$root/.github/native-targets.json"
    '{"assets":[],"aggregates":[]}' | Set-Content "$root/build/dependencies/native-artifacts.json"
    $revision = 'a' * 40
    $release = 'native-deps-fixture'
    'exact produced bytes' | Set-Content "$package/fixture.zip"
    $asset = @{ product = 'player'; component = 'fixture'; target = 'fixture'; configuration = 'release'; options = @(); release = $release;
        asset = 'fixture.zip'; sha256 = (Get-FileHash "$package/fixture.zip").Hash.ToLowerInvariant(); fingerprint = 'b' * 64; producerRevision = $revision }
    @{ assets = @($asset); aggregates = @() } | ConvertTo-Json -Depth 8 | Set-Content "$package/native-artifacts.json"
    if ($case -ceq 'corrupt') { 'modified' | Add-Content "$package/fixture.zip" }
    $case | Set-Content "$root/case.txt"
    @'
param($Operation, $EntryAssembly, $Dotnet, $PlanPath)
if ($Operation -cne 'Download') { throw 'Consumer certification attempted third-party production.' }
'downloaded' | Set-Content ([IO.Path]::GetFullPath('../../downloaded.txt', $PSScriptRoot))
'@ | Set-Content "$scripts/Invoke-NativeDependenciesCi.ps1"
    @'
$arguments = @($args)
$global:LASTEXITCODE = 0
$case = (Get-Content "$PSScriptRoot/case.txt").Trim()
($arguments -join ' ') | Add-Content "$PSScriptRoot/calls.txt"
if ($arguments[0] -ceq 'build') { 'managed entry' | Set-Content "$PSScriptRoot/entry.dll"; return }
if ($arguments[0] -ceq 'msbuild') { return "$PSScriptRoot/entry.dll" }
if ($arguments[1] -cne 'validate') { throw 'Unexpected certification command.' }
$output = $arguments[[Array]::IndexOf($arguments, '--output') + 1]
New-Item -ItemType Directory -Path $output -Force | Out-Null
if ($arguments[2] -ceq 'plan') {
    if ($case -ceq 'failed-plan') { $global:LASTEXITCODE = 1; return }
    @{ commit = 'a' * 40; tree = 'c' * 64; environment = 'd' * 64; full = $true;
       nativeRequests = @(@{ target = $(if ($case -ceq 'missing-target') { 'another' } else { 'fixture' }) });
       checks = @(@{ id = 'consumer'; kind = 'instructions'; exclusion = $null; prerequisites = @() }) } |
        ConvertTo-Json -Depth 8 | Set-Content "$output/plan.json"
    return
}
if ($arguments[2] -cne 'run' -or -not (Test-Path "$PSScriptRoot/downloaded.txt")) { throw 'Consumer execution bypassed selected artifact preparation.' }
if ($case -ceq 'failed-consumer') { $global:LASTEXITCODE = 1; return }
$plan = Get-Content "$output/plan.json" -Raw | ConvertFrom-Json
@{ commit = $plan.commit; tree = $plan.tree; environment = $plan.environment; full = $true; completed = $true;
   planIdentity = (Get-FileHash "$output/plan.json").Hash.ToLowerInvariant();
   results = @(@{ id = 'consumer'; status = 'Passed'; inputIdentity = 'e' * 64; outputs = @{} }) } |
    ConvertTo-Json -Depth 8 | Set-Content "$output/results.json"
'@ | Set-Content "$root/dotnet-fixture.ps1"
    $target = @{ target = 'fixture'; runner = $(if ($case -ceq 'unregistered') { 'other-host' } else { 'host' }) }
    $accepted = $false
    try {
        & "$scripts/Certify-NativeDependencies.ps1" -Directory "$root/received" -Targets @($target) -Platform host `
            -ProducerRevision $revision -Release $release -Dotnet "$root/dotnet-fixture.ps1"
        $accepted = $true
    } catch { if ($case -ceq 'valid') { throw } }
    if ($accepted -ne ($case -ceq 'valid')) { throw "Unexpected native certification result: $case" }
    $certificate = "$root/artifacts/dependencies/certification/fixture/native-producer.json"
    if ((Test-Path $certificate) -ne $accepted) { throw 'A failed or unexercised target acquired publication evidence.' }
    if ($case -in @('corrupt', 'unregistered') -and (Test-Path "$root/calls.txt")) { throw 'Invalid producer input reached consumer tools.' }
}
Write-Output "Native certification contracts passed: $($cases.Count)."
