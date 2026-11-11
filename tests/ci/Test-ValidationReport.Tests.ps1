$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath('../../artifacts/validation/report-tests', $PSScriptRoot)
$run = Join-Path $root ([Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $run -Force | Out-Null
$commit = 'a' * 40
$hash = 'b' * 64
$cases = [Collections.Generic.List[object]]::new()

function Invoke-Case([string]$Name, [scriptblock]$Change, [bool]$Accept) {
    $plan = @{
        commit = $commit; tree = $hash; environment = $hash; full = $false
        checks = @(@{ id = 'architecture'; kind = 'architecture'; prerequisites = @(); exclusion = $null })
    }
    $evidence = @{
        commit = $commit; tree = $hash; environment = $hash; completed = $true
        results = @(@{ id = 'architecture'; status = 'Passed'; inputIdentity = $hash; outputs = @{} })
    }
    $state = @{ plan = $plan; evidence = $evidence; conclusion = 'success'; expectedCommit = $commit; tamper = $false }
    & $Change $state
    $directory = Join-Path $run $Name
    New-Item -ItemType Directory -Path $directory | Out-Null
    $planFile = Join-Path $directory 'plan.json'
    $resultFile = Join-Path $directory 'results.json'
    $state.plan | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath $planFile -Encoding utf8NoBOM
    $state.evidence.planIdentity = (Get-FileHash -LiteralPath $planFile).Hash.ToLowerInvariant()
    $state.evidence | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath $resultFile -Encoding utf8NoBOM
    if ($state.tamper) { Add-Content -LiteralPath $planFile -Value ' ' }
    $accepted = $false
    $diagnostic = ''
    try {
        $null = & (Join-Path $PSScriptRoot 'Test-ValidationReport.ps1') -PlanPath $planFile -ResultsPath $resultFile `
            -ExpectedCommit $state.expectedCommit -JobConclusion $state.conclusion
        $accepted = $true
    } catch { $diagnostic = $_.Exception.Message }
    $cases.Add(@{ name = $Name; expectedAcceptance = $Accept; accepted = $accepted; diagnostic = $diagnostic })
    $cases | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $run 'tests.json') -Encoding utf8NoBOM
    if ($accepted -ne $Accept) { throw "Unexpected aggregation result for ${Name}: $diagnostic" }
}

Invoke-Case 'complete' { param($s) } $true
Invoke-Case 'unfinished-closing-verification' { param($s) $s.evidence.completed = $false } $false
Invoke-Case 'missing-completion' { param($s) $s.evidence.Remove('completed') } $false
Invoke-Case 'malformed-completion' { param($s) $s.evidence.completed = 'true' } $false
Invoke-Case 'missing-result' { param($s) $s.evidence.results = @() } $false
Invoke-Case 'skipped-required' { param($s) $s.evidence.results[0].status = 'NotApplicable' } $false
Invoke-Case 'canceled-job' { param($s) $s.conclusion = 'cancelled' } $false
Invoke-Case 'blocked-result' { param($s) $s.evidence.results[0].status = 'Blocked' } $false
Invoke-Case 'tampered-plan' { param($s) $s.tamper = $true } $false
Invoke-Case 'wrong-candidate' { param($s) $s.expectedCommit = 'c' * 40 } $false
Invoke-Case 'duplicate-result' { param($s) $s.evidence.results += $s.evidence.results[0] } $false
Invoke-Case 'empty-plan' { param($s) $s.plan.checks = @(); $s.evidence.results = @() } $false
Invoke-Case 'missing-output' { param($s) $s.plan.checks[0].kind = 'test' } $false
Invoke-Case 'compiled-with-dependency-inputs' {
    param($s)
    $s.plan.checks[0].kind = 'test'
    $s.evidence.results[0].outputs = @{ 'bin/example' = $hash }
    $s.evidence.results[0].dependencyIdentity = $hash
} $true
Invoke-Case 'compiled-missing-dependency-inputs' {
    param($s)
    $s.plan.checks[0].kind = 'test'
    $s.evidence.results[0].outputs = @{ 'bin/example' = $hash }
} $false
Invoke-Case 'compiled-malformed-dependency-inputs' {
    param($s)
    $s.plan.checks[0].kind = 'test'
    $s.evidence.results[0].outputs = @{ 'bin/example' = $hash }
    $s.evidence.results[0].dependencyIdentity = 'version-only'
} $false
Invoke-Case 'failed-prerequisite' { param($s) $s.plan.checks[0].prerequisites = @('missing-build') } $false
foreach ($kind in @('documentation-api', 'documentation-examples')) {
    Invoke-Case "$kind-complete" {
        param($s)
        $s.plan.checks[0].kind = $kind
        $s.plan.checks[0].inputs = @('@src/Example/Example.csproj', '@tooling')
        $s.evidence.results[0].outputs = @{ 'bin/example' = $hash }
        $s.evidence.results[0].dependencyIdentity = $hash
    } $true
    Invoke-Case "$kind-empty-outputs" {
        param($s)
        $s.plan.checks[0].kind = $kind
        $s.plan.checks[0].inputs = @('@src/Example/Example.csproj', '@tooling')
        $s.evidence.results[0].dependencyIdentity = $hash
    } $false
}
Invoke-Case 'explicit-inapplicable' {
    param($s)
    $s.plan.checks += @{ id = 'foreign-device'; kind = 'products'; exclusion = 'Requires another explicit device environment.' }
    $s.evidence.results += @{ id = 'foreign-device'; status = 'NotApplicable'; inputIdentity = $hash; outputs = $null }
} $true
Write-Output "Passed $($cases.Count) aggregate contract cases. Evidence: $run"
