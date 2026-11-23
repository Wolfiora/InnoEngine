using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;
using Inno.Extensibility.Types;
using Inno.Rendering.Shaders;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Returns a frozen typed-stage compilation candidate; failure never carries a usable artifact.
/// </summary>
[StableTypeID("cf544248-8a38-5999-b8aa-bbdcf94fa6e8")]
public sealed class ShaderStageToolResult
{
    private readonly byte[] m_bytes;

    /// <summary>
    /// Creates a candidate result from adapter-owned generation, compilation and diagnostics.
    /// </summary>
    /// <param name="bytes">
    /// Compiled binary, or empty after failure.
    /// </param>
    /// <param name="bindings">
    /// Logical-to-native resource layout used by this exact binary.
    /// </param>
    /// <param name="diagnostics">
    /// Structured adapter diagnostics with original source positions.
    /// </param>
    public ShaderStageToolResult(
        ReadOnlySpan<byte> bytes,
        IEnumerable<ShaderStageBinding> bindings,
        IEnumerable<ShaderSourceDiagnostic> diagnostics
    ) {
        ArgumentNullException.ThrowIfNull(bindings);
        ArgumentNullException.ThrowIfNull(diagnostics);
        this.bindings = Array.AsReadOnly(bindings.ToArray());
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
        if (!bytes.IsEmpty && this.diagnostics.Any(static value => value.severity == DiagnosticSeverity.Error))
        {
            throw new ArgumentException("A failed candidate cannot publish compiled bytes.", nameof(bytes));
        }
        m_bytes = bytes.ToArray();
    }

    /// <summary>
    /// Gets immutable target binary data, empty on failure.
    /// </summary>
    public ReadOnlyMemory<byte> bytes => m_bytes;

    /// <summary>
    /// Gets the exact generated logical-to-native resource layout.
    /// </summary>
    public IReadOnlyList<ShaderStageBinding> bindings { get; }

    /// <summary>
    /// Gets generation and native compiler diagnostics.
    /// </summary>
    public IReadOnlyList<ShaderSourceDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets whether a nonempty binary was compiled without error diagnostics.
    /// </summary>
    public bool succeeded => !bytes.IsEmpty && diagnostics.All(static value => value.severity != DiagnosticSeverity.Error);
}
