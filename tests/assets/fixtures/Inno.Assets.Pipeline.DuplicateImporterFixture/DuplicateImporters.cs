using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.DuplicateImporterFixture;

/// <summary>
/// Supplies a stable, field-free asset for conflicting importer registration fixtures.
/// </summary>
[StableTypeId("27a0c796-5e7b-4e65-b9e5-2a0ced76675c")]
public sealed class DuplicateImporterAsset : AssetObject;

/// <summary>
/// Deliberately shares its importer identity with <see cref="DuplicateImporterB"/> to test candidate rejection.
/// </summary>
[AssetImporter("inno.tests.duplicate-importer")]
public sealed class DuplicateImporterA : AssetImporter<DuplicateImporterAsset>
{
    /// <summary>
    /// Gets this fixture's extension; distinct extensions do not make duplicate importer identities valid.
    /// </summary>
    public override IReadOnlyList<string> supportedExtensions { get; } = [".duplicateid"];

    /// <summary>
    /// Supplies a fixture asset and writes the source bytes as its runtime artifact through the borrowed writer.
    /// </summary>
    /// <param name="context">The current source snapshot owned by the import operation.</param>
    /// <param name="output">The borrowed writer receiving the candidate asset and artifact.</param>
    /// <param name="cancellationToken">Cancels the artifact write without publishing a complete candidate.</param>
    /// <returns>The artifact write operation, preserving its failure or cancellation.</returns>
    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DuplicateImporterAsset> output,
        CancellationToken cancellationToken)
    {
        output.SetAsset(new DuplicateImporterAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}

/// <summary>
/// Deliberately conflicts with <see cref="DuplicateImporterA"/> despite using a different file extension.
/// </summary>
[AssetImporter("inno.tests.duplicate-importer")]
public sealed class DuplicateImporterB : AssetImporter<DuplicateImporterAsset>
{
    /// <summary>
    /// Gets the alternate extension used to prove identity conflicts are independent of extension selection.
    /// </summary>
    public override IReadOnlyList<string> supportedExtensions { get; } = [".duplicateid-b"];

    /// <summary>
    /// Supplies a fixture asset and writes the source bytes as its runtime artifact through the borrowed writer.
    /// </summary>
    /// <param name="context">The current source snapshot owned by the import operation.</param>
    /// <param name="output">The borrowed writer receiving the candidate asset and artifact.</param>
    /// <param name="cancellationToken">Cancels the artifact write without publishing a complete candidate.</param>
    /// <returns>The artifact write operation, preserving its failure or cancellation.</returns>
    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DuplicateImporterAsset> output,
        CancellationToken cancellationToken)
    {
        output.SetAsset(new DuplicateImporterAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}
