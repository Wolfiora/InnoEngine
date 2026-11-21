using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// Line-level facts and edits shared by the blank-line rules.
internal static class StyleLines
{
    // True for an empty or whitespace-only line that lies between tokens, not inside a literal or a comment.
    internal static bool IsBlank(
        StyleSource source,
        int line
    ) {
        TextLine textLine = source.text.Lines[line];
        if (!StyleLayout.IsBlank(source.content, textLine.Start, textLine.End)) return false;
        if (textLine.Start >= source.content.Length) return true;
        SyntaxTrivia trivia = source.root.FindTrivia(textLine.Start);
        return trivia.IsKind(SyntaxKind.EndOfLineTrivia) || trivia.IsKind(SyntaxKind.WhitespaceTrivia);
    }

    internal static StyleEdit Remove(
        StyleSource source,
        int line
    ) {
        TextLine textLine = source.text.Lines[line];
        return new(textLine.Start, textLine.EndIncludingLineBreak, "");
    }

    internal static StyleEdit InsertBefore(
        StyleSource source,
        int line
    ) {
        int start = source.text.Lines[line].Start;
        return new(start, start, "\n");
    }

    // The token that ends the line, or default when the line ends with a comment or inside a literal.
    internal static SyntaxToken LastToken(
        StyleSource source,
        int line
    ) {
        TextLine textLine = source.text.Lines[line];
        int position = textLine.End - 1;
        while (position >= textLine.Start && source.content[position] is ' ' or '\t')
        {
            position--;
        }

        if (position < textLine.Start) return default;
        SyntaxToken token = source.root.FindToken(position);
        return token.Span.End - 1 == position ? token : default;
    }

    // The token that starts the line, or default when the line starts with a comment or inside a literal.
    internal static SyntaxToken FirstToken(
        StyleSource source,
        int line
    ) {
        TextLine textLine = source.text.Lines[line];
        int position = textLine.Start;
        while (position < textLine.End && source.content[position] is ' ' or '\t')
        {
            position++;
        }

        if (position >= textLine.End) return default;
        SyntaxToken token = source.root.FindToken(position);
        return token.SpanStart == position ? token : default;
    }
}
