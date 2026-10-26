using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Build.Toolchains;

namespace Inno.Build.Composition;

/// <summary>
/// Registers an implemented native SDK capability without declaring a complete game product.
/// </summary>
public sealed class NativeToolchainContribution
{
    /// <summary>
    /// Binds immutable target facts to the provider that owns SDK resolution.
    /// </summary>
    /// <param name="descriptor">
    /// The exact target and ABI supported by this contribution.
    /// </param>
    /// <param name="provider">
    /// The borrowed provider that resolves executable tools for that target.
    /// </param>
    /// <param name="nativeProducts">
    /// Optional implemented native-only products, without game publication registration.
    /// </param>
    /// <param name="bindingGenerator">
    /// The target-owned generator, or null when the contribution generates no bindings.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A required contribution member is null.
    /// </exception>
    public NativeToolchainContribution(
        PlatformTargetDescriptor descriptor,
        INativeToolchainProvider provider,
        IReadOnlyList<NativeProductContribution>? nativeProducts = null,
        INativeBindingGenerator? bindingGenerator = null
    ) {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentNullException.ThrowIfNull(provider);
        this.descriptor = descriptor;
        this.bindingGenerator = bindingGenerator;
        this.provider = provider;
        nativeProducts ??= [];
        if (nativeProducts.Any(static plan => plan is null))
            throw new ArgumentException("Native product contributions cannot be null.", nameof(nativeProducts));
        this.nativeProducts = new ReadOnlyDictionary<string, NativeProductContribution>(
            nativeProducts.ToDictionary(static product => product.plan.productId, StringComparer.Ordinal));
    }

    /// <summary>
    /// Gets explicit target facts independently of the current execution machine.
    /// </summary>
    public PlatformTargetDescriptor descriptor { get; }

    /// <summary>
    /// Gets the registered SDK boundary; registration does not start tools or inspect the environment.
    /// </summary>
    public INativeToolchainProvider provider { get; }
    /// <summary>
    /// Gets explicitly implemented native-only product closures keyed by stable product identity.
    /// </summary>
    public IReadOnlyDictionary<string, NativeProductContribution> nativeProducts { get; }

    /// <summary>
    /// Gets the target-owned generator, or null when no binding capability is registered.
    /// </summary>
    public INativeBindingGenerator? bindingGenerator { get; }

}
