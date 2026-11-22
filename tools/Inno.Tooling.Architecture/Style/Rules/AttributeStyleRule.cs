using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// M-05: one attribute per pair of brackets, on its own line above the declaration, without the 'Attribute'
// suffix and without empty parentheses. An attribute on a parameter stays on the line of the parameter.
internal sealed class AttributeStyleRule : StyleRule
{
    private const string C_ID = "M-05";

    private const string C_SUFFIX = "Attribute";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((int position, string message, StyleEdit _) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, position, message));
        }
    }

    // Edits of different kinds can touch the same list, so each round applies the first kind that is present.
    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Violations(source)
            .GroupBy(static item => item.message, StringComparer.Ordinal)
            .Select(static group => group.Select(static item => item.edit).ToArray())
            .FirstOrDefault() ?? [];

    private static IEnumerable<(int position, string message, StyleEdit edit)> Violations(StyleSource source)
    {
        foreach (AttributeListSyntax list in source.root.DescendantNodes().OfType<AttributeListSyntax>())
        {
            bool isInline = list.Parent is not (MemberDeclarationSyntax or CompilationUnitSyntax or LocalFunctionStatementSyntax);
            var indent = new string(' ', source.Indentation(list.SpanStart));
            if (list.Attributes.Count > 1)
            {
                string target = list.Target is null ? string.Empty : list.Target.ToString().TrimEnd() + " ";
                string separator = isInline ? " " : "\n" + indent;
                string text = string.Join(separator, list.Attributes.Select(attribute => "[" + target + attribute + "]"));
                yield return (list.SpanStart, "Write one attribute per pair of brackets.", new StyleEdit(
                    list.SpanStart,
                    list.Span.End,
                    text
                ));
            }

            foreach (AttributeSyntax attribute in list.Attributes)
            {
                SimpleNameSyntax name = attribute.Name switch
                {
                    QualifiedNameSyntax qualified => qualified.Right,
                    AliasQualifiedNameSyntax alias => alias.Name,
                    SimpleNameSyntax simple => simple,
                    _ => throw new InvalidOperationException($"The attribute name '{attribute.Name}' has an unknown form.")
                };
                string identifier = name.Identifier.Text;
                if (identifier.Length > C_SUFFIX.Length && identifier.EndsWith(C_SUFFIX, StringComparison.Ordinal))
                {
                    yield return (
                        attribute.SpanStart,
                        "Omit the 'Attribute' suffix.",
                        new StyleEdit(name.Identifier.Span.End - C_SUFFIX.Length, name.Identifier.Span.End, string.Empty)
                    );
                }

                if (attribute.ArgumentList is { Arguments.Count: 0 } arguments)
                {
                    yield return (attribute.SpanStart, "Omit the empty parentheses.", new StyleEdit(
                        arguments.SpanStart,
                        arguments.Span.End,
                        string.Empty
                    ));
                }
            }

            if (isInline) continue;
            SyntaxToken close = list.CloseBracketToken;
            SyntaxToken next = close.GetNextToken();
            if (next.Span.Length > 0 && source.LineNumber(next.SpanStart) == source.LineNumber(close.SpanStart))
            {
                yield return (
                    list.SpanStart,
                    "Write the attribute on its own line above the declaration.",
                    new StyleEdit(close.Span.End, next.SpanStart, "\n" + indent)
                );
            }
        }
    }
}
