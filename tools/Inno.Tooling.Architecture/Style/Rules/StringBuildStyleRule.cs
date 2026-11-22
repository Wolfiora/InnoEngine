using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-09: a string is built from values with interpolation, not with `string.Format` or `string.Concat`.
// A format string that is itself a value, and the concatenation of one sequence, have no interpolated form.
internal sealed class StringBuildStyleRule : StyleRule
{
    private const string C_ID = "E-09";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (InvocationExpressionSyntax call in source.root.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            if (call.Expression is not MemberAccessExpressionSyntax { Expression: PredefinedTypeSyntax receiver } access)
            {
                continue;
            }
            if (!receiver.Keyword.IsKind(SyntaxKind.StringKeyword)) continue;
            SeparatedSyntaxList<ArgumentSyntax> arguments = call.ArgumentList.Arguments;
            bool isFormat = access.Name.Identifier.ValueText == "Format"
                            && arguments.Any(
                                static argument => argument.Expression is LiteralExpressionSyntax or InterpolatedStringExpressionSyntax
                            );
            bool isConcat = access.Name.Identifier.ValueText == "Concat" && arguments.Count >= 2;
            if (isFormat || isConcat)
            {
                findings.Add(source.Finding(C_ID, call.SpanStart, "Build the string with interpolation: `$\"...\"`."));
            }
        }
    }
}
