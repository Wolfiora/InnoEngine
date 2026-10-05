using System;
using System.IO;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;
using Inno.Runtime;
using Xunit;

using Inno.Extensibility.Catalogs;

namespace Inno.Runtime.Tests;

public sealed class StaticProjectSettingsTests
{
    private static readonly ProjectSettingId SettingId = new("tests.static-setting");

    [Fact]
    public void SettingsUseContributedMetadataAndFactoryWithoutReflectiveDeclarations()
    {
        string directory = CreateDirectory();
        try
        {
            using EngineHost host = CreateHost(directory, static () => new Setting(17));
            using var settings = new ProjectSettings(Path.Combine(directory, "Settings.Project.inno"),
                host.types, host.serialization, new ProjectId("tests.static"), SerializationContext.empty);

            Setting first = settings.Get<Setting>(SettingId);
            Assert.Equal(17, first.value);
            first.value = 42;
            Assert.Equal(17, settings.Get<Setting>(SettingId).value);
            settings.SetProjectOverride(SettingId, first, []);
            Assert.Equal(42, settings.Get<Setting>(SettingId).value);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void SettingsRejectMissingFactoriesDuringCandidatePreparation()
    {
        string directory = CreateDirectory();
        try
        {
            using EngineHost host = CreateHost(directory, factory: null);
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                new ProjectSettings(Path.Combine(directory, "Settings.Project.inno"), host.types,
                    host.serialization, new ProjectId("tests.static"), SerializationContext.empty));
            Assert.Contains("factory in the current type catalog", exception.Message);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static EngineHost CreateHost(
        string directory,
        Func<object>? factory
    ) {
        Attribute[] attributes = [new StableTypeIdAttribute("f506944b-bcfa-48e4-b839-a66b4d8f5887"),
            new ProjectSettingDefinitionAttribute(SettingId.value)];
        var types = new StaticTypeCatalogSource([register => register.Register(new TypeCatalogMetadata(typeof(Setting),
            [], [typeof(ISerializable)], attributes, attributes, []), factory)]);
        var serialization = new StaticSerializationMetadataSource([
            register => register(new SerializationTypeMetadata(typeof(Setting), [
                new SerializationMemberMetadata("value", typeof(int), PropertyVisibility.Show,
                    static value => ((Setting)value).value,
                    static (
                        target,
                        value
                    ) => ((Setting)target).value = (int)value!)
            ], static () => new Setting(17))),
            Inno.Core.Settings.Generated.RuntimeSerializationMetadataCatalog.Register
        ]);
        return new EngineHostBuilder().UseMetadataCache(Path.Combine(directory, "Metadata"))
            .UseMetadataSources(new StaticAssemblyCatalogSource([typeof(Setting).Assembly], [typeof(object).Assembly]),
                types, serialization).Build();
    }

    private static string CreateDirectory()
    {
        string directory = Path.Combine(Path.GetTempPath(), "inno-static-settings-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        return directory;
    }

    public sealed class Setting(int initialValue) : ISerializable
    {
        public int value { get; set; } = initialValue;
    }
}
