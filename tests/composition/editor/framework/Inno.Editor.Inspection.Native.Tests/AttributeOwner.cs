using System;
using Inno.Core.Serialization;
using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection.Native.Tests;

internal sealed class AttributeOwner : ISerializable
{
    [SerializableProperty]
    [Header("Presentation", "Decorators are composed before the property row.")]
    [Text("Persistent decorator text remains visible.")]
    [Tooltip("Reusable hover guidance.")]
    [InspectorName("Friendly Value")]
    [Range(2, 8)]
    [ShowIf(nameof(showValue))]
    [ProbeInspector]
    public AttributeValue value { get; set; } = new();

    internal bool showValue { get; set; }
}
