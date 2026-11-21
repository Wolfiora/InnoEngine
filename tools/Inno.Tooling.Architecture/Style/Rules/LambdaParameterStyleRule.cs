using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-11: a lambda with one untyped parameter writes it without parentheses.
internal sealed class LambdaParameterStyleRule : StyleRule
{
    private const string C_ID = "E-11";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (ParenthesizedLambdaExpressionSyntax lambda in Violations(source.root))
        {
            findings.Add(source.Finding(C_ID, lambda.SpanStart, "Write a single lambda parameter without parentheses."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Violations(source.root)
            .Select(static lambda => new StyleEdit(
                lambda.ParameterList.SpanStart,
                lambda.ParameterList.Span.End,
                lambda.ParameterList.Parameters[0].Identifier.Text
            ))
            .ToArray();

    private static IEnumerable<ParenthesizedLambdaExpressionSyntax> Violations(SyntaxNode root)
        => root.DescendantNodes()
            .OfType<ParenthesizedLambdaExpressionSyntax>()
            .Where(static lambda => (
                lambda.ReturnType is null
                && lambda.AttributeLists.Count == 0
                && lambda.ParameterList.Parameters.Count == 1
                && lambda.ParameterList.Parameters[0] is { Type: null, Default: null, AttributeLists.Count: 0, Modifiers.Count: 0 }
                && !lambda.ParameterList.DescendantTrivia().Any(static trivia => trivia.IsDirective || trivia.ToString().Contains('/'))
            ));
}
