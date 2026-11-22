using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// A-01: an argument list stays on one line whenever the line that results fits within the line width.
internal sealed class ArgumentJoinStyleRule : StyleRule
{
    private const string C_ID = "A-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((StyleList list, string _) in Joinable(source))
        {
            findings.Add(source.Finding(C_ID, list.open.SpanStart, "Keep the argument list on one line: it fits within the line width."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        List<TextSpan> spans = [];
        // Innermost lists first: an outer list is judged again once its content has settled.
        foreach ((StyleList list, string joined) in Joinable(source).OrderByDescending(static entry => entry.list.open.SpanStart))
        {
            TextSpan span = TextSpan.FromBounds(list.open.SpanStart, list.close.Span.End);
            if (spans.Any(inner => span.Contains(inner))) continue;
            edits.Add(new StyleEdit(span.Start, span.End, joined));
            spans.Add(span);
        }

        return edits;
    }

    private static IEnumerable<(StyleList list, string joined)> Joinable(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            StyleList? list = StyleList.Arguments(node);
            if (list is null) continue;
            int openLine = source.LineNumber(list.open.SpanStart);
            int closeLine = source.LineNumber(list.close.SpanStart);
            if (openLine == closeLine) continue;
            string? joined = StyleLayout.Join(source, list.open, list.close);
            if (joined is null) continue;
            int tail = source.text.Lines[closeLine].End - list.close.Span.End;
            if (source.Column(list.open.SpanStart) + joined.Length + tail > StyleLayout.C_WIDTH) continue;
            yield return (list, joined);
        }
    }
}
