using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("f25d91a5-df87-4ed6-a383-12a9071af66e")]
internal sealed class CoupledHistoryComponent : GameBehavior
{
    private bool m_primary = true;
    private CoupledHistoryMode m_mode;

    [SerializableProperty]
    public bool primary
    {
        get => m_primary;
        set => m_primary = value && mode == CoupledHistoryMode.Base;
    }

    [SerializableProperty]
    public CoupledHistoryMode mode
    {
        get => m_mode;
        set
        {
            m_mode = value;
            if (value == CoupledHistoryMode.Overlay)
            {
                m_primary = false;
            }
        }
    }
}
