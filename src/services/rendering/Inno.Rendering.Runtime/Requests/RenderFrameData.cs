using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Carries generation-scoped, frame-only payloads between a request producer and its pipeline.
/// </summary>
/// <remarks>
/// Values may use reloadable plugin types because a snapshot is retained only until the owning
/// render frame completes. Persistent configuration must use stable identifiers and serialized bytes.
/// </remarks>
[Inno.Extensibility.Types.StableTypeId("08a1cd24-757d-5026-a05d-a72ab9fd2b2b")]
public sealed class RenderFrameData
{
    private readonly Dictionary<FrameDataKey, object> m_values = [];
    private bool m_isReadOnly;

    /// <summary>
    /// Gets the number of populated channel and value-type pairs.
    /// </summary>
    public int count => m_values.Count;

    /// <summary>
    /// Adds or replaces one typed value in an open data channel.
    /// </summary>
    /// <typeparam name="TValue">
    /// Frame-local payload type.
    /// </typeparam>
    /// <param name="channel">
    /// Pipeline-defined stable data channel.
    /// </param>
    /// <param name="value">
    /// Value retained until the current frame completes.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown after the data has entered a request.
    /// </exception>
    public void Set<TValue>(
        Inno.Rendering.RenderDataChannelId channel,
        TValue value
    )
        where TValue : notnull
    {
        if (!channel.isValid)
            throw new ArgumentException("A render data channel must be valid.", nameof(channel));
        if (m_isReadOnly)
            throw new InvalidOperationException("Submitted render frame data is immutable.");
        ArgumentNullException.ThrowIfNull(value);
        m_values[new FrameDataKey(channel, typeof(TValue))] = value;
    }

    /// <summary>
    /// Tries to read one typed value from an open data channel.
    /// </summary>
    /// <typeparam name="TValue">
    /// Expected frame-local payload type.
    /// </typeparam>
    /// <param name="channel">
    /// Pipeline-defined stable data channel.
    /// </param>
    /// <param name="value">
    /// Receives the stored value when present.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the channel contains the exact requested type.
    /// </returns>
    public bool TryGet<TValue>(
        Inno.Rendering.RenderDataChannelId channel,
        out TValue? value
    ) {
        if (channel.isValid
            && m_values.TryGetValue(new FrameDataKey(channel, typeof(TValue)), out object? stored)
            && stored is TValue typed)
        {
            value = typed;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Removes all values before this object enters a submitted request.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown after the data has entered a request.
    /// </exception>
    public void Clear()
    {
        if (m_isReadOnly)
            throw new InvalidOperationException("Submitted render frame data is immutable.");
        m_values.Clear();
    }

    internal RenderFrameData Snapshot()
    {
        var snapshot = new RenderFrameData();
        foreach ((FrameDataKey key, object value) in m_values)
            snapshot.m_values.Add(key, value);
        snapshot.m_isReadOnly = true;
        return snapshot;
    }

    private readonly record struct FrameDataKey(
        Inno.Rendering.RenderDataChannelId channel,
        Type type
    );
}

