using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;

namespace Inno.Plugins.Authoring.Tests;

[StableTypeID("706c1248-478b-43f1-a44d-98a1a8f7b919")]
[ProjectSettingDefinition("tests.plugins.default")]
internal sealed class PluginDefaultTestSetting : ISerializable
{
    [SerializableProperty]
    public int value { get; set; } = 1;

    internal static ProjectSettingID id => new("tests.plugins.default");
}
