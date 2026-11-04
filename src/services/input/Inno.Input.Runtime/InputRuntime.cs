using System;

using Inno.Runtime;
using Inno.Runtime.Contracts;

namespace Inno.Input.Runtime;

/// <summary>
/// Captures one immutable input snapshot and binds it for the complete runtime frame.
/// </summary>
public sealed class InputRuntime : RuntimeSubsystem, IInputService
{
    private readonly IInputBackend m_backend;
    private int m_textOwnerThread;
    private bool m_stopping;

    /// <summary>
    /// Creates a feature whose backend ownership transfers to this instance.
    /// </summary>
    /// <param name="backend">
    /// The platform backend captured at frame boundaries.
    /// </param>
    public InputRuntime(IInputBackend backend)
    {
        m_backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    /// <summary>
    /// Gets the immutable input state captured at the beginning of the current frame.
    /// </summary>
    public InputSnapshot snapshot { get; private set; } = InputSnapshot.empty;

    /// <inheritdoc />
    public IInputTextSession? TryBeginTextInput()
    {
        ObjectDisposedException.ThrowIf(m_stopping, this);
        if (!isStarted)
            throw new InvalidOperationException("Text input requires an attached runtime input subsystem.");
        if (Environment.CurrentManagedThreadId != m_textOwnerThread)
            throw new InvalidOperationException("Text input requires the runtime input owner thread.");
        return (m_backend as IInputTextBackend)?.TryBeginTextInput();
    }

    /// <summary>
    /// Binds text-input admission to the subsystem's attached control thread.
    /// </summary>
    protected override void OnStart() => m_textOwnerThread = Environment.CurrentManagedThreadId;

    /// <summary>
    /// Captures pending backend input and binds the resulting snapshot to the runtime frame.
    /// </summary>
    /// <param name="frame">
    /// The immutable frame state whose index labels the captured snapshot.
    /// </param>
    protected override void OnBeginFrame(RuntimeFrame frame)
    {
        snapshot = m_backend.Capture(frame.frameIndex);
        OwnFrameScope(InputExecutionContext.EnterScope(this));
    }

    /// <summary>
    /// Releases the input backend after its frame scope and owned work have quiesced.
    /// </summary>
    protected override void OnStop()
    {
        m_stopping = true;
        m_backend.Dispose();
    }
}
