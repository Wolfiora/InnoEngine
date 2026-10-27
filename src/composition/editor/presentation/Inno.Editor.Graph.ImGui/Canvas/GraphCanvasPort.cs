using System;
using System.Numerics;
using Inno.Core.Graphs;

namespace Inno.Editor.Graph.ImGui;

/// <summary>
/// Describes a neutral port's node-relative geometry and display state without retaining extensions.
/// </summary>
public readonly record struct GraphCanvasPort
{
    /// <summary>
    /// Creates a port presentation whose position is measured in unscaled node coordinates.
    /// </summary>
    /// <param name="endpoint">
    /// The existing document endpoint represented by this port.
    /// </param>
    /// <param name="direction">
    /// The connection direction, independent of any domain-specific type system.
    /// </param>
    /// <param name="position">
    /// Node-relative graph coordinates of the port center.
    /// </param>
    /// <param name="label">
    /// The current display label, without domain objects or callbacks.
    /// </param>
    /// <param name="isOptional">
    /// Whether to distinguish an optional input visually.
    /// </param>
    /// <param name="isMissing">
    /// Whether retained document structure currently lacks its domain implementation.
    /// </param>
    /// <param name="labelBounds">
    /// Optional node-relative label bounds used by drawing and row hit testing; null limits hits to the socket.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An endpoint identity is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The label is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The position or direction is invalid.
    /// </exception>
    public GraphCanvasPort(
        GraphEndpoint endpoint,
        GraphPortDirection direction,
        Vector2 position,
        string label,
        bool isOptional = false,
        bool isMissing = false,
        (Vector2 minimum, Vector2 maximum)? labelBounds = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(endpoint.nodeId.value);
        ArgumentException.ThrowIfNullOrWhiteSpace(endpoint.portId.value);
        ArgumentNullException.ThrowIfNull(label);
        if (!float.IsFinite(position.X) || !float.IsFinite(position.Y))
            throw new ArgumentOutOfRangeException(nameof(position));
        if (direction is not GraphPortDirection.Input and not GraphPortDirection.Output)
            throw new ArgumentOutOfRangeException(nameof(direction));
        if (labelBounds is { } bounds && (!float.IsFinite(bounds.minimum.X) || !float.IsFinite(bounds.minimum.Y)
            || !float.IsFinite(bounds.maximum.X) || !float.IsFinite(bounds.maximum.Y)
            || bounds.maximum.X < bounds.minimum.X || bounds.maximum.Y < bounds.minimum.Y))
            throw new ArgumentOutOfRangeException(nameof(labelBounds));
        this.endpoint = endpoint;
        this.direction = direction;
        this.position = position;
        this.label = label;
        this.isOptional = isOptional;
        this.isMissing = isMissing;
        this.labelBounds = labelBounds;
    }

    /// <summary>
    /// Gets the document endpoint; it does not identify a live engine object.
    /// </summary>
    public GraphEndpoint endpoint { get; }

    /// <summary>
    /// Gets the neutral connection direction.
    /// </summary>
    public GraphPortDirection direction { get; }

    /// <summary>
    /// Gets the node-relative center in graph coordinates.
    /// </summary>
    public Vector2 position { get; }

    /// <summary>
    /// Gets the current display label.
    /// </summary>
    public string label { get; }

    /// <summary>
    /// Gets whether the input is visually optional.
    /// </summary>
    public bool isOptional { get; }

    /// <summary>
    /// Gets whether this retained endpoint currently lacks an implementation.
    /// </summary>
    public bool isMissing { get; }

    /// <summary>
    /// Gets optional node-relative label geometry shared by rendering and row hit testing.
    /// </summary>
    public (Vector2 minimum, Vector2 maximum)? labelBounds { get; }
}
