using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Inno.Tooling.Validation;

internal sealed record ValidationPlan(
    string root,
    string baseline,
    string commit,
    string tree,
    string environment,
    string configuration,
    string dotnet,
    string artifacts,
    bool full,
    string[] changes,
    string[] owners,
    ValidationCheck[] checks,
    string[] diagnostics,
    Dictionary<string, string> inputs,
    Dictionary<string, string[]> inputSets,
    Dictionary<string, ValidationContext> contexts,
    bool extended = false,
    Dictionary<string, double>? planningSeconds = null
);

internal sealed record ValidationContext(
    Dictionary<string, string> properties,
    JsonElement sdkGraph
);

internal sealed record ValidationCheck(
    string id,
    string kind,
    string owner,
    string reason,
    string environment,
    string[] prerequisites,
    string[] inputs,
    bool reusable,
    string? exclusion,
    string? context = null,
    string layer = "static",
    string[]? projects = null
);

internal sealed record ValidationResult(
    string id,
    string status,
    string inputIdentity,
    double seconds,
    string detail,
    Dictionary<string, string>? outputs = null,
    string dependencyIdentity = ""
);

internal sealed record ValidationEvidence(
    string commit,
    string tree,
    string environment,
    DateTimeOffset started,
    ValidationResult[] results,
    string planIdentity = ""
);
