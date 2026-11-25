using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.DuplicateImporterFixture;

/// <summary>
/// Supplies a stable, field-free asset for conflicting importer registration fixtures.
/// </summary>
[StableTypeID("27a0c796-5e7b-4e65-b9e5-2a0ced76675c")]
public sealed class DuplicateImporterAsset : AssetObject;
