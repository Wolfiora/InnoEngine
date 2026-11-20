param(
    [Parameter(Mandatory)][string]$PlanDirectory,
    [Parameter(Mandatory)][string]$Platform,
    [Parameter(Mandatory)][string]$Repository,
    [string]$ApiUrl = 'https://api.github.com',
    [string]$Dotnet = 'dotnet'
)

$ErrorActionPreference = 'Stop'
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$planRoot = [IO.Path]::GetFullPath($PlanDirectory, $engine)
$planPath = Join-Path $planRoot 'plan.json'
$plan = Get-Content -LiteralPath $planPath -Raw | ConvertFrom-Json
$decision = @{ reusable = $false; platform = $Platform; commit = $plan.commit; reason = 'No retained trusted complete acceptance matches the current environment.' }
if ($plan.backendStore -and $null -eq $plan.backendPackages) {
    $decision.reason = 'This candidate requires backend production before it can match previous functional acceptance.'
    $decision | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $planRoot 'acceptance-reuse.json')
    return
}
if (-not $env:GH_TOKEN) { throw 'Acceptance discovery requires a repository read token.' }
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
$base = "$ApiUrl/repos/$Repository"
$repositoryData = Invoke-RestMethod -Uri $base -Headers $headers
$runs = Invoke-RestMethod -Uri "$base/actions/runs?head_sha=$($plan.commit)&status=success&per_page=100" -Headers $headers
$hosts = @(Get-Content -LiteralPath (Join-Path $engine '.github/validation-hosts.json') -Raw | ConvertFrom-Json)
$requiredNames = @('engine-full-certification', 'engine-acceptance-evidence') + @($hosts | ForEach-Object { "deliverable-$($_.runner)" })
foreach ($run in @($runs.workflow_runs)) {
    if ($run.head_sha -cne $plan.commit -or $run.head_repository.id -ne $repositoryData.id -or
        $run.head_branch -cne $repositoryData.default_branch -or $run.status -cne 'completed' -or $run.conclusion -cne 'success' -or
        $run.event -cnotin @('push', 'schedule', 'workflow_dispatch') -or
        $run.path -cnotin @('.github/workflows/engine-ci.yml', '.github/workflows/engine-regression.yml')) { continue }
    $inventory = Invoke-RestMethod -Uri "$base/actions/runs/$($run.id)/artifacts?per_page=100" -Headers $headers
    if ($inventory.total_count -gt 100) { throw 'Acceptance discovery refuses a partial artifact inventory.' }
    $available = @($inventory.artifacts | Where-Object { -not $_.expired -and [DateTimeOffset]::Parse($_.expires_at) -gt [DateTimeOffset]::UtcNow })
    if (@($requiredNames | Where-Object { $name = $_; @($available | Where-Object name -CLike "$name-attempt-*").Count -eq 0 }).Count -ne 0) { continue }
    $workflow = Invoke-RestMethod -Uri "$base/actions/workflows/$([IO.Path]::GetFileName($run.path))" -Headers $headers
    $releaseArtifacts = @(& "$PSScriptRoot/Test-ReleaseSource.ps1" -Repository $repositoryData -Run $run -Artifacts $available `
        -Commit $plan.commit -WorkflowId $workflow.id -RunId $run.id)
    $evidence = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $available -Name 'engine-acceptance-evidence' `
        -MaximumAttempt $run.run_attempt -RequireCurrentAttempt
    if ($evidence.digest -cnotmatch '^sha256:[a-f0-9]{64}$' -or $evidence.workflow_run.id -ne $run.id -or
        $evidence.workflow_run.head_sha -cne $plan.commit -or $evidence.workflow_run.repository_id -ne $repositoryData.id) {
        throw 'Acceptance evidence lacks authenticated repository and candidate provenance.'
    }
    $certificate = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $releaseArtifacts -Name 'engine-full-certification' `
        -MaximumAttempt $run.run_attempt -RequireCurrentAttempt
    $owner = Join-Path $planRoot "accepted/$($run.id)"
    New-Item -ItemType Directory -Path $owner -Force | Out-Null
    foreach ($artifact in @($certificate, $evidence)) {
        $zip = Join-Path $owner ($artifact.name + '.zip')
        Invoke-WebRequest -Uri "$base/actions/artifacts/$($artifact.id)/zip" -Headers $headers -OutFile $zip
        & "$PSScriptRoot/Expand-ValidationArtifact.ps1" -ArchivePath $zip -Destination (Join-Path $owner $artifact.name) -ExpectedDigest $artifact.digest
    }
    $certification = @(Get-Content -LiteralPath (Join-Path $owner "$($certificate.name)/certification.json") -Raw | ConvertFrom-Json)
    if ($certification.Count -ne $hosts.Count -or @($hosts.runner | Where-Object { $hostName = $_; @($certification | Where-Object platform -CEQ $hostName).Count -ne 1 }).Count) {
        throw 'Accepted functional certification does not cover the registered host matrix.'
    }
    $previous = Join-Path $owner "$($evidence.name)/$Platform"
    $receipt = & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath (Join-Path $previous 'plan.json') `
        -ResultsPath (Join-Path $previous 'results.json') -ExpectedCommit $plan.commit -JobConclusion success
    $certified = @($certification | Where-Object platform -CEQ $Platform)[0].receipt
    if (-not $receipt.full -or $receipt.planIdentity -cne $certified.planIdentity -or $receipt.resultsIdentity -cne $certified.resultsIdentity) {
        throw 'The original complete certification does not identify these exact acceptance bytes.'
    }
    $entry = Get-Content -LiteralPath (Join-Path $planRoot 'entry.json') -Raw | ConvertFrom-Json
    $assembly = [IO.Path]::GetFullPath($entry.assembly, $engine)
    if ((Get-FileHash -LiteralPath $assembly).Hash -cne $entry.sha256) { throw 'Acceptance comparison requires the current frozen execution entry.' }
    & $Dotnet $assembly validate acceptance --engine-root $engine --dotnet $Dotnet --plan $planPath `
        --accepted-plan (Join-Path $previous 'plan.json') --evidence (Join-Path $previous 'results.json') --output $owner
    if ($LASTEXITCODE -ne 0) { throw 'The accepted evidence was invalid; retain its first failure.' }
    $comparison = Get-Content -LiteralPath (Join-Path $owner 'acceptance.json') -Raw | ConvertFrom-Json
    if ($comparison.reusable -isnot [bool]) { throw 'Acceptance comparison did not produce an explicit decision.' }
    if ($comparison.reusable) {
        $decision = @{ reusable = $true; platform = $Platform; commit = $plan.commit; reason = $comparison.reason;
            planIdentity = $comparison.planIdentity; acceptedPlanIdentity = $comparison.acceptedPlanIdentity;
            sourceRun = $run.id; sourceUrl = $run.html_url; evidenceDigest = $evidence.digest; receipt = $receipt }
        break
    }
}
$decision | ConvertTo-Json -Depth 15 | Set-Content -LiteralPath (Join-Path $planRoot 'acceptance-reuse.json')
