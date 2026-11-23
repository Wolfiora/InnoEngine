using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline;

internal struct AssetImportDependencyData
{
    [SerializableProperty]
    internal int kind { get; set; }

    [SerializableProperty]
    internal string key { get; set; }

    [SerializableProperty]
    internal string fingerprint { get; set; }

    [SerializableProperty]
    internal bool sourceStampValid { get; set; }

    [SerializableProperty]
    internal long sourceLength { get; set; }

    [SerializableProperty]
    internal long sourceLastWriteUtcTicks { get; set; }

    [SerializableProperty]
    internal long sourceCreationTimeUtcTicks { get; set; }
}
