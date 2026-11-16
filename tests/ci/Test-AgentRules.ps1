param(
    [string[]]$Paths = @(),
    [string[]]$Rules = @(),
    [string[]]$Behaviors = @(),
    [switch]$ShowInstructions,
    [int]$Stage = 0,
    [string]$DocsRoot
)
$ErrorActionPreference = 'Stop'
$OutputEncoding = [Text.UTF8Encoding]::new($false)
[Console]::OutputEncoding = $OutputEncoding
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$ruleRoot = Join-Path $repository '.agents/rulebook'
$manifest = Get-Content -LiteralPath (Join-Path $ruleRoot 'coverage.json') -Raw | ConvertFrom-Json
$rootText = Get-Content -LiteralPath (Join-Path $repository 'AGENTS.md') -Raw
$rootBytes = [Text.Encoding]::UTF8.GetByteCount($rootText)
if ($rootBytes -gt $manifest.rootBudgetBytes) { throw 'Root instructions exceed 8 KiB.' }
$sources = @{}
$sourceIdentities = @("$($manifest.baseline):AGENTS.md") + @($manifest.units | ForEach-Object source) +
    @($manifest.units | Where-Object { $_.approvedAddition.translation } | ForEach-Object { $_.approvedAddition.translation.source })
$sourceReader = [Diagnostics.Process]::new()
$sourceReader.StartInfo.FileName = 'git'
foreach ($argument in @('-C', $repository, 'cat-file', '--batch')) { $sourceReader.StartInfo.ArgumentList.Add($argument) }
$sourceReader.StartInfo.UseShellExecute = $false
$sourceReader.StartInfo.CreateNoWindow = $true
$sourceReader.StartInfo.RedirectStandardInput = $true
$sourceReader.StartInfo.RedirectStandardOutput = $true
$sourceReader.StartInfo.RedirectStandardError = $true
$sourceReader.StartInfo.StandardInputEncoding = [Text.UTF8Encoding]::new($false)
# Preparation owns acquisition. Reading rules never hydrates a partial clone over the network.
$sourceReader.StartInfo.Environment['GIT_NO_LAZY_FETCH'] = '1'
$sourceReader.StartInfo.Environment['GIT_TERMINAL_PROMPT'] = '0'
$sourceStarted = $false
$bytes = $null
try {
    $sourceStarted = $sourceReader.Start()
    if (-not $sourceStarted) { throw 'Cannot read immutable rule provenance.' }
    $sourceErrors = $sourceReader.StandardError.ReadToEndAsync()
    $bytes = [IO.BinaryReader]::new($sourceReader.StandardOutput.BaseStream)
    foreach ($identity in @($sourceIdentities | Sort-Object -Unique)) {
        if ($identity -cnotmatch '^[a-f0-9]{40}:[^\r\n\x00]+$') { throw "Invalid immutable rule source: $identity" }
        # Exchange one request and its size-delimited response at a time: neither pipe can fill
        # while its peer is waiting for the complete batch, even for a large original rulebook.
        $sourceReader.StandardInput.WriteLine($identity)
        $sourceReader.StandardInput.Flush()
        $header = [Text.StringBuilder]::new()
        while (($value = $bytes.ReadByte()) -ne 10) {
            if ($header.Length -ge 4096) { throw 'Invalid rule provenance response.' }
            [void]$header.Append([char]$value)
        }
        $match = [regex]::Match($header.ToString(), '^[a-f0-9]{40} blob ([0-9]+)$')
        if (-not $match.Success) { throw "Unavailable rule source: $identity" }
        $length = [long]::Parse($match.Groups[1].Value)
        if ($length -gt 16777216) { throw "Rule provenance exceeds the bounded source size: $identity" }
        $content = $bytes.ReadBytes([int]$length)
        if ($content.Length -ne $length -or $bytes.ReadByte() -ne 10) { throw "Incomplete rule source: $identity" }
        $sources[$identity] = [Text.UTF8Encoding]::new($false, $true).GetString($content).Replace("`r`n", "`n").TrimEnd([char]10)
    }
    $sourceReader.StandardInput.Close()
    $sourceReader.WaitForExit()
    $diagnostic = $sourceErrors.GetAwaiter().GetResult()
    if ($sourceReader.ExitCode -ne 0) { throw "Cannot read immutable rule provenance: $diagnostic" }
} finally {
    if ($sourceStarted -and -not $sourceReader.HasExited) { $sourceReader.Kill(); $sourceReader.WaitForExit() }
    if ($bytes) { $bytes.Dispose() }
    $sourceReader.Dispose()
}
function Read-Source([string]$identity) {
    if (-not $sources.ContainsKey($identity)) { throw "Unavailable rule source: $identity" }
    return $sources[$identity]
}
function Get-Hash([string]$value) {
    return [Convert]::ToHexStringLower([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($value)))
}
function Normalize-Links([string]$value) {
    # Relocated link destinations do not change their labels or surrounding requirements.
    return [regex]::Replace($value.Replace("`r`n", "`n").Trim(), '(\]\()[^)]+(\))', '$1<relocated>$2')
}
function Get-RuleLines([string]$value) {
    # Every nonblank line participates, including headings, examples and nested requirements.
    return @($value.Replace("`r`n", "`n").Trim() -split "`n" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
}
function Normalize-RuleLine([string]$value) {
    # Indentation expresses nested Markdown obligations and code examples; preserve it.
    return [regex]::Replace($value, '(\]\()[^)]+(\))', '$1<relocated>$2')
}
function Test-Translation($unit, [string]$source, [string]$target) {
    $translation = $unit.translation
    if ($translation.language -cne 'en' -or [string]::IsNullOrWhiteSpace($translation.approval) -or
        [string]::IsNullOrWhiteSpace($translation.review)) { throw "Incomplete translation review: $($unit.id)" }
    $reviewRoot = [IO.Path]::GetFullPath('.agents/records/', $repository)
    $reviewPath = [IO.Path]::GetFullPath(($translation.review -split '#')[0], $repository)
    if (-not $reviewPath.StartsWith($reviewRoot, [StringComparison]::Ordinal) -or
        -not (Test-Path -LiteralPath $reviewPath -PathType Leaf)) { throw "Missing translation review: $($unit.id)" }
    $sourceLines = @(Get-RuleLines $source)
    $targetLines = @(Get-RuleLines $target)
    if ($translation.lines.Count -ne $sourceLines.Count -or $targetLines.Count -ne $sourceLines.Count) {
        throw "Incomplete translation coverage: $($unit.id)"
    }
    for ($index = 0; $index -lt $sourceLines.Count; $index++) {
        $line = $translation.lines[$index]
        if ($line.ordinal -ne $index + 1 -or $line.sourceHash -cne (Get-Hash (Normalize-RuleLine $sourceLines[$index])) -or
            $line.targetHash -cne (Get-Hash (Normalize-RuleLine $targetLines[$index]))) {
            throw "Changed or unmapped translation clause: $($unit.id) line $($index + 1)"
        }
    }
}
$units = @{}; $bodies = @{}; $external = @{}
function Resolve-Authority($unit) {
    $ownerRoot = $repository
    if ($unit.PSObject.Properties.Name -contains 'repository') {
        if ($unit.repository -cne 'Wolfiora/InnoEngine.Docs') { throw "Unknown rule repository: $($unit.id)" }
        $external[$unit.id] = $true
        if ($DocsRoot) { $ownerRoot = [IO.Path]::GetFullPath($DocsRoot) }
    }
    $ownerRules = [IO.Path]::GetFullPath('.agents/rulebook/', $ownerRoot)
    $destination = [IO.Path]::GetFullPath($unit.path, $ownerRoot)
    if (-not $destination.StartsWith($ownerRules, [StringComparison]::Ordinal)) { throw "Rule path escapes owner: $($unit.id)" }
    if ($external.ContainsKey($unit.id) -and -not $DocsRoot) { return $null }
    $currentPath = $destination
    while ($currentPath -and $currentPath.Length -ge $ownerRoot.Length) {
        if (Test-Path -LiteralPath $currentPath) {
            if ((Get-Item -LiteralPath $currentPath -Force).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw "Rule authority cannot traverse a link: $($unit.id)" }
        }
        $currentPath = Split-Path $currentPath -Parent
    }
    return $destination
}
foreach ($unit in $manifest.units) {
    if ($units.ContainsKey($unit.id)) { throw "Duplicate rule unit: $($unit.id)" }
    $destination = Resolve-Authority $unit
    $source = Read-Source $unit.source
    if ($unit.PSObject.Properties.Name -contains 'sourceSection') { $source = [regex]::Split($source, '(?m)(?=^## )')[$unit.sourceSection] }
    if ((Get-Hash $source.Trim()) -cne $unit.sourceHash) { throw "Changed source identity: $($unit.id)" }
    foreach ($replacement in $manifest.pathReplacements) { $source = $source.Replace($replacement.old, $replacement.current) }
    if ($null -eq $destination) {
        $sourceLines = @(Get-RuleLines $source)
        if ($unit.translation.language -cne 'en' -or [string]::IsNullOrWhiteSpace($unit.translation.approval) -or
            $unit.translation.lines.Count -ne $sourceLines.Count) { throw "External rule source mapping is incomplete: $($unit.id)" }
        for ($index = 0; $index -lt $sourceLines.Count; $index++) {
            $line = $unit.translation.lines[$index]
            if ($line.ordinal -ne $index + 1 -or $line.sourceHash -cne (Get-Hash (Normalize-RuleLine $sourceLines[$index])) -or
                $line.targetHash -cnotmatch '^[a-f0-9]{64}$') { throw "Invalid external rule source clause: $($unit.id)" }
        }
        $units[$unit.id] = $unit
        continue
    }
    $text = Get-Content -LiteralPath $destination -Raw
    $pattern = '(?s)<!-- rule-unit: ' + [regex]::Escape($unit.id) + ' -->\s*<a id="[^"\r\n]+"></a>\s*(.*?)\s*<!-- /rule-unit: ' + [regex]::Escape($unit.id) + ' -->'
    $matches = [regex]::Matches($text, $pattern)
    if ($matches.Count -ne 1) { throw "Missing or repeated complete rule unit: $($unit.id)" }
    $body = $matches[0].Groups[1].Value
    $preservedBody = $body
    if ($unit.PSObject.Properties.Name -contains 'approvedAddition') {
        $parts = [regex]::Split($body, '(?m)^<!-- approved-addition -->\r?$')
        if ($parts.Count -ne 2 -or [string]::IsNullOrWhiteSpace($parts[1]) -or
            [string]::IsNullOrWhiteSpace($unit.approvedAddition.approval)) {
            throw "Invalid approved rule addition: $($unit.id)"
        }
        if ($unit.approvedAddition.PSObject.Properties.Name -contains 'translation') {
            $additionOriginal = Read-Source $unit.approvedAddition.translation.source
            $additionOriginal = [regex]::Match($additionOriginal, $pattern).Groups[1].Value
            $additionParts = [regex]::Split($additionOriginal, '(?m)^<!-- approved-addition -->\r?$')
            if ($additionParts.Count -ne 2 -or (Get-Hash $additionParts[1].Replace("`r`n", "`n").Trim()) -cne $unit.approvedAddition.hash) {
                throw "Invalid approved addition source: $($unit.id)"
            }
            Test-Translation @{ id = $unit.id + '/addition'; translation = $unit.approvedAddition.translation } $additionParts[1] $parts[1]
        } elseif ((Get-Hash $parts[1].Replace("`r`n", "`n").Trim()) -cne $unit.approvedAddition.hash) {
            throw "Invalid approved rule addition: $($unit.id)"
        }
        $preservedBody = $parts[0]
    }
    if ($unit.PSObject.Properties.Name -contains 'translation') {
        Test-Translation $unit $source $preservedBody
    } elseif ((Normalize-Links $preservedBody) -cne (Normalize-Links $source)) { throw "Lost nested requirements, examples or prose: $($unit.id)" }
    $units[$unit.id] = $unit; $bodies[$unit.id] = $body
}
$original = Read-Source "$($manifest.baseline):AGENTS.md"
if ($DocsRoot) {
    foreach ($path in @($manifest.units | Where-Object repository | ForEach-Object path | Sort-Object -Unique)) {
        foreach ($marker in [regex]::Matches([IO.File]::ReadAllText((Join-Path $DocsRoot $path)), '<!-- rule-unit: ([^ ]+) -->')) {
            $id = $marker.Groups[1].Value
            if (-not $external.ContainsKey($id) -or $units[$id].path -cne $path) { throw "Unregistered external rule authority: $id" }
        }
    }
}
foreach ($file in Get-ChildItem -LiteralPath $ruleRoot -Filter '*.md' -Recurse) {
    if ($file.Name -cne 'README.md' -and -not @($manifest.units | Where-Object { -not $_.repository -and [IO.Path]::GetFullPath($_.path, $repository) -ceq $file.FullName }).Count) {
        throw "Unregistered rule file: $($file.FullName)"
    }
    foreach ($marker in [regex]::Matches((Get-Content -LiteralPath $file.FullName -Raw), '<!-- rule-unit: ([^ ]+) -->')) {
        $id = $marker.Groups[1].Value
        if (-not $units.ContainsKey($id) -or $units[$id].repository -or [IO.Path]::GetFullPath($units[$id].path, $repository) -cne $file.FullName) {
            throw "Unregistered rule unit or authority: $id in $($file.FullName)"
        }
    }
}
$count = 0
foreach ($section in [regex]::Matches($original, '(?ms)^## (\d+)\. .*?(?=^## \d+\. |\z)')) {
    $number = [int]$section.Groups[1].Value; $ordinal = 0
    foreach ($bullet in [regex]::Matches($section.Value, '(?m)^- .*')) {
        $ordinal++; $id = "$number.$ordinal"
        $rows = @($manifest.rules | Where-Object id -CEQ $id)
        if ($rows.Count -ne 1) { throw "Missing or duplicate rule mapping: $id" }
        $row = $rows[0]
        if ($row.originalHash -cne (Get-Hash $bullet.Value) -or -not $row.check -or -not $row.reviewer) { throw "Invalid provenance or responsibility: $id" }
        if (-not $units.ContainsKey($row.unit) -or $units[$row.unit].path -cne $row.path -or $units[$row.unit].repository -cne $row.repository) { throw "Rule has no unique authority: $id" }
        $expected = $bullet.Value
        foreach ($replacement in $manifest.pathReplacements) { $expected = $expected.Replace($replacement.old, $replacement.current) }
        if ($row.disposition -eq 'preserved') {
            if ($units[$row.unit].PSObject.Properties.Name -contains 'translation') {
                $mapping = @($units[$row.unit].translation.lines | Where-Object sourceHash -CEQ (Get-Hash (Normalize-RuleLine $expected)))
                if ($mapping.Count -ne 1 -or $row.translatedHash -cne $mapping[0].targetHash) { throw "Rule translation was lost: $id" }
            } elseif (-not (Normalize-Links $bodies[$row.unit]).Contains((Normalize-Links $expected), [StringComparison]::Ordinal)) { throw "Rule was lost: $id" }
        } elseif ($number -ne 8 -or $row.disposition -ne 'superseded-by-approved-plan') { throw "Unapproved replacement: $id" }
        $count++
    }
}
if ($count -ne $manifest.rules.Count) { throw 'Unknown baseline rule mappings.' }
$selected = @{}
$active = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$checked = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
function Check-Dependencies([string]$id) {
    if (-not $units.ContainsKey($id)) { throw "Unknown rule: $id" }
    if ($checked.Contains($id)) { return }
    if (-not $active.Add($id)) { throw "Cyclic rule dependency: $id" }
    foreach ($dependency in $units[$id].requires) { Check-Dependencies $dependency }
    [void]$active.Remove($id); [void]$checked.Add($id)
}
foreach ($id in $units.Keys) { Check-Dependencies $id }
foreach ($route in $manifest.routes) {
    [void][regex]::new($route.pattern)
    foreach ($id in $route.units) {
        if (-not $units.ContainsKey($id)) { throw "Unknown routed rule: $id" }
    }
}
function Select-Unit([string]$id, [string]$reason) {
    if (-not $units.ContainsKey($id)) { throw "Unknown rule: $id" }
    if ($selected.ContainsKey($id)) { $selected[$id].Add($reason); return }
    $selected[$id] = [Collections.Generic.List[string]]::new(); $selected[$id].Add($reason)
    foreach ($dependency in $units[$id].requires) { Select-Unit $dependency "required by $id" }
}
foreach ($id in $Rules) { Select-Unit $id 'explicit rule' }
foreach ($tag in $Behaviors) {
    $matches = @($manifest.units | Where-Object { $tag -cin $_.tags })
    if ($matches.Count -eq 0) { throw "Unknown behavior: $tag" }
    foreach ($unit in $matches) { Select-Unit $unit.id "behavior: $tag" }
}
foreach ($path in $Paths) {
    $normalized = $path.Replace('\', '/')
    if ([IO.Path]::IsPathRooted($path)) { $normalized = [IO.Path]::GetRelativePath($repository, $path).Replace('\', '/') }
    foreach ($route in $manifest.routes) {
        if ($normalized -match $route.pattern) { foreach ($id in $route.units) { Select-Unit $id "path: $normalized" } }
    }
}
foreach ($file in @((Join-Path $repository 'AGENTS.md')) + @(Get-ChildItem -LiteralPath $ruleRoot -Filter '*.md' -Recurse | ForEach-Object FullName)) {
    foreach ($link in [regex]::Matches((Get-Content -LiteralPath $file -Raw), '\]\(([^)#]+)(?:#[^)]*)?\)')) {
        $target = $link.Groups[1].Value
        if ($target -match '^[a-z]+:') { continue }
        if (-not (Test-Path -LiteralPath ([IO.Path]::GetFullPath($target, (Split-Path $file))))) { throw "Broken instruction link: $file -> $target" }
    }
}
$stages = [Collections.Generic.List[object]]::new()
$current = [Collections.Generic.List[string]]::new(); $bytes = $rootBytes
foreach ($id in ($selected.Keys | Sort-Object)) {
    if (-not $bodies.ContainsKey($id)) { throw "Selected rule $id belongs to Wolfiora/InnoEngine.Docs. Supply -DocsRoot with the explicitly selected checkout; no network fetch or sibling-path guess is performed." }
    $size = [Text.Encoding]::UTF8.GetByteCount($bodies[$id])
    if ($size + $rootBytes -gt $manifest.taskBudgetBytes) { throw "Unit $id needs smaller complete subchapters; truncation is forbidden." }
    if ($bytes + $size -gt $manifest.taskBudgetBytes) {
        $stages.Add(@{ ids = $current.ToArray(); bytes = $bytes }); $current.Clear(); $bytes = $rootBytes
    }
    $current.Add($id); $bytes += $size
}
$stages.Add(@{ ids = $current.ToArray(); bytes = $bytes })
if ($Stage -lt 0 -or $Stage -gt $stages.Count) { throw 'Invalid instruction stage.' }
Write-Output "Preserved mapping: $count rules; $($units.Count) complete units. Root: $rootBytes bytes. Stages: $($stages.Count)."
if ($external.Count -gt 0) {
    if ($DocsRoot) { Write-Output "External authority verified: $($external.Count) complete units in $([IO.Path]::GetFullPath($DocsRoot))." }
    else { Write-Output "External authority not read: $($external.Count) source mappings only. Documentation work requires -DocsRoot; this result certifies engine-owned rule bodies only." }
}
for ($index = 0; $index -lt $stages.Count; $index++) {
    $entry = $stages[$index]
    Write-Output "Stage $($index + 1): $($entry.bytes) bytes; units: $($entry.ids -join ', ')"
    foreach ($id in $entry.ids) { Write-Output "$id | $($units[$id].repository)/$($units[$id].path)#$id | source: $($units[$id].source) | $($selected[$id] -join '; ')" }
    if ($ShowInstructions -and ($Stage -eq 0 -or $Stage -eq $index + 1)) {
        Write-Output $rootText
        foreach ($id in $entry.ids) { Write-Output $bodies[$id] }
    }
}
