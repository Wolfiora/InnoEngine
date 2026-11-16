param([Parameter(Mandatory)]$Inputs)

$ErrorActionPreference = 'Stop'
$map = $Inputs | ConvertTo-Json -Depth 4 | ConvertFrom-Json -AsHashtable
if ($map -isnot [Collections.IDictionary] -or $map.Count -eq 0) {
    throw 'Documentation input compatibility requires an explicit nonempty repository content map.'
}
foreach ($inputFile in $map.GetEnumerator()) {
    $path = [string]$inputFile.Key
    if ([string]::IsNullOrWhiteSpace($path) -or [IO.Path]::IsPathRooted($path) -or $path.Contains('\') -or $path.Contains(':') `
        -or $path -match '(^|/)\.{1,2}(/|$)' -or $path.Contains([char]0) `
        -or $inputFile.Value -cnotmatch '^(?:[a-f0-9]{40}|[a-f0-9]{64}|missing)$') {
        throw 'Documentation input compatibility contains an invalid repository path or content identity.'
    }
}
return $map
