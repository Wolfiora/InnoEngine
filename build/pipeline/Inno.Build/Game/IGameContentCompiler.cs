using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build;

/// <summary>
/// Compiles source-free runtime content independently of application packaging.
/// </summary>
public interface IGameContentCompiler
{
    /// <summary>
    /// Gets the exact publication target whose content this compiler produces.
    /// </summary>
    BuildTargetId target { get; }

    /// <summary>
    /// Writes target content into the current operation's isolated staging directory.
    /// </summary>
    /// <param name="context">
    /// The borrowed authoring generation and operation-owned content destination.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels compilation before publication; associated work must finish before returning.
    /// </param>
    /// <returns>
    /// An operation completing after every required artifact is staged, or faulting on failure.
    /// </returns>
    /// <exception cref="System.OperationCanceledException">
    /// Cancellation was requested before compilation completed.
    /// </exception>
    ValueTask CompileAsync(
        GameBuildContentContext context,
        CancellationToken cancellationToken = default
    );
}
