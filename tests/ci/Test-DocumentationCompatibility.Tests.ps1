$ErrorActionPreference = 'Stop'
$run = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/documentation-compatibility-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$hash = 'a' * 64
$baseline = 'b' * 40
$commit = 'c' * 40
$tree = 'd' * 40
$results = [Collections.Generic.List[object]]::new()
function Invoke-Case([string]$Name, [scriptblock]$Change, [string]$Expected) {
    $directory = Join-Path $run $Name
    [IO.Directory]::CreateDirectory($directory) | Out-Null
    $sources = @{ 'src/Probe.csproj' = $hash; 'src/Probe.cs' = $hash; 'tools/Checker.cs' = $hash }
    $input = @{ project = 'Probe'; path = 'src/Probe.csproj'; owners = @('src/Probe.csproj'); platform = 'host-one'
        sourceIdentity = $hash; environment = $hash; configuration = 'Debug'; target = $null; referenceIdentity = $hash
        packages = @{}; sources = $sources; reportIdentity = $hash; inputMode = 'Compiled' }
    $state = @{
        previous = @{ format = 'inno-documentation-source/1'; repository = 'https://github.com/Wolfiora/InnoEngine'; sourceCommit = $baseline
            policyIdentity = (& "$PSScriptRoot/Get-DocumentationCertificatePolicy.ps1")
            sourceTree = 'e' * 40; inputs = @($input); examples = @(@{ page = 'task.probe'; mode = 'host'; projects = @('Probe'); blocks = @($hash)
                contexts = @(@{ platform = 'host-one'; configuration = 'Debug'; target = $null; reportIdentity = $hash; inputs = @($input) }) }) }
        catalog = @{ repository = 'https://github.com/Wolfiora/InnoEngine'; sourceTree = $tree; snapshot = $hash; contentIdentity = $hash
            references = @(@{ path = 'reference/projects/catalog.json'; identity = $hash; model = 'project-inventory' },
                @{ path = 'reference/api/probe.json'; identity = $hash; model = 'source-clr'; configuration = 'Debug'; target = $null; projects = @('Probe') })
            pages = @(@{ id = 'task.probe'; examples = @{ mode = 'host'; projects = @('Probe') }
                codeBlocks = @(@{ language = 'csharp'; contentIdentity = $hash; declarationSymbols = @() }) }) }
        plan = @{ commit = $commit; baseline = $baseline; environment = $hash; configuration = 'Debug'; owners = @()
            inputs = $sources.Clone(); inputSets = @{ 'src/Probe.csproj' = @('src/Probe.csproj', 'src/Probe.cs'); tooling = @('tools/Checker.cs') }
            documentationSources = @{ ('reference/api/probe.json' + "`0Probe") = $hash } }
    }
    & $Change $state
    $prior = Join-Path $directory 'previous.json'
    $plan = Join-Path $directory 'plan.json'
    $catalog = Join-Path $directory 'catalog.json'
    $receipt = Join-Path $directory 'certification.json'
    $state.previous | ConvertTo-Json -Depth 15 | Set-Content $prior -Encoding utf8NoBOM
    $state.plan | ConvertTo-Json -Depth 12 | Set-Content $plan -Encoding utf8NoBOM
    $state.catalog | ConvertTo-Json -Depth 12 | Set-Content $catalog -Encoding utf8NoBOM
    $planIdentity = (Get-FileHash $plan).Hash.ToLowerInvariant()
    @(@{ platform = 'host-one'; receipt = @{ commit = $commit; environment = $state.plan.environment; required = 2; passed = 2; reused = 0
        planIdentity = $(if ($state.tamper) { 'f' * 64 } else { $planIdentity }); sourceApi = @(); sourceExamples = @()
        documentation = @{ engineTree = $tree; documentationSnapshot = $hash; contentIdentity = $hash } } }) |
        ConvertTo-Json -Depth 10 | Set-Content $receipt -Encoding utf8NoBOM
    $actual = 'Rejected'; $diagnostic = ''
    try {
        $output = Join-Path $directory 'output'
        $report = & "$PSScriptRoot/New-DocumentationSourceCertificate.ps1" -CertificationPath $receipt -CatalogPaths @($catalog) `
            -OutputDirectory $output -ProducerOrigin 'fixture://current-authenticated-run' -PreviousCertificatePath $prior `
            -PreviousOrigin 'fixture://previous-authenticated-run' -PlanPaths @($plan)
        $actual = $report.status
        $file = Join-Path $output 'source-certificate.json'
        if ($actual -ceq 'Passed') {
            $certificate = Get-Content $file -Raw | ConvertFrom-Json
            if ($certificate.sourceCommit -cne $commit -or $certificate.inputs.Count -ne 1 -or $certificate.examples.Count -ne 1 `
                -or $certificate.inputs[0].compatibility.sourceCommit -cne $baseline `
                -or $certificate.inputs[0].compatibility.certificateIdentity -cne (Get-FileHash $prior).Hash.ToLowerInvariant() `
                -or $certificate.examples[0].contexts[0].compatibility.planIdentity -cne $planIdentity `
                -or $certificate.inputs[0].reportIdentity -cne $hash) { throw 'Compatibility lost original evidence or current input provenance.' }
        } elseif (Test-Path $file) { throw 'Incomplete compatibility published a certificate.' }
    } catch { $actual = 'Rejected'; $diagnostic = $_.Exception.Message }
    $results.Add(@{ name = $Name; actual = $actual; expected = $Expected; diagnostic = $diagnostic })
    $results | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $run 'tests.json') -Encoding utf8NoBOM
    if ($actual -cne $Expected) { throw "Unexpected compatibility for ${Name}: $actual $diagnostic" }
}
Invoke-Case 'unchanged-owner-and-example' { param($s) } 'Passed'
function Add-ScriptCompatibility($s) {
    $s.catalog.references += @{ path = 'reference/api/runtime-script.json'; identity = $hash; model = 'runtime-script'; configuration = 'Debug'; target = $null; projects = @('Probe') }
    $s.previous.scriptReferences = @(@{ platform = 'host-one'; path = 'reference/api/runtime-script.json'; identity = $hash; model = 'runtime-script'
        configuration = 'Debug'; projects = @('Probe'); projectionIdentities = @{ 'Inno.ScriptApi.Runtime' = $hash }; documentationIdentity = $hash
        reportIdentity = $hash; inputs = @($s.previous.inputs[0]) })
}
Invoke-Case 'unchanged-projected-inputs' { param($s) Add-ScriptCompatibility $s } 'Passed'
Invoke-Case 'changed-projected-reference' { param($s) Add-ScriptCompatibility $s; $s.catalog.references[2].identity = 'f' * 64 } 'Blocked'
Invoke-Case 'changed-projected-source' { param($s) Add-ScriptCompatibility $s; $s.plan.inputs['src/Probe.cs'] = 'f' * 64 } 'Blocked'
Invoke-Case 'unhashed-projected-input' { param($s) Add-ScriptCompatibility $s; $s.previous.scriptReferences[0].projectionIdentities['Inno.ScriptApi.Runtime'] = 'unknown' } 'Blocked'
Invoke-Case 'unrelated-owner-change' { param($s) $s.plan.owners = @('src/Unrelated.csproj') } 'Passed'
Invoke-Case 'unchanged-unprepared-native-pin' { param($s) $s.previous.inputs[0].sources['extern/native'] = 'f' * 40; $s.plan.inputs['extern/native'] = 'unavailable:' + ('f' * 40) } 'Passed'
Invoke-Case 'changed-unprepared-native-pin' { param($s) $s.previous.inputs[0].sources['extern/native'] = 'f' * 40; $s.plan.inputs['extern/native'] = 'unavailable:' + ('e' * 40) } 'Blocked'
Invoke-Case 'changed-owner' { param($s) $s.plan.owners = @('src/Probe.csproj') } 'Blocked'
Invoke-Case 'changed-source' { param($s) $s.plan.inputs['src/Probe.cs'] = 'f' * 64 } 'Blocked'
Invoke-Case 'missing-source' { param($s) $s.plan.inputs.Remove('src/Probe.cs') } 'Blocked'
Invoke-Case 'new-source' { param($s) $s.plan.inputs['src/Added.cs'] = $hash; $s.plan.inputSets['src/Probe.csproj'] += 'src/Added.cs' } 'Blocked'
Invoke-Case 'new-tool-input' { param($s) $s.plan.inputs['tools/Added.cs'] = $hash; $s.plan.inputSets.tooling += 'tools/Added.cs' } 'Blocked'
Invoke-Case 'changed-checker' { param($s) $s.plan.inputs['tools/Checker.cs'] = 'f' * 64 } 'Blocked'
Invoke-Case 'changed-certificate-policy' { param($s) $s.previous.policyIdentity = 'f' * 64 } 'Blocked'
Invoke-Case 'changed-environment' { param($s) $s.plan.environment = 'f' * 64 } 'Blocked'
Invoke-Case 'changed-configuration' { param($s) $s.plan.configuration = 'Release' } 'Blocked'
Invoke-Case 'different-baseline' { param($s) $s.plan.baseline = 'f' * 40 } 'Blocked'
Invoke-Case 'changed-reference-facts' { param($s) $s.plan.documentationSources["reference/api/probe.json`0Probe"] = 'f' * 64 } 'Blocked'
Invoke-Case 'changed-example-block' { param($s) $s.catalog.pages[0].codeBlocks[0].contentIdentity = 'f' * 64 } 'Blocked'
Invoke-Case 'changed-example-mode' { param($s) $s.catalog.pages[0].examples.mode = 'runtime-script' } 'Blocked'
Invoke-Case 'changed-example-owner' { param($s) $s.catalog.pages[0].examples.projects = @('Other') } 'Blocked'
Invoke-Case 'missing-prior-inputs' { param($s) $s.previous.inputs = @() } 'Blocked'
Invoke-Case 'missing-example-provenance' { param($s) $s.previous.examples[0].contexts[0].inputs = @() } 'Blocked'
Invoke-Case 'tampered-plan' { param($s) $s.tamper = $true } 'Rejected'
Invoke-Case 'different-catalog-source' { param($s) $s.catalog.sourceTree = 'f' * 40 } 'Rejected'
Invoke-Case 'foreign-certificate' { param($s) $s.previous.repository = 'https://github.com/foreign/repository' } 'Rejected'
Write-Output "Passed $($results.Count) documentation input compatibility cases. Evidence: $run"
