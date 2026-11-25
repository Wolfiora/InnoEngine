$ErrorActionPreference = 'Stop'
if ($env:ROUTE_RESULT -cne 'success') { throw 'Event routing did not succeed.' }
if ($env:SHOULD_RUN -ceq 'false') {
  if ($env:GITHUB_EVENT_NAME -cne 'push') { throw 'A required candidate gate cannot be delegated.' }
  'An open pull request owns this candidate validation.' >> $env:GITHUB_STEP_SUMMARY
  return
}
if ($env:SHOULD_RUN -cne 'true' -or $env:PLATFORM_RESULT -cne 'success') { throw "Required platform execution did not succeed ($env:PLATFORM_RESULT)." }
./tools/ci/Join-ValidationCi.ps1 -ReceivedDirectory artifacts/validation/received -Hosts (($env:HOSTS | ConvertFrom-Json).include.runner) `
  -ExpectedCommit $env:EXPECTED_COMMIT -Attempt ([int]$env:GITHUB_RUN_ATTEMPT)
