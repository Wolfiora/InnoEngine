using System;
using System.Linq;
using Xunit;
using Inno.Core.Settings;
using Inno.Scene;
using Inno.Scene.Layers;

namespace Inno.Scene.Tests;

/// <summary>
/// Verifies runtime layer identifiers, masks, stack configuration, and Scene lookup behavior.
/// </summary>
[Collection(SceneTestsCollection.C_NAME)]
public sealed class GameLayerTests : IDisposable
{
    private readonly IDisposable m_sceneScope;

    public GameLayerTests(SceneTestsFixture fixture)
    {
        m_sceneScope = fixture.world.EnterScope();
    }

    public void Dispose()
    {
        SceneManager.UnloadAllScenes();
        m_sceneScope.Dispose();
    }

    /// <summary>
    /// Verifies that layer identifiers accept exactly the thirty-two mask-backed slots.
    /// </summary>
    [Fact]
    public void GameLayerValidatesSupportedSlotRange()
    {
        Assert.Equal(0, GameLayer.defaultLayer.index);
        Assert.Equal(31, new GameLayer(31).index);
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameLayer(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameLayer(32));
    }

    /// <summary>
    /// Verifies immutable mask construction and membership operations.
    /// </summary>
    [Fact]
    public void GameLayerMaskComposesAndFiltersLayers()
    {
        var first = new GameLayer(1);
        var second = new GameLayer(2);
        GameLayerMask mask = GameLayerMask.none.With(first).With(second);

        Assert.True(mask.Contains(first));
        Assert.True(mask.Contains(second));
        Assert.False(mask.Contains(GameLayer.defaultLayer));
        Assert.True(mask.Without(first).Contains(second));
        Assert.False(mask.Without(first).Contains(first));
        Assert.Equal(mask, GameLayerMask.FromLayers([first, second]));
    }

    /// <summary>
    /// Verifies layer naming constraints and symmetric interaction updates.
    /// </summary>
    [Fact]
    public void GameLayerCatalogEnforcesUniqueDefinitionsAndSymmetricInteractions()
    {
        var stack = new GameLayerCatalog();
        var player = new GameLayer(1);
        var enemy = new GameLayer(2);
        stack.Define(player, "Player");
        stack.Define(enemy, "Enemy");
        stack.SetInteraction(player, enemy, canInteract: false);

        Assert.Equal("Default", stack.GetName(GameLayer.defaultLayer));
        Assert.True(stack.TryGetLayer("Player", out GameLayer resolved));
        Assert.Equal(player, resolved);
        Assert.False(stack.CanInteract(player, enemy));
        Assert.False(stack.CanInteract(enemy, player));
        Assert.Throws<ArgumentException>(() => stack.Define(new GameLayer(3), "Player"));
        Assert.Throws<InvalidOperationException>(() => stack.Remove(GameLayer.defaultLayer));

        GameLayerCatalog copy = stack.Clone();
        copy.SetInteraction(player, enemy, canInteract: true);
        Assert.False(stack.CanInteract(player, enemy));
        Assert.True(copy.CanInteract(player, enemy));
    }

    /// <summary>
    /// Verifies that project-scoped identities are resolved from stable local keys at the read boundary.
    /// </summary>
    [Fact]
    public void ProjectScopedIDsFollowProjectIdentityWithoutRewritingDefinitions()
    {
        var firstProject = new ProjectID("studio.first");
        Assert.Equal("innoengine-rendering2d", ProjectID.FromName("InnoEngine.Rendering2D").value);
        Assert.Equal("inno.project", ProjectID.FromName("项目").value);

        var secondProject = new ProjectID("studio.second");
        var layer = new GameLayer(1);
        var layers = new GameLayerCatalog();
        layers.Define(layer, "Player");

        Assert.Equal("layer.01", layers.GetLocalID(layer)?.value);
        Assert.Equal("studio.first.layer.01", layers.GetID(firstProject, layer)?.value);
        Assert.Equal("studio.second.layer.01", layers.GetID(secondProject, layer)?.value);

        layers.Define(layer, "Hero");

        Assert.Equal("layer.01", layers.GetLocalID(layer)?.value);
        Assert.Equal("studio.second.layer.01", layers.GetID(secondProject, layer)?.value);

        var tags = new GameTagCatalog();
        Assert.True(tags.Add("Enemy Boss"));
        Assert.Equal("enemy-boss", tags.GetLocalID("Enemy Boss").value);
        Assert.Equal("studio.first.enemy-boss", tags.GetID(firstProject, "Enemy Boss").value);
        Assert.Equal("studio.second.enemy-boss", tags.GetID(secondProject, "Enemy Boss").value);
    }

    /// <summary>
    /// Verifies that Scene layer indexes remain coherent after live metadata changes.
    /// </summary>
    [Fact]
    public void GameSceneLayerQueriesTrackLiveObjectChanges()
    {
        var scene = new GameScene("Layers");
        GameObject first = scene.CreateObject("First");
        GameObject second = scene.CreateObject("Second");
        var player = new GameLayer(1);
        var enemy = new GameLayer(2);
        first.layer = player;
        second.layer = enemy;

        Assert.Same(first, scene.FindObjectWithLayer(player));
        Assert.Equal([first], scene.FindObjectsWithLayer(player));
        Assert.Equal([first, second], scene.FindObjectsWithLayers(GameLayerMask.FromLayers([player, enemy])));

        first.layer = enemy;

        Assert.Null(scene.FindObjectWithLayer(player));
        Assert.Equal([first, second], scene.FindObjectsWithLayer(enemy));
    }
}
