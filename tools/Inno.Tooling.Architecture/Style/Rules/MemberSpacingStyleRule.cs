using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// S-03: types, methods, constructors, documented members and multi-line members are separated by one blank
// line. Enum members are never separated by blank lines.
internal sealed class MemberSpacingStyleRule : StyleRule
{
    private const string C_ID = "S-03";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((StyleEdit edit, string message) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, edit.start, message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Violations(source).Select(static violation => violation.edit).ToArray();

    private static IEnumerable<(StyleEdit edit, string message)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodesAndSelf())
        {
            IReadOnlyList<SyntaxNode> members = node switch
            {
                CompilationUnitSyntax unit => unit.Members,
                BaseNamespaceDeclarationSyntax scope => scope.Members,
                TypeDeclarationSyntax type => type.Members,
                EnumDeclarationSyntax declaration => declaration.Members,
                _ => []
            };
            for (int index = 1; index < members.Count; index++)
            {
                SyntaxNode previous = members[index - 1];
                SyntaxNode current = members[index];
                if (previous is BaseNamespaceDeclarationSyntax) continue;
                int end = source.LineNumber(previous.Span.End);
                int start = source.LineNumber(current.SpanStart);
                if (start <= end) continue;
                int next = end + 1;
                while (next < start && StyleLines.IsBlank(source, next))
                {
                    next++;
                }

                if (node is EnumDeclarationSyntax)
                {
                    for (int line = end + 1; line < next; line++)
                    {
                        yield return (StyleLines.Remove(source, line), "Do not separate enum members with blank lines.");
                    }

                    continue;
                }

                if (next > end + 1 || !(IsSeparated(source, previous) || IsSeparated(source, current))) continue;
                // A preprocessor directive between two members keeps its position.
                if (current.GetLeadingTrivia().Any(static trivia => trivia.IsDirective)) continue;
                yield return (StyleLines.InsertBefore(source, next), "Separate these members with one blank line.");
            }
        }
    }

    private static bool IsSeparated(
        StyleSource source,
        SyntaxNode member
    ) => member is BaseMethodDeclarationSyntax or BaseTypeDeclarationSyntax or DelegateDeclarationSyntax
         || source.LineNumber(member.SpanStart) != source.LineNumber(member.Span.End)
         || member.GetLeadingTrivia().Any(
             static trivia => (
                 trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)
                 || trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia)
             )
         );
}
