using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal sealed class ValidationBuildInputs
{
    private readonly ValidationPlan m_plan;
    private readonly ValidationCheck[] m_checks;
    private readonly string m_path;
    private readonly Dictionary<string, Compilation> m_completed;
    private readonly Dictionary<string, string> m_outputDigests;
    private readonly CancellationToken m_cancellation;
    private readonly HashSet<string> m_selectedSources = new(StringComparer.Ordinal);

    internal ValidationBuildInputs(
        ValidationPlan plan,
        string context,
        Dictionary<string, string> outputDigests,
        CancellationToken cancellation
    ) {
        m_plan = plan;
        m_outputDigests = outputDigests;
        m_cancellation = cancellation;
        m_checks = plan.checks.Where(check => check.kind == "build" && check.context == context && check.exclusion is null).ToArray();
        foreach (ValidationCheck check in m_checks)
        {
            JsonElement snapshot = plan.contexts[context].sdkGraph.EnumerateArray().Single(snapshot =>
                Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/') == check.owner);
            var dependencyPaths = check.inputs.Where(input => input != "@" + check.owner)
                .SelectMany(input => input.StartsWith('@') ? plan.inputSets[input[1..]] : [input]).ToHashSet(StringComparer.Ordinal);
            m_selectedSources.UnionWith(snapshot.GetProperty("Items").GetProperty("Compile").EnumerateArray()
                .Select(item => Path.GetRelativePath(plan.root, item.GetProperty("FullPath").GetString()!).Replace('\\', '/'))
                .Where(path => plan.inputs.ContainsKey(path) && !dependencyPaths.Contains(path)));
        }
        m_path = PathBoundary.RequireUnlinkedPath(plan.artifacts, Path.Combine(plan.artifacts, "validation-build-" + context + ".json"));
        m_completed = File.Exists(m_path) ? JsonSerializer.Deserialize<Dictionary<string, Compilation>>(File.ReadAllBytes(m_path))
            ?? throw new InvalidDataException("Completed compilation input identities are empty.") : new(StringComparer.Ordinal);
    }

    internal bool RequiresCompilation(ValidationDependencyInputs dependencies)
        => m_checks.Any(check => !m_completed.TryGetValue(check.owner, out Compilation? previous)
            || previous.inputs != Identity(check, dependencies, allowMissing: true)
            || !ValidationOutputs.Matches(m_plan, check, new(check.id, "Passed", previous.inputs, 0, string.Empty, previous.outputs),
                m_cancellation, m_outputDigests));

    internal bool RequiresDependencyCompilation(ValidationDependencyInputs dependencies)
    {
        foreach (ValidationCheck check in m_checks)
        {
            string? reason = !m_completed.TryGetValue(check.owner, out Compilation? previous) ? "no completed compilation"
                : previous.dependencySources != Identity(check, dependencies, allowMissing: true, excludeSelectedSources: true)
                    ? "retained dependency inputs changed"
                : !ValidationOutputs.Matches(m_plan, check, new(check.id, "Passed", previous.inputs, 0, string.Empty, previous.outputs),
                    m_cancellation, m_outputDigests) ? "compiled output content changed" : null;
            if (reason is not null)
            {
                Console.WriteLine($"Dependency clean required for {check.owner}: {reason}.");
                return true;
            }
        }
        Console.WriteLine($"Retaining verified dependency outputs; cleaning {m_checks.Length} selected roots.");
        return false;
    }

    internal void Record(ValidationDependencyInputs dependencies)
    {
        foreach (ValidationCheck check in m_checks)
            m_completed[check.owner] = new(Identity(check, dependencies), ValidationOutputs.Capture(m_plan, check, m_cancellation, m_outputDigests),
                Identity(check, dependencies, excludeSelectedSources: true));
        // Publish only after closing freshness succeeds. Failed assertions do not invalidate a verified compilation.
        AtomicFile.WriteAllBytes(m_path, JsonSerializer.SerializeToUtf8Bytes(m_completed));
    }

    private string Identity(
        ValidationCheck check,
        ValidationDependencyInputs dependencies,
        bool allowMissing = false,
        bool excludeSelectedSources = false
    ) {
        ValidationCheck sourceCheck = check;
        if (excludeSelectedSources)
        {
            // Every selected root is explicitly cleaned. Preserve dependencies only when all remaining inputs and outputs match.
            // Shared inputs, imports, resources, generated files and tooling remain protected by their actual input sets.
            sourceCheck = check with { inputs = check.inputs.SelectMany(input => input.StartsWith('@') ? m_plan.inputSets[input[1..]] : [input])
                .Where(path => !m_selectedSources.Contains(path)).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray() };
        }
        return ValidationInputs.Identity(new Dictionary<string, string>
        {
            ["source"] = ValidationInputs.CheckIdentity(m_plan, sourceCheck, m_plan.inputs),
            ["dependencies"] = dependencies.Capture(check, allowMissing)
        });
    }

    private sealed record Compilation(
        string inputs,
        Dictionary<string, string> outputs,
        string dependencySources = ""
    );
}
