using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Holds validated, frozen preparation inputs without owning running processes or staging resources.
/// </summary>
public abstract class PlayerSupportPackPlan
{
    /// <summary>
    /// Fixes the publication identity before preparation is permitted.
    /// </summary>
    /// <param name="target">
    /// The nonempty target whose closure this plan prepares.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The target has no stable identity.
    /// </exception>
    protected PlayerSupportPackPlan(BuildTargetId target)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(target.value);
        this.target = target;
    }

    /// <summary>
    /// Gets the immutable target checked by the publisher before creating output.
    /// </summary>
    public BuildTargetId target { get; }

    /// <summary>
    /// Executes frozen inputs in publisher-owned staging without selecting tools again.
    /// </summary>
    /// <param name="stagingDirectory">
    /// The isolated empty directory borrowed for this execution only.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels execution and drains owned work before returning.
    /// </param>
    /// <returns>
    /// Completion when the full closure is prepared; failures propagate without publishing.
    /// </returns>
    public abstract ValueTask PrepareAsync(
        string stagingDirectory,
        CancellationToken cancellationToken
    );
}
