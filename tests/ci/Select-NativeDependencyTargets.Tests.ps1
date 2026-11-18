$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$workflow = Get-Content (Join-Path $repository '.github/workflows/native-dependencies.yml') -Raw
$match = [regex]::Match($workflow, '(?s)      - id: select.*?        run: \|\r?\n(?<script>.*?)(?=  produce:)')
if (-not $match.Success) { throw 'The native producer has no executable target selection step.' }
$script = [regex]::Replace($match.Groups['script'].Value, '(?m)^          ', '')
$owner = Join-Path $repository ('artifacts/validation/native-target-contracts/' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path "$owner/.github" -Force | Out-Null
'[{"runner":"host"}]' | Set-Content "$owner/.github/validation-hosts.json"
$original = (Get-Location).Path
$environment = @{}
foreach ($name in @('GITHUB_OUTPUT', 'GITHUB_RUN_ID', 'GITHUB_RUN_ATTEMPT', 'TARGETS', 'CONFIGURATIONS')) {
    $environment[$name] = [Environment]::GetEnvironmentVariable($name)
}
$cases = @('complete', 'release', 'unsupported', 'duplicate-request', 'unknown-target', 'empty-registration', 'duplicate-registration', 'duplicate-target')
try {
    Set-Location -LiteralPath $owner
    $env:GITHUB_RUN_ID = '123'
    $env:GITHUB_RUN_ATTEMPT = '1'
    foreach ($case in $cases) {
        $registrations = @(
            @{ target = 'desktop'; runner = 'host'; workloads = @(); configurations = @('Debug', 'Release') },
            @{ target = 'cross'; runner = 'host'; workloads = @('cross-sdk'); configurations = @('Release') }
        )
        if ($case -ceq 'empty-registration') { $registrations[1].configurations = @() }
        if ($case -ceq 'duplicate-registration') { $registrations[1].configurations = @('Release', 'Release') }
        if ($case -ceq 'duplicate-target') { $registrations += $registrations[1] }
        ConvertTo-Json -InputObject $registrations -Depth 4 | Set-Content "$owner/.github/native-targets.json"
        $env:GITHUB_OUTPUT = Join-Path $owner "$case-output.txt"
        $env:TARGETS = switch ($case) { unsupported { 'cross' }; 'unknown-target' { 'unknown' }; default { '' } }
        $env:CONFIGURATIONS = switch ($case) { release { 'Release' }; unsupported { 'Debug' }; 'duplicate-request' { 'Debug,Debug' }; default { 'Debug,Release' } }
        $failed = $false
        try { & ([scriptblock]::Create($script)) } catch { $failed = $true; $_ | Out-String | Set-Content "$owner/$case-failure.log" }
        if ($failed -ne ($case -cnotin @('complete', 'release'))) { throw "Unexpected target matrix selection result: $case" }
        if ($failed) {
            if (Test-Path -LiteralPath $env:GITHUB_OUTPUT) { throw 'Invalid production selection reached job scheduling.' }
            continue
        }
        $output = @(Get-Content $env:GITHUB_OUTPUT | Where-Object { $_.StartsWith('hosts=', [StringComparison]::Ordinal) })
        if ($output.Count -ne 1) { throw 'Producer selection did not export one matrix.' }
        $matrix = ($output[0].Substring('hosts='.Length) | ConvertFrom-Json).include
        if (@($matrix).Count -ne 2 -or ($matrix.target -join ',') -cne 'desktop,cross') { throw 'Production target ownership changed.' }
        $desktop = if ($case -ceq 'release') { 'Release' } else { 'Debug,Release' }
        if (($matrix[0].configurations -join ',') -cne $desktop -or ($matrix[1].configurations -join ',') -cne 'Release' -or
            ($matrix[1].workloads -join ',') -cne 'cross-sdk') { throw 'Production expanded beyond a target configuration or lost its workload.' }
    }
} finally {
    Set-Location -LiteralPath $original
    foreach ($name in $environment.Keys) { [Environment]::SetEnvironmentVariable($name, $environment[$name]) }
}
Write-Output "Passed $($cases.Count) target-owned native production matrix contracts. Evidence: $owner"
$global:LASTEXITCODE = 0
