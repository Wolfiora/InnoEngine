namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationPageReference(
    string pageId,
    string key,
    string payloadIdentity,
    DocumentationMetadata metadata
);
