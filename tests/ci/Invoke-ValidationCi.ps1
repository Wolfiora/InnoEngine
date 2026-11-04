param(
    [Parameter(Mandatory)][string]$BaseRevision,
    [Parameter(Mandatory)][string]$ExpectedCommit,
    [Parameter(Mandatory)][ValidateSet('Plan', 'Run', 'Retain')][string]$Stage,
    [string]$Dotnet = 'dotnet',
    [switch]$Full,
    [switch]$Extended,
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')][string]$ValidationConclusion
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
Set-Location -LiteralPath $repository
$actual = (& git rev-parse HEAD).Trim()
if ($LASTEXITCODE -ne 0 -or $actual -cne $ExpectedCommit) { throw 'Checkout is not the requested merge candidate.' }
$output = Join-Path $repository 'artifacts/validation/ci'
$drive = [IO.DriveInfo]::new([IO.Path]::GetPathRoot($repository))
Write-Host "Validation stage ${Stage}; available disk bytes: $($drive.AvailableFreeSpace)."
if ($Stage -eq 'Plan') {
    if (Test-Path -LiteralPath $output) { throw 'A CI attempt must retain a distinct result directory.' }
    New-Item -ItemType Directory -Path $output | Out-Null
    $bootstrap = Join-Path $repository 'artifacts/validation/bootstrap'
    $entry = 'entry/InnoEngine.Build/InnoEngine.Build.csproj'
    & $Dotnet build $entry --configuration Debug --artifacts-path $bootstrap --disable-build-servers "-m:$([Math]::Min([Environment]::ProcessorCount, 4))" -nr:false `
        2>&1 | Tee-Object -FilePath (Join-Path $output 'bootstrap.log')
    if ($LASTEXITCODE -ne 0) { throw "Validation bootstrap failed; see $output/bootstrap.log." }
    $assembly = @(& $Dotnet msbuild $entry -nologo -p:Configuration=Debug "-p:ArtifactsPath=$bootstrap" -getProperty:TargetPath)
    if ($LASTEXITCODE -ne 0 -or $assembly.Count -ne 1 -or -not (Test-Path -LiteralPath $assembly[0])) {
        throw 'The SDK did not identify the built validation entry.'
    }
    @{ assembly = $assembly[0]; sha256 = (Get-FileHash -LiteralPath $assembly[0]).Hash } | ConvertTo-Json | Set-Content (Join-Path $output 'entry.json')
    & $Dotnet $assembly[0] validate plan --engine-root $repository --dotnet $Dotnet --base $BaseRevision --output $output `
        --full $Full.ToString().ToLowerInvariant() --extended $Extended.ToString().ToLowerInvariant() 2>&1 | Tee-Object -FilePath (Join-Path $output 'planning.log')
    if ($LASTEXITCODE -ne 0) { throw 'Validation plan generation failed; no downstream stage can certify success.' }
    $plan = Get-Content -LiteralPath (Join-Path $output 'plan.json') -Raw | ConvertFrom-Json
    "native=$(($null -ne $plan.contexts.native).ToString().ToLowerInvariant())" >> $env:GITHUB_OUTPUT
    return
}
$entry = Get-Content -LiteralPath (Join-Path $output 'entry.json') -Raw | ConvertFrom-Json
if ((Get-FileHash -LiteralPath $entry.assembly).Hash -cne $entry.sha256) { throw 'The prepared validation entry changed before execution.' }
$assembly = @($entry.assembly)
$cache = Join-Path $repository 'artifacts/validation/native-cache.zip'
if ($Stage -eq 'Retain') {
    if (-not $ValidationConclusion) { throw 'Retention requires the original validation conclusion.' }
    if ($ValidationConclusion -cne 'success' -or -not (Test-Path -LiteralPath $cache)) {
        $cache = Join-Path $repository 'artifacts/validation/completed-native-cache.zip'
        & $Dotnet $assembly[0] validate evidence --engine-root $repository --dotnet $Dotnet `
            --plan (Join-Path $output 'plan.json') --evidence (Join-Path $output 'results.json') `
            --cache-output $cache --output (Join-Path $output 'retention') *> (Join-Path $output 'retention.log')
        $retentionExit = $LASTEXITCODE
        if ($ValidationConclusion -ceq 'success' -and $retentionExit -ne 0) { throw 'Successful validation failed artifact retention or evidence reconciliation.' }
        if ($retentionExit -ne 0) { Write-Host 'Original validation remains unsuccessful; only completely exported immutable artifacts can be retained.' }
    }
    if (Test-Path -LiteralPath $cache) { "native=$cache" >> $env:GITHUB_OUTPUT }
    return
}
& $Dotnet $assembly[0] validate run --engine-root $repository --dotnet $Dotnet --plan (Join-Path $output 'plan.json') --output $output `
    --full $Full.ToString().ToLowerInvariant() --extended $Extended.ToString().ToLowerInvariant() `
    --cache-input $cache --cache-output $cache `
    2>&1 | Tee-Object -FilePath (Join-Path $output 'validation.log')
if ($LASTEXITCODE -ne 0) { throw "Required validation failed; inspect the retained plan, results and first failure in $output." }
& $Dotnet $assembly[0] validate evidence --engine-root $repository --dotnet $Dotnet `
    --plan (Join-Path $output 'plan.json') --evidence (Join-Path $output 'results.json') --output (Join-Path $output 'reconciliation') `
    *> (Join-Path $output 'evidence.log')
if ($LASTEXITCODE -ne 0) { throw 'Completed evidence does not match the current inputs and outputs.' }
& "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath (Join-Path $output 'plan.json') -ResultsPath (Join-Path $output 'results.json') `
    -ExpectedCommit $ExpectedCommit -JobConclusion success | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath (Join-Path $output 'receipt.json')
$plan = Get-Content -LiteralPath (Join-Path $output 'plan.json') -Raw | ConvertFrom-Json
if ($plan.full) {
    $bundle = @($plan.checks | Where-Object { $_.id -ceq 'release-bundle' -and $null -eq $_.exclusion })
    if ($bundle.Count -ne 1) { throw 'Full hosted validation lacks its required Release product artifact check.' }
    "deliverables=$($plan.artifacts)/deliverables" >> $env:GITHUB_OUTPUT
}
$dirty = @(& git status --porcelain --untracked-files=all)
if ($LASTEXITCODE -ne 0 -or $dirty.Count -ne 0) { throw "Validation changed source inputs: $($dirty -join ', ')" }
Write-Host "Validation complete. Available disk bytes: $($drive.AvailableFreeSpace)."
