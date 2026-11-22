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
    internal const int C_EVIDENCE_VERSION = 2;

    internal static ValidationShard[] Create(ValidationCheck[] checks)
    {
        Dictionary<string, string> owners = checks.ToDictionary(
            static check => check.id,
            static check => (
                check.exclusion is not null || check.context is null
                ? "architecture"
                : check.context == "managed" ? "managed" : "native"
            ),
            StringComparer.Ordinal
        );
        List<ValidationShard> shards = [];
        foreach (IGrouping<string, ValidationCheck> group in checks.GroupBy(check => owners[check.id], StringComparer.Ordinal))
        {
            var prerequisites = new HashSet<string>(StringComparer.Ordinal);
            foreach (string required in group.Where(static check => check.exclusion is null)
                .SelectMany(static check => check.prerequisites)
            ) {
                if (!owners.TryGetValue(required, out string? owner))
                {
                    throw new InvalidDataException("A shard requires an unplanned check: " + required);
                }
                if (owner != group.Key)
                {
                    prerequisites.Add(owner);
                }
            }
            shards.Add(
                new ValidationShard(
                    group.Key,
                    group.Select(static check => check.id).ToArray(),
                    prerequisites.Order(StringComparer.Ordinal).ToArray()
                )
            );
        }
        return shards.ToArray();
    }

    internal static ValidationShard Require(
        ValidationPlan plan,
        string id
    ) {
        ValidationShard[] expected = Create(plan.checks);
        if (plan.shards is null
            || plan.shards.Length != expected.Length
            || expected.Any(
                shard => (
                    plan.shards.Count(
                        actual => (
                            actual.id == shard.id
                            && actual.checks.SequenceEqual(shard.checks, StringComparer.Ordinal)
                            && actual.prerequisites.SequenceEqual(shard.prerequisites, StringComparer.Ordinal)
                        )
                    ) != 1
                )
            )
        ) {
            throw new InvalidDataException("The frozen shard partition does not cover the exact plan.");
        }
        return expected.SingleOrDefault(shard => shard.id == id) ?? throw new InvalidDataException("Unknown validation shard: " + id);
    }

    internal static HashSet<string>? InvalidatedChecks(
        ValidationPlan plan,
        ValidationEvidence? previous
    ) {
        if (previous is not { completed: true, version: C_EVIDENCE_VERSION, failures: not null }
            || previous.environment != plan.environment
            || previous.results.DistinctBy(result => result.id).Count() != previous.results.Length
        ) {
            return null;
        }

        var invalid = new HashSet<string>(StringComparer.Ordinal);
        foreach (ValidationRunFailure fault in previous.failures)
        {
            if (fault.stage == "output-stability")
            {
                ValidationCheck? producer = plan.checks.SingleOrDefault(check => check.id == fault.owner);
                if (producer is null)
                {
                    return null;
                }
                invalid.Add(producer.id);
                foreach (ValidationCheck consumer in plan.checks.Where(check => (
                    check.context == producer.context
                    && check.inputs.Contains("@" + producer.owner, StringComparer.Ordinal)
                ))) {
                    invalid.Add(consumer.id);
                }
            }
            else if (fault.stage == "test-execution")
            {
                ValidationCheck[] owners = plan.checks.Where(check => check.kind == "test" && check.context == fault.owner).ToArray();
                if (!owners.Any(owner => previous.results.Any(result => result.id == owner.id && result.status == "Failed")))
                {
                    invalid.UnionWith(owners.Select(owner => owner.id));
                }
            }
            else
            {
                return null;
            }
        }
        if (invalid.Count != 0)
        {
            int before;
            do
            {
                before = invalid.Count;
                foreach (ValidationCheck check in plan.checks.Where(check => check.prerequisites.Any(invalid.Contains)))
                {
                    invalid.Add(check.id);
                }
            }
            while (before != invalid.Count);
        }
        return invalid;
    }

    internal static ValidationResult[] ValidateEvidence(
        ValidationPlan plan,
        ValidationEvidence evidence,
        string? shard = null
    ) {
        if (evidence.version != C_EVIDENCE_VERSION || evidence.failures is null)
        {
            throw new InvalidDataException("Validation evidence has an unsupported format.");
        }
        if (evidence.commit != plan.commit
            || evidence.tree != plan.tree
            || evidence.environment != plan.environment
            || evidence.planIdentity != ValidationInputs.PlanIdentity(plan)
            || evidence.shard != shard
        ) {
            throw new InvalidDataException("Validation evidence has a different candidate, plan, environment or execution scope.");
        }
        HashSet<string>? selected = shard is null ? null : Require(plan, shard).checks.ToHashSet(StringComparer.Ordinal);
        ValidationCheck[] checks = plan.checks.Where(check => selected is null || selected.Contains(check.id)).ToArray();
        if (checks.Length == 0)
        {
            throw new InvalidDataException("No planned checks were discovered.");
        }
        HashSet<string> expected = checks.Select(static check => check.id).ToHashSet(StringComparer.Ordinal);
        ILookup<string, ValidationResult> observed = evidence.results.ToLookup(static result => result.id, StringComparer.Ordinal);
        List<string> errors = [];
        foreach (IGrouping<string, ValidationResult> group in observed)
        {
            if (!expected.Contains(group.Key))
            {
                errors.Add("Unplanned check: " + group.Key);
            }
            if (group.Count() != 1)
            {
                errors.Add("Duplicate check: " + group.Key);
            }
        }
        foreach (ValidationCheck check in checks)
        {
            ValidationResult? result = observed[check.id].FirstOrDefault();
            if (result is null)
            {
                errors.Add("Missing check: " + check.id);
            }
            else if (result.inputIdentity != ValidationInputs.CheckIdentity(plan, check, plan.inputs))
            {
                errors.Add("Stale check: " + check.id);
            }
            else if (check.exclusion is null ? result.status is not ("Passed" or "Reused") : result.status != "NotApplicable")
            {
                errors.Add(check.id + ": " + result.detail);
            }
        }
        errors.AddRange(evidence.failures.Select(static fault => fault.stage + ": " + fault.detail));
        if (!evidence.completed)
        {
            errors.Add("Validation did not complete closing verification.");
        }
        if (errors.Count != 0)
        {
            throw new InvalidDataException(string.Join(Environment.NewLine, errors));
        }
        return evidence.results;
    }

    internal static ValidationResult[] Prerequisites(
        ValidationPlan plan,
        ValidationShard shard,
        IReadOnlyList<ValidationEvidence> evidence
    ) {
        if (evidence.Count != shard.prerequisites.Length)
        {
            throw new InvalidDataException("The shard is missing prerequisites or received unrelated receipts.");
        }
        return shard.prerequisites.SelectMany(id =>
        {
            ValidationEvidence[] matches = evidence.Where(receipt => receipt.shard == id).ToArray();
            if (matches.Length != 1)
            {
                throw new InvalidDataException("Missing or duplicate prerequisite shard: " + id);
            }
            return ValidateEvidence(plan, matches[0], id);
        }).ToArray();
    }

    internal static ValidationEvidence Join(
        ValidationPlan plan,
        IReadOnlyList<ValidationEvidence> evidence
    ) {
        if (plan.shards is null || evidence.Count != plan.shards.Length)
        {
            throw new InvalidDataException("The required shard matrix is incomplete or duplicated.");
        }
        List<ValidationResult> results = [];
        foreach (ValidationShard shard in plan.shards)
        {
            _ = Require(plan, shard.id);
            ValidationEvidence[] matches = evidence.Where(receipt => receipt.shard == shard.id).ToArray();
            if (matches.Length != 1)
            {
                throw new InvalidDataException("Missing or duplicate execution shard: " + shard.id);
            }
            results.AddRange(ValidateEvidence(plan, matches[0], shard.id));
        }

        var joined = new ValidationEvidence(
            plan.commit,
            plan.tree,
            plan.environment,
            evidence.Min(static receipt => receipt.started),
            results.ToArray(),
            C_EVIDENCE_VERSION,
            [],
            ValidationInputs.PlanIdentity(plan),
            completed: true
        );
        _ = ValidateEvidence(plan, joined);
        return joined;
    }
}
