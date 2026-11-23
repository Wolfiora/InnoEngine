using System;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Scene;

internal sealed class SceneElementReferenceResolver(
    SceneWorld world,
    TypeCacheSnapshot types
) : IReferenceResolver
{
    ReferenceKindID IReferenceResolver.kindID => id;

    ReferenceResolution IReferenceResolver.Resolve(ReferenceDescriptor descriptor)
    {
        EngineObject? target = world.Find<EngineObject>(descriptor.targetPersistentID);
        if (target is null || target.isDestroyed || target is MissingGameComponent or MissingGameSystem)
        {
            return new ReferenceResolution(
                descriptor,
                ReferenceResolutionState.Missing,
                diagnostic: $"Scene element '{descriptor.targetPersistentID:D}' or its type is unavailable."
            );
        }
        if (descriptor.expectedStableTypeID != Guid.Empty
            && types.GetTypeRef(target.GetType()).stableID != descriptor.expectedStableTypeID
        ) {
            return new ReferenceResolution(
                descriptor,
                ReferenceResolutionState.TypeMismatch,
                diagnostic: $"Scene element '{descriptor.targetPersistentID:D}' does not implement stable type '{descriptor.expectedStableTypeID:D}'."
            );
        }
        return new ReferenceResolution(descriptor, ReferenceResolutionState.Resolved, target.identity.runtimeIdentity);
    }

    internal static ReferenceKindID id { get; } = new("inno.reference.scene-element");
}
