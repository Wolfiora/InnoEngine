using System;
using System.Collections.Generic;
using Inno.Core.Serialization;

namespace Inno.Core.Settings;

/// <summary>
/// Defines the non-generic base for a protocol-owned project setting composer.
/// </summary>
public abstract class ProjectSettingComposer
{
    internal abstract Type settingType { get; }

    internal abstract bool TryCapture(
        SerializationRegistry serialization,
        SerializationContext serializationContext,
        ISerializable baseline,
        ISerializable value,
        out byte[] contributionData
    );

    internal abstract ISerializable Compose(
        SerializationRegistry serialization,
        SerializationContext serializationContext,
        ISerializable hostDefault,
        IReadOnlyList<ProjectSettingCompositionEntry> contributions
    );
}

/// <summary>
/// Lets one setting protocol define deterministic delta capture and multi-contributor composition.
/// </summary>
/// <typeparam name="TSetting">
/// The exact effective setting type.
/// </typeparam>
/// <typeparam name="TContribution">
/// The exact serializable delta type stored in Plugin and project records.
/// </typeparam>
public abstract class ProjectSettingComposer<TSetting, TContribution> : ProjectSettingComposer
    where TSetting : class, ISerializable
    where TContribution : class, ISerializable
{
    internal sealed override Type settingType => typeof(TSetting);

    /// <summary>
    /// Captures the contribution introduced by the supplied project setting value.
    /// </summary>
    /// <param name="baseline">
    /// The value composed from lower-precedence contributors.
    /// </param>
    /// <param name="value">
    /// The complete authored value.
    /// </param>
    /// <returns>
    /// A detached neutral contribution.
    /// </returns>
    protected abstract TContribution CaptureContribution(
        TSetting baseline,
        TSetting value
    );

    /// <summary>
    /// Gets whether a captured contribution contains no semantic operation.
    /// </summary>
    /// <param name="contribution">
    /// The contribution to inspect.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the record should be omitted.
    /// </returns>
    protected abstract bool IsEmpty(TContribution contribution);

    /// <summary>
    /// Composes dependency-ordered Plugin deltas and the optional final project delta into a host default.
    /// </summary>
    /// <param name="target">
    /// The newly created host default to mutate into the effective value.
    /// </param>
    /// <param name="contributions">
    /// Decoded contributions in deterministic precedence order.
    /// </param>
    protected abstract void Compose(
        TSetting target,
        IReadOnlyList<ProjectSettingContribution<TContribution>> contributions
    );

    internal sealed override bool TryCapture(
        SerializationRegistry serialization,
        SerializationContext serializationContext,
        ISerializable baseline,
        ISerializable value,
        out byte[] contributionData
    ) {
        if (baseline is not TSetting typedBaseline || value is not TSetting typedValue)
        {
            throw new ArgumentException(
                $"Composer '{GetType().FullName}' requires setting type '{typeof(TSetting).FullName}'."
            );
        }
        TContribution contribution = CaptureContribution(typedBaseline, typedValue)
            ?? throw new InvalidOperationException($"Composer '{GetType().FullName}' returned a null contribution.");
        if (IsEmpty(contribution))
        {
            contributionData = [];
            return false;
        }
        contributionData = serialization.Serialize(contribution, serializationContext);
        return true;
    }

    internal sealed override ISerializable Compose(
        SerializationRegistry serialization,
        SerializationContext serializationContext,
        ISerializable hostDefault,
        IReadOnlyList<ProjectSettingCompositionEntry> contributions
    ) {
        if (hostDefault is not TSetting target)
        {
            throw new ArgumentException(
                $"Composer '{GetType().FullName}' requires setting type '{typeof(TSetting).FullName}'.",
                nameof(hostDefault)
            );
        }
        var decoded = new List<ProjectSettingContribution<TContribution>>(contributions.Count);
        foreach (ProjectSettingCompositionEntry contribution in contributions)
        {
            TContribution value = serialization.Deserialize<TContribution>(contribution.data, serializationContext);
            if (IsEmpty(value))
            {
                throw new InvalidOperationException(
                    $"Composer '{GetType().FullName}' received an empty persisted contribution from "
                    + $"'{contribution.context.contributorID}'."
                );
            }
            decoded.Add(new ProjectSettingContribution<TContribution>(contribution.context, value));
        }
        Compose(target, decoded);
        return target;
    }
}
