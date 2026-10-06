using Inno.Assets.Pipeline;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Build.Composition;

/// <summary>
/// Composes a platform packager over the caller's active authoring generation.
/// </summary>
/// <param name="assets">
/// The borrowed authoring asset owner whose content is frozen by the build pipeline.
/// </param>
/// <param name="serialization">
/// The borrowed serialization registry owning authored contracts.
/// </param>
/// <param name="types">
/// The borrowed extension catalog owning compiler and shader implementations.
/// </param>
/// <returns>
/// A non-null platform target retained by the pipeline, with no independent lifetime owner.
/// </returns>
public delegate IGameBuildTarget BuildTargetFactory(
    AssetPipeline assets,
    SerializationRegistry serialization,
    TypeCatalog types
);
