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
    & "$PSScriptRoot/../../tools/ci/Initialize-BackendProduction.ps1" @arguments
    $receipt = Get-Content -LiteralPath (Join-Path $output 'candidate.json') -Raw | ConvertFrom-Json
    if (!$receipt.reused -or $receipt.sha256 -cne $digest -or $receipt.target -cne $target.target -or
        (Get-FileHash -LiteralPath (Join-Path $output $target.asset)).Hash.ToLowerInvariant() -cne $digest -or (Test-Path -LiteralPath $native)) {
        throw 'Unchanged platform transfer performed native preparation or changed the ZIP.'
    }
    $arguments.OutputDirectory = Join-Path $owner 'bad'
    $arguments.PreviousDigest = '0'*64
    try { & "$PSScriptRoot/../../tools/ci/Initialize-BackendProduction.ps1" @arguments; throw 'A different seed was accepted.' }
    catch { if ($_.Exception.Message -cne 'Backend producer seed differs from the catalog.') { throw } }
    if (Test-Path -LiteralPath (Join-Path $arguments.OutputDirectory 'candidate.json')) { throw 'Rejected transfer published a receipt.' }
    $root = Join-Path $owner 'workspace with spaces'
    $scripts = Join-Path $root 'tools/ci'
    $project = Join-Path $root 'entry/InnoEngine.Build'
    New-Item -ItemType Directory -Path $project,$scripts,"$root/.github" -Force | Out-Null
    '<Project />' | Set-Content "$root/Directory.Build.props"
    '<Project />' | Set-Content "$root/Directory.Build.targets"
    '<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>' |
        Set-Content "$project/InnoEngine.Build.csproj"
    'System.Console.WriteLine("backend-entry-verified");' | Set-Content "$project/Program.cs"
    '[{"target":"fixture-host"}]' | Set-Content "$root/.github/native-targets.json"
    Copy-Item "$PSScriptRoot/../../tools/ci/Initialize-BackendProduction.ps1" $scripts
    'return $false' | Set-Content "$scripts/Initialize-NativeDependencyWorkloads.ps1"
    @'
param($Target,$HostTarget,$BuildEntry,$Dotnet,$Directory,$PreviousDirectory)
if ((& $Dotnet $BuildEntry) -cne 'backend-entry-verified' -or $LASTEXITCODE) { throw 'Could not execute the produced Build entry.' }
[IO.File]::WriteAllText($Directory,$BuildEntry)
'@ | Set-Content "$scripts/Prepare-BackendNativeInputs.ps1"
    $observed = Join-Path $root 'observed.txt'
    & "$scripts/Initialize-BackendProduction.ps1" -Target fixture-host -HostTarget fixture-host -Operation produce `
        -OutputDirectory "$root/candidate" -NativeDirectory $observed | Out-Null
    $relative = [IO.Path]::GetRelativePath("$root/artifacts/bn/sdk",(Get-Content $observed -Raw)).Replace('\','/')
    if ([IO.Path]::IsPathRooted($relative) -or $relative.StartsWith('../') -or [IO.Path]::GetFileName($relative) -cne 'InnoEngine.Build.dll') { throw "Unexpected SDK entry owner: $relative." }
} finally { [IO.Directory]::Delete($owner,$true) }
