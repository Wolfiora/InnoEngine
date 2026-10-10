using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Selects the publication owner of generated sources.
/// </summary>
public enum NativeBindingOutputMode
{
    /// <summary>
    /// Replaces the component's authored host source and bridge outputs together.
    /// </summary>
    HostSource,
    /// <summary>
    /// Publishes immutable target outputs under the component's generation fingerprint.
    /// </summary>
    TargetArtifacts
}

/// <summary>
/// Freezes the complete binding request before ownership waits or candidate creation.
/// </summary>
public sealed class NativeBindingGenerationRequest
{
    /// <summary>
    /// Captures explicit owners and a target without discovering components or parsing a build project.
    /// </summary>
    /// <param name="components">
    /// The component owners, each declaring one binding definition.
    /// </param>
    /// <param name="targetId">
    /// The target identity selecting a declared configuration; host output uses the host definition.
    /// </param>
    /// <param name="outputMode">
    /// Whether to write authored host sources or isolated target artifacts.
    /// </param>
    /// <param name="checkOnly">
    /// Whether to compare complete output without publishing a candidate.
    /// </param>
    /// <param name="expectedFingerprints">
    /// Optional expected identities indexed by Native project; mismatches reject the request.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A target, owner, output mode or expected identity is invalid or duplicated.
    /// </exception>
    public NativeBindingGenerationRequest(
        IEnumerable<NativeComponentDescriptor> components,
        string targetId,
        NativeBindingOutputMode outputMode,
        bool checkOnly = false,
        IReadOnlyDictionary<string, string>? expectedFingerprints = null
    ) {
        ArgumentNullException.ThrowIfNull(components);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        if (!Enum.IsDefined(outputMode) || targetId is "." or ".."
            || targetId.Any(static character => !char.IsAsciiLetterOrDigit(character)
                && character is not ('-' or '_' or '.')))
            throw new ArgumentException("Binding requests require a portable target and a supported output mode.");
        NativeComponentDescriptor[] owners = components.ToArray();
        if (owners.Any(static owner => owner is null || owner.bindingDefinition is null)
            || owners.Select(static owner => owner.nativeProject).Distinct(StringComparer.Ordinal).Count() != owners.Length)
            throw new ArgumentException("Binding owners must be distinct and declare their sole definition.", nameof(components));
        this.components = Array.AsReadOnly(owners);
        this.targetId = targetId;
        this.outputMode = outputMode;
        this.checkOnly = checkOnly;
        Dictionary<string, string> expected = new(expectedFingerprints ?? new Dictionary<string, string>(), StringComparer.Ordinal);
        if (expected.Any(pair => !owners.Any(owner => owner.nativeProject == pair.Key)
            || string.IsNullOrWhiteSpace(pair.Value)))
            throw new ArgumentException("An expected identity must belong to the requested binding closure.", nameof(expectedFingerprints));
        this.expectedFingerprints = new ReadOnlyDictionary<string, string>(expected);
    }

    /// <summary>
    /// Gets the immutable component closure owned by this request.
    /// </summary>
    public IReadOnlyList<NativeComponentDescriptor> components { get; }
    /// <summary>
    /// Gets the explicit configuration target, independent of the build execution host.
    /// </summary>
    public string targetId { get; }
    /// <summary>
    /// Gets the selected output ownership policy.
    /// </summary>
    public NativeBindingOutputMode outputMode { get; }
    /// <summary>
    /// Gets whether publication is forbidden and current output must match fresh generation.
    /// </summary>
    public bool checkOnly { get; }
    /// <summary>
    /// Gets the expected identities which must match before any candidate is generated.
    /// </summary>
    public IReadOnlyDictionary<string, string> expectedFingerprints { get; }
}
