using System;
using System.Collections.Generic;

using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Scene;

namespace Inno.Scene;

/// <summary>
/// Captures and restores individual scene-object properties while preserving scene reference identities.
/// </summary>
/// <remarks>
/// Each operation borrows its owning Core identity domain and resolves current references within that scene.
/// Property access does not enumerate unrelated scene objects. Complete graph and prefab operations keep
/// their explicit frozen local-identity boundaries. Asset references retain the supplied resolver context.
/// </remarks>
public static class ScenePropertySerialization
{
    /// <summary>
    /// Captures one persistent property without serializing the complete scene.
    /// </summary>
    /// <param name="target">
    /// The live scene object containing the property.
    /// </param>
    /// <param name="propertyName">
    /// The exact serialized member key.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry that owns the active converter generation.
    /// </param>
    /// <param name="assets">
    /// The resolver used to preserve canonical asset references in captured values.
    /// </param>
    /// <returns>
    /// Neutral bytes that can be restored into the same logical object identity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="target"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="propertyName"/> is empty or unknown.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the target is destroyed or not owned by a loaded scene.
    /// </exception>
    public static byte[] CaptureProperty(
        EngineObject target,
        string propertyName,
        SerializationRegistry serialization,
        IAssetReferenceResolver assets
    ) {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(assets);
        if (target is not ISerializable serializable)
            throw new ArgumentException($"Scene object '{target.GetType().FullName}' is not serializable.", nameof(target));
        ScenePropertyReferenceScope references = CreateReferences(ResolveScene(target));
        SerializationContext context = AssetSerializationContext.Create(assets);
        using (references.Enter())
            return serialization.CapturePropertyData(serializable, propertyName, context);
    }

    /// <summary>
    /// Captures all persistent properties without serializing the complete scene.
    /// </summary>
    /// <param name="target">
    /// The live scene object whose state should be captured.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry that owns the active converter generation.
    /// </param>
    /// <param name="assets">
    /// The resolver used to preserve canonical asset references in captured values.
    /// </param>
    /// <returns>
    /// Neutral bytes containing independently encoded properties.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="target"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the target is destroyed or not owned by a loaded scene.
    /// </exception>
    public static byte[] CaptureProperties(
        EngineObject target,
        SerializationRegistry serialization,
        IAssetReferenceResolver assets
    ) {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(assets);
        if (target is not ISerializable serializable)
            throw new ArgumentException($"Scene object '{target.GetType().FullName}' is not serializable.", nameof(target));
        ScenePropertyReferenceScope references = CreateReferences(ResolveScene(target));
        SerializationContext context = AssetSerializationContext.Create(assets);
        using (references.Enter())
            return serialization.CapturePropertiesData(serializable, context);
    }

    /// <summary>
    /// Captures each persistent property independently while preserving scene-reference identities.
    /// </summary>
    /// <param name="target">
    /// Live scene object whose properties should be captured.
    /// </param>
    /// <param name="serialization">
    /// Registry owning the active converter generation.
    /// </param>
    /// <param name="assets">
    /// Resolver used to preserve canonical asset references.
    /// </param>
    /// <returns>
    /// Stable ordered property snapshots suitable for compact delta comparison.
    /// </returns>
    public static IReadOnlyList<SerializationPropertySnapshot> CapturePropertySnapshots(
        EngineObject target,
        SerializationRegistry serialization,
        IAssetReferenceResolver assets
    ) {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(assets);
        if (target is not ISerializable serializable)
            throw new ArgumentException($"Scene object '{target.GetType().FullName}' is not serializable.", nameof(target));
        ScenePropertyReferenceScope references = CreateReferences(ResolveScene(target));
        SerializationContext context = AssetSerializationContext.Create(assets);
        using (references.Enter())
            return serialization.CaptureProperties(serializable, context);
    }

    /// <summary>
    /// Restores independently captured properties into a live scene object.
    /// </summary>
    /// <param name="target">
    /// The current object generation receiving the values.
    /// </param>
    /// <param name="data">
    /// Bytes produced by <see cref="CaptureProperty"/> or <see cref="CaptureProperties"/>.
    /// </param>
    /// <param name="mode">
    /// The policy applied to unavailable or mismatched current properties.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry that owns the active converter generation.
    /// </param>
    /// <param name="assets">
    /// The resolver used to materialize canonical asset references in restored values.
    /// </param>
    /// <returns>
    /// A summary of restored, ignored, and incompatible properties.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="target"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when strict restoration fails or the target is not in a loaded scene.
    /// </exception>
    public static SerializationPropertyRestoreResult RestoreProperties(
        EngineObject target,
        ReadOnlySpan<byte> data,
        SerializationRegistry serialization,
        IAssetReferenceResolver assets,
        SerializationPropertyRestoreMode mode = SerializationPropertyRestoreMode.Strict
    ) {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(serialization);
        ArgumentNullException.ThrowIfNull(assets);
        if (target is not ISerializable serializable)
            throw new ArgumentException($"Scene object '{target.GetType().FullName}' is not serializable.", nameof(target));
        ScenePropertyReferenceScope references = CreateReferences(ResolveScene(target));
        SerializationContext context = AssetSerializationContext.Create(assets);
        using (references.Enter())
            return serialization.RestorePropertiesData(serializable, data, mode, context);
    }

    internal static ScenePropertyReferenceScope CreateReferences(
        GameScene scene,
        IReadOnlyDictionary<Guid, Guid>? aliases = null
    ) => new(scene, aliases);

    internal static GameScene ResolveScene(EngineObject target)
    {
        if (target.isDestroyed)
            throw new InvalidOperationException($"Destroyed scene object '{target.identity.persistentId}' cannot be serialized.");
        return target switch
        {
            GameScene scene when scene.isLoaded => scene,
            GameObject gameObject when gameObject.isRuntimeValid => gameObject.scene,
            GameComponent component when !component.isDestroyed => component.gameObject.scene,
            GameSystem system when system.ownerScene is { isLoaded: true } owner => owner,
            _ => throw new InvalidOperationException(
                $"Scene object '{target.identity.persistentId}' is not owned by a loaded scene.")
        };
    }

}
