$ErrorActionPreference = 'Stop'
$owner = [IO.Directory]::CreateDirectory((Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/backend-bootstrap', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N')))).FullName
$target = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)[0]
try {
    $archive = Join-Path $owner 'verified.zip'
    [IO.File]::WriteAllBytes($archive,[byte[]](7,4,2))
    $digest = (Get-FileHash -LiteralPath $archive).Hash.ToLowerInvariant()
    $output = Join-Path $owner 'candidate'
    $native = Join-Path $owner 'native'
    $arguments = @{Target=$target.target;HostTarget=$target.target;Operation='consume';OutputDirectory=$output;NativeDirectory=$native
        PreviousZip=$archive;PreviousDigest=$digest;Dotnet='an-sdk-must-not-run-for-an-unchanged-transfer'}
    & "$PSScriptRoot/Initialize-BackendProduction.ps1" @arguments
    $receipt = Get-Content -LiteralPath (Join-Path $output 'candidate.json') -Raw | ConvertFrom-Json
    if (!$receipt.reused -or $receipt.sha256 -cne $digest -or $receipt.target -cne $target.target -or
        (Get-FileHash -LiteralPath (Join-Path $output $target.asset)).Hash.ToLowerInvariant() -cne $digest -or (Test-Path -LiteralPath $native)) {
        throw 'Unchanged platform transfer performed native preparation or changed the ZIP.'
    }
    $arguments.OutputDirectory = Join-Path $owner 'bad'
    $arguments.PreviousDigest = '0'*64
    try { & "$PSScriptRoot/Initialize-BackendProduction.ps1" @arguments; throw 'A different seed was accepted.' }
    catch { if ($_.Exception.Message -cne 'Backend producer seed differs from the catalog.') { throw } }
    if (Test-Path -LiteralPath (Join-Path $arguments.OutputDirectory 'candidate.json')) { throw 'Rejected transfer published a receipt.' }
} finally { [IO.Directory]::Delete($owner,$true) }
