param(
    [string[]]$Paths = @(),
    [string[]]$Rules = @(),
    [string[]]$Tags = @(),
    [string]$DocsRoot
)
$ErrorActionPreference = 'Stop'
$OutputEncoding = [Text.UTF8Encoding]::new($false)
[Console]::OutputEncoding = $OutputEncoding
$rootBudget = 8192
$fileBudget = 16384
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
# `pwsh -File` from another shell passes `a,b` as one value; accept both forms.
$Paths, $Rules, $Tags = @($Paths, $Rules, $Tags | ForEach-Object { , @($_ -split ',' | ForEach-Object Trim | Where-Object { $_ }) })

function ConvertTo-PathPattern([string]$glob, [string]$owner) {
    # Globs are repository-relative and match the whole path: `**/` spans directories, `*` stays inside one name.
    if ($glob -cnotmatch '^[^/\\][^\\]*$' -or $glob -cmatch '(^|/)\.\.?(/|$)') { throw "Rule path is not a repository-relative glob: $owner -> $glob" }
    $pattern = [regex]::Escape($glob).Replace('\*\*/', '(?:.*/)?').Replace('\*\*', '.*').Replace('\*', '[^/]*')
    return [regex]::new('^' + $pattern + '$')
}
function Read-Rules([string]$root, [string]$prefix) {
    $ruleRoot = Join-Path $root '.agents/rulebook'
    $index = Join-Path $ruleRoot 'README.md'
    foreach ($file in Get-ChildItem -LiteralPath $ruleRoot -Filter '*.md' -Recurse | Sort-Object FullName) {
        if ($file.FullName -ceq $index) { continue }
        $relative = [IO.Path]::GetRelativePath($ruleRoot, $file.FullName).Replace('\', '/')
        $id = $prefix + $relative.Substring(0, $relative.Length - 3)
        $text = (Get-Content -LiteralPath $file.FullName -Raw).Replace("`r`n", "`n")
        $header = [regex]::Match($text, '\A---\n((?:[a-z]+: \[[^\]\n]*\]\n)+)---\n')
        if (-not $header.Success) { throw "Rule file needs front matter with paths or tags: $id" }
        $fields = @{ paths = @(); tags = @() }
        foreach ($line in $header.Groups[1].Value.TrimEnd("`n") -split "`n") {
            $key, $list = $line -split ': ', 2
            if (-not $fields.ContainsKey($key) -or $fields[$key].Count) { throw "Unknown or repeated front matter field: $id -> $key" }
            $fields[$key] = @($list.Trim('[', ']') -split ',' | ForEach-Object { $_.Trim().Trim('"') })
            if (-not $fields[$key].Count -or $fields[$key] -contains '') { throw "Empty front matter value: $id -> $key" }
        }
        foreach ($tag in $fields.tags) { if ($tag -cnotmatch '^[a-z0-9-]+$') { throw "Invalid rule tag: $id -> $tag" } }
        $size = [Text.Encoding]::UTF8.GetByteCount($text)
        if ($size -gt $fileBudget) { throw "Rule file exceeds $fileBudget bytes; split it by topic: $id" }
        [pscustomobject]@{
            id = $id; root = $root; file = $file.FullName; bytes = $size; tags = $fields.tags
            patterns = @($fields.paths | ForEach-Object { ConvertTo-PathPattern $_ $id })
        }
    }
}

$rootBytes = [Text.Encoding]::UTF8.GetByteCount((Get-Content -LiteralPath (Join-Path $repository 'AGENTS.md') -Raw))
if ($rootBytes -gt $rootBudget) { throw "Root instructions exceed $rootBudget bytes." }
$catalog = @(Read-Rules $repository '')
$docs = $null
if ($DocsRoot) {
    $docs = (Resolve-Path -LiteralPath $DocsRoot).Path
    $catalog += @(Read-Rules $docs 'docs/')
}
function Select-ByPath([string]$path) {
    $full = [IO.Path]::GetFullPath($path, $repository)
    $owner = if ($docs -and $full.StartsWith($docs + [IO.Path]::DirectorySeparatorChar, [StringComparison]::Ordinal)) { $docs } else { $repository }
    $relative = [IO.Path]::GetRelativePath($owner, $full).Replace('\', '/')
    if ($relative -cmatch '^\.\.(/|$)') { throw "Path is outside the repository: $path" }
    foreach ($rule in $catalog) {
        if ($rule.root -ceq $owner -and @($rule.patterns | Where-Object { $_.IsMatch($relative) }).Count) { [pscustomobject]@{ rule = $rule; reason = "path: $relative" } }
    }
}

$selected = [ordered]@{}
function Add-Selection($rule, [string]$reason) {
    if (-not $selected.Contains($rule.id)) { $selected[$rule.id] = @{ rule = $rule; reasons = [Collections.Generic.List[string]]::new() } }
    $selected[$rule.id].reasons.Add($reason)
}
foreach ($id in $Rules) {
    $rule = @($catalog | Where-Object id -CEQ $id)
    if (-not $rule.Count) { throw "Unknown rule: $id" }
    Add-Selection $rule[0] 'explicit rule'
}
foreach ($tag in $Tags) {
    $tagged = @($catalog | Where-Object { $tag -cin $_.tags })
    if (-not $tagged.Count) { throw "Unknown tag: $tag" }
    foreach ($rule in $tagged) { Add-Selection $rule "tag: $tag" }
}
foreach ($path in $Paths) { foreach ($match in Select-ByPath $path) { Add-Selection $match.rule $match.reason } }

Write-Output "Rules are valid: $($catalog.Count) files. Root: $rootBytes bytes."
if ($selected.Count) {
    $total = ($selected.Values | ForEach-Object { $_.rule.bytes } | Measure-Object -Sum).Sum
    Write-Output "Read each file completely: $($selected.Count) files, $total bytes."
    foreach ($entry in $selected.Values) {
        $shown = if ($entry.rule.root -ceq $repository) { [IO.Path]::GetRelativePath($repository, $entry.rule.file).Replace('\', '/') } else { $entry.rule.file }
        Write-Output "$shown | $(@($entry.reasons | Select-Object -Unique) -join '; ')"
    }
}
