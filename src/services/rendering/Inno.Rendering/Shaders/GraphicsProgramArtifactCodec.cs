using System;
using System.IO;
using System.Text;

namespace Inno.Rendering;

/// <summary>
/// Transports compiled graphics programs and device binding facts independently of authored assets.
/// </summary>
public static class GraphicsProgramArtifactCodec
{
    private const int C_MAX_STAGE_BYTES = 128 * 1024 * 1024;
    private const int C_MAX_BINDINGS = 4096;
    private const int C_MAX_NAME_BYTES = 8192;
    private static readonly UTF8Encoding Utf8 = new(false, true);
    private static readonly byte[] Magic = "INNOGPU"u8.ToArray();

    /// <summary>
    /// Encodes owned program binaries, reflected bindings, and raster state for device distribution.
    /// Vertex layout belongs to the eventual caller and is not part of this artifact.
    /// </summary>
    /// <param name="descriptor">
    /// The compiled graphics program to distribute without any asset or material metadata.
    /// </param>
    /// <returns>
    /// Deterministic binary bytes owned by the caller.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The descriptor is null.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A stage, binding, or reflected name exceeds the distribution limits.
    /// </exception>
    public static byte[] Encode(GraphicsPipelineDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        using var output = new MemoryStream();
        using var writer = new BinaryWriter(output, Utf8, leaveOpen: true);
        writer.Write(Magic);
        WriteStage(writer, descriptor.vertexShader.Span);
        WriteStage(writer, descriptor.fragmentShader.Span);
        if (descriptor.bindings.Count > C_MAX_BINDINGS)
            throw new InvalidDataException("The graphics program contains too many bindings.");
        writer.Write(descriptor.bindings.Count);
        foreach (RenderShaderBindingDescriptor binding in descriptor.bindings)
        {
            WriteName(writer, binding.id.value);
            writer.Write((int)binding.kind);
            writer.Write(binding.slot);
            writer.Write((int)binding.uniformType);
            writer.Write(binding.count);
            writer.Write((int)binding.storageAccess);
            WriteName(writer, binding.nativeName);
        }
        WriteRaster(writer, descriptor.rasterState);
        writer.Flush();
        return output.ToArray();
    }

    /// <summary>
    /// Validates a complete device program artifact before constructing an owned pipeline descriptor.
    /// </summary>
    /// <param name="bytes">
    /// The complete current artifact, including its strict magic header.
    /// </param>
    /// <param name="vertexLayout">
    /// The caller's vertex layout, or null for a procedural pipeline.
    /// </param>
    /// <returns>
    /// A detached descriptor containing the validated binaries, bindings, and raster state.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The input is truncated, oversized, malformed, or contains trailing data.
    /// </exception>
    public static GraphicsPipelineDescriptor Decode(
        ReadOnlySpan<byte> bytes,
        RenderVertexLayout? vertexLayout = null
    ) {
        if (bytes.Length > 2L * C_MAX_STAGE_BYTES + 2L * C_MAX_BINDINGS * C_MAX_NAME_BYTES + 1024 * 1024)
            throw new InvalidDataException("The graphics program exceeds its input budget.");
        try
        {
            using var input = new MemoryStream(bytes.ToArray(), writable: false);
            using var reader = new BinaryReader(input, Utf8, leaveOpen: true);
            if (!ReadBytes(reader, Magic.Length).AsSpan().SequenceEqual(Magic))
                throw new InvalidDataException("The graphics program magic is invalid.");
            byte[] vertex = ReadBytes(reader, ReadCount(reader, C_MAX_STAGE_BYTES));
            byte[] fragment = ReadBytes(reader, ReadCount(reader, C_MAX_STAGE_BYTES));
            int count = ReadCount(reader, C_MAX_BINDINGS);
            var bindings = new RenderShaderBindingDescriptor[count];
            for (int index = 0; index < count; index++)
            {
                bindings[index] = new RenderShaderBindingDescriptor(
                    new RenderBindingId(ReadName(reader)),
                    ReadEnum<RenderShaderBindingKind>(reader),
                    reader.ReadInt32(),
                    ReadEnum<RenderUniformType>(reader),
                    reader.ReadInt32(),
                    ReadEnum<RenderStorageAccess>(reader),
                    ReadName(reader));
            }
            RenderRasterState raster = ReadRaster(reader);
            if (input.Position != input.Length)
                throw new InvalidDataException("The graphics program contains trailing data.");
            return new GraphicsPipelineDescriptor(vertex, fragment, bindings, vertexLayout, raster);
        }
        catch (InvalidDataException) { throw; }
        catch (Exception failure) when (failure is IOException or ArgumentException or OverflowException)
        {
            throw new InvalidDataException("The graphics program is malformed.", failure);
        }
    }

    private static void WriteStage(
        BinaryWriter writer,
        ReadOnlySpan<byte> bytes
    ) {
        if (bytes.IsEmpty || bytes.Length > C_MAX_STAGE_BYTES)
            throw new InvalidDataException("A graphics program stage has an invalid length.");
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    private static void WriteName(
        BinaryWriter writer,
        string value
    ) {
        byte[] bytes = Utf8.GetBytes(value);
        if (bytes.Length == 0 || bytes.Length > C_MAX_NAME_BYTES)
            throw new InvalidDataException("A graphics program binding name has an invalid length.");
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    private static string ReadName(BinaryReader reader)
        => Utf8.GetString(ReadBytes(reader, ReadCount(reader, C_MAX_NAME_BYTES)));

    private static int ReadCount(
        BinaryReader reader,
        int maximum
    ) {
        int count = reader.ReadInt32();
        if (count < 0 || count > maximum)
            throw new InvalidDataException("A graphics program field exceeds its length budget.");
        return count;
    }

    private static byte[] ReadBytes(
        BinaryReader reader,
        int count
    ) {
        if (count > reader.BaseStream.Length - reader.BaseStream.Position)
            throw new InvalidDataException("The graphics program is truncated.");
        byte[] bytes = reader.ReadBytes(count);
        if (bytes.Length != count)
            throw new InvalidDataException("The graphics program is truncated.");
        return bytes;
    }

    private static T ReadEnum<T>(BinaryReader reader) where T : struct, Enum
    {
        T value = (T)Enum.ToObject(typeof(T), reader.ReadInt32());
        return Enum.IsDefined(value) ? value : throw new InvalidDataException("A graphics program enum is invalid.");
    }

    private static bool ReadBoolean(BinaryReader reader)
        => reader.ReadByte() switch
        {
            0 => false,
            1 => true,
            _ => throw new InvalidDataException("A graphics program boolean is invalid.")
        };

    private static void WriteRaster(
        BinaryWriter writer,
        RenderRasterState state
    ) {
        if (!Enum.IsDefined(state.topology) || !Enum.IsDefined(state.cull)
            || !Enum.IsDefined(state.frontFace) || !Enum.IsDefined(state.depthCompare)
            || (state.colorWriteMask & ~0xF) != 0
            || !Enum.IsDefined(state.blend.colorSource) || !Enum.IsDefined(state.blend.colorDestination)
            || !Enum.IsDefined(state.blend.colorEquation) || !Enum.IsDefined(state.blend.alphaSource)
            || !Enum.IsDefined(state.blend.alphaDestination) || !Enum.IsDefined(state.blend.alphaEquation))
            throw new InvalidDataException("The graphics program raster state is invalid.");
        writer.Write((int)state.topology);
        writer.Write((int)state.cull);
        writer.Write((int)state.frontFace);
        writer.Write((int)state.depthCompare);
        writer.Write(state.depthWrite);
        writer.Write(state.colorWriteMask);
        writer.Write(state.multisampling);
        writer.Write(state.blend.enabled);
        writer.Write((int)state.blend.colorSource);
        writer.Write((int)state.blend.colorDestination);
        writer.Write((int)state.blend.colorEquation);
        writer.Write((int)state.blend.alphaSource);
        writer.Write((int)state.blend.alphaDestination);
        writer.Write((int)state.blend.alphaEquation);
        writer.Write(state.blend.constantRgba);
        writer.Write(state.blend.alphaToCoverage);
    }

    private static RenderRasterState ReadRaster(BinaryReader reader)
    {
        RenderPrimitiveTopology topology = ReadEnum<RenderPrimitiveTopology>(reader);
        RenderCullMode cull = ReadEnum<RenderCullMode>(reader);
        RenderFrontFace frontFace = ReadEnum<RenderFrontFace>(reader);
        RenderDepthCompare depthCompare = ReadEnum<RenderDepthCompare>(reader);
        bool depthWrite = ReadBoolean(reader);
        byte mask = reader.ReadByte();
        if ((mask & ~0xF) != 0)
            throw new InvalidDataException("A graphics program color mask is invalid.");
        bool multisampling = ReadBoolean(reader);
        var blend = new RenderBlendState
        {
            enabled = ReadBoolean(reader),
            colorSource = ReadEnum<RenderBlendFactor>(reader),
            colorDestination = ReadEnum<RenderBlendFactor>(reader),
            colorEquation = ReadEnum<RenderBlendEquation>(reader),
            alphaSource = ReadEnum<RenderBlendFactor>(reader),
            alphaDestination = ReadEnum<RenderBlendFactor>(reader),
            alphaEquation = ReadEnum<RenderBlendEquation>(reader),
            constantRgba = reader.ReadUInt32(),
            alphaToCoverage = ReadBoolean(reader)
        };
        return new RenderRasterState(cull, frontFace, depthCompare, depthWrite, blend, mask, multisampling, topology);
    }
}
