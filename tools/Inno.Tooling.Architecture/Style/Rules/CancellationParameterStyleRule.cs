using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// P-03: a `CancellationToken` parameter of a declaration is named `cancellationToken` and is the last parameter.
// A lambda parameter is not reported: it may have to differ from a token of the enclosing member.
// A required token that precedes optional or `params` parameters is not reported: it cannot move without a default.
internal sealed class CancellationParameterStyleRule : StyleRule
{
    private const string C_ID = "P-03";
    private const string C_NAME = "cancellationToken";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (ParameterSyntax parameter in source.root.DescendantNodes().OfType<ParameterSyntax>())
        {
            if (!IsCancellationToken(parameter)) continue;
            if (parameter.Parent is not ParameterListSyntax { Parent: not AnonymousFunctionExpressionSyntax } list)
            {
                continue;
            }
            if (parameter.Identifier.ValueText != C_NAME)
            {
                findings.Add(
                    source.Finding(
                        C_ID,
                        parameter.Identifier.SpanStart,
                        $"Name the parameter `{C_NAME}`. Found `{parameter.Identifier.ValueText}`."
                    )
                );
            }

            if (MustMoveLast(parameter, list))
            {
                findings.Add(
                    source.Finding(
                        C_ID,
                        parameter.SpanStart,
                        $"Make `{parameter.Identifier.ValueText}` the last parameter."
                    )
                );
            }
        }
    }

    private static bool MustMoveLast(
        ParameterSyntax parameter,
        ParameterListSyntax list
    ) {
        int index = list.Parameters.IndexOf(parameter);
        if (index == list.Parameters.Count - 1) return false;

        List<ParameterSyntax> later = list.Parameters.Skip(index + 1).ToList();
        if (later.Any(IsCancellationToken)) return false;

        return parameter.Default is not null
               || !later.Any(other => other.Default is not null || other.Modifiers.Any(SyntaxKind.ParamsKeyword));
    }

    private static bool IsCancellationToken(ParameterSyntax parameter)
        => parameter.Type is IdentifierNameSyntax { Identifier.ValueText: "CancellationToken" };
}
