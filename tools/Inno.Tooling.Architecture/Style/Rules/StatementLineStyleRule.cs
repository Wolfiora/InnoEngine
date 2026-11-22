using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// B-09: a line holds one statement, and the statement after a 'case' label starts on the next line.
internal sealed class StatementLineStyleRule : StyleRule
{
    private const string C_ID = "B-09";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((StatementSyntax statement, int _) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, statement.SpanStart, "Start the statement on its own line."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        int fixedLine = -1;
        foreach ((StatementSyntax statement, int indentation) in Violations(source))
        {
            // Statements that share lines with an already moved statement wait for the next round.
            int first = source.LineNumber(statement.SpanStart);
            if (first <= fixedLine) continue;
            int last = source.LineNumber(statement.Span.End);
            edits.Add(
                new StyleEdit(
                    statement.GetFirstToken().GetPreviousToken().Span.End,
                    statement.SpanStart,
                    "\n" + new string(' ', indentation)
                )
            );
            int delta = indentation - source.Indentation(statement.SpanStart);
            for (int line = first + 1; delta != 0 && line <= last; line++)
            {
                StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
                if (shift is not null) edits.Add(shift);
            }

            fixedLine = last;
        }

        return edits;
    }

    private static IEnumerable<(StatementSyntax statement, int indentation)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            SyntaxList<StatementSyntax> statements;
            SyntaxToken anchor;
            int first;
            switch (node)
            {
                // The first statement of a block is placed by the brace rules.
                case BlockSyntax block:
                    (statements, anchor, first) = (block.Statements, block.OpenBraceToken, 1);
                    break;
                case SwitchSectionSyntax section:
                    (statements, anchor, first) = (section.Statements, section.Labels[0].GetFirstToken(), 0);
                    break;
                default:
                    continue;
            }

            if (anchor.IsMissing || StyleLayout.IsInInterpolation(node) || IsGrid(source, statements)) continue;
            int indentation = source.Indentation(anchor.SpanStart) + StyleLayout.C_INDENT;
            for (int index = first; index < statements.Count; index++)
            {
                StatementSyntax statement = statements[index];
                SyntaxToken token = statement.GetFirstToken();
                if (source.IsFirstOnLine(token)) continue;
                if (!StyleLayout.IsBlank(source.content, token.GetPreviousToken().Span.End, token.SpanStart)) continue;
                yield return (statement, indentation);
            }
        }
    }

    // Rows of assignments with the same number of columns write a numeric matrix, which keeps its rows.
    private static bool IsGrid(
        StyleSource source,
        SyntaxList<StatementSyntax> statements
    ) {
        if (statements.Count == 0 || statements.Any(static statement => !IsAssignment(statement))) return false;
        int[] rows = statements
            .GroupBy(statement => source.LineNumber(statement.SpanStart))
            .Select(static row => row.Count())
            .ToArray();
        return rows.Length > 1 && rows[0] > 1 && rows.All(row => row == rows[0]);
    }

    private static bool IsAssignment(StatementSyntax statement)
        => statement is ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax assignment }
           && assignment.IsKind(SyntaxKind.SimpleAssignmentExpression);
}
