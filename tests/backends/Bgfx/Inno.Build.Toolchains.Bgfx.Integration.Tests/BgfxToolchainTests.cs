using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;
using Inno.Testing.Bgfx;

using static Inno.Testing.Bgfx.ShaderProfileFixture;

namespace Inno.Build.Toolchains.Bgfx.Integration.Tests;

public sealed class BgfxToolchainTests : IDisposable
{
    private static readonly byte[] KTX_IDENTIFIER =
    [
        0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31,
        0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A
    ];

    private readonly string m_root = Path.Combine(
        Path.GetTempPath(),
        "InnoBgfxToolchainTests",
        Guid.NewGuid().ToString("N")
    );

    public BgfxToolchainTests()
    {
        Directory.CreateDirectory(m_root);
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public async Task BrowserProfileCompilesTheCommonGraphOnWindows()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        using var modules = new ModuleHost(new ModuleHostOptions()
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(ShaderNodeCompilerRegistry).Assembly)
        });
        using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        using var serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
        using var nodes = new ShaderNodeCompilerRegistry(types);
        GraphDocument graph = ShaderGraphTemplates.CreateRaster(serialization, SerializationContext.empty);
        ShaderGraphProgramResult program = new ShaderGraphProgramCompiler(nodes).Lower(
            graph,
            "bgfx",
            new Dictionary<GraphNodeID, ShaderSourceModuleAnalysis>(),
            serialization,
            SerializationContext.empty
        );
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(ShaderProfileFixture.browser));

        ShaderCompilationResult result = await compiler.CompileAsync(
            ShaderGraphDocument.ReadDefinition(graph, serialization, SerializationContext.empty),
            program,
            compiler.CreateTarget(CreateCapabilities(GraphicsApi.openGLES, GraphicsCapability.None)),
            RenderShaderVariant.empty,
            serialization,
            SerializationContext.empty
        );

        Assert.True(
            result.succeeded,
            string.Join(
                Environment.NewLine,
                result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")
            )
        );
        Assert.Single(result.artifact!.passes);
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public async Task Direct3DProfileCompilesTheCommonGraphOnWindows()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        using var modules = new ModuleHost(new ModuleHostOptions()
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(ShaderNodeCompilerRegistry).Assembly)
        });
        using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        using var serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
        using var nodes = new ShaderNodeCompilerRegistry(types);
        GraphDocument graph = ShaderGraphTemplates.CreateRaster(serialization, SerializationContext.empty);
        ShaderGraphProgramResult program = new ShaderGraphProgramCompiler(nodes).Lower(
            graph,
            "bgfx",
            new Dictionary<GraphNodeID, ShaderSourceModuleAnalysis>(),
            serialization,
            SerializationContext.empty
        );
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(ShaderProfileFixture.windows));

        ShaderCompilationResult result = await compiler.CompileAsync(
            ShaderGraphDocument.ReadDefinition(graph, serialization, SerializationContext.empty),
            program,
            compiler.CreateTarget(CreateCapabilities(GraphicsApi.direct3D11, GraphicsCapability.None)),
            RenderShaderVariant.empty,
            serialization,
            SerializationContext.empty
        );

        Assert.True(
            result.succeeded,
            string.Join(
                Environment.NewLine,
                result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")
            )
        );
        Assert.Single(result.artifact!.passes);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public async Task GlslReflectionPreservesUsedBindingsAndRemovesOptimizedBindings(bool browser)
    {
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS())
        {
            return;
        }
        GraphicsApi backend = browser ? GraphicsApi.openGLES : GraphicsApi.openGL;
        var builder = new ShaderIrBuilder();
        ShaderSourceType colorType = ShaderSourceType.Atomic("float4");
        ShaderIrValue color = builder.Input("used-tint", colorType);
        _ = builder.Input("unused-tint", colorType);
        ShaderSourceType textureType = ShaderSourceType.Atomic("sampled-texture2d");
        ShaderIrValue texture = builder.Input("sampled", textureType);
        ShaderIrValue coordinate = builder.Construct(
            ShaderSourceType.Atomic("float2"),
            [builder.Constant(0.5f), builder.Constant(0.5f)]
        );
        ShaderIrValue output = builder.Binary(ShaderIrOperation.Multiply, color, builder.Sample(texture, coordinate));
        var stage = new ShaderIrStage(
            ShaderStage.Fragment,
            builder.Build(new Dictionary<string, ShaderIrValue> { ["color"] = output }),
            [
                new ShaderIrStageInput("used-tint", colorType, ShaderIrInputKind.Uniform),
                new ShaderIrStageInput("unused-tint", colorType, ShaderIrInputKind.Uniform),
                new ShaderIrStageInput("sampled", textureType, ShaderIrInputKind.SampledTexture)
            ],
            [new ShaderIrStageOutput("color", ShaderIrOutputKind.Color)]
        );
        BgfxShaderTargetProfile platform = backend == GraphicsApi.openGLES
                                           ? ShaderProfileFixture.browser
                                           : OperatingSystem.IsWindows()
                                             ? ShaderProfileFixture.windows
                                             : ShaderProfileFixture.mac;
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(platform));

        ShaderStageToolResult result = await compiler.CompileAsync(
            stage,
            compiler.CreateTarget(CreateCapabilities(backend, GraphicsCapability.None))
        );

        Assert.True(
            result.succeeded,
            string.Join(
                Environment.NewLine,
                result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")
            )
        );
        Assert.Equal(new[] { "sampled", "used-tint" }, result.bindings.Select(static binding => binding.id).Order().ToArray());
        Dictionary<string, byte> uniforms = ReadBinaryUniformTypes(result.bytes.Span);
        Assert.Equal(2, uniforms.Count);
        Assert.Equal(0, uniforms[result.bindings.Single(static binding => binding.id == "sampled").nativeName]);
        Assert.Equal(2, uniforms[result.bindings.Single(static binding => binding.id == "used-tint").nativeName]);
    }

    [Fact]
    public async Task TextureCompilerProducesValidatedPortableContainer()
    {
        string sourcePath = Path.Combine(m_root, "checker.tga");
        File.WriteAllBytes(sourcePath, CreateTga());

        using FileStream source = File.OpenRead(sourcePath);
        byte[] artifact = await new BgfxTextureTargetCompiler().CompileKtxAsync(source, TextureColorSpace.Srgb);

        Assert.True(artifact.Length > KTX_IDENTIFIER.Length);
        Assert.Equal(KTX_IDENTIFIER, artifact[..KTX_IDENTIFIER.Length]);
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
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("macos")]
    public async Task GlslFragmentOutputsPreserveSparseAttachmentLocations(bool browser)
    {
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsMacOS())
        {
            return;
        }
        GraphicsApi backend = browser ? GraphicsApi.openGLES : GraphicsApi.openGL;
        BgfxShaderTargetProfile platform = browser
                                           ? ShaderProfileFixture.browser
                                           : OperatingSystem.IsWindows() ? ShaderProfileFixture.windows : ShaderProfileFixture.mac;
        var builder = new ShaderIrBuilder();
        ShaderIrValue color = builder.Construct(
            ShaderSourceType.Atomic("float4"),
            [builder.Constant(0.25f), builder.Constant(0.5f), builder.Constant(0.75f), builder.Constant(1f)]
        );
        var stage = new ShaderIrStage(
            ShaderStage.Fragment,
            builder.Build(new Dictionary<string, ShaderIrValue> { ["first"] = color, ["third"] = color }),
            [],
            [
                new ShaderIrStageOutput("first", ShaderIrOutputKind.Color, location: 0),
                new ShaderIrStageOutput("third", ShaderIrOutputKind.Color, location: 2)
            ]
        );
        var compiler = new ShaderCompiler(new BgfxShadercToolchain(platform));

        ShaderStageToolResult result = await compiler.CompileAsync(
            stage,
            compiler.CreateTarget(CreateCapabilities(backend, GraphicsCapability.None))
        );

        Assert.True(
            result.succeeded,
            string.Join(
                Environment.NewLine,
                result.diagnostics.Select(static diagnostic => $"{diagnostic.code}: {diagnostic.message}")
            )
        );
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
        {
            Directory.Delete(m_root, recursive: true);
        }
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

    private static byte[] CreateTga()
    {
        var bytes = new byte[18 + (2 * 2 * 4)];
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
