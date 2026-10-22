using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Bindings;

internal sealed class BindingGenerationOperation : IDisposable
{
    private readonly NativeBindingProfile m_profile;
    private readonly NativeBuildContext m_context;
    private readonly NativeBindingGenerationRequest m_request;
    private readonly List<FileLease> m_leases = [];
    private readonly List<IDisposable> m_reading = [];
    private readonly List<BindingGenerationPublication> m_publications = [];

    internal BindingGenerationOperation(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        NativeBindingProfile profile
    ) {
        m_profile = profile;
        m_context = context;
        m_request = request;
    }

    internal async ValueTask<NativeBindingGenerationResult> ExecuteAsync(CancellationToken cancellation)
    {
        BindingGenerationConfiguration[] definitions = m_request.components.Select(component =>
            BindingDefinitionReader.Read(m_context, component, m_request, m_profile)).ToArray();
        string[] fingerprints = definitions.Select(definition => Compute(definition, m_context, cancellation)).ToArray();
        for (int index = 0; index < definitions.Length; index++)
            if (m_request.expectedFingerprints.TryGetValue(definitions[index].component.nativeProject, out string? expected)
                && expected != fingerprints[index])
                throw new InvalidOperationException("Binding inputs changed after their generation was selected.");
        BuildCacheOwnerCatalog.Register(m_context.engineRoot, Path.Combine(m_context.engineRoot, "artifacts", "bindings"),
            "bindings", BuildCacheArtifactRole.ImmutableArtifact, cancellation);
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
                    m_reading.Add(await BuildArtifactReadLease.AcquireAsync(destination, Path.Combine(m_context.engineRoot, "artifacts", "bindings"), fingerprint,
                        ["Native", "Generated"], cancellation, m_context).ConfigureAwait(false));
                    protectedTargets.Add(destination);
                    if (!m_request.checkOnly)
                    {
                        results.Add(definition.component.nativeProject, TargetBindingGeneration.Describe(
                            Path.Combine(m_context.engineRoot, "artifacts", "bindings"), destination, fingerprint));
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
                results.Add(definition.component.nativeProject, TargetBindingGeneration.Describe(
                    Path.Combine(m_context.engineRoot, "artifacts", "bindings"), destination, fingerprint));
            }
            else
            {
                var publication = BindingGenerationPublication.CreateHost(Path.Combine(m_context.engineRoot, "artifacts", "bindings"), output, definition.GetHostNativeRoot(m_context), cancellation);
                m_publications.Add(publication);
                HostBindingGeneration.Generate(m_context, definition, loaded, publication, fingerprint, cancellation);
                results.Add(definition.component.nativeProject, new NativeBindingGenerationDescriptor
                {
                    fingerprint = fingerprint, bindingsPath = Path.Combine(output, "Bindings.cs"),
                    artifactDirectory = string.Empty,
                    artifactOwnerDirectory = string.Empty,
                    nativeDirectory = publication.nativeDestination ?? string.Empty
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
                        publication.managedDestination, Path.Combine(m_context.engineRoot, "artifacts", "bindings"), cancellation).ConfigureAwait(false);
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
                    m_reading.Add(await BuildArtifactReadLease.AcquireAsync(directory, Path.Combine(m_context.engineRoot, "artifacts", "bindings"), descriptor.fingerprint,
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
            BindingGenerationConfiguration current = BindingDefinitionReader.Read(verification, definitions[index].component, m_request, m_profile);
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
        return NativeBindingGenerationIdentity.Compute(loaded, definition, context, cancellation);
    }
}
