using System;

namespace Inno.Build;

/// <summary>
/// Binds independent packaging and content implementations for one publication target.
/// </summary>
public sealed class GameBuildTargetBinding
{
    /// <summary>
    /// Validates a complete binding without starting tools or taking ownership of services.
    /// </summary>
    /// <param name="packager">
    /// The borrowed platform packaging implementation.
    /// </param>
    /// <param name="compiler">
    /// The borrowed content implementation for the same target and authoring generation.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Either implementation is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A target is unassigned or the two implementation identities disagree.
    /// </exception>
    public GameBuildTargetBinding(
        IGameBuildTarget packager,
        IGameContentCompiler compiler
    ) {
        ArgumentNullException.ThrowIfNull(packager);
        ArgumentNullException.ThrowIfNull(compiler);
        if (string.IsNullOrWhiteSpace(packager.id.value) || packager.id != compiler.target)
            throw new ArgumentException("Packaging and content compilation require the same assigned target.", nameof(compiler));
        this.packager = packager;
        this.compiler = compiler;
    }

    /// <summary>
    /// Gets the platform implementation responsible for layout and Support Pack validation.
    /// </summary>
    public IGameBuildTarget packager { get; }

    /// <summary>
    /// Gets the content implementation borrowing this pipeline's authoring generation.
    /// </summary>
    public IGameContentCompiler compiler { get; }
}
