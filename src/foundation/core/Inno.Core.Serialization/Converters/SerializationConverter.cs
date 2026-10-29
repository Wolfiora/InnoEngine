using System;

namespace Inno.Core.Serialization.Converters;

/// <summary>
/// Identifies a concrete serialization converter for generation-scoped type discovery.
/// </summary>
public abstract class SerializationConverter
{
    internal abstract ConverterInvoker CreateInvoker();
}

/// <summary>
/// Defines the single advanced extension contract for serializing a specific value type.
/// </summary>
/// <typeparam name="T">
/// The value contract handled by the converter.
/// </typeparam>
public abstract class SerializationConverter<T> : SerializationConverter
{
    /// <summary>
    /// Writes a value into the current structured object.
    /// </summary>
    /// <param name="writer">
    /// The operation-scoped writer.
    /// </param>
    /// <param name="value">
    /// The value to write.
    /// </param>
    public abstract void Write(
        SerializationWriter writer,
        T value
    );

    /// <summary>
    /// Reads and creates a value from the current structured object.
    /// </summary>
    /// <param name="reader">
    /// The operation-scoped reader.
    /// </param>
    /// <returns>
    /// The restored value.
    /// </returns>
    public abstract T Read(SerializationReader reader);

    /// <summary>
    /// Restores data into an existing value.
    /// </summary>
    /// <param name="reader">
    /// The operation-scoped reader.
    /// </param>
    /// <param name="target">
    /// The existing target value.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// Thrown when the converter does not support restoring existing values.
    /// </exception>
    public virtual void Restore(
        SerializationReader reader,
        T target
    )
        => throw new NotSupportedException(
            $"Serialization converter '{GetType().FullName}' does not support restoring existing '{typeof(T).FullName}' values.");

    /// <summary>
    /// Optionally declares every input to a snapshot capture without writing data.
    /// </summary>
    /// <param name="value">
    /// The current registered identity source.
    /// </param>
    /// <param name="key">
    /// The borrowed Core key builder, valid only during this synchronous callback.
    /// </param>
    /// <returns>
    /// True only when the complete output is covered; false retains ordinary authoritative capture.
    /// </returns>
    /// <remarks>
    /// This callback must be pure and complete. It runs only for CaptureSnapshot; ordinary Serialize never uses it.
    /// After a changed key, Core verifies the key again before publishing. Default converters do not opt in.
    /// </remarks>
    protected virtual bool TryCaptureKey(
        T value,
        SerializationCaptureKeyBuilder key
    ) => false;

    internal bool CaptureKey(
        T value,
        SerializationCaptureKeyBuilder key
    ) => TryCaptureKey(value, key);

    internal sealed override ConverterInvoker CreateInvoker() => new ConverterInvoker<T>(this, GetType());
}
