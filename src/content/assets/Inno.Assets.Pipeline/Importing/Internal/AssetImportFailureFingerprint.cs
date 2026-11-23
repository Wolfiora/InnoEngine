namespace Inno.Assets.Pipeline;

internal readonly record struct AssetImportFailureFingerprint(
    string relativePath,
    int status,
    string sourceHash,
    string importerID,
    string diagnostics
);
