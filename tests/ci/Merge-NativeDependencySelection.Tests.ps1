$ErrorActionPreference = 'Stop'
$owner = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/native-selection-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @('replace-selected', 'retain-other-product', 'identical-fragments', 'conflicting-fragments', 'foreign-revision', 'empty-production')
foreach ($case in $cases) {
    $root = Join-Path $owner $case
    New-Item -ItemType Directory -Path $root -Force | Out-Null
    $revision = 'a' * 40
    $release = 'native-deps-fixture-2'
    function Asset($product, $digest, $producer, $tag) {
        @{ product = $product; component = 'shared-library'; target = 'fixture'; configuration = 'debug'; options = @()
            fingerprint = 'b' * 64; asset = "$product.zip"; sha256 = $digest; producerRevision = $producer; release = $tag }
    }
    @{ assets = @((Asset editor ('c' * 64) ('d' * 40) 'native-deps-fixture-1'), (Asset player ('e' * 64) ('d' * 40) 'native-deps-fixture-1')); aggregates = @() } |
        ConvertTo-Json -Depth 8 | Set-Content (Join-Path $root 'baseline.json')
    $asset = Asset editor ('f' * 64) $revision $release
    if ($case -ceq 'foreign-revision') { $asset.producerRevision = '0' * 40 }
    if ($case -ceq 'retain-other-product') { $asset.product = 'shader-tools' }
    $assets = @(if ($case -cne 'empty-production') { $asset })
    @{ assets = $assets; aggregates = @() } | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $root 'produced.json')
    $fragments = @((Join-Path $root 'produced.json'))
    if ($case -cin @('identical-fragments', 'conflicting-fragments')) {
        if ($case -ceq 'conflicting-fragments') { $asset.sha256 = '1' * 64 }
        @{ assets = @($asset); aggregates = @() } | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $root 'second.json')
        $fragments += Join-Path $root 'second.json'
    }
    $failed = $false
    try {
        & "$PSScriptRoot/Merge-NativeDependencySelection.ps1" -Fragments $fragments -BaselinePath (Join-Path $root 'baseline.json') `
            -OutputPath (Join-Path $root 'merged.json') -ProducerRevision $revision -Release $release
    } catch { $failed = $true; $_ | Out-String | Set-Content (Join-Path $root 'failure.log') }
    if ($failed -ne ($case -cin @('conflicting-fragments', 'foreign-revision', 'empty-production'))) { throw "Unexpected native selection merge: $case. See $root." }
    if (-not $failed) {
        $bytes = [IO.File]::ReadAllBytes((Join-Path $root 'merged.json'))
        if ($bytes[0] -ne 123 -or $bytes[-1] -ne 10 -or $bytes -contains 13) { throw 'The published selector must have stable UTF-8 and LF bytes.' }
        $selection = Get-Content (Join-Path $root 'merged.json') -Raw | ConvertFrom-Json
        $player = @($selection.assets | Where-Object product -CEQ player)
        if ($player.Count -ne 1 -or $player[0].sha256 -cne ('e' * 64)) { throw 'An unrelated product selection was overwritten.' }
        $expectedCount = if ($case -ceq 'retain-other-product') { 3 } else { 2 }
        if (@($selection.assets).Count -ne $expectedCount -or @($selection.assets | Where-Object producerRevision -CEQ $revision).Count -ne 1) { throw 'Product-scoped replacement was not exact.' }
    }
}
Write-Output "Passed $($cases.Count) native selection merge contracts. Evidence: $owner"
$global:LASTEXITCODE = 0
