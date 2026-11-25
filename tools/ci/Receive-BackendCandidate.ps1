param(
    [Parameter(Mandatory)]$Platform,
    [Parameter(Mandatory)][string]$Directory,
    [string]$ApiUrl = $env:GITHUB_API_URL,
    [string]$Repository = $env:GITHUB_REPOSITORY
)
$ErrorActionPreference = 'Stop'
$source = $Platform.promotion
$registration = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json |
    Where-Object target -CEQ $Platform.target)
if ($registration.Count -ne 1 -or $registration[0].asset -cne $Platform.asset -or
    $source.digest -cnotmatch '^sha256:[a-f0-9]{64}$' -or [string]$source.artifact -cnotmatch '^[1-9][0-9]*$') { throw 'Invalid promoted backend identity.' }
$directoryPath = [IO.Directory]::CreateDirectory((Join-Path $Directory "backend-candidate-$($Platform.target)")).FullName
$temp = [IO.Path]::GetTempFileName()
try {
    Invoke-WebRequest -Uri "$ApiUrl/repos/$Repository/actions/artifacts/$($source.artifact)/zip" -Headers @{Authorization="Bearer $env:GH_TOKEN"} -OutFile $temp
    if ('sha256:'+(Get-FileHash $temp).Hash.ToLowerInvariant() -cne $source.digest) { throw 'The certified Actions artifact changed.' }
    $zip = [IO.Compression.ZipFile]::OpenRead($temp)
    try {
        foreach ($name in @($Platform.asset,'consumption.json')) {
            $matchesByName = @($zip.Entries | Where-Object FullName -CEQ $name)
            if ($matchesByName.Count -ne 1) { throw "Missing or ambiguous candidate file $name." }
            [IO.Compression.ZipFileExtensions]::ExtractToFile($matchesByName[0],(Join-Path $directoryPath $name),$false)
        }
    } finally { $zip.Dispose() }
    $receipt = Get-Content (Join-Path $directoryPath 'consumption.json') -Raw | ConvertFrom-Json
    $archive = Join-Path $directoryPath $Platform.asset
    if ($receipt.commit -cne $source.commit -or $receipt.version -ne 2 -or $receipt.target -cne $Platform.target -or
        $receipt.sha256 -cne (Get-FileHash $archive).Hash.ToLowerInvariant()) { throw 'Invalid certified backend ZIP.' }
    Write-Host "Using $($Platform.target) from workflow run $($source.run), attempt $($source.attempt), artifact $($source.artifact), ZIP $($receipt.sha256)."
    $archive
} finally { [IO.File]::Delete($temp) }
