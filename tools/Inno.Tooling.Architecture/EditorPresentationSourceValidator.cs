using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class EditorPresentationSourceValidator
{
    private const string C_FACADE = "Inno.Editor.ImGui.ImGui";

    internal static void Validate(
        string relativePath,
        SyntaxTree tree,
        ICollection<string> failures
    ) {
        if (!relativePath.StartsWith("src/composition/editor/", StringComparison.Ordinal))
            return;
        SyntaxNode root = tree.GetRoot();
        UsingDirectiveSyntax[] aliases = root.DescendantNodes().OfType<UsingDirectiveSyntax>()
            .Where(static directive => directive.Alias is not null && directive.Name is not null).ToArray();
        foreach (IGrouping<string, UsingDirectiveSyntax> group in aliases.GroupBy(
            static directive => directive.Name!.ToString(), StringComparer.Ordinal))
        {
            if (group.Count() > 1)
                Report(relativePath, tree, group.First(), "one type cannot have multiple presentation aliases in the same file", failures);
        }
        var facadeAliases = aliases.Where(static directive => directive.Name!.ToString() == C_FACADE)
            .Select(static directive => directive.Alias!.Name.Identifier.ValueText).ToHashSet(StringComparer.Ordinal);
        facadeAliases.Add(C_FACADE);
        facadeAliases.Add("global::" + C_FACADE);
        foreach (BlockSyntax block in root.DescendantNodes().OfType<BlockSyntax>())
        {
            if (block.Parent is not (ForStatementSyntax or ForEachStatementSyntax or ForEachVariableStatementSyntax or WhileStatementSyntax or DoStatementSyntax))
                continue;
            ExpressionStatementSyntax[] statements = block.Statements.OfType<ExpressionStatementSyntax>().ToArray();
            foreach (ExpressionStatementSyntax push in statements.Where(statement => IsFacadeCall(statement, "PushId", facadeAliases)))
            {
                if (statements.Any(statement => statement.SpanStart > push.SpanStart && IsFacadeCall(statement, "PopId", facadeAliases)))
                    Report(relativePath, tree, push, "presentation loop ID scopes require local try/finally cleanup", failures);
            }
        }
        foreach (DocumentationCommentTriviaSyntax comment in root.DescendantTrivia()
            .Select(static trivia => trivia.GetStructure()).OfType<DocumentationCommentTriviaSyntax>())
        {
            if (comment.Content.OfType<XmlElementSyntax>().Count(static element => element.StartTag.Name.LocalName.ValueText == "summary") > 1)
                Report(relativePath, tree, comment, "a declaration cannot retain multiple XML summary elements", failures);
        }
    }

    private static bool IsFacadeCall(
        ExpressionStatementSyntax statement,
        string method,
        IReadOnlySet<string> receivers
    ) => statement.Expression is InvocationExpressionSyntax { Expression: MemberAccessExpressionSyntax member }
        && member.Name.Identifier.ValueText == method && receivers.Contains(member.Expression.ToString());

    private static void Report(
        string relativePath,
        SyntaxTree tree,
        SyntaxNode node,
        string message,
        ICollection<string> failures
    ) => failures.Add($"{relativePath}:{tree.GetLineSpan(node.Span).StartLinePosition.Line + 1}: {message}.");
}
