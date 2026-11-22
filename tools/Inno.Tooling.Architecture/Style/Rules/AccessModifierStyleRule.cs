using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// T-01: every type and member states its accessibility; interface members do not write `public`.
internal sealed class AccessModifierStyleRule : StyleRule
{
    private const string C_ID = "T-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (MemberDeclarationSyntax member in source.root.DescendantNodes().OfType<MemberDeclarationSyntax>())
        {
            if (Missing(member) is not null)
            {
                findings.Add(source.Finding(C_ID, Anchor(member).SpanStart, "State the accessibility of this declaration."));
            }
            else if (Redundant(member) is { } token)
            {
                findings.Add(source.Finding(C_ID, token.SpanStart, "Do not write `public` on an interface member."));
            }
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        foreach (MemberDeclarationSyntax member in source.root.DescendantNodes().OfType<MemberDeclarationSyntax>())
        {
            if (Missing(member) is { } accessibility)
            {
                // Another part of a partial type may state a different accessibility, so only the author can write it.
                if (member is BaseTypeDeclarationSyntax && member.Modifiers.Any(SyntaxKind.PartialKeyword)) continue;
                int start = Anchor(member).SpanStart;
                edits.Add(new StyleEdit(start, start, accessibility + " "));
            }
            else if (Redundant(member) is { } plain
                     && plain.TrailingTrivia.All(static trivia => trivia.IsKind(SyntaxKind.WhitespaceTrivia))
            ) {
                edits.Add(new StyleEdit(plain.SpanStart, plain.FullSpan.End, string.Empty));
            }
        }

        return edits;
    }

    // Returns the accessibility the declaration has implicitly, or null when it is stated or cannot be stated.
    private static string? Missing(MemberDeclarationSyntax member)
    {
        if (member is BaseNamespaceDeclarationSyntax
            or GlobalStatementSyntax
            or EnumMemberDeclarationSyntax
            or IncompleteMemberSyntax
            or DestructorDeclarationSyntax
        ) {
            return null;
        }

        SyntaxTokenList modifiers = member.Modifiers;
        if (modifiers.Any(IsAccessibility) || modifiers.Any(SyntaxKind.FileKeyword)) return null;
        if (member.Parent is InterfaceDeclarationSyntax) return null;
        if (member is ConstructorDeclarationSyntax && modifiers.Any(SyntaxKind.StaticKeyword)) return null;
        if (member is MethodDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
            or PropertyDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
            or EventDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
            or IndexerDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
            or OperatorDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
            or ConversionOperatorDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
        ) {
            return null;
        }

        // A partial method without accessibility has different rules from one that states it.
        if (member is MethodDeclarationSyntax && modifiers.Any(SyntaxKind.PartialKeyword)) return null;
        return member.Parent is TypeDeclarationSyntax ? "private" : "internal";
    }

    private static SyntaxToken? Redundant(MemberDeclarationSyntax member)
    {
        if (member.Parent is not InterfaceDeclarationSyntax || member is BaseTypeDeclarationSyntax or DelegateDeclarationSyntax)
        {
            return null;
        }
        SyntaxTokenList modifiers = member.Modifiers;
        if (modifiers.Count(IsAccessibility) != 1) return null;
        SyntaxToken token = modifiers.First(IsAccessibility);
        return token.IsKind(SyntaxKind.PublicKeyword) ? token : null;
    }

    private static bool IsAccessibility(SyntaxToken token)
        => token.Kind() is SyntaxKind.PublicKeyword
           or SyntaxKind.PrivateKeyword
           or SyntaxKind.ProtectedKeyword
           or SyntaxKind.InternalKeyword;

    // The first token after the attribute lists, where a missing accessibility is written.
    private static SyntaxToken Anchor(MemberDeclarationSyntax member)
        => member.AttributeLists.Count == 0 ? member.GetFirstToken() : member.AttributeLists.Last().GetLastToken().GetNextToken();
}
