$ErrorActionPreference = 'Stop'
$root = [IO.Path]::GetFullPath('../../artifacts/validation/source-receipt-contracts', $PSScriptRoot)
$run = Join-Path $root ([Guid]::NewGuid().ToString('N'))
$hash = 'a' * 64
$tree = 'b' * 40
$results = [Collections.Generic.List[object]]::new()
function Invoke-Case([string]$Name, [scriptblock]$Change, [bool]$Expected) {
    $directory = Join-Path $run $Name
    [IO.Directory]::CreateDirectory($directory) | Out-Null
    $id = 'documentation-api:managed:default'
    $state = @{
        plan = @{ checks = @(@{ id = $id; kind = 'documentation-api'; projects = @('src/Probe.csproj'); exclusion = $null
            documentation = @{ references = @('reference/api/probe.json'); configuration = 'Debug'; target = $null } }) }
        evidence = @{ results = @(@{ id = $id; status = 'Passed' }) }
        pairing = @{ documentationSnapshot = $hash; contentIdentity = $hash; engineTree = $tree }
        catalog = @{ snapshot = $hash; contentIdentity = $hash; sourceTree = $tree; repository = 'https://github.com/Wolfiora/InnoEngine'
            references = @(@{ path = 'reference/api/probe.json'; identity = $hash; model = 'source-clr'; configuration = 'Debug'; target = $null }) }
        report = @{ status = 'Passed'; scope = 'SelectedSourceReferenceComparison'; documentationSnapshot = $hash; documentationSourceTree = $tree
            configuration = 'Debug'; target = $null; references = @(@{ path = 'reference/api/probe.json'; identity = $hash })
            projects = @(@{ project = 'src/Probe.csproj'; name = 'Probe'; status = 'Passed'; differences = @() }) }
    }
    & $Change $state
    $state.catalog | ConvertTo-Json -Depth 10 | Set-Content (Join-Path $directory 'documentation-catalog.json') -Encoding utf8NoBOM
    $file = Join-Path $directory 'documentation-api-managed-default.json'
    if (-not $state.missing) { $state.report | ConvertTo-Json -Depth 10 | Set-Content $file -Encoding utf8NoBOM }
    $accepted = $false
    $diagnostic = ''
    try {
        $receipt = @(& "$PSScriptRoot/Get-DocumentationSourceReceipt.ps1" -Plan $state.plan -Evidence $state.evidence -Pairing $state.pairing -Directory $directory)
        if ($receipt.Count -ne 1 -or $receipt[0].reportIdentity -cne (Get-FileHash $file).Hash.ToLowerInvariant() `
            -or $receipt[0].projects[0] -cne 'Probe') { throw 'The source receipt lost exact coverage or report provenance.' }
        $accepted = $true
    } catch { $diagnostic = $_.Exception.Message }
    $results.Add(@{ name = $Name; accepted = $accepted; expected = $Expected; diagnostic = $diagnostic })
    $results | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $run 'tests.json') -Encoding utf8NoBOM
    if ($accepted -ne $Expected) { throw "Unexpected source certification result for ${Name}: $diagnostic" }
}
Invoke-Case 'complete' { param($s) } $true
Invoke-Case 'missing-report' { param($s) $s.missing = $true } $false
Invoke-Case 'failed-check' { param($s) $s.evidence.results[0].status = 'Failed' } $false
Invoke-Case 'changed-tree' { param($s) $s.report.documentationSourceTree = 'c' * 40 } $false
Invoke-Case 'changed-snapshot' { param($s) $s.report.documentationSnapshot = 'c' * 64 } $false
Invoke-Case 'changed-catalog' { param($s) $s.catalog.contentIdentity = 'c' * 64 } $false
Invoke-Case 'failed-comparison' { param($s) $s.report.status = 'Failed' } $false
Invoke-Case 'failed-owner' { param($s) $s.report.projects[0].status = 'Failed' } $false
Invoke-Case 'hidden-differences' { param($s) $s.report.projects[0].differences = @('Changed declaration') } $false
Invoke-Case 'missing-owner' { param($s) $s.report.projects = @() } $false
Invoke-Case 'extra-owner' { param($s) $s.report.projects += $s.report.projects[0] } $false
Invoke-Case 'wrong-target' { param($s) $s.report.target = 'another-platform' } $false
Invoke-Case 'changed-reference' { param($s) $s.report.references[0].identity = 'c' * 64 } $false
Invoke-Case 'missing-reference' { param($s) $s.report.references = @() } $false
Invoke-Case 'repeated-reference' { param($s) $s.report.references += $s.report.references[0] } $false
Invoke-Case 'escaping-report' { param($s) $s.plan.checks[0].id = '../outside' } $false
Write-Output "Passed $($results.Count) source receipt reconciliation cases. Evidence: $run"
