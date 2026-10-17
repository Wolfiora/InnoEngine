using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BGCS.Core.IO;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Bindings;

internal sealed class BindingGenerationOperation : IDisposable
{
    private readonly NativeBuildContext m_context;
    private readonly NativeBindingGenerationRequest m_request;
    private readonly string m_dotnetHost;
    private readonly List<FileLease> m_leases = [];
    private readonly List<IDisposable> m_reading = [];
    private readonly List<BindingGenerationPublication> m_publications = [];

    internal BindingGenerationOperation(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        string dotnetHost
    ) {
        m_context = context;
        m_request = request;
        m_dotnetHost = dotnetHost;
    }

    internal async ValueTask<NativeBindingGenerationResult> ExecuteAsync(CancellationToken cancellation)
    {
        BindingGenerationConfiguration[] definitions = m_request.components.Select(component =>
            BindingDefinitionReader.Read(m_context, component, m_request)).ToArray();
        Dictionary<string, string> extensions = new(StringComparer.Ordinal);
        foreach (string project in definitions.Select(static definition => definition.extensionProject)
            .OfType<string>().Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal))
        {
            BindingExtensionPreparation.Result extension = await BindingExtensionPreparation.PrepareAsync(m_context, project, m_dotnetHost, cancellation).ConfigureAwait(false);
            m_reading.Add(extension);
            extensions.Add(project, extension.assembly);
        }
        foreach (BindingGenerationConfiguration definition in definitions)
            definition.extensionAssembly = definition.extensionProject is null ? null : extensions[definition.extensionProject];
        string[] fingerprints = definitions.Select(definition => Compute(definition, m_context, cancellation)).ToArray();
        for (int index = 0; index < definitions.Length; index++)
            if (m_request.expectedFingerprints.TryGetValue(definitions[index].component.nativeProject, out string? expected)
                && expected != fingerprints[index])
                throw new InvalidOperationException("Binding inputs changed after their generation was selected.");
        // Batch leases are acquired in canonical path order. No partial closure is returned on failure.
        StringComparer paths = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        foreach (string path in definitions.Select(definition => Path.Combine(
            definition.component.GetNativeRoot(m_context.engineRoot), "obj", "bindings", "generation.lock")).Order(paths))
            m_leases.Add(await FileLease.AcquireAsync(path, Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false));
        if (m_request.outputMode == NativeBindingOutputMode.TargetArtifacts)
        {
            var cache = new BuildCachePublisher(Path.Combine(m_context.engineRoot, "artifacts", "bindings"), BuildCacheArtifactRole.ImmutableArtifact);
            m_leases.Add(await cache.AcquireUseAsync(cancellation).ConfigureAwait(false));
        }
        m_context.RecordBindingBatch();
        Verify(definitions, fingerprints, m_context.BeginInputVerification("binding-locks"), cancellation);
        Dictionary<string, NativeBindingGenerationDescriptor> results = new(StringComparer.Ordinal);
        HashSet<string> protectedTargets = new(StringComparer.Ordinal);
        for (int index = 0; index < definitions.Length; index++)
        {
            cancellation.ThrowIfCancellationRequested();
            BindingGenerationConfiguration definition = definitions[index];
            string fingerprint = fingerprints[index];
            string output = definition.GetOutputRoot(m_context);
            var loaded = definition.Load(m_context);
            if (m_request.outputMode == NativeBindingOutputMode.TargetArtifacts)
            {
                string destination = Path.Combine(output, fingerprint);
                try
                {
                    m_reading.Add(await BuildArtifactReadLease.AcquireAsync(destination, fingerprint,
                        ["Native", "Generated"], cancellation, m_context).ConfigureAwait(false));
                    protectedTargets.Add(destination);
                    if (!m_request.checkOnly)
                    {
                        results.Add(definition.component.nativeProject, TargetBindingGeneration.Describe(destination, fingerprint, loaded.bridge is not null));
                        continue;
                    }
                }
                catch (InvalidDataException)
                {
                    // Invalid output is repaired only after exclusive artifact ownership is acquired at commit.
                }
                var publication = BindingGenerationPublication.CreateTarget(Path.Combine(m_context.engineRoot, "artifacts", "bindings"), destination, cancellation);
                m_publications.Add(publication);
                TargetBindingGeneration.Generate(m_context, definition, loaded, publication, fingerprint, cancellation);
                results.Add(definition.component.nativeProject, TargetBindingGeneration.Describe(destination, fingerprint, loaded.bridge is not null));
            }
            else
            {
                var publication = BindingGenerationPublication.CreateHost(Path.Combine(m_context.engineRoot, "artifacts", "bindings"), output, loaded.bridge is null ? null
                    : NativeBindingGenerationIdentity.Resolve(loaded.bridge.outputPath, loaded.bridge.configDirectory!), cancellation);
                m_publications.Add(publication);
                HostBindingGeneration.Generate(m_context, definition, loaded, publication, fingerprint, cancellation);
                results.Add(definition.component.nativeProject, new NativeBindingGenerationDescriptor
                {
                    fingerprint = fingerprint, bindingsPath = Path.Combine(output, "Bindings.cs"),
                    artifactDirectory = string.Empty,
                    bridgeDirectory = publication.nativeDestination ?? string.Empty
                });
            }
        }
        Verify(definitions, fingerprints, m_context.BeginInputVerification("binding-generation"), cancellation);
        cancellation.ThrowIfCancellationRequested();
        foreach (BindingGenerationPublication publication in m_publications)
            if (!m_request.checkOnly)
            {
                if (m_request.outputMode == NativeBindingOutputMode.TargetArtifacts)
                {
                    using BuildArtifactWriteLease mutation = await BuildArtifactWriteLease.AcquireAsync(
                        publication.managedDestination, cancellation).ConfigureAwait(false);
                    publication.Commit();
                }
                else
                    publication.Commit();
            }
        if (m_request.outputMode == NativeBindingOutputMode.TargetArtifacts)
        {
            foreach (NativeBindingGenerationDescriptor descriptor in results.Values)
            {
                string directory = descriptor.artifactDirectory;
                if (!protectedTargets.Contains(directory))
                    m_reading.Add(await BuildArtifactReadLease.AcquireAsync(directory, descriptor.fingerprint,
                        ["Native", "Generated"], cancellation, m_context).ConfigureAwait(false));
            }
        }
        else
        {
            // Mutable host sources stay protected by their generation owner until the consumer finishes.
            m_reading.AddRange(m_leases);
            m_leases.Clear();
        }
        if (m_request.outputMode == NativeBindingOutputMode.TargetArtifacts && !m_request.checkOnly)
        {
            var cache = new BuildCachePublisher(Path.Combine(m_context.engineRoot, "artifacts", "bindings"), BuildCacheArtifactRole.ImmutableArtifact);
            foreach (BindingGenerationConfiguration definition in definitions)
            {
                NativeBindingGenerationDescriptor generation = results[definition.component.nativeProject];
                cache.Record(generation.artifactDirectory, generation.artifactDirectory + ".read.lock", [],
                    definition.component.id + "/" + m_request.targetId, m_request.targetId, cancellation);
            }
        }
        var result = new NativeBindingGenerationResult(results, m_reading);
        m_reading.Clear();
        return result;
    }

    /// <summary>
    /// Releases candidate outputs and generation leases after the complete batch has drained.
    /// </summary>
    public void Dispose()
    {
        List<Exception> failures = [];
        for (int index = m_reading.Count - 1; index >= 0; index--)
        {
            try
            {
                m_reading[index].Dispose();
            }
            catch (Exception failure)
            {
                failures.Add(failure);
            }
        }
        for (int index = m_publications.Count - 1; index >= 0; index--)
        {
            try
            {
                m_publications[index].Dispose();
            }
            catch (Exception failure)
            {
                failures.Add(failure);
            }
        }
        for (int index = m_leases.Count - 1; index >= 0; index--)
        {
            try
            {
                m_leases[index].Dispose();
            }
            catch (Exception failure)
            {
                failures.Add(failure);
            }
        }
        if (failures.Count > 0)
            throw new AggregateException("Binding batch cleanup failed after draining generation.", failures);
    }

    private void Verify(
        IReadOnlyList<BindingGenerationConfiguration> definitions,
        IReadOnlyList<string> fingerprints,
        NativeBuildContext verification,
        CancellationToken cancellation
    ) {
        for (int index = 0; index < definitions.Count; index++)
        {
            BindingGenerationConfiguration current = BindingDefinitionReader.Read(verification, definitions[index].component, m_request);
            current.extensionAssembly = definitions[index].extensionAssembly;
            if (Compute(current, verification, cancellation) != fingerprints[index])
                throw new InvalidOperationException("Binding inputs changed during an ownership wait or generation; the candidate was not published.");
        }
    }

    private string Compute(
        BindingGenerationConfiguration definition,
        NativeBuildContext context,
        CancellationToken cancellation
    ) {
        var loaded = definition.Load(context);
        return NativeBindingGenerationIdentity.Compute(loaded.managed, loaded.bridge, definition,
            definition.GetOutputRoot(context), context,
            loaded.hostBridgeRoot, cancellation, m_publications.SelectMany(static publication => publication.stagingPaths).ToArray());
    }
}
