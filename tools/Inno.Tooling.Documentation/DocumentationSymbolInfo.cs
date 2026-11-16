using System.Collections.Generic;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Describes an authored symbol's canonical anchor, declared signature identity and visible code.
/// These facts support source comparisons; they do not certify behavioral prose or examples.
/// </summary>
/// <param name="anchor">
/// Explicit API anchor derived from the exact XML documentation identity.
/// </param>
/// <param name="signatureIdentity">
/// Authored SHA-256 signature mapping to compare with the selected source manifest.
/// </param>
/// <param name="code">
/// Original C# code fences within the symbol's section, or inline code in its legacy declaration row.
/// An empty list provides no visible-declaration evidence.
/// </param>
public sealed record DocumentationSymbolInfo(
    string anchor,
    string signatureIdentity,
    IReadOnlyList<string> code
);
