namespace Inno.Entry.Bootstrap;

internal sealed record DevelopmentBuildWorkerRequest(
    string root,
    string configuration,
    string dotnet,
    string assembly,
    string operation,
    string nonce,
    string tool
);
