$ErrorActionPreference = 'Stop'
$root = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/backend-publication', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$platforms = @(Get-Content -LiteralPath (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$revision = 'a'*40
$original = $env:GH_TOKEN
$retiredTag = 'backend-prebuilts-2020.01.01.1'
$retiredObject = 'f'*40
$server = @{release=$null;tag=$null;reference=$null;uploads=@{};interrupted=$false;denyDraft=$false;wrongDigest=$false;recoveredPlaceholder=$false;changedPlaceholder=$false}
function Invoke-RestMethod($Method,$Uri,$Headers,$ContentType,$Body,$InFile) {
    if ($Uri -like '*/actions/runs?status=*') { return @{total_count=0;workflow_runs=@()} }
    if ($Uri.EndsWith('/releases/3')) {
        throw [Microsoft.PowerShell.Commands.HttpResponseException]::new('Retired release',[Net.Http.HttpResponseMessage]::new([Net.HttpStatusCode]::NotFound))
    }
    if ($Uri.EndsWith("/git/ref/tags/$retiredTag")) { return $server.retiredRef }
    if ($Uri.EndsWith("/git/tags/$retiredObject")) { return @{tag=$retiredTag;object=$server.tag.object;message=$server.tag.message} }
    if ($Uri.EndsWith("/git/refs/tags/$retiredTag") -and $Method -ceq 'Delete') { $server.retiredRef = $null; return }
    if ($Uri -like '*/releases?per_page=*') { if ($server.release) { return $server.release }; return }
    if ($Uri -like '*/releases' -and $Method -ceq 'Post') {
        if ($server.denyDraft) { throw 'Release write permission denied.' }
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
        if ($server.wrongDigest) { return @{id=99;name=$name;state='uploaded';digest='sha256:'+('0'*64)} }
        if ($server.uploads.Count -eq 1 -and !$server.interrupted) {
            $server.interrupted = $true
            $server.release.assets += @{id=98;name=$name;state='starter';size=0;digest=$null}
            throw 'Interrupted test upload'
        }
        if ($server.uploads.ContainsKey($name)) { throw 'Uploaded the same asset twice.' }
        $asset = @{id=20+$server.uploads.Count;name=$name;state='uploaded';digest='sha256:'+(Get-FileHash -LiteralPath $InFile).Hash.ToLowerInvariant()}
        $server.uploads[$name] = $asset
        $server.release.assets += $asset
        return $asset
    }
    if ($Uri.EndsWith('/releases/assets/98') -and $Method -ceq 'Delete') {
        $server.release.assets = @($server.release.assets | Where-Object id -NE 98)
        $server.recoveredPlaceholder = $true
        return
    }
    if ($Uri -like '*/releases/7') {
        if ($server.changedPlaceholder) {
            $current = $server.release | ConvertTo-Json -Depth 5 | ConvertFrom-Json -AsHashtable
            ($current.assets | Where-Object state -CEQ 'starter').id = 99
            return $current
        }
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
        $graph = @(@{Path='${workspace}/src/Player.csproj';Properties=@{InnoProductTarget=$platform.target;InnoProductId='player'}})
        $checks = @($platform.configurations | ForEach-Object { @{configuration=$_;project='src/Player.csproj';product='player';seconds=2;identity='c'*64;source='executed'} })
        @{version=2;commit=$revision;target=$platform.target;sha256=(Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant();checks=$checks} |
            ConvertTo-Json -Depth 6 | Set-Content (Join-Path $directory 'consumption.json')
        $plan = @{contexts=@{managed=@{sdkGraph=$graph}};commit=$revision;tree='a'*64;environment='b'*64;full=$true;documentation=$null;nativeRequests=@(@{target=$platform.target})
            backendPackages=@(@{target=$platform.target;sha256=(Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant()})
            checks=@(@{id='consumer';kind='instructions';owner='engine';exclusion=$null;prerequisites=@()})}
        $plan | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $directory 'plan.json')
        @{commit=$revision;tree=$plan.tree;environment=$plan.environment;version = 2; failures = @(); completed=$true
          planIdentity=(Get-FileHash (Join-Path $directory 'plan.json')).Hash.ToLowerInvariant()
          results=@(@{id='consumer';status='Passed';seconds=1.0;inputIdentity='d'*64;outputs=@{}})} |
            ConvertTo-Json -Depth 8 | Set-Content (Join-Path $directory 'results.json')
    }
    function Publish { & "$PSScriptRoot/Publish-BackendPrebuilts.ps1" -Directory $root -Repository 'fixture/engine' -Revision $revision -ApiUrl 'https://fixture.invalid' }
    $proofPath = Join-Path (Join-Path $root $platforms[0].target) 'consumption.json'
    $completeProof = [IO.File]::ReadAllBytes($proofPath)
    $incomplete = Get-Content $proofPath -Raw | ConvertFrom-Json
    $incomplete.checks[0].source = 'backend-prebuilts-2020.01.01.1'
    $incomplete | ConvertTo-Json -Depth 6 | Set-Content $proofPath
    try { Publish; throw 'An untrusted product proof passed.' } catch {
        if ($_.Exception.Message -cnotin @('Missing real cold product consumption.', 'Untrusted SDK proof source.')) { throw }
    }
    $incomplete.checks = @()
    $incomplete | ConvertTo-Json -Depth 6 | Set-Content $proofPath
    try { Publish; throw 'An incomplete SDK matrix passed.' } catch { if ($_.Exception.Message -cne 'Missing verified SDK product matrix.') { throw } }
    if ($server.release) { throw 'An incomplete SDK matrix created a draft.' }
    [IO.File]::WriteAllBytes($proofPath,$completeProof)
    $server.denyDraft = $true
    try { Publish; throw 'A denied publication passed.' } catch { if ($_.Exception.Message -cne 'Release write permission denied.') { throw } }
    if ($server.release -or $server.tag) { throw 'Permission failure published an asset.' }
    $server.denyDraft = $false
    $server.wrongDigest = $true
    try { Publish; throw 'An incorrect upload receipt passed.' } catch { if ($_.Exception.Message -notlike 'GitHub failed to verify *') { throw } }
    if (!$server.release.draft -or $server.reference) { throw 'A mismatched upload became consumable.' }
    $server.wrongDigest = $false
    try { Publish; throw 'The interrupted upload passed.' } catch { if ($_.Exception.Message -cne 'Interrupted test upload') { throw } }
    if (!$server.release.draft -or $server.tag -or $server.uploads.Count -ne 1) { throw 'A partial release became consumable.' }
    $server.changedPlaceholder = $true
    try { Publish; throw 'Replaced draft asset passed.' } catch { if ($_.Exception.Message -cne 'The interrupted upload changed before recovery.') { throw } }
    if ($server.recoveredPlaceholder -or !$server.release.draft) { throw 'A changed asset was deleted.' }
    $server.changedPlaceholder = $false
    $result = Publish
    if ($result -notlike 'Published *' -or $server.release.draft -or !$server.reference -or !$server.recoveredPlaceholder -or $server.uploads.Count -ne $platforms.Count) { throw 'Recovery did not publish the complete target set.' }
    $result = Publish
    if ($result -notlike 'Reused *' -or $server.uploads.Count -ne $platforms.Count) { throw 'An identical retry changed release ownership.' }

    # Resume across the remote boundary where the old release was deleted and its tag still exists.
    $server.release.draft = $true
    $server.release.body += "`n`n<!-- backend-retirement 3 $retiredTag $revision $retiredObject -->"
    $server.retiredRef = @{object=@{type='tag';sha='0'*40}}
    try { Publish; throw 'A replaced tag could be retired.' } catch { if ($_.Exception.Message -notlike 'Retiring tag * was replaced.') { throw } }
    if (!$server.release.draft -or !$server.retiredRef) { throw 'A changed tag was removed.' }
    $server.retiredRef.object.sha = $retiredObject
    $result = Publish
    if ($result -notlike 'Published *' -or $server.retiredRef -or $server.release.draft) { throw 'Interrupted retirement left an orphaned tag.' }
} finally {
    $env:GH_TOKEN = $original
    if ([IO.Directory]::Exists($root)) { [IO.Directory]::Delete($root,$true) }
}
