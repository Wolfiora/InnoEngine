using System;
using System.Collections.Generic;

using Inno.Scripting.Api;

namespace Inno.Rendering;

/// <summary>
/// Builds one generation-scoped render graph from explicit passes and resources.
/// </summary>
public sealed partial class RenderGraphBuilder
{
    private readonly GraphicsCapabilities m_capabilities;
    private readonly uint m_generation;
    private readonly List<RenderTextureRecord> m_textures = [];
    private readonly List<RenderBufferRecord> m_buffers = [];
    private readonly List<RenderPassRecord> m_passes = [];
    private readonly HashSet<RenderResourceKey> m_outputs = [];
    private readonly List<(long id, string name)> m_nameScopes = [];
    private readonly List<RenderGraphMutationScope> m_mutations = [];
    private readonly List<RenderResourceKey> m_addedOutputs = [];
    private RenderGraphValidationState? m_validation;
    private long m_revision;
    private long m_validatedRevision = -1;
    private long m_nextNameScopeId;
    private ulong m_nextResourceId;
    private int m_frozenPassCount;
    private bool m_compiled;

    /// <summary>
    /// Creates a render graph builder for one frame generation.
    /// </summary>
    /// <param name="generation">
    /// Non-zero frame-scoped generation.
    /// </param>
    /// <param name="capabilities">
    /// Device capabilities used during validation.
    /// </param>
    public RenderGraphBuilder(
        uint generation,
        GraphicsCapabilities capabilities
    ) {
        if (generation == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(generation), "Generation must be non-zero.");
        }

        ArgumentNullException.ThrowIfNull(capabilities);
        m_generation = generation;
        m_capabilities = capabilities;
    }

    /// <summary>
    /// Creates a transient texture eligible for lifetime aliasing.
    /// </summary>
    /// <param name="name">
    /// Debug and diagnostic name.
    /// </param>
    /// <param name="descriptor">
    /// Texture requirements.
    /// </param>
    /// <returns>
    /// A handle valid only for this graph generation.
    /// </returns>
    public RenderTextureHandle CreateTexture(
        string name,
        RenderTextureDescriptor descriptor
    ) {
        EnsureMutating();
        ValidateNameAndValue(name, descriptor);
        RenderTextureHandle handle = new(m_textures.Count, m_generation, checked(++m_nextResourceId));
        m_textures.Add(new RenderTextureRecord(QualifyName(name), descriptor, false, default, handle.allocationId));
        InvalidateValidation();
        return handle;
    }

    /// <summary>
    /// Imports a persistent device texture into the current graph.
    /// </summary>
    /// <param name="name">
    /// Debug and diagnostic name.
    /// </param>
    /// <param name="texture">
    /// Persistent texture owned by the active device generation.
    /// </param>
    /// <param name="descriptor">
    /// Texture requirements and usage.
    /// </param>
    /// <returns>
    /// A frame-scoped handle for graph declarations.
    /// </returns>
    public RenderTextureHandle ImportTexture(
        string name,
        PersistentTextureHandle texture,
        RenderTextureDescriptor descriptor
    ) {
        EnsureMutating();
        ValidateNameAndValue(name, descriptor);
        if (!texture.isValid)
        {
            throw new ArgumentException("Persistent texture handle is invalid.", nameof(texture));
        }

        RenderTextureHandle handle = new(m_textures.Count, m_generation, checked(++m_nextResourceId));
        m_textures.Add(new RenderTextureRecord(QualifyName(name), descriptor, true, texture, handle.allocationId));
        InvalidateValidation();
        return handle;
    }

    /// <summary>
    /// Creates a transient buffer eligible for lifetime aliasing.
    /// </summary>
    /// <param name="name">
    /// Debug and diagnostic name.
    /// </param>
    /// <param name="descriptor">
    /// Buffer requirements.
    /// </param>
    /// <returns>
    /// A handle valid only for this graph generation.
    /// </returns>
    public RenderBufferHandle CreateBuffer(
        string name,
        RenderBufferDescriptor descriptor
    ) {
        EnsureMutating();
        ValidateNameAndValue(name, descriptor);
        RenderBufferHandle handle = new(m_buffers.Count, m_generation, checked(++m_nextResourceId));
        m_buffers.Add(new RenderBufferRecord(QualifyName(name), descriptor, false, default, handle.allocationId));
        InvalidateValidation();
        return handle;
    }

    /// <summary>
    /// Imports a persistent device buffer into the current graph.
    /// </summary>
    /// <param name="name">
    /// Debug and diagnostic name.
    /// </param>
    /// <param name="buffer">
    /// Persistent buffer owned by the active device generation.
    /// </param>
    /// <param name="descriptor">
    /// Buffer requirements and usage.
    /// </param>
    /// <returns>
    /// A frame-scoped handle for graph declarations.
    /// </returns>
    public RenderBufferHandle ImportBuffer(
        string name,
        PersistentBufferHandle buffer,
        RenderBufferDescriptor descriptor
    ) {
        EnsureMutating();
        ValidateNameAndValue(name, descriptor);
        if (!buffer.isValid)
        {
            throw new ArgumentException("Persistent buffer handle is invalid.", nameof(buffer));
        }

        RenderBufferHandle handle = new(m_buffers.Count, m_generation, checked(++m_nextResourceId));
        m_buffers.Add(new RenderBufferRecord(QualifyName(name), descriptor, true, buffer, handle.allocationId));
        InvalidateValidation();
        return handle;
    }

    /// <summary>
    /// Marks a texture as a graph output that keeps its producers alive.
    /// </summary>
    /// <param name="texture">
    /// Output texture.
    /// </param>
    public void MarkOutput(RenderTextureHandle texture)
    {
        EnsureTexture(texture);
        MarkOutput(new RenderResourceKey(true, texture.index));
    }

    /// <summary>
    /// Marks a buffer as a graph output that keeps its producers alive.
    /// </summary>
    /// <param name="buffer">
    /// Output buffer.
    /// </param>
    public void MarkOutput(RenderBufferHandle buffer)
    {
        EnsureBuffer(buffer);
        MarkOutput(new RenderResourceKey(false, buffer.index));
    }

    /// <summary>
    /// Begins an isolated graph mutation that rolls back every added pass and resource unless committed.
    /// </summary>
    /// <returns>
    /// A mutation scope used by reloadable features and other failure-isolated producers.
    /// </returns>
    public RenderGraphMutationScope BeginMutationScope()
    {
        EnsureMutating();
        m_frozenPassCount = m_passes.Count;
        var scope = new RenderGraphMutationScope(
            this, m_textures.Count, m_buffers.Count, m_passes.Count, m_addedOutputs.Count, m_nameScopes.Count,
            m_validatedRevision == m_revision ? m_validation : null);
        m_mutations.Add(scope);
        return scope;
    }

    /// <summary>
    /// Prefixes pass and resource diagnostic names until the returned scope is disposed.
    /// </summary>
    /// <param name="name">
    /// Non-empty scope segment.
    /// </param>
    /// <returns>
    /// A disposable name scope that must be closed in nesting order.
    /// </returns>
    public RenderGraphNameScope BeginNameScope(string name)
    {
        EnsureMutating();
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        long id = checked(++m_nextNameScopeId);
        m_nameScopes.Add((id, name.Trim()));
        return new RenderGraphNameScope(this, id);
    }

    /// <summary>
    /// Adds a frame-scoped raster pass.
    /// </summary>
    /// <typeparam name="TPassData">
    /// Pass payload type.
    /// </typeparam>
    /// <param name="name">
    /// Unique diagnostic name.
    /// </param>
    /// <param name="phase">
    /// Open render phase identifier.
    /// </param>
    /// <param name="passData">
    /// Payload that lives only until this graph executes.
    /// </param>
    /// <param name="execute">
    /// Command recording callback.
    /// </param>
    /// <returns>
    /// A builder for raster resources, attachments and ordering.
    /// </returns>
    public RasterPassBuilder AddRasterPass<TPassData>(
        string name,
        RenderPhaseId phase,
        TPassData passData,
        RenderPassExecute<TPassData> execute
    )
        where TPassData : notnull
        => new(this, AddPass(name, phase, RenderPassKind.Raster, passData, execute));

    /// <summary>
    /// Adds a frame-scoped compute pass.
    /// </summary>
    /// <typeparam name="TPassData">
    /// Pass payload type.
    /// </typeparam>
    /// <param name="name">
    /// Unique diagnostic name.
    /// </param>
    /// <param name="phase">
    /// Open render phase identifier.
    /// </param>
    /// <param name="passData">
    /// Payload that lives only until this graph executes.
    /// </param>
    /// <param name="execute">
    /// Command recording callback.
    /// </param>
    /// <returns>
    /// A builder for compute resources and ordering.
    /// </returns>
    public ComputePassBuilder AddComputePass<TPassData>(
        string name,
        RenderPhaseId phase,
        TPassData passData,
        RenderPassExecute<TPassData> execute
    )
        where TPassData : notnull
        => new(this, AddPass(name, phase, RenderPassKind.Compute, passData, execute));

    /// <summary>
    /// Adds a frame-scoped resource copy pass.
    /// </summary>
    /// <typeparam name="TPassData">
    /// Pass payload type.
    /// </typeparam>
    /// <param name="name">
    /// Unique diagnostic name.
    /// </param>
    /// <param name="phase">
    /// Open render phase identifier.
    /// </param>
    /// <param name="passData">
    /// Payload that lives only until this graph executes.
    /// </param>
    /// <param name="execute">
    /// Command recording callback.
    /// </param>
    /// <returns>
    /// A builder for copy resources and ordering.
    /// </returns>
    public CopyPassBuilder AddCopyPass<TPassData>(
        string name,
        RenderPhaseId phase,
        TPassData passData,
        RenderPassExecute<TPassData> execute
    )
        where TPassData : notnull
        => new(this, AddPass(name, phase, RenderPassKind.Copy, passData, execute));

    /// <summary>
    /// Validates, culls and schedules this graph exactly once.
    /// </summary>
    /// <returns>
    /// A compilation result containing either an executable graph or diagnostics.
    /// </returns>
    [ScriptingApiIgnore]
    public RenderGraphCompileResult Compile()
    {
        EnsureBuilding();
        if (m_mutations.Count != 0)
            throw new InvalidOperationException("End all graph mutation scopes before final compilation.");
        RenderGraphValidationState validation = GetValidation();
        m_compiled = true;
        return RenderGraphCompiler.Compile(m_generation, m_textures, m_buffers, m_passes, validation);
    }

    /// <summary>
    /// Validates current declarations and caches their analysis without constructing an executable graph.
    /// </summary>
    /// <returns>
    /// A frozen result for the current revision; subsequent mutations invalidate the cached analysis.
    /// </returns>
    [ScriptingApiIgnore]
    public RenderGraphValidationResult Validate()
    {
        EnsureBuilding();
        return GetValidation().result;
    }

    internal void EndNameScope(long id)
    {
        int index = m_nameScopes.FindIndex(scope => scope.id == id);
        if (index < 0)
            return;
        if (index != m_nameScopes.Count - 1)
            throw new InvalidOperationException("Render graph name scopes must be disposed in nesting order.");
        if (m_mutations.Count != 0 && index < m_mutations[^1].nameScopeCount)
            throw new InvalidOperationException("A mutation cannot close a name scope owned by an earlier declaration.");
        m_nameScopes.RemoveAt(index);
    }

    internal void AddUse(
        RenderPassRecord pass,
        RenderTextureHandle texture,
        RenderResourceAccess access,
        RenderResourceUseKind kind
    ) {
        EnsureTexture(texture);
        EnsurePassMutable(pass);
        pass.resources.Add(new RenderResourceUse(new RenderResourceKey(true, texture.index), access, kind));
    }

    internal void AddUse(
        RenderPassRecord pass,
        RenderBufferHandle buffer,
        RenderResourceAccess access,
        RenderResourceUseKind kind
    ) {
        EnsureBuffer(buffer);
        EnsurePassMutable(pass);
        pass.resources.Add(new RenderResourceUse(new RenderResourceKey(false, buffer.index), access, kind));
    }

    internal void AddAttachment(
        RenderPassRecord pass,
        RenderAttachment attachment
    ) {
        EnsureTexture(attachment.texture);
        EnsurePassMutable(pass);
        pass.attachments.Add(attachment);
        AddUse(
            pass,
            attachment.texture,
            attachment.loadAction == RenderLoadAction.Load
                ? RenderResourceAccess.ReadWrite
                : RenderResourceAccess.Write,
            attachment.isDepth
                ? RenderResourceUseKind.DepthStencilAttachment
                : RenderResourceUseKind.ColorAttachment);
    }

    private RenderPassRecord AddPass<TPassData>(
        string name,
        RenderPhaseId phase,
        RenderPassKind kind,
        TPassData passData,
        RenderPassExecute<TPassData> execute
    )
        where TPassData : notnull
    {
        EnsureMutating();
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(execute);
        string qualifiedName = QualifyName(name);
        if (m_passes.Exists(pass => StringComparer.Ordinal.Equals(pass.name, qualifiedName)))
        {
            throw new ArgumentException($"Render pass '{qualifiedName}' already exists.", nameof(name));
        }

        RenderPassRecord pass = new()
        {
            index = m_passes.Count,
            name = qualifiedName,
            phase = phase,
            kind = kind,
            execute = context => execute(passData, context)
        };
        m_passes.Add(pass);
        InvalidateValidation();
        return pass;
    }

    private void EnsureTexture(RenderTextureHandle handle)
    {
        EnsureMutating();
        if (handle.generation != m_generation || handle.index < 0 || handle.index >= m_textures.Count
            || handle.allocationId != m_textures[handle.index].allocationId)
        {
            throw new ArgumentException("Texture handle does not belong to this render graph generation.", nameof(handle));
        }
    }

    private void EnsureBuffer(RenderBufferHandle handle)
    {
        EnsureMutating();
        if (handle.generation != m_generation || handle.index < 0 || handle.index >= m_buffers.Count
            || handle.allocationId != m_buffers[handle.index].allocationId)
        {
            throw new ArgumentException("Buffer handle does not belong to this render graph generation.", nameof(handle));
        }
    }

    private void EnsureBuilding()
    {
        if (m_compiled)
        {
            throw new InvalidOperationException("The render graph has already been compiled.");
        }
    }

    private string QualifyName(string name)
        => m_nameScopes.Count == 0
            ? name
            : $"{string.Join('/', m_nameScopes.ConvertAll(static scope => scope.name))}/{name}";

    private static void ValidateNameAndValue<T>(
        string name,
        T value
    )
        where T : class
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(value);
    }
}
