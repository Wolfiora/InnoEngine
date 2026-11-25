$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/ci-shard-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @('managed', 'absent-native', 'absent-architecture', 'duplicate-shard', 'wrong-candidate', 'changed-entry', 'missing-prerequisite', 'failed-execution')
$originalOutput = $env:GITHUB_OUTPUT
$originalDirectory = (Get-Location).Path
try {
    foreach ($case in $cases) {
        $root = Join-Path $directory $case
        $scripts = Join-Path $root 'tools/ci'
        $plan = Join-Path $root 'artifacts/validation/ci-plan'
        New-Item -ItemType Directory -Path $scripts, $plan -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/../../tools/ci/Invoke-ValidationCi.ps1" -Destination $scripts
        "artifacts/`noutputs.txt`n" | Set-Content (Join-Path $root '.gitignore')
        'entry fixture' | Set-Content (Join-Path $root 'entry.dll')
        $case | Set-Content (Join-Path $root 'case.txt')
        $stub = Join-Path $root 'dotnet-fixture.ps1'
        @'
$arguments = @($args)
if ($arguments[1] -cne 'validate' -or $arguments[2] -cne 'run' -or $arguments -notcontains '--shard') {
    throw 'Shard execution repeated planning, reconciliation or native production.'
}
$output = $arguments[[Array]::IndexOf($arguments, '--output') + 1]
'executed once' | Add-Content (Join-Path $output 'calls.txt')
$global:LASTEXITCODE = if ((Get-Content (Join-Path $PSScriptRoot 'case.txt')).Trim() -ceq 'failed-execution') { 1 } else { 0 }
'@ | Set-Content $stub
        & git -C $root init -q
        & git -C $root add .
        & git -C $root -c user.name='CI Contract Fixture' -c user.email='ci-fixture@example.invalid' commit -qm 'Shard contract fixture'
        if ($LASTEXITCODE -ne 0) { throw 'Cannot commit the shard fixture.' }
        $commit = (& git -C $root rev-parse HEAD).Trim()
        $digest = (Get-FileHash (Join-Path $root 'entry.dll')).Hash
        @{ assembly = 'entry.dll'; sha256 = $(if ($case -ceq 'changed-entry') { 'A' * 64 } else { $digest }) } |
            ConvertTo-Json | Set-Content (Join-Path $plan 'entry.json')
        $shard = if ($case -ceq 'absent-native') { 'native' } elseif ($case -ceq 'absent-architecture') { 'architecture' } else { 'managed' }
        $dependencies = @()
        if ($case -ceq 'missing-prerequisite') { $dependencies += 'architecture' }
        $shards = @(if ($case -notlike 'absent-*') { @{ id = 'managed'; checks = @('fixture'); prerequisites = @($dependencies) } })
        if ($case -ceq 'duplicate-shard') { $shards += $shards[0] }
        @{ commit = $(if ($case -ceq 'wrong-candidate') { 'a' * 40 } else { $commit }); checks = @(); shards = @($shards); nativeRequests = @() } |
            ConvertTo-Json -Depth 6 | Set-Content (Join-Path $plan 'plan.json')
        $env:GITHUB_OUTPUT = Join-Path $root 'outputs.txt'
        [IO.File]::WriteAllText($env:GITHUB_OUTPUT, '')
        $failed = $false
        try {
            & (Join-Path $scripts 'Invoke-ValidationCi.ps1') -Stage Run -BaseRevision $commit -ExpectedCommit $commit -Dotnet $stub `
                -Shard $shard -PlanDirectory artifacts/validation/ci-plan -OutputDirectory artifacts/validation/result
        } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $plan 'failure.log') }
        if ($failed -ne ($case -cnotin @('managed', 'absent-native'))) { throw "Unexpected shard adapter outcome: $case. See $root." }
        $calls = Join-Path $root 'artifacts/validation/result/calls.txt'
        $expectedCalls = if ($case -cin @('managed', 'failed-execution')) { 1 } else { 0 }
        $actualCalls = if (Test-Path -LiteralPath $calls) { @(Get-Content -LiteralPath $calls).Count } else { 0 }
        if ($actualCalls -ne $expectedCalls) { throw 'The adapter executed unexpected work or repeated execution.' }
        if ($case -ceq 'absent-native' -and (Get-Content $env:GITHUB_OUTPUT) -cne 'selected=false') { throw 'An absent optional shard was not explicitly reported.' }
    }
} finally {
    $env:GITHUB_OUTPUT = $originalOutput
    Set-Location -LiteralPath $originalDirectory
}
Write-Output "Passed $($cases.Count) shard adapter contracts. Real execution and receipt validity are tested by the validation integration suite. Evidence: $directory"
$global:LASTEXITCODE = 0
