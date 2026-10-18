using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Scripting.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Represents the runtime contract imported from a shader graph.
/// </summary>
[StableTypeId("e6672287-145f-4f51-8380-a6aeaf57a801")]
public class ShaderAsset : AssetObject
{
    private ShaderDefinitionSnapshot? m_definition;

    [SerializableProperty(PropertyVisibility.Hide)]
    private byte[] m_definitionData = [];

    /// <summary>
    /// Gets a detached editable copy of the committed backend-neutral definition, or null before import.
    /// Nested declaration arrays are copied; referenced assets remain identity-owned resources.
    /// </summary>
    public ShaderDefinition? definition => m_definition?.CreateDefinition();

    /// <summary>
    /// Commits a validated definition through the native serialization channel.
    /// </summary>
    /// <param name="value">
    /// Complete definition visible to materials and target compilation.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry that owns the active shader converter generation.
    /// </param>
    /// <param name="context">
    /// The complete owner reference context used for material texture defaults.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A declaration collection is null. The previous committed definition remains unchanged.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Serialization cannot encode the candidate. No part of the candidate is published.
    /// </exception>
    [ScriptingApiIgnore]
    public void SetDefinition(
        ShaderDefinition value,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(context);
        var snapshot = new ShaderDefinitionSnapshot(value);
        ShaderDefinition captured = snapshot.CreateDefinition();
        byte[] definitionData = serialization.Serialize(captured, context);
        m_definitionData = definitionData;
        m_definition = snapshot;
    }

    [OnSerializableRestored]
    private void OnSerializableRestored(SerializationContext context)
    {
        m_definition = m_definitionData.Length == 0
            ? null
            : new ShaderDefinitionSnapshot(context.GetRequired<SerializationRegistry>().Deserialize<ShaderDefinition>(
                m_definitionData,
                context));
    }
}

