using Inno.Core.Diagnostics;
using System;
using System.Collections.Generic;

namespace Inno.Rendering;

internal static partial class RenderGraphValidator
{
    internal static RenderGraphValidationState Validate(
        GraphicsCapabilities capabilities,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlySet<RenderResourceKey> outputs
    ) {
        List<RenderGraphDiagnostic> diagnostics = [];
        ValidateResources(capabilities, textures, buffers, diagnostics);
        ValidatePasses(capabilities, textures, buffers, passes, diagnostics);
        List<HashSet<int>> dependencies = CreateEdgeSets(passes.Count);
        List<HashSet<int>> dataDependencies = CreateEdgeSets(passes.Count);
        BuildResourceDependencies(textures, buffers, passes, outputs, dependencies, dataDependencies, diagnostics);
        BuildPhaseDependencies(passes, dependencies);
        if (ContainsErrors(diagnostics))
            return CreateState(null);

        HashSet<int> livePasses = FindLivePasses(textures, buffers, passes, outputs, dataDependencies);
        List<int>? schedule = TopologicalSort(passes.Count, livePasses, dependencies);
        if (schedule is null)
        {
            diagnostics.Add(new RenderGraphDiagnostic(
                "RENDER_GRAPH_CYCLE",
                "Pass ordering and resource dependencies contain a cycle.",
                DiagnosticSeverity.Error));
            return CreateState(null);
        }
        if (schedule.Count > capabilities.limits.maxViews)
        {
            diagnostics.Add(new RenderGraphDiagnostic(
                "RENDER_GRAPH_VIEW_LIMIT",
                $"Graph requires {schedule.Count} views but the device supports {capabilities.limits.maxViews}.",
                DiagnosticSeverity.Error));
            return CreateState(null);
        }
        return CreateState(schedule.ToArray());

        RenderGraphValidationState CreateState(int[]? orderedPasses)
            => new(new RenderGraphValidationResult(
                orderedPasses is not null,
                diagnostics,
                passes.Count,
                orderedPasses?.Length ?? 0,
                textures.Count,
                buffers.Count), orderedPasses ?? []);
    }

    private static void ValidateResources(
        GraphicsCapabilities capabilities,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        foreach (RenderTextureRecord texture in textures)
        {
            RenderTextureDescriptor descriptor = texture.descriptor;
            if (descriptor.width > capabilities.limits.maxTextureSize
                || descriptor.height > capabilities.limits.maxTextureSize
                || descriptor.depth > capabilities.limits.maxTextureSize)
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_TEXTURE_LIMIT",
                    $"Texture '{texture.name}' exceeds the device texture extent limit.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }

            if (descriptor.dimension == RenderTextureDimension.Texture2D
                && descriptor.arrayLayers > 1
                && !capabilities.Supports(GraphicsCapability.Texture2DArray))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_TEXTURE_ARRAY_UNSUPPORTED",
                    $"Texture '{texture.name}' requires two-dimensional texture-array capability.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }

            if (descriptor.dimension == RenderTextureDimension.Texture3D
                && !capabilities.Supports(GraphicsCapability.Texture3D))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_TEXTURE_3D_UNSUPPORTED",
                    $"Texture '{texture.name}' requires three-dimensional texture capability.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }

            if (descriptor.dimension == RenderTextureDimension.Cube
                && descriptor.arrayLayers > 1
                && !capabilities.Supports(GraphicsCapability.TextureCubeArray))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_TEXTURE_CUBE_ARRAY_UNSUPPORTED",
                    $"Texture '{texture.name}' requires cubemap-array capability.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }

            bool attachmentUsage = (descriptor.usage
                & (RenderTextureUsage.ColorAttachment | RenderTextureUsage.DepthStencilAttachment)) != 0;
            if ((descriptor.usage & RenderTextureUsage.Sampled) != 0
                && !capabilities.SupportsSampled(descriptor.format, descriptor.dimension))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_FORMAT_SAMPLED_UNSUPPORTED",
                    $"Texture format '{descriptor.format}' is not supported for sampling.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }

            if (attachmentUsage && !capabilities.SupportsRenderTarget(descriptor.format))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_FORMAT_ATTACHMENT_UNSUPPORTED",
                    $"Texture format '{descriptor.format}' is not supported as an attachment.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }

            if (attachmentUsage
                && descriptor.sampleCount > 1
                && !capabilities.SupportsMultisampleRenderTarget(descriptor.format))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_FORMAT_MSAA_UNSUPPORTED",
                    $"Texture format '{descriptor.format}' is not supported as a multisampled attachment.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }

            if ((descriptor.usage & RenderTextureUsage.Storage) != 0
                && (!capabilities.Supports(GraphicsCapability.Compute)
                    || !capabilities.Supports(GraphicsCapability.StorageTexture)
                    || (!capabilities.SupportsStorage(descriptor.format, RenderStorageAccess.Read)
                        && !capabilities.SupportsStorage(descriptor.format, RenderStorageAccess.Write))))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_FORMAT_STORAGE_UNSUPPORTED",
                    $"Texture '{texture.name}' cannot be used for unordered shader access on this device.",
                    DiagnosticSeverity.Error,
                    resourceName: texture.name));
            }
        }

        foreach (RenderBufferRecord buffer in buffers)
        {
            if ((buffer.descriptor.usage & RenderBufferUsage.Storage) != 0
                && !capabilities.Supports(GraphicsCapability.StorageBuffer))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_STORAGE_BUFFER_UNSUPPORTED",
                    $"Buffer '{buffer.name}' requires storage-buffer capability.",
                    DiagnosticSeverity.Error,
                    resourceName: buffer.name));
            }


            if ((buffer.descriptor.usage & RenderBufferUsage.Index) != 0
                && buffer.descriptor.elementStride == sizeof(uint)
                && !capabilities.Supports(GraphicsCapability.Index32))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_INDEX32_UNSUPPORTED",
                    $"Buffer '{buffer.name}' requires unsigned 32-bit index capability.",
                    DiagnosticSeverity.Error,
                    resourceName: buffer.name));
            }
        }
    }

    private static void ValidatePasses(
        GraphicsCapabilities capabilities,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        IReadOnlyList<RenderPassRecord> passes,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        foreach (RenderPassRecord pass in passes)
        {
            if (pass.kind == RenderPassKind.Compute && !capabilities.Supports(GraphicsCapability.Compute))
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_COMPUTE_UNSUPPORTED",
                    $"Compute pass '{pass.name}' requires compute capability.",
                    DiagnosticSeverity.Error,
                    pass.name));
            }

            ValidatePassResourceConflicts(pass, diagnostics);
            ValidatePassResourceUsage(capabilities, textures, buffers, pass, diagnostics);
            if (pass.kind != RenderPassKind.Raster && pass.attachments.Count != 0)
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_ATTACHMENT_DOMAIN",
                    $"Pass '{pass.name}' is not a raster pass and cannot own attachments.",
                    DiagnosticSeverity.Error,
                    pass.name));
            }

            if (pass.surface.isValid && pass.kind != RenderPassKind.Raster)
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_SURFACE_DOMAIN",
                    $"Pass '{pass.name}' is not a raster pass and cannot target a presentation surface.",
                    DiagnosticSeverity.Error,
                    pass.name));
            }

            if (pass.surface.isValid && pass.attachments.Count != 0)
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_SURFACE_ATTACHMENTS",
                    $"Raster pass '{pass.name}' cannot target a presentation surface and texture attachments together.",
                    DiagnosticSeverity.Error,
                    pass.name));
            }

            if (pass.clearsPresentationTarget && pass.attachments.Count != 0)
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_PRESENTATION_CLEAR_ATTACHMENTS",
                    $"Raster pass '{pass.name}' cannot combine a presentation clear with texture attachments.",
                    DiagnosticSeverity.Error,
                    pass.name));
            }

            if (pass.attachments.Count == 0)
            {
                continue;
            }

            HashSet<int> colorSlots = [];
            HashSet<int> attachedTextures = [];
            int colorCount = 0;
            int depthCount = 0;
            RenderTextureDescriptor? firstDescriptor = null;
            foreach (RenderAttachment attachment in pass.attachments)
            {
                RenderTextureRecord texture = textures[attachment.texture.index];
                RenderTextureDescriptor descriptor = texture.descriptor;
                if (!attachedTextures.Add(attachment.texture.index))
                {
                    diagnostics.Add(new RenderGraphDiagnostic(
                        "RENDER_GRAPH_DUPLICATE_ATTACHMENT_RESOURCE",
                        $"Raster pass '{pass.name}' attaches texture '{texture.name}' more than once.",
                        DiagnosticSeverity.Error,
                        pass.name,
                        texture.name));
                }

                if (attachment.mipLevel >= descriptor.mipCount
                    || (attachment.mipLevel < descriptor.mipCount
                        && attachment.arrayLayer >= descriptor.GetSubresourceLayerCount(attachment.mipLevel)))
                {
                    diagnostics.Add(new RenderGraphDiagnostic(
                        "RENDER_GRAPH_ATTACHMENT_SUBRESOURCE",
                        $"Raster pass '{pass.name}' attachment subresource is outside texture '{texture.name}'.",
                        DiagnosticSeverity.Error,
                        pass.name,
                        texture.name));
                }

                if (attachment.isDepth)
                {
                    depthCount++;
                    if ((descriptor.usage & RenderTextureUsage.DepthStencilAttachment) == 0
                        || !IsDepthFormat(descriptor.format))
                    {
                        AddAttachmentUsageError(pass, texture, "depth-stencil", diagnostics);
                    }
                }
                else
                {
                    colorCount++;
                    if (!colorSlots.Add(attachment.slot))
                    {
                        diagnostics.Add(new RenderGraphDiagnostic(
                            "RENDER_GRAPH_DUPLICATE_ATTACHMENT",
                            $"Raster pass '{pass.name}' uses color attachment slot {attachment.slot} more than once.",
                            DiagnosticSeverity.Error,
                            pass.name,
                            texture.name));
                    }

                    if ((descriptor.usage & RenderTextureUsage.ColorAttachment) == 0
                        || IsDepthFormat(descriptor.format))
                    {
                        AddAttachmentUsageError(pass, texture, "color", diagnostics);
                    }
                }

                firstDescriptor ??= descriptor;
                int attachmentWidth = MipExtent(descriptor.width, attachment.mipLevel);
                int attachmentHeight = MipExtent(descriptor.height, attachment.mipLevel);
                int firstWidth = MipExtent(firstDescriptor.width, pass.attachments[0].mipLevel);
                int firstHeight = MipExtent(firstDescriptor.height, pass.attachments[0].mipLevel);
                if (firstWidth != attachmentWidth
                    || firstHeight != attachmentHeight
                    || firstDescriptor.sampleCount != descriptor.sampleCount)
                {
                    diagnostics.Add(new RenderGraphDiagnostic(
                        "RENDER_GRAPH_ATTACHMENT_MISMATCH",
                        $"Raster pass '{pass.name}' attachments must have equal width, height and sample count.",
                        DiagnosticSeverity.Error,
                        pass.name,
                        texture.name));
                }
            }

            if (colorCount > capabilities.limits.maxColorAttachments || depthCount > 1)
            {
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_ATTACHMENT_LIMIT",
                    $"Raster pass '{pass.name}' exceeds the device attachment limit.",
                    DiagnosticSeverity.Error,
                    pass.name));
            }
        }
    }

    private static int MipExtent(
        int extent,
        int mipLevel
    ) => mipLevel >= 31 ? 1 : Math.Max(1, extent >> mipLevel);

    private static void ValidatePassResourceConflicts(
        RenderPassRecord pass,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        Dictionary<RenderResourceKey, RenderResourceAccess> accessByResource = [];
        foreach (RenderResourceUse use in pass.resources)
        {
            if (!accessByResource.TryGetValue(use.key, out RenderResourceAccess previous))
            {
                accessByResource.Add(use.key, use.access);
                continue;
            }

            if (previous == use.access && use.access != RenderResourceAccess.ReadWrite)
            {
                continue;
            }

            diagnostics.Add(new RenderGraphDiagnostic(
                "RENDER_GRAPH_PASS_HAZARD",
                $"Pass '{pass.name}' declares conflicting access for one resource. Use an explicit ReadWrite declaration.",
                DiagnosticSeverity.Error,
                pass.name));
        }
    }

    private static void ValidatePassResourceUsage(
        GraphicsCapabilities capabilities,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        RenderPassRecord pass,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        foreach (RenderResourceUse use in pass.resources)
        {
            if (use.key.isTexture)
            {
                RenderTextureRecord texture = textures[use.key.index];
                RenderTextureUsage required = use.kind switch
                {
                    RenderResourceUseKind.GenericRead => RenderTextureUsage.Sampled,
                    RenderResourceUseKind.StorageRead
                        or RenderResourceUseKind.StorageWrite
                        or RenderResourceUseKind.StorageReadWrite
                        => RenderTextureUsage.Storage,
                    RenderResourceUseKind.CopySource => RenderTextureUsage.CopySource,
                    RenderResourceUseKind.CopyDestination => RenderTextureUsage.CopyDestination,
                    RenderResourceUseKind.ColorAttachment => RenderTextureUsage.ColorAttachment,
                    RenderResourceUseKind.DepthStencilAttachment
                        => RenderTextureUsage.DepthStencilAttachment,
                    _ => 0
                };
                if ((texture.descriptor.usage & required) != required)
                {
                    AddResourceUsageError(pass, texture.name, required.ToString(), diagnostics);
                }

                if (use.kind is RenderResourceUseKind.CopySource
                    or RenderResourceUseKind.CopyDestination
                    && !capabilities.Supports(GraphicsCapability.TextureBlit))
                {
                    AddCapabilityError(
                        pass,
                        texture.name,
                        "texture-copy",
                        GraphicsCapability.TextureBlit,
                        diagnostics);
                }

                if (use.kind is RenderResourceUseKind.StorageRead
                    or RenderResourceUseKind.StorageWrite
                    or RenderResourceUseKind.StorageReadWrite)
                {
                    RenderStorageAccess access = use.kind switch
                    {
                        RenderResourceUseKind.StorageRead => RenderStorageAccess.Read,
                        RenderResourceUseKind.StorageWrite => RenderStorageAccess.Write,
                        RenderResourceUseKind.StorageReadWrite => RenderStorageAccess.ReadWrite,
                        _ => throw new InvalidOperationException("Unexpected storage texture access.")
                    };
                    if (!capabilities.Supports(GraphicsCapability.StorageTexture)
                        || !capabilities.SupportsStorage(texture.descriptor.format, access))
                    {
                        diagnostics.Add(new RenderGraphDiagnostic(
                            "RENDER_GRAPH_STORAGE_TEXTURE_ACCESS_UNSUPPORTED",
                            $"Texture '{texture.name}' does not support {access} storage access required by pass '{pass.name}'.",
                            DiagnosticSeverity.Error,
                            pass.name,
                            texture.name));
                    }
                }

                continue;
            }

            RenderBufferUsage requiredBufferUsage = use.kind switch
            {
                RenderResourceUseKind.StorageRead
                    or RenderResourceUseKind.StorageWrite
                    or RenderResourceUseKind.StorageReadWrite
                    => RenderBufferUsage.Storage,
                RenderResourceUseKind.CopySource => RenderBufferUsage.CopySource,
                RenderResourceUseKind.CopyDestination => RenderBufferUsage.CopyDestination,
                _ => 0
            };
            if (requiredBufferUsage != 0)
            {
                // Buffer names and descriptors are validated by the overload below.
                ValidateBufferUse(
                    capabilities,
                    buffers,
                    pass,
                    use,
                    requiredBufferUsage,
                    diagnostics);
            }
        }
    }

    private static void ValidateBufferUse(
        GraphicsCapabilities capabilities,
        IReadOnlyList<RenderBufferRecord> buffers,
        RenderPassRecord pass,
        RenderResourceUse use,
        RenderBufferUsage requiredUsage,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        RenderBufferRecord buffer = buffers[use.key.index];
        if ((buffer.descriptor.usage & requiredUsage) != requiredUsage)
        {
            AddResourceUsageError(pass, buffer.name, requiredUsage.ToString(), diagnostics);
        }

        if (use.kind is RenderResourceUseKind.CopySource or RenderResourceUseKind.CopyDestination
            && !capabilities.Supports(GraphicsCapability.BufferCopy))
        {
            AddCapabilityError(
                pass,
                buffer.name,
                "buffer-copy",
                GraphicsCapability.BufferCopy,
                diagnostics);
        }
    }

    private static bool IsDepthFormat(RenderTextureFormat format)
        => format is RenderTextureFormat.Depth24Stencil8 or RenderTextureFormat.Depth32Float;

    private static bool ContainsErrors(IReadOnlyList<RenderGraphDiagnostic> diagnostics)
    {
        foreach (RenderGraphDiagnostic diagnostic in diagnostics)
        {
            if (diagnostic.severity == DiagnosticSeverity.Error)
            {
                return true;
            }
        }

        return false;
    }

    private static void AddAttachmentUsageError(
        RenderPassRecord pass,
        RenderTextureRecord texture,
        string role,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        diagnostics.Add(new RenderGraphDiagnostic(
            "RENDER_GRAPH_ATTACHMENT_USAGE",
            $"Texture '{texture.name}' is not declared for {role} attachment usage.",
            DiagnosticSeverity.Error,
            pass.name,
            texture.name));
    }

    private static void AddResourceUsageError(
        RenderPassRecord pass,
        string resourceName,
        string requiredUsage,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        diagnostics.Add(new RenderGraphDiagnostic(
            "RENDER_GRAPH_RESOURCE_USAGE",
            $"Resource '{resourceName}' used by pass '{pass.name}' requires '{requiredUsage}' usage.",
            DiagnosticSeverity.Error,
            pass.name,
            resourceName));
    }

    private static void AddCapabilityError(
        RenderPassRecord pass,
        string resourceName,
        string operation,
        GraphicsCapability requiredFeature,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        diagnostics.Add(new RenderGraphDiagnostic(
            "RENDER_GRAPH_CAPABILITY_UNSUPPORTED",
            $"Pass '{pass.name}' requires '{requiredFeature}' for {operation} operations.",
            DiagnosticSeverity.Error,
            pass.name,
            resourceName));
    }
}
