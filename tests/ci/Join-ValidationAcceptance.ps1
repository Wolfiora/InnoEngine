param(
    [Parameter(Mandatory)][string]$ReceivedDirectory,
    [Parameter(Mandatory)][string[]]$Hosts,
    [Parameter(Mandatory)][string]$ExpectedCommit,
    [Parameter(Mandatory)][int]$Attempt,
    [Parameter(Mandatory)][string]$OutputPath
)

$ErrorActionPreference = 'Stop'
if ($Hosts.Count -eq 0 -or @($Hosts | Sort-Object -Unique).Count -ne $Hosts.Count) { throw 'Acceptance reuse requires the exact nonempty host matrix.' }
$artifacts = @(Get-ChildItem -LiteralPath $ReceivedDirectory -Directory | ForEach-Object { [pscustomobject]@{ name = $_.Name; directory = $_.FullName } })
$decisions = @()
foreach ($platform in $Hosts) {
    $artifact = & "$PSScriptRoot/Select-ValidationArtifact.ps1" -Artifacts $artifacts -Name "validation-preflight-$platform" -MaximumAttempt $Attempt
    $path = Join-Path $artifact.directory 'acceptance-reuse.json'
    if (-not (Test-Path -LiteralPath $path)) { continue }
    $decision = Get-Content -LiteralPath $path -Raw | ConvertFrom-Json
    if ($decision.reusable -isnot [bool] -or $decision.platform -cne $platform -or $decision.commit -cne $ExpectedCommit) {
        throw 'Acceptance reuse decision belongs to another host or candidate.'
    }
    if ($decision.reusable) {
        if ($decision.planIdentity -cne (Get-FileHash (Join-Path $artifact.directory 'plan.json')).Hash.ToLowerInvariant() -or
            $decision.sourceRun -le 0 -or $decision.evidenceDigest -cnotmatch '^sha256:[a-f0-9]{64}$' -or
            -not $decision.receipt.full -or $decision.acceptedPlanIdentity -cne $decision.receipt.planIdentity) {
            throw 'Acceptance reuse has no complete original provenance.'
        }
        $decisions += $decision
    }
}
$result = @{ reusable = $decisions.Count -eq $Hosts.Count; hosts = $decisions }
[IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName([IO.Path]::GetFullPath($OutputPath))) | Out-Null
$result | ConvertTo-Json -Depth 18 | Set-Content -LiteralPath $OutputPath
"reusable=$($result.reusable.ToString().ToLowerInvariant())" >> $env:GITHUB_OUTPUT
if ($result.reusable) {
    foreach ($decision in $decisions) {
        "$($decision.platform): reused complete functional acceptance from $($decision.sourceUrl); current inputs, checks and environment match. This run produces no new release artifacts." >> $env:GITHUB_STEP_SUMMARY
    }
}
