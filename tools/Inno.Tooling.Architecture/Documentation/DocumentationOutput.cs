using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Inno.Tooling.Architecture.Documentation;

internal static class DocumentationOutput
{
    internal static readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    internal static void Write(
        DocumentationResult result,
        bool json
    ) {
        if (json)
        {
            Console.WriteLine(JsonSerializer.Serialize(result, options));
            return;
        }
        Console.WriteLine($"{result.status}; snapshot: {result.snapshot}; source API: {result.sourceApiStatus}");
        Console.WriteLine(result.message);
        foreach (DocumentationHit hit in result.hits)
        {
            Console.WriteLine($"\n{hit.page}#{hit.section} (line {hit.line}, owner: {hit.project ?? "guide"})");
            Console.WriteLine(hit.text);
        }
        foreach (DocumentationContinuation next in result.continuations)
            Console.WriteLine($"Continue: {next.page}#{next.section} --offset {next.offset} — {next.title}");
    }
}

internal sealed record DocumentationResult(
    string status,
    string snapshot,
    string sourceApiStatus,
    string message,
    IReadOnlyList<DocumentationHit> hits,
    IReadOnlyList<DocumentationContinuation> continuations
);

internal sealed record DocumentationHit(
    string page,
    string section,
    int line,
    string? project,
    string text
);

internal sealed record DocumentationContinuation(
    string page,
    string section,
    string title,
    int offset = 0
);
