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
        plan = @{ environment = $hash; checks = @(@{ id = $id; kind = 'documentation-api'; projects = @('src/Probe.csproj'); exclusion = $null
            inputs = @('@src/Probe.csproj')
            documentation = @{ references = @('reference/api/probe.json'); configuration = 'Debug'; target = $null } }) }
        evidence = @{ results = @(@{ id = $id; status = 'Passed'; documentationInputs = @(@{
            project = 'src/Probe.csproj'; owners = @('src/Probe.csproj'); sourceIdentity = $hash; environment = $hash
            configuration = 'Debug'; target = $null; packages = @{ 'probe/1.0.0' = $hash }; sources = @{ 'src/Probe.csproj' = $hash } }) }) }
        pairing = @{ documentationSnapshot = $hash; contentIdentity = $hash; engineTree = $tree }
        catalog = @{ snapshot = $hash; contentIdentity = $hash; sourceTree = $tree; repository = 'https://github.com/Wolfiora/InnoEngine'
            references = @(@{ path = 'reference/api/probe.json'; identity = $hash; model = 'source-clr'; configuration = 'Debug'; target = $null }) }
        report = @{ status = 'Passed'; scope = 'SelectedSourceReferenceComparison'; documentationSnapshot = $hash; documentationSourceTree = $tree
            configuration = 'Debug'; target = $null; references = @(@{ path = 'reference/api/probe.json'; identity = $hash })
            projects = @(@{ project = 'src/Probe.csproj'; name = 'Probe'; status = 'Passed'; differences = @(); referenceIdentity = $hash }) }
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
        if ($state.plan.checks[0].inputs.Count -gt 0 -and ($receipt[0].inputs.Count -ne 1 `
            -or $receipt[0].inputs[0].packages['probe/1.0.0'] -cne $hash)) { throw 'The source receipt lost its owner input proof.' }
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
Invoke-Case 'missing-input-proof' { param($s) $s.evidence.results[0].documentationInputs = @() } $false
Invoke-Case 'duplicate-input-proof' { param($s) $s.evidence.results[0].documentationInputs += $s.evidence.results[0].documentationInputs[0] } $false
Invoke-Case 'wrong-input-owner' { param($s) $s.evidence.results[0].documentationInputs[0].project = 'src/Other.csproj' } $false
Invoke-Case 'wrong-input-environment' { param($s) $s.evidence.results[0].documentationInputs[0].environment = 'c' * 64 } $false
Invoke-Case 'wrong-input-context' { param($s) $s.evidence.results[0].documentationInputs[0].configuration = 'Release' } $false
Invoke-Case 'missing-input-closure' { param($s) $s.evidence.results[0].documentationInputs[0].owners = @() } $false
Invoke-Case 'escaped-input-closure' { param($s) $s.evidence.results[0].documentationInputs[0].owners += '../outside.csproj' } $false
Invoke-Case 'missing-source-content' { param($s) $s.evidence.results[0].documentationInputs[0].sourceIdentity = '' } $false
Invoke-Case 'unhashed-package' { param($s) $s.evidence.results[0].documentationInputs[0].packages['probe/1.0.0'] = 'missing' } $false
Invoke-Case 'escaped-package' { param($s) $s.evidence.results[0].documentationInputs[0].packages['../outside'] = $hash } $false
Invoke-Case 'unhashed-owner-reference' { param($s) $s.report.projects[0].referenceIdentity = '' } $false
Invoke-Case 'source-only-empty-surface' { param($s) $s.plan.checks[0].inputs = @(); $s.evidence.results[0].documentationInputs[0].packages = @{} } $true
Write-Output "Passed $($results.Count) source receipt reconciliation cases. Evidence: $run"
