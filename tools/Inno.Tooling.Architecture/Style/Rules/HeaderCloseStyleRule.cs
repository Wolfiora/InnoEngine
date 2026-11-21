using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// W-05: the closing parenthesis of a control statement header that spans several lines starts its own line,
// aligned with the keyword, and the opening brace follows it: ') {'.
internal sealed class HeaderCloseStyleRule : StyleRule
{
    private const string C_ID = "W-05";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleEdit edit in Fix(source))
        {
            findings.Add(
                source.Finding(C_ID, edit.end, "Close a multi-line statement header with ') {' on its own line, aligned with the keyword.")
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (StyleLayout.Header(node) is not { } header || header.open.IsMissing || header.close.IsMissing) continue;
            if (source.LineNumber(header.open.SpanStart) == source.LineNumber(header.close.SpanStart)) continue;
            SyntaxToken brace = header.close.GetNextToken();
            // A body without braces is given its braces first (B-05).
            if (!brace.IsKind(SyntaxKind.OpenBraceToken)) continue;
            int indentation = source.Indentation(node.GetFirstToken().SpanStart);
            if (!ClosesLine(source, header.close, indentation))
            {
                int start = header.close.SpanStart;
                while (start > 0 && source.content[start - 1] == ' ')
                {
                    start--;
                }

                edits.Add(new(start, header.close.SpanStart, "\n" + new string(' ', indentation)));
            }

            string gap = source.content[header.close.Span.End..brace.SpanStart];
            if (gap != " " && StyleLayout.IsBlank(gap, 0, gap.Length))
            {
                edits.Add(new(header.close.Span.End, brace.SpanStart, " "));
            }
        }

        return edits;
    }

    // Whether only closing delimiters precede the parenthesis on a line that is aligned with the keyword.
    private static bool ClosesLine(
        StyleSource source,
        SyntaxToken close,
        int indentation
    ) {
        if (source.Indentation(close.SpanStart) != indentation) return false;
        int lineStart = source.text.Lines[source.LineNumber(close.SpanStart)].Start;
        for (int index = lineStart + indentation; index < close.SpanStart; index++)
        {
            if (source.content[index] is not (')' or ']' or '}')) return false;
        }

        return true;
    }
}
