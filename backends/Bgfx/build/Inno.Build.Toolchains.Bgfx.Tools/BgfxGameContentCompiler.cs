using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Extensibility.Types;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Produces source-free BGFX shader and texture artifacts for one Player target.
/// </summary>
public sealed class BgfxGameContentCompiler : IGameContentCompiler
{
    private readonly AssetPipeline m_assets;
    private readonly TypeCatalog m_types;
    private readonly BgfxShaderTargetProfile m_platform;
    private readonly GraphicsApi[] m_backends;
    private readonly SerializationRegistry m_serialization;

    /// <summary>
    /// Creates a compiler using explicit target profiles and the selected runtime API closure.
    /// </summary>
    /// <param name="assets">
    /// The active authoring asset owner.
    /// </param>
    /// <param name="serialization">
    /// The matching serialization registry.
    /// </param>
    /// <param name="types">
    /// The matching extension catalog.
    /// </param>
    /// <param name="platform">
    /// The immutable platform-contributed shader configuration.
    /// </param>
    /// <param name="backends">
    /// The nonempty renderer closure to publish.
    /// </param>
    /// <exception name="ArgumentException">
    /// A renderer is missing, duplicated or not deployable.
    /// </exception>
    public BgfxGameContentCompiler(
        AssetPipeline assets,
        SerializationRegistry serialization,
        TypeCatalog types,
        BgfxShaderTargetProfile platform,
        IEnumerable<GraphicsApi> backends
    ) {
        ArgumentNullException.ThrowIfNull(assets);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(backends);
        GraphicsApi[] snapshot = backends.Distinct().ToArray();
        if (snapshot.Length == 0)
            throw new ArgumentException("At least one Player graphics backend is required.", nameof(backends));
        if (snapshot.Contains(GraphicsApi.Noop))
            throw new ArgumentException("A deployable Player cannot target the Noop graphics backend.", nameof(backends));
        m_assets = assets;
        m_types = types ?? throw new ArgumentNullException(nameof(types));
        m_serialization = serialization;
        ArgumentNullException.ThrowIfNull(platform);
        foreach (GraphicsApi backend in snapshot)
            platform.Resolve(backend);
        m_platform = platform;
        m_backends = snapshot;
    }

    /// <inheritdoc />
    public BuildTargetId target => new(m_platform.id);

    /// <summary>
    /// Captures the active Asset generation and compiles every required runtime variant.
    /// </summary>
    /// <param name="context">
    /// The target staging path and immutable build generation services.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation observed before every compiler invocation and artifact write.
    /// </param>
    /// <returns>
    /// An operation that completes when the source-free target closure is staged.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when an asset has no stable identity, source mount, or valid target compilation result.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The build context is null.
    /// </exception>
    public async ValueTask CompileAsync(
        GameBuildContentContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        if (context.profile.target != target)
            throw new ArgumentException("The content context target does not match the compiler.", nameof(context));
        ContentSnapshot snapshot = CaptureSnapshot();
        string outputRoot = Path.GetFullPath(context.outputDirectory);
        Directory.CreateDirectory(outputRoot);
        var shaderCompiler = new ShaderCompiler(new BgfxShadercToolchain(m_platform));
        foreach (GraphicsApi backend in m_backends.Order())
        {
            GraphicsCapabilities capabilities = m_platform.Resolve(backend).capabilities;
            ShaderCompileTarget target = shaderCompiler.CreateTarget(
                capabilities,
                optimize: true,
                debugInformation: false);
            foreach (ShaderInput shader in snapshot.shaders)
            {
                foreach (RenderShaderVariant variant in shader.variants.OrderBy(static value => value.value, StringComparer.Ordinal))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    ShaderCompilationResult result = await shaderCompiler.CompileGraphAsync(
                            shader.asset,
                            target,
                            variant,
                            m_types, m_serialization, AssetSerializationContext.Create(m_assets), m_assets,
                            cancellationToken)
                        .ConfigureAwait(false);
                    if (!result.succeeded)
                    {
                        string diagnostics = string.Join(
                            Environment.NewLine,
                            result.diagnostics.Select(static value => $"[{value.code}] {value.message}"));
                        throw new InvalidOperationException(
                            $"Shader '{shader.asset.assetPath}' failed target compilation for '{backend}':{Environment.NewLine}" +
                            diagnostics);
                    }
                    string destination = ResolveOutput(
                        outputRoot,
                        RenderTargetArtifactPath.GetShaderPath(
                            shader.asset.identity.persistentId,
                            backend,
                            variant));
                    Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                    await File.WriteAllBytesAsync(
                            destination,
                            RenderShaderArtifactCodec.Encode(result.artifact!.CreateRuntimeArtifact()),
                            cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }

        var textureCompiler = new BgfxTextureTargetCompiler();
        foreach (TextureInput texture in snapshot.textures)
        {
            cancellationToken.ThrowIfCancellationRequested();
            byte[] bytes;
            using (ArtifactLease source = m_assets.AcquireArtifact(
                texture.reference.assetId,
                texture.reference.slot.sourceOutputName))
            {
                using Stream encoded = source.OpenRead();
                bytes = await textureCompiler.CompileKtxAsync(
                        encoded,
                        texture.reference.slot.colorSpace,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            string destination = ResolveOutput(
                outputRoot,
                RenderTargetArtifactPath.GetTexturePath(texture.reference));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            await File.WriteAllBytesAsync(destination, bytes, cancellationToken).ConfigureAwait(false);
        }
    }

    private ContentSnapshot CaptureSnapshot()
    {
        if (!m_assets.isInitialized)
            throw new InvalidOperationException("BGFX target compilation requires an active authoring Asset database.");
        Dictionary<AssetSourceId, AssetSourceMount> mounts = m_assets.sourceMounts.ToDictionary(
            static mount => mount.id);
        var shaders = new Dictionary<Guid, ShaderInput>();
        var textures = new Dictionary<TextureArtifactKey, TextureInput>();
        var materialVariants = new List<(ShaderAsset shader, RenderShaderVariant variant)>();
        AssetFileEntry[] entries = m_assets.GetFileSystemEntries(includeDirectories: false)
            .OrderBy(static entry => entry.assetPath.ToString(), StringComparer.Ordinal)
            .ToArray();
        foreach (AssetFileEntry entry in entries)
        {
            if (!m_assets.TryGetAssetType(entry.assetPath, out Type? assetType) || assetType is null)
                continue;
            if (typeof(MaterialAsset).IsAssignableFrom(assetType))
            {
                MaterialAsset material = m_assets.Load<MaterialAsset>(entry.assetPath);
                if (material.shader is not null)
                    materialVariants.Add((material.shader, RenderShaderVariant.FromMaterial(material)));
            }
            if (typeof(ShaderAsset).IsAssignableFrom(assetType))
            {
                ShaderAsset shader = m_assets.Load<ShaderAsset>(entry.assetPath);
                // Passless graphs are reusable authoring nodes, not executable shaders.
                if (shader.definition is { passes.Length: > 0 })
                    AddShader(shaders, mounts, shader, RenderShaderVariant.empty);
            }
            if (typeof(IRenderTextureArtifactSource).IsAssignableFrom(assetType))
            {
                AssetObject asset = m_assets.Load(entry.assetPath, assetType);
                AddTextures(textures, asset);
            }
        }
        foreach ((ShaderAsset shader, RenderShaderVariant variant) in materialVariants)
            AddShader(shaders, mounts, shader, variant);
        return new ContentSnapshot(
            shaders.Values.OrderBy(static value => value.asset.identity.persistentId).ToArray(),
            textures.Values
                .OrderBy(static value => value.reference.assetId)
                .ThenBy(static value => value.reference.slot.id, StringComparer.Ordinal)
                .ToArray());
    }

    private static void AddTextures(
        IDictionary<TextureArtifactKey, TextureInput> textures,
        AssetObject asset
    ) {
        if (asset is not IRenderTextureArtifactSource source)
            return;
        Guid id = RequireIdentity(asset);
        var slots = new HashSet<string>(StringComparer.Ordinal);
        foreach (RenderTextureArtifactSlot slot in source.textureArtifacts)
        {
            if (string.IsNullOrWhiteSpace(slot.id) || string.IsNullOrWhiteSpace(slot.sourceOutputName))
            {
                throw new InvalidOperationException(
                    $"Texture source asset '{asset.assetPath}' declares an invalid artifact slot.");
            }
            if (!slots.Add(slot.id))
            {
                throw new InvalidOperationException(
                    $"Texture source asset '{asset.assetPath}' declares duplicate slot '{slot.id}'.");
            }
            var reference = new RenderTextureArtifactReference(id, asset.contentVersion, slot);
            textures.Add(new TextureArtifactKey(id, slot.id), new TextureInput(reference));
        }
    }

    private static void AddShader(
        IDictionary<Guid, ShaderInput> shaders,
        IReadOnlyDictionary<AssetSourceId, AssetSourceMount> mounts,
        ShaderAsset shader,
        RenderShaderVariant variant
    ) {
        Guid id = RequireIdentity(shader);
        if (!shaders.TryGetValue(id, out ShaderInput? input))
        {
            RequireMount(mounts, shader.assetPath);
            input = new ShaderInput(shader);
            shaders.Add(id, input);
        }
        input.variants.Add(variant);
    }

    private static Guid RequireIdentity(AssetObject asset)
    {
        Guid id = asset.identity.persistentId;
        if (id == Guid.Empty)
            throw new InvalidOperationException($"Runtime asset '{asset.assetPath}' has no persistent identity.");
        return id;
    }

    private static void RequireMount(
        IReadOnlyDictionary<AssetSourceId, AssetSourceMount> mounts,
        AssetPath path
    ) {
        if (!mounts.ContainsKey(path.source))
            throw new InvalidOperationException($"Asset source mount '{path.source}' is not active.");
    }

    private static string ResolveOutput(
        string root,
        string relativePath
    ) {
        string result = Path.GetFullPath(Path.Combine(root, relativePath));
        string prefix = Path.TrimEndingDirectorySeparator(root) + Path.DirectorySeparatorChar;
        StringComparison comparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!result.StartsWith(prefix, comparison))
            throw new InvalidOperationException("A target artifact path escaped its staging directory.");
        return result;
    }

    private sealed record ContentSnapshot(
        ShaderInput[] shaders,
        TextureInput[] textures
    );

    private sealed class ShaderInput
    {
        internal ShaderInput(ShaderAsset asset)
        {
            this.asset = asset;
        }

        internal ShaderAsset asset { get; }
        internal HashSet<RenderShaderVariant> variants { get; } = [];
    }

    private readonly record struct TextureArtifactKey(
        Guid assetId,
        string slotId
    );

    private sealed record TextureInput(RenderTextureArtifactReference reference);
}
