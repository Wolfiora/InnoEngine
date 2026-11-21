using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

internal sealed class StyleSource
{
    private static readonly CSharpParseOptions PARSE_OPTIONS = new(LanguageVersion.CSharp14);

    internal string path { get; }

    internal string content { get; }

    internal SyntaxTree tree { get; }

    internal SyntaxNode root { get; }

    internal SourceText text { get; }

    internal StyleSource(
        string path,
        string content
    ) {
        this.path = path;
        this.content = content;
        this.tree = CSharpSyntaxTree.ParseText(content, PARSE_OPTIONS, path: path);
        this.root = this.tree.GetRoot();
        this.text = this.tree.GetText();
    }

    internal int LineNumber(int position) => text.Lines.GetLineFromPosition(position).LineNumber;

    internal TextLine Line(int position) => text.Lines.GetLineFromPosition(position);

    // Counts the columns of leading whitespace on the line that contains the position.
    internal int Indentation(int position)
    {
        TextLine line = Line(position);
        int column = 0;
        while (line.Start + column < line.End && content[line.Start + column] == ' ')
        {
            column++;
        }

        return column;
    }

    internal int Column(int position) => position - Line(position).Start;

    internal bool IsFirstOnLine(SyntaxToken token)
    {
        TextLine line = Line(token.SpanStart);
        for (int index = line.Start; index < token.SpanStart; index++)
        {
            if (!char.IsWhiteSpace(content[index])) return false;
        }

        return true;
    }

    internal bool IsLastOnLine(SyntaxToken token)
    {
        TextLine line = Line(token.Span.End);
        for (int index = token.Span.End; index < line.End; index++)
        {
            if (!char.IsWhiteSpace(content[index])) return false;
        }

        return true;
    }

    // Ranges whose characters belong to program text or disabled code and must never be reformatted:
    // tokens that span several lines (verbatim and raw strings) and text excluded by the preprocessor.
    internal IReadOnlyList<TextSpan> ProtectedSpans()
    {
        var spans = new List<TextSpan>();
        foreach (SyntaxToken token in root.DescendantTokens(descendIntoTrivia: true))
        {
            if (token.Span.Length > 0 && LineNumber(token.SpanStart) != LineNumber(token.Span.End))
            {
                spans.Add(token.Span);
            }
        }

        // The text and closing delimiter of a multi-line interpolated string depend on their own indentation.
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            if (node is InterpolatedStringExpressionSyntax && LineNumber(node.SpanStart) != LineNumber(node.Span.End))
            {
                spans.Add(node.Span);
            }
        }

        foreach (SyntaxTrivia trivia in root.DescendantTrivia(descendIntoTrivia: true))
        {
            if (trivia.IsKind(SyntaxKind.DisabledTextTrivia))
            {
                spans.Add(trivia.Span);
            }
        }

        return spans;
    }

    internal StyleFinding Finding(
        string rule,
        int position,
        string message
    ) => new(rule, path, LineNumber(position) + 1, message);

    // The token texts in source order. Layout rules must leave this sequence unchanged.
    internal IReadOnlyList<string> TokenTexts()
        => root.DescendantTokens(descendIntoTrivia: false).Select(static token => token.Text).ToArray();

    internal bool hasSyntaxErrors
        => tree.GetDiagnostics().Any(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

    internal static string Apply(
        string content,
        IReadOnlyList<StyleEdit> edits
    ) {
        if (edits.Count == 0) return content;
        StyleEdit[] ordered = edits.OrderBy(static edit => edit.start).ThenBy(static edit => edit.end).ToArray();
        var builder = new StringBuilder(content.Length + 64);
        int position = 0;
        foreach (StyleEdit edit in ordered)
        {
            if (edit.start < position)
            {
                throw new InvalidOperationException("Style edits overlap.");
            }

            builder.Append(content, position, edit.start - position);
            builder.Append(edit.text);
            position = edit.end;
        }

        builder.Append(content, position, content.Length - position);
        return builder.ToString();
    }
}
