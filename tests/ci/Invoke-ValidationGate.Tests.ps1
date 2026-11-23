$ErrorActionPreference = 'Stop'
$workflow = Get-Content (Join-Path $PSScriptRoot '../../.github/workflows/engine-ci.yml') -Raw
$gate = [regex]::Match($workflow, '(?ms)^  engine-validation:\r?\n.*?(?=^  [\w-]+:|\z)').Value
if ($gate -notmatch 'if: \$\{\{ !cancelled\(\) \}\}' -or $gate -notmatch 'run: \./tools/ci/Invoke-ValidationGate\.ps1' -or
    $gate -notmatch "'Branch validation' \|\| 'Engine validation'") { throw 'The required gate lost its cancellation, invocation or event naming contract.' }
$env:ROUTE_RESULT = 'success'
$env:SHOULD_RUN = 'true'
$env:GITHUB_EVENT_NAME = 'pull_request'
$env:DISPATCH = 'true'
$env:PLAN_RESULT = 'success'
$env:REUSED = 'false'
$env:MATRIX_RESULT = 'success'
$env:INDEPENDENT = '{"include":[]}'
$env:INDEPENDENT_RESULT = 'skipped'
$env:BACKENDS_REQUIRED = 'true'
$env:BACKENDS_RESULT = 'failure'
function Require-Rejection([string]$Reason) {
    try { & "$PSScriptRoot/../../tools/ci/Invoke-ValidationGate.ps1" } catch {
        if ($_.Exception.Message -notlike "*$Reason*") { throw }
        return
    }
    throw "An invalid candidate gate passed: $Reason"
}
Require-Rejection 'required execution jobs'
$env:PLAN_RESULT = 'failure'
Require-Rejection 'planning did not succeed'
$env:PLAN_RESULT = 'success'
$env:SHOULD_RUN = 'false'
Require-Rejection 'cannot be delegated'
$env:SHOULD_RUN = 'true'
$env:REUSED = 'true'
Require-Rejection 'Only scheduled'
Write-Output 'Passed active candidate, failed planning, PR delegation, reuse and workflow wiring checks.'
$global:LASTEXITCODE = 0
