using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using BGCS.Runtime;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Inno.Scene;
using Inno.Scene.Components;
using Xunit;
using NativeGui = Inno.Native.ImGui.ImGui;
using NativeVector = Inno.Native.ImGui.ImVec2C;
using SceneVector = Inno.Core.Mathematics.Vector3;

namespace Inno.Editor.PlayMode.Tests;

public sealed partial class LargePanelPerformanceTests
{
    [Theory]
    [InlineData("hierarchy-scroll")]
    [InlineData("assets-scroll")]
    [InlineData("assets-search")]
    [InlineData("hierarchy-edit")]
    [Trait("Acceptance", "Performance")]
    public unsafe void LargeViewsMeasureActualInputAndContinuousEditWork(string scenario)
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        bool hierarchy = scenario.StartsWith("hierarchy", StringComparison.Ordinal);
        GameScene? scene = hierarchy ? LoadInteractionHierarchy(host) : null;
        if (!hierarchy)
        {
            string? input = InputDirectory("assets");
            if (input is not null && Directory.Exists(input))
                CopyDirectory(input, Path.Combine(host.projectRoot, "Assets"));
            else
                for (int index = 0; index < 10_000; index++)
                    Directory.CreateDirectory(Path.Combine(host.projectRoot, "Assets", "Folder " + index.ToString("D5")));
            host.assets.Rescan();
            Assert.Equal(10_000, Directory.EnumerateDirectories(Path.Combine(host.projectRoot, "Assets"), "Folder *").Count());
        }
        EditorPanelExtension panel = host.native.GetPanel(hierarchy ? "scene.hierarchy" : "asset.file-browser");
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        ImGuiIO* io = NativeGui.Raw.GetIO();
        int edits = 0;
        float maximumScroll = 0f;
        SceneVector initial = scene?.GetObjects()[0].transform.localPosition ?? default;
        Transform? transform = scene?.GetObjects()[0].transform;
        using EditorEditGesture? gesture = scenario == "hierarchy-edit"
            ? new EditorEditGesture(host.runtime.interactions.history, "Measured continuous edit")
            : null;
        if (scenario == "assets-search")
            FocusAssetSearch(host, panel);
        Action operation = scenario switch
        {
            "assets-search" => Search,
            "hierarchy-edit" => Edit,
            _ => Scroll
        };
        MeasureInteraction(scenario, operation);
        if (gesture is not null)
        {
            gesture.Commit();
            Assert.True(host.runtime.interactions.history.Undo().succeeded);
            Assert.Equal(initial, transform!.localPosition);
            Assert.False(host.runtime.interactions.history.canUndo);
            Assert.True(host.runtime.interactions.history.Redo().succeeded);
            Assert.Equal((float)edits, transform.localPosition.x);
        }
        else if (scenario.EndsWith("scroll", StringComparison.Ordinal))
            Assert.True(maximumScroll > 0f, "Actual wheel input never moved the visible list.");

        void Scroll()
        {
            ImGuiWindow* window = FindInteractionWindow(hierarchy ? "##HierarchyContent" : "##EntriesScroll");
            Vector2 position = Unsafe.BitCast<NativeVector, Vector2>(window->Pos) + new Vector2(window->Size.X * 0.7f, 140f);
            NativeGui.Raw.AddMousePosEvent(io, position.X, position.Y);
            NativeGui.Raw.AddMouseWheelEvent(io, 0f, -3f);
            host.native.DrawPanel(panel);
            maximumScroll = MathF.Max(maximumScroll, window->Scroll.Y);
        }

        void Search()
        {
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 1);
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.A, 1);
            host.native.DrawPanel(panel);
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.A, 0);
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 0);
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.Backspace, 1);
            host.native.DrawPanel(panel);
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.Backspace, 0);
            string query = ++edits % 2 == 0 ? "Folder 00000" : "Folder 00001";
            using NativeStringLease text = Utf8Codec.EncodeNullTerminated(query, allowsNull: false);
            NativeGui.Raw.AddInputCharactersUTF8(io, (sbyte*)text.pointer);
            host.native.DrawPanel(panel);
            host.native.DrawPanel(panel);
            Assert.NotEqual(0u, NativeGui.Raw.GetCurrentContext()->InputTextState.ID);
            Assert.Equal(query.Length, NativeGui.Raw.GetCurrentContext()->InputTextState.TextLen);
        }

        void Edit()
        {
            int value = ++edits;
            Assert.True(host.edits.ChangeProperty(transform!, nameof(Transform.localPosition),
                () => transform!.localPosition = new(value, 0f, 0f), "Measured continuous edit", gesture!.mergeKey));
            host.native.DrawPanel(panel);
        }
    }

    private static GameScene LoadInteractionHierarchy(NativeProjectPanelHost host)
    {
        string? input = InputDirectory("hierarchy");
        if (input is not null && Directory.Exists(input))
        {
            CopyDirectory(input, Path.Combine(host.projectRoot, "Assets"));
            host.assets.Rescan();
            return host.workspace.Open("Untitled Scene.iscene");
        }
        GameScene scene = host.CreateScene();
        for (int index = 0; index < 10_000; index++)
            _ = scene.CreateObject("Object " + index.ToString("D5"));
        _ = host.workspace.Save(scene, string.Empty);
        return scene;
    }

    private static unsafe void FocusAssetSearch(
        NativeProjectPanelHost host,
        EditorPanelExtension panel
    ) {
        ImGuiWindow* pane = FindInteractionWindow("##ContentPane");
        Vector2 start = Unsafe.BitCast<NativeVector, Vector2>(pane->DC.CursorStartPos);
        ImGuiIO* io = NativeGui.Raw.GetIO();
        // Locate the existing text input using ordinary clicks; no private Panel field is read or changed.
        for (float offset = 24f; offset <= 64f; offset += 4f)
        {
            NativeGui.Raw.AddMousePosEvent(io, pane->Pos.X + pane->Size.X - 70f, start.Y + offset);
            NativeGui.Raw.AddMouseButtonEvent(io, 0, 1);
            host.native.DrawPanel(panel);
            NativeGui.Raw.AddMouseButtonEvent(io, 0, 0);
            host.native.DrawPanel(panel);
            ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
            if (context->InputTextState.ID != 0 && context->ActiveId == context->InputTextState.ID)
                return;
        }
        throw new InvalidOperationException("Ordinary pointer input did not focus the existing asset search field.");
    }

    private static unsafe ImGuiWindow* FindInteractionWindow(string fragment)
    {
        ImGuiNativeContext* context = NativeGui.Raw.GetCurrentContext();
        for (int index = 0; index < context->Windows.Size; index++)
        {
            ImGuiWindow* window = context->Windows.Data[index];
            string? name = Marshal.PtrToStringUTF8((nint)window->Name);
            if (window->Active != 0 && name?.Contains(fragment, StringComparison.Ordinal) == true)
                return window;
        }
        throw new InvalidOperationException("The drawn native input owner is absent: " + fragment);
    }

    private void MeasureInteraction(
        string scenario,
        Action operation
    ) {
        for (int warmup = 0; warmup < 5; warmup++)
            operation();
        int repetitions = int.Parse(Environment.GetEnvironmentVariable("INNO_UI_PERFORMANCE_REPETITIONS") ?? "5",
            System.Globalization.CultureInfo.InvariantCulture);
        Assert.InRange(repetitions, 1, 5);
        using Process process = Process.GetCurrentProcess();
        var samples = new List<object>();
        for (int repetition = 0; repetition < repetitions; repetition++)
        {
            var milliseconds = new double[10];
            long before = GC.GetAllocatedBytesForCurrentThread();
            long totalBefore = GC.GetTotalAllocatedBytes(precise: true);
            TimeSpan cpuBefore = process.TotalProcessorTime;
            int gen0 = GC.CollectionCount(0);
            int gen1 = GC.CollectionCount(1);
            int gen2 = GC.CollectionCount(2);
            for (int frame = 0; frame < milliseconds.Length; frame++)
            {
                long started = Stopwatch.GetTimestamp();
                operation();
                milliseconds[frame] = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
            }
            long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
            double[] sorted = milliseconds.Order().ToArray();
            samples.Add(new
            {
                repetition, milliseconds, median = (sorted[4] + sorted[5]) / 2, p95 = sorted[9], allocatedBytes = allocated,
                totalAllocatedBytes = GC.GetTotalAllocatedBytes(precise: true) - totalBefore,
                cpuMilliseconds = (process.TotalProcessorTime - cpuBefore).TotalMilliseconds,
                gen0Collections = GC.CollectionCount(0) - gen0,
                gen1Collections = GC.CollectionCount(1) - gen1,
                gen2Collections = GC.CollectionCount(2) - gen2
            });
        }
        output.WriteLine(JsonSerializer.Serialize(new
        {
            workload = scenario, samples,
            configuration = typeof(LargePanelPerformanceTests).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration,
            viewport = "1000x800", warmupOperations = 5, measuredOperationsPerRepetition = 10,
            nativeFramesPerOperation = scenario == "assets-search" ? 4 : 1,
            consumer = scenario == "hierarchy-edit" ? "continuous-domain-gesture-and-actual-hierarchy-draw" : "actual-native-input-and-panel-draw",
            sharedInput = InputDirectory(scenario.StartsWith("hierarchy", StringComparison.Ordinal) ? "hierarchy" : "assets")
        }));
    }
}
