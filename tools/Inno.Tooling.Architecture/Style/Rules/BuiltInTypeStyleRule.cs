using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-03: built-in types are written with their keywords, also when accessing static members.
internal sealed class BuiltInTypeStyleRule : StyleRule
{
    private const string C_ID = "E-03";

    private static readonly Dictionary<string, string> KEYWORDS = new()
    {
        ["Boolean"] = "bool",
        ["Byte"] = "byte",
        ["SByte"] = "sbyte",
        ["Char"] = "char",
        ["Decimal"] = "decimal",
        ["Double"] = "double",
        ["Single"] = "float",
        ["Int16"] = "short",
        ["UInt16"] = "ushort",
        ["Int32"] = "int",
        ["UInt32"] = "uint",
        ["Int64"] = "long",
        ["UInt64"] = "ulong",
        ["IntPtr"] = "nint",
        ["UIntPtr"] = "nuint",
        ["Object"] = "object",
        ["String"] = "string"
    };

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (IdentifierNameSyntax name in Violations(source.root))
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    name.SpanStart,
                    $"Write `{KEYWORDS[name.Identifier.ValueText]}` instead of `{name.Identifier.ValueText}`."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Violations(source.root)
            .Select(static name => new StyleEdit(name.SpanStart, name.Span.End, KEYWORDS[name.Identifier.ValueText]))
            .ToArray();

    private static IEnumerable<IdentifierNameSyntax> Violations(SyntaxNode root)
    {
        bool importsSystem = root
            .DescendantNodes()
            .OfType<UsingDirectiveSyntax>()
            .Any(static directive => directive.Alias is null && directive.Name?.ToString() == "System");
        foreach (IdentifierNameSyntax name in root.DescendantNodes(descendIntoTrivia: false).OfType<IdentifierNameSyntax>())
        {
            if (!KEYWORDS.ContainsKey(name.Identifier.ValueText) || name.Identifier.Text.StartsWith('@')) continue;
            SyntaxNode? parent = name.Parent;
            if (!importsSystem || IsQualifiedBySystem(name)) continue;
            if (parent is QualifiedNameSyntax or AliasQualifiedNameSyntax or NameEqualsSyntax or NameColonSyntax or UsingDirectiveSyntax)
            {
                continue;
            }
            if (parent is MemberAccessExpressionSyntax access && access.Name == name) continue;
            if (parent is MemberBindingExpressionSyntax) continue;
            if (name.Ancestors().Any(IsNameOf)) continue;
            bool isType = IsTypePosition(name);
            bool isStaticAccess = parent is MemberAccessExpressionSyntax member && member.Expression == name;
            if (isType || isStaticAccess) yield return name;
        }
    }

    private static bool IsQualifiedBySystem(IdentifierNameSyntax name)
        => name.Parent is QualifiedNameSyntax { Left: IdentifierNameSyntax { Identifier.ValueText: "System" } } qualified
           && qualified.Right == name;

    private static bool IsNameOf(SyntaxNode node)
        => node is InvocationExpressionSyntax { Expression: IdentifierNameSyntax { Identifier.ValueText: "nameof" } };

    // An identifier is a type when the parser placed it where only a type can stand.
    private static bool IsTypePosition(IdentifierNameSyntax name)
        => name.Parent switch
    {
        VariableDeclarationSyntax declaration => declaration.Type == name,
        ParameterSyntax parameter => parameter.Type == name,
        MethodDeclarationSyntax method => method.ReturnType == name,
        LocalFunctionStatementSyntax local => local.ReturnType == name,
        PropertyDeclarationSyntax property => property.Type == name,
        IndexerDeclarationSyntax indexer => indexer.Type == name,
        DelegateDeclarationSyntax @delegate => @delegate.ReturnType == name,
        TypeArgumentListSyntax or ArrayTypeSyntax or NullableTypeSyntax or PointerTypeSyntax or RefTypeSyntax => true,
        ObjectCreationExpressionSyntax creation => creation.Type == name,
        CastExpressionSyntax cast => cast.Type == name,
        TypeOfExpressionSyntax or DefaultExpressionSyntax or SizeOfExpressionSyntax => true,
        DeclarationExpressionSyntax declaration => declaration.Type == name,
        DeclarationPatternSyntax or TypePatternSyntax or RecursivePatternSyntax => true,
        ForEachStatementSyntax loop => loop.Type == name,
        TupleElementSyntax or SimpleBaseTypeSyntax or TypeConstraintSyntax => true,
        BinaryExpressionSyntax binary => binary.Right == name && binary.Kind() is SyntaxKind.IsExpression or SyntaxKind.AsExpression,
        _ => false
    };
}
