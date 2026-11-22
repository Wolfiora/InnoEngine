using Inno.Core.Serialization;
using Inno.Core.Settings;

namespace Inno.Editor.PlayMode.Tests;

[GenerateSerializationConverter]
[ProjectSettingDefinition("tests.ui.atomic-settings")]
public sealed class AtomicProjectSetting : ISerializable
{
    [SerializableProperty]
    public int value { get; set; }
}
