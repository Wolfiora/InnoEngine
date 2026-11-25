using System;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("9ea6d04d-4ed3-48f3-ac3a-b09372a575ab")]
internal sealed class BatchValidationComponent : GameComponent
{
    [SerializableProperty]
    public int value { get; set; }

    [SerializableProperty]
    public bool rejectAnswer { get; set; }

    [OnSerializableRestored]
    private void Validate()
    {
        if (rejectAnswer && value == 42)
        {
            throw new InvalidOperationException("The current owner rejected this batch value.");
        }
    }
}
