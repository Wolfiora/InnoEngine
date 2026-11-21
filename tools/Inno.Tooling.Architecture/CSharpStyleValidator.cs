using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Inno.Tooling.Architecture;

internal static class CSharpStyleValidator
{
    internal static void Validate(
        string relativePath,
        string content,
        SyntaxTree tree,
        ICollection<string> failures
    ) {
        EditorPresentationSourceValidator.Validate(relativePath, tree, failures);
        ValidateStyle(relativePath, content, failures);
    }

    // Reports each finding of the style rulebook as '<path>:<line>: <rule>: <message>'.
    internal static void ValidateStyle(
        string relativePath,
        string content,
        ICollection<string> failures
    ) {
        foreach (StyleFinding finding in StyleEngine.Check(relativePath, content))
        {
            failures.Add($"{finding.path}:{finding.line}: {finding.rule}: {finding.message}");
        }
    }
}
