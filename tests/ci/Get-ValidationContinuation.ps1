param(
    [Parameter(Mandatory)][long]$RunId,
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][string]$Platform,
    [Parameter(Mandatory)][ValidateSet('architecture', 'managed', 'native')][string]$Shard,
    [Parameter(Mandatory)][string]$Repository,
    [Parameter(Mandatory)][string]$OutputDirectory,
    [switch]$PlanOnly,
    [string]$ApiUrl = 'https://api.github.com'
)

$ErrorActionPreference = 'Stop'
if ($RunId -le 0 -or $Commit -cnotmatch '^[a-f0-9]{40}$' -or -not $env:GH_TOKEN) {
    throw 'Continuation requires an exact source run, commit and repository read token.'
}
$root = [IO.Path]::GetFullPath($OutputDirectory)
if (Test-Path -LiteralPath $root) { throw 'Continuation evidence requires a distinct retained directory.' }
$headers = @{ Authorization = "Bearer $env:GH_TOKEN"; Accept = 'application/vnd.github+json'; 'X-GitHub-Api-Version' = '2022-11-28' }
$base = "$ApiUrl/repos/$Repository"
$repositoryData = Invoke-RestMethod -Uri $base -Headers $headers
$run = Invoke-RestMethod -Uri "$base/actions/runs/$RunId" -Headers $headers
if ($run.id -ne $RunId -or $run.status -cne 'completed' -or $run.conclusion -cne 'success' -or
    $run.head_repository.id -ne $repositoryData.id -or $run.head_sha -cne $Commit -or
    $run.head_branch -cne $repositoryData.default_branch -or $run.event -cnotin @('push', 'schedule', 'workflow_dispatch') -or
    $run.path -cnotin @('.github/workflows/engine-ci.yml', '.github/workflows/engine-regression.yml')) {
    throw 'Continuation requires successful exact default-branch engine evidence; PR and foreign evidence are forbidden.'
}
$workflow = Invoke-RestMethod -Uri "$base/actions/workflows/$([IO.Path]::GetFileName($run.path))" -Headers $headers
if ($workflow.id -ne $run.workflow_id) { throw 'The original workflow identity does not match its registered path.' }
$inventory = Invoke-RestMethod -Uri "$base/actions/runs/$RunId/artifacts?per_page=100" -Headers $headers
if ($inventory.total_count -gt 100) { throw 'Continuation refuses an incomplete artifact inventory.' }
$names = @('engine-certification', 'engine-acceptance-evidence')
$outputName = "validation-outputs-$Platform-$Shard"
if (!$PlanOnly -and @($inventory.artifacts | Where-Object name -CLike "$outputName-attempt-*").Count -gt 0) {
    $latest = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $inventory.artifacts -Name $outputName -MaximumAttempt $run.run_attempt
    if (!$latest.expired -and [DateTimeOffset]::Parse($latest.expires_at) -gt [DateTimeOffset]::UtcNow) { $names += $outputName }
}
$selected = foreach ($name in $names) {
    $artifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $inventory.artifacts -Name $name -MaximumAttempt $run.run_attempt `
        -RequireCurrentAttempt:($name -cin @('engine-certification', 'engine-acceptance-evidence'))
    if ($artifact.expired -or [DateTimeOffset]::Parse($artifact.expires_at) -le [DateTimeOffset]::UtcNow -or
        $artifact.digest -cnotmatch '^sha256:[a-f0-9]{64}$' -or $artifact.workflow_run.id -ne $RunId -or
        $artifact.workflow_run.head_sha -cne $Commit -or $artifact.workflow_run.repository_id -ne $repositoryData.id) {
        throw "Continuation artifact lacks immutable source provenance: $name"
    }
    [pscustomobject]@{ role = $name; artifact = $artifact }
}
New-Item -ItemType Directory -Path $root | Out-Null
foreach ($item in $selected) {
    $zip = Join-Path $root ($item.role + '.zip')
    Invoke-WebRequest -Uri "$base/actions/artifacts/$($item.artifact.id)/zip" -Headers $headers -OutFile $zip
    & "$PSScriptRoot/Expand-ValidationArtifact.ps1" -ArchivePath $zip -Destination (Join-Path $root $item.role) -ExpectedDigest $item.artifact.digest
}
$accepted = Join-Path $root "engine-acceptance-evidence/$Platform"
$certificate = @(Get-Content (Join-Path $root 'engine-certification/certification.json') -Raw | ConvertFrom-Json)
$platformReceipt = @($certificate | Where-Object platform -CEQ $Platform)
if ($platformReceipt.Count -ne 1) { throw 'The accepted platform certification is absent or duplicated.' }
$receipt = & "$PSScriptRoot/Test-ValidationReport.ps1" -PlanPath (Join-Path $accepted 'plan.json') `
    -ResultsPath (Join-Path $accepted 'results.json') -ExpectedCommit $Commit -JobConclusion success
if ($receipt.planIdentity -cne $platformReceipt[0].receipt.planIdentity -or $receipt.resultsIdentity -cne $platformReceipt[0].receipt.resultsIdentity) {
    throw 'Continuation evidence is not the exact certified plan and result.'
}
$plan = Get-Content (Join-Path $accepted 'plan.json') -Raw | ConvertFrom-Json
if ($plan.profile -notin @(2, 3) -or $plan.extended) { throw 'Only master or full functional evidence may be continued.' }
$outputs = $null
if ($outputName -cin $selected.role) {
    $outputs = Join-Path $root 'compiled'
    & "$PSScriptRoot/Transfer-ValidationOutputs.ps1" -Operation Unpack -Directory $outputs -Archive (Join-Path $root "$outputName/outputs-$Shard.tar")
}
[pscustomobject]@{
    runId = $RunId
    commit = $Commit
    plan = Join-Path $accepted 'plan.json'
    evidence = Join-Path $accepted 'results.json'
    outputs = $outputs
} | ConvertTo-Json | Set-Content (Join-Path $root 'continuation.json')
