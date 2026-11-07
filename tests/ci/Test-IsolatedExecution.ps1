param(
    [Parameter(Mandatory)][string]$Report,
    [Parameter(Mandatory)][string]$Contract
)

$ErrorActionPreference = 'Stop'
if (-not (Test-Path -LiteralPath $Report -PathType Leaf)) { throw "Missing isolated execution evidence: $Contract" }
[xml]$document = Get-Content -LiteralPath $Report -Raw
$counters = @($document.SelectNodes('//*[local-name()="Counters"]'))
if ($counters.Count -ne 1 -or [int]$counters[0].executed -le 0 -or
    [int]$counters[0].failed -ne 0 -or [int]$counters[0].error -ne 0 -or
    [int]$counters[0].passed -ne [int]$counters[0].executed) {
    throw "The isolated contract did not produce successful nonempty execution evidence: $Contract"
}
$results = @($document.SelectNodes('//*[local-name()="UnitTestResult"]'))
if ($results.Count -ne [int]$counters[0].executed -or @($results | Where-Object { $_.outcome -cne 'Passed' }).Count -ne 0) {
    throw "The isolated contract result records do not match its execution counters: $Contract"
}
$methods = @($document.SelectNodes('//*[local-name()="TestMethod"]'))
if ($methods.Count -eq 0 -or @($methods | Where-Object { "$($_.className).$($_.name)" -cne $Contract }).Count -ne 0) {
    throw "Isolated execution evidence belongs to another contract: $Contract"
}
