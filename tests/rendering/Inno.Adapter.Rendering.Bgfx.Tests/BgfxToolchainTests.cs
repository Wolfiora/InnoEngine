using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Rendering.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering.Assets;
using Inno.Rendering;
using Xunit;

namespace Inno.Adapter.Rendering.Bgfx.Tests;

public sealed class BgfxToolchainTests : IDisposable
{
    private static readonly byte[] C_KTX_IDENTIFIER =
    [
        0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31,
        0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A
    ];

    private readonly string m_root = Path.Combine(
        Path.GetTempPath(),
        "InnoBgfxToolchainTests",
        Guid.NewGuid().ToString("N"));

    public BgfxToolchainTests() => Directory.CreateDirectory(m_root);

    [Fact]
    public void MetalProfileIsOwnedByMacBgfxToolchain()
    {
        GraphicsCapabilities capabilities = CreateCapabilities(
            GraphicsApi.Metal,
            GraphicsCapability.Compute);
        var toolchain = new BgfxShadercToolchain(BgfxShaderTargetPlatform.MacOSArm64);

        ShaderCompileTarget target = toolchain.CreateTarget(capabilities);

        Assert.Contains("bgfx-shaderc:MacOSArm64:Metal", target.profileKey, StringComparison.Ordinal);
        Assert.Throws<NotSupportedException>(() =>
            new BgfxShadercToolchain(BgfxShaderTargetPlatform.WindowsX64).CreateTarget(capabilities));
    }

    [Fact]
    public void Direct3DAndMetalTargetsSelectProfilesWithoutForkingShaderSource()
    {
        ShaderCompileTarget metal = new BgfxShadercToolchain(BgfxShaderTargetPlatform.MacOSArm64)
            .CreateTarget(CreateCapabilities(GraphicsApi.Metal, GraphicsCapability.Compute));
        ShaderCompileTarget direct3D = new BgfxShadercToolchain(BgfxShaderTargetPlatform.WindowsX64)
            .CreateTarget(CreateCapabilities(GraphicsApi.Direct3D11, GraphicsCapability.Compute));

        Assert.Contains(":Metal:", metal.profileKey, StringComparison.Ordinal);
        Assert.Contains(":Direct3D11:", direct3D.profileKey, StringComparison.Ordinal);
        Assert.NotEqual(metal.profileKey, direct3D.profileKey);
    }

    [Fact]
    public void BrowserProfileRequiresWebGlWithoutCompute()
    {
        var toolchain = new BgfxShadercToolchain(BgfxShaderTargetPlatform.BrowserWasm);

        ShaderCompileTarget target = toolchain.CreateTarget(
            CreateCapabilities(GraphicsApi.OpenGLES, GraphicsCapability.None));

        Assert.Contains("bgfx-shaderc:BrowserWasm:OpenGLES:300_es:300_es", target.profileKey, StringComparison.Ordinal);
        Assert.Throws<NotSupportedException>(() => toolchain.CreateTarget(
            CreateCapabilities(GraphicsApi.OpenGLES, GraphicsCapability.Compute)));
        Assert.Throws<NotSupportedException>(() => toolchain.CreateTarget(
            CreateCapabilities(GraphicsApi.OpenGL, GraphicsCapability.None)));
    }

    [Fact]
    public async Task BrowserProfileCompilesTheCommonGraphOnWindows()
    {
        if (!OperatingSystem.IsWindows())
            return;
        using var modules = new ModuleHost(new() { cacheDirectory = Path.Combine(m_root, "Modules") });
        using var types = new TypeCatalog(modules);
        using var serialization = new SerializationRegistry(types);
        using var nodes = new ShaderNodeCompilerRegistry(types);
        GraphDocument graph = ShaderGraphTemplates.CreateRaster(serialization, SerializationContext.empty);
        ShaderGraphProgramResult program = new ShaderGraphProgramCompiler(nodes).Lower(graph, "bgfx",
            new Dictionary<GraphNodeId, ShaderSourceModuleAnalysis>(), serialization, SerializationContext.empty);
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(BgfxShaderTargetPlatform.BrowserWasm));

        ShaderCompilationResult result = await compiler.CompileAsync(
            ShaderGraphDocument.ReadDefinition(graph, serialization, SerializationContext.empty),
            program,
            compiler.CreateTarget(CreateCapabilities(GraphicsApi.OpenGLES, GraphicsCapability.None)),
            RenderShaderVariant.empty,
            serialization, SerializationContext.empty);

        Assert.True(result.succeeded, string.Join(
            Environment.NewLine,
            result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")));
        Assert.Single(result.artifact!.passes);
    }

    [Fact]
    public async Task Direct3DProfileCompilesTheCommonGraphOnWindows()
    {
        if (!OperatingSystem.IsWindows())
            return;
        using var modules = new ModuleHost(new() { cacheDirectory = Path.Combine(m_root, "Modules") });
        using var types = new TypeCatalog(modules);
        using var serialization = new SerializationRegistry(types);
        using var nodes = new ShaderNodeCompilerRegistry(types);
        GraphDocument graph = ShaderGraphTemplates.CreateRaster(serialization, SerializationContext.empty);
        ShaderGraphProgramResult program = new ShaderGraphProgramCompiler(nodes).Lower(graph, "bgfx",
            new Dictionary<GraphNodeId, ShaderSourceModuleAnalysis>(), serialization, SerializationContext.empty);
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(BgfxShaderTargetPlatform.WindowsX64));

        ShaderCompilationResult result = await compiler.CompileAsync(
            ShaderGraphDocument.ReadDefinition(graph, serialization, SerializationContext.empty),
            program,
            compiler.CreateTarget(CreateCapabilities(GraphicsApi.Direct3D11, GraphicsCapability.None)),
            RenderShaderVariant.empty,
            serialization, SerializationContext.empty);

        Assert.True(result.succeeded, string.Join(
            Environment.NewLine,
            result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")));
        Assert.Single(result.artifact!.passes);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GlslReflectionPreservesUsedBindingsAndRemovesOptimizedBindings(bool browser)
    {
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS())
            return;
        GraphicsApi backend = browser ? GraphicsApi.OpenGLES : GraphicsApi.OpenGL;
        var builder = new ShaderIrBuilder();
        ShaderSourceType colorType = ShaderSourceType.Atomic("float4");
        ShaderIrValue color = builder.Input("used-tint", colorType);
        _ = builder.Input("unused-tint", colorType);
        ShaderSourceType textureType = ShaderSourceType.Atomic("sampled-texture2d");
        ShaderIrValue texture = builder.Input("sampled", textureType);
        ShaderIrValue coordinate = builder.Construct(ShaderSourceType.Atomic("float2"),
            [builder.Constant(0.5f), builder.Constant(0.5f)]);
        ShaderIrValue output = builder.Binary(ShaderIrOperation.Multiply, color, builder.Sample(texture, coordinate));
        var stage = new ShaderIrStage(
            ShaderStage.Fragment,
            builder.Build(new Dictionary<string, ShaderIrValue> { ["color"] = output }),
            [
                new("used-tint", colorType, ShaderIrInputKind.Uniform),
                new("unused-tint", colorType, ShaderIrInputKind.Uniform),
                new("sampled", textureType, ShaderIrInputKind.SampledTexture)
            ],
            [new("color", ShaderIrOutputKind.Color)]);
        BgfxShaderTargetPlatform platform = backend == GraphicsApi.OpenGLES
            ? BgfxShaderTargetPlatform.BrowserWasm
            : OperatingSystem.IsWindows()
                ? BgfxShaderTargetPlatform.WindowsX64
                : BgfxShaderTargetPlatform.MacOSArm64;
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(platform));

        ShaderStageToolResult result = await compiler.CompileAsync(
            stage,
            compiler.CreateTarget(CreateCapabilities(backend, GraphicsCapability.None)));

        Assert.True(result.succeeded, string.Join(
            Environment.NewLine,
            result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")));
        Assert.Equal(new[] { "sampled", "used-tint" }, result.bindings.Select(static binding => binding.id).Order().ToArray());
        Dictionary<string, byte> uniforms = ReadBinaryUniformTypes(result.bytes.Span);
        Assert.Equal(2, uniforms.Count);
        Assert.Equal(0, uniforms[result.bindings.Single(static binding => binding.id == "sampled").nativeName]);
        Assert.Equal(2, uniforms[result.bindings.Single(static binding => binding.id == "used-tint").nativeName]);
    }

    [Fact]
    public async System.Threading.Tasks.Task TextureCompilerProducesValidatedPortableContainer()
    {
        string sourcePath = Path.Combine(m_root, "checker.tga");
        File.WriteAllBytes(sourcePath, CreateTga());

        byte[] artifact = await new BgfxTextureTargetCompiler().CompileKtxAsync(
            sourcePath,
            TextureColorSpace.Srgb);

        Assert.True(artifact.Length > C_KTX_IDENTIFIER.Length);
        Assert.Equal(C_KTX_IDENTIFIER, artifact[..C_KTX_IDENTIFIER.Length]);
    }

    /// <summary>
    /// Compiles sparse color outputs through the real desktop and WebGL shader toolchains.
    /// </summary>
    /// <param name="browser">
    /// Selects ESSL instead of the desktop GLSL profile.
    /// </param>
    /// <returns>
    /// An operation that completes after validating the compiled output declarations.
    /// </returns>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task GlslFragmentOutputsPreserveSparseAttachmentLocations(bool browser)
    {
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS())
            return;
        GraphicsApi backend = browser ? GraphicsApi.OpenGLES : GraphicsApi.OpenGL;
        BgfxShaderTargetPlatform platform = browser ? BgfxShaderTargetPlatform.BrowserWasm
            : OperatingSystem.IsWindows() ? BgfxShaderTargetPlatform.WindowsX64 : BgfxShaderTargetPlatform.MacOSArm64;
        var builder = new ShaderIrBuilder();
        ShaderIrValue color = builder.Construct(ShaderSourceType.Atomic("float4"),
            [builder.Constant(0.25f), builder.Constant(0.5f), builder.Constant(0.75f), builder.Constant(1f)]);
        var stage = new ShaderIrStage(
            ShaderStage.Fragment,
            builder.Build(new Dictionary<string, ShaderIrValue> { ["first"] = color, ["third"] = color }),
            [],
            [new("first", ShaderIrOutputKind.Color, location: 0), new("third", ShaderIrOutputKind.Color, location: 2)]);
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(platform));

        ShaderStageToolResult result = await compiler.CompileAsync(stage,
            compiler.CreateTarget(CreateCapabilities(backend, GraphicsCapability.None)));

        Assert.True(result.succeeded, string.Join(Environment.NewLine,
            result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")));
        string glsl = Encoding.UTF8.GetString(result.bytes.Span);
        if (browser)
        {
            Assert.Contains("bgfx_FragData[0]", glsl);
            Assert.Contains("bgfx_FragData[2]", glsl);
        }
        else
        {
            Assert.Contains("layout(location = 0) out vec4 inno_fragment_color_0", glsl);
            Assert.Contains("layout(location = 2) out vec4 inno_fragment_color_2", glsl);
            Assert.DoesNotContain("gl_FragData", glsl);
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private static Dictionary<string, byte> ReadBinaryUniformTypes(ReadOnlySpan<byte> binary)
    {
        ushort count = BinaryPrimitives.ReadUInt16LittleEndian(binary[12..]);
        int offset = 14;
        var types = new Dictionary<string, byte>(StringComparer.Ordinal);
        for (int index = 0; index < count; index++)
        {
            int nameLength = binary[offset++];
            string name = Encoding.UTF8.GetString(binary.Slice(offset, nameLength));
            offset += nameLength;
            types.Add(name, binary[offset]);
            offset += 10;
        }
        return types;
    }

    private static GraphicsCapabilities CreateCapabilities(
        GraphicsApi backend,
        GraphicsCapability features
    ) => new(
            backend,
            features,
            new GraphicsLimits(256, 8, 8192, 16),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            Enum.GetValues<RenderTextureFormat>(),
            originBottomLeft: false,
            homogeneousDepth: false);

    private static byte[] CreateTga()
    {
        byte[] bytes = new byte[18 + (2 * 2 * 4)];
        bytes[2] = 2;
        bytes[12] = 2;
        bytes[14] = 2;
        bytes[16] = 32;
        bytes[17] = 0x28;

        ReadOnlySpan<byte> pixels =
        [
            0x00, 0x00, 0xFF, 0xFF,
            0x00, 0xFF, 0x00, 0xFF,
            0xFF, 0x00, 0x00, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF
        ];
        pixels.CopyTo(bytes.AsSpan(18));
        return bytes;
    }
}
