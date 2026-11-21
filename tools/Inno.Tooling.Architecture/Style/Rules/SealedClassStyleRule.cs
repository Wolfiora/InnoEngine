using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// T-03: a class or record is static, abstract or sealed unless its documentation states a derivation contract.
internal sealed class SealedClassStyleRule : StyleRule
{
    private const string C_ID = "T-03";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (TypeDeclarationSyntax type in source.root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            if (IsOpenClass(type) && !DocumentsDerivation(type))
            {
                findings.Add(
                    source.Finding(
                        C_ID,
                        type.Identifier.SpanStart,
                        "Seal this class, or document its derivation contract in its XML documentation."
                    )
                );
            }
        }
    }

    private static bool IsOpenClass(TypeDeclarationSyntax type)
        => (type is ClassDeclarationSyntax
            || type is RecordDeclarationSyntax record && !record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword))
           && !type.Modifiers.Any(
               static token => token.Kind() is SyntaxKind.StaticKeyword or SyntaxKind.AbstractKeyword or SyntaxKind.SealedKeyword
           );

    // A partial type states the contract on any of its parts, so only an undocumented non-partial class is certain.
    private static bool DocumentsDerivation(TypeDeclarationSyntax type)
        => type.Modifiers.Any(SyntaxKind.PartialKeyword)
           || type.GetLeadingTrivia()
               .Where(
                   static trivia => (
                       trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)
                       || trivia.IsKind(SyntaxKind.SingleLineCommentTrivia)
                   )
               )
               .Any(static trivia => trivia.ToFullString().Contains("deriv", StringComparison.OrdinalIgnoreCase));
}
