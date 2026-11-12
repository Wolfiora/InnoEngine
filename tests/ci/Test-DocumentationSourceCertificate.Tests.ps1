$ErrorActionPreference = 'Stop'
$run = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/source-certificate-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$hash = 'a' * 64
$tree = 'b' * 40
$results = [Collections.Generic.List[object]]::new()
function Invoke-Case([string]$Name, [scriptblock]$Change, [string]$Expected) {
    $directory = Join-Path $run $Name
    [IO.Directory]::CreateDirectory($directory) | Out-Null
    $catalog = @{ repository = 'https://github.com/Wolfiora/InnoEngine'; sourceTree = $tree; snapshot = $hash; contentIdentity = $hash
        pages = @(@{ id = 'task.probe'; examples = @{ mode = 'host'; projects = @('Probe') }
            codeBlocks = @(@{ language = 'csharp'; contentIdentity = $hash; declarationSymbols = @() }) })
        references = @(@{ path = 'reference/projects/catalog.json'; identity = $hash; model = 'project-inventory' },
            @{ path = 'reference/api/probe.json'; identity = $hash; model = 'source-clr'; configuration = 'Debug'; target = $null; projects = @('Probe', 'Other') }) }
    $catalogFile = Join-Path $directory 'catalog.json'
    $catalog | ConvertTo-Json -Depth 10 | Set-Content $catalogFile -Encoding utf8NoBOM
    $catalogHash = (Get-FileHash $catalogFile).Hash.ToLowerInvariant()
    $rows = @('host-one', 'host-two') | ForEach-Object {
        @{ platform = $_; receipt = @{ commit = 'd' * 40; required = 2; passed = 2; reused = 0
            documentation = @{ engineTree = $tree; documentationSnapshot = $hash; contentIdentity = $hash }
            sourceExamples = @(@{ configuration = 'Debug'; target = $null; catalogIdentity = $catalogHash; reportIdentity = $hash
                pages = @(@{ page = 'task.probe'; mode = 'host'; projects = @('Probe'); blocks = @($hash) }) })
            sourceApi = @(@{ configuration = 'Debug'; target = $null; catalogIdentity = $catalogHash; reportIdentity = $hash
                references = @(@{ path = 'reference/api/probe.json'; identity = $hash }); projects = @($(if ($_ -ceq 'host-one') { 'Probe' } else { 'Other' })) }) } }
    }
    $state = @{ rows = $rows; catalogPaths = @($catalogFile, $catalogFile) }
    & $Change $state
    $certification = Join-Path $directory 'certification.json'
    $state.rows | ConvertTo-Json -Depth 12 | Set-Content $certification -Encoding utf8NoBOM
    $output = Join-Path $directory 'output'
    $actual = 'Rejected'; $diagnostic = ''
    try {
        $report = & "$PSScriptRoot/New-DocumentationSourceCertificate.ps1" -CertificationPath $certification `
            -CatalogPaths $state.catalogPaths -OutputDirectory $output -ProducerOrigin 'fixture://authenticated-matrix'
        $actual = $report.status
        $file = Join-Path $output 'source-certificate.json'
        if ($actual -ceq 'Passed') {
            $proof = Get-Content $file -Raw | ConvertFrom-Json
            if ($proof.references[0].projects.Count -ne 2 -or $proof.projectManifestIdentity -cne $hash -or $proof.sourceCommit -cne ('d' * 40) `
                -or $proof.examples.Count -ne 1 -or $proof.examples[0].contexts.Count -ne 2 `
                -or $proof.producers[0].receiptIdentity -cne (Get-FileHash $certification).Hash.ToLowerInvariant()) { throw 'Source certificate lost exact coverage or provenance.' }
        } elseif (Test-Path $file) { throw 'Incomplete coverage published a certificate.' }
    } catch { $actual = 'Rejected'; $diagnostic = $_.Exception.Message }
    $results.Add(@{ name = $Name; actual = $actual; expected = $Expected; diagnostic = $diagnostic })
    $results | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $run 'tests.json') -Encoding utf8NoBOM
    if ($actual -cne $Expected) { throw "Unexpected certificate result for ${Name}: $actual $diagnostic" }
}
Invoke-Case 'combined-platform-coverage' { param($s) } 'Passed'
Invoke-Case 'missing-owner' { param($s) $s.rows[1].receipt.sourceApi[0].projects = @() } 'Blocked'
Invoke-Case 'no-comparisons' { param($s) $s.rows[0].receipt.sourceApi = @(); $s.rows[1].receipt.sourceApi = @() } 'Blocked'
Invoke-Case 'failed-required-check' { param($s) $s.rows[0].receipt.passed = 1 } 'Rejected'
Invoke-Case 'duplicate-platform' { param($s) $s.rows[1].platform = $s.rows[0].platform } 'Rejected'
Invoke-Case 'changed-source' { param($s) $s.rows[0].receipt.documentation.engineTree = 'c' * 40 } 'Rejected'
Invoke-Case 'missing-tested-commit' { param($s) $s.rows[0].receipt.Remove('commit') } 'Rejected'
Invoke-Case 'different-tested-commits' { param($s) $s.rows[0].receipt.commit = 'e' * 40 } 'Rejected'
Invoke-Case 'changed-catalog' { param($s) $s.rows[0].receipt.sourceApi[0].catalogIdentity = 'c' * 64 } 'Rejected'
Invoke-Case 'changed-reference' { param($s) $s.rows[0].receipt.sourceApi[0].references[0].identity = 'c' * 64 } 'Rejected'
Invoke-Case 'wrong-target' { param($s) $s.rows[0].receipt.sourceApi[0].target = 'other' } 'Rejected'
Invoke-Case 'missing-examples' { param($s) $s.rows[0].receipt.sourceExamples = @(); $s.rows[1].receipt.sourceExamples = @() } 'Blocked'
Invoke-Case 'changed-example-code' { param($s) $s.rows[0].receipt.sourceExamples[0].pages[0].blocks = @('c' * 64) } 'Rejected'
Invoke-Case 'changed-example-mode' { param($s) $s.rows[0].receipt.sourceExamples[0].pages[0].mode = 'runtime-script' } 'Rejected'
Invoke-Case 'changed-example-owner' { param($s) $s.rows[0].receipt.sourceExamples[0].pages[0].projects = @('Other') } 'Rejected'
Invoke-Case 'unhashed-example-report' { param($s) $s.rows[0].receipt.sourceExamples[0].reportIdentity = 'unknown' } 'Rejected'
Write-Output "Passed $($results.Count) source certificate cases. Evidence: $run"
