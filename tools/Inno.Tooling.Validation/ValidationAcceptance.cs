using System;
using System.IO;
using System.Linq;

namespace Inno.Tooling.Validation;

internal static class ValidationAcceptance
{
    internal static void RequireContinuation(
        ValidationPlan current,
        ValidationPlan accepted,
        ValidationEvidence evidence
    ) {
        _ = ValidationShards.ValidateEvidence(accepted, evidence);
        if (current.profile != ValidationProfile.Full || !current.full || current.extended
            || accepted.profile is not (ValidationProfile.Master or ValidationProfile.Full) || accepted.extended
            || current.commit != accepted.commit || current.tree != accepted.tree
            || current.nativeDependencies != accepted.nativeDependencies
        ) {
            throw new InvalidDataException("Continuation requires an exact functional candidate and default-branch execution policy.");
        }
    }

    // This reuses a completed functional certification, never mutable runner outputs or performance samples.
    // The event adapter must authenticate the original run and retained immutable artifacts before calling it.
    internal static bool Matches(
        ValidationPlan current,
        ValidationPlan accepted,
        ValidationEvidence evidence
    ) {
        _ = ValidationShards.ValidateEvidence(accepted, evidence);
        if (!current.full || !accepted.full || current.extended || accepted.extended
            || current.profile != ValidationProfile.Full || accepted.profile is not (ValidationProfile.Full or ValidationProfile.Master)
            || current.commit != accepted.commit || current.tree != accepted.tree
            || current.environment != accepted.environment || current.configuration != accepted.configuration
            || current.nativeDependencies != accepted.nativeDependencies || current.checks.Length != accepted.checks.Length
        ) {
            return false;
        }

        foreach (ValidationCheck check in current.checks)
        {
            ValidationCheck? previous = accepted.checks.SingleOrDefault(candidate => candidate.id == check.id);
            if (previous is null || check.layer == "performance" && check.exclusion is null
                || ValidationInputs.CheckIdentity(current, check, current.inputs)
                   != ValidationInputs.CheckIdentity(accepted, previous, accepted.inputs)
            ) {
                return false;
            }
            ValidationResult result = evidence.results.Single(candidate => candidate.id == check.id);
            if (check.exclusion is null && result.outputs is null)
            {
                throw new InvalidDataException("Completed acceptance has no output inventory: " + check.id);
            }
        }
        return true;
    }
}
