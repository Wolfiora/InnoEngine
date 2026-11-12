param(
    [Parameter(Mandatory)][string]$CertificationPath,
    [Parameter(Mandatory)][string[]]$CatalogPaths,
    [Parameter(Mandatory)][string]$OutputDirectory,
    [Parameter(Mandatory)][string]$ProducerOrigin
)

$ErrorActionPreference = 'Stop'
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
            $exampleCoverage[$proof.page].contexts.Add(@{ platform = $row.platform; configuration = $check.configuration; target = $check.target; reportIdentity = $check.reportIdentity })
        }
    }
}
$references = @($catalog.references | Where-Object model -CNE 'project-inventory')
$missing = @($references | ForEach-Object {
    $reference = $_
    if ($reference.model -cne 'source-clr') { "Unsupported source certification surface: $($reference.path)" }
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
$report = @{ status = $(if ($missing.Count) { 'Blocked' } else { 'Passed' }); missing = $missing; sourceTree = $catalog.sourceTree }
$report | ConvertTo-Json -Depth 5 | Set-Content (Join-Path $OutputDirectory 'source-certification-status.json') -Encoding utf8NoBOM
if ($missing.Count) { return [pscustomobject]$report }
$certificate = [ordered]@{
    format = 'inno-documentation-source/1'; repository = $catalog.repository; sourceTree = $catalog.sourceTree; sourceCommit = $commits[0]
    projectManifestIdentity = $inventory[0].identity
    references = @($references | Sort-Object path -CaseSensitive | ForEach-Object {
        [ordered]@{ path = $_.path; identity = $_.identity; model = $_.model; configuration = $_.configuration; target = $_.target
            projects = @($_.projects | Sort-Object -CaseSensitive) }
    })
    producers = @(@{ origin = $ProducerOrigin; receiptIdentity = (Get-FileHash -LiteralPath $CertificationPath -Algorithm SHA256).Hash.ToLowerInvariant() })
    examples = @($exampleCoverage.Keys | Sort-Object -CaseSensitive | ForEach-Object { $exampleCoverage[$_] })
}
$file = Join-Path $OutputDirectory 'source-certificate.json'
if (Test-Path -LiteralPath $file) { throw 'Source certification must not overwrite a previous producer result.' }
$certificate | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $file -Encoding utf8NoBOM
[pscustomobject]$report
