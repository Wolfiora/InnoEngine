using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Inno.Editor.Interactions;
using Inno.Scene;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed class SceneDocumentLifecycleNativeTests
{
    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void OneHundredSavedSceneCyclesRetireLiveScenesAndPreserveSourceAndHistory()
    {
        using var host = new NativeProjectPanelHost();
        GameScene original = host.CreateScene();
        _ = original.CreateObject("Persistent lifecycle object");
        string path = host.workspace.Save(original, string.Empty);
        byte[] source = File.ReadAllBytes(Path.Combine(host.projectRoot, "Assets", path));
        Assert.True(host.edits.CloseScene(original));
        EditorPanelExtension panel = host.native.GetPanel("scene.hierarchy");
        var retired = new List<WeakReference<GameScene>>();
        for (int iteration = 0; iteration < 100; iteration++)
            retired.Add(OpenDrawAndClose(host, panel, path));
        Assert.Empty(host.workspace.scenes);
        Assert.True(host.runtime.interactions.history.canUndo);
        Assert.Equal(source, File.ReadAllBytes(Path.Combine(host.projectRoot, "Assets", path)));
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.All(retired, static reference => Assert.False(reference.TryGetTarget(out _)));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference<GameScene> OpenDrawAndClose(
        NativeProjectPanelHost host,
        EditorPanelExtension panel,
        string path
    ) {
        GameScene scene = host.workspace.Open(path);
        host.native.DrawPanel(panel);
        Assert.True(host.edits.CloseScene(scene));
        host.native.DrawPanel(panel);
        Assert.True(scene.isDestroyed);
        return new WeakReference<GameScene>(scene);
    }
}
