using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

[SerializationRevision("m_revision")]
internal sealed class PureFieldCaptureProperties : IdentityObject, ISerializable
{
    private ulong? m_revision = 0;
    private PureCaptureFields m_value;

    [SerializableProperty]
    public PureCaptureFields value
    {
        get => m_value;
        set
        {
            m_revision++;
            m_value = value;
        }
    }
}
