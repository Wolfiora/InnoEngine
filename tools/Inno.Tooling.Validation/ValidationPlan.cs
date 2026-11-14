using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Inno.Tooling.Validation;

internal sealed record ValidationPlanningResult(
    ValidationPlan? plan,
    ValidationPreparation? preparation
);

internal sealed record ValidationPreparation(
    string status,
    string kind,
    string root,
    string commit,
    string baseline,
    string inputIdentity,
    string reason,
    string[] owners,
    Dictionary<string, string> sources
);

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
    Dictionary<string, double>? planningSeconds = null,
    ValidationDocumentationBinding? documentation = null,
    Dictionary<string, string>? documentationSources = null
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
    string[]? projects = null,
    ValidationDocumentationCheck? documentation = null
);

internal sealed record ValidationDocumentationBinding(
    string cacheRoot,
    string snapshot,
    string sourceTree,
    string? baseline = null
);

internal sealed record ValidationDocumentationCheck(
    string[] references,
    string[] pages,
    string contentIdentity,
    string configuration,
    string? target
);

internal sealed record ValidationResult(
    string id,
    string status,
    string inputIdentity,
    double seconds,
    string detail,
    Dictionary<string, string>? outputs = null,
    string dependencyIdentity = "",
    ValidationDocumentationInput[]? documentationInputs = null
);

internal sealed record ValidationDocumentationInput(
    string project,
    string[] owners,
    string sourceIdentity,
    string environment,
    string configuration,
    string? target,
    Dictionary<string, string> packages,
    Dictionary<string, string> sources
);

internal sealed record ValidationEvidence(
    string commit,
    string tree,
    string environment,
    DateTimeOffset started,
    ValidationResult[] results,
    string planIdentity = "",
    bool completed = false
);
