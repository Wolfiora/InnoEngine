using System;

using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeId("b6abaad9-d570-4605-875f-f7a24f4b49cd")]
internal sealed class HistoryValueProbe : GameComponent
{
    internal static int reads;
    internal static bool rejectAnswer;

    private int m_value;

    [SerializableProperty]
    public int value
    {
        get
        {
            reads++;
            return m_value;
        }
        set
        {
            if (rejectAnswer && value == 42)
                throw new InvalidOperationException("The requested value was rejected.");
            m_value = value;
        }
    }
}
