using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

internal static class CSharpStyleValidator
{
    internal static void Validate(
        string relativePath,
        string source,
        ICollection<string> failures
    ) {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview));
        SourceText text = tree.GetText();
        foreach (ParameterListSyntax parameters in tree.GetRoot().DescendantNodes().OfType<ParameterListSyntax>())
        {
            if (parameters.Parameters.Count <= 1)
                continue;
            var violations = new List<string>();
            TextLine opening = text.Lines.GetLineFromPosition(parameters.OpenParenToken.SpanStart);
            int previous = opening.LineNumber;
            foreach (ParameterSyntax parameter in parameters.Parameters)
            {
                TextLine line = text.Lines.GetLineFromPosition(parameter.SpanStart);
                if (line.LineNumber <= previous)
                    violations.Add("place each parameter on its own line after the opening parenthesis");
                previous = text.Lines.GetLineFromPosition(parameter.Span.End - 1).LineNumber;
            }

            TextLine closing = text.Lines.GetLineFromPosition(parameters.CloseParenToken.SpanStart);
            string prefix = text.ToString(TextSpan.FromBounds(closing.Start, parameters.CloseParenToken.SpanStart));
            TextLine declaration = text.Lines.GetLineFromPosition(parameters.Parent!.SpanStart);
            string indentation = new(declaration.ToString().TakeWhile(static value => value is ' ' or '\t').ToArray());
            if (closing.LineNumber <= previous || prefix != indentation)
                violations.Add("align the closing parenthesis on its own line with the declaration");

            BlockSyntax? body = parameters.Parent switch
            {
                MethodDeclarationSyntax { ConstraintClauses.Count: 0 } method => method.Body,
                ConstructorDeclarationSyntax { Initializer: null } constructor => constructor.Body,
                LocalFunctionStatementSyntax { ConstraintClauses.Count: 0 } local => local.Body,
                OperatorDeclarationSyntax operation => operation.Body,
                ConversionOperatorDeclarationSyntax conversion => conversion.Body,
                _ => null
            };
            if (body is not null && text.Lines.GetLineFromPosition(body.OpenBraceToken.SpanStart).LineNumber != closing.LineNumber)
                violations.Add("place the function body opening brace after the closing parenthesis as ') {'");

            if (violations.Count > 0)
                failures.Add($"{relativePath}:{opening.LineNumber + 1}: {string.Join("; ", violations.Distinct(StringComparer.Ordinal))}.");
        }
    }
}
