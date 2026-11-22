using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

/// <summary>
/// Provides a real public extension type for collectible metadata and capture-key acceptance.
/// </summary>
[SerializationRevision("m_revision")]
public sealed class CaptureRevisionProperties : IdentityObject, ISerializable
{
    private ulong? m_revision = 0;
    private int m_number;

    /// <summary>
    /// Creates an initially empty tracked property source.
    /// </summary>
    public CaptureRevisionProperties() {}

    /// <summary>
    /// Gets or sets the serialized number, advancing its complete source-owned revision before mutation.
    /// </summary>
    [SerializableProperty]
    public int number
    {
        get => m_number;
        set
        {
            if (m_revision is { } revision)
            {
                m_revision = revision == ulong.MaxValue ? null : revision + 1;
            }
            m_number = value;
        }
    }
}
