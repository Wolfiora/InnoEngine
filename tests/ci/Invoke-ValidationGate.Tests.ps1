$ErrorActionPreference = 'Stop'
$workflow = Get-Content (Join-Path $PSScriptRoot '../../.github/workflows/engine-ci.yml') -Raw
$gate = [regex]::Match($workflow, '(?ms)^  engine-validation:\r?\n.*?(?=^  [\w-]+:|\z)').Value
if ($gate -notmatch 'if: \$\{\{ !cancelled\(\) \}\}' -or $gate -notmatch 'run: \./tools/ci/Invoke-ValidationGate\.ps1' -or
    $gate -notmatch "'Branch validation' \|\| 'Engine validation'") { throw 'The required gate lost its cancellation, invocation or event naming contract.' }
$env:ROUTE_RESULT = 'success'
$env:SHOULD_RUN = 'true'
$env:GITHUB_EVENT_NAME = 'pull_request'
function Require-Rejection([string]$Reason) {
    try { & "$PSScriptRoot/../../tools/ci/Invoke-ValidationGate.ps1" } catch {
        if ($_.Exception.Message -notlike "*$Reason*") { throw }
        return
    }
    throw "An invalid candidate gate passed: $Reason"
}
foreach ($conclusion in 'failure','skipped','cancelled') {
    $env:PLATFORM_RESULT = $conclusion
    Require-Rejection 'Required platform execution'
}
$env:PLATFORM_RESULT = 'success'
$env:SHOULD_RUN = 'false'
Require-Rejection 'cannot be delegated'
Write-Output 'Passed active candidate, failed planning, PR delegation and workflow wiring checks.'
$global:LASTEXITCODE = 0
