using System;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Marks a reloadable provider that produces model-neutral render requests each frame.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[StableTypeID("54ca2eb2-0fc2-5b3e-91d1-0bd52d01caab")]
public sealed class RenderRequestProviderExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a render request provider declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable provider identifier.
    /// </param>
    /// <param name="priority">
    /// Provider invocation priority; lower values run first.
    /// </param>
    public RenderRequestProviderExtensionAttribute(
        string id,
        int priority = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
        this.priority = priority;
    }

    /// <summary>
    /// Gets the globally stable provider identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the provider invocation priority.
    /// </summary>
    public int priority { get; }
}
