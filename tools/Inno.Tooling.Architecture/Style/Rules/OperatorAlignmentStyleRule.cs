using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// F-05: a continuation line that begins with an operator aligns with the first operand of its expression.
// When alignment would pass the hard line limit, the lines are indented four spaces from the statement.
internal sealed class OperatorAlignmentStyleRule : StyleRule
{
    private const string C_ID = "F-05";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((int position, List<StyleEdit> _) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, position, "Align the operator continuation lines with the first operand."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        int fixedEnd = -1;
        foreach ((int position, List<StyleEdit> chainEdits) in Violations(source))
        {
            // A chain nested in the lines of a fixed chain waits for the next round.
            if (position < fixedEnd) continue;
            edits.AddRange(chainEdits);
            fixedEnd = chainEdits.Max(static edit => edit.end);
        }

        return edits;
    }

    private static IEnumerable<(int position, List<StyleEdit> edits)> Violations(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        foreach ((SyntaxNode chain, SyntaxToken anchor, IReadOnlyList<SyntaxToken> operators) in StyleOperators.Chains(source.root).OrderBy(
            static entry => entry.chain.SpanStart
        )) {
            int[] lines = operators
                .Where(token => source.IsFirstOnLine(token) && source.LineNumber(token.SpanStart) > source.LineNumber(anchor.SpanStart))
                .Select(token => source.LineNumber(token.SpanStart))
                .Distinct()
                .Order()
                .ToArray();
            if (lines.Length == 0) continue;
            int lastLine = source.LineNumber(chain.Span.End);
            int aligned = source.Column(anchor.SpanStart);
            int target = aligned;
            if (Overflows(source, lines, lastLine, aligned))
            {
                SyntaxNode? owner = chain.AncestorsAndSelf().FirstOrDefault(
                    static node => node is StatementSyntax or MemberDeclarationSyntax
                );
                if (owner is null) continue;
                target = source.Indentation(owner.GetFirstToken().SpanStart) + StyleLayout.C_INDENT;
            }

            List<StyleEdit> edits = [];
            for (int index = 0; index < lines.Length; index++)
            {
                int delta = target - source.Indentation(source.text.Lines[lines[index]].Start);
                int end = index + 1 < lines.Length ? lines[index + 1] - 1 : lastLine;
                for (int line = lines[index]; line <= end && delta != 0; line++)
                {
                    StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
                    if (shift is not null) edits.Add(shift);
                }
            }

            if (edits.Count > 0) yield return (source.text.Lines[lines[0]].Start, edits);
        }
    }

    private static bool Overflows(
        StyleSource source,
        int[] lines,
        int lastLine,
        int target
    ) {
        for (int index = 0; index < lines.Length; index++)
        {
            int delta = target - source.Indentation(source.text.Lines[lines[index]].Start);
            int end = index + 1 < lines.Length ? lines[index + 1] - 1 : lastLine;
            for (int line = lines[index]; line <= end; line++)
            {
                TextLine textLine = source.text.Lines[line];
                if (textLine.End - textLine.Start + delta > StyleLayout.C_HARD_WIDTH) return true;
            }
        }

        return false;
    }
}
