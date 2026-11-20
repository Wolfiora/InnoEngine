using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Tooling.Validation;
internal sealed record ValidationShard(
    string id,
    string[] checks,
    string[] prerequisites
);
internal static class ValidationShards
{
    internal static ValidationShard[] Create(ValidationCheck[] checks)
    {
        var owners = checks.ToDictionary(static check => check.id,
            static check => check.exclusion is not null || check.context is null ? "architecture"
                : check.context == "managed" ? "managed" : "native", StringComparer.Ordinal);
        var shards = new List<ValidationShard>();
        foreach (var group in checks.GroupBy(check => owners[check.id], StringComparer.Ordinal))
        {
            var prerequisites = new HashSet<string>(StringComparer.Ordinal);
            foreach (string required in group.Where(static check => check.exclusion is null)
                .SelectMany(static check => check.prerequisites))
            {
                if (!owners.TryGetValue(required, out string? owner))
                    throw new InvalidDataException("A shard requires an unplanned check: " + required);
                if (owner != group.Key)
                    prerequisites.Add(owner);
            }
            shards.Add(new(group.Key, group.Select(static check => check.id).ToArray(), prerequisites.Order(StringComparer.Ordinal).ToArray()));
        }
        return shards.ToArray();
    }

    internal static ValidationShard Require(
        ValidationPlan plan,
        string id
    ) {
        ValidationShard[] expected = Create(plan.checks);
        if (plan.shards is null || plan.shards.Length != expected.Length || expected.Any(shard => plan.shards.Count(actual => actual.id == shard.id && actual.checks.SequenceEqual(shard.checks, StringComparer.Ordinal) && actual.prerequisites.SequenceEqual(shard.prerequisites, StringComparer.Ordinal)) != 1))
            throw new InvalidDataException("The frozen shard partition does not cover the exact plan.");
        return expected.SingleOrDefault(shard => shard.id == id) ?? throw new InvalidDataException("Unknown validation shard: " + id);
    }

    internal static ValidationResult[] ValidateEvidence(
        ValidationPlan plan,
        ValidationEvidence evidence,
        string? shard = null
    ) {
        if (!evidence.completed || evidence.commit != plan.commit || evidence.tree != plan.tree || evidence.environment != plan.environment || evidence.planIdentity != ValidationInputs.PlanIdentity(plan) || evidence.shard != shard)
            throw new InvalidDataException("Validation evidence has a different candidate, plan, environment or execution scope.");
        HashSet<string>? selected = shard is null ? null : Require(plan, shard).checks.ToHashSet(StringComparer.Ordinal);
        ValidationCheck[] checks = plan.checks.Where(check => selected is null || selected.Contains(check.id)).ToArray();
        if (checks.Length == 0 || evidence.results.Length != checks.Length)
            throw new InvalidDataException("Validation evidence is empty, missing, duplicated or contains unplanned results.");
        foreach (ValidationCheck check in checks)
        {
            ValidationResult[] results = evidence.results.Where(result => result.id == check.id).ToArray();
            if (results.Length != 1 || results[0].inputIdentity != ValidationInputs.CheckIdentity(plan, check, plan.inputs) || (check.exclusion is null ? results[0].status is not ("Passed" or "Reused") : results[0].status != "NotApplicable"))
                throw new InvalidDataException("Required validation evidence is unsuccessful or stale: " + check.id);
        }

        return evidence.results;
    }

    internal static ValidationResult[] Prerequisites(
        ValidationPlan plan,
        ValidationShard shard,
        IReadOnlyList<ValidationEvidence> evidence
    ) {
        if (evidence.Count != shard.prerequisites.Length)
            throw new InvalidDataException("The shard is missing prerequisites or received unrelated receipts.");
        return shard.prerequisites.SelectMany(id =>
        {
            ValidationEvidence[] matches = evidence.Where(receipt => receipt.shard == id).ToArray();
            if (matches.Length != 1)
                throw new InvalidDataException("Missing or duplicate prerequisite shard: " + id);
            return ValidateEvidence(plan, matches[0], id);
        }).ToArray();
    }

    internal static ValidationEvidence Join(
        ValidationPlan plan,
        IReadOnlyList<ValidationEvidence> evidence
    ) {
        if (plan.shards is null || evidence.Count != plan.shards.Length)
            throw new InvalidDataException("The required shard matrix is incomplete or duplicated.");
        var results = new List<ValidationResult>();
        foreach (ValidationShard shard in plan.shards)
        {
            _ = Require(plan, shard.id);
            ValidationEvidence[] matches = evidence.Where(receipt => receipt.shard == shard.id).ToArray();
            if (matches.Length != 1)
                throw new InvalidDataException("Missing or duplicate execution shard: " + shard.id);
            results.AddRange(ValidateEvidence(plan, matches[0], shard.id));
        }

        var joined = new ValidationEvidence(plan.commit, plan.tree, plan.environment, evidence.Min(static receipt => receipt.started), results.ToArray(), ValidationInputs.PlanIdentity(plan), completed: true);
        _ = ValidateEvidence(plan, joined);
        return joined;
    }
}
