using Inno.Core.Settings;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("cbeec86a-29da-4894-915f-b6239086ee05")]
[ProjectSettingDefinition("tests.settings.composed-clip")]
internal sealed class ComposedClipSettings : ClipSettingsBase
{
    internal static ProjectSettingID id => new("tests.settings.composed-clip");
}
