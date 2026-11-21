using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// B-09 and M-01: a block that holds statements is never squeezed onto one line, and a property whose
// accessors have bodies is expanded with one accessor per line.
internal sealed class SingleLineBlockStyleRule : StyleRule
{
    private const string C_ID = "B-09";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((SyntaxToken open, List<StyleEdit> _) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, open.SpanStart, "Expand the braces: a body with content is not written on one line."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        int fixedEnd = -1;
        foreach ((SyntaxToken open, List<StyleEdit> blockEdits) in Violations(source))
        {
            // A block nested in an expanded block waits for the next round.
            if (open.SpanStart < fixedEnd) continue;
            edits.AddRange(blockEdits);
            fixedEnd = blockEdits.Max(static edit => edit.end);
        }

        return edits;
    }

    private static IEnumerable<(SyntaxToken open, List<StyleEdit> edits)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            SyntaxToken open;
            SyntaxToken close;
            IReadOnlyList<SyntaxNode> items;
            switch (node)
            {
                case BlockSyntax { Statements.Count: > 0 } block:
                    (open, close, items) = (block.OpenBraceToken, block.CloseBraceToken, block.Statements);
                    break;
                case AccessorListSyntax accessors when accessors.Accessors.Any(
                    static accessor => accessor.Body is not null || accessor.ExpressionBody is not null
                ):
                    (open, close, items) = (accessors.OpenBraceToken, accessors.CloseBraceToken, accessors.Accessors);
                    break;
                default:
                    continue;
            }

            if (open.IsMissing || close.IsMissing || source.LineNumber(open.SpanStart) != source.LineNumber(close.SpanStart))
            {
                continue;
            }
            SyntaxToken previous = open.GetPreviousToken();
            int indentation = source.Indentation(open.SpanStart);
            string outer = "\n" + new string(' ', indentation);
            string inner = "\n" + new string(' ', indentation + StyleLayout.C_INDENT);
            var edits = new List<StyleEdit>();
            if (!source.IsFirstOnLine(open) && !StyleLayout.ClosesMultiLineHeader(source, previous))
            {
                if (!StyleLayout.IsBlank(source.content, previous.Span.End, open.SpanStart)) continue;
                edits.Add(new(previous.Span.End, open.SpanStart, outer));
            }

            int position = open.Span.End;
            bool isPlain = true;
            foreach (SyntaxNode item in items)
            {
                isPlain &= StyleLayout.IsBlank(source.content, position, item.SpanStart);
                edits.Add(new(position, item.SpanStart, inner));
                position = item.Span.End;
            }

            if (!isPlain || !StyleLayout.IsBlank(source.content, position, close.SpanStart)) continue;
            edits.Add(new(position, close.SpanStart, outer));
            yield return (open, edits);
        }
    }
}
