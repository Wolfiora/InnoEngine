using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// E-12: a resource that lives until the end of the enclosing block is a `using` declaration.
// A `using` statement that closes its block releases the resource at the same point, so it is rewritten.
internal sealed class UsingDeclarationStyleRule : StyleRule
{
    private const string C_ID = "E-12";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (UsingStatementSyntax statement in Violations(source.root))
        {
            findings.Add(
                source.Finding(C_ID, statement.SpanStart, "Write a `using` declaration: the resource lives until the block ends.")
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        List<StyleEdit> edits = [];
        int fixedEnd = -1;
        foreach (UsingStatementSyntax statement in Violations(source.root))
        {
            // A statement nested in one that this round rewrites waits for the next round.
            if (statement.SpanStart < fixedEnd) continue;
            List<StyleEdit>? rewrite = Rewrite(source, statement, protectedSpans);
            if (rewrite is null) continue;
            edits.AddRange(rewrite);
            fixedEnd = statement.Span.End;
        }

        return edits;
    }

    private static IEnumerable<UsingStatementSyntax> Violations(SyntaxNode root)
    {
        foreach (UsingStatementSyntax statement in root.DescendantNodes().OfType<UsingStatementSyntax>())
        {
            // An expression without a variable has no declaration form.
            if (statement.Declaration is null) continue;
            if (statement.Parent is not BlockSyntax block || block.Statements.Last() != statement) continue;

            // A declaration cannot follow a label, and a jump into its scope is not allowed.
            if (block.Statements.Any(static sibling => sibling is LabeledStatementSyntax)) continue;

            // A name declared in the statement and again in a sibling scope would collide in the shared block.
            HashSet<string> names = [.. DeclaredNames(statement)];
            if (block.Statements.Where(sibling => sibling != statement).SelectMany(DeclaredNames).Any(names.Contains))
            {
                continue;
            }
            yield return statement;
        }
    }

    private static IEnumerable<string> DeclaredNames(SyntaxNode scope)
    {
        foreach (SyntaxNode node in scope.DescendantNodes())
        {
            SyntaxToken identifier = node switch
            {
                VariableDeclaratorSyntax declarator => declarator.Identifier,
                SingleVariableDesignationSyntax designation => designation.Identifier,
                ForEachStatementSyntax loop => loop.Identifier,
                CatchDeclarationSyntax caught => caught.Identifier,
                ParameterSyntax parameter => parameter.Identifier,
                LocalFunctionStatementSyntax function => function.Identifier,
                _ => default
            };
            if (!identifier.IsKind(SyntaxKind.None)) yield return identifier.ValueText;
        }
    }

    private static List<StyleEdit>? Rewrite(
        StyleSource source,
        UsingStatementSyntax statement,
        IReadOnlyList<TextSpan> protectedSpans
    ) {
        VariableDeclarationSyntax declaration = statement.Declaration!;
        if (!StyleLayout.IsBlank(source.content, statement.OpenParenToken.Span.End, declaration.SpanStart)) return null;
        if (!StyleLayout.IsBlank(source.content, declaration.Span.End, statement.CloseParenToken.SpanStart))
        {
            return null;
        }
        if (!source.IsLastOnLine(statement.CloseParenToken)) return null;
        List<StyleEdit> edits =
        [
            new StyleEdit(statement.UsingKeyword.Span.End, declaration.SpanStart, " "),
            new StyleEdit(declaration.Span.End, statement.CloseParenToken.Span.End, ";")
        ];
        int indentation = source.Indentation(statement.SpanStart);
        if (statement.Statement is not BlockSyntax body)
        {
            AddShifts(
                source,
                edits,
                source.LineNumber(statement.Statement.SpanStart),
                source.LineNumber(statement.Statement.Span.End),
                indentation - source.Indentation(statement.Statement.SpanStart),
                protectedSpans
            );
            return edits;
        }
        if (!source.IsFirstOnLine(body.OpenBraceToken) || !source.IsLastOnLine(body.OpenBraceToken)) return null;
        if (!source.IsFirstOnLine(body.CloseBraceToken) || !source.IsLastOnLine(body.CloseBraceToken)) return null;
        if (body.OpenBraceToken.HasLeadingTrivia && body.OpenBraceToken.LeadingTrivia.Any(IsComment)) return null;
        if (body.CloseBraceToken.LeadingTrivia.Any(IsComment)) return null;
        int openLine = source.LineNumber(body.OpenBraceToken.SpanStart);
        int closeLine = source.LineNumber(body.CloseBraceToken.SpanStart);
        edits.Add(RemoveLine(source, openLine));
        AddShifts(source, edits, openLine + 1, closeLine - 1, -StyleLayout.C_INDENT, protectedSpans);
        edits.Add(RemoveLine(source, closeLine));
        return edits;
    }

    private static bool IsComment(SyntaxTrivia trivia)
        => trivia.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.MultiLineCommentTrivia || trivia.IsDirective;

    private static StyleEdit RemoveLine(
        StyleSource source,
        int line
    ) {
        TextLine textLine = source.text.Lines[line];
        return new StyleEdit(textLine.Start, textLine.EndIncludingLineBreak, string.Empty);
    }

    private static void AddShifts(
        StyleSource source,
        List<StyleEdit> edits,
        int firstLine,
        int lastLine,
        int delta,
        IReadOnlyList<TextSpan> protectedSpans
    ) {
        if (delta == 0) return;
        for (int line = firstLine; line <= lastLine; line++)
        {
            StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
            if (shift is not null) edits.Add(shift);
        }
    }
}
