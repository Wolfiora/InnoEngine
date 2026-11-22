using System.IO;
using Xunit;
using Inno.Build;
using Inno.Core.Input;
using Inno.Editor.Core;
using Inno.Editor.Exporting;
using Inno.Editor.Interactions;
using Inno.Testing.Build;

namespace Inno.Editor.Exporting.Tests;

public sealed class ExportWorkflowTests : BuildPipelineFixture
{
    [Fact]
    public void ExportPresentationKeepsUnknownProgressAndRejectsStaleCancellation()
    {
        BuildSettings defaults = BuildSettings.CreateDefault("Test Game", "Scenes/Startup.iscene", new BuildTargetID("macos-arm64"));
        var settings = new BuildSettingsStore(
            Path.Combine(m_projectRoot, "Settings.Build.inno"),
            m_engine.serialization,
            defaults
        );
        m_engine.modules.Register("Tests.ExportProgress", [typeof(EditorExportService).Assembly]);
        var context = new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl"));
        using var runtime = new EditorInteractionRuntime(
            context,
            m_engine.types,
            m_engine.logs,
            [m_pipeline, settings, m_settings]
        );
        runtime.Start();
        Assert.True(runtime.interactions.TryGetModule<EditorExportService>(out EditorExportService? exports));
        exports!.StartGame(new GameBuildRequest
        {
            profile = CreateProfile(new BuildTargetID("macos-arm64")),
            outputDirectory = Path.Combine(m_root, "ProgressOutput")
        });
        EditorExportProgress observed = exports.progress;
        Assert.Null(observed.fraction);
        Assert.True(observed.isBusy);
        exports.Cancel(observed.operationID - 1);
        Assert.False(exports.progress.cancellationRequested);
        exports.Cancel(observed.operationID);
        Assert.True(exports.progress.cancellationRequested);
        Assert.False(observed.cancellationRequested);
        exports.Stop(context);
        Assert.False(exports.progress.isBusy);
    }
}
