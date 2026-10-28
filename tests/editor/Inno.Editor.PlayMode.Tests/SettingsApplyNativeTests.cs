using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Inno.Core.IO;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Editor.Interactions;
using Inno.Editor.Settings;
using Inno.Native.ImGui;
using Xunit;
using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class SettingsApplyNativeTests
{
    [Fact]
    [Trait("Acceptance", "Workflows")]
    public void FaultedDrawerReleasesReloadAdmissionUntilItsExtensionsRecover()
    {
        using var host = new NativeProjectPanelHost(includeSettings: true);
        var driver = new SettingsInput(host);
        Assert.True(host.runtime.interactions.For("editor/main-menu").Execute("editor.settings.open"));
        driver.Draw();
        AtomicEditorSetting.throwOnDraw = true;
        try
        {
            Assert.ThrowsAny<Xunit.Sdk.XunitException>(() => driver.Search("Atomic Editor"));
        }
        finally
        {
            AtomicEditorSetting.throwOnDraw = false;
        }
        Assert.True(host.reloads.CanActivateReload(out _));
        EditorActionState state = host.runtime.interactions.For("editor/main-menu").Query("editor.settings.open");
        Assert.True(state.isVisible);
        Assert.False(state.isEnabled);
        Assert.False(string.IsNullOrWhiteSpace(state.unavailableReason));
        Assert.False(host.runtime.interactions.For("editor/main-menu").Execute("editor.settings.open"));
        host.RefreshTypes();
        Assert.True(host.runtime.interactions.For("editor/main-menu").Execute("editor.settings.open"));
        driver.Draw();
        driver.Search("Atomic Editor");
        driver.CloseClean();
        Assert.False(driver.isVisible);
        Assert.True(host.reloads.CanActivateReload(out _));
    }

    [Fact]
    [Trait("Acceptance", "Lifecycle")]
    public void OneHundredSettingsCyclesReleaseDrawnValuesAndReloadAdmission()
    {
        AtomicEditorSetting.snapshots.Clear();
        AtomicEditorSetting.observe = true;
        try
        {
            using var host = new NativeProjectPanelHost(includeSettings: true);
            for (int cycle = 0; cycle < 100; cycle++)
                OpenAndClose(host);
            Assert.True(AtomicEditorSetting.snapshots.Count >= 100);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.All(AtomicEditorSetting.snapshots, snapshot => Assert.False(snapshot.TryGetTarget(out _)));
        }
        finally
        {
            AtomicEditorSetting.observe = false;
            AtomicEditorSetting.snapshots.Clear();
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void OpenAndClose(NativeProjectPanelHost host)
    {
        Assert.True(host.reloads.CanRequestReload(out _));
        Assert.True(host.runtime.interactions.For("editor/main-menu").Execute("editor.settings.open"));
        var driver = new SettingsInput(host);
        driver.Draw();
        Assert.False(host.reloads.CanActivateReload(out string? reason));
        Assert.Contains("Settings", reason);
        driver.Search("Atomic Editor");
        driver.CloseClean();
        Assert.False(driver.isVisible);
        Assert.True(host.reloads.CanRequestReload(out _));
        Assert.True(host.reloads.CanActivateReload(out _));
    }

    [Fact]
    [Trait("Acceptance", "Workflows")]
    public void FailedRollbackKeepsHistoryFaultedAndPreventsFurtherApply()
    {
        string directory = Path.Combine(Path.GetTempPath(), "InnoSettingsRollback", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var document = new FailingSettingsDocument(new FileByteDocumentStore(Path.Combine(directory, "Project.inno")));
            using var host = new NativeProjectPanelHost(includeSettings: true, settingsDocument: document);
            Assert.True(host.runtime.interactions.TryGetModule<EditorSettings>(out var editor));
            var driver = new SettingsInput(host);
            Assert.True(host.runtime.interactions.For("editor/main-menu").Execute("editor.settings.open"));
            driver.Draw();
            AtomicEditorSetting.stage = true;
            driver.Search("Atomic Editor");
            Assert.False(AtomicEditorSetting.stage);
            AtomicProjectEditor.stage = true;
            driver.Search("Atomic Project");
            Assert.False(AtomicProjectEditor.stage);
            string editorFile = Path.Combine(host.projectRoot, "Settings.Editor.inno");
            string savedFile = Path.Combine(directory, "Editor.inno");
            bool obstructed = false;
            using IDisposable listener = editor!.ListenChanges(_ =>
            {
                if (obstructed)
                    return;
                File.Move(editorFile, savedFile);
                Directory.CreateDirectory(editorFile);
                obstructed = true;
            });
            document.failWrites = true;
            try
            {
                driver.Apply();
                Assert.True(obstructed);
                Assert.Equal(42, editor.Get("Editor/Atomic Editor/Value").GetAsInt32("value"));
                Assert.Equal(0, host.projectSettings.Get<AtomicProjectSetting>(new("tests.ui.atomic-settings")).value);
                Assert.True(host.runtime.interactions.history.isFaulted);
                Assert.False(host.runtime.interactions.history.canUndo);
                Assert.False(host.runtime.interactions.history.canRedo);
                Assert.False(string.IsNullOrWhiteSpace(host.runtime.interactions.history.faultReason));
            }
            finally
            {
                if (obstructed)
                {
                    Directory.Delete(editorFile);
                    File.Move(savedFile, editorFile);
                }
            }
            document.failWrites = false;
            driver.Apply();
            Assert.True(host.runtime.interactions.history.isFaulted);
            Assert.Equal(0, host.projectSettings.Get<AtomicProjectSetting>(new("tests.ui.atomic-settings")).value);
            driver.Cancel();
            host.native.logs.Flush();
            Assert.False(driver.isVisible, driver.diagnostic + Environment.NewLine + string.Join(Environment.NewLine, host.native.console.Capture().occurrences.Select(entry => entry.message)));
        }
        finally
        {
            AtomicEditorSetting.stage = false;
            AtomicProjectEditor.stage = false;
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    [Trait("Acceptance", "Workflows")]
    public void FailedSettingsBatchPreservesAllOwnersAndDraftsForRetry()
    {
        string directory = Path.Combine(Path.GetTempPath(), "InnoSettingsPublication", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var document = new FailingSettingsDocument(new FileByteDocumentStore(Path.Combine(directory, "Project.inno")));
            using var host = new NativeProjectPanelHost(includeSettings: true, settingsDocument: document);
            Assert.True(host.runtime.interactions.TryGetModule<EditorSettings>(out var editor));
            var driver = new SettingsInput(host);
            Assert.True(host.runtime.interactions.For("editor/main-menu").Execute("editor.settings.open"));
            driver.Draw();
            AtomicEditorSetting.stage = true;
            driver.Search("Atomic Editor");
            Assert.False(AtomicEditorSetting.stage);
            AtomicProjectEditor.stage = true;
            driver.Search("Atomic Project");
            Assert.False(AtomicProjectEditor.stage);
            document.failWrites = true;
            driver.Apply();
            Assert.Equal(0, editor!.Get("Editor/Atomic Editor/Value").GetAsInt32("value"));
            Assert.Equal(0, host.projectSettings.Get<AtomicProjectSetting>(new("tests.ui.atomic-settings")).value);
            Assert.False(host.runtime.interactions.history.canUndo);
            Assert.False(host.runtime.interactions.history.isFaulted);
            document.failWrites = false;
            driver.Apply();
            Assert.Equal(42, editor.Get("Editor/Atomic Editor/Value").GetAsInt32("value"));
            Assert.Equal(84, host.projectSettings.Get<AtomicProjectSetting>(new("tests.ui.atomic-settings")).value);
            Assert.True(host.runtime.interactions.history.Undo().succeeded);
            Assert.Equal(0, editor.Get("Editor/Atomic Editor/Value").GetAsInt32("value"));
            Assert.Equal(0, host.projectSettings.Get<AtomicProjectSetting>(new("tests.ui.atomic-settings")).value);
            Assert.False(host.runtime.interactions.history.canUndo);
            Assert.True(host.runtime.interactions.history.Redo().succeeded);
            Assert.Equal(42, editor.Get("Editor/Atomic Editor/Value").GetAsInt32("value"));
            Assert.Equal(84, host.projectSettings.Get<AtomicProjectSetting>(new("tests.ui.atomic-settings")).value);
        }
        finally
        {
            AtomicEditorSetting.stage = false;
            AtomicProjectEditor.stage = false;
            Directory.Delete(directory, recursive: true);
        }
    }

    private sealed class FailingSettingsDocument(IByteDocumentStore owner) : IByteDocumentStore
    {
        internal bool failWrites { get; set; }
        public string documentName => owner.documentName;
        public bool canWrite => owner.canWrite;
        public bool exists => owner.exists;
        public byte[]? Read() => owner.Read();
        public void Write(ReadOnlySpan<byte> data)
        {
            if (failWrites)
                throw new IOException("Controlled project settings publication failure.");
            owner.Write(data);
        }
    }

    private sealed class SettingsInput(NativeProjectPanelHost host)
    {
        private Vector2 m_search;
        private Vector2 m_apply;
        private Vector2 m_cancel;
        internal bool isVisible { get; private set; }
        internal string diagnostic => $"Apply={m_apply}; Cancel={m_cancel}; Search={m_search}";

        internal void Draw()
        {
            host.native.Frame(() =>
            {
                NativeGui.SetNextWindowPos(new Vector2(20f), (int)ImGuiCond.Always);
                NativeGui.SetNextWindowSize(new Vector2(1000f, 800f), (int)ImGuiCond.Always);
                bool open = true;
                _ = NativeGui.Begin("Settings acceptance", ref open, (int)ImGuiWindowFlags.NoSavedSettings);
                try
                {
                    Vector2 content = NativeGui.GetCursorScreenPos();
                    m_search = content + new Vector2(115f, NativeGui.GetFrameHeight() * 0.5f + 3f);
                    EditorModalExtension modal = Assert.Single(host.runtime.modals, modal => modal.id == "editor.settings");
                    Assert.True(modal.TryGetPresentation(out var presentation));
                    isVisible = presentation.isVisible;
                    if (!isVisible)
                        return;
                    Assert.True(modal.Draw(host.runtime.context), "The operational Settings failure must not quarantine the modal.");
                    m_apply = (NativeGui.GetItemRectMin() + NativeGui.GetItemRectMax()) * 0.5f;
                    var style = NativeGui.Raw.GetStyle();
                    m_cancel = new Vector2(NativeGui.GetItemRectMin().X - style->ItemSpacing.X
                        - NativeGui.CalcTextSize("Cancel").X * 0.5f - style->FramePadding.X, m_apply.Y);
                }
                finally
                {
                    NativeGui.End();
                }
            });
        }

        internal void Search(string text)
        {
            Click(m_search);
            ImGuiIO* io = NativeGui.Raw.GetIO();
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 1);
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.A, 1);
            Draw();
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.A, 0);
            NativeGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 0);
            Draw();
            foreach (char character in text)
                NativeGui.Raw.AddInputCharacter(io, character);
            Draw();
            Draw();
        }

        internal void Apply() => Click(m_apply);
        internal void Cancel() => Click(m_cancel);
        internal void CloseClean() => Click(m_apply);

        private void Click(Vector2 point)
        {
            ImGuiIO* io = NativeGui.Raw.GetIO();
            NativeGui.Raw.AddMousePosEvent(io, point.X, point.Y);
            Draw();
            NativeGui.Raw.AddMouseButtonEvent(io, 0, 1);
            Draw();
            NativeGui.Raw.AddMouseButtonEvent(io, 0, 0);
            Draw();
            Draw();
        }
    }
}

[EditorSettingPath("Editor/Atomic Editor/Value")]
public sealed class AtomicEditorSetting : EditorSetting
{
    public static bool stage;
    public static bool throwOnDraw;
    public static bool observe;
    public static readonly List<WeakReference<EditorSettingObject>> snapshots = [];
    public override EditorSettingObject defaultValue
    {
        get
        {
            var value = new EditorSettingObject();
            value.SetAsInt32("value", 0);
            return value;
        }
    }
    protected override void OnDraw(EditorSettingObject setting)
    {
        if (throwOnDraw)
            throw new InvalidOperationException("Controlled Settings drawer failure.");
        if (observe && (snapshots.Count == 0 || !snapshots[^1].TryGetTarget(out var last) || !ReferenceEquals(last, setting)))
            snapshots.Add(new WeakReference<EditorSettingObject>(setting));
        if (!stage)
            return;
        setting.SetAsInt32("value", 42);
        stage = false;
    }
}

[GenerateSerializationConverter]
[ProjectSettingDefinition("tests.ui.atomic-settings")]
public sealed class AtomicProjectSetting : ISerializable
{
    [SerializableProperty]
    public int value { get; set; }
}

[ProjectSettingPath("Project/Atomic Project/Value")]
public sealed class AtomicProjectEditor : ProjectSettingEditor<AtomicProjectSetting>
{
    public static bool stage;
    protected override void OnDraw(AtomicProjectSetting setting)
    {
        if (!stage)
            return;
        setting.value = 84;
        stage = false;
    }
}
