using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// D-05: a comment is on its own line above the code it explains, and '//' is followed by one space.
internal sealed class CommentStyleRule : StyleRule
{
    private const string C_ID = "D-05";

    private const string C_PREFIX = "//";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((SyntaxTrivia comment, bool isTrailing) in Violations(source))
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    comment.SpanStart,
                    isTrailing ? "Move the comment to its own line above the code." : "Write one space after '//'."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        var movedLines = new HashSet<int>();
        foreach ((SyntaxTrivia comment, bool isTrailing) in Violations(source))
        {
            if (!isTrailing)
            {
                edits.Add(new StyleEdit(comment.SpanStart + C_PREFIX.Length, comment.SpanStart + C_PREFIX.Length, " "));
                continue;
            }

            TextLine line = source.Line(comment.SpanStart);
            if (!movedLines.Add(line.LineNumber)) continue;
            string code = source.content[line.Start..comment.SpanStart];
            string indent = code[..(code.Length - code.TrimStart().Length)];
            edits.Add(new StyleEdit(line.Start, line.Start, indent + comment + "\n"));
            edits.Add(new StyleEdit(line.Start + code.TrimEnd().Length, comment.Span.End, string.Empty));
        }

        return edits;
    }

    private static IEnumerable<(SyntaxTrivia comment, bool isTrailing)> Violations(StyleSource source)
    {
        foreach (SyntaxTrivia trivia in source.root.DescendantTrivia())
        {
            if (!trivia.IsKind(SyntaxKind.SingleLineCommentTrivia)) continue;
            TextLine line = source.Line(trivia.SpanStart);
            if (source.content[line.Start..trivia.SpanStart].Trim().Length > 0)
            {
                yield return (trivia, true);
                continue;
            }

            string text = trivia.ToString();
            if (text.Length > C_PREFIX.Length && char.IsLetterOrDigit(text[C_PREFIX.Length]))
            {
                yield return (trivia, false);
            }
        }
    }
}
