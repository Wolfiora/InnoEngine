$ErrorActionPreference = 'Stop'
$run = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/example-receipt-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$hash = 'a' * 64
$tree = 'b' * 40
$results = [Collections.Generic.List[object]]::new()
function Invoke-Case([string]$Name, [scriptblock]$Change, [bool]$Expected) {
    $directory = Join-Path $run $Name
    [IO.Directory]::CreateDirectory($directory) | Out-Null
    $id = 'documentation-examples:managed'
    $block = @{ language = 'csharp'; contentIdentity = $hash; declarationSymbols = @() }
    $state = @{
        plan = @{ checks = @(@{ id = $id; kind = 'documentation-examples'; exclusion = $null
            documentation = @{ pages = @('task.probe'); configuration = 'Debug'; target = $null } }) }
        evidence = @{ results = @(@{ id = $id; status = 'Passed' }) }
        pairing = @{ documentationSnapshot = $hash; contentIdentity = $hash; engineTree = $tree }
        catalog = @{ repository = 'https://github.com/Wolfiora/InnoEngine'; snapshot = $hash; contentIdentity = $hash; sourceTree = $tree
            pages = @(@{ id = 'task.probe'; contentIdentity = $hash; examples = @{ mode = 'host'; projects = @('Probe') }; codeBlocks = @($block,
                @{ language = 'csharp'; contentIdentity = 'c' * 64; declarationSymbols = @('T:Probe') }) }) }
        report = @{ status = 'Passed'; snapshot = $hash; sourceTree = $tree; contentIdentity = $hash; configuration = 'Debug'; target = $null
            results = @(@{ page = 'task.probe'; contentIdentity = $hash; mode = 'host'; status = 'Passed'; errors = @(); blocks = @($block); references = @{ 'Probe.dll' = $hash } }) }
    }
    & $Change $state
    $state.catalog | ConvertTo-Json -Depth 12 | Set-Content (Join-Path $directory 'documentation-catalog.json') -Encoding utf8NoBOM
    $file = Join-Path $directory 'documentation-examples-managed.json'
    if (-not $state.missing) { $state.report | ConvertTo-Json -Depth 12 | Set-Content $file -Encoding utf8NoBOM }
    $accepted = $false; $diagnostic = ''
    try {
        $receipt = @(& "$PSScriptRoot/Get-DocumentationExampleReceipt.ps1" -Plan $state.plan -Evidence $state.evidence -Pairing $state.pairing -Directory $directory)
        if ($receipt.Count -ne 1 -or $receipt[0].reportIdentity -cne (Get-FileHash $file).Hash.ToLowerInvariant() `
            -or $receipt[0].pages[0].blocks.Count -ne 1 -or $receipt[0].pages[0].projects[0] -cne 'Probe') { throw 'Example receipt lost actual compiled input or included a reference declaration.' }
        $accepted = $true
    } catch { $diagnostic = $_.Exception.Message }
    $results.Add(@{ name = $Name; accepted = $accepted; expected = $Expected; diagnostic = $diagnostic })
    $results | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $run 'tests.json') -Encoding utf8NoBOM
    if ($accepted -ne $Expected) { throw "Unexpected example receipt for ${Name}: $diagnostic" }
}
Invoke-Case 'complete' { param($s) } $true
Invoke-Case 'missing-report' { param($s) $s.missing = $true } $false
Invoke-Case 'failed-check' { param($s) $s.evidence.results[0].status = 'Failed' } $false
Invoke-Case 'stale-tree' { param($s) $s.report.sourceTree = 'c' * 40 } $false
Invoke-Case 'stale-snapshot' { param($s) $s.report.snapshot = 'c' * 64 } $false
Invoke-Case 'stale-page' { param($s) $s.report.results[0].contentIdentity = 'c' * 64 } $false
Invoke-Case 'failed-compilation' { param($s) $s.report.results[0].status = 'Failed' } $false
Invoke-Case 'hidden-errors' { param($s) $s.report.results[0].errors = @('CS0006') } $false
Invoke-Case 'wrong-mode' { param($s) $s.report.results[0].mode = 'runtime-script' } $false
Invoke-Case 'wrong-target' { param($s) $s.report.target = 'foreign' } $false
Invoke-Case 'wrong-configuration' { param($s) $s.report.configuration = 'Release' } $false
Invoke-Case 'empty-discovery' { param($s) $s.report.results = @() } $false
Invoke-Case 'duplicate-page' { param($s) $s.report.results += $s.report.results[0] } $false
Invoke-Case 'missing-block' { param($s) $s.report.results[0].blocks = @() } $false
Invoke-Case 'missing-reference' { param($s) $s.report.results[0].references = @{} } $false
Invoke-Case 'unhashed-reference' { param($s) $s.report.results[0].references['Probe.dll'] = 'unknown' } $false
Invoke-Case 'escaping-report' { param($s) $s.plan.checks[0].id = '../outside' } $false
Write-Output "Passed $($results.Count) executable example receipt cases. Evidence: $run"
