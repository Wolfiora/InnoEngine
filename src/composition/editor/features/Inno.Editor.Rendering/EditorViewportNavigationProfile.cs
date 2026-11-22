using System;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Describes how the host may navigate one provider-owned viewport without exposing the provider's camera model.
/// </summary>
public sealed class EditorViewportNavigationProfile
{
    /// <summary>
    /// Creates a provider-defined navigation profile.
    /// </summary>
    /// <param name="id">
    /// Stable profile identity.
    /// </param>
    /// <param name="capabilities">
    /// Operations accepted by the provider.
    /// </param>
    /// <param name="defaultMode">
    /// Preferred mode when current state is unsupported.
    /// </param>
    public EditorViewportNavigationProfile(
        EditorViewportNavigationProfileID id,
        EditorViewportNavigationCapabilities capabilities,
        EditorViewportNavigationMode defaultMode
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A valid navigation profile ID is required.", nameof(id));
        }
        const EditorViewportNavigationCapabilities C_VALID_CAPABILITIES = EditorViewportNavigationCapabilities.Pan
                                                                          | EditorViewportNavigationCapabilities.Zoom
                                                                          | EditorViewportNavigationCapabilities.Orbit
                                                                          | EditorViewportNavigationCapabilities.Fly
                                                                          | EditorViewportNavigationCapabilities.FrameSelection;
        if ((capabilities & ~C_VALID_CAPABILITIES) != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capabilities));
        }
        if (capabilities != EditorViewportNavigationCapabilities.None
            && !SupportsMode(capabilities, defaultMode)
        ) {
            throw new ArgumentException(
                "The default navigation mode must be enabled by the profile capabilities.",
                nameof(defaultMode)
            );
        }
        this.id = id;
        this.capabilities = capabilities;
        this.defaultMode = defaultMode;
    }

    /// <summary>
    /// Gets the stable profile identity.
    /// </summary>
    public EditorViewportNavigationProfileID id { get; }

    /// <summary>
    /// Gets the operations accepted by this provider.
    /// </summary>
    public EditorViewportNavigationCapabilities capabilities { get; }

    /// <summary>
    /// Gets the mode selected when current state is not supported.
    /// </summary>
    public EditorViewportNavigationMode defaultMode { get; }

    /// <summary>
    /// Gets or sets the provider-defined world-up direction.
    /// </summary>
    public Vector3 worldUp { get; set; } = Vector3.UP;

    /// <summary>
    /// Gets or sets the optional current selection bound.
    /// </summary>
    public EditorViewportFocusBounds? focusBounds { get; set; }

    /// <summary>
    /// Gets or sets pointer rotation sensitivity in radians per pixel.
    /// </summary>
    public float rotationSensitivity { get; set; } = 0.005f;

    /// <summary>
    /// Gets or sets exponential wheel zoom sensitivity.
    /// </summary>
    public float zoomSensitivity { get; set; } = 0.16f;

    /// <summary>
    /// Gets or sets minimum orthographic half-height.
    /// </summary>
    public float minimumOrthographicSize { get; set; } = 0.001f;

    /// <summary>
    /// Gets or sets maximum orthographic half-height.
    /// </summary>
    public float maximumOrthographicSize { get; set; } = 100000f;

    /// <summary>
    /// Gets or sets minimum orbit or framing distance.
    /// </summary>
    public float minimumFocusDistance { get; set; } = 0.01f;

    /// <summary>
    /// Gets or sets maximum orbit or framing distance.
    /// </summary>
    public float maximumFocusDistance { get; set; } = 1000000f;

    /// <summary>
    /// Gets or sets the multiplier used while fast fly movement is requested.
    /// </summary>
    public float fastMovementMultiplier { get; set; } = 4f;

    /// <summary>
    /// Gets or sets additional framing space around selection bounds.
    /// </summary>
    public float framePadding { get; set; } = 1.25f;

    /// <summary>
    /// Gets a disabled fallback profile used when no provider navigation contract exists.
    /// </summary>
    public static EditorViewportNavigationProfile disabled { get; } = new(
        new EditorViewportNavigationProfileID("inno.editor.navigation.disabled"),
        EditorViewportNavigationCapabilities.None,
        EditorViewportNavigationMode.Planar
    );

    private static bool SupportsMode(
        EditorViewportNavigationCapabilities capabilities,
        EditorViewportNavigationMode mode
    ) => mode switch
    {
        EditorViewportNavigationMode.Planar =>
            (capabilities & EditorViewportNavigationCapabilities.Planar) != 0,
        EditorViewportNavigationMode.Orbit =>
            capabilities.HasFlag(EditorViewportNavigationCapabilities.Orbit),
        EditorViewportNavigationMode.Fly =>
            capabilities.HasFlag(EditorViewportNavigationCapabilities.Fly),
        _ => false
    };
}
