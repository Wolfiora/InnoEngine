using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Build.Managed;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;

namespace Inno.Build.Composition;

/// <summary>
/// Binds one implemented target to its factory, Support Pack source and SDK provider as a single registration.
/// </summary>
public sealed class BuildPlatformContribution
{
    /// <summary>
    /// Creates a complete target registration and rejects mismatched source identities immediately.
    /// </summary>
    /// <param name="descriptor">
    /// The immutable target facts owned by the platform package.
    /// </param>
    /// <param name="factory">
    /// The authoring target factory for these exact target facts.
    /// </param>
    /// <param name="supportPackSource">
    /// The matching runtime and compiler input source.
    /// </param>
    /// <param name="toolchainProvider">
    /// The SDK resolver for this target, independent of the host.
    /// </param>
    /// <param name="editorProject">
    /// The portable checkout-relative Editor product, or null when no Editor is supported.
    /// </param>
    /// <param name="nativeProducts">
    /// Explicit product-native plans owned by this target; null declares no ordinary native product build.
    /// </param>
    /// <param name="bindingGenerator">
    /// The target-owned generator, or null when the contribution generates no bindings.
    /// </param>
    /// <param name="managedDeployments">
    /// Deployment identities supported by this platform, or null when none is contributed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A required contribution is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A source identity differs from its target, an Editor declaration is inconsistent,
    /// native product plans are null or duplicated, or deployment identities are unassigned or repeated.
    /// </exception>
    public BuildPlatformContribution(
        PlatformTargetDescriptor descriptor,
        BuildTargetFactory factory,
        IPlayerSupportPackSource supportPackSource,
        INativeToolchainProvider toolchainProvider,
        string? editorProject = null,
        IReadOnlyList<NativeProductContribution>? nativeProducts = null,
        INativeBindingGenerator? bindingGenerator = null,
        IReadOnlyList<ManagedDeploymentId>? managedDeployments = null
    ) {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(supportPackSource);
        ArgumentNullException.ThrowIfNull(toolchainProvider);
        if (descriptor.id != supportPackSource.target)
            throw new ArgumentException("A platform source must match its declared target.", nameof(supportPackSource));
        string? editor = editorProject?.Replace('\\', '/');
        if (descriptor.supportsEditor != (editor is not null)
            || editor is not null && (editor.StartsWith('/') || editor.Contains(':')
                || !editor.EndsWith(".csproj", StringComparison.Ordinal)
                || editor.Split('/').Any(static segment => segment is "" or "." or "..")))
            throw new ArgumentException("Editor capability requires one explicit contained product project.", nameof(editorProject));
        this.descriptor = descriptor;
        this.bindingGenerator = bindingGenerator;
        this.factory = factory;
        this.supportPackSource = supportPackSource;
        this.toolchainProvider = toolchainProvider;
        this.editorProject = editor;
        var products = new Dictionary<string, NativeProductContribution>(StringComparer.Ordinal);
        foreach (NativeProductContribution product in nativeProducts ?? [])
            if (product is null || !products.TryAdd(product.plan.productId, product))
                throw new ArgumentException("Native product plans must be complete and have unique product identities.", nameof(nativeProducts));
        this.nativeProducts = new ReadOnlyDictionary<string, NativeProductContribution>(products);
        ManagedDeploymentId[] deployments = (managedDeployments ?? []).ToArray();
        if (deployments.Any(static id => string.IsNullOrWhiteSpace(id.value))
            || deployments.Distinct().Count() != deployments.Length)
            throw new ArgumentException("Platform deployment identities must be assigned and unique.", nameof(managedDeployments));
        this.managedDeployments = Array.AsReadOnly(deployments);
    }

    /// <summary>
    /// Gets the platform-owned, explicit target mapping.
    /// </summary>
    public PlatformTargetDescriptor descriptor { get; }

    /// <summary>
    /// Gets the matching authoring target factory.
    /// </summary>
    public BuildTargetFactory factory { get; }

    /// <summary>
    /// Gets the matching product input source.
    /// </summary>
    public IPlayerSupportPackSource supportPackSource { get; }

    /// <summary>
    /// Gets the explicit SDK resolver for this target.
    /// </summary>
    public INativeToolchainProvider toolchainProvider { get; }

    /// <summary>
    /// Gets the implemented Editor product location without deriving a project from target naming.
    /// </summary>
    public string? editorProject { get; }

    /// <summary>
    /// Gets the frozen ordinary product-build closures explicitly contributed by this target.
    /// </summary>
    public IReadOnlyDictionary<string, NativeProductContribution> nativeProducts { get; }
    /// <summary>
    /// Gets the target-owned generator, or null when no binding capability is registered.
    /// </summary>
    public INativeBindingGenerator? bindingGenerator { get; }

    /// <summary>
    /// Gets the deployment identities this platform authorizes for its declared managed runtime target.
    /// </summary>
    public IReadOnlyList<ManagedDeploymentId> managedDeployments { get; }

}
