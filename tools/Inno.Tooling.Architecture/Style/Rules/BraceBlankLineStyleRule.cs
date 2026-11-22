using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

// S-04: no blank line follows an opening brace or precedes a closing brace.
internal sealed class BraceBlankLineStyleRule : StyleRule
{
    private const string C_ID = "S-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleEdit edit in Fix(source))
        {
            findings.Add(source.Finding(C_ID, edit.start, "Remove the blank line next to the brace."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        int count = source.text.Lines.Count;
        for (int line = 1; line < count - 1; line++)
        {
            if (!StyleLines.IsBlank(source, line)) continue;
            bool afterOpen = StyleLines.LastToken(source, line - 1).IsKind(SyntaxKind.OpenBraceToken);
            bool beforeClose = StyleLines.FirstToken(source, line + 1).IsKind(SyntaxKind.CloseBraceToken);
            if (afterOpen || beforeClose) edits.Add(StyleLines.Remove(source, line));
        }

        return edits;
    }
}
