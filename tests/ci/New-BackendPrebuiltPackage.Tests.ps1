param([string]$Dotnet = 'dotnet', [string]$EntryAssembly)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$root = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$boundary = [IO.Directory]::CreateDirectory((Join-Path $root 'artifacts/validation/backend-package-tests')).FullName
$owner = [IO.Directory]::CreateDirectory((Join-Path $boundary ([guid]::NewGuid().ToString('N')))).FullName
try {
    if (!$EntryAssembly) {
        $EntryAssembly = @(& "$PSScriptRoot/Initialize-ValidationEntry.ps1" -Dotnet (Get-Command $Dotnet).Source -LogPath "$owner/entry.log" -WorkspaceDirectory "$owner/bootstrap")[0]
    }
    $target = (Get-Content "$root/.github/native-targets.json" -Raw | ConvertFrom-Json)[0].target
    $stage = [IO.Directory]::CreateDirectory("$owner/staging").FullName
    $null = [IO.Directory]::CreateDirectory("$stage/receipts/variant")
    $null = [IO.Directory]::CreateDirectory("$stage/managed/tool/lib")
    $managed = "$stage/managed/tool/lib/tool.dll"
    [IO.File]::WriteAllText($managed, 'managed fixture payload')
    $source = 'backends/Fixture/Fixture.csproj'
    "$source|Debug|managed/tool/lib/tool.dll|$((Get-FileHash $managed).Hash.ToLowerInvariant())" | Set-Content "$stage/receipts/variant/files.txt"
    $plan = @{target=$target;configuration='Debug';projects=@{$source=('a' * 64)}}
    $plan | ConvertTo-Json -Depth 5 | Set-Content "$owner/plan.json"
    $component = "$owner/tool.zip"
    [IO.File]::WriteAllText($component, 'opaque native fixture payload')
    $digest = (Get-FileHash $component).Hash.ToLowerInvariant()
    $selection = @{assets=@(@{product='compiler';component='tool';target=$target;configuration='release';asset='tool.zip';sha256=$digest;fingerprint=('b' * 64)})
        aggregates=@(); requests=@(@{target=$target;product='compiler';configuration='Debug';digests=@($digest)})}
    $argsForPackage = @{Staging=$stage; InputPlans=@("$owner/plan.json"); NativeSelection="$owner/selection.json"; EntryAssembly=$EntryAssembly;
        Target=$target;OutputDirectory="$owner/output";Dotnet=$Dotnet}
    $selection | ConvertTo-Json -Depth 8 | Set-Content "$owner/selection.json"
    $archive = @(& "$PSScriptRoot/New-BackendPrebuiltPackage.ps1" @argsForPackage)[-1]
    $zip = [IO.Compression.ZipFile]::OpenRead($archive)
    try {
        if (!$zip.GetEntry("native/$digest.zip") -or !$zip.GetEntry('managed/tool/lib/tool.dll')) { throw 'The certified native or managed payload is missing.' }
    } finally { $zip.Dispose() }
    $selection.requests[0].digests += ('c' * 64)
    $selection | ConvertTo-Json -Depth 8 | Set-Content "$owner/selection.json"
    try { & "$PSScriptRoot/New-BackendPrebuiltPackage.ps1" @argsForPackage; throw 'A receipt with a missing component was accepted.' }
    catch { if ($_.Exception.Message -cne 'A certified native product references a missing payload.') { throw } }
} finally {
    if ([IO.Directory]::GetParent($owner).FullName -ne $boundary) { throw 'Unexpected fixture owner.' }
    Remove-Item -LiteralPath $owner -Recurse -Force
}
