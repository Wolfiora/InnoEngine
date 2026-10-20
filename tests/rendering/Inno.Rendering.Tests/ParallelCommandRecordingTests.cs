using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Inno.Rendering;
using Xunit;

namespace Inno.Rendering.Tests;

public sealed class ParallelCommandRecordingTests
{
    [Fact]
    public void EveryCommandReplaysTheSameArgumentsAfterBufferGrowthAndSourceReuse()
    {
        RenderGraphBuilder builder = CreateGraph();
        var texture = new RenderTextureDescriptor(32, 32, RenderTextureFormat.RGBA8,
            RenderTextureUsage.Sampled | RenderTextureUsage.ColorAttachment);
        var buffer = new RenderBufferDescriptor(32, 16, RenderBufferUsage.Vertex | RenderBufferUsage.Storage);
        var data = new CommandInputs
        {
            source = builder.ImportTexture("source", DeviceHandles.texture, texture),
            destination = builder.ImportTexture("destination", DeviceHandles.otherTexture, texture),
            buffer = builder.ImportBuffer("buffer", DeviceHandles.buffer, buffer),
            otherBuffer = builder.ImportBuffer("other-buffer", DeviceHandles.otherBuffer, buffer)
        };
        builder.AddRasterPass("Commands", new RenderPhaseId("tests.commands"), data,
            static (
                inputs,
                context
            ) => IssueCommands(inputs, context.commands))
            .AllowParallelRecording()
            .ReadTexture(data.source)
            .ReadTexture(data.destination)
            .ReadBuffer(data.buffer)
            .ReadBuffer(data.otherBuffer)
            .HasSideEffect();
        CompiledRenderGraph graph = builder.Compile().graph!;
        var expected = new ProbeEncoder();
        IssueCommands(data, expected);
        var backend = new ProbeBackend(string.Empty);

        graph.Execute(backend, 42);

        Assert.True(backend.ended);
        Assert.Equal(1986, expected.calls.Count);
        Assert.Equal(expected.calls.Count, backend.encoder.calls.Count);
        for (int index = 0; index < expected.calls.Count; index++)
        {
            Assert.Equal(expected.calls[index].operation, backend.encoder.calls[index].operation);
            Assert.Equal(expected.calls[index].arguments, backend.encoder.calls[index].arguments);
        }

        backend.encoder.calls.Clear();
        graph.Execute(backend, 43);
        Assert.Equal(expected.calls.Count, backend.encoder.calls.Count);
        for (int index = 0; index < expected.calls.Count; index++)
            Assert.Equal(expected.calls[index].arguments, backend.encoder.calls[index].arguments);
    }

    [Theory]
    [InlineData("success")]
    [InlineData("record")]
    [InlineData("begin-pass")]
    [InlineData("replay")]
    [InlineData("end-pass")]
    [InlineData("end-graph")]
    [InlineData("record,end-graph")]
    [InlineData("replay,end-pass,end-graph")]
    public void RecordedReferencesAreReleasedOnEveryExecutionExit(string phase)
    {
        RenderGraphBuilder builder = CreateGraph();
        var data = new ReferenceInputs(phase);
        builder.AddRasterPass("Lifetime", new RenderPhaseId("tests.lifetime"), data, RecordReferences)
            .AllowParallelRecording()
            .HasSideEffect();
        CompiledRenderGraph graph = builder.Compile().graph!;
        var backend = new ProbeBackend(phase, capture: false);
        if (phase == "success")
            graph.Execute(backend, 1);
        else
        {
            Exception failure = Assert.ThrowsAny<Exception>(() => graph.Execute(backend, 1));
            foreach (string expected in phase.Split(','))
                Assert.Contains(expected, failure.ToString(), StringComparison.Ordinal);
            if (phase.Contains(',', StringComparison.Ordinal))
                Assert.Equal(phase.Split(',').Length, Assert.IsType<AggregateException>(failure).Flatten().InnerExceptions.Count);
        }

        Assert.True(backend.ended);
        Assert.NotNull(data.reference);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(data.reference!.IsAlive);
        Assert.NotNull(data.borrowedCommands);
        GC.KeepAlive(graph);
        GC.KeepAlive(data.borrowedCommands);
    }

    private static RenderGraphBuilder CreateGraph() => new(1, new GraphicsCapabilities(
        GraphicsApi.Noop,
        GraphicsCapability.Compute | GraphicsCapability.StorageBuffer | GraphicsCapability.StorageTexture,
        new GraphicsLimits(16, 8, 16384, 16),
        [RenderTextureFormat.RGBA8], [RenderTextureFormat.RGBA8],
        [RenderTextureFormat.RGBA8], [RenderTextureFormat.RGBA8],
        originBottomLeft: false, homogeneousDepth: false));

    private static void IssueCommands(
        CommandInputs data,
        RenderCommandEncoder commands
    ) {
        var binding = new RenderBindingId("weights.\u03b1");
        for (int index = 0; index < 64; index++)
        {
            byte[] uniform = new byte[4097];
            for (int offset = 0; offset < uniform.Length; offset++)
                uniform[offset] = unchecked((byte)(offset + index));
            float[] transform = new float[16];
            for (int element = 0; element < transform.Length; element++)
                transform[element] = index + element * 0.25f;
            var sampler = RenderSamplerState.linearClamp;
            var sourceRegion = new RenderTextureRegion(0, 1, 2, 0, 3, 4);
            var destinationRegion = new RenderTextureRegion(0, 5, 6, 0, 3, 4);
            commands.BindGraphicsPipeline(DeviceHandles.graphics);
            commands.BindComputePipeline(DeviceHandles.compute);
            commands.BindTexture(binding, data.source, sampler);
            commands.BindTexture(binding, DeviceHandles.texture, sampler);
            commands.BindStorageTexture(binding, data.destination, 1);
            commands.BindStorageTexture(binding, DeviceHandles.texture, 2);
            commands.BindBuffer(binding, data.buffer);
            commands.BindBuffer(binding, DeviceHandles.buffer);
            commands.SetUniform(binding, uniform);
            Array.Fill(uniform, (byte)0);
            commands.SetTransform(transform);
            Array.Fill(transform, -1f);
            commands.SetRasterState(data.raster);
            commands.SetStencil(data.stencil);
            commands.SetViewport(1, 2, 31, 29);
            commands.SetScissor(3, 4, 23, 19);
            commands.BindVertexBuffer(data.buffer, 5);
            commands.BindVertexBuffer(DeviceHandles.buffer, 7);
            commands.BindIndexBuffer(data.otherBuffer, 11);
            commands.BindIndexBuffer(DeviceHandles.otherBuffer, 13);
            commands.BindInstanceBuffer(data.buffer, 17, 3);
            commands.BindInstanceBuffer(DeviceHandles.buffer, 19, 5);
            commands.Draw(23 + index, 7);
            commands.DrawProcedural(29, 11);
            commands.DrawIndexed(31, 13);
            commands.DrawIndirect(data.buffer, 2, 3);
            commands.DrawIndirect(DeviceHandles.buffer, 5, 7);
            commands.Dispatch(2, 3, 5);
            commands.DispatchIndirect(data.otherBuffer, 11, 13);
            commands.DispatchIndirect(DeviceHandles.otherBuffer, 17, 19);
            commands.CopyTexture(data.source, data.destination);
            commands.BlitTexture(data.source, sourceRegion, data.destination, destinationRegion);
            commands.CopyBuffer(data.buffer, data.otherBuffer);
        }
        commands.SetUniform(default, []);
        commands.SetTransform([]);
    }

    private static void RecordReferences(
        ReferenceInputs data,
        RenderPassContext context
    ) {
        var state = new RenderRasterState();
        data.reference = new WeakReference(state);
        data.borrowedCommands = context.commands;
        context.commands.SetRasterState(state);
        for (int index = 0; index < 32; index++)
            context.commands.SetUniform(new RenderBindingId("lifetime"), new byte[257]);
        if (data.phase.Contains("record", StringComparison.Ordinal))
            throw new InvalidOperationException("record");
    }

    private sealed class CommandInputs
    {
        internal RenderTextureHandle source;
        internal RenderTextureHandle destination;
        internal RenderBufferHandle buffer;
        internal RenderBufferHandle otherBuffer;
        internal readonly RenderRasterState raster = new();
        internal readonly RenderStencilState stencil = new();
    }

    private sealed class ReferenceInputs(string phase)
    {
        internal readonly string phase = phase;
        internal WeakReference? reference;
        internal RenderCommandEncoder? borrowedCommands;
    }

    private sealed class DeviceHandles : RenderDevice
    {
        internal static GraphicsPipelineHandle graphics => CreateGraphicsPipelineHandle(0x123456789, 7);
        internal static ComputePipelineHandle compute => CreateComputePipelineHandle(0x987654321, 7);
        internal static PersistentTextureHandle texture => CreatePersistentTextureHandle(0x123456781, 7);
        internal static PersistentTextureHandle otherTexture => CreatePersistentTextureHandle(0x123456782, 7);
        internal static PersistentBufferHandle buffer => CreatePersistentBufferHandle(0x123456783, 7);
        internal static PersistentBufferHandle otherBuffer => CreatePersistentBufferHandle(0x123456784, 7);
    }

    private sealed class ProbeBackend : IRenderGraphBackend
    {
        private readonly string m_phase;
        internal readonly ProbeEncoder encoder;
        internal bool ended;

        internal ProbeBackend(
            string phase,
            bool capture = true
        ) {
            m_phase = phase;
            encoder = new ProbeEncoder(phase, capture);
        }

        public void BeginGraph(CompiledRenderGraph graph) { }

        public RenderCommandEncoder BeginPass(CompiledRenderPass pass)
        {
            if (m_phase == "begin-pass")
                throw new InvalidOperationException(m_phase);
            return encoder;
        }

        public void EndPass(CompiledRenderPass pass)
        {
            if (m_phase.Contains("end-pass", StringComparison.Ordinal))
                throw new InvalidOperationException("end-pass");
        }

        public void EndGraph(CompiledRenderGraph graph)
        {
            ended = true;
            if (m_phase.Contains("end-graph", StringComparison.Ordinal))
                throw new InvalidOperationException("end-graph");
        }
    }

    private sealed class ProbeEncoder : RenderCommandEncoder
    {
        private readonly string m_phase;
        private readonly bool m_capture;

        internal readonly List<ObservedCommand> calls = [];

        internal ProbeEncoder(
            string phase = "",
            bool capture = true
        ) {
            m_phase = phase;
            m_capture = capture;
        }

        public override void BindGraphicsPipeline(GraphicsPipelineHandle pipeline) => Record("BindGraphicsPipeline", pipeline);

        public override void BindComputePipeline(ComputePipelineHandle pipeline) => Record("BindComputePipeline", pipeline);

        public override void BindTexture(
            RenderBindingId binding,
            RenderTextureHandle texture,
            RenderSamplerState sampler
        ) => Record("BindTextureGraph", binding, texture, sampler);

        public override void BindTexture(
            RenderBindingId binding,
            PersistentTextureHandle texture,
            RenderSamplerState sampler
        ) => Record("BindTexturePersistent", binding, texture, sampler);

        public override void BindStorageTexture(
            RenderBindingId binding,
            RenderTextureHandle texture,
            int mipLevel = 0
        ) => Record("BindStorageTextureGraph", binding, texture, mipLevel);

        public override void BindStorageTexture(
            RenderBindingId binding,
            PersistentTextureHandle texture,
            int mipLevel = 0
        ) => Record("BindStorageTexturePersistent", binding, texture, mipLevel);

        public override void BindBuffer(
            RenderBindingId binding,
            RenderBufferHandle buffer
        ) => Record("BindBufferGraph", binding, buffer);

        public override void BindBuffer(
            RenderBindingId binding,
            PersistentBufferHandle buffer
        ) => Record("BindBufferPersistent", binding, buffer);

        public override void SetUniform(
            RenderBindingId binding,
            ReadOnlySpan<byte> value
        ) => Record("SetUniform", binding, Convert.ToHexString(value));

        public override void SetTransform(ReadOnlySpan<float> columnMajorMatrix) => Record("SetTransform", Convert.ToHexString(MemoryMarshal.AsBytes(columnMajorMatrix)));

        public override void SetRasterState(RenderRasterState state) => Record("SetRasterState", state);

        public override void SetStencil(RenderStencilState state) => Record("SetStencil", state);

        public override void SetViewport(
            int x,
            int y,
            int width,
            int height
        ) => Record("SetViewport", x, y, width, height);

        public override void SetScissor(
            int x,
            int y,
            int width,
            int height
        ) => Record("SetScissor", x, y, width, height);

        public override void BindVertexBuffer(
            RenderBufferHandle buffer,
            int firstVertex = 0
        ) => Record("BindVertexBufferGraph", buffer, firstVertex);

        public override void BindVertexBuffer(
            PersistentBufferHandle buffer,
            int firstVertex = 0
        ) => Record("BindVertexBufferPersistent", buffer, firstVertex);

        public override void BindIndexBuffer(
            RenderBufferHandle buffer,
            int firstIndex = 0
        ) => Record("BindIndexBufferGraph", buffer, firstIndex);

        public override void BindIndexBuffer(
            PersistentBufferHandle buffer,
            int firstIndex = 0
        ) => Record("BindIndexBufferPersistent", buffer, firstIndex);

        public override void BindInstanceBuffer(
            RenderBufferHandle buffer,
            int firstInstance,
            int instanceCount
        ) => Record("BindInstanceBufferGraph", buffer, firstInstance, instanceCount);

        public override void BindInstanceBuffer(
            PersistentBufferHandle buffer,
            int firstInstance,
            int instanceCount
        ) => Record("BindInstanceBufferPersistent", buffer, firstInstance, instanceCount);

        public override void Draw(
            int vertexCount,
            int instanceCount = 1
        ) => Record("Draw", vertexCount, instanceCount);

        public override void DrawProcedural(
            int vertexCount,
            int instanceCount = 1
        ) => Record("DrawProcedural", vertexCount, instanceCount);

        public override void DrawIndexed(
            int indexCount,
            int instanceCount = 1
        ) => Record("DrawIndexed", indexCount, instanceCount);

        public override void DrawIndirect(
            RenderBufferHandle buffer,
            int firstCommand = 0,
            int commandCount = 1
        ) => Record("DrawIndirectGraph", buffer, firstCommand, commandCount);

        public override void DrawIndirect(
            PersistentBufferHandle buffer,
            int firstCommand = 0,
            int commandCount = 1
        ) => Record("DrawIndirectPersistent", buffer, firstCommand, commandCount);

        public override void Dispatch(
            int groupCountX,
            int groupCountY = 1,
            int groupCountZ = 1
        ) => Record("Dispatch", groupCountX, groupCountY, groupCountZ);

        public override void DispatchIndirect(
            RenderBufferHandle buffer,
            int firstCommand = 0,
            int commandCount = 1
        ) => Record("DispatchIndirectGraph", buffer, firstCommand, commandCount);

        public override void DispatchIndirect(
            PersistentBufferHandle buffer,
            int firstCommand = 0,
            int commandCount = 1
        ) => Record("DispatchIndirectPersistent", buffer, firstCommand, commandCount);

        public override void CopyTexture(
            RenderTextureHandle source,
            RenderTextureHandle destination
        ) => Record("CopyTextureGraph", source, destination);

        public override void BlitTexture(
            RenderTextureHandle source,
            RenderTextureRegion sourceRegion,
            RenderTextureHandle destination,
            RenderTextureRegion destinationRegion
        ) => Record("BlitTextureGraph", source, sourceRegion, destination, destinationRegion);

        public override void CopyBuffer(
            RenderBufferHandle source,
            RenderBufferHandle destination
        ) => Record("CopyBufferGraph", source, destination);

        private void Record(
            string operation,
            params object?[] arguments
        ) {
            if (m_phase.Contains("replay", StringComparison.Ordinal))
                throw new InvalidOperationException("replay");
            if (m_capture)
                calls.Add(new ObservedCommand(operation, arguments));
        }
    }

    private sealed record ObservedCommand(
        string operation,
        object?[] arguments
    );
}
