using System;
using System.Collections.Generic;
using Xunit;
using Inno.Build;
using Inno.Core.Settings;
using Inno.Testing.Build;

namespace Inno.Build.Products.Tests;

public sealed class BuildTargetCatalogTests : BuildPipelineFixture
{
    [Fact]
    public void ProjectSettingsUseTheHostSuppliedInitialIdentity()
    {
        Assert.Equal("tests.build", m_settings.projectId.value);
        Assert.False(m_settings.HasProjectOverride(ProjectIdentitySettings.settingId));
    }

    [Fact]
    public void BuildTargetsAreOpenIdsOwnedByTheRegisteredAdapterCatalog()
    {
        Assert.Equal(
            [new BuildTargetId("macos-arm64"), new BuildTargetId("windows-x64")],
            m_pipeline.availableGameTargets
        );
        Assert.True(
            m_pipeline.TryGetGameTargetDisplayName(new BuildTargetId("macos-arm64"), out string macOSDisplayName)
        );
        Assert.Equal("macOS (Apple silicon)", macOSDisplayName);
        Assert.False(
            m_pipeline.TryGetGameTargetDisplayName(new BuildTargetId("linux-x64"), out string missingDisplayName)
        );
        Assert.Empty(missingDisplayName);

        var profile = new BuildProfile
        {
            applicationId = "tests.open-target",
            productName = "Open Target",
            startupScene = "Scenes/Startup.iscene",
            target = new BuildTargetId("linux-x64")
        };
        profile.Validate();
    }

    [Fact]
    public void BuildTargetInventoryCannotBeMutatedThroughItsCollectionView()
    {
        IList<BuildTargetId> targets = Assert.IsAssignableFrom<IList<BuildTargetId>>(m_pipeline.availableGameTargets);

        Assert.True(targets.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => targets[0] = new BuildTargetId("unknown-target"));
        Assert.Equal([new BuildTargetId("macos-arm64"), new BuildTargetId("windows-x64")], m_pipeline.availableGameTargets);
    }
}
