using System;
using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

internal static class StorageNodePorts
{
    internal static ShaderSourceType Resource(ShaderNodeDescriptionContext context)
        => context
            .Read(
                "resource",
                new ShaderGraphType { isStorage = true, storageElement = new() { id = "float4" }, access = RenderStorageAccess.ReadWrite }
            )
            .CreateType();

    internal static ShaderSourceType Coordinate(ShaderStorageType storage)
        => ShaderSourceType.Atomic(
            !storage.isImage ? "uint" : storage.array || storage.dimension == RenderTextureDimension.Texture3D ? "int3" : "int2"
        );

    internal static IReadOnlyList<ShaderNodePort> Describe(
        ShaderNodeDescriptionContext context,
        bool write,
        bool atomic
    ) {
        ShaderSourceType type = Resource(context);
        ShaderStorageType storage = type.storage ?? throw new ArgumentException("A storage node requires a storage resource descriptor.");
        List<ShaderNodePort> ports =
        [
            new ShaderNodePort("resource", type, GraphPortDirection.Input),
            new ShaderNodePort("coordinate", Coordinate(storage), GraphPortDirection.Input),
            new ShaderNodePort("after", ShaderSourceType.Atomic("bool"), GraphPortDirection.Input, false),
            new ShaderNodePort("then", ShaderSourceType.Atomic("bool"), GraphPortDirection.Output)
        ];
        if (write)
        {
            ports.Add(new ShaderNodePort("value", storage.valueType, GraphPortDirection.Input));
        }
        if (!write || atomic)
        {
            ports.Add(new ShaderNodePort(atomic ? "previous" : "value", storage.valueType, GraphPortDirection.Output));
        }
        return ports;
    }
}
