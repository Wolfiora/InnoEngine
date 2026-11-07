using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class GenerationCleanupValidator
{
    internal static void Validate(
        string relative,
        SyntaxTree syntax,
        ICollection<string> failures
    ) {
        SyntaxNode root = syntax.GetRoot();
        foreach (CatchClauseSyntax clause in root.DescendantNodes().OfType<CatchClauseSyntax>())
        {
            string? type = clause.Declaration?.Type.ToString().Split('.').Last();
            if (type is "RetirementPendingException" or "RetirementTimeoutException")
            {
                int line = clause.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                failures.Add($"{relative}:{line}: retirement catches must classify the full exception tree with RetirementPendingException.Find; direct typed catches bypass wrapped pending ownership.");
            }
        }
        foreach (InvocationExpressionSyntax invocation in root
                     .DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            string? name = invocation.Expression switch
            {
                IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
                MemberAccessExpressionSyntax member => member.Name.Identifier.ValueText,
                _ => null
            };
            if (name == "OnCleanupFailed" &&
                !relative.EndsWith("/Inno.Extensibility.Types/TypeRegistry.cs", StringComparison.Ordinal))
            {
                int line = invocation.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                failures.Add($"{relative}:{line}: cleanup failures must propagate through registry retirement; a diagnostic-only OnCleanupFailed call bypasses the generation fault barrier.");
            }
        }
    }
}
