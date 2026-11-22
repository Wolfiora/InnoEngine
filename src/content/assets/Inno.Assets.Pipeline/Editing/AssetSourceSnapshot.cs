namespace Inno.Assets.Pipeline;

/// <summary>
/// Contains detached authoring bytes and conflict-detection state.
/// </summary>
public sealed class AssetSourceSnapshot
{
    private readonly byte[] m_bytes;

    internal AssetSourceSnapshot(
        byte[] bytes,
        string hash,
        bool readOnly
    ) {
        m_bytes = bytes;
        this.contentHash = hash;
        this.isReadOnly = readOnly;
    }

    /// <summary>
    /// Gets a copy of the captured native source bytes.
    /// </summary>
    public byte[] bytes => (byte[])m_bytes.Clone();

    /// <summary>
    /// Gets the source fingerprint required by a subsequent save.
    /// </summary>
    public string contentHash { get; }

    /// <summary>
    /// Gets whether the installation source cannot be edited in place.
    /// </summary>
    public bool isReadOnly { get; }
}
