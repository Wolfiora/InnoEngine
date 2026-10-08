using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Shaders;
using NativeBgfx = Inno.Native.Bgfx.bgfx;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Keeps compiled GLSL bindings and the BGFX binary uniform table consistent.
/// </summary>
public sealed partial class BgfxShadercToolchain
{
    private static byte[] NormalizeGlslUniformTable(
        byte[] binary,
        ShaderIrStage stage,
        IReadOnlyList<ShaderStageBinding> bindings
    ) {
        HashSet<string> activeNames = ReadReflectedUniformNames(binary, GraphicsApi.OpenGL);
        ushort count = BinaryPrimitives.ReadUInt16LittleEndian(binary.AsSpan(C_SHADER_BINARY_HEADER_SIZE));
        int offset = C_SHADER_BINARY_HEADER_SIZE + sizeof(ushort);
        var records = new Dictionary<string, byte[]>(StringComparer.Ordinal);
        for (int index = 0; index < count; index++)
        {
            int start = offset;
            int nameLength = binary[offset++];
            string name = Encoding.UTF8.GetString(binary, offset, nameLength);
            offset += nameLength + C_SHADER_UNIFORM_METADATA_SIZE;
            if (activeNames.Contains(name))
                records.Add(name, binary.AsSpan(start, offset - start).ToArray());
        }

        // Shaderc's line scanner stops at layout-qualified declarations. Complete its metadata
        // from the generated IR only after the compiled GLSL confirms that a binding remains active.
        foreach (ShaderStageBinding binding in bindings)
        {
            ShaderIrStageInput input = stage.inputs.Single(value => value.id == binding.id);
            if (input.kind != ShaderIrInputKind.Storage && !records.ContainsKey(binding.nativeName))
                records.Add(binding.nativeName, EncodeGlslUniform(input, binding.nativeName));
        }

        if (records.Count > ushort.MaxValue)
            throw new InvalidDataException("The compiled GLSL uniform table exceeds the BGFX binary limit.");
        using var output = new MemoryStream();
        output.Write(binary.AsSpan(0, C_SHADER_BINARY_HEADER_SIZE));
        Span<byte> encodedCount = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(encodedCount, (ushort)records.Count);
        output.Write(encodedCount);
        foreach (byte[] record in records.OrderBy(static pair => pair.Key, StringComparer.Ordinal).Select(static pair => pair.Value))
            output.Write(record);
        output.Write(binary.AsSpan(offset));
        return output.ToArray();
    }

    private static byte[] EncodeGlslUniform(
        ShaderIrStageInput input,
        string nativeName
    ) {
        byte[] name = Encoding.UTF8.GetBytes(nativeName);
        int count = input.type.elementType is null ? 1 : input.type.elementCount;
        string typeId = input.type.elementType?.id ?? input.type.id;
        NativeBgfx.UniformType type = input.kind == ShaderIrInputKind.SampledTexture
            ? NativeBgfx.UniformType.Sampler
            : typeId switch
            {
                "float" or "float2" or "float3" or "float4" => NativeBgfx.UniformType.Vec4,
                "float3x3" => NativeBgfx.UniformType.Mat3,
                "float4x4" => NativeBgfx.UniformType.Mat4,
                _ => throw new InvalidDataException($"The compiled GLSL uniform '{nativeName}' has unsupported storage '{typeId}'.")
            };
        if (name.Length == 0 || name.Length > byte.MaxValue || count <= 0 || count > byte.MaxValue)
            throw new InvalidDataException($"The compiled GLSL uniform '{nativeName}' exceeds the BGFX binary limits.");
        int registerCount = count * (type == NativeBgfx.UniformType.Mat3 ? 3 : type == NativeBgfx.UniformType.Mat4 ? 4 : 1);
        byte[] record = new byte[1 + name.Length + C_SHADER_UNIFORM_METADATA_SIZE];
        record[0] = (byte)name.Length;
        name.CopyTo(record, 1);
        Span<byte> metadata = record.AsSpan(1 + name.Length);
        metadata[0] = (byte)type;
        metadata[1] = (byte)count;
        BinaryPrimitives.WriteUInt16LittleEndian(metadata[4..], (ushort)registerCount);
        return record;
    }
}
