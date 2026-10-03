using System;
using System.Threading;

using Inno.Assets.Pipeline;

namespace Inno.Editor.Scripting.Tests;

[AssetSampleSourceRewriter("tests.scripting.controlled-sample")]
internal sealed class ControlledSampleSourceRewriter : IAssetSampleSourceRewriter
{
    private static readonly AsyncLocal<Control?> m_current = new();
    private readonly Control? m_control = m_current.Value;

    /// <summary>
    /// Captures the current test-owned control through the real source rewriter discovery contract.
    /// </summary>
    public ControlledSampleSourceRewriter()
    {
    }

    internal static Control? current { get => m_current.Value; set => m_current.Value = value; }

    /// <inheritdoc />
    public void Transform(AssetSampleTransformContext context)
    {
        if (m_control is null)
            return;
        m_control.workerThread = Environment.CurrentManagedThreadId;
        m_control.started.Set();
        using CancellationTokenRegistration cancellation = context.cancellationToken.Register(m_control.canceled.Set);
        if (!m_control.release.Wait(TimeSpan.FromSeconds(15)))
            throw new TimeoutException("The test-owned source rewriter was not released.");
        context.cancellationToken.ThrowIfCancellationRequested();
    }

    internal sealed class Control : IDisposable
    {
        internal ManualResetEventSlim started { get; } = new();
        internal ManualResetEventSlim release { get; } = new();
        internal ManualResetEventSlim canceled { get; } = new();
        internal int workerThread { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            started.Dispose();
            release.Dispose();
            canceled.Dispose();
        }
    }
}
