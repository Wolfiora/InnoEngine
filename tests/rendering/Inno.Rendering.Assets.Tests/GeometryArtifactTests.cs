using Inno.Core.Mathematics;
using Xunit;

namespace Inno.Rendering.Assets.Tests;

public sealed class GeometryArtifactTests
{
    [Fact]
    public void RuntimeGeometryDecodesWithoutAnImporterOrRenderDevice()
    {
        var vertex = new GeometryVertex(new Vector3(1, 2, 3), new Vector3(0, 1, 0),
            new Vector4(1, 0, 0, 1), new Vector2(0.5f, 0.25f));
        var data = new GeometryData([vertex, vertex, vertex], [0, 1, 2], [new GeometrySection(0, 3)]);
        GeometryData decoded = GeometryArtifact.Decode(GeometryArtifact.Encode(data));
        Assert.Equal(data.vertices, decoded.vertices);
        Assert.Equal(data.indices, decoded.indices);
        Assert.Equal(data.sections, decoded.sections);
    }
}
