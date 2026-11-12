param(
    [Parameter(Mandatory)]$Plan,
    [Parameter(Mandatory)]$Evidence,
    [Parameter(Mandatory)]$Pairing,
    [Parameter(Mandatory)][string]$Directory
)

$ErrorActionPreference = 'Stop'
$selected = @($Plan.checks | Where-Object { $_.kind -ceq 'documentation-examples' -and $null -eq $_.exclusion })
if ($selected.Count -eq 0) { return }
$directory = [IO.Path]::GetFullPath($Directory)
$catalogPath = Join-Path $directory 'documentation-catalog.json'
if ((Get-Item -LiteralPath $catalogPath).LinkType) { throw 'Example certification cannot follow a linked catalog.' }
$catalog = Get-Content -LiteralPath $catalogPath -Raw | ConvertFrom-Json
if ($catalog.snapshot -cne $Pairing.documentationSnapshot -or $catalog.sourceTree -cne $Pairing.engineTree `
    -or $catalog.contentIdentity -cne $Pairing.contentIdentity -or $catalog.repository -cne 'https://github.com/Wolfiora/InnoEngine') {
    throw 'Example certification catalog differs from its reconciled pair.'
}
foreach ($check in $selected) {
    if ($check.id -cnotmatch '^documentation-examples:[a-zA-Z0-9_-]+$') { throw 'Invalid example report identity.' }
    $passed = @($Evidence.results | Where-Object { $_.id -ceq $check.id -and $_.status -cin @('Passed', 'Reused') })
    if ($passed.Count -ne 1) { throw 'Required example execution did not pass exactly once.' }
    $file = Join-Path $directory ($check.id.Replace(':', '-') + '.json')
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
        [pscustomobject]@{ page = $result.page; mode = $result.mode; projects = @($page[0].examples.projects); blocks = $identities }
    }
    [pscustomobject]@{
        check = $check.id
        catalogIdentity = (Get-FileHash -LiteralPath $catalogPath -Algorithm SHA256).Hash.ToLowerInvariant()
        reportIdentity = (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash.ToLowerInvariant()
        configuration = $report.configuration
        target = $report.target
        pages = @($pages)
    }
}
