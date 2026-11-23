using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetBuildProcessor("inno.tests.aggregate-build")]
internal sealed class TestBuildProcessor : AssetBuildProcessor<TestBuildDefinitionAsset>
{
    protected override ValueTask BuildAsync(
        AssetBuildContext<TestBuildDefinitionAsset> context,
        AssetArtifactWriter output,
        CancellationToken cancellationToken
    ) {
        string value = context.definition.label + ":" + context.inputs.Count;
        return output.WriteAsync("result", Encoding.UTF8.GetBytes(value), cancellationToken: cancellationToken);
    }
}
