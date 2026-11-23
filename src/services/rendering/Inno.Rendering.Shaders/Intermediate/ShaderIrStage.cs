using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Freezes a typed GPU stage and its explicit interface. It contains no main function, varying declarations or source expressions.
/// Targets own stage/resource layout; adapters translate these semantics into their native language.
/// </summary>
public sealed class ShaderIrStage
{
    /// <summary>
    /// Validates and freezes a single stage's interface and ordered body.
    /// </summary>
    /// <param name="stage">
    /// Exactly Vertex, Fragment or Compute.
    /// </param>
    /// <param name="body">
    /// Immutable typed instructions and named outputs.
    /// </param>
    /// <param name="inputs">
    /// Exactly the input bindings read by the body.
    /// </param>
    /// <param name="outputs">
    /// Exactly the GPU destinations written by the body.
    /// </param>
    /// <param name="threadsX">
    /// Compute workgroup X size; one for raster stages.
    /// </param>
    /// <param name="threadsY">
    /// Compute workgroup Y size; one for raster stages.
    /// </param>
    /// <param name="threadsZ">
    /// Compute workgroup Z size; one for raster stages.
    /// </param>
    public ShaderIrStage(
        ShaderStage stage,
        ShaderIrBlock body,
        IEnumerable<ShaderIrStageInput> inputs,
        IEnumerable<ShaderIrStageOutput> outputs,
        int threadsX = 1,
        int threadsY = 1,
        int threadsZ = 1
    ) {
        ArgumentNullException.ThrowIfNull(body);
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(outputs);
        if (stage is not (ShaderStage.Vertex or ShaderStage.Fragment or ShaderStage.Compute))
        {
            throw new ArgumentException("A single programmable stage is required.", nameof(stage));
        }
        if (threadsX <= 0
            || threadsY <= 0
            || threadsZ <= 0
            || (stage != ShaderStage.Compute && (threadsX != 1 || threadsY != 1 || threadsZ != 1))
        ) {
            throw new ArgumentException("Workgroup dimensions must be positive and apply only to compute stages.");
        }
        this.inputs = Array.AsReadOnly(inputs.ToArray());
        this.outputs = Array.AsReadOnly(outputs.ToArray());
        this.stage = stage;
        this.body = body;
        this.threadsX = threadsX;
        this.threadsY = threadsY;
        this.threadsZ = threadsZ;
        Validate();
        this.contentHash = ShaderIrSemanticHash.Compute(this);
    }

    /// <summary>
    /// Gets the programmable stage.
    /// </summary>
    public ShaderStage stage { get; }

    /// <summary>
    /// Gets its immutable ordered instructions.
    /// </summary>
    public ShaderIrBlock body { get; }

    /// <summary>
    /// Gets all input bindings.
    /// </summary>
    public IReadOnlyList<ShaderIrStageInput> inputs { get; }

    /// <summary>
    /// Gets all output destinations.
    /// </summary>
    public IReadOnlyList<ShaderIrStageOutput> outputs { get; }

    /// <summary>
    /// Gets the compute workgroup X size.
    /// </summary>
    public int threadsX { get; }

    /// <summary>
    /// Gets the compute workgroup Y size.
    /// </summary>
    public int threadsY { get; }

    /// <summary>
    /// Gets the compute workgroup Z size.
    /// </summary>
    public int threadsZ { get; }

    /// <summary>
    /// Gets a deterministic hash of stage semantics, ordered effects, nested regions, complete types, resource layout and frozen sources.
    /// A compiled artifact cache must additionally include toolchain identity, target capabilities, optimization and variant configuration.
    /// </summary>
    public string contentHash { get; }

    private void Validate()
    {
        var bindings = new Dictionary<string, ShaderIrStageInput>(StringComparer.Ordinal);
        var occupied = new HashSet<(ShaderIrInputKind, string, int)>();
        foreach (ShaderIrStageInput input in inputs)
        {
            ArgumentNullException.ThrowIfNull(input);
            if (!bindings.TryAdd(input.id, input))
            {
                throw new ArgumentException($"Duplicate stage input '{input.id}'.");
            }
            if ((input.type.storage is not null) != (input.kind == ShaderIrInputKind.Storage))
            {
                throw new ArgumentException("Storage types require storage bindings, and storage bindings require complete storage types.");
            }
            string semantic = input.kind is ShaderIrInputKind.SampledTexture or ShaderIrInputKind.Storage ? string.Empty : input.semantic;
            if (input.kind != ShaderIrInputKind.Uniform && !occupied.Add((input.kind, semantic, input.location)))
            {
                throw new ArgumentException($"Two stage inputs alias the same {input.kind} location.");
            }
            if ((input.kind == ShaderIrInputKind.VertexAttribute && stage != ShaderStage.Vertex)
                || (input.kind == ShaderIrInputKind.Varying && stage != ShaderStage.Fragment)
            ) {
                throw new ArgumentException($"{input.kind} is not an input of the {stage} stage.");
            }
        }
        ShaderIrInstruction[] reads = body.instructions.Where(static value => value.operation == ShaderIrOperation.Input).ToArray();
        if (reads.Length != bindings.Count)
        {
            throw new ArgumentException("The stage interface must describe exactly every body input.");
        }
        foreach (ShaderIrInstruction read in reads)
        {
            if (!bindings.TryGetValue(read.inputName!, out ShaderIrStageInput? input) || !input.type.IsEquivalentTo(read.outputs[0].type))
            {
                throw new ArgumentException($"Stage input '{read.inputName}' has a missing or incompatible binding.");
            }
        }

        if (stage != ShaderStage.Fragment && RequiresFragment(body))
        {
            throw new ArgumentException("Implicit-derivative texture sampling and discard are fragment-only operations.");
        }

        var names = new HashSet<string>(StringComparer.Ordinal);
        var destinations = new HashSet<(ShaderIrOutputKind, string, int)>();
        foreach (ShaderIrStageOutput output in outputs)
        {
            ArgumentNullException.ThrowIfNull(output);
            if (!names.Add(output.id) || !destinations.Add((output.kind, output.semantic, output.location)))
            {
                throw new ArgumentException("Stage output identities and destinations must be unique.");
            }
            if (!body.outputs.TryGetValue(output.id, out ShaderIrValue? value))
            {
                throw new ArgumentException($"Missing body output '{output.id}'.");
            }
            bool vertexOutput = output.kind is ShaderIrOutputKind.ClipPosition or ShaderIrOutputKind.Varying;
            if (stage == ShaderStage.Compute || vertexOutput != (stage == ShaderStage.Vertex))
            {
                throw new ArgumentException($"{output.kind} is not an output of the {stage} stage.");
            }
            if (output.kind is ShaderIrOutputKind.ClipPosition or ShaderIrOutputKind.Color
                && !value.type.IsEquivalentTo(ShaderSourceType.Atomic("float4"))
            ) {
                throw new ArgumentException($"{output.kind} requires float4.");
            }
            if (output.kind == ShaderIrOutputKind.Depth && !value.type.IsEquivalentTo(ShaderSourceType.Atomic("float")))
            {
                throw new ArgumentException("Depth requires float.");
            }
            if (output.kind is ShaderIrOutputKind.Depth or ShaderIrOutputKind.ClipPosition
                && (output.location != 0 || output.semantic.Length != 0)
            ) {
                throw new ArgumentException("Position and depth have exactly one fixed destination.");
            }
            if (output.kind == ShaderIrOutputKind.Color && output.semantic.Length != 0)
            {
                throw new ArgumentException("Color attachments are addressed only by location.");
            }
        }
        if (names.Count != body.outputs.Count)
        {
            throw new ArgumentException("Every body output requires a GPU destination.");
        }
        if (stage == ShaderStage.Vertex && !outputs.Any(static value => value.kind == ShaderIrOutputKind.ClipPosition))
        {
            throw new ArgumentException("A vertex stage must write clip position.");
        }
    }

    private static bool RequiresFragment(ShaderIrBlock block)
        => block.instructions.Any(static instruction => (
            instruction.operation is ShaderIrOperation.TextureSample
            or ShaderIrOperation.Discard
            || instruction.regions.Any(RequiresFragment)
        ));
}

/// <summary>
/// Identifies the GPU interface through which a stage receives a value.
/// </summary>
public enum ShaderIrInputKind
{
    /// <summary>
    /// A vertex stream attribute, including instance-rate streams.
    /// </summary>
    VertexAttribute,
    /// <summary>
    /// An interpolated value written by the preceding raster stage.
    /// </summary>
    Varying,
    /// <summary>
    /// A material, frame or draw uniform identified by a stable binding identity.
    /// </summary>
    Uniform,
    /// <summary>
    /// A sampled texture binding whose slot is assigned by target resource layout.
    /// </summary>
    SampledTexture,
    /// <summary>
    /// A typed storage buffer or image whose access, shape and layout are explicit.
    /// </summary>
    Storage,
    /// <summary>
    /// A standard GPU stage input identified by a target semantic, not native source text.
    /// </summary>
    Builtin
}

/// <summary>
/// Identifies where a typed stage output is written.
/// </summary>
public enum ShaderIrOutputKind
{
    /// <summary>
    /// The vertex position in homogeneous clip space.
    /// </summary>
    ClipPosition,
    /// <summary>
    /// A value interpolated for the next raster stage.
    /// </summary>
    Varying,
    /// <summary>
    /// A fragment color attachment at its explicit location.
    /// </summary>
    Color,
    /// <summary>
    /// The fragment depth value.
    /// </summary>
    Depth
}
