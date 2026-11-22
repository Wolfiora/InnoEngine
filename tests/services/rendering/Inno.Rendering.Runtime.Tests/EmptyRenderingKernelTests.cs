using System;
using Xunit;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime.Tests;

public sealed class EmptyRenderingKernelTests
{
    [Fact]
    public void OutputRouteAssignsEachWorldContentSourceOnlyOnce()
    {
        Assert.Throws<ArgumentException>(() => new RenderOutputRoute(
            [
                new RenderOutputLayer("model-a", ["canvas"]),
                new RenderOutputLayer("model-b", ["canvas"])
            ]
        ));
        var route = new RenderOutputRoute(
            [
                new RenderOutputLayer("model-a", ["canvas"]),
                new RenderOutputLayer("model-b", [])
            ]
        );
        Assert.Equal("canvas", Assert.Single(route.layers[0].sourceIDs));
        Assert.Empty(route.layers[1].sourceIDs);
    }

    [Theory]
    [InlineData("Pbr")]
    [InlineData("Forward")]
    [InlineData("Deferred")]
    [InlineData("DirectionalLight")]
    [InlineData("MeshRenderer")]
    public void ProductionRenderingAssemblyDoesNotDeclareConcreteRenderingWorldviews(string forbiddenName)
    {
        Type[] types = typeof(RenderPipeline).Assembly.GetTypes();
        Assert.DoesNotContain(types, type => type.Name.Contains(forbiddenName, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void OpenShaderProtocolsRemainBackendNeutral()
    {
        Type[] publicTypes = typeof(ShaderContractID).Assembly.GetExportedTypes();
        Assert.DoesNotContain(publicTypes, static type => (
            type.FullName?.Contains("Bgfx", StringComparison.OrdinalIgnoreCase) == true
        ));
        Assert.Contains(publicTypes, static type => type == typeof(ShaderContractID));
        Assert.Contains(publicTypes, static type => type == typeof(ShaderPassRoleID));
    }
}
