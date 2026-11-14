param(
    [Parameter(Mandatory)][string]$RequirementsPath,
    [Parameter(Mandatory)][string]$ExpectedCommit
)

$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$request = Get-Content -LiteralPath $RequirementsPath -Raw | ConvertFrom-Json
if ($ExpectedCommit -cnotmatch '^[a-f0-9]{40}$' -or $request.status -cne 'Blocked' -or $request.kind -cne 'NativeSources' `
    -or $request.commit -cne $ExpectedCommit -or [IO.Path]::GetFullPath($request.root) -cne $repository) {
    throw 'Native preparation requires a blocked request for this exact workspace and commit.'
}
$pins = @($request.sources.PSObject.Properties)
if ($pins.Count -eq 0 -or @($request.owners).Count -eq 0) { throw 'Native preparation requires explicit source pins and affected owners.' }
$actual = (& git -C $repository rev-parse HEAD).Trim()
if ($LASTEXITCODE -ne 0 -or $actual -cne $ExpectedCommit) { throw 'Engine candidate changed before source preparation.' }
$dirty = @(& git -C $repository status --porcelain --untracked-files=all --ignore-submodules=none)
if ($LASTEXITCODE -ne 0 -or $dirty.Count -ne 0) { throw 'Hosted source preparation requires unchanged committed inputs.' }
$index = @(& git -C $repository ls-files --stage -z) -join "`n"
if ($LASTEXITCODE -ne 0) { throw 'Cannot read pinned source identities.' }
$gitlinks = @{}
foreach ($item in $index.Split([char]0, [StringSplitOptions]::RemoveEmptyEntries)) {
    $match = [regex]::Match($item, '^160000 ([a-f0-9]{40}) 0\t(.+)$')
    if ($match.Success) { $gitlinks.Add($match.Groups[2].Value, $match.Groups[1].Value) }
}
foreach ($pin in $pins) {
    if ($pin.Value -cnotmatch '^[a-f0-9]{40}$' -or $gitlinks[$pin.Name] -cne $pin.Value) { throw 'Requested source is not the current pinned gitlink: ' + $pin.Name }
    $path = [IO.Path]::GetFullPath($pin.Name, $repository)
    $relative = [IO.Path]::GetRelativePath($repository, $path)
    if ([IO.Path]::IsPathRooted($relative) -or $relative -eq '..' -or $relative.StartsWith('..' + [IO.Path]::DirectorySeparatorChar)) {
        throw 'Requested source escapes its workspace.'
    }
    $current = $repository
    foreach ($segment in $relative.Split([IO.Path]::DirectorySeparatorChar)) {
        $current = Join-Path $current $segment
        if ((Test-Path -LiteralPath $current) -and ((Get-Item -LiteralPath $current -Force).Attributes -band [IO.FileAttributes]::ReparsePoint)) {
            throw 'Requested source traverses a linked path.'
        }
    }
}
$paths = @($pins.Name | Sort-Object)
& git -C $repository submodule sync --recursive -- @paths | Write-Host
if ($LASTEXITCODE -ne 0) { throw 'Pinned source URL synchronization failed.' }
& git -C $repository -c protocol.version=2 submodule update --init --recursive --depth=1 -- @paths | Write-Host
if ($LASTEXITCODE -ne 0) { throw 'Required pinned source preparation failed; no executable plan can be reused.' }
foreach ($pin in $pins) {
    $actual = (& git -C (Join-Path $repository $pin.Name) rev-parse HEAD).Trim()
    if ($LASTEXITCODE -ne 0 -or $actual -cne $pin.Value) { throw 'Prepared source does not match its frozen request: ' + $pin.Name }
}
$actual = (& git -C $repository rev-parse HEAD).Trim()
$dirty = @(& git -C $repository status --porcelain --untracked-files=all --ignore-submodules=none)
if ($LASTEXITCODE -ne 0 -or $actual -cne $ExpectedCommit -or $dirty.Count -ne 0) { throw 'Engine inputs changed during source preparation.' }
Write-Host "Prepared $($pins.Count) pinned source checkouts. Generate a new plan for their available contents."
$global:LASTEXITCODE = 0
