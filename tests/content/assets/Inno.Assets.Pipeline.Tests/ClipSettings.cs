using System;
using Inno.Core.Settings;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("2c7288db-eb9e-43a5-9894-bf9d10b22a2d")]
[ProjectSettingDefinition("tests.settings.clip")]
internal sealed class ClipSettings : ClipSettingsBase
{
    internal static readonly Guid TYPE_ID = new("2c7288db-eb9e-43a5-9894-bf9d10b22a2d");
    internal static ProjectSettingID id => new("tests.settings.clip");
}
