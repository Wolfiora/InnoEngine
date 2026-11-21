using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// P-01: a parameter list with two or more parameters is fully expanded; one with fewer stays on one line.
internal sealed class ParameterListStyleRule : StyleRule
{
    private const string C_ID = "P-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((StyleList list, bool expand) in Violations(source))
        {
            string message = expand
                             ? "Expand the parameter list: one parameter per line and ')' on its own line."
                             : "Keep a parameter list with fewer than two parameters on one line.";
            findings.Add(source.Finding(C_ID, list.open.SpanStart, message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        var edits = new List<StyleEdit>();
        var spans = new List<TextSpan>();
        foreach ((StyleList list, bool expand) in Violations(source).OrderBy(static entry => entry.list.open.SpanStart))
        {
            // Nested lists are fixed in a later round, after their container has settled.
            TextSpan span = TextSpan.FromBounds(list.open.SpanStart, list.close.Span.End);
            if (spans.Any(outer => outer.Contains(span))) continue;
            if (expand)
            {
                List<StyleEdit>? listEdits = StyleLayout.Expand(source, list, protectedSpans);
                if (listEdits is null) continue;
                edits.AddRange(listEdits);
            }
            else
            {
                string? joined = StyleLayout.Join(source, list.open, list.close);
                if (joined is null) continue;
                edits.Add(new(span.Start, span.End, joined));
            }

            spans.Add(span);
        }

        return edits;
    }

    private static IEnumerable<(StyleList list, bool expand)> Violations(StyleSource source)
    {
        foreach (ParameterListSyntax parameters in source.root.DescendantNodes().OfType<ParameterListSyntax>())
        {
            if (parameters.OpenParenToken.IsMissing || parameters.CloseParenToken.IsMissing) continue;
            var list = new StyleList(
                parameters.OpenParenToken,
                parameters.CloseParenToken,
                parameters.Parameters.ToArray<SyntaxNode>()
            );
            bool multiLine = source.LineNumber(list.open.SpanStart) != source.LineNumber(list.close.SpanStart);
            if (list.items.Count >= 2)
            {
                if (!StyleLayout.IsExpanded(source, list)) yield return (list, true);
            }
            else if (multiLine)
            {
                // The hard line limit (F-04) outranks the one-line form of a single parameter.
                string? joined = StyleLayout.Join(source, list.open, list.close);
                int tail = source.Line(list.close.SpanStart).End - list.close.Span.End;
                if (joined is null || source.Column(list.open.SpanStart) + joined.Length + tail > StyleLayout.C_HARD_WIDTH)
                {
                    continue;
                }
                yield return (list, false);
            }
        }
    }
}
