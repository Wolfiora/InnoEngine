using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Benchmark.Services;
using Inno.Content;
using Inno.Core.Diagnostics;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Runtime;
using Inno.Testing.Content;

namespace Inno.Benchmark.Rendering;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class RenderResourceBenchmarks
{
    private readonly byte[] m_pixels = new byte[320 * 256 * 4];
    private readonly RenderPersistentResourceID m_id = new("bench.raw");
    private readonly RenderTextureDescriptor m_descriptor = new(320, 256, RenderTextureFormat.RGBA8, RenderTextureUsage.Sampled);
    private BenchmarkServices m_services = null!;
    private DiagnosticReporter m_reporter = null!;
    private RenderRuntime m_runtime = null!;
    private CountingStore m_content = null!;
    private CountingRenderDevice m_device = null!;
    private TextureAsset m_texture = null!;
    private MaterialAsset m_material = null!;

    [IterationSetup(Targets = [nameof(FirstTexture), nameof(FirstMaterial), nameof(ConstructInput)])]
    public void Setup()
    {
        m_services = new BenchmarkServices(typeof(RenderResourceBenchmarks).Assembly);
        m_device = new CountingRenderDevice();
        m_reporter = m_services.diagnostics.CreateReporter(new DiagnosticSource("bench.render", "Render benchmark"));
        m_texture = new TextureAsset(1, 1, TextureColorSpace.Srgb, "png");
        m_services.identities.InitializePersistentIdentity(m_texture, Guid.NewGuid());
        var shader = new ShaderAsset();
        m_services.identities.InitializePersistentIdentity(shader, Guid.NewGuid());
        var definition = new ShaderDefinition(
            "Benchmark",
            [],
            [],
            [new ShaderPassDefinition("main", ShaderProgramKind.Raster)],
            [new ShaderTechniqueDefinition(
                new ShaderTechniqueID("default"),
                new ShaderContractID("bench.render"),
                [new ShaderTechniquePass(new ShaderPassRoleID("draw"), "main")]
            )]
        );
        shader.SetDefinition(definition, m_services.serialization, SerializationContext.empty);
        m_material = new MaterialAsset { shader = shader };
        var artifact = new RenderShaderArtifact(
            "Benchmark",
            "bench.metal",
            RenderShaderVariant.empty,
            new ShaderInterface([]),
            [new RenderShaderPassArtifact(
                "main",
                ShaderProgramKind.Raster,
                new RenderRasterState(),
                new ShaderInterface([]),
                [new RenderShaderStageArtifact(ShaderStage.Vertex, [1]), new RenderShaderStageArtifact(ShaderStage.Fragment, [1])]
            )],
            m_services.serialization.Serialize(definition, SerializationContext.empty)
        );
        var payloads = new Dictionary<ContentKey, byte[]>
        {
            [new ContentKey(RenderTargetArtifactPath.GetTexturePath(m_texture.GetTextureArtifactReference()))] = m_pixels,
            [new ContentKey(
                RenderTargetArtifactPath.GetShaderPath(
                    shader.identity.persistentID,
                    m_device.capabilities.backend,
                    RenderShaderVariant.empty
                )
            )] = RenderShaderArtifactCodec.Encode(artifact)
        };
        m_content = new CountingStore(new ContentTestStore(payloads));
        m_runtime = new RenderRuntime(
            m_services.types,
            m_device,
            m_reporter,
            targetArtifacts:
                new ContentRenderTargetArtifactProvider(m_content, m_services.serialization, SerializationContext.empty)
        );
        m_runtime.Attach();
    }

    [IterationSetup(Target = nameof(RepeatTexture))]
    public void WarmTexture()
    {
        Setup();
        FirstTexture();
    }

    [IterationSetup(Target = nameof(RepeatMaterial))]
    public void WarmMaterial()
    {
        Setup();
        FirstMaterial();
    }

    [IterationSetup(Targets = [nameof(SameRevision), nameof(NextRevision)])]
    public void WarmRaw()
    {
        Setup();
        Acquire(1);
    }

    [Benchmark]
    public void FirstTexture() => m_runtime.resources.PrewarmTexture(m_texture);

    [Benchmark]
    public void RepeatTexture() => FirstTexture();

    [Benchmark]
    public void FirstMaterial()
    {
        if (m_runtime.resources.PrewarmMaterial(m_material) != RenderTargetArtifactStatus.Ready)
        {
            throw new InvalidOperationException("Material artifact unavailable.");
        }
    }

    [Benchmark]
    public void RepeatMaterial() => FirstMaterial();

    [Benchmark]
    public RenderTextureSubresourceData ConstructInput() => new(0, 0, m_pixels);

    [Benchmark]
    public PersistentTextureHandle SameRevision() => Acquire(1);

    [Benchmark]
    public PersistentTextureHandle NextRevision() => Acquire(2);

    [IterationCleanup]
    public void Cleanup()
    {
        m_runtime.Dispose();
        m_content.Dispose();
        m_device.Dispose();
        m_reporter.Dispose();
        m_services.Dispose();
    }

    internal (long opens, long bytes, int creates, int uploads) Counters()
        => (m_content.opens, m_content.bytes, m_device.creates, m_device.uploads);

    private PersistentTextureHandle Acquire(long revision)
        => m_runtime.resources.AcquireTexture(m_id, revision, m_descriptor, [ConstructInput()], "Bench raw");

    private sealed class CountingStore(ContentTestStore store) : IRuntimeContentStore
    {
        public long opens;
        public long bytes;
        public ContentPackIndex index => store.index;
        public ContentPackDescriptor descriptor => store.descriptor;

        public ContentReadLease Acquire(ContentKey key)
        {
            ContentReadLease lease = store.Acquire(key);
            return new ContentReadLease(lease.entry, () =>
            {
                Stream stream = lease.OpenRead();
                opens++;
                return new ReadCounter(stream, n => bytes += n);
            }, lease.Dispose);
        }

        public void Dispose() => store.Dispose();
    }
}
