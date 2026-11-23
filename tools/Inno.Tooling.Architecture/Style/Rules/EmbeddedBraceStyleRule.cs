using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// B-05 and B-06: the body of a control statement is enclosed in braces. An 'if' without 'else' whose single
// statement fits on the line of its header may keep the compact one-line form.
internal sealed class EmbeddedBraceStyleRule : StyleRule
{
    private const string C_ID = "B-05";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((SyntaxNode _, StatementSyntax body) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, body.SpanStart, "Enclose the body of the control statement in braces."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        (SyntaxNode owner, StatementSyntax body)[] violations = Violations(source).ToArray();
        List<StyleEdit> edits = [];
        foreach ((SyntaxNode owner, StatementSyntax body) in violations)
        {
            // Innermost bodies first: an outer body is braced once its content has settled.
            if (violations.Any(other => other.body != body && body.Span.Contains(other.body.Span))) continue;
            var indent = new string(' ', source.Indentation(owner.GetFirstToken().SpanStart));
            SyntaxToken previous = body.GetFirstToken().GetPreviousToken();
            SyntaxToken last = body.GetLastToken();
            SyntaxToken next = last.GetNextToken();
            if (source.LineNumber(previous.SpanStart) == source.LineNumber(body.SpanStart))
            {
                edits.Add(
                    new StyleEdit(previous.Span.End, body.SpanStart, "\n" + indent + "{\n" + indent + new string(' ', StyleLayout.C_INDENT))
                );
            }
            else
            {
                int end = source.text.Lines[source.LineNumber(previous.Span.End)].End;
                edits.Add(new StyleEdit(end, end, "\n" + indent + "{"));
            }

            if (!next.IsKind(SyntaxKind.None) && source.LineNumber(next.SpanStart) == source.LineNumber(last.Span.End))
            {
                edits.Add(new StyleEdit(last.Span.End, next.SpanStart, "\n" + indent + "}\n" + indent));
            }
            else
            {
                int end = source.text.Lines[source.LineNumber(last.Span.End)].End;
                edits.Add(new StyleEdit(end, end, "\n" + indent + "}"));
            }
        }

        return edits;
    }

    private static IEnumerable<(SyntaxNode owner, StatementSyntax body)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            StatementSyntax? body = node switch
            {
                IfStatementSyntax statement => statement.Statement,
                ElseClauseSyntax clause when clause.Statement is not IfStatementSyntax => clause.Statement,
                WhileStatementSyntax statement => statement.Statement,
                DoStatementSyntax statement => statement.Statement,
                ForStatementSyntax statement => statement.Statement,
                CommonForEachStatementSyntax statement => statement.Statement,
                LockStatementSyntax statement => statement.Statement,
                // Stacked headers share the braced body of the innermost one.
                UsingStatementSyntax statement when statement.Statement is not UsingStatementSyntax => statement.Statement,
                FixedStatementSyntax statement when statement.Statement is not FixedStatementSyntax => statement.Statement,
                _ => null
            };
            if (body is null or BlockSyntax || body.IsMissing) continue;
            if (node is IfStatementSyntax { Else: null } compact
                && compact.Parent is not ElseClauseSyntax
                && source.LineNumber(compact.SpanStart) == source.LineNumber(compact.Span.End)
                && source.text.Lines[source.LineNumber(compact.SpanStart)].Span.Length <= StyleLayout.C_WIDTH
            ) {
                continue;
            }

            yield return (node, body);
        }
    }
}
