param(
    [Parameter(Mandatory)][string]$PlanPath,
    [Parameter(Mandatory)][string]$ResultsPath,
    [Parameter(Mandatory)][string]$ExpectedCommit,
    [Parameter(Mandatory)][string]$JobConclusion
)

$ErrorActionPreference = 'Stop'
if ($JobConclusion -cne 'success') { throw "The required producer job did not succeed: $JobConclusion" }
if ($ExpectedCommit -cnotmatch '^[0-9a-f]{40}$') { throw 'Certification requires an exact commit SHA.' }
$plan = Get-Content -LiteralPath $PlanPath -Raw | ConvertFrom-Json
$evidence = Get-Content -LiteralPath $ResultsPath -Raw | ConvertFrom-Json
if ($evidence.completed -isnot [bool] -or -not $evidence.completed) {
    throw 'The run did not complete its closing input, environment and output verification.'
}
$identity = (Get-FileHash -LiteralPath $PlanPath -Algorithm SHA256).Hash.ToLowerInvariant()
if ($evidence.planIdentity -cne $identity) { throw 'The results do not certify the exact uploaded plan bytes.' }
if ($plan.commit -cne $ExpectedCommit -or $evidence.commit -cne $ExpectedCommit) { throw 'The evidence belongs to another commit or merge candidate.' }
if ($plan.tree -cnotmatch '^[0-9a-f]{64}$' -or $evidence.tree -cne $plan.tree -or
    $plan.environment -cnotmatch '^[0-9a-f]{64}$' -or $evidence.environment -cne $plan.environment) {
    throw 'The source or execution environment identity is missing or inconsistent.'
}
$checks = @($plan.checks)
$results = @($evidence.results)
if ($checks.Count -eq 0 -or @($checks | Where-Object { $null -eq $_.exclusion }).Count -eq 0 -or $checks.Count -ne $results.Count) {
    throw 'The required plan is empty or results are missing or unplanned.'
}
$seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
foreach ($check in $checks) {
    if ([string]::IsNullOrWhiteSpace($check.id) -or -not $seen.Add($check.id)) { throw 'The plan contains an empty or duplicated check identity.' }
    $matches = @($results | Where-Object { $_.id -ceq $check.id })
    if ($matches.Count -ne 1) { throw "Required result is absent or duplicated: $($check.id)" }
    $result = $matches[0]
    if ($result.inputIdentity -cnotmatch '^[0-9a-f]{64}$') { throw "Input evidence is missing: $($check.id)" }
    if ($null -ne $check.exclusion) {
        if ($result.status -cne 'NotApplicable' -or [string]::IsNullOrWhiteSpace($check.exclusion)) {
            throw "An excluded check lacks its explicit applicability result: $($check.id)"
        }
    } elseif ($result.status -cnotin @('Passed', 'Reused')) {
        throw "Required check did not pass: $($check.id) ($($result.status))"
    } else {
        if ($null -eq $result.outputs) { throw "Required output evidence is missing: $($check.id)" }
        $compiled = $check.kind -cin @('build', 'test', 'api', 'products', 'support-pack', 'release-bundle') -or
            ($check.kind -ceq 'wiki' -and @($check.inputs | Where-Object { $_ -like '@*' -and $_ -cne '@tooling' }).Count -gt 0)
        if ($compiled -and $result.dependencyIdentity -cnotmatch '^[0-9a-f]{64}$') {
            throw "Restored dependency evidence is missing: $($check.id)"
        }
        if ($check.kind -in @('build', 'test', 'api', 'products') -and @($result.outputs.PSObject.Properties).Count -eq 0) {
            throw "Compiled output discovery is empty: $($check.id)"
        }
        foreach ($output in $result.outputs.PSObject.Properties) {
            if ([IO.Path]::IsPathRooted($output.Name) -or $output.Name -match '(^|[\\/])\.\.([\\/]|$)' -or $output.Value -cnotmatch '^[0-9a-f]{64}$') {
                throw "Invalid output identity in $($check.id): $($output.Name)"
            }
        }
        foreach ($dependency in @($check.prerequisites)) {
            $required = @($results | Where-Object { $_.id -ceq $dependency -and $_.status -cin @('Passed', 'Reused') })
            if ($required.Count -ne 1) { throw "A prerequisite did not pass: $($check.id) -> $dependency" }
        }
    }
}
[pscustomobject]@{
    commit = $ExpectedCommit
    tree = $plan.tree
    environment = $plan.environment
    full = [bool]$plan.full
    planIdentity = $identity
    resultsIdentity = (Get-FileHash -LiteralPath $ResultsPath -Algorithm SHA256).Hash.ToLowerInvariant()
    required = @($checks | Where-Object { $null -eq $_.exclusion }).Count
    passed = @($results | Where-Object status -CEQ 'Passed').Count
    reused = @($results | Where-Object status -CEQ 'Reused').Count
    notApplicable = @($results | Where-Object status -CEQ 'NotApplicable').Count
    timings = @($checks | Where-Object { $null -eq $_.exclusion } | ForEach-Object {
        $check = $_
        $result = $results | Where-Object id -CEQ $check.id
        if ($null -ne $result.seconds -and $result.status -ceq 'Passed') {
            [pscustomobject]@{ id = $check.id; kind = $check.kind; owner = $check.owner; seconds = [double]$result.seconds }
        }
    })
}
