using Inno.Editor.Settings;

namespace Inno.Editor.Presentation.Tests;

[ProjectSettingPath("Project/Tests/Multi/Secondary")]
public sealed class SecondaryMultiPresentationEditor : ProjectSettingEditor<MultiPresentationSetting>
{
    protected override void OnDraw(MultiPresentationSetting setting) => _ = setting;
}
