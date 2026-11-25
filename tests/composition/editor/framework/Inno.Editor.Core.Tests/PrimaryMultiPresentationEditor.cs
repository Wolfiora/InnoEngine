using Inno.Editor.Settings;

namespace Inno.Editor.Core.Tests;

[ProjectSettingPath("Project/Tests/Multi/Primary")]
public sealed class PrimaryMultiPresentationEditor : ProjectSettingEditor<MultiPresentationSetting>
{
    protected override void OnDraw(MultiPresentationSetting setting) => _ = setting;
}
