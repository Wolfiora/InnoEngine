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
    /// <param name="preparationIdentity">
    /// The complete lowercase SHA-256 input identity captured without preparing product output.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The target has no stable identity.
    /// </exception>
    protected PlayerSupportPackPlan(
        BuildTargetId target,
        string preparationIdentity
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(target.value);
        if (preparationIdentity is null || preparationIdentity.Length != 64
            || System.Linq.Enumerable.Any(preparationIdentity, static character => !char.IsAsciiHexDigitLower(character)))
            throw new ArgumentException("A Support Pack requires a complete lowercase source input identity.", nameof(preparationIdentity));
        this.target = target;
        this.preparationIdentity = preparationIdentity;
    }

    /// <summary>
    /// Gets the immutable target checked by the publisher before creating output.
    /// </summary>
    public BuildTargetId target { get; }

    /// <summary>
    /// Gets the complete source preparation identity, independently of the resulting Pack content fingerprint.
    /// </summary>
    public string preparationIdentity { get; }

    /// <summary>
    /// Freshly verifies the planned source graph after an ownership wait or preparation work.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels verification before selecting any Pack.
    /// </param>
    /// <returns>
    /// Completion only when all current inputs still match the frozen preparation identity.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The planned inputs changed and cannot be accepted.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Verification was canceled.
    /// </exception>
    public abstract ValueTask VerifyInputsAsync(CancellationToken cancellationToken);

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
