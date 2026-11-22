param(
    [Parameter(Mandatory)][string]$BaseRevision,
    [Parameter(Mandatory)][string]$ExpectedCommit,
    [Parameter(Mandatory)][ValidateSet('Plan', 'Run', 'Join')][string]$Stage,
    [string]$Dotnet = 'dotnet',
    [ValidateSet('fast', 'pull-request', 'master', 'full')][string]$Profile = 'pull-request',
    [string]$Shard,
    [string]$Platform,
    [string]$PlanDirectory = 'artifacts/validation/ci',
    [string]$OutputDirectory = 'artifacts/validation/ci',
    [string]$PrerequisitesDirectory,
    [switch]$Scheduled,
    [string]$BackendStore = 'artifacts/b',
    [long]$SourceRunId = 0
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
Set-Location -LiteralPath $repository
$actual = (& git rev-parse HEAD).Trim()
if ($LASTEXITCODE -ne 0 -or $actual -cne $ExpectedCommit) { throw 'Checkout is not the requested merge candidate.' }
$planRoot = [IO.Path]::GetFullPath($PlanDirectory, $repository)
$output = [IO.Path]::GetFullPath($OutputDirectory, $repository)
foreach ($owned in @($planRoot, $output)) {
    $relative = [IO.Path]::GetRelativePath($repository, $owned)
    if ([IO.Path]::IsPathRooted($relative) -or $relative -notmatch '^artifacts[\\/]validation[\\/]') { throw 'CI output must belong to the explicit validation artifact owner.' }
}
if ($Stage -ceq 'Plan') {
    $matched = $false
    if ($Scheduled) {
        $planPath = Join-Path $planRoot 'plan.json'
        $plan = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json
        $base = (& git merge-base $BaseRevision HEAD).Trim()
        if ($LASTEXITCODE -or $plan.baseline -cne $base -or $plan.commit -cne $ExpectedCommit) { throw 'Scheduled plan belongs to a different candidate or baseline.' }
        $entry = Get-Content -LiteralPath (Join-Path $planRoot 'entry.json') -Raw | ConvertFrom-Json
        $assembly = @([IO.Path]::GetFullPath($entry.assembly, $repository))
        $bootstrap = [IO.Path]::GetFullPath('artifacts/validation/bootstrap/outputs/bin', $repository) + [IO.Path]::DirectorySeparatorChar
        if (!$assembly[0].StartsWith($bootstrap, [StringComparison]::Ordinal) -or (Get-FileHash -LiteralPath $assembly[0]).Hash -cne $entry.sha256) {
            throw 'Scheduled validation entry does not match its registered output.'
        }
        if (@($plan.nativeRequests).Count) { $null = & "$PSScriptRoot/Initialize-NativeDependencyWorkloads.ps1" -Targets @($plan.nativeRequests.target) -Dotnet $Dotnet }
        & $Dotnet $assembly[0] validate plan --engine-root $repository --dotnet $Dotnet --plan $planPath --profile $Profile --output $planRoot
        if ($LASTEXITCODE -notin @(0, 3)) { throw 'Could not bind the scheduled plan.' }
        $matched = $LASTEXITCODE -eq 0
        if (!$matched) { Move-Item -LiteralPath $planPath -Destination (Join-Path $planRoot 'plan-before-environment-change.json') }
    } else {
        if (Test-Path -LiteralPath $planRoot) { throw 'A CI attempt must retain a distinct plan directory.' }
        New-Item -ItemType Directory -Path $planRoot | Out-Null
        $assembly = @(& "$PSScriptRoot/Initialize-ValidationEntry.ps1" -Dotnet $Dotnet -LogPath (Join-Path $planRoot 'bootstrap.log'))
        @{ assembly = [IO.Path]::GetRelativePath($repository, $assembly[0]); sha256 = (Get-FileHash -LiteralPath $assembly[0]).Hash } |
            ConvertTo-Json | Set-Content (Join-Path $planRoot 'entry.json')
    }
    if (!$matched) {
        $baselineOptions = @()
        if ($env:INNO_VALIDATION_RUNNER -and $env:GH_TOKEN) {
            $base = (& git merge-base $BaseRevision HEAD).Trim()
            if ($LASTEXITCODE) { throw 'Validation baseline is unavailable.' }
            $removed = @(& git diff --no-renames --diff-filter=D --name-only $base HEAD)
            if ($LASTEXITCODE) { throw 'Could not enumerate removed validation inputs.' }
            if ($removed.Count) {
                try {
                    $headers = @{Authorization="Bearer $env:GH_TOKEN"}
                    $api = "$env:GITHUB_API_URL/repos/$env:GITHUB_REPOSITORY/actions/workflows/engine-ci.yml/runs"
                    $page = Invoke-RestMethod -Headers $headers -Uri "${api}?head_sha=$base&event=push&status=success&per_page=20"
                    $run = $page.workflow_runs | Where-Object head_sha -CEQ $base | Select-Object -First 1
                    if ($run) {
                        $prior = $planRoot + '-baseline'
                        & "$PSScriptRoot/Get-ValidationContinuation.ps1" -RunId $run.id -Commit $base -Platform $env:INNO_VALIDATION_RUNNER -Shard architecture -PlanOnly `
                            -Repository $env:GITHUB_REPOSITORY -ApiUrl $env:GITHUB_API_URL -OutputDirectory $prior
                        $baselineOptions = @('--baseline-plan',(Get-Content (Join-Path $prior 'continuation.json') -Raw | ConvertFrom-Json).plan)
                    }
                } catch { Write-Warning "Previous SDK graph unavailable. The planner will widen validation. $($_.Exception.Message)" }
            }
        }
        & $Dotnet $assembly[0] validate plan --engine-root $repository --dotnet $Dotnet --base $BaseRevision --output $planRoot @baselineOptions `
            --profile $Profile --backend-store $BackendStore 2>&1 | Tee-Object -FilePath (Join-Path $planRoot 'planning.log')
        if ($LASTEXITCODE -ne 0) { throw 'Validation plan generation failed; no downstream stage can certify success.' }
        $plan = Get-Content -LiteralPath (Join-Path $planRoot 'plan.json') -Raw | ConvertFrom-Json
        if (@($plan.nativeRequests).Count -gt 0 -and (& "$PSScriptRoot/Initialize-NativeDependencyWorkloads.ps1" -Targets @($plan.nativeRequests.target) -Dotnet $Dotnet)) {
            Copy-Item (Join-Path $planRoot 'plan.json') (Join-Path $planRoot 'plan-before-workloads.json')
            & $Dotnet $assembly[0] validate plan --engine-root $repository --dotnet $Dotnet --base $BaseRevision --output $planRoot @baselineOptions `
                --profile $Profile --backend-store $BackendStore 2>&1 | Tee-Object -FilePath (Join-Path $planRoot 'planning-prepared.log')
            if ($LASTEXITCODE -ne 0) { throw 'Validation planning failed after explicit SDK preparation.' }
            $plan = Get-Content -LiteralPath (Join-Path $planRoot 'plan.json') -Raw | ConvertFrom-Json
        }
    }
    if (@($plan.checks).Count -eq 0 -or @($plan.shards).Count -eq 0) { throw 'Required hosted validation discovery is empty.' }
    $requests = @($plan.nativeRequests)
    "native=$(($requests.Count -gt 0).ToString().ToLowerInvariant())" >> $env:GITHUB_OUTPUT
    "shards=$(ConvertTo-Json -InputObject @($plan.shards.id) -Compress)" >> $env:GITHUB_OUTPUT
    return
}
$entry = Get-Content -LiteralPath (Join-Path $planRoot 'entry.json') -Raw | ConvertFrom-Json
$assembly = [IO.Path]::GetFullPath($entry.assembly, $repository)
if ((Get-FileHash -LiteralPath $assembly).Hash -cne $entry.sha256) { throw 'The prepared validation entry changed before execution.' }
$planPath = Join-Path $planRoot 'plan.json'
$plan = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json
if ($plan.commit -cne $ExpectedCommit) { throw 'The frozen plan belongs to another candidate.' }
New-Item -ItemType Directory -Path $output -Force | Out-Null
if ($Stage -ceq 'Join') {
    if (-not $PrerequisitesDirectory -or -not $Platform) { throw 'Host certification requires a registered platform and all shard receipts.' }
    & $Dotnet $assembly validate join --engine-root $repository --dotnet $Dotnet --plan $planPath --output $output `
        --prerequisites $PrerequisitesDirectory *> (Join-Path $output 'join.log')
    if ($LASTEXITCODE -ne 0) { Get-Content (Join-Path $output 'join.log') -Tail 20 | Out-Host; throw 'The required shard matrix did not certify the frozen plan.' }
    Copy-Item -LiteralPath $planPath -Destination (Join-Path $output 'plan.json') -Force
    $receipt = & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath (Join-Path $output 'plan.json') -ResultsPath (Join-Path $output 'results.json') `
        -ExpectedCommit $ExpectedCommit -JobConclusion success
    $shards = foreach ($item in $plan.shards) {
        $path = Join-Path $PrerequisitesDirectory "$($item.id)/results.json"
        [pscustomobject]@{ id = $item.id; sha256 = (Get-FileHash -LiteralPath $path).Hash.ToLowerInvariant() }
    }
    $receipt | Add-Member -NotePropertyName shards -NotePropertyValue @($shards)
    $receipt | Add-Member -NotePropertyName platform -NotePropertyValue $Platform
    $receipt | Add-Member -NotePropertyName executor -NotePropertyValue $entry.sha256
    $receipt | ConvertTo-Json -Depth 12 | Set-Content (Join-Path $output 'receipt.json')
    return
}
if (-not $Shard) { throw 'CI execution requires an explicit frozen shard.' }
$selected = @($plan.shards | Where-Object id -CEQ $Shard)
if ($selected.Count -eq 0) {
    if ($Shard -cnotin @('managed', 'native')) { throw 'An unknown required shard cannot be skipped.' }
    Write-Host "The frozen plan declares no $Shard shard. The final gate verifies complete coverage independently."
    'selected=false' >> $env:GITHUB_OUTPUT
    return
}
if ($selected.Count -ne 1) { throw 'The frozen shard is duplicated.' }
'selected=true' >> $env:GITHUB_OUTPUT
# Planning already prepared this runner's SDK workloads before freezing the environment.
$prerequisites = @()
if (@($selected[0].prerequisites).Count -gt 0) {
    if (-not $PrerequisitesDirectory) { throw 'This shard requires completed prerequisite receipts.' }
    $prerequisites = @('--prerequisites', $PrerequisitesDirectory)
}
$operation = 'run'
$continuationArguments = @()
if ($SourceRunId -gt 0) {
    $continuationRoot = Join-Path $output 'accepted'
    & "$PSScriptRoot/Get-ValidationContinuation.ps1" -RunId $SourceRunId -Commit $ExpectedCommit -Platform $env:VALIDATION_PLATFORM `
        -Shard $Shard -Repository $env:GITHUB_REPOSITORY -ApiUrl $env:GITHUB_API_URL -OutputDirectory $continuationRoot
    $accepted = Get-Content (Join-Path $continuationRoot 'continuation.json') -Raw | ConvertFrom-Json
    $operation = 'resume'
    $continuationArguments = @('--accepted-plan', $accepted.plan, '--evidence', $accepted.evidence)
    if ($accepted.outputs) { $continuationArguments += @('--retained-outputs', $accepted.outputs) }
}
& $Dotnet $assembly validate $operation --engine-root $repository --dotnet $Dotnet --plan $planPath --output $output `
    --shard $Shard @prerequisites @continuationArguments 2>&1 | Tee-Object -FilePath (Join-Path $output 'validation.log')
if ($LASTEXITCODE -ne 0) { throw "Required validation failed; retained evidence: $output." }
if (@($plan.checks | Where-Object { $_.kind -ceq 'build' -and $null -eq $_.exclusion -and $_.id -cin $selected[0].checks }).Count -gt 0) {
    $bound = Get-Content -LiteralPath (Join-Path $output 'execution-plan.json') -Raw | ConvertFrom-Json
    "compiledOutputs=$($bound.artifacts)" >> $env:GITHUB_OUTPUT
}
if (@($plan.checks | Where-Object { $_.id -ceq 'release-bundle' -and $null -eq $_.exclusion -and $_.id -cin $selected[0].checks }).Count -eq 1) {
    $bound = Get-Content -LiteralPath (Join-Path $output 'execution-plan.json') -Raw | ConvertFrom-Json
    "deliverables=$($bound.artifacts)/deliverables" >> $env:GITHUB_OUTPUT
}
$dirty = @(& git status --porcelain --untracked-files=all)
if ($LASTEXITCODE -ne 0 -or $dirty.Count -ne 0) { throw "Validation changed source inputs: $($dirty -join ', ')" }
