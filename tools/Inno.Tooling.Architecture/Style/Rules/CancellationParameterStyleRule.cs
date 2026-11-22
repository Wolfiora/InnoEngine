using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// P-03: a `CancellationToken` parameter of a declaration is named `cancellationToken`.
// A lambda parameter is not reported: it may have to differ from a token of the enclosing member.
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
            if (parameter.Type is not IdentifierNameSyntax { Identifier.ValueText: "CancellationToken" }) continue;
            if (parameter.Parent is not ParameterListSyntax { Parent: not AnonymousFunctionExpressionSyntax }) continue;
            if (parameter.Identifier.ValueText == C_NAME) continue;
            findings.Add(
                source.Finding(
                    C_ID,
                    parameter.Identifier.SpanStart,
                    $"Name the parameter `{C_NAME}`. Found `{parameter.Identifier.ValueText}`."
                )
            );
        }
    }
}
