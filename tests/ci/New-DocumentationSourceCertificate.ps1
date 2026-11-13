param(
    [Parameter(Mandatory)][string]$CertificationPath,
    [Parameter(Mandatory)][string[]]$CatalogPaths,
    [Parameter(Mandatory)][string]$OutputDirectory,
    [Parameter(Mandatory)][string]$ProducerOrigin,
    [string]$PreviousCertificatePath,
    [string]$PreviousOrigin,
    [string[]]$PlanPaths
)

$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/DocumentationCertificateInputs.ps1"
$inputStore = New-DocumentationInputStore
$policyIdentity = & "$PSScriptRoot/Get-DocumentationCertificatePolicy.ps1"
$receipts = @(Get-Content -LiteralPath $CertificationPath -Raw | ConvertFrom-Json)
if ($receipts.Count -eq 0 -or $receipts.Count -ne $CatalogPaths.Count -or [string]::IsNullOrWhiteSpace($ProducerOrigin)) {
    throw 'Source certification requires every reconciled producer and its exact catalog.'
}
$catalogs = @($CatalogPaths | ForEach-Object { Get-Content -LiteralPath $_ -Raw | ConvertFrom-Json })
$catalog = $catalogs[0]
if (@($receipts | Where-Object { [string]$_.receipt.commit -cnotmatch '^[a-f0-9]{40}$' }).Count -ne 0) {
    throw 'Every source certification receipt must identify its exact tested commit.'
}
$commits = @($receipts | ForEach-Object { $_.receipt.commit } | Sort-Object -Unique)
if ($commits.Count -ne 1 -or $commits[0] -cnotmatch '^[a-f0-9]{40}$') {
    throw 'Source certification requires one exact tested commit across the reconciled matrix.'
}
$inventory = @($catalog.references | Where-Object model -CEQ 'project-inventory')
if ($inventory.Count -ne 1 -or $inventory[0].identity -cnotmatch '^[a-f0-9]{64}$') { throw 'The source catalog lacks its exact project inventory.' }
$coverage = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$exampleCoverage = [Collections.Generic.Dictionary[string,object]]::new([StringComparer]::Ordinal)
$scriptCoverage = [Collections.Generic.List[object]]::new()
$sourceInputs = [Collections.Generic.List[object]]::new()
$inputOwners = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
if ($null -eq $catalog.pages) { throw 'Source certification requires the canonical example inventory.' }
$platforms = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
for ($index = 0; $index -lt $receipts.Count; $index++) {
    $row = $receipts[$index]
    $receipt = $row.receipt
    $selected = $catalogs[$index]
    if (-not $platforms.Add($row.platform) -or [string]::IsNullOrWhiteSpace($row.platform) `
        -or $receipt.required -le 0 -or $receipt.required -ne ($receipt.passed + $receipt.reused) `
        -or $receipt.documentation.engineTree -cne $catalog.sourceTree `
        -or $receipt.documentation.documentationSnapshot -cne $catalog.snapshot `
        -or $receipt.documentation.contentIdentity -cne $catalog.contentIdentity `
        -or $selected.snapshot -cne $catalog.snapshot -or $selected.repository -cne $catalog.repository `
        -or $selected.sourceTree -cne $catalog.sourceTree -or $selected.contentIdentity -cne $catalog.contentIdentity) {
        throw 'Source certification requires a complete matching matrix and one immutable documentation input.'
    }
    $catalogIdentity = (Get-FileHash -LiteralPath $CatalogPaths[$index] -Algorithm SHA256).Hash.ToLowerInvariant()
    foreach ($check in @($receipt.sourceApi)) {
        if ($null -eq $check -or $check.catalogIdentity -cne $catalogIdentity -or $check.reportIdentity -cnotmatch '^[a-f0-9]{64}$') {
            throw 'Source comparison provenance does not bind its exact catalog and report.'
        }
        if ($check.inputMode -cnotin @('Compiled', 'SourceOnly') `
            -or @($check.inputs).Count -ne @($check.projects).Count) {
            throw 'Source comparison lacks explicit compiled-input or source-only provenance.'
        }
        foreach ($proof in @($check.inputs)) {
            $key = $row.platform + "`0" + $check.configuration + "`0" + $check.target + "`0" + $proof.project
            if ($proof.project -cnotin $check.projects -or -not $inputOwners.Add($key) `
                -or $proof.sourceIdentity -cnotmatch '^[a-f0-9]{64}$' -or $proof.referenceIdentity -cnotmatch '^[a-f0-9]{64}$' `
                -or $receipt.environment -cnotmatch '^[a-f0-9]{64}$' -or $proof.environment -cne $receipt.environment `
                -or $proof.configuration -cne $check.configuration -or $proof.target -cne $check.target `
                -or @($proof.owners).Count -eq 0 -or $proof.path -cnotin $proof.owners `
                -or @($proof.owners | Sort-Object -Unique).Count -ne @($proof.owners).Count -or $null -eq $proof.packages) {
                throw 'Compiled source input provenance is incomplete, duplicated or belongs to another execution context.'
            }
            foreach ($owner in @($proof.owners)) {
                if ([string]::IsNullOrWhiteSpace($owner) -or [IO.Path]::IsPathRooted($owner) -or $owner.Contains('\') `
                    -or $owner.Contains(':') -or $owner -match '(^|/)\.{1,2}(/|$)' -or -not $owner.EndsWith('.csproj')) {
                    throw 'Compiled source input provenance has an invalid repository owner.'
                }
            }
            $packages = $proof.packages | ConvertTo-Json -Depth 4 | ConvertFrom-Json -AsHashtable
            if ($packages -isnot [Collections.IDictionary]) { throw 'Compiled source packages must be an explicit content map.' }
            if ($check.inputMode -ceq 'SourceOnly' -and $packages.Count -ne 0) { throw 'Source-only inspection cannot claim compiled packages.' }
            foreach ($package in $packages.GetEnumerator()) {
                if ($package.Key -cnotmatch '^[a-z0-9_.-]+/[a-z0-9_.+-]+$' -or $package.Key -match '(^|/)\.{1,2}(/|$)' `
                    -or $package.Value -cnotmatch '^[a-f0-9]{64}$') { throw 'Compiled source packages have invalid paths or identities.' }
            }
            $sources = & "$PSScriptRoot/Get-DocumentationSourceInputs.ps1" -Inputs $proof.sources
            $sourceInputs.Add((Add-DocumentationInputProof $inputStore ([ordered]@{ platform = $row.platform; check = $check.check; inputMode = $check.inputMode; project = $proof.project; path = $proof.path
                owners = @($proof.owners); sourceIdentity = $proof.sourceIdentity; environment = $proof.environment
                configuration = $proof.configuration; target = $proof.target; referenceIdentity = $proof.referenceIdentity
                packages = $packages; sources = $sources; reportIdentity = $check.reportIdentity })))
        }
        foreach ($reference in @($check.references)) {
            $bound = @($catalog.references | Where-Object { $_.path -ceq $reference.path -and $_.identity -ceq $reference.identity `
                -and $_.model -ceq 'source-clr' -and $_.configuration -ceq $check.configuration -and $_.target -ceq $check.target })
            if ($bound.Count -ne 1) { throw 'A reconciled source comparison names a different reference context.' }
            foreach ($project in @($bound[0].projects | Where-Object { $_ -cin $check.projects })) {
                [void]$coverage.Add($reference.path + "`0" + $project)
            }
        }
    }
    foreach ($check in @($receipt.sourceExamples)) {
        if ($null -eq $check) { continue }
        if ($check.catalogIdentity -cne $catalogIdentity -or $check.reportIdentity -cnotmatch '^[a-f0-9]{64}$' `
            -or [string]::IsNullOrWhiteSpace($check.configuration) -or @($check.pages).Count -eq 0) {
            throw 'Example certification lacks its exact catalog, context or successful report.'
        }
        if (@($check.inputs).Count -eq 0) { throw 'Example certification lacks its source input closure.' }
        $normalized = foreach ($inputProof in @($check.inputs)) {
            if ($inputProof.environment -cne $receipt.environment -or $inputProof.configuration -cne $check.configuration `
                -or $inputProof.target -cne $check.target -or $inputProof.sourceIdentity -cnotmatch '^[a-f0-9]{64}$') {
                throw 'Example certification input proof has a different environment or context.'
            }
            Add-DocumentationInputProof $inputStore $inputProof
        }
        $group = $inputStore.groups.Count
        $inputStore.groups.Add(@{ platform = $row.platform; configuration = $check.configuration; target = $check.target
            reportIdentity = $check.reportIdentity; inputs = @($normalized) })
        foreach ($projection in @($check.scriptReferences | Where-Object { $null -ne $_ })) {
            $bound = @($catalog.references | Where-Object { $_.path -ceq $projection.path -and $_.identity -ceq $projection.identity `
                -and $_.model -ceq $projection.model -and $_.model -cin @('runtime-script', 'editor-script') `
                -and $_.configuration -ceq $check.configuration -and $null -eq $_.target })
            if ($bound.Count -ne 1 -or $projection.status -cne 'Passed' -or @($projection.differences).Count -ne 0 `
                -or $projection.configuration -cne $check.configuration -or @($check.inputs).Count -eq 0 `
                -or @($check.pages | Where-Object { $_.mode -ceq $projection.model -or ($projection.model -ceq 'runtime-script' -and $_.mode -ceq 'editor-script') }).Count -eq 0 `
                -or $projection.documentationIdentity -cnotmatch '^[a-f0-9]{64}$' `
                -or (@($projection.projects) -join "`n") -cne (@($bound[0].projects) -join "`n")) {
                throw 'Script certification differs from its actual successful projection comparison.'
            }
            $names = @($projection.projectionIdentities.PSObject.Properties | ForEach-Object Name | Sort-Object -CaseSensitive)
            $required = if ($projection.model -ceq 'editor-script') { @('Inno.ScriptApi.Editor', 'Inno.ScriptApi.Runtime') } else { @('Inno.ScriptApi.Runtime') }
            if (($names -join "`n") -cne ($required -join "`n") `
                -or @($projection.projectionIdentities.PSObject.Properties | Where-Object { $_.Value -cnotmatch '^[a-f0-9]{64}$' }).Count -ne 0) {
                throw 'Script certification lacks its actual logical assembly identities.'
            }
            foreach ($project in $bound[0].projects) { [void]$coverage.Add($projection.path + "`0" + $project) }
            $scriptCoverage.Add(@{ platform = $row.platform; path = $projection.path; identity = $projection.identity; model = $projection.model
                configuration = $check.configuration; target = $check.target; projects = @($projection.projects); projectionIdentities = $projection.projectionIdentities
                documentationIdentity = $projection.documentationIdentity; reportIdentity = $check.reportIdentity; inputGroup = $group })
        }
        foreach ($proof in @($check.pages)) {
            $page = @($catalog.pages | Where-Object id -CEQ $proof.page)
            if ($page.Count -ne 1 -or $null -eq $page[0].examples -or $proof.mode -cne $page[0].examples.mode `
                -or (@($proof.projects) -join "`n") -cne (@($page[0].examples.projects) -join "`n")) {
                throw 'Example certification differs from its canonical execution context.'
            }
            $blocks = @($page[0].codeBlocks | Where-Object { $_.language -cin @('cs', 'csharp') -and @($_.declarationSymbols).Count -eq 0 } | ForEach-Object contentIdentity)
            if ($blocks.Count -eq 0 -or ($blocks -join "`n") -cne (@($proof.blocks) -join "`n") `
                -or @($blocks | Where-Object { $_ -cnotmatch '^[a-f0-9]{64}$' }).Count -ne 0) {
                throw 'Example certification differs from its canonical executable blocks.'
            }
            if (-not $exampleCoverage.ContainsKey($proof.page)) {
                $exampleCoverage.Add($proof.page, [ordered]@{ page = $proof.page; mode = $proof.mode; projects = @($proof.projects); blocks = $blocks
                    contexts = [Collections.Generic.List[object]]::new() })
            }
            $exampleCoverage[$proof.page].contexts.Add(@{ platform = $row.platform; configuration = $check.configuration; target = $check.target
                reportIdentity = $check.reportIdentity; inputGroup = $group })
        }
    }
}
$compatibility = $null
if ($PreviousCertificatePath) {
    $compatibility = & "$PSScriptRoot/Get-CompatibleDocumentationEvidence.ps1" -PreviousCertificatePath $PreviousCertificatePath `
        -CertificationPath $CertificationPath -PlanPaths $PlanPaths -CatalogPaths $CatalogPaths -PreviousOrigin $PreviousOrigin -PolicyIdentity $policyIdentity
    foreach ($inherited in @($compatibility.api)) {
        $proof = $inherited.proof
        [void]$coverage.Add($inherited.reference + "`0" + $proof.project)
        $key = $proof.platform + "`0" + $proof.configuration + "`0" + $proof.target + "`0" + $proof.project
        if (-not $inputOwners.Add($key)) { continue }
        $entry = $proof | ConvertTo-Json -Depth 12 | ConvertFrom-Json -AsHashtable
        $entry['compatibility'] = $inherited.compatibility
        $sourceInputs.Add((Add-DocumentationInputProof $inputStore $entry))
    }
    $inheritedGroups = @{}
    function Import-InputGroup($context, $proofs) {
        $key = $context.platform + "`0" + $context.configuration + "`0" + $context.target + "`0" + $context.reportIdentity
        if (-not $inheritedGroups.ContainsKey($key)) {
            $inheritedGroups[$key] = $inputStore.groups.Count
            $inputStore.groups.Add(@{ platform = $context.platform; configuration = $context.configuration; target = $context.target
                reportIdentity = $context.reportIdentity; inputs = @($proofs | ForEach-Object { Add-DocumentationInputProof $inputStore $_ }) })
        }
        $context['inputGroup'] = $inheritedGroups[$key]
    }
    foreach ($inherited in @($compatibility.examples)) {
        $context = $inherited.context | ConvertTo-Json -Depth 12 | ConvertFrom-Json -AsHashtable
        $context['compatibility'] = $inherited.compatibility
        Import-InputGroup $context $inherited.inputs
        if (-not $exampleCoverage.ContainsKey($inherited.page)) {
            $exampleCoverage.Add($inherited.page, [ordered]@{ page = $inherited.page; mode = $inherited.mode
                projects = @($inherited.projects); blocks = @($inherited.blocks); contexts = [Collections.Generic.List[object]]::new() })
        }
        $contexts = $exampleCoverage[$inherited.page].contexts
        if (@($contexts | Where-Object { $_.platform -ceq $context.platform -and $_.configuration -ceq $context.configuration `
            -and $_.target -ceq $context.target }).Count -eq 0) { $contexts.Add($context) }
    }
    foreach ($inherited in @($compatibility.scripts | Where-Object { $null -ne $_ })) {
        $proof = $inherited.proof
        foreach ($project in $proof.projects) { [void]$coverage.Add($proof.path + "`0" + $project) }
        if (@($scriptCoverage | Where-Object { $_.platform -ceq $proof.platform -and $_.path -ceq $proof.path }).Count -eq 0) {
            $entry = $proof | ConvertTo-Json -Depth 15 | ConvertFrom-Json -AsHashtable
            $entry['compatibility'] = $inherited.compatibility
            Import-InputGroup $entry $inherited.inputs
            $scriptCoverage.Add($entry)
        }
    }
}
$references = @($catalog.references | Where-Object model -CNE 'project-inventory')
$missing = @($references | ForEach-Object {
    $reference = $_
    if ($reference.model -cnotin @('source-clr', 'runtime-script', 'editor-script')) { "Unsupported source certification surface: $($reference.path)" }
    foreach ($project in @($reference.projects)) {
        if (-not $coverage.Contains($reference.path + "`0" + $project)) { "$($reference.path): $project" }
    }
})
if ($references.Count -eq 0) { $missing += 'No source API references were captured.' }
foreach ($page in @($catalog.pages)) {
    $executable = @($page.codeBlocks | Where-Object { $_.language -cin @('cs', 'csharp') -and @($_.declarationSymbols).Count -eq 0 })
    if ($executable.Count -gt 0 -and -not $exampleCoverage.ContainsKey($page.id)) { $missing += "Executable examples: $($page.id)" }
}
[IO.Directory]::CreateDirectory([IO.Path]::GetFullPath($OutputDirectory)) | Out-Null
$report = @{ status = $(if ($missing.Count) { 'Blocked' } else { 'Passed' }); missing = $missing; sourceTree = $catalog.sourceTree
    compatibility = $(if ($null -ne $compatibility) { @{ sourceOwners = @($compatibility.api).Count
        exampleContexts = @($compatibility.examples).Count; diagnostics = @($compatibility.diagnostics) } } else { $null }) }
$report | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory 'source-certification-status.json') -Encoding utf8NoBOM
if ($missing.Count) { return [pscustomobject]$report }
$certificate = [ordered]@{
    format = 'inno-documentation-source/2'; repository = $catalog.repository; sourceTree = $catalog.sourceTree; sourceCommit = $commits[0]
    policyIdentity = $policyIdentity
    sourceFiles = $inputStore.files.ToArray()
    sourceSets = $inputStore.sets.ToArray()
    inputGroups = $inputStore.groups.ToArray()
    scriptReferences = @($scriptCoverage)
    projectManifestIdentity = $inventory[0].identity
    references = @($references | Sort-Object path -CaseSensitive | ForEach-Object {
        [ordered]@{ path = $_.path; identity = $_.identity; model = $_.model; configuration = $_.configuration; target = $_.target
            projects = @($_.projects | Sort-Object -CaseSensitive) }
    })
    producers = @(@{ origin = $ProducerOrigin; receiptIdentity = (Get-FileHash -LiteralPath $CertificationPath -Algorithm SHA256).Hash.ToLowerInvariant() })
    examples = @($exampleCoverage.Keys | Sort-Object -CaseSensitive | ForEach-Object { $exampleCoverage[$_] })
    inputs = @($sourceInputs | Sort-Object platform, project, configuration, target -CaseSensitive)
}
$file = Join-Path $OutputDirectory 'source-certificate.json'
if (Test-Path -LiteralPath $file) { throw 'Source certification must not overwrite a previous producer result.' }
$json = $certificate | ConvertTo-Json -Depth 12 -Compress
if ([Text.Encoding]::UTF8.GetByteCount($json) -gt 16MB) { throw 'Normalized source certificate exceeds its 16 MiB import boundary.' }
[IO.File]::WriteAllText($file, $json, [Text.UTF8Encoding]::new($false))
[pscustomobject]$report
