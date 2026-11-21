using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// M-03: '=> expression' stays on the declaration line when the member fits; otherwise '=>' starts the next
// line, indented four spaces. It never ends a line, and a brace-delimited body after a wrapped '=>' puts
// its braces at the column of the declaration.
internal sealed class ArrowMemberStyleRule : StyleRule
{
    private const string C_ID = "M-03";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is not ArrowExpressionClauseSyntax clause) continue;
            (string message, List<StyleEdit> edits)? violation = Violation(source, clause, protectedSpans);
            if (violation is { } found) findings.Add(source.Finding(C_ID, clause.ArrowToken.SpanStart, found.message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        var edits = new List<StyleEdit>();
        int fixedEnd = -1;
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            // A member nested in the body of another fixed member waits for the next round.
            if (node is not ArrowExpressionClauseSyntax clause || clause.SpanStart < fixedEnd) continue;
            if (Violation(source, clause, protectedSpans) is not { } found) continue;
            edits.AddRange(found.edits);
            fixedEnd = clause.Span.End;
        }

        return edits;
    }

    private static (string message, List<StyleEdit> edits)? Violation(
        StyleSource source,
        ArrowExpressionClauseSyntax clause,
        IReadOnlyList<TextSpan> protectedSpans
    ) {
        SyntaxToken arrow = clause.ArrowToken;
        SyntaxToken previous = arrow.GetPreviousToken();
        SyntaxToken first = clause.Expression.GetFirstToken();
        if (arrow.IsMissing || first.IsMissing) return null;
        string content = source.content;
        if (!StyleLayout.IsBlank(content, previous.Span.End, arrow.SpanStart)
            || !StyleLayout.IsBlank(content, arrow.Span.End, first.SpanStart)
        ) {
            return null;
        }
        int declaration = source.Indentation(previous.SpanStart);
        int arrowLine = source.LineNumber(arrow.SpanStart);
        int firstLine = source.LineNumber(first.SpanStart);
        int lastLine = source.LineNumber(clause.Expression.Span.End);
        bool afterParameters = StyleLayout.ClosesMultiLineHeader(source, previous);
        var edits = new List<StyleEdit>();
        if (arrowLine != firstLine)
        {
            // '=>' ends its line: it moves in front of the expression.
            bool wrapped = !afterParameters;
            if (wrapped)
            {
                TextLine signatureLine = source.Line(previous.Span.End);
                int joined = signatureLine.End - signatureLine.Start + " => ".Length + source.text.Lines[firstLine].End - first.SpanStart;
                wrapped = firstLine != lastLine || joined > StyleLayout.C_WIDTH;
            }

            edits.Add(new(previous.Span.End, arrow.SpanStart, wrapped ? "\n" + new string(' ', declaration + StyleLayout.C_INDENT) : " "));
            edits.Add(new(arrow.Span.End, first.SpanStart, " "));
            if (!wrapped)
            {
                AddShifts(source, edits, firstLine + 1, lastLine, declaration - source.Indentation(first.SpanStart), protectedSpans);
            }

            return ("Do not end a line with '=>'.", edits);
        }

        if (!source.IsFirstOnLine(arrow))
        {
            // M-03: a body that does not fit after a one-line signature starts on the next line with '=>'.
            if (afterParameters || firstLine == lastLine || clause.Parent is AccessorDeclarationSyntax) return null;
            // A multi-line string has no layout of its own to move.
            if (clause.Expression is LiteralExpressionSyntax or InterpolatedStringExpressionSyntax) return null;
            if (source.LineNumber(clause.Parent!.GetFirstToken().SpanStart) != arrowLine) return null;
            edits.Add(new(previous.Span.End, arrow.SpanStart, "\n" + new string(' ', declaration + StyleLayout.C_INDENT)));
            bool isDelimited = clause.Expression is SwitchExpressionSyntax or WithExpressionSyntax
                               or BaseObjectCreationExpressionSyntax { Initializer: not null };
            if (!isDelimited) AddShifts(source, edits, firstLine + 1, lastLine, StyleLayout.C_INDENT, protectedSpans);
            return ("Move '=>' to the next line: the body does not fit after the signature.", edits);
        }

        if (afterParameters) return null;
        // A wrapped '=>' whose member fits on one line returns to the declaration line.
        TextLine signature = source.text.Lines[source.LineNumber(previous.Span.End)];
        if (firstLine == lastLine
            && signature.End - signature.Start + 1 + source.text.Lines[arrowLine].End - arrow.SpanStart <= StyleLayout.C_WIDTH
            && source.LineNumber(clause.Parent!.GetFirstToken().SpanStart) == source.LineNumber(previous.Span.End)
        ) {
            edits.Add(new(previous.Span.End, arrow.SpanStart, " "));
            return ("Keep '=> expression' on the declaration line: the member fits on one line.", edits);
        }

        int delta = declaration + StyleLayout.C_INDENT - source.Column(arrow.SpanStart);
        if (delta != 0)
        {
            AddShifts(source, edits, arrowLine, lastLine, delta, protectedSpans);
            return ("Indent a wrapped '=>' four spaces from the declaration.", edits);
        }

        // Braces of a switch expression, an initializer or a 'with' expression sit at the declaration column.
        SyntaxToken brace = clause.Expression switch
        {
            SwitchExpressionSyntax expression => expression.OpenBraceToken,
            WithExpressionSyntax expression => expression.Initializer.OpenBraceToken,
            BaseObjectCreationExpressionSyntax { Initializer: not null } expression => expression.Initializer.OpenBraceToken,
            _ => default
        };
        if (brace.Span.Length == 0 || !source.IsFirstOnLine(brace) || source.Column(brace.SpanStart) == declaration)
        {
            return null;
        }
        AddShifts(
            source,
            edits,
            source.LineNumber(brace.SpanStart),
            lastLine,
            declaration - source.Column(brace.SpanStart),
            protectedSpans
        );
        return ("Put the braces of a wrapped expression body at the column of the declaration.", edits);
    }

    private static void AddShifts(
        StyleSource source,
        List<StyleEdit> edits,
        int firstLine,
        int lastLine,
        int delta,
        IReadOnlyList<TextSpan> protectedSpans
    ) {
        for (int line = firstLine; line <= lastLine && delta != 0; line++)
        {
            StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
            if (shift is not null) edits.Add(shift);
        }
    }
}
