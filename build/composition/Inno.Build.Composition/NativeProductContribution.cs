using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Composition;

/// <summary>
/// Binds a product's immutable graph to its platform-selected execution strategy.
/// </summary>
public sealed class NativeProductContribution
{
    /// <summary>
    /// Captures a complete product without SDK discovery or output creation.
    /// </summary>
    /// <param name="plan">
    /// The immutable component graph.
    /// </param>
    /// <param name="executor">
    /// The explicit execution strategy.
    /// </param>
    public NativeProductContribution(
        ProductNativeBuildPlan plan,
        INativeProductExecutor executor
    ) {
        this.plan = plan ?? throw new ArgumentNullException(nameof(plan));
        this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
    }

    /// <summary>
    /// Gets the immutable product graph.
    /// </summary>
    public ProductNativeBuildPlan plan { get; }
    /// <summary>
    /// Gets the platform-selected executor.
    /// </summary>
    public INativeProductExecutor executor { get; }

    /// <summary>
    /// Executes the selected product without inspecting its target identity.
    /// </summary>
    /// <param name="context">
    /// The operation with frozen tools and binding configuration.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation and unpublished output.
    /// </param>
    /// <returns>
    /// The complete owned native result.
    /// </returns>
    public Task<ProductNativeBuildResult> BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken
    ) => executor.BuildAsync(context, plan, cancellationToken);
}
