using Inno.Editor.Settings;

namespace Inno.Editor.PlayMode.Tests;

[ProjectSettingPath("Project/Atomic Project/Value")]
public sealed class AtomicProjectEditor : ProjectSettingEditor<AtomicProjectSetting>
{
    public static bool stage;

    protected override void OnDraw(AtomicProjectSetting setting)
    {
        if (!stage)
        {
            return;
        }
        setting.value = 84;
        stage = false;
    }
}
