param([Parameter(Mandatory)][string] $Project)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$path = [IO.Path]::GetFullPath($Project)
[xml] $ide = [IO.File]::ReadAllText($path)
# Generated IDE files also contain Compile and None item groups. PowerShell's
# member enumeration fails on those groups under StrictMode; select XML nodes explicitly.
$references = @($ide.SelectNodes("/*[local-name()='Project']/*[local-name()='ItemGroup']/*[local-name()='Reference']"))
$result = @{}
foreach ($surface in @('Runtime', 'Editor')) {
    $selected = @($references | Where-Object { $_.GetAttribute('Include') -ceq "Inno.ScriptApi.$surface" })
    if ($selected.Count -ne 1) { throw "Actual script preparation must expose exactly one $surface logical reference." }
    $hints = @($selected[0].SelectNodes("*[local-name()='HintPath']"))
    if ($hints.Count -ne 1 -or [string]::IsNullOrWhiteSpace($hints[0].InnerText)) { throw "The $surface logical reference lacks its actual artifact path." }
    $artifact = [IO.Path]::GetFullPath($hints[0].InnerText, [IO.Path]::GetDirectoryName($path))
    if (![IO.File]::Exists($artifact)) { throw "The prepared $surface logical artifact is missing: $artifact" }
    $result[$surface.ToLowerInvariant()] = $artifact
}
[pscustomobject] $result
