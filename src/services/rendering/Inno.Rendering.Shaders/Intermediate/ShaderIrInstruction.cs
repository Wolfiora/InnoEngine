using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Stores one immutable typed instruction without generated stage source or backend expressions.
/// </summary>
public sealed class ShaderIrInstruction
{
    internal ShaderIrInstruction(
        ShaderIrOperation operation,
        IEnumerable<ShaderIrValue> inputs,
        IEnumerable<ShaderIrValue> outputs,
        string? inputName = null,
        ulong constantBits = 0,
        int memberIndex = 0,
        ShaderSourceImplementationAnalysis? source = null,
        IEnumerable<ShaderIrBlock>? regions = null
    ) {
        this.operation = operation;
        this.inputs = Array.AsReadOnly(inputs.ToArray());
        this.outputs = Array.AsReadOnly(outputs.ToArray());
        this.inputName = inputName;
        this.constantBits = constantBits;
        this.memberIndex = memberIndex;
        this.source = source;
        this.regions = Array.AsReadOnly(regions?.ToArray() ?? []);
    }

    /// <summary>
    /// Gets the semantic operation independent of any source language.
    /// </summary>
    public ShaderIrOperation operation { get; }

    /// <summary>
    /// Gets immutable operands in operation or function-declaration order.
    /// </summary>
    public IReadOnlyList<ShaderIrValue> inputs { get; }

    /// <summary>
    /// Gets produced values; a call orders its return value first, then out/inout parameters.
    /// </summary>
    public IReadOnlyList<ShaderIrValue> outputs { get; }

    /// <summary>
    /// Gets the stable interface or region-parameter identity, only for Input or RegionInput.
    /// </summary>
    public string? inputName { get; }

    /// <summary>
    /// Gets exact scalar bits for a Constant instruction; the result type determines interpretation.
    /// </summary>
    public ulong constantBits { get; }

    /// <summary>
    /// Gets the zero-based component/member/element ordinal for an Extract instruction.
    /// </summary>
    public int memberIndex { get; }

    /// <summary>
    /// Gets a frozen, analyzed function implementation only for a SourceCall instruction.
    /// </summary>
    public ShaderSourceImplementationAnalysis? source { get; }

    /// <summary>
    /// Gets immutable nested regions: true/false for Branch, or one body for Loop.
    /// </summary>
    public IReadOnlyList<ShaderIrBlock> regions { get; }

    /// <summary>
    /// Gets whether removing or reordering this instruction may change observable behavior.
    /// </summary>
    public bool hasSideEffects
        => operation is ShaderIrOperation.SourceCall or ShaderIrOperation.StorageLoad
           or ShaderIrOperation.StorageStore or ShaderIrOperation.StorageAtomicAdd or ShaderIrOperation.Discard
           or ShaderIrOperation.Branch or ShaderIrOperation.Loop;
}
