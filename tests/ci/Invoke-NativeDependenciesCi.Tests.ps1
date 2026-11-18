$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/native-dependency-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @('download', 'cached', 'missing', 'corrupt', 'selection-failed', 'draft', 'digest-mismatch', 'managed')
$original = (Get-Location).Path
try {
    foreach ($case in $cases) {
        $root = Join-Path $owner $case
        $scripts = Join-Path $root 'tests/ci'
        $downloads = Join-Path $root 'artifacts/dependencies/downloads'
        New-Item -ItemType Directory -Path $scripts, $downloads -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/Invoke-NativeDependenciesCi.ps1" -Destination $scripts
        $entry = Join-Path $root 'entry.dll'
        'fixture' | Set-Content $entry
        $payload = [Text.Encoding]::UTF8.GetBytes('exact native archive transport fixture')
        $digest = [Convert]::ToHexStringLower([Security.Cryptography.SHA256]::HashData($payload))
        $asset = @{ release = 'native-deps-fixture-1'; asset = 'fixture.zip'; sha256 = $digest }
        $asset | ConvertTo-Json | Set-Content (Join-Path $root 'selected.json')
        $case | Set-Content (Join-Path $root 'case.txt')
        $requests = if ($case -ceq 'managed') { @() } else { @(@{ target = 'fixture'; configuration = 'Debug'; product = 'fixture' }) }
        @{ root = $root; nativeDependencies = 'selection.json'; nativeRequests = @($requests)
           contexts = @{ native = @{ properties = @{ InnoToolTarget = 'host-fixture' } } } } | ConvertTo-Json -Depth 6 | Set-Content (Join-Path $root 'plan.json')
        '{}' | Set-Content (Join-Path $root 'selection.json')
        $stub = Join-Path $root 'dotnet-fixture.ps1'
        @'
$arguments = @($args)
if ($arguments -contains 'produce' -or $arguments -contains 'build') { throw 'The consumer attempted production.' }
if ($arguments -notcontains 'select') { throw 'The consumer bypassed composition selection.' }
if ($arguments -notcontains '--tools-target' -or $arguments[[Array]::IndexOf($arguments, '--tools-target') + 1] -cne 'host-fixture') { throw 'The consumer lost its explicit host tools target.' }
if ((Get-Content (Join-Path $PSScriptRoot 'case.txt')).Trim() -ceq 'selection-failed') { $global:LASTEXITCODE = 1; return }
$destination = $arguments[[Array]::IndexOf($arguments, '--output') + 1]
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'selected.json') -Destination $destination
$global:LASTEXITCODE = 0
'@ | Set-Content $stub
        if ($case -cin @('cached', 'corrupt')) {
            [IO.File]::WriteAllBytes((Join-Path $downloads ($digest + '.zip')), $(if ($case -ceq 'cached') { $payload } else { [byte[]](1, 2, 3) }))
        }
        $calls = @{ requests = 0 }
        function Invoke-RestMethod {
            param($Uri, $Headers)
            $calls.requests++
            if ($Uri -cne 'https://api.github.com/repos/fixture/repository/releases/tags/native-deps-fixture-1') { throw 'Foreign release requested.' }
            @{ draft = ($case -ceq 'draft'); tag_name = 'native-deps-fixture-1'; assets = $(if ($case -ceq 'missing') { @() } else { @(@{ name = 'fixture.zip'; id = 12; state = 'uploaded' }) }) }
        }
        function Invoke-WebRequest {
            param($Uri, $Headers, $OutFile)
            if ($Uri -cne 'https://api.github.com/repos/fixture/repository/releases/assets/12') { throw 'Foreign asset requested.' }
            [IO.File]::WriteAllBytes($OutFile, $(if ($case -ceq 'digest-mismatch') { [byte[]](1, 2, 3) } else { $payload }))
        }
        $failed = $false
        try {
            & (Join-Path $scripts 'Invoke-NativeDependenciesCi.ps1') -Operation Download -EntryAssembly $entry -Dotnet $stub `
                -PlanPath (Join-Path $root 'plan.json') -Repository 'fixture/repository'
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
        $expectedFailure = $case -cnotin @('download', 'cached', 'managed')
        if ($failed -ne $expectedFailure) { throw "Unexpected dependency transport outcome: $case. See $root." }
        if ($case -cin @('cached', 'corrupt', 'selection-failed', 'managed') -and $calls.requests -ne 0) { throw 'Network used before selection or for an existing exact archive.' }
        if ($case -ceq 'download' -and $calls.requests -ne 1) { throw 'The selected release was not fetched exactly once.' }
        if ($case -cin @('download', 'cached') -and (Get-FileHash (Join-Path $downloads ($digest + '.zip'))).Hash.ToLowerInvariant() -cne $digest) { throw 'Wrong dependency download bytes.' }
        if ($case -ceq 'digest-mismatch' -and (Test-Path (Join-Path $downloads ($digest + '.zip')))) { throw 'Corrupt download became consumable.' }
    }
    foreach ($production in @('produce', 'production-failed')) {
        $root = Join-Path $owner $production
        $scripts = Join-Path $root 'tests/ci'
        New-Item -ItemType Directory -Path $scripts -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/Invoke-NativeDependenciesCi.ps1" -Destination $scripts
        $entry = Join-Path $root 'entry.dll'
        'fixture' | Set-Content $entry
        $stub = Join-Path $root 'dotnet-fixture.ps1'
        @'
$arguments = @($args)
foreach ($pair in @(@('--tools-target', 'host-fixture'), @('--target', 'cross-target'), @('--operation', 'produce'), @('--configuration', 'Release'))) {
    $index = [Array]::IndexOf($arguments, $pair[0])
    if ($index -lt 0 -or $arguments[$index + 1] -cne $pair[1]) { throw "Missing explicit production input: $($pair[0])" }
}
$product = $arguments[[Array]::IndexOf($arguments, '--product') + 1]
Add-Content (Join-Path $PSScriptRoot 'calls.txt') $product
$global:LASTEXITCODE = $(if ((Split-Path $PSScriptRoot -Leaf) -ceq 'production-failed') { 1 } else { 0 })
'@ | Set-Content $stub
        $failed = $false
        try {
            & (Join-Path $scripts 'Invoke-NativeDependenciesCi.ps1') -Operation Produce -EntryAssembly $entry -Dotnet $stub `
                -ToolsTarget host-fixture -Target cross-target -Products @('first', 'second') -Configuration Release `
                -Release native-deps-fixture-1 -OutputDirectory (Join-Path $root 'export')
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
        if ($failed -ne ($production -ceq 'production-failed')) { throw "Unexpected production adapter outcome: $production. See $root." }
        $actual = @(Get-Content (Join-Path $root 'calls.txt'))
        $expected = if ($failed) { 'first' } else { 'first,second' }
        if (($actual -join ',') -cne $expected) { throw 'Production did not preserve the first failure or exact product order.' }
    }
} finally {
    Set-Location -LiteralPath $original
}
Write-Output "Passed $($cases.Count + 2) native dependency transport/production adapter contracts. Native ABI execution remains a separate obligation. Evidence: $owner"
$global:LASTEXITCODE = 0
