param(
    [Parameter(Mandatory)][string]$ReceivedDirectory,
    [Parameter(Mandatory)][string[]]$Hosts,
    [Parameter(Mandatory)][string]$ExpectedCommit,
    [Parameter(Mandatory)][int]$Attempt
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$received = [IO.Path]::GetFullPath($ReceivedDirectory, $repository)
$available = @(Get-ChildItem -LiteralPath $received -Directory | ForEach-Object { [pscustomobject]@{ name = $_.Name; directory = $_.FullName } })
if ($Hosts.Count -eq 0 -or @($Hosts | Sort-Object -Unique).Count -ne $Hosts.Count) { throw 'The required hosted matrix is empty or duplicated.' }
$routing = [ordered]@{}
$certification = foreach ($platform in $Hosts) {
    $selectedPlan = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-plan-$platform" -MaximumAttempt $Attempt
    $planPath = Join-Path $selectedPlan.directory 'plan.json'
    $plan = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json
    $joined = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-joined-$platform" -MaximumAttempt $Attempt
    $entry = Get-Content -LiteralPath (Join-Path $selectedPlan.directory 'entry.json') -Raw | ConvertFrom-Json
    $proof = Get-Content -LiteralPath (Join-Path $joined.directory 'receipt.json') -Raw | ConvertFrom-Json
    if ($proof.platform -cne $platform) { throw 'The host certification belongs to another platform.' }
    if ($entry.sha256 -cnotmatch '^[0-9A-F]{64}$' -or $proof.executor -cne $entry.sha256) { throw 'The host used another validation entry.' }
    if (@($plan.shards).Count -eq 0 -or @($plan.shards.id | Sort-Object -Unique).Count -ne @($plan.shards).Count -or
        @($proof.shards).Count -ne @($plan.shards).Count) { throw 'The host certified a different shard matrix.' }
    foreach ($shard in $plan.shards) {
        $record = @($proof.shards | Where-Object id -CEQ $shard.id)
        $artifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name "validation-result-$platform-$($shard.id)" -MaximumAttempt $Attempt
        $hash = (Get-FileHash -LiteralPath (Join-Path $artifact.directory 'results.json')).Hash.ToLowerInvariant()
        if ($record.Count -ne 1 -or $record[0].sha256 -cne $hash) { throw 'The host did not join the exact uploaded shard receipts.' }
    }
    $output = Join-Path $repository "artifacts/validation/joined/$platform"
    if (Test-Path -LiteralPath $output) { throw 'A gate attempt must retain distinct evidence.' }
    New-Item -ItemType Directory -Path $output | Out-Null
    Copy-Item -LiteralPath $planPath, (Join-Path $joined.directory 'results.json') -Destination $output
    $receipt = & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath (Join-Path $output 'plan.json') -ResultsPath (Join-Path $output 'results.json') `
        -ExpectedCommit $ExpectedCommit -JobConclusion success
    if ($proof.planIdentity -cne $receipt.planIdentity -or $proof.resultsIdentity -cne $receipt.resultsIdentity) {
        throw 'The frozen plan or its joined result changed during transport.'
    }
    "$platform`: $($receipt.passed) passed, $($receipt.reused) reused, $($receipt.notApplicable) explicitly not applicable." >> $env:GITHUB_STEP_SUMMARY
    $routing[$platform] = @($plan.hostExcludedSources | Where-Object { $_ })
    [pscustomobject]@{ platform = $platform; receipt = $receipt }
}
[IO.File]::WriteAllText((Join-Path $repository 'artifacts/validation/host-routing.json'),
    (ConvertTo-Json @{commit=$ExpectedCommit;hosts=$routing} -Depth 6 -Compress))
$certification | ConvertTo-Json -Depth 12 | Set-Content (Join-Path $repository 'artifacts/validation/certification.json')
if (@($certification | Where-Object { -not $_.receipt.full }).Count -eq 0) { 'INNO_FULL_CERTIFICATION=true' >> $env:GITHUB_ENV }
