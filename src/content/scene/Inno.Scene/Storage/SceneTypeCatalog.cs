using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Catalogs;
using Inno.Extensibility.Types;

namespace Inno.Scene;

internal sealed class SceneTypeCatalog : IDisposable
{
    private readonly TypeCatalog m_types;
    private readonly SceneTypeRegistry m_registry;

    internal SceneTypeCatalog(TypeCatalog types)
    {
        ArgumentNullException.ThrowIfNull(types);
        m_types = types;
        m_registry = new SceneTypeRegistry(types);
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose() => m_registry.Dispose();

    internal TypeCacheSnapshot snapshot => m_types.current;

    internal long generation => m_registry.snapshot.generation;

    internal bool TryGetComponent(
        Type type,
        out SceneComponentTypeDescriptor? descriptor
    ) {
        ArgumentNullException.ThrowIfNull(type);
        descriptor = null;
        return m_types.TryGetTypeRef(type, out TypeRef typeRef)
               && m_registry.snapshot.componentsByRuntimeID.TryGetValue(typeRef.runtimeID, out descriptor);
    }

    internal SceneComponentTypeDescriptor GetComponent(Type type)
    {
        if (TryGetComponent(type, out SceneComponentTypeDescriptor? descriptor))
        {
            return descriptor!;
        }
        throw new InvalidOperationException(
            $"GameComponent type '{type.FullName}' is not part of the active TypeCache generation."
        );
    }

    internal SceneComponentTypeDescriptor GetComponent(int runtimeTypeID)
    {
        if (m_registry.snapshot.componentsByRuntimeID.TryGetValue(
            runtimeTypeID,
            out SceneComponentTypeDescriptor? descriptor
        )) {
            return descriptor;
        }
        throw new InvalidOperationException(
            $"Component runtime type ID '{runtimeTypeID}' is not part of the active TypeCache generation."
        );
    }

    internal bool TryGetSystem(
        Type type,
        out SceneSystemTypeDescriptor? descriptor
    ) {
        ArgumentNullException.ThrowIfNull(type);
        descriptor = null;
        return m_types.TryGetTypeRef(type, out TypeRef typeRef)
               && m_registry.snapshot.systemsByRuntimeID.TryGetValue(typeRef.runtimeID, out descriptor);
    }

    internal SceneSystemTypeDescriptor GetSystem(Type type)
    {
        if (TryGetSystem(type, out SceneSystemTypeDescriptor? descriptor))
        {
            return descriptor!;
        }
        throw new InvalidOperationException(
            $"GameSystem type '{type.FullName}' is not part of the active TypeCache generation."
        );
    }

    internal Type Resolve(TypeRef typeRef) => m_types.Resolve(typeRef);

    internal TypeRef GetTypeRef(Type type) => m_types.GetTypeRef(type);

    private sealed class SceneTypeRegistry : TypeRegistry<SceneTypeSnapshot>
    {
        internal SceneTypeRegistry(TypeCatalog types)
            : base(types) {}

        internal SceneTypeSnapshot snapshot => current;

        /// <summary>
        /// Builds a validated result from the current immutable input snapshot.
        /// </summary>
        /// <param name="types">
        /// The active type catalog generation used for extension resolution.
        /// </param>
        /// <returns>
        /// The validated scene type snapshot that represents the completed operation.
        /// </returns>
        protected override SceneTypeSnapshot Build(TypeCacheSnapshot types)
        {
            Type[] componentTypes = types.types
                .Select(typeRef => typeRef.Resolve(types))
                .Where(static type => typeof(GameComponent).IsAssignableFrom(type))
                .ToArray();
            Type[] concreteComponentTypes = componentTypes
                .Where(static type => type.IsClass && !type.IsAbstract)
                .ToArray();

            var componentsByRuntimeID = new Dictionary<int, SceneComponentTypeDescriptor>(componentTypes.Length);
            foreach (Type componentType in componentTypes)
            {
                TypeRef componentTypeRef = types.GetTypeRef(componentType);
                int[] assignableConcreteRuntimeTypeIDs = concreteComponentTypes
                    .Where(componentType.IsAssignableFrom)
                    .OrderBy(static type => type.FullName, StringComparer.Ordinal)
                    .Select(types.GetTypeRef)
                    .Select(static typeRef => typeRef.runtimeID)
                    .ToArray();
                var descriptor = new SceneComponentTypeDescriptor(
                    componentTypeRef.runtimeID,
                    componentType.FullName ?? componentType.Name,
                    componentType.IsClass && !componentType.IsAbstract,
                    types.GetAttribute<AllowMultipleComponentAttribute>(componentTypeRef, inherit: true) is not null,
                    GetBehaviorPhases(types.GetMetadata(componentTypeRef)),
                    assignableConcreteRuntimeTypeIDs,
                    assignableConcreteRuntimeTypeIDs.ToFrozenSet()
                );
                componentsByRuntimeID.Add(descriptor.runtimeTypeID, descriptor);
            }

            Type[] systemTypes = types.types
                .Select(typeRef => typeRef.Resolve(types))
                .Where(static type => typeof(GameSystem).IsAssignableFrom(type))
                .ToArray();
            var systemsByRuntimeID = new Dictionary<int, SceneSystemTypeDescriptor>(systemTypes.Length);
            foreach (Type systemType in systemTypes)
            {
                TypeRef systemTypeRef = types.GetTypeRef(systemType);
                var descriptor = new SceneSystemTypeDescriptor(
                    systemTypeRef.runtimeID,
                    systemType.FullName ?? systemType.Name,
                    systemType.IsClass && !systemType.IsAbstract,
                    types.GetAttribute<AllowMultipleSystemAttribute>(systemTypeRef, inherit: false) is not null
                );
                systemsByRuntimeID.Add(descriptor.runtimeTypeID, descriptor);
            }

            return new SceneTypeSnapshot(
                types.version,
                componentsByRuntimeID.ToFrozenDictionary(),
                systemsByRuntimeID.ToFrozenDictionary()
            );
        }

        /// <summary>
        /// Validates and prepares a candidate snapshot before it can become active.
        /// </summary>
        /// <param name="previous">
        /// The previous consumed by on activating; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <param name="candidate">
        /// The candidate consumed by on activating; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        protected override void OnActivating(
            SceneTypeSnapshot? previous,
            SceneTypeSnapshot candidate
        ) => SceneStore.InvalidateAllTypeCaches();

        /// <summary>
        /// Restores state retained for the previous snapshot after candidate activation fails.
        /// </summary>
        /// <param name="previous">
        /// The previous consumed by on activation rolled back; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <param name="candidate">
        /// The candidate consumed by on activation rolled back; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        protected override void OnActivationRolledBack(
            SceneTypeSnapshot? previous,
            SceneTypeSnapshot candidate
        ) => SceneStore.InvalidateAllTypeCaches();

        private static GameBehaviorLifecyclePhase GetBehaviorPhases(TypeCatalogMetadata metadata)
        {
            if (!typeof(GameBehavior).IsAssignableFrom(metadata.type))
            {
                return GameBehaviorLifecyclePhase.None;
            }

            GameBehaviorLifecyclePhase phases = GameBehaviorLifecyclePhase.None;
            AddOverride("Awake", GameBehaviorLifecyclePhase.Awake);
            AddOverride("Start", GameBehaviorLifecyclePhase.Start);
            AddOverride("OnEnable", GameBehaviorLifecyclePhase.Enable);
            AddOverride("OnDisable", GameBehaviorLifecyclePhase.Disable);
            AddOverride("Update", GameBehaviorLifecyclePhase.Update);
            AddOverride("FixedUpdate", GameBehaviorLifecyclePhase.FixedUpdate);
            AddOverride("LateUpdate", GameBehaviorLifecyclePhase.LateUpdate);
            AddOverride("OnDestroy", GameBehaviorLifecyclePhase.Destroy);
            return phases;

            void AddOverride(
                string callbackName,
                GameBehaviorLifecyclePhase phase
            ) {
                if (metadata.parameterlessOverrides.Contains((callbackName, typeof(GameBehavior))))
                {
                    phases |= phase;
                }
            }
        }
    }
}

internal sealed record SceneTypeSnapshot(
    long generation,
    FrozenDictionary<int, SceneComponentTypeDescriptor> componentsByRuntimeID,
    FrozenDictionary<int, SceneSystemTypeDescriptor> systemsByRuntimeID
);

internal sealed record SceneComponentTypeDescriptor(
    int runtimeTypeID,
    string displayName,
    bool isConcrete,
    bool allowsMultiple,
    GameBehaviorLifecyclePhase behaviorPhases,
    int[] assignableConcreteRuntimeTypeIDs,
    FrozenSet<int> assignableConcreteRuntimeTypeIDSet
) {
    internal bool IsAssignableFrom(int concreteRuntimeTypeID) => assignableConcreteRuntimeTypeIDSet.Contains(concreteRuntimeTypeID);
}

[Flags]
internal enum GameBehaviorLifecyclePhase
{
    None = 0,
    Awake = 1 << 0,
    Start = 1 << 1,
    Enable = 1 << 2,
    Disable = 1 << 3,
    Update = 1 << 4,
    FixedUpdate = 1 << 5,
    LateUpdate = 1 << 6,
    Destroy = 1 << 7,
    Activation = Awake | Enable | Disable | Destroy,
    VariableFrame = Start | Update | LateUpdate,
    Any = Activation | VariableFrame | FixedUpdate
}

internal sealed record SceneSystemTypeDescriptor(
    int runtimeTypeID,
    string displayName,
    bool isConcrete,
    bool allowsMultiple
);
