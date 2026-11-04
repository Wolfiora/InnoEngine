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

    internal void Record(ValidationDependencyInputs dependencies)
    {
        foreach (ValidationCheck check in m_checks)
            m_completed[check.owner] = new(Identity(check, dependencies), ValidationOutputs.Capture(m_plan, check, m_cancellation, m_outputDigests));
        // This records completed compilation, never successful tests. The caller retains the workspace writer lease.
        AtomicFile.WriteAllBytes(m_path, JsonSerializer.SerializeToUtf8Bytes(m_completed));
    }

    private string Identity(
        ValidationCheck check,
        ValidationDependencyInputs dependencies,
        bool allowMissing = false
    )
        => ValidationInputs.Identity(new Dictionary<string, string>
        {
            ["source"] = ValidationInputs.CheckIdentity(m_plan, check, m_plan.inputs),
            ["dependencies"] = dependencies.Capture(check, allowMissing)
        });

    private sealed record Compilation(
        string inputs,
        Dictionary<string, string> outputs
    );
}
