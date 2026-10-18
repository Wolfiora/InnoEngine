using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.Toolchains.Bgfx;

/// <summary>
/// Freezes an SDK tool name and ordered arguments for execution from the component source root.
/// </summary>
public sealed class BgfxBuildInvocation
{
    /// <summary>
    /// Snapshots the invocation so later caller mutations cannot change its fingerprint or execution.
    /// </summary>
    /// <param name="tool">
    /// The logical executable declared by the selected native toolchain.
    /// </param>
    /// <param name="arguments">
    /// Ordered arguments, with source paths relative to the owned source root.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The tool is blank or an argument is null.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The argument sequence is null.
    /// </exception>
    public BgfxBuildInvocation(
        string tool,
        IEnumerable<string> arguments
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(tool);
        ArgumentNullException.ThrowIfNull(arguments);
        string[] snapshot = arguments.ToArray();
        if (snapshot.Any(static value => value is null))
            throw new ArgumentException("Invocation arguments cannot be null.", nameof(arguments));
        this.tool = tool;
        this.arguments = Array.AsReadOnly(snapshot);
    }

    /// <summary>
    /// Gets the declared tool name resolved by the operation's frozen toolchain.
    /// </summary>
    public string tool { get; }

    /// <summary>
    /// Gets immutable, ordered compiler arguments included in the recipe identity.
    /// </summary>
    public IReadOnlyList<string> arguments { get; }
}
