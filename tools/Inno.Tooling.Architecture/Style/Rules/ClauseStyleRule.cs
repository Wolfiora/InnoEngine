using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// B-03 and T-06: after a single-line signature a constructor initializer or a 'where' clause starts the
// next line, indented four spaces. Further 'where' clauses each take a line aligned with the first, and
// the base list of a type stays on the line of the type name.
internal sealed class ClauseStyleRule : StyleRule
{
    private const string C_ID = "B-03";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((StyleEdit edit, string message) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, edit.end, message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Violations(source).Select(static violation => violation.edit).ToArray();

    private static IEnumerable<(StyleEdit edit, string message)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            SyntaxList<TypeParameterConstraintClauseSyntax> constraints = node switch
            {
                MethodDeclarationSyntax method => method.ConstraintClauses,
                LocalFunctionStatementSyntax local => local.ConstraintClauses,
                DelegateDeclarationSyntax callback => callback.ConstraintClauses,
                TypeDeclarationSyntax type => type.ConstraintClauses,
                _ => default
            };
            SyntaxToken first = node is ConstructorDeclarationSyntax { Initializer: { } initializer }
                                ? initializer.ColonToken
                                : constraints.Count > 0 ? constraints[0].WhereKeyword : default;
            if (first.Span.Length == 0 || first.IsMissing) continue;
            int declaration = source.Indentation(node.GetFirstToken().SpanStart);
            SyntaxToken previous = first.GetPreviousToken();
            string clause = first.IsKind(SyntaxKind.ColonToken) ? "constructor initializer" : "'where' clause";
            bool follows = previous.IsKind(SyntaxKind.CloseParenToken) && StyleLayout.ClosesMultiLineHeader(source, previous);
            if (StyleLayout.IsBlank(source.content, previous.Span.End, first.SpanStart))
            {
                // After a multi-line parameter list the clause follows ')' (P-02); otherwise it starts a line.
                string expected = "\n" + new string(' ', declaration + StyleLayout.C_INDENT);
                if (!follows && source.content[previous.Span.End..first.SpanStart] != expected)
                {
                    yield return (new(
                        previous.Span.End,
                        first.SpanStart,
                        expected
                    ), $"Start the {clause} on its own line, indented four spaces.");
                    continue;
                }
            }

            for (int index = 1; index < constraints.Count; index++)
            {
                SyntaxToken keyword = constraints[index].WhereKeyword;
                SyntaxToken before = keyword.GetPreviousToken();
                if (!StyleLayout.IsBlank(source.content, before.Span.End, keyword.SpanStart)) continue;
                string expected = "\n" + new string(' ', source.Column(first.SpanStart));
                if (source.content[before.Span.End..keyword.SpanStart] == expected) continue;
                yield return (new(
                    before.Span.End,
                    keyword.SpanStart,
                    expected
                ), "Put each 'where' clause on its own line, aligned with the first.");
            }
        }

        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is not BaseTypeDeclarationSyntax { BaseList: { } bases }) continue;
            SyntaxToken previous = bases.ColonToken.GetPreviousToken();
            if (source.LineNumber(previous.Span.End) == source.LineNumber(bases.ColonToken.SpanStart)) continue;
            if (!StyleLayout.IsBlank(source.content, previous.Span.End, bases.ColonToken.SpanStart)) continue;
            if (StyleLayout.ClosesMultiLineHeader(source, previous)) continue;
            int joined = source.Column(previous.Span.End) + 1 + source.Line(bases.ColonToken.SpanStart).End - bases.ColonToken.SpanStart;
            if (joined > StyleLayout.C_HARD_WIDTH) continue;
            yield return (new(previous.Span.End, bases.ColonToken.SpanStart, " "), "Keep the base list on the line of the type name.");
        }
    }
}
