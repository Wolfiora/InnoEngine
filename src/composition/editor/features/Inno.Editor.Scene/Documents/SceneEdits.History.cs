using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Extensibility.Types;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scene.Layers;

namespace Inno.Editor.Scene;

public sealed partial class SceneEdits
{
    private void RecordDocument(
        string name,
        bool existsBefore,
        bool existsAfter,
        EditorSceneWorkspace.SceneDocumentSnapshot snapshot,
        Guid? activeBefore,
        Guid? activeAfter,
        Guid? selectedBefore,
        Guid? selectedAfter
    ) {
        var data = new SceneDocumentHistoryData(
            existsBefore,
            existsAfter,
            snapshot,
            activeBefore,
            activeAfter,
            selectedBefore,
            selectedAfter
        );
        m_interactions.history.RecordApplied(
            name,
            new EditorHistoryChange(SceneHistoryKinds.C_DOCUMENT, EditorHistoryPayload.FromBytes(data.Encode()))
        );
    }

    private void RecordSubtree(
        string name,
        GameObject root,
        bool existsBefore,
        bool existsAfter,
        byte[] subtree,
        SceneIncomingReferenceState[] incoming,
        Guid? selectedBefore,
        Guid? selectedAfter
    ) {
        var data = new SceneSubtreeHistoryData(
            root.scene.identity.persistentID,
            root.identity.persistentID,
            root.transform.parent?.gameObject.identity.persistentID,
            root.transform.siblingIndex,
            existsBefore,
            existsAfter,
            subtree,
            incoming,
            selectedBefore,
            selectedAfter
        );
        Record(name, SceneHistoryKinds.C_SUBTREE, data.Encode());
    }

    private void Record(
        string name,
        string kind,
        byte[] data,
        string? mergeKey = null
    ) => m_interactions.history.RecordApplied(
        name,
        new EditorHistoryChange(kind, EditorHistoryPayload.FromBytes(data), mergeKey)
    );

    private void RecordElement(
        string name,
        SceneElementHistoryData data
    ) => Record(name, SceneHistoryKinds.C_ELEMENT, data.Encode());

    private Guid? GetActiveSceneID()
        => m_workspace.activeScene is { isDestroyed: false } scene
           ? scene.identity.persistentID
           : null;

    private GameScene ResolveOwnerScene(EngineObject target)
        => target switch
    {
        GameScene scene => scene,
        GameObject gameObject => gameObject.scene,
        GameComponent component => component.gameObject.scene,
        GameSystem system => m_workspace.scenes.FirstOrDefault(scene => scene.GetSystems().Contains(system))
                             ?? throw new InvalidOperationException("The system has no loaded scene."),
        _ => throw new InvalidOperationException("The object has no loaded scene.")
    };

    private Guid? GetSelectionID()
        => m_interactions.selection.selectedTarget is EngineObject { isDestroyed: false } target
           ? target.identity.persistentID
           : null;

    private TypeRef GetTypeRef(Type type)
        => m_workspace.types.TryGetTypeRef(type, out TypeRef typeRef)
           ? typeRef
           : throw new InvalidOperationException(
               $"Scene element type '{type.FullName}' does not have an active StableTypeID."
           );

    private static void RecordWithRollback(
        Action record,
        Action rollback
    ) {
        try
        {
            record();
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, rollback);
        }
    }

    private static void RollbackAndRethrow(
        Exception failure,
        Action rollback
    ) {
        try
        {
            rollback();
        }
        catch (Exception rollbackException)
        {
            throw new AggregateException(
                "An editor mutation could not be recorded and its compensation also failed.",
                failure,
                rollbackException
            );
        }
        ExceptionDispatchInfo.Capture(failure).Throw();
    }
}
