using System;
using Inno.Adapter.Presentation;
using Inno.Build.Composition;
using Inno.Build;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Extensibility.Modules;
using Inno.Runtime;
using Inno.Scripting.Compiler;
using Inno.Shell;

namespace Inno.Editor.Hosting;

/// <summary>
/// Supplies product-owned composition to the shared Editor lifecycle without selecting a platform or backend.
/// </summary>
public sealed class EditorLaunchOptions
{
    /// <summary>
    /// Gets the current authoring project location; shared authoring services own its project IO.
    /// </summary>
    public required string projectDirectory { get; init; }

    /// <summary>
    /// Gets explicit keyboard conventions selected by the Editor product.
    /// </summary>
    public required EditorKeyboardPolicy keyboard { get; init; }

    /// <summary>
    /// Gets the borrowed immutable runtime, authoring and presentation provider catalogs.
    /// </summary>
    public required IAuthoringAdapterCatalog adapterCatalog { get; init; }

    /// <summary>
    /// Gets explicit backend, primary-window and rendering policy selected by the product.
    /// </summary>
    public required ShellOptions shell { get; init; }

    /// <summary>
    /// Gets the explicit presentation implementation selected by the product.
    /// </summary>
    public required PresentationBackendId presentation { get; init; }

    /// <summary>
    /// Gets the borrowed owner-thread scheduling policy.
    /// </summary>
    public required IShellFrameDriver frameDriver { get; init; }

    /// <summary>
    /// Gets the borrowed immutable publication distribution.
    /// </summary>
    public required BuildDistribution distribution { get; init; }

    /// <summary>
    /// Gets the explicit default game target for new project build settings.
    /// </summary>
    public required BuildTargetId defaultBuildTarget { get; init; }

    /// <summary>
    /// Gets the explicitly selected SDK and build execution host.
    /// </summary>
    public required BuildCompositionContext buildContext { get; init; }

    /// <summary>
    /// Gets the explicit installed Support Pack location provided by product composition.
    /// </summary>
    public required string supportPackRoot { get; init; }

    /// <summary>
    /// Gets the factory transferring a newly created metadata-configured engine to the Editor owner.
    /// </summary>
    public required Func<EngineHost> createEngineHost { get; init; }

    /// <summary>
    /// Gets the factory transferring a candidate script module source to the existing reload transaction.
    /// </summary>
    public required Func<ScriptModuleDeployment, IModuleSource> createScriptModuleSource { get; init; }

    /// <summary>
    /// Gets an optional factory transferring each edit or play log sink to its owning session.
    /// </summary>
    public Func<RuntimeSessionKind, LogSessionId, ILogSink>? createSessionLogSink { get; init; }

    /// <summary>
    /// Gets an optional factory transferring one host log sink to the Editor lifetime.
    /// </summary>
    public Func<ILogSink?>? createHostLogSink { get; init; }

    /// <summary>
    /// Gets an optional positive frame limit for bounded product verification.
    /// </summary>
    public int? smokeFrameLimit { get; init; }
}
