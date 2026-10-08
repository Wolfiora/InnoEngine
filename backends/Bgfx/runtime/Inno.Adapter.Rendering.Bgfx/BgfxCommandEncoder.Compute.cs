using System;
using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed unsafe partial class BgfxCommandEncoder

{

    /// <summary>
    /// Dispatches compute work using the supplied thread-group dimensions.
    /// </summary>
    /// <param name="groupCountX">
    /// The group count x consumed by dispatch; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="groupCountY">
    /// The group count y consumed by dispatch; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="groupCountZ">
    /// The group count z consumed by dispatch; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void Dispatch(
        int groupCountX,
        int groupCountY = 1,
        int groupCountZ = 1
    ) {
        BgfxPipelineResource pipeline = RequireComputePipeline();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(groupCountX);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(groupCountY);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(groupCountZ);
        bgfx.encoder_dispatch(
            m_encoder,
            m_viewId,
            pipeline.program,
            checked((uint)groupCountX),
            checked((uint)groupCountY),
            checked((uint)groupCountZ),
            checked((byte)bgfx.DiscardFlags.All));
        m_device.RecordDispatch();
    }

    /// <summary>
    /// Dispatches compute work using dimensions read from the supplied indirect buffer.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by dispatch indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstCommand">
    /// The first command consumed by dispatch indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="commandCount">
    /// The command count consumed by dispatch indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void DispatchIndirect(
        RenderBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    )
        => DispatchIndirect(m_device.ResolveBuffer(buffer), firstCommand, commandCount);

    /// <summary>
    /// Dispatches compute work using dimensions read from the supplied indirect buffer.
    /// </summary>
    /// <param name="buffer">
    /// The buffer consumed by dispatch indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="firstCommand">
    /// The first command consumed by dispatch indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="commandCount">
    /// The command count consumed by dispatch indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public override void DispatchIndirect(
        PersistentBufferHandle buffer,
        int firstCommand = 0,
        int commandCount = 1
    )
        => DispatchIndirect(m_device.ResolveBuffer(buffer), firstCommand, commandCount);

    private void DispatchIndirect(
        BgfxBufferResource buffer,
        int firstCommand,
        int commandCount
    ) {
        BgfxPipelineResource pipeline = RequireComputePipeline();
        ValidateIndirect(buffer, firstCommand, commandCount);
        bgfx.encoder_dispatch_indirect(
            m_encoder,
            m_viewId,
            pipeline.program,
            new bgfx.IndirectBufferHandle { idx = buffer.nativeIndex },
            checked((uint)firstCommand),
            checked((uint)commandCount),
            checked((byte)bgfx.DiscardFlags.All));
        m_device.RecordDispatch(commandCount);
    }

}
