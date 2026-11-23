using System;
using Inno.Core.Diagnostics;
using Inno.Rendering.Assets;
using Inno.Rendering.Runtime;

namespace Inno.Editor.Shaders;

/// <summary>
/// Carries frame-only preview inputs; persistent provider state must retain stable values rather than this object.
/// </summary>
public sealed class ShaderPreviewContext
{
    /// <summary>
    /// Creates a preview invocation from host-owned detached inputs.
    /// </summary>
    /// <param name="resourceID">
    /// Isolated material publication and viewport resource identity.
    /// </param>
    /// <param name="material">
    /// Detached values with a current Shader reference; do not modify the reference.
    /// </param>
    /// <param name="artifact">
    /// Compiled candidate, never published as a canonical Shader.
    /// </param>
    /// <param name="definition">
    /// Detached contract captured by the candidate.
    /// </param>
    /// <param name="diagnostics">
    /// Preview-local reporter; errors must not contaminate project diagnostics.
    /// </param>
    /// <param name="pixelWidth">
    /// Positive target width.
    /// </param>
    /// <param name="pixelHeight">
    /// Positive target height.
    /// </param>
    public ShaderPreviewContext(
        RenderPersistentResourceID resourceID,
        MaterialAsset material,
        RenderShaderArtifact artifact,
        ShaderDefinition definition,
        IDiagnosticReporter diagnostics,
        int pixelWidth,
        int pixelHeight
    ) {
        if (!resourceID.isValid)
        {
            throw new ArgumentException("A resource identity is required.", nameof(resourceID));
        }
        ArgumentNullException.ThrowIfNull(material);
        ArgumentNullException.ThrowIfNull(artifact);
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(diagnostics);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelHeight);
        this.resourceID = resourceID;
        this.material = material;
        this.artifact = artifact;
        this.definition = definition;
        this.diagnostics = diagnostics;
        this.pixelWidth = pixelWidth;
        this.pixelHeight = pixelHeight;
    }

    /// <summary>
    /// Gets the publication scope released by the host when this preview closes.
    /// </summary>
    public RenderPersistentResourceID resourceID { get; }

    /// <summary>
    /// Gets the invocation-local Material values.
    /// </summary>
    public MaterialAsset material { get; }

    /// <summary>
    /// Gets the complete immutable GPU candidate.
    /// </summary>
    public RenderShaderArtifact artifact { get; }

    /// <summary>
    /// Gets the candidate's exact contract, not the latest uncompiled source interface.
    /// </summary>
    public ShaderDefinition definition { get; }

    /// <summary>
    /// Gets the host-owned, isolated diagnostic producer.
    /// </summary>
    public IDiagnosticReporter diagnostics { get; }

    /// <summary>
    /// Gets target width in physical pixels.
    /// </summary>
    public int pixelWidth { get; }

    /// <summary>
    /// Gets target height in physical pixels.
    /// </summary>
    public int pixelHeight { get; }
}
