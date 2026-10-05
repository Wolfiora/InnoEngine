using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Bgfx.Platforms;

internal abstract class BgfxBuilder
{
    internal abstract string outputPlatform { get; }
    internal abstract string artifactPathToken { get; }
    internal abstract bool IsSupported();
    internal abstract Task BuildAsync(
        string source,
        string genie,
        NativeBuildContext context,
        bool includeTools,
        CancellationToken cancellationToken
    );
}
