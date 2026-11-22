using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;

namespace Inno.Editor.Presentation.Tests;

[StableTypeID("0fbd66e2-303e-4b35-83a1-fc2b90d58360")]
[ProjectSettingDefinition("tests.editor.multi-presentation")]
public sealed class MultiPresentationSetting : ISerializable
{
    public static ProjectSettingID settingID => new("tests.editor.multi-presentation");

    [SerializableProperty]
    public int value { get; set; } = 7;
}
