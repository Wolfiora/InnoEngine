using System;
using System.Collections.Generic;
using Inno.Editor.Settings;

namespace Inno.Editor.PlayMode.Tests;

[EditorSettingPath("Editor/Atomic Editor/Value")]
public sealed class AtomicEditorSetting : EditorSetting
{
    public static readonly List<WeakReference<EditorSettingObject>> SNAPSHOTS = [];
    public static bool stage;
    public static bool throwOnDraw;
    public static bool observe;

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
        {
            throw new InvalidOperationException("Controlled Settings drawer failure.");
        }
        if (observe
            && (SNAPSHOTS.Count == 0 || !SNAPSHOTS[^1].TryGetTarget(out EditorSettingObject? last) || !ReferenceEquals(last, setting))
        ) {
            SNAPSHOTS.Add(new WeakReference<EditorSettingObject>(setting));
        }
        if (!stage)
        {
            return;
        }
        setting.SetAsInt32("value", 42);
        stage = false;
    }
}
