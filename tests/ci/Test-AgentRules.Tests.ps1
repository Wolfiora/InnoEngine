param([string]$DocsRoot)
$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$entry = Join-Path $PSScriptRoot 'Test-AgentRules.ps1'
$root = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion) -join "`n"
Push-Location (Join-Path $repository 'tools')
try { $nested = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion) -join "`n" }
finally { Pop-Location }
if ($root -cne $nested) { throw 'Root and nested invocation selected different instruction units.' }
if ($root -notmatch 'Preserved mapping: 241 rules') { throw 'Original rule coverage is incomplete.' }
foreach ($path in @('Directory.Build.targets', 'Directory.Solution.targets')) {
    $selection = @(& $entry -Paths $path) -join "`n"
    foreach ($required in @('build-host', 'platform-products', 'backend-build', 'solution')) {
        if (-not $selection.Contains($required)) { throw "Shared build input $path did not route to $required." }
    }
}
foreach ($scenario in @(@{ Rules = 'unknown-unit' }, @{ Behaviors = 'unknown-behavior' }, @{ Stage = 999 })) {
    $rejected = $false
    try { & $entry @scenario | Out-Null } catch { $rejected = $true }
    if (-not $rejected) { throw 'Invalid rule selection was accepted.' }
}
$manifest = Get-Content -LiteralPath (Join-Path $repository '.agents/rulebook/coverage.json') -Raw | ConvertFrom-Json
foreach ($unit in $manifest.units) {
    if (-not $unit.sourceHash -or -not $unit.check -or -not $unit.reviewer) { throw "Incomplete rule responsibility: $($unit.id)" }
}
$all = @(& $entry -Rules @($manifest.units | Where-Object { -not $_.repository } | ForEach-Object id)) -join "`n"
foreach ($match in [regex]::Matches($all, 'Stage \d+: (\d+) bytes')) {
    if ([int]$match.Groups[1].Value -gt 16384) { throw 'A staged instruction set exceeds its complete-read budget.' }
}
$first = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion -Stage 1 -ShowInstructions) -join "`n"
if (-not $first.Contains('# InnoEngine agent instructions') -or -not $first.Contains('Define clear assembly/domain boundaries')) { throw 'Actual instruction bodies were not emitted.' }
$fixtureParent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$fixture = Join-Path $fixtureParent ('InnoRuleTests-' + [guid]::NewGuid().ToString('N'))
try {
    & git clone --quiet --shared --no-checkout $repository $fixture
    if ($LASTEXITCODE -ne 0) { throw 'Cannot create the isolated rule provenance fixture.' }
    # A clone of a shallow CI checkout does not inherit separately fetched provenance objects.
    # Acquire only missing original revisions from the explicit local owner, never the network.
    $provenance = @("$($manifest.baseline):AGENTS.md") + @($manifest.units | ForEach-Object source) +
        @($manifest.units | Where-Object { $_.approvedAddition.translation } | ForEach-Object { $_.approvedAddition.translation.source })
    foreach ($source in @($provenance | Sort-Object -Unique)) {
        & git -C $fixture cat-file -e $source 2>$null
        if ($LASTEXITCODE -eq 0) { continue }
        $revision = ($source -split ':', 2)[0]
        if ($revision -cnotmatch '^[a-f0-9]{40}$') { throw 'The rule fixture requires immutable local provenance.' }
        & git -C $fixture fetch --quiet --no-tags --depth=1 $repository $revision
        if ($LASTEXITCODE -ne 0) { throw 'Cannot prepare original rule provenance from the local checkout.' }
        & git -C $fixture cat-file -e $source 2>$null
        if ($LASTEXITCODE -ne 0) { throw "Original rule provenance is unavailable in the fixture: $source" }
    }
    [void][IO.Directory]::CreateDirectory((Join-Path $fixture 'tests/ci'))
    [void][IO.Directory]::CreateDirectory((Join-Path $fixture '.agents'))
    Copy-Item -LiteralPath (Join-Path $repository '.agents/rulebook') -Destination (Join-Path $fixture '.agents/rulebook') -Recurse
    Copy-Item -LiteralPath (Join-Path $repository 'AGENTS.md') -Destination $fixture
    Copy-Item -LiteralPath $entry -Destination (Join-Path $fixture 'tests/ci/Test-AgentRules.ps1')
    foreach ($instruction in @((Join-Path $repository 'AGENTS.md')) + @(Get-ChildItem (Join-Path $repository '.agents/rulebook') -Recurse -Filter '*.md' | ForEach-Object FullName)) {
        foreach ($link in [regex]::Matches([IO.File]::ReadAllText($instruction), '\]\(([^)#]+)(?:#[^)]*)?\)')) {
            $relative = $link.Groups[1].Value
            if ($relative -match '^[a-z]+:') { continue }
            $sourceFile = [IO.Path]::GetFullPath($relative, (Split-Path $instruction))
            $target = Join-Path $fixture ([IO.Path]::GetRelativePath($repository, $sourceFile))
            if (-not [IO.Path]::GetFullPath($target).StartsWith($fixture + [IO.Path]::DirectorySeparatorChar, [StringComparison]::Ordinal)) {
                throw 'Instruction fixture link escaped its isolated owner.'
            }
            [void][IO.Directory]::CreateDirectory((Split-Path $target))
            Copy-Item -LiteralPath $sourceFile -Destination $target
        }
    }
    foreach ($review in @($manifest.units | Where-Object translation | ForEach-Object { ($_.translation.review -split '#')[0] } | Sort-Object -Unique)) {
        $reviewTarget = Join-Path $fixture $review
        [void][IO.Directory]::CreateDirectory((Split-Path $reviewTarget))
        Copy-Item -LiteralPath (Join-Path $repository $review) -Destination $reviewTarget
    }
    $fixtureEntry = Join-Path $fixture 'tests/ci/Test-AgentRules.ps1'
    $registry = Join-Path $fixture '.agents/rulebook/coverage.json'
    $originalRegistry = [IO.File]::ReadAllText($registry)
    # Use one real preserved engine unit as an isolated external-authority fixture. No network,
    # sibling checkout or copied current Docs tree is needed by engine-only regression execution.
    $authorityRoot = Join-Path $fixture 'external-authority'
    $authorityRules = Join-Path $authorityRoot '.agents/rulebook'
    [void][IO.Directory]::CreateDirectory($authorityRules)
    $externalCopy = $originalRegistry | ConvertFrom-Json
    $externalUnit = $externalCopy.units | Where-Object id -CEQ 'style'
    $localAuthority = Join-Path $fixture $externalUnit.path
    $localText = [IO.File]::ReadAllText($localAuthority)
    $unitPattern = '(?s)<!-- rule-unit: style -->.*?<!-- /rule-unit: style -->'
    $externalText = [regex]::Match($localText, $unitPattern).Value
    $externalUnit | Add-Member repository 'Wolfiora/InnoEngine.Docs'
    $externalUnit.path = '.agents/rulebook/imported.md'
    foreach ($row in $externalCopy.rules | Where-Object unit -CEQ 'style') {
        $row | Add-Member repository 'Wolfiora/InnoEngine.Docs'
        $row.path = $externalUnit.path
    }
    $externalPath = Join-Path $authorityRoot $externalUnit.path
    [IO.File]::WriteAllText($externalPath, $externalText)
    [IO.File]::WriteAllText($localAuthority, [regex]::Replace($localText, $unitPattern, ''))
    [IO.File]::WriteAllText($registry, ($externalCopy | ConvertTo-Json -Depth 50))
    # Other migrated units are not part of this single-authority fixture; selecting a missing
    # authority must fail before any complete instruction output can be claimed.
    $failure = ''
    try { & $fixtureEntry -Rules style | Out-Null } catch { $failure = $_.Exception.Message }
    if (-not $failure.Contains('Supply -DocsRoot')) { throw "Missing explicit external rule authority was accepted: $failure" }
    foreach ($scenario in @('unknown-repository', 'escaped-authority')) {
        $originalPath = $externalUnit.path
        if ($scenario -eq 'unknown-repository') { $externalUnit.repository = 'Other/Rules' }
        else { $externalUnit.path = '../../escaped.md' }
        [IO.File]::WriteAllText($registry, ($externalCopy | ConvertTo-Json -Depth 50))
        $failure = ''
        try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
        $expected = if ($scenario -eq 'unknown-repository') { 'Unknown rule repository' } else { 'Rule path escapes owner' }
        if (-not $failure.Contains($expected)) { throw "Wrong external authority rejection for ${scenario}: $failure" }
        $externalUnit.repository = 'Wolfiora/InnoEngine.Docs'
        $externalUnit.path = $originalPath
    }
    [IO.File]::WriteAllText($registry, $originalRegistry)
    [IO.File]::WriteAllText($localAuthority, $localText)
    if ($DocsRoot) {
        foreach ($path in @($manifest.units | Where-Object repository | ForEach-Object path | Sort-Object -Unique)) {
            $target = Join-Path $authorityRoot $path
            [void][IO.Directory]::CreateDirectory((Split-Path $target))
            Copy-Item -LiteralPath (Join-Path $DocsRoot $path) -Destination $target
        }
        $complete = @(& $fixtureEntry -DocsRoot $authorityRoot -Rules wiki -ShowInstructions) -join "`n"
        if (-not $complete.Contains('External authority verified: 8 complete units') -or -not $complete.Contains('Canonical source and example verification')) {
            throw 'Explicit external authority did not load complete current requirements.'
        }
        Push-Location (Join-Path $fixture 'tests')
        try { $nestedExternal = @(& $fixtureEntry -DocsRoot $authorityRoot -Rules wiki -ShowInstructions) -join "`n" }
        finally { Pop-Location }
        if ($complete -cne $nestedExternal) { throw 'Nested invocation changed external authority or rule bodies.' }
        $authority = Join-Path $authorityRoot '.agents/rulebook/documentation.md'
        $originalAuthority = [IO.File]::ReadAllText($authority)
        [IO.File]::WriteAllText($authority, $originalAuthority.Replace('## API reference and source', '## Weakened contract'))
        $failure = ''
        try { & $fixtureEntry -DocsRoot $authorityRoot | Out-Null } catch { $failure = $_.Exception.Message }
        if (-not $failure.Contains('Changed or unmapped translation clause')) { throw 'Changed external rule body passed provenance validation.' }
        [IO.File]::WriteAllText($authority, $originalAuthority)
        [IO.File]::WriteAllText($authority, $originalAuthority + "`n<!-- rule-unit: unknown-external -->`n")
        $failure = ''
        try { & $fixtureEntry -DocsRoot $authorityRoot | Out-Null } catch { $failure = $_.Exception.Message }
        if (-not $failure.Contains('Unregistered external rule authority')) { throw 'Unregistered external rule authority passed validation.' }
        [IO.File]::WriteAllText($authority, $originalAuthority)
    }
    foreach ($scenario in @('cycle', 'missing-section', 'unregistered-section')) {
        [IO.File]::WriteAllText($registry, $originalRegistry)
        $copy = $originalRegistry | ConvertFrom-Json
        $unitPath = Join-Path $fixture $copy.units[0].path
        $originalUnit = [IO.File]::ReadAllText($unitPath)
        $expected = switch ($scenario) {
            'cycle' {
                $copy.units[0].requires = @($copy.units[0].id)
                [IO.File]::WriteAllText($registry, ($copy | ConvertTo-Json -Depth 50))
                'Cyclic rule dependency'
            }
            'missing-section' {
                [IO.File]::WriteAllText($unitPath, $originalUnit.Replace('<!-- rule-unit: ' + $copy.units[0].id + ' -->', '<!-- missing -->'))
                'Missing or repeated complete rule unit'
            }
            'unregistered-section' {
                [IO.File]::WriteAllText($unitPath, $originalUnit + "`n<!-- rule-unit: unregistered -->`n")
                'Unregistered rule unit'
            }
        }
        $failure = ''
        try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
        [IO.File]::WriteAllText($unitPath, $originalUnit)
        if (-not $failure.Contains($expected)) { throw "Wrong rejection for $scenario : $failure" }
    }
    [IO.File]::WriteAllText($registry, $originalRegistry)
    $translationUnit = ($originalRegistry | ConvertFrom-Json).units | Where-Object id -CEQ 'delivery'
    $translationPath = Join-Path $fixture $translationUnit.path
    $originalTranslation = [IO.File]::ReadAllText($translationPath)
    foreach ($scenario in @('changed-translation', 'missing-clause', 'changed-source-map', 'missing-review', 'escaped-review', 'lost-original-rule')) {
        $copy = $originalRegistry | ConvertFrom-Json
        $translated = $copy.units | Where-Object id -CEQ 'delivery'
        [IO.File]::WriteAllText($translationPath, $originalTranslation)
        $expected = switch ($scenario) {
            'changed-translation' {
                [IO.File]::WriteAllText($translationPath, $originalTranslation.Replace('must be entirely in English', 'may be in any language'))
                'Changed or unmapped translation clause'
            }
            'missing-clause' {
                $translated.translation.lines = @($translated.translation.lines | Select-Object -Skip 1)
                'Incomplete translation coverage'
            }
            'changed-source-map' {
                $translated.translation.lines[0].sourceHash = 'invalid'
                'Changed or unmapped translation clause'
            }
            'missing-review' {
                $translated.translation.review = '.agents/records/missing.md'
                'Missing translation review'
            }
            'escaped-review' {
                $translated.translation.review = '.agents/records/../../AGENTS.md'
                'Missing translation review'
            }
            'lost-original-rule' {
                ($copy.rules | Where-Object id -CEQ '31.1').translatedHash = 'invalid'
                'Rule translation was lost'
            }
        }
        [IO.File]::WriteAllText($registry, ($copy | ConvertTo-Json -Depth 50))
        $failure = ''
        try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
        if (-not $failure.Contains($expected)) { throw "Wrong rejection for $scenario : $failure" }
    }
    [IO.File]::WriteAllText($translationPath, $originalTranslation)
    [IO.File]::WriteAllText($registry, $originalRegistry)
    $nestedRulePath = Join-Path $fixture '.agents/rulebook/coding.md'
    $nestedRuleText = [IO.File]::ReadAllText($nestedRulePath)
    [IO.File]::WriteAllText($nestedRulePath, $nestedRuleText.Replace('  - Types and members', '- Types and members'))
    $failure = ''
    try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
    [IO.File]::WriteAllText($nestedRulePath, $nestedRuleText)
    if (-not $failure.Contains('Changed or unmapped translation clause')) { throw 'Nested rule indentation lost its provenance protection.' }
    $additionUnit = ($originalRegistry | ConvertFrom-Json).units | Where-Object id -CEQ 'csharp-standard-5'
    $additionPath = Join-Path $fixture $additionUnit.path
    $originalAddition = [IO.File]::ReadAllText($additionPath)
    $loaded = @(& $entry -Rules $additionUnit.id -ShowInstructions) -join "`n"
    if (-not $loaded.Contains('Handwritten C# must not use `global::`')) { throw 'Approved addition was not loaded.' }
    foreach ($scenario in @('changed-addition', 'missing-addition', 'missing-approval', 'changed-baseline')) {
        [IO.File]::WriteAllText($registry, $originalRegistry)
        [IO.File]::WriteAllText($additionPath, $originalAddition)
        $expected = 'Invalid approved rule addition'
        switch ($scenario) {
            'changed-addition' {
                [IO.File]::WriteAllText($additionPath, $originalAddition.Replace('Handwritten C# must not use', 'Handwritten C# may use'))
                $expected = 'Changed or unmapped translation clause'
            }
            'missing-addition' {
                [IO.File]::WriteAllText($additionPath, $originalAddition.Replace('<!-- approved-addition -->', ''))
            }
            'missing-approval' {
                $copy = $originalRegistry | ConvertFrom-Json
                ($copy.units | Where-Object id -CEQ $additionUnit.id).approvedAddition.approval = ''
                [IO.File]::WriteAllText($registry, ($copy | ConvertTo-Json -Depth 50))
            }
            'changed-baseline' {
                [IO.File]::WriteAllText($additionPath, $originalAddition.Replace('- Explicitly declare ordinary using directives in every file.', ''))
                $expected = 'Incomplete translation coverage'
            }
        }
        $failure = ''
        try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
        if (-not $failure.Contains($expected)) { throw "Wrong rejection for $scenario : $failure" }
    }
} finally {
    $resolved = [IO.Path]::GetFullPath($fixture)
    if (-not $resolved.StartsWith($fixtureParent, [StringComparison]::Ordinal) -or (Split-Path $resolved -Leaf) -notmatch '^InnoRuleTests-[a-f0-9]{32}$') {
        throw 'Rule fixture cleanup escaped its unique owner.'
    }
    if (Test-Path -LiteralPath $resolved) { Remove-Item -LiteralPath $resolved -Recurse -Force }
}
Write-Output 'Agent rule coverage, actual loading, root/nested routing, staged budgets, invalid selections, missing units, cyclic dependencies, approved additions and English translation provenance passed.'
