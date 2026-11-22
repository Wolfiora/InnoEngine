using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationBinding(
    string repository,
    string revision,
    string sourceTree,
    string projectManifest,
    string projectManifestIdentity,
    string toolingRevision,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<DocumentationApiCapture>? apiManifests = null
);
