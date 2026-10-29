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

    protected SceneReferenceScope(
        GameScene scene,
        GameObject? captureRoot = null
    ) {
        this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
        m_captureRoot = captureRoot;
    }

    internal static SceneReferenceScope current => C_CURRENT.current;
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

    protected abstract bool TryGetSourceId(
        EngineObject engineObject,
        out Guid sourceId
    );

    protected abstract bool TryResolve(
        Guid sourceId,
        out EngineObject? engineObject
    );
}
