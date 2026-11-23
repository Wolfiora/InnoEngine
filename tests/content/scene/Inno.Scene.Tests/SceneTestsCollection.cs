using Xunit;

namespace Inno.Scene.Tests;

[CollectionDefinition(C_NAME)]
public sealed class SceneTestsCollection : ICollectionFixture<SceneTestsFixture>
{
    public const string C_NAME = nameof(SceneTestsCollection);
}
