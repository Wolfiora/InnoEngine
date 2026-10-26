param(
    [Parameter(Mandatory = $true)][string]$EditorAssembly,
    [Parameter(Mandatory = $true)][string]$ProjectDirectory,
    [Parameter(Mandatory = $true)][string]$BackendPattern,
    [string]$GraphicsApi,
    [string]$Dotnet = 'dotnet',
    [int]$Frames = 3,
    [int]$TimeoutSeconds = 180
)

$ErrorActionPreference = 'Stop'
if ($Frames -le 0 -or $TimeoutSeconds -le 0) { throw 'Smoke limits must be positive.' }
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$EditorAssembly = [IO.Path]::GetFullPath($EditorAssembly)
if (-not (Test-Path -LiteralPath $EditorAssembly -PathType Leaf)) { throw 'Build the selected Editor before smoke acceptance.' }
$ProjectDirectory = [IO.Path]::GetFullPath($ProjectDirectory)
New-Item -ItemType Directory -Force (Join-Path $ProjectDirectory 'Assets') | Out-Null
$logs = Join-Path $ProjectDirectory 'Logs'
New-Item -ItemType Directory -Force $logs | Out-Null
$outputPath = Join-Path $logs 'NativeRendering.stdout.log'
$errorPath = Join-Path $logs 'NativeRendering.stderr.log'
$nativeLog = Join-Path $logs 'NativeRendering.log'
$bootPath = Join-Path $logs 'EditorBoot.log'
if (Test-Path -LiteralPath $bootPath -PathType Leaf) { [IO.File]::Delete($bootPath) }
$start = [Diagnostics.ProcessStartInfo]::new($Dotnet)
$start.UseShellExecute = $false
$start.CreateNoWindow = $true
$start.RedirectStandardOutput = $true
$start.RedirectStandardError = $true
$start.WorkingDirectory = $ProjectDirectory
foreach ($argument in @($EditorAssembly, $ProjectDirectory, '--smoke-frames', [string]$Frames)) {
    $start.ArgumentList.Add($argument)
}
if ($GraphicsApi) {
    $start.ArgumentList.Add('--graphics-api')
    $start.ArgumentList.Add($GraphicsApi)
}
$process = [Diagnostics.Process]::new()
$process.StartInfo = $start
$copies = [Threading.CancellationTokenSource]::new()
$stdout = [IO.File]::Create($outputPath)
$stderr = [IO.File]::Create($errorPath)
$failure = $null
$exitCode = $null
$started = $false
try {
    if (-not $process.Start()) { throw 'The Editor smoke process did not start.' }
    $started = $true
    Write-Output "Editor smoke process: $($process.Id); deadline: $TimeoutSeconds seconds."
    $outputCopy = $process.StandardOutput.BaseStream.CopyToAsync($stdout, 81920, $copies.Token)
    $errorCopy = $process.StandardError.BaseStream.CopyToAsync($stderr, 81920, $copies.Token)
    if (-not $process.WaitForExit($TimeoutSeconds * 1000)) {
        if ($IsMacOS) {
            # Capture the blocked native/managed boundary before terminating this owned child.
            & /usr/bin/sample $process.Id 2 -file (Join-Path $logs 'EditorProcess.sample.txt')
            Write-Output "Native sample exit code: $LASTEXITCODE."
        }
        throw "Editor smoke process $($process.Id) did not exit within $TimeoutSeconds seconds."
    }
    $exitCode = $process.ExitCode
    Write-Output "Editor smoke process exited with code $exitCode."
}
catch {
    $failure = $_
}
finally {
    if ($started -and -not $process.HasExited) {
        $process.Kill($true)
        if (-not $process.WaitForExit(10000)) { throw 'The owned smoke process did not terminate.' }
    }
    if ($outputCopy -and $errorCopy) {
        try {
            [Threading.Tasks.Task]::WhenAll([Threading.Tasks.Task[]]@($outputCopy, $errorCopy)).WaitAsync(
                [TimeSpan]::FromSeconds(10)).GetAwaiter().GetResult()
        }
        catch {
            if (-not $failure) { $failure = $_ }
            $copies.Cancel()
            $process.StandardOutput.Dispose()
            $process.StandardError.Dispose()
            try { [Threading.Tasks.Task]::WhenAll([Threading.Tasks.Task[]]@($outputCopy, $errorCopy)).WaitAsync([TimeSpan]::FromSeconds(5)).GetAwaiter().GetResult() }
            catch {
                # The original stream failure remains fatal; observe cancellation/closure failures too.
                Write-Output "Smoke output retirement: $($_.Exception.Message)"
            }
        }
    }
    $stdout.Dispose()
    $stderr.Dispose()
    $copies.Dispose()
    $process.Dispose()
    [IO.File]::WriteAllText($nativeLog, [IO.File]::ReadAllText($outputPath) + [IO.File]::ReadAllText($errorPath))
}
if ($failure) { throw $failure }
if ($exitCode -ne 0) { throw "Renderer smoke exited with code $exitCode." }
if (-not (Test-Path -LiteralPath $bootPath)) { throw 'Renderer smoke did not produce EditorBoot.log.' }
$boot = Get-Content -LiteralPath $bootPath -Raw
if ($boot -notmatch $BackendPattern) { throw "Renderer smoke did not initialize '$BackendPattern'." }
if ($boot -notmatch "Smoke frame limit reached after $Frames frame\(s\)\.") { throw 'Renderer smoke did not submit the required frame count.' }
if ($boot -notmatch 'Dispose end\.') { throw 'Renderer smoke did not complete common adapter shutdown.' }
if ($boot -match 'Teardown failure') { throw 'Renderer smoke reported a teardown failure.' }
if ($boot -notmatch 'Primary presentation encodes sRGB: True\.') { throw 'Renderer smoke did not enable the sRGB backbuffer contract.' }
if ((Get-Content -LiteralPath $nativeLog -Raw) -match 'BGFX FATAL') { throw 'Renderer smoke reported a BGFX fatal diagnostic.' }
Write-Output "Renderer smoke passed: $Frames frames, orderly process exit, complete teardown and sRGB presentation."
