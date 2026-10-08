using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Inno.Editor.Hosting;

/// <summary>
/// Runs the shared Editor product with explicitly supplied platform and publication composition.
/// </summary>
public static class EditorApplication
{
    /// <summary>
    /// Validates composition, owns the created Editor host and retires resources on exit, cancellation or failure.
    /// </summary>
    /// <param name="options">
    /// Borrowed catalogs, factories and product configuration; created resources transfer to the host.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels before creation or requests orderly owner-thread shutdown.
    /// </param>
    /// <returns>
    /// Zero after orderly completion; startup, execution and retirement failures propagate.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Options or a required dependency is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A required location or product selection is invalid.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// A selected backend lacks its matching runtime or authoring provider.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Startup or execution was canceled after resource retirement.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Product startup or resource retirement fails.
    /// </exception>
    public static async Task<int> RunAsync(
        EditorLaunchOptions options,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.keyboard);
        ArgumentNullException.ThrowIfNull(options.adapterCatalog);
        ArgumentNullException.ThrowIfNull(options.shell);
        ArgumentNullException.ThrowIfNull(options.frameDriver);
        ArgumentNullException.ThrowIfNull(options.distribution);
        ArgumentNullException.ThrowIfNull(options.buildContext);
        ArgumentNullException.ThrowIfNull(options.createEngineHost);
        ArgumentNullException.ThrowIfNull(options.createScriptModuleSource);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.projectDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(options.supportPackRoot);
        if (options.smokeFrameLimit is <= 0)
            throw new ArgumentOutOfRangeException(nameof(options), "A smoke frame limit must be positive.");
        if (!options.distribution.availableTargets.Contains(options.defaultBuildTarget))
            throw new ArgumentException("The explicit default build target is absent from the distribution.", nameof(options));
        options.shell.adapters.Validate(options.adapterCatalog);
        if (!options.presentation.isValid || !options.adapterCatalog.presentation.supportedBackends.Contains(options.presentation))
            throw new NotSupportedException("The selected presentation backend is not registered.");
        if (!options.adapterCatalog.renderingAuthoring.supportedBackends.Contains(options.shell.adapters.rendering))
            throw new NotSupportedException("The selected rendering backend requires a matching authoring provider.");
        cancellationToken.ThrowIfCancellationRequested();
        using EditorHost host = EditorHost.Create(options);
        cancellationToken.ThrowIfCancellationRequested();
        return await host.RunAsync(options.frameDriver, options.smokeFrameLimit, cancellationToken);
    }
}
