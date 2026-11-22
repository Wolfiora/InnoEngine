using System;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Editor.Inspection;
using Inno.Editor.Rendering;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Shaders.ImGui;

/// <summary>
/// Exposes detached node values and one-gesture writes without exposing the live graph document.
/// </summary>
public sealed class ShaderNodeDrawContext
{
    private readonly GraphNodeRecord m_node;
    private readonly SerializationRegistry m_serialization;
    private readonly SerializationContext m_context;
    private readonly Action<string, GraphSerializedValue, string?> m_write;
    private readonly InspectionDrawContext m_inspection;
    private readonly bool m_readOnly;

    /// <summary>
    /// Creates invocation-scoped access for a graph or Inspector host.
    /// </summary>
    /// <param name="node">
    /// Detached node values for this invocation.
    /// </param>
    /// <param name="serialization">
    /// Current owner converter registry.
    /// </param>
    /// <param name="context">
    /// Complete owner reference context.
    /// </param>
    /// <param name="write">
    /// Host callback recording changes in the shared draft history.
    /// </param>
    /// <param name="previews">
    /// Current generation preview service.
    /// </param>
    /// <param name="inspection">
    /// Shared Inspector context, including native property controls and drag/drop.
    /// </param>
    /// <param name="readOnly">
    /// Whether the source belongs to an immutable installation.
    /// </param>
    public ShaderNodeDrawContext(
        GraphNodeRecord node,
        SerializationRegistry serialization,
        SerializationContext context,
        Action<string, GraphSerializedValue, string?> write,
        IEditorPreviewService previews,
        InspectionDrawContext inspection,
        bool readOnly
    ) {
        m_node = node;
        m_serialization = serialization;
        m_context = context;
        m_write = write;
        this.previews = previews;
        m_inspection = inspection;
        m_readOnly = readOnly;
    }

    /// <summary>
    /// Gets the shared generation-scoped preview service for optional inline texture previews; valid only during this draw.
    /// </summary>
    public IEditorPreviewService previews { get; }

    /// <summary>
    /// Gets the stable node identity for Editor widget IDs.
    /// </summary>
    public GraphNodeID nodeID => m_node.id;

    /// <summary>
    /// Reads a detached property value; missing values use the node's declared default.
    /// </summary>
    /// <typeparam name="T">
    /// Native serializable property type.
    /// </typeparam>
    /// <param name="key">
    /// Stable node-local property key.
    /// </param>
    /// <param name="defaultValue">
    /// Default used only when the property is absent.
    /// </param>
    /// <returns>
    /// The detached value. Malformed stored data is not silently replaced.
    /// </returns>
    public T Read<T>(
        string key,
        T defaultValue
    ) => ShaderGraphDocument.Read(m_node, key, defaultValue, m_serialization, m_context);

    /// <summary>
    /// Records an unsaved draft edit through shared history; only an explicit document save applies it to the asset.
    /// </summary>
    /// <typeparam name="T">
    /// Native serializable property type.
    /// </typeparam>
    /// <param name="key">
    /// Stable node-local property key.
    /// </param>
    /// <param name="value">
    /// New detached value.
    /// </param>
    /// <param name="mergeKey">
    /// The current shared gesture key, or null for an independent edit.
    /// </param>
    public void Write<T>(
        string key,
        T value,
        string? mergeKey = null
    ) {
        if (m_readOnly)
        {
            throw new InvalidOperationException("Copy this installed Shader into the project before editing.");
        }
        m_write(key, ShaderGraphDocument.Encode(value, m_serialization, m_context), mergeKey);
    }

    /// <summary>
    /// Draws a node property through the existing Inspector controls, including precise numbers and asset picking.
    /// </summary>
    /// <typeparam name="T">
    /// Supported native property type.
    /// </typeparam>
    /// <param name="key">
    /// Stable node property identity.
    /// </param>
    /// <param name="label">
    /// Artist-facing property label.
    /// </param>
    /// <param name="defaultValue">
    /// Value used only when no property is stored.
    /// </param>
    public void DrawProperty<T>(
        string key,
        string label,
        T defaultValue
    ) {
        T value = Read(key, defaultValue);
        m_inspection.properties.DrawValue(
            m_inspection.editorContext,
            m_node,
            "shader.node." + key,
            label,
            typeof(T),
            () => value,
            edited => value = (T)edited!,
            new Edits((
                mutation,
                mergeKey
            ) =>
            {
                mutation();
                Write(key, value, mergeKey);
            }),
            m_readOnly
        );
    }

    private sealed class Edits(Action<Action, string?> apply) : IInspectionPropertyEditService
    {
        /// <inheritdoc/>
        public bool ChangeProperty(
            object owner,
            string propertyName,
            Action mutation,
            string historyName,
            string? mergeKey,
            string? componentName
        ) {
            apply(mutation, mergeKey);
            return true;
        }
    }
}
