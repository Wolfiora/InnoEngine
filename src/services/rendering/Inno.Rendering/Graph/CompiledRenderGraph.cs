using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Inno.Rendering;

/// <summary>
/// Contains validated, culled and topologically scheduled frame work.
/// </summary>
public sealed class CompiledRenderGraph
{
    private readonly IReadOnlyList<CompiledRenderPass> m_passes;
    private readonly CompiledRenderPass[] m_parallelPasses;
    private readonly IReadOnlyList<CompiledRenderTexture> m_textures;
    private readonly IReadOnlyList<CompiledRenderBuffer> m_buffers;

    internal CompiledRenderGraph(
        uint generation,
        IReadOnlyList<CompiledRenderPass> passes,
        IReadOnlyList<CompiledRenderTexture> textures,
        IReadOnlyList<CompiledRenderBuffer> buffers
    ) {
        this.generation = generation;
        m_passes = Array.AsReadOnly(passes.ToArray());
        m_parallelPasses = m_passes.Where(static pass => pass.recordingMode == RenderPassRecordingMode.Parallel).ToArray();
        m_textures = Array.AsReadOnly(textures.ToArray());
        m_buffers = Array.AsReadOnly(buffers.ToArray());
    }

    /// <summary>
    /// Gets the source frame generation.
    /// </summary>
    public uint generation { get; }

    /// <summary>
    /// Gets scheduled passes in backend view order.
    /// </summary>
    public IReadOnlyList<CompiledRenderPass> passes => m_passes;

    /// <summary>
    /// Gets compiled logical texture allocations.
    /// </summary>
    public IReadOnlyList<CompiledRenderTexture> textures => m_textures;

    /// <summary>
    /// Gets compiled logical buffer allocations.
    /// </summary>
    public IReadOnlyList<CompiledRenderBuffer> buffers => m_buffers;

    /// <summary>
    /// Executes pass callbacks through a concrete backend with complete unwind.
    /// </summary>
    /// <param name="backend">
    /// Backend bridge for the current device generation.
    /// </param>
    /// <param name="frameIndex">
    /// Monotonic frame index exposed to pass callbacks.
    /// </param>
    /// <exception cref="AggregateException">
    /// Thrown when execution and cleanup both fail.
    /// </exception>
    public void Execute(
        IRenderGraphBackend backend,
        ulong frameIndex
    ) {
        ArgumentNullException.ThrowIfNull(backend);
        List<Exception>? errors = null;
        Dictionary<CompiledRenderPass, RecordedRenderCommandEncoder>? recorded = null;
        bool graphStarted = false;

        try
        {
            backend.BeginGraph(this);
            graphStarted = true;
            if (m_parallelPasses.Length != 0)
            {
                recorded = RecordParallelPasses(frameIndex);
            }
            foreach (CompiledRenderPass pass in m_passes)
            {
                bool passStarted = false;
                Exception? passFailure = null;
                try
                {
                    RenderCommandEncoder commands = backend.BeginPass(pass);
                    ArgumentNullException.ThrowIfNull(commands);
                    commands.SetFrameIndex(frameIndex);
                    passStarted = true;
                    if (recorded is not null && recorded.TryGetValue(pass, out RecordedRenderCommandEncoder? commandList))
                    {
                        commandList.Replay(commands);
                    }
                    else
                    {
                        pass.Execute(new RenderPassContext(commands, frameIndex));
                    }
                }
                catch (Exception exception)
                {
                    passFailure = exception;
                }
                finally
                {
                    if (passStarted)
                    {
                        try
                        {
                            backend.EndPass(pass);
                        }
                        catch (Exception exception)
                        {
                            passFailure = passFailure is null
                                ? exception
                                : new AggregateException("Render pass execution and cleanup both failed.", passFailure, exception);
                        }
                    }
                }
                if (passFailure is not null)
                {
                    ExceptionDispatchInfo.Capture(passFailure).Throw();
                }
            }
        }
        catch (Exception exception)
        {
            errors = [exception];
        }
        finally
        {
            if (graphStarted)
            {
                try
                {
                    backend.EndGraph(this);
                }
                catch (Exception exception)
                {
                    errors ??= [];
                    errors.Add(exception);
                }
            }
            if (recorded is not null)
            {
                foreach (RecordedRenderCommandEncoder commands in recorded.Values)
                {
                    commands.Dispose();
                }
            }
        }

        if (errors is null)
        {
            return;
        }

        if (errors.Count == 1)
        {
            ExceptionDispatchInfo.Capture(errors[0]).Throw();
        }

        throw new AggregateException("Render graph execution failed during recording or cleanup.", errors);
    }

    private Dictionary<CompiledRenderPass, RecordedRenderCommandEncoder> RecordParallelPasses(ulong frameIndex)
    {
        var recorded = new ConcurrentDictionary<CompiledRenderPass, RecordedRenderCommandEncoder>();
        try
        {
            Parallel.ForEach(
                m_parallelPasses,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Math.Max(1, Math.Min(Environment.ProcessorCount, m_parallelPasses.Length))
                },
                pass =>
                {
                    var commands = new RecordedRenderCommandEncoder();
                    try
                    {
                        commands.SetFrameIndex(frameIndex);
                        pass.Execute(new RenderPassContext(commands, frameIndex));
                        commands.Seal();
                        if (!recorded.TryAdd(pass, commands))
                        {
                            throw new InvalidOperationException("A parallel render pass was recorded more than once.");
                        }
                    }
                    catch
                    {
                        commands.Dispose();
                        throw;
                    }
                }
            );
            return recorded.ToDictionary(static pair => pair.Key, static pair => pair.Value);
        }
        catch
        {
            foreach (RecordedRenderCommandEncoder commands in recorded.Values)
            {
                commands.Dispose();
            }
            throw;
        }
    }
}
