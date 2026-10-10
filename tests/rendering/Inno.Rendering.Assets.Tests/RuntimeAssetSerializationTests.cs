using System;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Xunit;

namespace Inno.Rendering.Assets.Tests;

public sealed class RuntimeAssetSerializationTests
{
    [Fact]
    public void MovedPersistentTypesKeepTheirRecordedIdentity()
    {
        using var modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(RuntimeAssetSerializationTests).Assembly)
        });
        using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        Assert.Equal(Guid.Parse("5bce7c89-203f-5921-8efc-1bca0f364943"), types.GetTypeRef(typeof(ShaderDefinition)).stableId);
        Assert.Equal(Guid.Parse("0f50b92b-4d4b-55f8-a954-953630083303"), types.GetTypeRef(typeof(ShaderPassDefinition)).stableId);
        Assert.Equal(Guid.Parse("dc64bc3e-f26f-55e0-bffe-99e98bd95b4f"), types.GetTypeRef(typeof(MaterialValue)).stableId);
        Assert.Equal(Guid.Parse("a9af9e77-9a97-5f66-82a0-d720ac5604dc"), types.GetTypeRef(typeof(MaterialValueKind)).stableId);
    }

    [Fact]
    public void ShaderDeclarationsRoundTripThroughTheCommonSerializationContract()
    {
        using var modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(RuntimeAssetSerializationTests).Assembly)
        });
        using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        using var serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
        var definition = new ShaderDefinition("Fixture", [], [],
            [new ShaderPassDefinition("color", ShaderProgramKind.Raster,
                metadata: [new ShaderMetadataEntry("fixture", "value")])]);
        byte[] bytes = serialization.Serialize(definition);
        ShaderDefinition decoded = serialization.Deserialize<ShaderDefinition>(bytes);
        Assert.Equal(definition.name, decoded.name);
        Assert.Equal("color", Assert.Single(decoded.passes).name);
        Assert.Equal("value", Assert.Single(decoded.passes[0].metadata).value);
        ShaderPassDefinition detached = decoded.passes[0].Copy();
        detached.metadata[0] = new ShaderMetadataEntry("fixture", "changed");
        Assert.Equal("value", decoded.passes[0].metadata[0].value);
    }
}
