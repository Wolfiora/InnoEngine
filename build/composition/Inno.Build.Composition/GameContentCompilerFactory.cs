using Inno.Assets.Pipeline;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Build.Composition;

/// <summary>
/// Creates a content compiler borrowing the active authoring services for one pipeline.
/// </summary>
/// <param name="assets">
/// The asset owner whose content the build freezes.
/// </param>
/// <param name="serialization">
/// The matching serialization registry; ownership remains with the host.
/// </param>
/// <param name="types">
/// The matching extension catalog; the compiler must not outlive its pipeline.
/// </param>
/// <returns>
/// A non-null scoped compiler that never disposes or globally retains these services.
/// </returns>
public delegate IGameContentCompiler GameContentCompilerFactory(
    AssetPipeline assets,
    SerializationRegistry serialization,
    TypeCatalog types
);
