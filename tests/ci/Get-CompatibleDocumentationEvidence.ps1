param(
    [Parameter(Mandatory)][string]$PreviousCertificatePath,
    [Parameter(Mandatory)][string]$CertificationPath,
    [Parameter(Mandatory)][string[]]$PlanPaths,
    [Parameter(Mandatory)][string[]]$CatalogPaths,
    [Parameter(Mandatory)][string]$PreviousOrigin,
    [Parameter(Mandatory)][string]$PolicyIdentity
)

$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/DocumentationCertificateInputs.ps1"
if ((Get-Item -LiteralPath $PreviousCertificatePath).Length -gt 16MB) { throw 'Source certificate exceeds its normalized metadata boundary.' }
$previous = Get-Content -LiteralPath $PreviousCertificatePath -Raw | ConvertFrom-Json
$receipts = @(Get-Content -LiteralPath $CertificationPath -Raw | ConvertFrom-Json)
if ($previous.format -cne 'inno-documentation-source/2' -or $previous.repository -cne 'https://github.com/Wolfiora/InnoEngine' `
    -or $previous.sourceCommit -cnotmatch '^[a-f0-9]{40}$' -or $previous.sourceTree -cnotmatch '^[a-f0-9]{40}$' `
    -or $PlanPaths.Count -ne $receipts.Count -or $CatalogPaths.Count -ne $receipts.Count -or [string]::IsNullOrWhiteSpace($PreviousOrigin)) {
    throw 'Documentation compatibility requires a bound prior producer and every reconciled current plan.'
}
$identity = (Get-FileHash -LiteralPath $PreviousCertificatePath -Algorithm SHA256).Hash.ToLowerInvariant()
$api = [Collections.Generic.List[object]]::new()
$examples = [Collections.Generic.List[object]]::new()
$scripts = [Collections.Generic.List[object]]::new()
$diagnostics = [Collections.Generic.List[string]]::new()
if ($PolicyIdentity -cnotmatch '^[a-f0-9]{64}$') { throw 'Current documentation certificate policy has no content identity.' }
if ($previous.policyIdentity -cne $PolicyIdentity) {
    return [pscustomobject]@{ api = @(); examples = @(); diagnostics = @('Certificate checker inputs changed or prior policy provenance is unavailable.') }
}
function Test-SourceIdentity([string]$Expected, [string]$Current) {
    # Missing Native checkouts retain the Git index's exact immutable source pin.
    # This proves source compatibility, not Native preparation or execution on this runner.
    return $Expected -ceq $Current -or ($Expected -cmatch '^[a-f0-9]{40}$' -and $Current -ceq ('unavailable:' + $Expected))
}
function Test-Inputs($Proof, $Plan) {
    if ($Proof.environment -cne $Plan.environment -or $Proof.configuration -cne $Plan.configuration `
        -or $Proof.sourceIdentity -cnotmatch '^[a-f0-9]{64}$' -or @($Proof.owners).Count -eq 0 -or $null -eq $Proof.sources) {
        $diagnostics.Add("$($row.platform): source proof lacks matching environment, configuration or input metadata.")
        return $false
    }
    $sources = & "$PSScriptRoot/Get-DocumentationSourceInputs.ps1" -Inputs $Proof.sources
    foreach ($owner in @($Proof.owners)) {
        if ($owner -cin @($Plan.owners) -or -not $Plan.inputSets.Contains($owner)) {
            $diagnostics.Add("$($row.platform): changed, missing or conservatively selected owner $owner.")
            return $false
        }
    }
    foreach ($inputFile in $sources.GetEnumerator()) {
        if (-not $Plan.inputs.Contains($inputFile.Key) -or -not (Test-SourceIdentity $inputFile.Value $Plan.inputs[$inputFile.Key])) {
            $diagnostics.Add("$($row.platform): source input changed: $($inputFile.Key).")
            return $false
        }
    }
    # Current evaluated inputs also detect added imports, source files and tool dependencies.
    # Retained native-context inputs are compared above even when this bounded plan needs no native context.
    foreach ($owner in @($Proof.owners) + @('tooling')) {
        foreach ($path in @($Plan.inputSets[$owner])) {
            if (-not $sources.Contains($path) -or -not (Test-SourceIdentity $sources[$path] $Plan.inputs[$path])) {
                $diagnostics.Add("$($row.platform): added or changed evaluated input: $path.")
                return $false
            }
        }
    }
    return $true
}

for ($index = 0; $index -lt $receipts.Count; $index++) {
    $row = $receipts[$index]
    $plan = Get-Content -LiteralPath $PlanPaths[$index] -Raw | ConvertFrom-Json -AsHashtable
    $catalog = Get-Content -LiteralPath $CatalogPaths[$index] -Raw | ConvertFrom-Json
    if ((Get-FileHash -LiteralPath $PlanPaths[$index] -Algorithm SHA256).Hash.ToLowerInvariant() -cne $row.receipt.planIdentity `
        -or $plan.commit -cne $row.receipt.commit -or $plan.environment -cne $row.receipt.environment `
        -or $catalog.sourceTree -cne $row.receipt.documentation.engineTree `
        -or $catalog.snapshot -cne $row.receipt.documentation.documentationSnapshot) {
        throw 'Documentation compatibility plan or catalog differs from reconciled execution.'
    }
    if ($plan.baseline -cne $previous.sourceCommit) {
        $diagnostics.Add("$($row.platform): prior source commit is not this plan's exact impact baseline.")
        continue
    }
    $binding = [ordered]@{ method = 'UnchangedDeclaredInputs'; origin = $PreviousOrigin; certificateIdentity = $identity
        sourceCommit = $previous.sourceCommit; sourceTree = $previous.sourceTree; candidateCommit = $plan.commit; planIdentity = $row.receipt.planIdentity }
    foreach ($proof in @($previous.inputs | Where-Object { $_.platform -ceq $row.platform })) {
        $proof = Get-DocumentationInputProof $previous $proof
        if (-not (Test-Inputs $proof $plan)) { continue }
        foreach ($reference in @($catalog.references | Where-Object { $_.model -ceq 'source-clr' -and $proof.project -cin $_.projects `
            -and $_.configuration -ceq $proof.configuration -and $_.target -ceq $proof.target })) {
            $key = $reference.path + "`0" + $proof.project
            if ($null -eq $plan.documentationSources -or $plan.documentationSources[$key] -cne $proof.referenceIdentity) {
                $diagnostics.Add("$($row.platform): source facts changed or are unavailable for $($proof.project).")
                continue
            }
            $api.Add([pscustomobject]@{ reference = $reference.path; proof = $proof; compatibility = $binding })
        }
    }
    $groupChecks = @{}
    $groupInputs = @{}
    function Test-Group($context) {
        if ($null -eq $context.inputGroup) { throw 'Certificate context lacks its input group.' }
        $key = $context.inputGroup
        if (-not $groupChecks.ContainsKey($key)) {
            $proofs = @(Get-DocumentationInputGroup $previous $context)
            $groupInputs[$key] = $proofs
            $groupChecks[$key] = @($proofs | Where-Object { -not (Test-Inputs $_ $plan) }).Count -eq 0
        }
        $group = $previous.inputGroups[$key]
        if ($group.platform -cne $context.platform -or $group.configuration -cne $context.configuration `
            -or $group.target -cne $context.target -or $group.reportIdentity -cne $context.reportIdentity) {
            throw 'Certificate input group belongs to another execution context.'
        }
        $groupChecks[$key]
    }
    foreach ($example in @($previous.examples)) {
        $page = @($catalog.pages | Where-Object id -CEQ $example.page)
        if ($page.Count -ne 1 -or $null -eq $page[0].examples -or $page[0].examples.mode -cne $example.mode `
            -or (@($page[0].examples.projects) -join "`n") -cne (@($example.projects) -join "`n")) { continue }
        $blocks = @($page[0].codeBlocks | Where-Object { $_.language -cin @('cs', 'csharp') -and @($_.declarationSymbols).Count -eq 0 } | ForEach-Object contentIdentity)
        if ($blocks.Count -eq 0 -or ($blocks -join "`n") -cne (@($example.blocks) -join "`n")) { continue }
        foreach ($context in @($example.contexts | Where-Object platform -CEQ $row.platform)) {
            if (-not (Test-Group $context)) { continue }
            $examples.Add([pscustomobject]@{ page = $example.page; mode = $example.mode; projects = @($example.projects); blocks = $blocks
                context = $context; inputs = $groupInputs[$context.inputGroup]; compatibility = $binding })
        }
    }
    foreach ($projection in @($previous.scriptReferences | Where-Object { $null -ne $_ -and $_.platform -ceq $row.platform })) {
        $bound = @($catalog.references | Where-Object { $_.path -ceq $projection.path -and $_.identity -ceq $projection.identity `
            -and $_.model -ceq $projection.model -and $_.model -cin @('runtime-script', 'editor-script') `
            -and $_.configuration -ceq $projection.configuration -and $null -eq $_.target })
        if ($bound.Count -ne 1 -or (@($projection.projects) -join "`n") -cne (@($bound[0].projects) -join "`n") `
            -or $projection.reportIdentity -cnotmatch '^[a-f0-9]{64}$' -or $projection.documentationIdentity -cnotmatch '^[a-f0-9]{64}$' `
            -or -not (Test-Group $projection)) { continue }
        $names = @($projection.projectionIdentities.PSObject.Properties | ForEach-Object Name | Sort-Object -CaseSensitive)
        $required = if ($projection.model -ceq 'editor-script') { @('Inno.ScriptApi.Editor', 'Inno.ScriptApi.Runtime') } else { @('Inno.ScriptApi.Runtime') }
        if (($names -join "`n") -cne ($required -join "`n") `
            -or @($projection.projectionIdentities.PSObject.Properties | Where-Object { $_.Value -cnotmatch '^[a-f0-9]{64}$' }).Count -ne 0) { continue }
        $scripts.Add([pscustomobject]@{ proof = $projection; inputs = $groupInputs[$projection.inputGroup]; compatibility = $binding })
    }
}
[pscustomobject]@{ api = $api.ToArray(); examples = $examples.ToArray(); scripts = $scripts.ToArray(); diagnostics = $diagnostics.ToArray() }
