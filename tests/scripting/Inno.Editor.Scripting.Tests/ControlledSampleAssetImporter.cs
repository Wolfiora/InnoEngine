using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets;
using Inno.Assets.Pipeline;

namespace Inno.Editor.Scripting.Tests;

[AssetImporter("tests.scripting.controlled-sample-asset")]
internal sealed class ControlledSampleAssetImporter : AssetImporter<TextAsset>
{
    private static readonly AsyncLocal<Control?> CURRENT = new();

    /// <summary>
    /// Creates a discoverable importer that uses only test-owned synchronization state.
    /// </summary>
    public ControlledSampleAssetImporter() {}

    /// <summary>
    /// Gets the unique source extension used by the real importer cancellation test.
    /// </summary>
    public override IReadOnlyList<string> supportedExtensions { get; } = [".samplebusy"];

    internal static Control? current
    {
        get => CURRENT.Value;
        set => CURRENT.Value = value;
    }

    /// <summary>
    /// Suspends a Project clone until the transaction's cancellation reaches its importer.
    /// </summary>
    /// <param name="context">
    /// The isolated source import context.
    /// </param>
    /// <param name="output">
    /// The candidate asset writer.
    /// </param>
    /// <param name="cancellationToken">
    /// The sample preparation cancellation token.
    /// </param>
    /// <returns>
    /// The completed import, or cancellation while the test-owned gate is suspended.
    /// </returns>
    protected override async ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<TextAsset> output,
        CancellationToken cancellationToken
    ) {
        Control? control = CURRENT.Value;
        if (control is not null && context.assetPath.source == AssetSourceID.project)
        {
            control.workerThread = Environment.CurrentManagedThreadId;
            control.started.Set();
            using CancellationTokenRegistration cancellation = cancellationToken.Register(control.canceled.Set);
            await Task.Delay(Timeout.Infinite, cancellationToken).ConfigureAwait(false);
        }
        output.SetAsset(new TextAsset(context.ReadUtf8Text()));
        await output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken).ConfigureAwait(false);
    }

    internal sealed class Control : IDisposable
    {
        /// <summary>
        /// Releases the test-owned synchronization handles after the importer drains.
        /// </summary>
        public void Dispose()
        {
            started.Dispose();
            canceled.Dispose();
        }

        internal ManualResetEventSlim started { get; } = new();
        internal ManualResetEventSlim canceled { get; } = new();
        internal int workerThread { get; set; }
    }
}
