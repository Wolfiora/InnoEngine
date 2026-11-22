param(
    [Parameter(Mandatory)][object]$Repository,
    [Parameter(Mandatory)][object]$Run,
    [Parameter(Mandatory)][object[]]$Artifacts,
    [Parameter(Mandatory)][string]$Commit,
    [Parameter(Mandatory)][long]$WorkflowId,
    [Parameter(Mandatory)][long]$RunId,
    [switch]$AllowPartial
)

$ErrorActionPreference = 'Stop'
if ($Commit -cnotmatch '^[0-9a-f]{40}$' -or $RunId -le 0 -or $WorkflowId -le 0) { throw 'Release provenance requires exact source identities.' }
if ($Run.id -ne $RunId -or $Run.workflow_id -ne $WorkflowId -or $Run.path -cnotin @('.github/workflows/engine-ci.yml', '.github/workflows/engine-regression.yml')) {
    throw 'The source run is not the trusted engine validation workflow.'
}
if ($Run.status -cne 'completed' -or $Run.conclusion -cne 'success') { throw 'The source run did not complete successfully.' }
if ($Run.head_repository.id -ne $Repository.id -or $Run.head_sha -cne $Commit -or $Run.head_branch -cne $Repository.default_branch) {
    throw 'Release input must be the exact validated commit from this repository default branch.'
}
if ($Run.event -cnotin @('push', 'schedule', 'workflow_dispatch')) { throw 'Pull request or foreign workflow evidence cannot authorize a release.' }
$hosts = @(Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/validation-hosts.json') -Raw | ConvertFrom-Json)
$required = if ($AllowPartial) { @('engine-certification', 'engine-acceptance-evidence') }
    else { @('engine-full-certification') + @($hosts | ForEach-Object { "deliverable-$($_.runner)" }) }
foreach ($name in $required) {
    $artifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $Artifacts -Name $name -MaximumAttempt $Run.run_attempt `
        -RequireCurrentAttempt:($name -cin @('engine-full-certification', 'engine-certification', 'engine-acceptance-evidence'))
    if ($artifact.expired -or [DateTimeOffset]::Parse($artifact.expires_at) -le [DateTimeOffset]::UtcNow -or
        $artifact.digest -cnotmatch '^sha256:[0-9a-f]{64}$' -or $artifact.workflow_run.id -ne $RunId -or
        $artifact.workflow_run.head_sha -cne $Commit -or $artifact.workflow_run.repository_id -ne $Repository.id) {
        throw "A release artifact has expired or lacks matching server provenance: $name"
    }
    $artifact
}
