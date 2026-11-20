function ConvertTo-BackendTagMessage([Collections.IDictionary]$Catalog) {
    $bytes = [Text.Encoding]::UTF8.GetBytes((ConvertTo-Json $Catalog -Depth 80 -Compress))
    $buffer = [IO.MemoryStream]::new()
    try {
        $gzip = [IO.Compression.GZipStream]::new($buffer, [IO.Compression.CompressionLevel]::SmallestSize, $true)
        try { $gzip.Write($bytes) } finally { $gzip.Dispose() }
        return "InnoEngine backend prebuilt catalog v1`n`n" + [Convert]::ToBase64String($buffer.ToArray())
    } finally { $buffer.Dispose() }
}

function Read-BackendTagCatalog([string]$Message) {
    $prefix = "InnoEngine backend prebuilt catalog v1`n`n"
    if (!$Message.StartsWith($prefix, [StringComparison]::Ordinal) -or $Message.Length -gt 8MB) { throw 'Invalid backend tag catalog header.' }
    $inputStream = [IO.MemoryStream]::new([Convert]::FromBase64String($Message.Substring($prefix.Length).TrimEnd()))
    $outputStream = [IO.MemoryStream]::new()
    $gzip = [IO.Compression.GZipStream]::new($inputStream, [IO.Compression.CompressionMode]::Decompress)
    try {
        $buffer = [byte[]]::new(65536)
        while (($count = $gzip.Read($buffer, 0, $buffer.Length)) -gt 0) {
            if ($outputStream.Length + $count -gt 32MB) { throw 'Backend tag catalog exceeds its size limit.' }
            $outputStream.Write($buffer, 0, $count)
        }
        $catalog = [Text.Encoding]::UTF8.GetString($outputStream.ToArray()) | ConvertFrom-Json -AsHashtable -Depth 80
        if ($catalog.version -ne 1 -or $catalog.revision -cnotmatch '^[a-f0-9]{40}$' -or !$catalog.platforms.Count) { throw 'Incomplete backend tag catalog.' }
        return $catalog
    } finally {
        $gzip.Dispose()
        $outputStream.Dispose()
        $inputStream.Dispose()
    }
}

function Get-BackendCatalogPlatform([string]$Archive, [string]$Asset, [string]$Target, [string]$Digest) {
    $zip = [IO.Compression.ZipFile]::OpenRead($Archive)
    try {
        function Read-PackageJson([string]$Path) {
            $entry = $zip.GetEntry($Path)
            if (!$entry -or $entry.Length -gt 16MB) { throw "Missing or excessive package metadata: $Path" }
            $reader = [IO.StreamReader]::new($entry.Open())
            try { $reader.ReadToEnd() | ConvertFrom-Json -AsHashtable -Depth 80 }
            finally { $reader.Dispose() }
        }
        $manifest = Read-PackageJson 'manifest.json'
        if ($manifest.version -ne 1 -or $manifest.target -cne $Target) { throw 'The package declares a different target or schema.' }
        $native = Read-PackageJson 'native/selection.json'
        $configurations = [ordered]@{}
        foreach ($file in $manifest.files) {
            if (!$file.path.StartsWith('managed/')) { continue }
            if (!$configurations.Contains($file.configuration)) { $configurations[$file.configuration] = [ordered]@{} }
            $owners = $configurations[$file.configuration]
            if ($owners.Contains($file.owner) -and $owners[$file.owner] -cne $file.input) { throw "Conflicting certified owner: $($file.owner)" }
            $owners[$file.owner] = $file.input
        }
        if (!$configurations.Count) { throw 'A backend platform package contains no compiled owners.' }
        return [ordered]@{target=$Target; asset=$Asset; sha256=$Digest; configurations=$configurations; native=$native}
    } finally { $zip.Dispose() }
}

function Get-BackendCatalog([string]$Base, [hashtable]$Headers, [object]$Release) {
    if ($Release.draft -or !$Release.prerelease -or $Release.tag_name -cnotmatch '^backend-prebuilts-\d{4}\.\d{2}\.\d{2}\.[1-9]\d*$') {
        throw 'Only a published versioned backend prerelease can supply a catalog.'
    }
    $ref = Invoke-RestMethod -Headers $Headers -Uri "$Base/git/ref/tags/$($Release.tag_name)"
    if ($ref.object.type -cne 'tag' -or $ref.object.sha -cnotmatch '^[a-f0-9]{40}$') { throw 'The backend release needs an annotated catalog tag.' }
    $object = Invoke-RestMethod -Headers $Headers -Uri "$Base/git/tags/$($ref.object.sha)"
    $catalog = Read-BackendTagCatalog $object.message
    if ($object.tag -cne $Release.tag_name -or $object.object.type -cne 'commit' -or $object.object.sha -cne $catalog.revision) {
        throw 'Backend tag metadata and source commit disagree.'
    }
    $assets = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($key in $catalog.platforms.Keys) {
        $platform = $catalog.platforms[$key]
        if ($key -cnotmatch '^[a-z0-9-]+$' -or $key -cne $platform.target -or $platform.asset -cnotmatch '^[a-z0-9]+-prebuilt-backends\.zip$' -or
            !$assets.Add($platform.asset) -or $platform.sha256 -cnotmatch '^[a-f0-9]{64}$') { throw 'Invalid backend catalog platform or asset identity.' }
    }
    if ($Release.assets.Count -ne $catalog.platforms.Count) { throw 'Backend release asset matrix differs from its catalog.' }
    foreach ($platform in $catalog.platforms.Values) {
        $asset = @($Release.assets | Where-Object name -CEQ $platform.asset)
        if ($asset.Count -ne 1 -or $asset[0].state -cne 'uploaded' -or $asset[0].digest -cne ('sha256:' + $platform.sha256)) {
            throw "Backend release bytes disagree with the tag for $($platform.target)."
        }
    }
    return $catalog
}
