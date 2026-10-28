using System;
using System.Collections.Generic;

namespace Inno.Core.Serialization;

/// <summary>
/// Provides immutable, operation-independent context to serialization converters.
/// </summary>
public sealed class SerializationContext
{
    private readonly KeyValuePair<Type, object>[] m_values;

    private SerializationContext(KeyValuePair<Type, object>[] values)
    {
        m_values = values;
    }

    /// <summary>
    /// Gets an empty serialization context.
    /// </summary>
    public static SerializationContext empty { get; } = new([]);

    /// <summary>
    /// Returns a new context containing the supplied value under its declared context type.
    /// </summary>
    /// <typeparam name="TContext">
    /// The exact context contract type.
    /// </typeparam>
    /// <param name="value">
    /// The context value to register.
    /// </param>
    /// <returns>
    /// A new context containing the registered value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is null.
    /// </exception>
    public SerializationContext With<TContext>(TContext value) where TContext : class
    {
        ArgumentNullException.ThrowIfNull(value);
        Type key = typeof(TContext);
        for (int index = 0; index < m_values.Length; index++)
        {
            if (m_values[index].Key != key)
                continue;
            if (ReferenceEquals(m_values[index].Value, value))
                return new SerializationContext(m_values);
            var updated = (KeyValuePair<Type, object>[])m_values.Clone();
            updated[index] = new(key, value);
            return new SerializationContext(updated);
        }
        var expanded = new KeyValuePair<Type, object>[m_values.Length + 1];
        m_values.CopyTo(expanded, 0);
        expanded[^1] = new(key, value);
        return new SerializationContext(expanded);
    }

    /// <summary>
    /// Attempts to resolve a value registered under the exact context contract type.
    /// </summary>
    /// <typeparam name="TContext">
    /// The exact context contract type.
    /// </typeparam>
    /// <param name="value">
    /// The registered value when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a value is registered; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGet<TContext>(out TContext? value) where TContext : class
    {
        foreach (KeyValuePair<Type, object> pair in m_values)
        {
            if (pair.Key == typeof(TContext) && pair.Value is TContext typed)
            {
                value = typed;
                return true;
            }
        }

        value = null;
        return false;
    }

    /// <summary>
    /// Resolves a value registered under the exact context contract type.
    /// </summary>
    /// <typeparam name="TContext">
    /// The exact context contract type.
    /// </typeparam>
    /// <returns>
    /// The registered context value.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the requested context was not registered.
    /// </exception>
    public TContext GetRequired<TContext>() where TContext : class
        => TryGet<TContext>(out TContext? value) && value is not null
            ? value
            : throw new InvalidOperationException(
                $"Serialization context '{typeof(TContext).FullName}' is not registered.");
}
