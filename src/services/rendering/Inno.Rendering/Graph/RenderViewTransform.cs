using System;

namespace Inno.Rendering;

/// <summary>
/// Stores backend-ready column-major transform matrices for one programmable view.
/// </summary>
public sealed class RenderViewTransform
{
    private readonly float[] m_viewMatrix;
    private readonly float[] m_projectionMatrix;

    /// <summary>
    /// Creates a raster view transform.
    /// </summary>
    /// <param name="viewMatrix">
    /// Exactly sixteen column-major world-to-view values.
    /// </param>
    /// <param name="projectionMatrix">
    /// Exactly sixteen column-major projection values.
    /// </param>
    public RenderViewTransform(
        ReadOnlySpan<float> viewMatrix,
        ReadOnlySpan<float> projectionMatrix
    ) {
        if (viewMatrix.Length != 16)
        {
            throw new ArgumentException("A view matrix requires exactly sixteen values.", nameof(viewMatrix));
        }

        if (projectionMatrix.Length != 16)
        {
            throw new ArgumentException(
                "A projection matrix requires exactly sixteen values.",
                nameof(projectionMatrix)
            );
        }

        m_viewMatrix = viewMatrix.ToArray();
        m_projectionMatrix = projectionMatrix.ToArray();
    }

    /// <summary>
    /// Gets the immutable column-major view matrix.
    /// </summary>
    public ReadOnlyMemory<float> viewMatrix => m_viewMatrix;

    /// <summary>
    /// Gets the immutable column-major projection matrix.
    /// </summary>
    public ReadOnlyMemory<float> projectionMatrix => m_projectionMatrix;
}
