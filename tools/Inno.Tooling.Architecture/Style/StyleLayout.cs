using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// Text operations shared by the layout rules.
internal static class StyleLayout
{
    internal const int C_INDENT = 4;
    internal const int C_WIDTH = 120;
    internal const int C_HARD_WIDTH = 140;

    // An expression in a hole of an interpolated string is never broken: the string stays one unit.
    internal static bool IsInInterpolation(SyntaxNode node)
        => node.Ancestors().Any(static ancestor => ancestor is InterpolationSyntax);

    internal static bool IsBlank(
        string content,
        int start,
        int end
    ) {
        for (int index = start; index < end; index++)
        {
            if (!char.IsWhiteSpace(content[index])) return false;
        }

        return true;
    }

    // Moves the indentation of one line by a number of columns, unless the line starts inside protected text.
    internal static StyleEdit? Shift(
        StyleSource source,
        int line,
        int delta,
        IReadOnlyList<TextSpan> protectedSpans
    ) {
        TextLine textLine = source.text.Lines[line];
        int start = textLine.Start;
        foreach (TextSpan span in protectedSpans)
        {
            if (start > span.Start && start < span.End) return null;
        }

        int end = start;
        while (end < textLine.End && source.content[end] == ' ')
        {
            end++;
        }

        if (end == textLine.End) return null;
        int width = end - start + delta;
        return new(start, end, new string(' ', width < 0 ? 0 : width));
    }

    // Whether the list has the expanded form: the opener ends its line, each item is on its own line one level
    // deeper, and the closer starts its own line at the indentation of the line that opens the list.
    internal static bool IsExpanded(
        StyleSource source,
        StyleList list
    ) {
        int indentation = source.Indentation(list.open.SpanStart);
        if (!source.IsLastOnLine(list.open)) return false;
        foreach (SyntaxNode argument in list.items)
        {
            SyntaxToken first = argument.GetFirstToken();
            if (!source.IsFirstOnLine(first) || source.Column(first.SpanStart) != indentation + C_INDENT) return false;
        }

        return source.IsFirstOnLine(list.close) && source.Column(list.close.SpanStart) == indentation;
    }

    // The edits that give the list its expanded form, or null when something other than whitespace separates its items.
    internal static List<StyleEdit>? Expand(
        StyleSource source,
        StyleList list,
        IReadOnlyList<TextSpan> protectedSpans
    ) {
        string content = source.content;
        int indentation = source.Indentation(list.open.SpanStart);
        var argumentIndent = new string(' ', indentation + C_INDENT);
        var edits = new List<StyleEdit>();
        int previousEnd = list.open.Span.End;
        foreach (SyntaxNode argument in list.items)
        {
            SyntaxToken first = argument.GetFirstToken();
            if (!IsBlank(content, previousEnd, first.SpanStart)) return null;
            int startLine = source.LineNumber(first.SpanStart);
            int delta = indentation + C_INDENT - source.Indentation(first.SpanStart);
            edits.Add(new(previousEnd, first.SpanStart, "\n" + argumentIndent));
            int endLine = source.LineNumber(argument.Span.End);
            for (int line = startLine + 1; line <= endLine && delta != 0; line++)
            {
                StyleEdit? shift = Shift(source, line, delta, protectedSpans);
                if (shift is not null) edits.Add(shift);
            }

            // The separator stays on the argument's last line.
            SyntaxToken next = argument.GetLastToken().GetNextToken();
            previousEnd = next.IsKind(SyntaxKind.CommaToken) ? next.Span.End : argument.Span.End;
        }

        if (!IsBlank(content, previousEnd, list.close.SpanStart)) return null;
        edits.Add(new(previousEnd, list.close.SpanStart, "\n" + new string(' ', indentation)));
        return edits;
    }

    // The parentheses of the header of a control statement, when it has one.
    internal static (SyntaxToken open, SyntaxToken close)? Header(SyntaxNode statement)
        => statement switch
    {
        IfStatementSyntax node => (node.OpenParenToken, node.CloseParenToken),
        WhileStatementSyntax node => (node.OpenParenToken, node.CloseParenToken),
        ForStatementSyntax node => (node.OpenParenToken, node.CloseParenToken),
        CommonForEachStatementSyntax node => (node.OpenParenToken, node.CloseParenToken),
        SwitchStatementSyntax node when !node.OpenParenToken.IsKind(SyntaxKind.None) => (node.OpenParenToken, node.CloseParenToken),
        LockStatementSyntax node => (node.OpenParenToken, node.CloseParenToken),
        UsingStatementSyntax node => (node.OpenParenToken, node.CloseParenToken),
        FixedStatementSyntax node => (node.OpenParenToken, node.CloseParenToken),
        _ => null
    };

    // Whether the parenthesis closes a parameter list or a control statement header that spans several lines.
    internal static bool ClosesMultiLineHeader(
        StyleSource source,
        SyntaxToken close
    ) {
        if (!close.IsKind(SyntaxKind.CloseParenToken) || close.Parent is null) return false;
        SyntaxToken open;
        if (close.Parent is ParameterListSyntax parameters)
        {
            open = parameters.OpenParenToken;
        }
        else if (Header(close.Parent) is { } header && header.close == close)
        {
            open = header.open;
        }
        else
        {
            return false;
        }

        return source.LineNumber(open.SpanStart) != source.LineNumber(close.SpanStart);
    }

    // The text between two tokens joined onto one line, or null when a comment, a directive, a multi-line
    // token or a construct that owns its own multi-line layout lies between them.
    internal static string? Join(
        StyleSource source,
        SyntaxToken open,
        SyntaxToken close
    ) {
        SyntaxNode parent = open.Parent!;
        foreach (SyntaxNode node in parent.DescendantNodes())
        {
            // A nested list of two or more parameters is always expanded (P-01), so its owner cannot be joined.
            if (node is ParameterListSyntax { Parameters.Count: >= 2 }) return null;
            if (!OwnsLayout(node)) continue;
            if (source.LineNumber(node.SpanStart) != source.LineNumber(node.Span.End)) return null;
        }

        var builder = new StringBuilder();
        SyntaxToken token = open;
        while (true)
        {
            if (source.LineNumber(token.SpanStart) != source.LineNumber(token.Span.End)) return null;
            builder.Append(token.Text);
            if (token == close) break;
            SyntaxToken next = token.GetNextToken();
            string gap = source.content[token.Span.End..next.SpanStart];
            if (!IsBlank(gap, 0, gap.Length)) return null;
            if (gap.Contains('\n'))
            {
                gap = Glues(token, next) ? string.Empty : " ";
            }

            builder.Append(gap);
            token = next;
        }

        return builder.ToString();
    }

    private static bool OwnsLayout(SyntaxNode node)
        => node is BlockSyntax
           or SwitchExpressionSyntax
           or InitializerExpressionSyntax
           or CollectionExpressionSyntax
           or AnonymousObjectCreationExpressionSyntax
           or QueryExpressionSyntax
           or InterpolatedStringExpressionSyntax
           or WithExpressionSyntax
           or PropertyPatternClauseSyntax;

    private static bool Glues(
        SyntaxToken left,
        SyntaxToken right
    ) => left.IsKind(SyntaxKind.OpenParenToken)
         || left.IsKind(SyntaxKind.OpenBracketToken)
         || left.IsKind(SyntaxKind.DotToken)
         || right.IsKind(SyntaxKind.CloseParenToken)
         || right.IsKind(SyntaxKind.CloseBracketToken)
         || right.IsKind(SyntaxKind.CommaToken)
         || right.IsKind(SyntaxKind.SemicolonToken)
         || right.IsKind(SyntaxKind.DotToken)
         || right.Parent is ConditionalAccessExpressionSyntax && right.IsKind(SyntaxKind.QuestionToken);
}
