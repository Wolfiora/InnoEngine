using System;

namespace Inno.Rendering;

/// <summary>
/// Describes a persistent offscreen target without owning a backend-native handle.
/// </summary>
public sealed class RenderTexture
{
    private RenderTextureDescriptor m_descriptor;

    /// <summary>
    /// Creates an offscreen render target description.
    /// </summary>
    /// <param name="name">
    /// Artist-facing and diagnostic name.
    /// </param>
    /// <param name="descriptor">
    /// Initial texture requirements.
    /// </param>
    public RenderTexture(
        string name,
        RenderTextureDescriptor descriptor
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(descriptor);
        this.name = name;
        m_descriptor = descriptor;
    }

    /// <summary>
    /// Gets the artist-facing and diagnostic name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the current texture requirements.
    /// </summary>
    public RenderTextureDescriptor descriptor => m_descriptor;

    /// <summary>
    /// Gets a counter incremented whenever the descriptor changes.
    /// </summary>
    public long contentRevision { get; private set; }

    /// <summary>
    /// Replaces texture requirements at the next render-frame safety point.
    /// </summary>
    /// <param name="descriptor">
    /// New texture requirements.
    /// </param>
    public void Resize(RenderTextureDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        if (m_descriptor.Equals(descriptor))
        {
            return;
        }

        m_descriptor = descriptor;
        contentRevision++;
    }
}

