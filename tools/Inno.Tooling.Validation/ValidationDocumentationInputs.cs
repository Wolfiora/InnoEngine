using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Inno.Tooling.Validation;

internal static class ValidationDocumentationInputs
{
    internal static ValidationDocumentationInput[] Capture(
        ValidationPlan plan,
        ValidationCheck check,
        ValidationDependencyInputs dependencies
    ) {
        if (check.kind is not ("documentation-api" or "documentation-examples") || check.projects is not { Length: > 0 } || check.documentation is null)
            return [];
        var contexts = plan.contexts[check.context ?? "managed"].sdkGraph.EnumerateArray().ToArray();
        var owners = contexts.SelectMany(snapshot => snapshot.GetProperty("Instances").EnumerateArray()
            .Select(instance => new KeyValuePair<string, string>(instance.GetProperty("Identity").GetString()!,
                Path.GetRelativePath(plan.root, snapshot.GetProperty("Path").GetString()!).Replace('\\', '/'))))
            .ToDictionary(StringComparer.Ordinal);
        return check.projects.Select(project =>
        {
            // Script example execution also consumes its separately declared projection hosts.
            // Keep those real build inputs, rather than certifying only the page's API owner.
            string[] roots = check.kind == "documentation-examples"
                ? check.inputs.Where(static input => input.StartsWith('@') && input is not ("@tooling" or "@native-sources"))
                    .Select(static input => input[1..]).Append(project).Distinct(StringComparer.Ordinal).ToArray()
                : [project];
            ValidationCheck selected = check with { projects = roots };
            JsonElement[] instances = ValidationCompiledGraph.Select(plan, selected).ToArray();
            string[] closure = instances.Select(instance => owners[instance.GetProperty("Identity").GetString()!])
                .Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
            string[] inputSets = closure.Append("tooling")
                .Concat(check.inputs.Contains("@native-sources", StringComparer.Ordinal) ? ["native-sources"] : []).ToArray();
            var inputs = inputSets.SelectMany(owner => plan.inputSets[owner]).Distinct(StringComparer.Ordinal)
                .Select(path => new KeyValuePair<string, string>(path, plan.inputs.GetValueOrDefault(path, "missing")))
                .ToDictionary(StringComparer.Ordinal);
            var sources = new Dictionary<string, string>(inputs, StringComparer.Ordinal);
            inputs.Add("configuration", check.documentation.configuration);
            inputs.Add("target", check.documentation.target ?? string.Empty);
            inputs.Add("context", JsonSerializer.Serialize(plan.contexts[check.context ?? "managed"].properties));
            inputs.Add("instances", JsonSerializer.Serialize(instances));
            return new ValidationDocumentationInput(project, closure, ValidationInputs.Identity(inputs), plan.environment,
                check.documentation.configuration, check.documentation.target,
                ValidationDependencyInputs.Required(check) ? dependencies.CapturePackages(selected) : new(StringComparer.Ordinal), sources);
        }).ToArray();
    }
}
