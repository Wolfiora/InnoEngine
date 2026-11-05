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
        => m_checks.Any(check => !m_completed.TryGetValue(check.owner, out Compilation? previous)
            || previous.dependencySources != Identity(check, dependencies, allowMissing: true, excludeOwnerSources: true)
            || !ValidationOutputs.Matches(m_plan, check, new(check.id, "Passed", previous.inputs, 0, string.Empty, previous.outputs),
                m_cancellation, m_outputDigests));

    internal void Record(ValidationDependencyInputs dependencies)
    {
        foreach (ValidationCheck check in m_checks)
            m_completed[check.owner] = new(Identity(check, dependencies), ValidationOutputs.Capture(m_plan, check, m_cancellation, m_outputDigests),
                Identity(check, dependencies, excludeOwnerSources: true));
        // Publish only after closing freshness succeeds. Failed assertions do not invalidate a verified compilation.
        AtomicFile.WriteAllBytes(m_path, JsonSerializer.SerializeToUtf8Bytes(m_completed));
    }

    private string Identity(
        ValidationCheck check,
        ValidationDependencyInputs dependencies,
        bool allowMissing = false,
        bool excludeOwnerSources = false
    ) {
        ValidationCheck sourceCheck = check;
        if (excludeOwnerSources)
        {
            JsonElement snapshot = m_plan.contexts[check.context!].sdkGraph.EnumerateArray().Single(snapshot =>
                Path.GetRelativePath(m_plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/') == check.owner);
            var dependencyPaths = check.inputs.Where(input => input != "@" + check.owner)
                .SelectMany(input => input.StartsWith('@') ? m_plan.inputSets[input[1..]] : [input]).ToHashSet(StringComparer.Ordinal);
            var ownerSources = snapshot.GetProperty("Items").GetProperty("Compile").EnumerateArray()
                .Select(item => Path.GetRelativePath(m_plan.root, item.GetProperty("FullPath").GetString()!).Replace('\\', '/'))
                .Where(path => m_plan.inputs.ContainsKey(path) && !dependencyPaths.Contains(path)).ToHashSet(StringComparer.Ordinal);
            // Keep shared/linked inputs, imports, resources, generators, tools and the entire dependency closure in the proof.
            sourceCheck = check with { inputs = check.inputs.SelectMany(input => input.StartsWith('@') ? m_plan.inputSets[input[1..]] : [input])
                .Where(path => !ownerSources.Contains(path)).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray() };
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
