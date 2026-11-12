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
        $owner = if ($_ -ceq 'host-one') { 'Probe' } else { 'Other' }
        @{ platform = $_; receipt = @{ commit = 'd' * 40; environment = $hash; required = 2; passed = 2; reused = 0
            documentation = @{ engineTree = $tree; documentationSnapshot = $hash; contentIdentity = $hash }
            sourceExamples = @(@{ configuration = 'Debug'; target = $null; catalogIdentity = $catalogHash; reportIdentity = $hash
                inputs = @(@{ project = 'src/Probe.csproj'; owners = @('src/Probe.csproj'); sourceIdentity = $hash; environment = $hash
                    configuration = 'Debug'; target = $null; packages = @{}; sources = @{ 'src/Probe.csproj' = $hash } })
                pages = @(@{ page = 'task.probe'; mode = 'host'; projects = @('Probe'); blocks = @($hash) }) })
            sourceApi = @(@{ configuration = 'Debug'; target = $null; catalogIdentity = $catalogHash; reportIdentity = $hash
                inputMode = 'SourceOnly'; inputs = @(@{ project = $owner; path = "src/$owner.csproj"; owners = @("src/$owner.csproj")
                    sourceIdentity = $hash; environment = $hash; referenceIdentity = $hash; configuration = 'Debug'; target = $null
                    packages = @{}; sources = @{ "src/$owner.csproj" = $hash } })
                references = @(@{ path = 'reference/api/probe.json'; identity = $hash }); projects = @($owner) }) } }
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
            $expectedInputs = @($state.rows | ForEach-Object { $_.receipt.sourceApi | ForEach-Object { $_.inputs } })
            if (@($proof.inputs).Count -ne $expectedInputs.Count) { throw 'Source certificate lost owner input proofs.' }
            foreach ($inputProof in @($proof.inputs)) {
                $original = @($expectedInputs | Where-Object project -CEQ $inputProof.project)
                if ($original.Count -ne 1 -or $inputProof.sourceIdentity -cne $original[0].sourceIdentity `
                    -or $inputProof.referenceIdentity -cne $original[0].referenceIdentity `
                    -or $inputProof.packages.'probe/1.0.0' -cne $original[0].packages['probe/1.0.0'] -or $inputProof.reportIdentity -cne $hash `
                    -or $inputProof.sources.($original[0].path) -cne $hash) {
                    throw 'Source certificate altered source, dependency or comparison provenance.'
                }
            }
        } elseif (Test-Path $file) { throw 'Incomplete coverage published a certificate.' }
    } catch { $actual = 'Rejected'; $diagnostic = $_.Exception.Message }
    $results.Add(@{ name = $Name; actual = $actual; expected = $Expected; diagnostic = $diagnostic })
    $results | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $run 'tests.json') -Encoding utf8NoBOM
    if ($actual -cne $Expected) { throw "Unexpected certificate result for ${Name}: $actual $diagnostic" }
}
Invoke-Case 'combined-platform-coverage' { param($s) } 'Passed'
Invoke-Case 'missing-owner' { param($s) $s.rows[1].receipt.sourceApi[0].projects = @(); $s.rows[1].receipt.sourceApi[0].inputs = @() } 'Blocked'
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
function Add-CompiledInputs($state) {
    foreach ($row in $state.rows) {
        $check = $row.receipt.sourceApi[0]
        $owner = $check.projects[0]
        $check.inputMode = 'Compiled'
        $check.inputs = @(@{ project = $owner; path = "src/$owner.csproj"; owners = @("src/$owner.csproj")
            sourceIdentity = $hash; environment = $hash; referenceIdentity = $hash; configuration = 'Debug'; target = $null
            packages = @{ 'probe/1.0.0' = $hash }; sources = @{ "src/$owner.csproj" = $hash } })
    }
}
Invoke-Case 'compiled-owner-inputs' { param($s) Add-CompiledInputs $s } 'Passed'
Invoke-Case 'missing-input-mode' { param($s) $s.rows[0].receipt.sourceApi[0].Remove('inputMode') } 'Rejected'
Invoke-Case 'missing-compiled-inputs' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs = @() } 'Rejected'
Invoke-Case 'different-input-environment' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].environment = 'c' * 64 } 'Rejected'
Invoke-Case 'different-input-owner' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].project = 'Unknown' } 'Rejected'
Invoke-Case 'unhashed-owner-source' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].sourceIdentity = 'missing' } 'Rejected'
Invoke-Case 'unhashed-owner-reference' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].referenceIdentity = 'missing' } 'Rejected'
Invoke-Case 'escaping-input-owner' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].owners += '../Foreign.csproj' } 'Rejected'
Invoke-Case 'duplicate-input-owner' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].owners += 'src/Probe.csproj' } 'Rejected'
Invoke-Case 'invalid-package-map' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].packages = @('missing') } 'Rejected'
Invoke-Case 'escaping-package' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].packages = @{ '../1.0.0' = $hash } } 'Rejected'
Invoke-Case 'unhashed-package' { param($s) Add-CompiledInputs $s; $s.rows[0].receipt.sourceApi[0].inputs[0].packages = @{ 'probe/1.0.0' = 'missing' } } 'Rejected'
Invoke-Case 'missing-source-map' { param($s) $s.rows[0].receipt.sourceApi[0].inputs[0].sources = @{} } 'Rejected'
Invoke-Case 'escaping-source-map' { param($s) $s.rows[0].receipt.sourceApi[0].inputs[0].sources = @{ '../outside' = $hash } } 'Rejected'
Invoke-Case 'unhashed-source-map' { param($s) $s.rows[0].receipt.sourceApi[0].inputs[0].sources = @{ 'src/Probe.csproj' = 'unknown' } } 'Rejected'
Write-Output "Passed $($results.Count) source certificate cases. Evidence: $run"
