using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// Applies the `SuppressMessage("InnoStyle", ...)` attributes of one source to its findings (EX-01).
internal static class StyleSuppressions
{
    internal const string C_ID = "EX-01";

    private const string C_CATEGORY = "InnoStyle";

    private const string C_JUSTIFICATION = "Justification";

    // Removes the suppressed findings and reports every suppression that breaks EX-01. Only a rule in `rules` ran, so
    // only its suppressions can be unused.
    internal static List<StyleFinding> Apply(
        StyleSource source,
        IReadOnlyCollection<StyleFinding> findings,
        IReadOnlyCollection<StyleRule> rules,
        ICollection<StyleSuppression>? suppressions
    ) {
        List<StyleFinding> result = [];
        List<Scope> scopes = [];
        foreach (AttributeSyntax attribute in source.root.DescendantNodes().OfType<AttributeSyntax>())
        {
            if (!IsStyleSuppression(attribute)) continue;
            string? problem = Problem(attribute, out string ruleID, out string justification);
            if (problem is not null)
            {
                result.Add(source.Finding(C_ID, attribute.SpanStart, problem));
                continue;
            }

            MemberDeclarationSyntax declaration = (MemberDeclarationSyntax)attribute.Parent!.Parent!;
            scopes.Add(
                new Scope(
                    ruleID,
                    source.LineNumber(declaration.FullSpan.Start) + 1,
                    source.LineNumber(declaration.Span.End) + 1,
                    attribute.SpanStart
                )
            );
            suppressions?.Add(new StyleSuppression(ruleID, source.path, source.LineNumber(attribute.SpanStart) + 1, justification));
        }

        var used = new HashSet<Scope>();
        foreach (StyleFinding finding in findings)
        {
            Scope? scope = scopes
                .Where(candidate => candidate.Covers(finding))
                .OrderBy(static candidate => candidate.lastLine - candidate.firstLine)
                .FirstOrDefault();
            if (scope is null)
            {
                result.Add(finding);
            }
            else
            {
                used.Add(scope);
            }
        }

        foreach (Scope scope in scopes)
        {
            if (!used.Contains(scope) && rules.Any(rule => rule.Covers(scope.rule)))
            {
                result.Add(
                    source.Finding(C_ID, scope.position, $"The suppression of `{scope.rule}` hides no finding. Remove it.")
                );
            }
        }

        return result;
    }

    private static bool IsStyleSuppression(AttributeSyntax attribute)
        => attribute.Name.ToString().Split('.')[^1] is "SuppressMessage" or "SuppressMessageAttribute"
           && Literal(Positional(attribute, 0)) == C_CATEGORY;

    private static string? Problem(
        AttributeSyntax attribute,
        out string ruleID,
        out string justification
    ) {
        ruleID = Literal(Positional(attribute, 1)) ?? string.Empty;
        justification = Literal(
            attribute.ArgumentList?.Arguments
                .FirstOrDefault(static argument => argument.NameEquals?.Name.Identifier.ValueText == C_JUSTIFICATION)
        ) ?? string.Empty;
        if (ruleID.Length == 0) return "Name one rule ID as the second argument of the suppression.";

        string id = ruleID;
        StyleRule[] rules = StyleEngine.RULES.Where(rule => rule.Covers(id)).ToArray();
        if (rules.Length == 0) return $"The style check does not report `{ruleID}`. Remove the suppression.";
        if (rules.Any(static rule => rule.isFixable))
        {
            return $"`style fix` corrects `{ruleID}`. Remove the suppression.";
        }
        if (attribute.Parent is not AttributeListSyntax { Target: null, Parent: MemberDeclarationSyntax declaration }
            || declaration is BaseNamespaceDeclarationSyntax or GlobalStatementSyntax or IncompleteMemberSyntax
        ) {
            return $"Put the suppression of `{ruleID}` on a member or a type.";
        }

        return string.IsNullOrWhiteSpace(justification)
               ? $"State the reason for the suppression of `{ruleID}` in `{C_JUSTIFICATION}`."
               : null;
    }

    private static AttributeArgumentSyntax? Positional(
        AttributeSyntax attribute,
        int index
    ) => attribute.ArgumentList?.Arguments
        .Where(static argument => argument.NameEquals is null && argument.NameColon is null)
        .ElementAtOrDefault(index);

    private static string? Literal(AttributeArgumentSyntax? argument)
        => argument?.Expression is LiteralExpressionSyntax literal && literal.IsKind(SyntaxKind.StringLiteralExpression)
           ? literal.Token.ValueText
           : null;

    private sealed record Scope(
        string rule,
        int firstLine,
        int lastLine,
        int position
    ) {
        internal bool Covers(StyleFinding finding)
            => string.Equals(finding.rule, rule, StringComparison.Ordinal) && finding.line >= firstLine && finding.line <= lastLine;
    }
}
