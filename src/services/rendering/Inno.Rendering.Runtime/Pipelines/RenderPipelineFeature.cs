using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Adds capability-aware passes without owning frame graph state.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("a16b9e1e-2bf1-5ba4-85aa-ee24ebda097b")]
public abstract class RenderPipelineFeature
{
    /// <summary>
    /// Applies reload-safe settings to this feature generation.
    /// </summary>
    /// <param name="configuration">
    /// Stable feature configuration.
    /// </param>
    /// <param name="settings">
    /// Native context bound to the canonical Pipeline asset's actual owner.
    /// </param>
    public void Configure(
        RenderFeatureConfiguration configuration,
        RenderExtensionStateContext settings
    ) {
        ArgumentNullException.ThrowIfNull(settings);
        OnConfigure(configuration.state, settings);
    }

    /// <summary>
    /// Adds frame-scoped passes and dependencies.
    /// </summary>
    /// <param name="context">
    /// Frame-scoped feature context.
    /// </param>
    public abstract void AddRenderPasses(RenderFeatureContext context);

    /// <summary>
    /// Reads feature-owned settings from neutral state.
    /// </summary>
    /// <param name="state">
    /// Reload-safe extension state.
    /// </param>
    /// <param name="settings">
    /// Native context bound to the canonical Pipeline asset's actual owner.
    /// </param>
    protected virtual void OnConfigure(
        SerializedRenderExtensionState state,
        RenderExtensionStateContext settings
    ) { }
}

