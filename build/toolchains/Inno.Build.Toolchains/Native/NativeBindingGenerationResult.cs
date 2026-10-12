using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Inno.Build.Toolchains;

/// <summary>
/// Owns the reading protection of a complete binding closure until its final compiler consumer finishes.
/// </summary>
public sealed class NativeBindingGenerationResult : IReadOnlyDictionary<string, NativeBindingGenerationDescriptor>, IDisposable
{
    private readonly IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> m_generations;
    private IDisposable[]? m_ownership;

    /// <summary>
    /// Freezes a complete generation map and accepts its already acquired reading ownership.
    /// </summary>
    /// <param name="generations">
    /// The complete Native project to generation mapping.
    /// </param>
    /// <param name="ownership">
    /// Protection transferred on successful construction; retained by the caller on failure.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Either collection is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A generation or protection entry is null.
    /// </exception>
    public NativeBindingGenerationResult(
        IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> generations,
        IEnumerable<IDisposable> ownership
    ) {
        ArgumentNullException.ThrowIfNull(generations);
        ArgumentNullException.ThrowIfNull(ownership);
        var copy = new Dictionary<string, NativeBindingGenerationDescriptor>(generations, StringComparer.Ordinal);
        IDisposable[] owners = ownership.ToArray();
        if (copy.Values.Any(static value => value is null) || owners.Any(static value => value is null))
            throw new ArgumentException("Generation and ownership entries must be non-null.");
        m_generations = new ReadOnlyDictionary<string, NativeBindingGenerationDescriptor>(copy);
        m_ownership = owners;
    }

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">
    /// Reading ownership has ended.
    /// </exception>
    public int Count => RequireGenerations().Count;

    /// <inheritdoc />
    /// <remarks>
    /// The returned sequence is borrowed and must be consumed before this result is disposed.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Reading ownership has ended.
    /// </exception>
    public IEnumerable<string> Keys => RequireGenerations().Keys;

    /// <inheritdoc />
    /// <remarks>
    /// Neither the returned sequence nor its descriptors extend the lifetime of reading protection.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Reading ownership has ended.
    /// </exception>
    public IEnumerable<NativeBindingGenerationDescriptor> Values => RequireGenerations().Values;

    /// <summary>
    /// Gets a borrowed descriptor whose outputs remain protected until this result is disposed.
    /// </summary>
    /// <param name="key">
    /// The checkout-relative Native owner project.
    /// </param>
    /// <returns>
    /// The selected generation; neither its paths nor retained descriptors extend reading ownership.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// Reading ownership has ended.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The project is outside this closure.
    /// </exception>
    public NativeBindingGenerationDescriptor this[string key] => RequireGenerations()[key];

    /// <inheritdoc />
    /// <exception cref="ObjectDisposedException">
    /// Reading ownership has ended.
    /// </exception>
    public bool ContainsKey(string key) => RequireGenerations().ContainsKey(key);

    /// <inheritdoc />
    /// <remarks>
    /// A returned descriptor is borrowed and must not be used to access files after this result is disposed.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Reading ownership has ended.
    /// </exception>
    public bool TryGetValue(
        string key,
        [MaybeNullWhen(false)] out NativeBindingGenerationDescriptor value
    ) => RequireGenerations().TryGetValue(key, out value);

    /// <inheritdoc />
    /// <remarks>
    /// Enumeration and all descriptor consumers must finish before the result's reading protection ends.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Reading ownership has ended.
    /// </exception>
    public IEnumerator<KeyValuePair<string, NativeBindingGenerationDescriptor>> GetEnumerator() => RequireGenerations().GetEnumerator();

    /// <summary>
    /// Releases all protection in reverse acquisition order; repeated disposal has no effect.
    /// </summary>
    /// <exception cref="AggregateException">
    /// One or more owners failed to release after every owner was attempted.
    /// </exception>
    public void Dispose()
    {
        IDisposable[]? owners = Interlocked.Exchange(ref m_ownership, null);
        if (owners is null)
            return;
        List<Exception> failures = [];
        for (int index = owners.Length - 1; index >= 0; index--)
        {
            try
            {
                owners[index].Dispose();
            }
            catch (Exception failure)
            {
                failures.Add(failure);
            }
        }
        if (failures.Count != 0)
            throw new AggregateException("Binding reading ownership could not be completely released.", failures);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> RequireGenerations()
    {
        ObjectDisposedException.ThrowIf(m_ownership is null, this);
        return m_generations;
    }
}
