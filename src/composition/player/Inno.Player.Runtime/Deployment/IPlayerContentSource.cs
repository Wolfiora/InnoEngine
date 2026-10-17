using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Content;
using Inno.Core.Serialization;
using Inno.Storage;

namespace Inno.Player.Runtime;

/// <summary>
/// Supplies deployment metadata and verified content without prescribing a physical layout.
/// </summary>
public interface IPlayerContentSource
{
    /// <summary>
    /// Reads the bounded deployment manifest and content catalog before serialization starts.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels metadata acquisition before content preparation starts.
    /// </param>
    /// <returns>
    /// Independently owned metadata bytes; missing or unreadable metadata fails the operation.
    /// </returns>
    ValueTask<PlayerContentMetadata> ReadMetadataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Prepares the exact verified pack selected by the decoded catalog.
    /// </summary>
    /// <param name="pack">
    /// The validated pack identity; other packs must not be substituted.
    /// </param>
    /// <param name="scope">
    /// The application namespace used to isolate any preparation cache.
    /// </param>
    /// <param name="serialization">
    /// The active serialization generation borrowed until this operation completes.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels acquisition and preparation before ownership transfers to the caller.
    /// </param>
    /// <returns>
    /// A caller-owned immutable store whose leases pin independent readers.
    /// </returns>
    /// <exception cref="System.IO.InvalidDataException">
    /// Pack bytes, inventory, lengths, or payload hashes are invalid.
    /// </exception>
    ValueTask<IRuntimeContentStore> PrepareAsync(
        ContentPackDescriptor pack,
        StorageScope scope,
        SerializationGeneration serialization,
        CancellationToken cancellationToken = default
    );
}
