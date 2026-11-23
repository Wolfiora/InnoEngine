using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Core.Settings;

internal sealed class ProjectSettingsRegistry : TypeRegistry<ProjectSettingsRegistry.Snapshot>
{
    internal ProjectSettingsRegistry(TypeCatalog types)
        : base(types) {}

    internal Snapshot settings => current;

    /// <summary>
    /// Builds a validated result from the current immutable input snapshot.
    /// </summary>
    /// <param name="types">
    /// The active type catalog generation used for extension resolution.
    /// </param>
    /// <returns>
    /// The validated snapshot that represents the completed operation.
    /// </returns>
    protected override Snapshot Build(TypeCacheSnapshot types)
    {
        var settingTypes = new Dictionary<ProjectSettingID, (Guid stableTypeID, Type runtimeType, bool allowPluginContributions)>();
        foreach (TypeRef typeRef in types.GetTypesWithAttribute<ProjectSettingDefinitionAttribute>())
        {
            Type type = typeRef.Resolve(types);
            if (type.IsAbstract || !typeof(ISerializable).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    $"Project setting type '{type.FullName}' must be a non-abstract ISerializable class."
                );
            }
            if (!types.CanCreateInstance(typeRef))
            {
                throw new InvalidOperationException(
                    $"Project setting type '{type.FullName}' requires a factory in the current type catalog."
                );
            }
            if (!types.TryGetTypeRef(type, out TypeRef stableType))
            {
                throw new InvalidOperationException($"Project setting type '{type.FullName}' requires StableTypeID.");
            }
            ProjectSettingDefinitionAttribute attribute = types.GetAttribute<ProjectSettingDefinitionAttribute>(typeRef, inherit: false)!;
            var id = new ProjectSettingID(attribute.id);
            if (!settingTypes.TryAdd(id, (stableType.stableID, type, attribute.allowPluginContributions)))
            {
                throw new InvalidOperationException($"Project setting ID '{id}' is declared more than once.");
            }
        }

        var composers = new Dictionary<ProjectSettingID, ProjectSettingComposer>();
        foreach (TypeRef typeRef in types.GetTypesWithAttribute<ProjectSettingComposerAttribute>())
        {
            Type type = typeRef.Resolve(types);
            if (type.IsAbstract || !typeof(ProjectSettingComposer).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    $"Project setting composer '{type.FullName}' must be a non-abstract ProjectSettingComposer."
                );
            }
            if (!types.CanCreateInstance(typeRef))
            {
                throw new InvalidOperationException(
                    $"Project setting composer '{type.FullName}' requires a factory in the current type catalog."
                );
            }
            ProjectSettingComposerAttribute attribute = types.GetAttribute<ProjectSettingComposerAttribute>(typeRef, inherit: false)!;
            if (!settingTypes.TryGetValue(
                attribute.settingID,
                out (Guid stableTypeID, Type runtimeType, bool allowPluginContributions) setting
            )) {
                throw new InvalidOperationException(
                    $"Project setting composer '{type.FullName}' targets undefined setting '{attribute.settingID}'."
                );
            }
            ProjectSettingComposer composer = OwnCandidateExtension((ProjectSettingComposer)types.CreateInstance(type));
            if (composer.settingType != setting.runtimeType)
            {
                throw new InvalidOperationException(
                    $"Project setting composer '{type.FullName}' expects '{composer.settingType.FullName}', "
                    + $"but '{attribute.settingID}' is '{setting.runtimeType.FullName}'."
                );
            }
            if (!composers.TryAdd(attribute.settingID, composer))
            {
                throw new InvalidOperationException(
                    $"Project setting '{attribute.settingID}' declares more than one composer."
                );
            }
        }

        var definitions = new Dictionary<ProjectSettingID, Definition>();
        foreach ((ProjectSettingID id, (Guid stableTypeID, Type runtimeType, bool allowPluginContributions)) in settingTypes)
        {
            composers.TryGetValue(id, out ProjectSettingComposer? composer);
            definitions.Add(
                id,
                new Definition(
                    stableTypeID,
                    runtimeType,
                    composer,
                    allowPluginContributions,
                    () => (ISerializable)types.CreateInstance(runtimeType)
                )
            );
        }
        return new Snapshot(types.version, definitions.ToFrozenDictionary());
    }

    /// <summary>
    /// Retires every composer owned by the replaced definition snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// The definition generation whose composers are no longer reachable through settings.
    /// </param>
    protected override void DisposeSnapshot(Snapshot snapshot)
        => DisposeExtensions(snapshot.definitions.Select(static pair => pair.Value.composer).OfType<object>());

    internal sealed class Snapshot
    {
        private readonly FrozenDictionary<ProjectSettingID, Definition> m_definitions;

        internal Snapshot(
            long typeCacheVersion,
            FrozenDictionary<ProjectSettingID, Definition> definitions
        ) {
            this.typeCacheVersion = typeCacheVersion;
            m_definitions = definitions;
        }

        internal long typeCacheVersion { get; }

        internal IEnumerable<KeyValuePair<ProjectSettingID, Definition>> definitions => m_definitions;

        internal bool TryGet(
            ProjectSettingID id,
            out Definition definition
        ) => m_definitions.TryGetValue(id, out definition!);
    }

    internal sealed record Definition(
        Guid stableTypeID,
        Type runtimeType,
        ProjectSettingComposer? composer,
        bool allowPluginContributions,
        Func<ISerializable> factory
    ) {
        internal ISerializable Create() => factory();
    }
}
