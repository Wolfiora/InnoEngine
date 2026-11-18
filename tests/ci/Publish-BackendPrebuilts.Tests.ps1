$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/backend-publication', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$platforms = @(Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$revision = 'a'*40
$original = $env:GH_TOKEN
$server = @{release=$null;tag=$null;reference=$null;uploads=@{};interrupted=$false}
function Invoke-RestMethod($Method,$Uri,$Headers,$ContentType,$Body,$InFile) {
    if ($Uri -like '*/releases?per_page=*') { if ($server.release) { return $server.release }; return }
    if ($Uri -like '*/releases' -and $Method -ceq 'Post') {
        if ($server.release) { throw 'Created a duplicate release.' }
        $server.release = $Body | ConvertFrom-Json -AsHashtable
        $server.release.id = 7
        $server.release.upload_url = 'https://fixture.invalid/uploads'
        $server.release.assets = @()
        $server.release.published_at = '2026-11-19'
        return $server.release
    }
    if ($Uri -like 'https://fixture.invalid/uploads?name=*') {
        $name = $Uri.Split('=')[-1]
        if ($server.uploads.Count -eq 1 -and !$server.interrupted) {
            $server.interrupted = $true
            throw 'Interrupted test upload'
        }
        if ($server.uploads.ContainsKey($name)) { throw 'Uploaded the same asset twice.' }
        $asset = @{id=20+$server.uploads.Count;name=$name;state='uploaded';digest='sha256:'+(Get-FileHash -LiteralPath $InFile).Hash.ToLowerInvariant()}
        $server.uploads[$name] = $asset
        $server.release.assets += $asset
        return $asset
    }
    if ($Uri -like '*/releases/7') {
        if ($Method -ceq 'Patch') { $server.release.draft = $false }
        return $server.release
    }
    if ($Uri -like '*/git/ref/tags/*') {
        if ($server.reference) { return $server.reference }
        $response = [Net.Http.HttpResponseMessage]::new([Net.HttpStatusCode]::NotFound)
        throw [Microsoft.PowerShell.Commands.HttpResponseException]::new('Missing test tag',$response)
    }
    if ($Uri -like '*/git/tags' -and $Method -ceq 'Post') {
        $payload = $Body | ConvertFrom-Json
        $server.tag = @{tag=$payload.tag;object=@{type='commit';sha=$payload.object};message=$payload.message;sha='c'*40}
        return $server.tag
    }
    if ($Uri -like '*/git/refs' -and $Method -ceq 'Post') {
        $server.reference = @{object=@{type='tag';sha=$server.tag.sha}}
        return $server.reference
    }
    if ($Uri -like '*/git/tags/*') { return $server.tag }
    throw "Unexpected publication request $Method $Uri"
}
try {
    $env:GH_TOKEN = 'fixture'
    foreach ($platform in $platforms) {
        $directory = [IO.Directory]::CreateDirectory((Join-Path $root $platform.target)).FullName
        $archive = Join-Path $directory $platform.asset
        $zip = [IO.Compression.ZipFile]::Open($archive,[IO.Compression.ZipArchiveMode]::Create)
        try {
            $objects = @{ 'native/selection.json' = @{assets=@();aggregates=@()}
                'manifest.json' = @{version=1;target=$platform.target;files=@(@{path='managed/example.dll';owner='Example';input='b'*64;configuration='Debug'})}}
            foreach ($key in $objects.Keys) {
                $writer = [IO.StreamWriter]::new($zip.CreateEntry($key).Open())
                try { $writer.Write((ConvertTo-Json $objects[$key] -Depth 8)) } finally { $writer.Dispose() }
            }
        } finally { $zip.Dispose() }
        $plan = @{commit=$revision;tree='a'*64;environment='b'*64;full=$true;documentation=$null;nativeRequests=@(@{target=$platform.target})
            backendPackages=@(@{target=$platform.target;sha256=(Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant()})
            checks=@(@{id='consumer';kind='instructions';owner='engine';exclusion=$null;prerequisites=@()})}
        $plan | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $directory 'plan.json')
        @{commit=$revision;tree=$plan.tree;environment=$plan.environment;completed=$true
          planIdentity=(Get-FileHash (Join-Path $directory 'plan.json')).Hash.ToLowerInvariant()
          results=@(@{id='consumer';status='Passed';seconds=1.0;inputIdentity='d'*64;outputs=@{}})} |
            ConvertTo-Json -Depth 8 | Set-Content (Join-Path $directory 'results.json')
    }
    function Publish { & "$PSScriptRoot/Publish-BackendPrebuilts.ps1" -Directory $root -Repository 'fixture/engine' -Revision $revision -ApiUrl 'https://fixture.invalid' }
    try { Publish; throw 'The interrupted upload passed.' } catch { if ($_.Exception.Message -cne 'Interrupted test upload') { throw } }
    if (!$server.release.draft -or $server.tag -or $server.uploads.Count -ne 1) { throw 'A partial release became consumable.' }
    $result = Publish
    if ($result -notlike 'Published *' -or $server.release.draft -or !$server.reference -or $server.uploads.Count -ne $platforms.Count) { throw 'Recovery did not publish the complete target set.' }
    $result = Publish
    if ($result -notlike 'Reused *' -or $server.uploads.Count -ne $platforms.Count) { throw 'An identical retry changed release ownership.' }
} finally {
    $env:GH_TOKEN = $original
    if ([IO.Directory]::Exists($root)) { [IO.Directory]::Delete($root,$true) }
}
