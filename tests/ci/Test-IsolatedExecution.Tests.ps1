$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/isolated-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $directory -Force | Out-Null
$cases = @(
    @{ name = 'executed'; executed = 1; passed = 1; failed = 0; method = 'Run'; outcome = 'Passed'; accept = $true },
    @{ name = 'empty'; executed = 0; passed = 0; failed = 0; method = 'Run'; outcome = 'Passed'; accept = $false },
    @{ name = 'failed'; executed = 1; passed = 0; failed = 1; method = 'Run'; outcome = 'Failed'; accept = $false },
    @{ name = 'skipped'; executed = 1; passed = 0; failed = 0; method = 'Run'; outcome = 'NotExecuted'; accept = $false },
    @{ name = 'wrong-contract'; executed = 1; passed = 1; failed = 0; method = 'Other'; outcome = 'Passed'; accept = $false },
    @{ name = 'inconsistent-counters'; executed = 2; passed = 2; failed = 0; method = 'Run'; outcome = 'Passed'; accept = $false },
    @{ name = 'missing'; executed = 1; passed = 1; failed = 0; method = 'Run'; outcome = 'Passed'; accept = $false }
)
foreach ($case in $cases) {
    $report = Join-Path $directory ($case.name + '.trx')
    if ($case.name -ne 'missing') {
        @"
<TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
<Results><UnitTestResult outcome="$($case.outcome)" /></Results>
<TestDefinitions><UnitTest><TestMethod className="Inno.Contracts" name="$($case.method)" /></UnitTest></TestDefinitions>
<ResultSummary><Counters executed="$($case.executed)" passed="$($case.passed)" failed="$($case.failed)" error="0" /></ResultSummary>
</TestRun>
"@ | Set-Content -LiteralPath $report
    }
    $accepted = $false
    try {
        & "$PSScriptRoot/Test-IsolatedExecution.ps1" -Report $report -Contract 'Inno.Contracts.Run'
        $accepted = $true
    } catch { $_.Exception.Message | Set-Content -LiteralPath (Join-Path $directory ($case.name + '.log')) }
    if ($accepted -ne $case.accept) { throw "Unexpected isolated contract result: $($case.name)" }
}
Write-Output "Passed $($cases.Count) isolated execution contract cases. Evidence: $directory"
