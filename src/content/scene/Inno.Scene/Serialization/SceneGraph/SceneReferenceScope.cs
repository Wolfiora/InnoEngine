using System;
using Inno.Core.Execution;

namespace Inno.Scene;

/// <summary>
/// Owns the synchronous scene-reference protocol while a specific serialization boundary is active.
/// </summary>
internal abstract class SceneReferenceScope
{
    private static readonly ExecutionSlot<SceneReferenceScope> C_CURRENT = new("scene graph references");
    private readonly GameObject? m_captureRoot;

    /// <summary>
    /// Associates a synchronous reference boundary with its owning Scene.
    /// </summary>
    /// <param name="scene">
    /// The Scene whose object references this boundary may describe.
    /// </param>
    /// <param name="captureRoot">
    /// The optional frozen subtree root used in diagnostics, or null for a Scene-wide scope.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The owning Scene is null.
    /// </exception>
    protected SceneReferenceScope(
        GameScene scene,
        GameObject? captureRoot = null
    ) {
        this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
        m_captureRoot = captureRoot;
    }

    internal static SceneReferenceScope current => C_CURRENT.current;
    /// <summary>
    /// Gets the Scene that owns the references admitted by this boundary.
    /// </summary>
    protected GameScene scene { get; }

    internal static bool TryGetCurrent(out SceneReferenceScope? references)
        => C_CURRENT.TryGet(out references);

    internal static IDisposable Suspend() => C_CURRENT.Suspend();

    internal IDisposable Enter() => C_CURRENT.Enter(this, threadAffine: true);

    internal EngineReferenceToken Capture(
        EngineObject engineObject,
        string path
    ) {
        ArgumentNullException.ThrowIfNull(engineObject);
        if (engineObject.isDestroyed || !TryGetSourceId(engineObject, out Guid sourceId))
        {
            throw new InvalidOperationException(
                $"Reference to '{engineObject.GetType().FullName}' with identity " +
                $"'{engineObject.identity.persistentId}' at '{path}' is outside the serialized graph boundary" +
                (m_captureRoot is null
                    ? "."
                    : $" rooted at GameObject '{m_captureRoot.name}' ({m_captureRoot.identity.persistentId})."));
        }
        GameScene ownerScene = engineObject switch
        {
            GameObject gameObject => gameObject.scene,
            GameComponent component => component.gameObject.scene,
            GameSystem system => system.ownerScene
                ?? throw new InvalidOperationException($"Detached GameSystem reference at '{path}' cannot be serialized."),
            _ => throw new InvalidOperationException(
                $"Engine object type '{engineObject.GetType().FullName}' cannot be serialized as a scene reference at '{path}'.")
        };
        if (!ReferenceEquals(ownerScene, scene))
            throw new InvalidOperationException($"Cross-scene object reference at '{path}' cannot be serialized.");
        EngineReferenceKind kind = engineObject switch
        {
            GameObject => EngineReferenceKind.GameObject,
            GameComponent => EngineReferenceKind.GameComponent,
            GameSystem => EngineReferenceKind.GameSystem,
            _ => throw new InvalidOperationException($"Unsupported scene reference at '{path}'.")
        };
        return new EngineReferenceToken(kind, sourceId);
    }

    internal EngineObject Resolve(
        EngineReferenceToken token,
        Type expectedType,
        string path
    ) {
        if (!TryResolve(token.sourceId, out EngineObject? engineObject) || engineObject is null)
            throw new InvalidOperationException($"Scene graph reference '{token.sourceId}' at '{path}' could not be resolved.");
        if (!expectedType.IsInstanceOfType(engineObject))
        {
            throw new InvalidOperationException(
                $"Scene graph reference '{token.sourceId}' at '{path}' resolves to " +
                $"'{engineObject.GetType().FullName}', expected '{expectedType.FullName}'.");
        }
        return engineObject;
    }

    /// <summary>
    /// Looks up the identifier assigned to an object by this reference boundary.
    /// </summary>
    /// <param name="engineObject">
    /// The non-null object being captured within the synchronous owner operation.
    /// </param>
    /// <param name="sourceId">
    /// The admitted identifier, or the empty GUID when the boundary rejects the object.
    /// </param>
    /// <returns>
    /// True when the boundary maps the object; false when it lies outside the admitted scope.
    /// </returns>
    protected abstract bool TryGetSourceId(
        EngineObject engineObject,
        out Guid sourceId
    );

    /// <summary>
    /// Resolves one identifier within the frozen or current owner boundary.
    /// </summary>
    /// <param name="sourceId">
    /// The reference identifier from the current serialized payload.
    /// </param>
    /// <param name="engineObject">
    /// The mapped object, or null when the boundary cannot resolve the identifier.
    /// </param>
    /// <returns>
    /// True when the boundary contains the identifier; false without publishing a replacement object.
    /// </returns>
    protected abstract bool TryResolve(
        Guid sourceId,
        out EngineObject? engineObject
    );
}
