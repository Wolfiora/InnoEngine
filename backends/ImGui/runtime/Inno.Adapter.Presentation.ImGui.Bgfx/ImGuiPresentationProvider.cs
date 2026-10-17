using System;
using Inno.Adapter.Presentation;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Registers SDL3 window integration with BGFX ImGui presentation.
/// </summary>
public sealed class ImGuiPresentationProvider : PresentationBackendProvider
{
    private readonly ImGuiInteractionOptions m_interaction;

    /// <summary>
    /// Registers the bundled presentation implementation without creating a native context.
    /// </summary>
    /// <param name="interaction">
    /// The borrowed immutable conventions explicitly selected by the Editor product.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Interaction configuration is absent.
    /// </exception>
    public ImGuiPresentationProvider(ImGuiInteractionOptions interaction) : base(PresentationBackendId.imGui)
    {
        ArgumentNullException.ThrowIfNull(interaction);
        m_interaction = interaction;
    }

    /// <inheritdoc />
    public override IPresentationContext CreateContext(PresentationBackendOptions options)
        => new ImGuiPresentationContext(options, m_interaction);
}
