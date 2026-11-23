using System;
using System.IO;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Extensibility.Types;
using Inno.Runtime;

namespace Inno.Build.Tests;

public sealed class BuildProfileTests : IDisposable
{
    private readonly string m_projectRoot = Path.Combine(Path.GetTempPath(), "InnoBuildProfileTests", Guid.NewGuid().ToString("N"));
    private readonly EngineHost m_engine;

    public BuildProfileTests()
    {
        Directory.CreateDirectory(m_projectRoot);
        m_engine = new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(typeof(BuildProfile).Assembly),
            new ReflectionTypeCatalogSource(),
            new ReflectionSerializationMetadataSource()
        ).Build();
    }

    public void Dispose()
    {
        m_engine.Dispose();
        Directory.Delete(m_projectRoot, recursive: true);
    }

    [Theory]
    [InlineData("../other-game")]
    [InlineData("C:/absolute")]
    [InlineData("studio//game")]
    public void ProfileRejectsPersistentDataPathsOutsidePortableApplicationFolders(string path)
    {
        BuildProfile profile = CreateProfile(new BuildTargetID("windows-x64"));
        profile.persistentDataPath = path;
        Assert.Throws<InvalidDataException>(profile.Validate);
    }

    [Theory]
    [InlineData("CON")]
    [InlineData("nul.txt")]
    [InlineData("Trailing.")]
    [InlineData("Trailing ")]
    [InlineData("Bad:Name")]
    public void BuildProfileRejectsNamesThatAreInvalidOnEitherTarget(string productName)
    {
        BuildProfile profile = CreateProfile(new BuildTargetID("macos-arm64"), productName);

        Assert.Throws<InvalidDataException>(profile.Validate);
    }

    [Fact]
    public void BuildProfileStoreRoundTripsGeneratedCurrentFormat()
    {
        string path = Path.Combine(m_projectRoot, "BuildProfile.inno");
        var store = new BuildProfileStore(path, m_engine.serialization);
        BuildProfile source = CreateProfile(new BuildTargetID("windows-x64"), "Stored Game");
        source.windowWidth = 1920;
        source.windowHeight = 1080;

        store.Save(source);
        BuildProfile restored = store.Load();

        Assert.True(store.exists);
        Assert.Equal(source.applicationId, restored.applicationId);
        Assert.Equal(source.productName, restored.productName);
        Assert.Equal(source.startupScene, restored.startupScene);
        Assert.Equal(new BuildTargetID("windows-x64"), restored.target);
        Assert.Equal(1920, restored.windowWidth);
        Assert.Equal(1080, restored.windowHeight);
    }

    [Fact]
    public void BuildProfileStorePreservesCommittedDocumentWhenCandidateIsInvalid()
    {
        string path = Path.Combine(m_projectRoot, "BuildProfile.inno");
        var store = new BuildProfileStore(path, m_engine.serialization);
        BuildProfile committed = CreateProfile(new BuildTargetID("macos-arm64"), "Committed Game");
        store.Save(committed);
        byte[] committedBytes = File.ReadAllBytes(path);
        BuildProfile invalid = CreateProfile(new BuildTargetID("windows-x64"), "Invalid/Game");

        Assert.Throws<InvalidDataException>(() => store.Save(invalid));

        Assert.Equal(committedBytes, File.ReadAllBytes(path));
        Assert.Empty(Directory.EnumerateFiles(m_projectRoot, "BuildProfile.inno.staging-*"));
    }

    [Fact]
    public void BuildProfileStoreRejectsCorruptCurrentFormatWithoutFallback()
    {
        string path = Path.Combine(m_projectRoot, "BuildProfile.inno");
        File.WriteAllBytes(path, [0x42, 0x41, 0x44]);
        var store = new BuildProfileStore(path, m_engine.serialization);

        InvalidDataException failure = Assert.Throws<InvalidDataException>(store.Load);

        Assert.Contains("current-format", failure.Message, StringComparison.Ordinal);
    }

    private static BuildProfile CreateProfile(
        BuildTargetID target,
        string productName = "Test Game"
    ) => new()
    {
        applicationId = "tests.game",
        productName = productName,
        startupScene = "Scenes/Startup.iscene",
        target = target
    };
}
