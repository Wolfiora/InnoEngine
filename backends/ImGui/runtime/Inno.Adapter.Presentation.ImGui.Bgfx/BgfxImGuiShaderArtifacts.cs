using System;
using System.IO;
using System.Linq;
using Inno.Rendering;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Loads the graph-built, source-free ImGui program distributed with this presentation adapter.
/// </summary>
public static class BgfxImGuiShaderArtifacts
{
    /// <summary>
    /// Reads the precompiled program for the declared product and requested graphics API without invoking a compiler.
    /// </summary>
    /// <param name="api">
    /// The active device API.
    /// </param>
    /// <returns>
    /// The validated texture binding, stages and render state required by ImGui.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The installed adapter lacks a required artifact or its binding contract is invalid.
    /// </exception>
    public static GraphicsPipelineDescriptor Load(GraphicsApi api)
    {
        string resource = $"Inno.BuiltInShaders.ImGui.{api}";
        using Stream source = typeof(BgfxImGuiShaderArtifacts).Assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidDataException($"Missing precompiled ImGui shader '{resource}'. Rebuild or repair the adapter installation; runtime source compilation is not supported.");
        using var bytes = new MemoryStream();
        source.CopyTo(bytes);
        GraphicsPipelineDescriptor program = GraphicsProgramArtifactCodec.Decode(bytes.ToArray(), BgfxImGuiRenderer.vertexLayout);
        var bindings = program.bindings;
        RenderShaderBindingDescriptor? texture = bindings.SingleOrDefault(static binding => binding.id.value == "s_tex");
        RenderShaderBindingDescriptor? outputEncoding = bindings.SingleOrDefault(static binding => binding.id.value == "outputEncoding");
        if (bindings.Count != 2 || texture is null || texture.kind != RenderShaderBindingKind.Texture
            || texture.slot != 0 || outputEncoding is null
            || outputEncoding.kind != RenderShaderBindingKind.Uniform)
            throw new InvalidDataException("The built-in ImGui graph must expose s_tex and the outputEncoding render-pass uniform.");
        return program;
    }
}
