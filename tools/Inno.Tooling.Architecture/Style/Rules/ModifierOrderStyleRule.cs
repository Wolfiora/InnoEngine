using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// T-02: modifiers follow one order.
internal sealed class ModifierOrderStyleRule : StyleRule
{
    private const string C_ID = "T-02";

    private static readonly SyntaxKind[] ORDER =
    [
        SyntaxKind.FileKeyword,
        SyntaxKind.PublicKeyword,
        SyntaxKind.PrivateKeyword,
        SyntaxKind.ProtectedKeyword,
        SyntaxKind.InternalKeyword,
        SyntaxKind.StaticKeyword,
        SyntaxKind.ExternKeyword,
        SyntaxKind.NewKeyword,
        SyntaxKind.ConstKeyword,
        SyntaxKind.VirtualKeyword,
        SyntaxKind.AbstractKeyword,
        SyntaxKind.SealedKeyword,
        SyntaxKind.OverrideKeyword,
        SyntaxKind.ReadOnlyKeyword,
        SyntaxKind.UnsafeKeyword,
        SyntaxKind.RequiredKeyword,
        SyntaxKind.VolatileKeyword,
        SyntaxKind.FixedKeyword,
        SyntaxKind.AsyncKeyword,
        SyntaxKind.RefKeyword,
        SyntaxKind.PartialKeyword
    ];

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Reorder;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (SyntaxTokenList modifiers in Violations(source.root))
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    modifiers[0].SpanStart,
                    "Order the modifiers: accessibility, static, extern, new, virtual/abstract/sealed/override, readonly, unsafe, required, volatile, async, partial."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        foreach (SyntaxTokenList modifiers in Violations(source.root))
        {
            SyntaxToken[] sorted = modifiers.OrderBy(Rank).ToArray();
            for (int index = 0; index < modifiers.Count; index++)
            {
                if (modifiers[index].Text != sorted[index].Text)
                {
                    edits.Add(new(modifiers[index].SpanStart, modifiers[index].Span.End, sorted[index].Text));
                }
            }
        }

        return edits;
    }

    private static IEnumerable<SyntaxTokenList> Violations(SyntaxNode root)
    {
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            SyntaxTokenList modifiers = node switch
            {
                MemberDeclarationSyntax member => member.Modifiers,
                LocalFunctionStatementSyntax local => local.Modifiers,
                AccessorDeclarationSyntax accessor => accessor.Modifiers,
                _ => default
            };
            if (modifiers.Count < 2 || modifiers.Any(static token => Array.IndexOf(ORDER, token.Kind()) < 0)) continue;
            bool isOrdered = true;
            for (int index = 1; index < modifiers.Count; index++)
            {
                isOrdered &= Rank(modifiers[index - 1]) <= Rank(modifiers[index]);
            }

            if (!isOrdered) yield return modifiers;
        }
    }

    // `protected internal` and `private protected` keep the order they are written in.
    private static int Rank(SyntaxToken token)
    {
        int rank = Array.IndexOf(ORDER, token.Kind());
        return rank is >= 1 and <= 4 ? 1 : rank;
    }
}
