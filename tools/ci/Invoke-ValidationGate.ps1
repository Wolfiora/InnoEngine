$ErrorActionPreference = 'Stop'
if ($env:ROUTE_RESULT -cne 'success') { throw 'Event routing did not succeed.' }
if ($env:SHOULD_RUN -ceq 'false') {
  if ($env:GITHUB_EVENT_NAME -cne 'push') { throw 'A required candidate gate cannot be delegated.' }
  'An open pull request owns this candidate validation.' >> $env:GITHUB_STEP_SUMMARY
  return
}
if ($env:PLAN_RESULT -cne 'success') { throw "Validation planning did not succeed (result: $env:PLAN_RESULT)." }
$direct = !$env:DISPATCH
$localChecks = $env:INDEPENDENT -and (($env:INDEPENDENT | ConvertFrom-Json).include.Count -gt 0)
if ($env:INDEPENDENT_RESULT -cne $(if ($localChecks) { 'success' } else { 'skipped' })) { throw 'Independent host execution did not match the plan.' }
if ($direct) {
  if ($env:MATRIX_RESULT -cne 'skipped') { throw 'Unexpected production matrix after all hosts completed independently.' }
  $env:BACKENDS_REQUIRED = 'false'
}
if ($env:SHOULD_RUN -cne 'true' -or (!$direct -and $env:MATRIX_RESULT -cne 'success') -or
    ($env:BACKENDS_REQUIRED -ceq 'true' -and $env:BACKENDS_RESULT -cne 'success') -or
    ($env:BACKENDS_REQUIRED -ceq 'false' -and $env:BACKENDS_RESULT -cne 'skipped') -or
    $env:BACKENDS_REQUIRED -cnotin @('true','false')) {
  throw 'One or more required execution jobs did not complete successfully.'
}
./tools/ci/Join-ValidationCi.ps1 -ReceivedDirectory artifacts/validation/received -Hosts (($env:HOSTS | ConvertFrom-Json).include.runner) `
  -ExpectedCommit $env:EXPECTED_COMMIT -Attempt ([int]$env:GITHUB_RUN_ATTEMPT)
