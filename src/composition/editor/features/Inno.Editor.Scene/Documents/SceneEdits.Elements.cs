using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;

using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scene.Layers;

namespace Inno.Editor.Scene;

sealed partial class SceneEdits
{
    /// <summary>
    /// Adds one component and records its identity, stable type, index, and persistent properties.
    /// </summary>
    /// <param name="owner">
    /// The live GameObject receiving the component.
    /// </param>
    /// <param name="componentType">
    /// The concrete component type to create.
    /// </param>
    /// <param name="historyName">
    /// An optional user-facing history entry name.
    /// </param>
    /// <returns>
    /// The newly attached component.
    /// </returns>
    public GameComponent AddComponent(
        GameObject owner,
        Type componentType,
        string? historyName = null
    ) {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(componentType);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(owner.scene);
        GameComponent component = owner.AddComponent(componentType);
        try
        {
            TypeRef typeRef = GetTypeRef(componentType);
            byte[] state = SceneElementSerialization.CaptureState(
                component,
                m_workspace.serialization,
                m_workspace.assets);
            RecordElement(
                historyName ?? $"Add {componentType.Name}",
                new SceneElementHistoryData(
                    SceneElementKind.Component,
                    owner.scene.identity.persistentId,
                    owner.identity.persistentId,
                    component.identity.persistentId,
                    typeRef,
                    beforeIndex: -1,
                    afterIndex: owner.GetComponentIndex(component),
                    existsBefore: false,
                    existsAfter: true,
                    beforeState: [],
                    afterState: state,
                    incomingReferences: []));
            return component;
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () =>
            {
                if (!component.isDestroyed && !owner.RemoveComponent(component))
                    throw new InvalidOperationException("The unrecorded component could not be removed.");
            });
            throw;
        }
    }

    /// <summary>
    /// Removes the component from its scene owner and records a reversible serialized history change.
    /// </summary>
    /// <param name="component">
    /// The attached non-Transform component to remove.
    /// </param>
    /// <param name="historyName">
    /// An optional user-facing history entry name.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the component was removed and recorded.
    /// </returns>
    public bool RemoveComponent(
        GameComponent component,
        string? historyName = null
    ) {
        ArgumentNullException.ThrowIfNull(component);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        if (component.isDestroyed)
            return false;
        m_workspace.EnsureEditable(component.gameObject.scene);
        GameObject owner = component.gameObject;
        GameScene scene = owner.scene;
        TypeRef typeRef = GetElementType(component);
        byte[] state = SceneElementSerialization.CaptureState(
            component,
            m_workspace.serialization,
            m_workspace.assets);
        SceneIncomingReferenceState[] incoming = SceneReferenceIndex.CaptureIncoming(
            component,
            scene,
            m_workspace);
        int index = owner.GetComponentIndex(component);
        Guid componentId = component.identity.persistentId;
        Type componentType = component.GetType();
        try
        {
            if (!owner.RemoveComponent(component))
                return false;
            RecordElement(
                historyName ?? $"Remove {componentType.Name}",
                new SceneElementHistoryData(
                    SceneElementKind.Component,
                    scene.identity.persistentId,
                    owner.identity.persistentId,
                    componentId,
                    typeRef,
                    beforeIndex: index,
                    afterIndex: -1,
                    existsBefore: true,
                    existsAfter: false,
                    beforeState: state,
                    afterState: [],
                    incoming));
        }
        catch (Exception exception)
        {
            if (m_workspace.Find<GameComponent>(componentId) is not null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            RollbackAndRethrow(exception, () =>
            {
                GameComponent restored = SceneElementSerialization.RestoreComponent(
                    owner,
                    typeRef,
                    componentId,
                    index,
                    state,
                    m_workspace.serialization,
                    m_workspace.assets);
                RequireElementRestore(restored, state);
                RequireReferenceRestore(SceneReferenceIndex.RestoreIncoming(incoming, m_workspace));
            });
        }
        return true;
    }

    /// <summary>
    /// Resets one component and records its compact property state before and after Reset.
    /// </summary>
    /// <param name="component">
    /// The attached component to reset.
    /// </param>
    /// <param name="historyName">
    /// An optional user-facing history entry name.
    /// </param>
    public void ResetComponent(
        GameComponent component,
        string? historyName = null
    ) {
        ArgumentNullException.ThrowIfNull(component);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(component.gameObject.scene);
        GameObject owner = component.gameObject;
        byte[] before = SceneElementSerialization.CaptureState(
            component,
            m_workspace.serialization,
            m_workspace.assets);
        byte[] after;
        try
        {
            owner.ResetComponent(component);
            after = SceneElementSerialization.CaptureState(
                component,
                m_workspace.serialization,
                m_workspace.assets);
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () => RequireElementRestore(component, before));
            throw;
        }
        if (before.AsSpan().SequenceEqual(after))
            return;
        int index = owner.GetComponentIndex(component);
        RecordWithRollback(
            () => RecordElement(
                historyName ?? $"Reset {component.GetType().Name}",
                new SceneElementHistoryData(
                SceneElementKind.Component,
                owner.scene.identity.persistentId,
                owner.identity.persistentId,
                component.identity.persistentId,
                GetElementType(component),
                index,
                index,
                existsBefore: true,
                existsAfter: true,
                before,
                after,
                incomingReferences: [])),
            () => RequireElementRestore(component, before));
    }

    /// <summary>
    /// Moves an attached component and records only its two attachment indices.
    /// </summary>
    /// <param name="component">
    /// The attached component to move, including the mandatory Transform.
    /// </param>
    /// <param name="componentIndex">
    /// The requested attachment index.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    public void SetComponentIndex(
        GameComponent component,
        int componentIndex,
        string historyName = "Move Component"
    ) {
        ArgumentNullException.ThrowIfNull(component);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(component.gameObject.scene);
        GameObject owner = component.gameObject;
        int beforeIndex = owner.GetComponentIndex(component);
        int afterIndex;
        try
        {
            owner.SetComponentIndex(component, componentIndex);
            afterIndex = owner.GetComponentIndex(component);
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () => owner.SetComponentIndex(component, beforeIndex));
            throw;
        }
        if (beforeIndex == afterIndex)
            return;
        RecordWithRollback(
            () => RecordElement(
                historyName,
                new SceneElementHistoryData(
                SceneElementKind.Component,
                owner.scene.identity.persistentId,
                owner.identity.persistentId,
                component.identity.persistentId,
                GetElementType(component),
                beforeIndex,
                afterIndex,
                existsBefore: true,
                existsAfter: true,
                beforeState: [],
                afterState: [],
                incomingReferences: [])),
            () => owner.SetComponentIndex(component, beforeIndex));
    }

    /// <summary>
    /// Adds one scene system and records its identity, stable type, index, and persistent properties.
    /// </summary>
    /// <param name="scene">
    /// The loaded scene receiving the system.
    /// </param>
    /// <param name="systemType">
    /// The concrete system type to create.
    /// </param>
    /// <param name="historyName">
    /// An optional user-facing history entry name.
    /// </param>
    /// <returns>
    /// The newly registered system.
    /// </returns>
    public GameSystem AddSystem(
        GameScene scene,
        Type systemType,
        string? historyName = null
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(systemType);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(scene);
        GameSystem system = scene.AddSystem(systemType);
        try
        {
            RecordElement(
                historyName ?? $"Add {systemType.Name}",
                new SceneElementHistoryData(
                    SceneElementKind.System,
                    scene.identity.persistentId,
                    Guid.Empty,
                    system.identity.persistentId,
                    GetTypeRef(systemType),
                    beforeIndex: -1,
                    afterIndex: scene.GetSystemIndex(system),
                    existsBefore: false,
                    existsAfter: true,
                    beforeState: [],
                    afterState: SceneElementSerialization.CaptureState(
                        system,
                        m_workspace.serialization,
                        m_workspace.assets),
                    incomingReferences: []));
            return system;
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () =>
            {
                if (!system.isDestroyed && !scene.RemoveSystem(system))
                    throw new InvalidOperationException("The unrecorded system could not be removed.");
            });
            throw;
        }
    }

    /// <summary>
    /// Removes the system from its scene and records a reversible serialized history change.
    /// </summary>
    /// <param name="scene">
    /// The scene currently owning the system.
    /// </param>
    /// <param name="system">
    /// The registered system to remove.
    /// </param>
    /// <param name="historyName">
    /// An optional user-facing history entry name.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the system was removed and recorded.
    /// </returns>
    public bool RemoveSystem(
        GameScene scene,
        GameSystem system,
        string? historyName = null
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(system);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(scene);
        if (system.isDestroyed)
            return false;
        byte[] state = SceneElementSerialization.CaptureState(
            system,
            m_workspace.serialization,
            m_workspace.assets);
        SceneIncomingReferenceState[] incoming = SceneReferenceIndex.CaptureIncoming(
            system,
            scene,
            m_workspace);
        int index = scene.GetSystemIndex(system);
        Guid systemId = system.identity.persistentId;
        TypeRef typeRef = GetElementType(system);
        Type systemType = system.GetType();
        try
        {
            if (!scene.RemoveSystem(system))
                return false;
            RecordElement(
                historyName ?? $"Remove {systemType.Name}",
                new SceneElementHistoryData(
                SceneElementKind.System,
                scene.identity.persistentId,
                Guid.Empty,
                systemId,
                typeRef,
                beforeIndex: index,
                afterIndex: -1,
                existsBefore: true,
                existsAfter: false,
                beforeState: state,
                    afterState: [],
                    incoming));
        }
        catch (Exception exception)
        {
            if (m_workspace.Find<GameSystem>(systemId) is not null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
            RollbackAndRethrow(exception, () =>
            {
                GameSystem restored = SceneElementSerialization.RestoreSystem(
                    scene,
                    typeRef,
                    systemId,
                    index,
                    state,
                    m_workspace.serialization,
                    m_workspace.assets);
                RequireElementRestore(restored, state);
                RequireReferenceRestore(SceneReferenceIndex.RestoreIncoming(incoming, m_workspace));
            });
        }
        return true;
    }

    /// <summary>
    /// Resets one scene system and records its compact property state before and after Reset.
    /// </summary>
    /// <param name="scene">
    /// The scene currently owning the system.
    /// </param>
    /// <param name="system">
    /// The registered system to reset.
    /// </param>
    /// <param name="historyName">
    /// An optional user-facing history entry name.
    /// </param>
    public void ResetSystem(
        GameScene scene,
        GameSystem system,
        string? historyName = null
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(system);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(scene);
        byte[] before = SceneElementSerialization.CaptureState(
            system,
            m_workspace.serialization,
            m_workspace.assets);
        byte[] after;
        try
        {
            scene.ResetSystem(system);
            after = SceneElementSerialization.CaptureState(
                system,
                m_workspace.serialization,
                m_workspace.assets);
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () => RequireElementRestore(system, before));
            throw;
        }
        if (before.AsSpan().SequenceEqual(after))
            return;
        int index = scene.GetSystemIndex(system);
        RecordWithRollback(
            () => RecordElement(
                historyName ?? $"Reset {system.GetType().Name}",
                new SceneElementHistoryData(
                SceneElementKind.System,
                scene.identity.persistentId,
                Guid.Empty,
                system.identity.persistentId,
                GetElementType(system),
                index,
                index,
                existsBefore: true,
                existsAfter: true,
                before,
                after,
                incomingReferences: [])),
            () => RequireElementRestore(system, before));
    }

    /// <summary>
    /// Moves a registered system and records only its two display indices.
    /// </summary>
    /// <param name="scene">
    /// The scene currently owning the system.
    /// </param>
    /// <param name="system">
    /// The registered system to move.
    /// </param>
    /// <param name="systemIndex">
    /// The requested display index.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    public void SetSystemIndex(
        GameScene scene,
        GameSystem system,
        int systemIndex,
        string historyName = "Move System"
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(system);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(scene);
        int beforeIndex = scene.GetSystemIndex(system);
        int afterIndex;
        try
        {
            scene.SetSystemIndex(system, systemIndex);
            afterIndex = scene.GetSystemIndex(system);
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () => scene.SetSystemIndex(system, beforeIndex));
            throw;
        }
        if (beforeIndex == afterIndex)
            return;
        RecordWithRollback(
            () => RecordElement(
                historyName,
                new SceneElementHistoryData(
                SceneElementKind.System,
                scene.identity.persistentId,
                Guid.Empty,
                system.identity.persistentId,
                GetElementType(system),
                beforeIndex,
                afterIndex,
                existsBefore: true,
                existsAfter: true,
                beforeState: [],
                afterState: [],
                incomingReferences: [])),
            () => scene.SetSystemIndex(system, beforeIndex));
    }

    private TypeRef GetElementType(EngineObject element)
        => element switch
        {
            MissingGameComponent missing => missing.missingType,
            MissingGameSystem missing => missing.missingType,
            _ => GetTypeRef(element.GetType())
        };

    private void RequireElementRestore(
        EngineObject element,
        ReadOnlySpan<byte> data
    )
        => SceneElementSerialization.RestoreState(element, data, m_workspace.serialization, m_workspace.assets);

}
