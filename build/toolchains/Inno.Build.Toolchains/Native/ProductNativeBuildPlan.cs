using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Executes a frozen product closure with one target toolchain and no implicit backend selection.
/// </summary>
public sealed class ProductNativeBuildPlan
{
    private readonly ProductNativeBuildStep[] m_steps;

    /// <summary>
    /// Validates the complete ordered component graph before tools or staging can start.
    /// </summary>
    /// <param name="productId">
    /// The product identity whose native closure is selected.
    /// </param>
    /// <param name="steps">
    /// Unique, dependency-ordered component operations.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The product, component identity or dependency ordering is invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The step collection is null.
    /// </exception>
    public ProductNativeBuildPlan(
        string productId,
        IReadOnlyList<ProductNativeBuildStep> steps
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);
        ArgumentNullException.ThrowIfNull(steps);
        var completed = new HashSet<string>(StringComparer.Ordinal);
        if (steps.Count == 0)
            throw new ArgumentException("A native product requires an explicit component closure.", nameof(steps));
        foreach (ProductNativeBuildStep step in steps)
        {
            if (step is null || step.dependencies.Any(dependency => !completed.Contains(dependency)) || !completed.Add(step.id))
                throw new ArgumentException("Native steps must be unique and ordered after every dependency.", nameof(steps));
        }
        this.productId = productId;
        m_steps = steps.ToArray();
        this.steps = Array.AsReadOnly(m_steps);
    }

    /// <summary>
    /// Gets the explicit product identity.
    /// </summary>
    public string productId { get; }

    /// <summary>
    /// Gets the frozen dependency-ordered component closure.
    /// </summary>
    public IReadOnlyList<ProductNativeBuildStep> steps { get; }

    /// <summary>
    /// Prepares the declared closure using the context's already resolved target tools.
    /// </summary>
    /// <param name="context">
    /// The operation checkout, configuration and frozen target selection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels active component work and prevents subsequent steps.
    /// </param>
    /// <returns>
    /// The exact products in declaration order after complete preparation.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Tools are unassigned or a producer returns a foreign component or target.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The closure contains components owned by a static aggregate executor.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Preparation was canceled.
    /// </exception>
    public async Task<IReadOnlyList<NativeBuildProduct>> BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        if (m_steps.Any(static step => step.build is null))
            throw new NotSupportedException("This closure requires its platform's static aggregate executor.");
        NativeToolchainSelection toolchain = context.RequireToolchain();
        context = context.WithBindings(await NativeBindingPreparation.PrepareAsync(context,
            m_steps.Select(static step => step.component).ToArray(), cancellationToken).ConfigureAwait(false));
        var completed = new Dictionary<string, NativeBuildProduct>(StringComparer.Ordinal);
        var borrowed = new ReadOnlyDictionary<string, NativeBuildProduct>(completed);
        foreach (ProductNativeBuildStep step in m_steps)
        {
            cancellationToken.ThrowIfCancellationRequested();
            NativeBuildProduct product = await step.build!(context.WithComponentOptions(step.component, step.options), borrowed, cancellationToken).ConfigureAwait(false);
            if (product.component != step.id || product.targetId != toolchain.targetId)
                throw new InvalidOperationException($"Native step '{step.id}' returned a foreign component or target.");
            completed.Add(step.id, product);
        }
        return Array.AsReadOnly(m_steps.Select(step => completed[step.id]).ToArray());
    }

    /// <summary>
    /// Resolves exact deployment paths from the declared layout rather than component-name conventions.
    /// </summary>
    /// <param name="products">
    /// The complete result of this plan, in declaration order.
    /// </param>
    /// <returns>
    /// Owned immutable relative-output to source-file mappings for atomic deployment.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Products differ from the plan or deployment paths collide or escape their root.
    /// </exception>
    public IReadOnlyDictionary<string, string> CreateDeploymentFiles(IReadOnlyList<NativeBuildProduct> products)
    {
        ArgumentNullException.ThrowIfNull(products);
        if (products.Count != m_steps.Length)
            throw new ArgumentException("A deployment requires this plan's exact product closure.", nameof(products));
        var files = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (int index = 0; index < m_steps.Length; index++)
        {
            ProductNativeBuildStep step = m_steps[index];
            NativeBuildProduct product = products[index];
            if (step.id != product.component)
                throw new ArgumentException("The products do not match this plan's declaration order.", nameof(products));
            foreach (string file in product.files)
            {
                string relative = Path.GetRelativePath(Path.Combine(product.directory, "Outputs"), file);
                string? output = step.deploymentPath(relative, product.targetId);
                if (output is null)
                    continue;
                string portable = output.Replace('\\', '/');
                if (Path.IsPathFullyQualified(portable) || portable.Contains(':')
                    || portable.Split('/').Any(static segment => segment is "" or "." or "..")
                    || !files.TryAdd(portable.Replace('/', Path.DirectorySeparatorChar), file))
                    throw new ArgumentException("Native deployment paths must be portable, contained and unique.", nameof(products));
            }
        }
        return new ReadOnlyDictionary<string, string>(files);
    }
}
