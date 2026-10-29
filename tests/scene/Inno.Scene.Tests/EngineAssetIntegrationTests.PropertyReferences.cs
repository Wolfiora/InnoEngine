using Inno.Assets;
using Xunit;

namespace Inno.Scene.Tests;

public sealed partial class EngineAssetIntegrationTests
{
    [Fact]
    public void PropertyReferenceScopePreservesTheCanonicalAssetResolver()
    {
        WriteAsset("Text/property-reference.txt", "property source");
        Assert.True(m_assets.Import(AssetPath.Project("Text/property-reference.txt")));
        TextAsset asset = m_assets.Load<TextAsset>(AssetPath.Project("Text/property-reference.txt"));
        var scene = new GameScene("Asset property reference");
        EngineAssetReferenceComponent component = scene.CreateObject("Owner").AddComponent<EngineAssetReferenceComponent>();
        component.asset = asset;
        byte[] state = ScenePropertySerialization.CaptureProperties(component, m_fixture.serialization, m_assets);
        component.asset = null;
        _ = ScenePropertySerialization.RestoreProperties(component, state, m_fixture.serialization, m_assets);
        Assert.Same(asset, component.asset);
        DestroyScene(scene);
    }
}
