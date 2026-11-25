$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
. "$PSScriptRoot/../../tools/ci/BackendReleaseCatalog.ps1"
$owner = [IO.Directory]::CreateTempSubdirectory('InnoBackendCatalog-').FullName
try {
    $archive = Join-Path $owner 'fixture.zip'
    $zip = [IO.Compression.ZipFile]::Open($archive, [IO.Compression.ZipArchiveMode]::Create)
    try {
        $files = @([ordered]@{path='managed/p/lib/P.dll'; owner='backends/P/P.csproj'; configuration='Debug'; input=('b' * 64)})
        foreach ($pair in @{'manifest.json'=@{version=1;target='fixture';files=$files}; 'native/selection.json'=@{assets=@();aggregates=@()}}.GetEnumerator()) {
            $writer = [IO.StreamWriter]::new($zip.CreateEntry($pair.Key).Open())
            try { $writer.Write((ConvertTo-Json $pair.Value -Depth 8 -Compress)) } finally { $writer.Dispose() }
        }
    } finally { $zip.Dispose() }
    $digest = (Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant()
    $assetName = 'fixture-prebuilt-backends.zip'
    $platform = Get-BackendCatalogPlatform $archive $assetName 'fixture' $digest
    if ($platform.configurations.Debug['backends/P/P.csproj'] -cne ('b' * 64)) { throw 'The release lost its certified compiler identity.' }
    $revision = 'a' * 40
    $tag = 'backend-prebuilts-2026.11.19.1'
    $encoded = ConvertTo-BackendTagMessage ([ordered]@{version=1;revision=$revision;platforms=[ordered]@{fixture=$platform}})
    $backendTag = @{tag=$tag; object=@{type='commit';sha=$revision}; message=$encoded}
    $release = @{tag_name=$tag;draft=$false;prerelease=$true;assets=@(@{name=$assetName;state='uploaded';digest=('sha256:'+$digest)})}
    function Invoke-RestMethod([string]$Uri, [hashtable]$Headers) {
        if ($Uri -ceq "fixture/git/ref/tags/$tag") { return @{object=@{type='tag';sha=('c'*40)}} }
        if ($Uri -ceq ('fixture/git/tags/' + ('c'*40))) { return $backendTag }
        throw "Unexpected request to $Uri"
    }
    $catalog = Get-BackendCatalog 'fixture' @{} $release
    if ($catalog.platforms.fixture.sha256 -cne $digest) { throw 'Platform catalog did not round-trip.' }
    function Reject([string]$Reason) {
        try { $null = Get-BackendCatalog 'fixture' @{} $release }
        catch { if ($_.Exception.Message.Contains($Reason)) { return }; throw }
        throw 'An inconsistent backend release was accepted.'
    }
    $backendTag.object.sha = 'd' * 40
    Reject 'source commit disagree'
    $backendTag.object.sha = $revision
    $release.assets[0].digest = 'sha256:' + ('a' * 64)
    Reject 'bytes disagree'
    $release.draft = $true
    Reject 'published versioned'
} finally {
    if ([IO.File]::Exists($archive)) { [IO.File]::Delete($archive) }
    [IO.Directory]::Delete($owner)
}
