param(
    [Parameter(Mandatory)]$Plan,
    [Parameter(Mandatory)]$Evidence,
    [Parameter(Mandatory)]$Context,
    [Parameter(Mandatory)][string]$Directory
)

$ErrorActionPreference = 'Stop'
$selected = @($Plan.checks | Where-Object { $_.kind -ceq 'documentation-examples' -and $null -eq $_.exclusion })
if ($selected.Count -eq 0) { return }
$directory = [IO.Path]::GetFullPath($Directory)
$catalogPath = Join-Path $directory 'documentation-catalog.json'
if ((Get-Item -LiteralPath $catalogPath).LinkType) { throw 'Example certification cannot follow a linked catalog.' }
$catalog = Get-Content -LiteralPath $catalogPath -Raw | ConvertFrom-Json
if ($catalog.snapshot -cne $Context.snapshot -or $catalog.sourceTree -cne $Context.sourceTree `
    -or $catalog.contentIdentity -cne $Context.contentIdentity -or $catalog.repository -cne 'https://github.com/Wolfiora/InnoEngine') {
    throw 'Example certification catalog differs from its reconciled pair.'
}
foreach ($check in $selected) {
    if ($check.id -cnotmatch '^documentation-examples:[a-zA-Z0-9_-]+$') { throw 'Invalid example report identity.' }
    $passed = @($Evidence.results | Where-Object { $_.id -ceq $check.id -and $_.status -cin @('Passed', 'Reused') })
    if ($passed.Count -ne 1) { throw 'Required example execution did not pass exactly once.' }
    $proofs = @($passed[0].documentationInputs | Where-Object { $null -ne $_ })
    $expectedOwners = @($check.projects | Sort-Object -CaseSensitive)
    if ($proofs.Count -eq 0 -or (@($proofs | ForEach-Object project | Sort-Object -CaseSensitive) -join "`n") -cne ($expectedOwners -join "`n") `
        -or @($proofs | ForEach-Object project | Sort-Object -Unique).Count -ne $proofs.Count) {
        throw 'Example input provenance must cover every planned source owner exactly once.'
    }
    foreach ($proof in $proofs) {
        if ($proof.environment -cnotmatch '^[a-f0-9]{64}$' -or $proof.environment -cne $Plan.environment `
            -or $proof.sourceIdentity -cnotmatch '^[a-f0-9]{64}$' -or $proof.configuration -cne $check.documentation.configuration `
            -or $proof.target -cne $check.documentation.target -or $proof.project -cnotin @($proof.owners) `
            -or @($proof.owners | Sort-Object -Unique).Count -ne @($proof.owners).Count) {
            throw 'Example input provenance differs from its actual owner, environment or context.'
        }
        foreach ($owner in @($proof.owners)) {
            if ([string]::IsNullOrWhiteSpace($owner) -or [IO.Path]::IsPathRooted($owner) -or $owner.Contains('\') -or $owner.Contains(':') `
                -or $owner -match '(^|/)\.{1,2}(/|$)' -or -not $owner.EndsWith('.csproj')) { throw 'Example input owner escapes its repository boundary.' }
        }
        $null = & "$PSScriptRoot/Get-DocumentationSourceInputs.ps1" -Inputs $proof.sources
        $packages = $proof.packages | ConvertTo-Json -Depth 4 | ConvertFrom-Json -AsHashtable
        if ($packages -isnot [Collections.IDictionary]) { throw 'Example package inputs require an explicit content map.' }
        foreach ($package in $packages.GetEnumerator()) {
            if ($package.Key -cnotmatch '^[a-z0-9_.-]+/[a-z0-9_.+-]+$' -or $package.Key -match '(^|/)\.{1,2}(/|$)' `
                -or $package.Value -cnotmatch '^[a-f0-9]{64}$') { throw 'Example package inputs have an invalid path or identity.' }
        }
    }
    if ($check.context -cnotmatch '^[a-zA-Z0-9_-]+$' -or $check.id -cne "documentation-examples:$($check.context)") {
        throw 'Example receipt requires the exact planned execution context.'
    }
    $file = Join-Path $directory ($check.context + '-documentation-examples.json')
    if ((Get-Item -LiteralPath $file).LinkType) { throw 'Example certification cannot follow a linked report.' }
    $report = Get-Content -LiteralPath $file -Raw | ConvertFrom-Json
    if ($report.status -cne 'Passed' -or $report.snapshot -cne $catalog.snapshot -or $report.sourceTree -cne $catalog.sourceTree `
        -or $report.contentIdentity -cne $catalog.contentIdentity -or $report.configuration -cne $check.documentation.configuration `
        -or $report.target -cne $check.documentation.target) { throw 'Example report does not match its planned source context.' }
    $actual = @($report.results | ForEach-Object page | Sort-Object -CaseSensitive)
    $expected = @($check.documentation.pages | Sort-Object -CaseSensitive)
    if ($actual.Count -eq 0 -or ($actual -join "`n") -cne ($expected -join "`n") `
        -or @($actual | Select-Object -Unique).Count -ne $actual.Count) { throw 'Required example discovery was empty, incomplete or duplicated.' }
    $pages = foreach ($result in @($report.results)) {
        $page = @($catalog.pages | Where-Object id -CEQ $result.page)
        if ($page.Count -ne 1 -or $null -eq $page[0].examples -or $result.status -cne 'Passed' -or @($result.errors).Count -ne 0 `
            -or $result.contentIdentity -cne $page[0].contentIdentity -or $result.mode -cne $page[0].examples.mode) {
            throw 'Example result lost its exact page, mode or successful result.'
        }
        $blocks = @($page[0].codeBlocks | Where-Object { $_.language -cin @('cs', 'csharp') -and @($_.declarationSymbols).Count -eq 0 })
        $identities = @($blocks | ForEach-Object contentIdentity)
        if ($identities.Count -eq 0 -or ($identities -join "`n") -cne (@($result.blocks | ForEach-Object contentIdentity) -join "`n") `
            -or @($identities | Where-Object { $_ -cnotmatch '^[a-f0-9]{64}$' }).Count -ne 0 `
            -or $null -eq $result.references -or @($result.references.PSObject.Properties).Count -eq 0) {
            throw 'Example result lacks exact executable blocks or actual compilation references.'
        }
        foreach ($reference in $result.references.PSObject.Properties) {
            if ($reference.Value -cnotmatch '^[a-f0-9]{64}$') { throw 'Example compilation reference has no content identity.' }
        }
        if ($result.mode -cin @('runtime-script', 'editor-script')) {
            $names = @($result.references.PSObject.Properties | ForEach-Object Name)
            if ('Inno.ScriptApi.Runtime' -cnotin $names `
                -or ($result.mode -ceq 'editor-script' -and 'Inno.ScriptApi.Editor' -cnotin $names) `
                -or ($result.mode -ceq 'runtime-script' -and 'Inno.ScriptApi.Editor' -cin $names)) {
                throw 'Script example references do not match their required Runtime and Editor surfaces.'
            }
        }
        [pscustomobject]@{ page = $result.page; mode = $result.mode; projects = @($page[0].examples.projects); blocks = $identities }
    }
    $modes = @($pages | ForEach-Object mode)
    $expectedScriptReferences = @($catalog.references | Where-Object {
        ($_.model -ceq 'runtime-script' -and ('runtime-script' -cin $modes -or 'editor-script' -cin $modes)) `
        -or ($_.model -ceq 'editor-script' -and 'editor-script' -cin $modes)
    })
    $actualScriptReferences = @($report.scriptReferences | Where-Object { $null -ne $_ })
    if ((@($expectedScriptReferences.path | Sort-Object -CaseSensitive) -join "`n") `
        -cne (@($actualScriptReferences.path | Sort-Object -CaseSensitive) -join "`n")) {
        throw 'Projected API comparison is missing, duplicated or outside the selected script surfaces.'
    }
    foreach ($projection in $actualScriptReferences) {
        $bound = @($expectedScriptReferences | Where-Object path -CEQ $projection.path)
        if ($bound.Count -ne 1 -or $projection.status -cne 'Passed' -or @($projection.differences).Count -ne 0 `
            -or $projection.identity -cne $bound[0].identity -or $projection.model -cne $bound[0].model `
            -or $projection.configuration -cne $report.configuration -or $bound[0].configuration -cne $report.configuration `
            -or $null -ne $bound[0].target -or $projection.documentationIdentity -cnotmatch '^[a-f0-9]{64}$' `
            -or (@($projection.projects) -join "`n") -cne (@($bound[0].projects) -join "`n")) {
            throw 'Projected API comparison lost its exact reference, owners, context or successful result.'
        }
        $names = @($projection.projectionIdentities.PSObject.Properties | ForEach-Object Name | Sort-Object -CaseSensitive)
        $required = if ($projection.model -ceq 'editor-script') { @('Inno.ScriptApi.Editor', 'Inno.ScriptApi.Runtime') } else { @('Inno.ScriptApi.Runtime') }
        if (($names -join "`n") -cne ($required -join "`n")) { throw 'Projected API evidence has the wrong logical assemblies.' }
        foreach ($identity in $projection.projectionIdentities.PSObject.Properties) {
            if ($identity.Value -cnotmatch '^[a-f0-9]{64}$') { throw 'Projected API evidence has an invalid assembly identity.' }
            $consumers = @($report.results | Where-Object { $_.mode -ceq $projection.model -or ($projection.model -ceq 'runtime-script' -and $_.mode -ceq 'editor-script') })
            if ($consumers.Count -eq 0 -or @($consumers | Where-Object { $_.references.($identity.Name) -cne $identity.Value }).Count -ne 0) {
                throw 'Projected API evidence does not describe the assemblies used by the actual script compilation.'
            }
        }
    }
    [pscustomobject]@{
        check = $check.id
        catalogIdentity = (Get-FileHash -LiteralPath $catalogPath -Algorithm SHA256).Hash.ToLowerInvariant()
        reportIdentity = (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash.ToLowerInvariant()
        configuration = $report.configuration
        target = $report.target
        inputs = $proofs
        pages = @($pages)
        scriptReferences = $actualScriptReferences
    }
}
