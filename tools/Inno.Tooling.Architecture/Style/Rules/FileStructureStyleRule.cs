using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// F-03: no leading blank line; one blank line separates the using directives from the file-scoped
// namespace, and the namespace from the types.
internal sealed class FileStructureStyleRule : StyleRule
{
    private const string C_ID = "F-03";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleEdit edit in Fix(source))
        {
            findings.Add(
                source.Finding(C_ID, edit.start, "Use no leading blank line and exactly one blank line around the namespace declaration.")
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        int count = source.text.Lines.Count;
        int leading = 0;
        while (leading < count - 1 && StyleLines.IsBlank(source, leading))
        {
            edits.Add(StyleLines.Remove(source, leading));
            leading++;
        }

        if (source.root is not CompilationUnitSyntax unit) return edits;
        foreach (MemberDeclarationSyntax member in unit.Members)
        {
            if (member is not FileScopedNamespaceDeclarationSyntax scope || scope.SemicolonToken.IsMissing) continue;
            int first = source.LineNumber(scope.NamespaceKeyword.SpanStart);
            int last = source.LineNumber(scope.SemicolonToken.SpanStart);
            // A comment or an attribute directly above the namespace belongs to it.
            while (first > leading && !StyleLines.IsBlank(source, first - 1) && StyleLines.FirstToken(source, first - 1).Span.Length == 0)
            {
                first--;
            }

            if (first > leading) Separate(source, edits, first - 1, -1, leading);
            if (last < count - 1 && !StyleLayout.IsBlank(source.content, source.text.Lines[last].End, source.content.Length))
            {
                Separate(source, edits, last + 1, 1, count);
            }
        }

        return edits;
    }

    // Leaves exactly one blank line in the run that starts at 'line' and extends in 'direction'.
    private static void Separate(
        StyleSource source,
        List<StyleEdit> edits,
        int line,
        int direction,
        int bound
    ) {
        int blank = 0;
        for (int current = line; current != bound && StyleLines.IsBlank(source, current); current += direction)
        {
            if (++blank > 1) edits.Add(StyleLines.Remove(source, current));
        }

        if (blank == 0) edits.Add(StyleLines.InsertBefore(source, direction < 0 ? line + 1 : line));
    }
}
