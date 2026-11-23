param(
    [Parameter(Mandatory)][object[]]$Artifacts,
    [Parameter(Mandatory)][string]$Name,
    [Parameter(Mandatory)][int]$MaximumAttempt,
    [switch]$RequireCurrentAttempt
)

$ErrorActionPreference = 'Stop'
if ($MaximumAttempt -lt 1) { throw 'Artifact selection requires a positive workflow attempt.' }
$pattern = '^' + [regex]::Escape($Name) + '-attempt-([1-9][0-9]*)$'
$selected = @($Artifacts | Where-Object { $_.name -cmatch $pattern } | ForEach-Object {
    $null = $_.name -cmatch $pattern
    $attempt = [int]$Matches[1]
    if ($attempt -gt $MaximumAttempt) { throw "Artifact belongs to a future attempt: $($_.name)" }
    [pscustomobject]@{ attempt = $attempt; artifact = $_ }
} | Sort-Object attempt -Descending)
if ($selected.Count -eq 0 -or @($selected | Where-Object attempt -EQ $selected[0].attempt).Count -ne 1 -or
    ($RequireCurrentAttempt -and $selected[0].attempt -ne $MaximumAttempt)) {
    throw "The required artifact is missing, duplicated or from an uncertified attempt: $Name"
}
$selected[0].artifact
