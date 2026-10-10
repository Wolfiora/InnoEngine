using System;
using System.IO;

using Inno.Core.IO;
using Inno.Core.Serialization;

namespace Inno.Core.Settings;

/// <summary>
/// Provides validated current-format serialization and atomic persistence for one settings document type.
/// </summary>
/// <typeparam name="TDocument">
/// The complete serializable document type.
/// </typeparam>
public sealed class SettingsDocumentStore<TDocument>
    where TDocument : class, ISerializable
{
    private readonly Func<TDocument> m_createDefault;
    private readonly SerializationRegistry m_serialization;
    private readonly Action<TDocument> m_validate;
    private readonly IByteDocumentStore m_document;

    /// <summary>
    /// Creates a type-safe settings document store.
    /// </summary>
    /// <param name="document">
    /// The borrowed document boundary; read-only sources explicitly reject writes.
    /// </param>
    /// <param name="serialization">
    /// The active serialization registry.
    /// </param>
    /// <param name="createDefault">
    /// Creates a newly owned value when the document does not exist.
    /// </param>
    /// <param name="validate">
    /// Validates one deserialized or candidate document.
    /// </param>
    public SettingsDocumentStore(
        IByteDocumentStore document,
        SerializationRegistry serialization,
        Func<TDocument> createDefault,
        Action<TDocument>? validate = null
    ) {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(createDefault);
        m_document = document;
        m_serialization = serialization;
        m_createDefault = createDefault;
        m_validate = validate ?? (static _ => { });
    }

    /// <summary>
    /// Gets the source's logical diagnostic name without requiring a filesystem location.
    /// </summary>
    public string documentName => m_document.documentName;

    /// <summary>
    /// Gets whether the document currently exists.
    /// </summary>
    public bool exists => m_document.exists;

    /// <summary>
    /// Loads the saved value, or creates a validated default when absent.
    /// </summary>
    /// <returns>
    /// A newly owned current-format document.
    /// </returns>
    public TDocument Load()
    {
        byte[]? data = m_document.Read();
        if (data is null)
        {
            TDocument value = m_createDefault();
            m_validate(value);
            return Clone(value);
        }
        return Deserialize(data);
    }

    /// <summary>
    /// Loads a required saved value.
    /// </summary>
    /// <returns>
    /// A newly owned current-format document.
    /// </returns>
    public TDocument LoadRequired()
    {
        byte[] data = m_document.Read() ?? throw new InvalidDataException(
            $"Required settings document '{documentName}' does not exist.");
        return Deserialize(data);
    }

    /// <summary>
    /// Validates and atomically replaces the complete document.
    /// </summary>
    /// <param name="document">
    /// The complete candidate document.
    /// </param>
    public void Save(TDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        m_validate(document);
        m_document.Write(m_serialization.Serialize(document));
    }

    /// <summary>
    /// Serializes a validated document without changing the file.
    /// </summary>
    /// <param name="document">
    /// The document to capture.
    /// </param>
    /// <returns>
    /// A newly owned native payload.
    /// </returns>
    public byte[] Capture(TDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);
        m_validate(document);
        return m_serialization.Serialize(document);
    }

    /// <summary>
    /// Deserializes and validates a native payload without changing the file.
    /// </summary>
    /// <param name="data">
    /// The native payload.
    /// </param>
    /// <returns>
    /// A newly owned current-format document.
    /// </returns>
    public TDocument Deserialize(ReadOnlySpan<byte> data)
    {
        try
        {
            TDocument document = m_serialization.Deserialize<TDocument>(data);
            m_validate(document);
            return document;
        }
        catch (Exception exception) when (exception is ArgumentException
            or InvalidDataException
            or InvalidOperationException
            or NotSupportedException)
        {
            throw new InvalidDataException(
                $"Settings document '{documentName}' is not a valid current-format {typeof(TDocument).Name}.",
                exception);
        }
    }

    /// <summary>
    /// Validates and atomically restores a native document payload.
    /// </summary>
    /// <param name="data">
    /// The native payload.
    /// </param>
    public void Restore(ReadOnlySpan<byte> data) => Save(Deserialize(data));

    private TDocument Clone(TDocument document) => Deserialize(m_serialization.Serialize(document));
}
