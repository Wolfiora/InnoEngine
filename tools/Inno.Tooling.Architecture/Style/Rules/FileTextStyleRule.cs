using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// F-01: LF line endings, four-space indentation without tabs, no trailing whitespace,
// exactly one final newline and at most one consecutive blank line.
internal sealed class FileTextStyleRule : StyleRule
{
    private const string C_ID = "F-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (Violation violation in Scan(source))
        {
            findings.Add(source.Finding(C_ID, violation.start, violation.message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Scan(source)
            .Where(static violation => violation.replacement is not null)
            .Select(static violation => new StyleEdit(violation.start, violation.end, violation.replacement!))
            .ToArray();

    private static List<Violation> Scan(StyleSource source)
    {
        string content = source.content;
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        List<Violation> violations = [];
        if (content.Length > 0 && content[0] == '\uFEFF')
        {
            violations.Add(new Violation(0, 1, "Remove the byte order mark.", string.Empty));
        }

        int blankRun = 0;
        int position = content.Length > 0 && content[0] == '\uFEFF' ? 1 : 0;
        while (position < content.Length)
        {
            int lineStart = position;
            int lineEnd = content.IndexOf('\n', lineStart);
            int next = lineEnd < 0 ? content.Length : lineEnd + 1;
            if (lineEnd < 0) lineEnd = content.Length;
            int textEnd = lineEnd;
            if (textEnd > lineStart && content[textEnd - 1] == '\r')
            {
                textEnd--;
                AddUnlessProtected(textEnd, textEnd + 1, "Use LF line endings.", string.Empty);
            }

            int indentEnd = lineStart;
            while (indentEnd < textEnd && content[indentEnd] is ' ' or '\t')
            {
                indentEnd++;
            }

            bool isBlank = indentEnd == textEnd;
            if (isBlank)
            {
                blankRun++;
                // The whole surplus line is removed, including its line break.
                if (blankRun > 1 && next > lineStart)
                {
                    AddUnlessProtected(lineStart, next, "Keep at most one consecutive blank line.", string.Empty);
                }
                else if (textEnd > lineStart)
                {
                    AddUnlessProtected(lineStart, textEnd, "Remove trailing whitespace.", string.Empty);
                }
            }
            else
            {
                blankRun = 0;
                if (content.IndexOf('\t', lineStart, indentEnd - lineStart) >= 0)
                {
                    string indentation = content[lineStart..indentEnd].Replace("\t", "    ");
                    AddUnlessProtected(lineStart, indentEnd, "Indent with four spaces, not tabs.", indentation);
                }

                int trimmedEnd = textEnd;
                while (trimmedEnd > indentEnd && content[trimmedEnd - 1] is ' ' or '\t')
                {
                    trimmedEnd--;
                }

                if (trimmedEnd < textEnd)
                {
                    AddUnlessProtected(trimmedEnd, textEnd, "Remove trailing whitespace.", string.Empty);
                }
            }

            position = next;
        }

        AddFinalNewline();
        return violations;

        void AddUnlessProtected(
            int start,
            int end,
            string message,
            string replacement
        ) {
            foreach (TextSpan span in protectedSpans)
            {
                if (start < span.End && end > span.Start) return;
            }

            // A removed surplus blank line may contain trailing whitespace that was already reported.
            if (violations.Count > 0 && violations[^1].end > start) return;
            violations.Add(new Violation(start, end, message, replacement));
        }

        void AddFinalNewline()
        {
            if (content.Length == 0) return;
            int end = content.Length;
            int trimmed = end;
            while (trimmed > 0 && content[trimmed - 1] is '\n' or '\r' or ' ' or '\t')
            {
                trimmed--;
            }

            if (content[trimmed..end] == "\n") return;
            // Earlier edits may already cover part of the tail; replace the tail as one edit.
            violations.RemoveAll(violation => violation.start >= trimmed);
            violations.Add(new Violation(trimmed, end, "End the file with exactly one newline.", "\n"));
        }
    }

    private sealed record Violation(
        int start,
        int end,
        string message,
        string? replacement
    );
}
