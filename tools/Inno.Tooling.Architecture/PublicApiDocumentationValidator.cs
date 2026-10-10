using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class PublicApiDocumentationValidator
{
    internal static void Validate(
        string relativePath,
        string source,
        ICollection<string> failures,
        SemanticModel? model,
        DocumentationSourceModels models
    ) {
        SyntaxNode root = model?.SyntaxTree.GetRoot()
            ?? CSharpSyntaxTree.ParseText(source, path: relativePath).GetRoot();
        foreach (MemberDeclarationSyntax member in root.DescendantNodes().OfType<MemberDeclarationSyntax>())
        {
            if (!RequiresDocumentation(member))
                continue;

            DocumentationCommentTriviaSyntax? documentation = member.GetLeadingTrivia()
                .Select(static trivia => trivia.GetStructure())
                .OfType<DocumentationCommentTriviaSyntax>()
                .LastOrDefault();
            FileLinePositionSpan span = member.GetLocation().GetLineSpan();
            string location = $"{relativePath}:{span.StartLinePosition.Line + 1}";
            if (documentation is null)
            {
                failures.Add($"{location}: public or protected declaration is missing explicit XML documentation.");
                continue;
            }
            if (documentation.ContainsDiagnostics)
                failures.Add($"{location}: public or protected XML documentation is malformed.");
            XmlEmptyElementSyntax? inherited = documentation.Content.OfType<XmlEmptyElementSyntax>()
                .FirstOrDefault(static element => element.Name.LocalName.Text == "inheritdoc");
            if (inherited is not null)
            {
                SyntaxNode declaration = member is EventFieldDeclarationSyntax signal
                    ? signal.Declaration.Variables[0]
                    : member;
                ISymbol? symbol = model?.GetDeclaredSymbol(declaration);
                XmlCrefAttributeSyntax? reference = inherited.Attributes.OfType<XmlCrefAttributeSyntax>().FirstOrDefault();
                ISymbol? selected = reference is null ? null : model?.GetSymbolInfo(reference.Cref).Symbol;
                bool validReference = reference is null || symbol is not null && selected is not null
                    && InheritedMembers(symbol).Any(target => SymbolEqualityComparer.Default.Equals(
                        target.OriginalDefinition, selected.OriginalDefinition));
                if (!validReference || symbol is null
                    || !HasInheritedContract(symbol, models, new HashSet<ISymbol>(SymbolEqualityComparer.Default)))
                    failures.Add($"{location}: inheritdoc requires a documented overridden or implemented contract.");
                continue;
            }

            XmlElementSyntax? summary = FindElement(documentation, "summary");
            if (summary is null || string.IsNullOrWhiteSpace(GetText(summary)))
                failures.Add($"{location}: public or protected declaration requires a meaningful summary.");

            ValidateTypeParameters(location, member, documentation, failures);
            ValidateParameters(location, member, documentation, failures);
            if (RequiresReturns(member) && FindElement(documentation, "returns") is null)
                failures.Add($"{location}: non-void public or protected operation requires a returns contract.");
        }
    }

    private static bool HasInheritedContract(
        ISymbol symbol,
        DocumentationSourceModels models,
        ISet<ISymbol> visited
    ) {
        if (!visited.Add(symbol))
            return false;
        foreach (ISymbol target in InheritedMembers(symbol))
        {
            string xml = target.GetDocumentationCommentXml() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(xml))
            {
                // The repository owns its contracts; external framework documentation is supplied by its publisher.
                if (!models.OwnsAssembly(target.ContainingAssembly))
                    return true;
                continue;
            }
            XElement document;
            try
            {
                document = XElement.Parse(xml);
            }
            catch (System.Xml.XmlException)
            {
                continue;
            }
            if (document.Element("inheritdoc") is not null && HasInheritedContract(target, models, visited))
                return true;
            if (string.IsNullOrWhiteSpace(document.Element("summary")?.Value))
                continue;
            IEnumerable<IParameterSymbol> parameters = target switch
            {
                IMethodSymbol method => method.Parameters,
                IPropertySymbol property => property.Parameters,
                _ => []
            };
            var documented = document.Elements("param").Select(static element => (string?)element.Attribute("name"))
                .ToHashSet(StringComparer.Ordinal);
            if (parameters.Any(parameter => !documented.Contains(parameter.Name)))
                continue;
            if (target is IMethodSymbol generic)
            {
                var typeParameters = document.Elements("typeparam")
                    .Select(static element => (string?)element.Attribute("name")).ToHashSet(StringComparer.Ordinal);
                if (generic.TypeParameters.Any(parameter => !typeParameters.Contains(parameter.Name)))
                    continue;
            }
            if (target is IMethodSymbol { ReturnsVoid: false } && document.Element("returns") is null)
                continue;
            return true;
        }
        return false;
    }

    private static IEnumerable<ISymbol> InheritedMembers(ISymbol symbol)
    {
        ISymbol? overridden = symbol switch
        {
            IMethodSymbol method => method.OverriddenMethod,
            IPropertySymbol property => property.OverriddenProperty,
            IEventSymbol signal => signal.OverriddenEvent,
            _ => null
        };
        if (overridden is not null)
            yield return overridden;
        if (symbol.ContainingType is not INamedTypeSymbol owner)
            yield break;
        foreach (INamedTypeSymbol contract in owner.AllInterfaces)
        {
            foreach (ISymbol member in contract.GetMembers())
            {
                if (SymbolEqualityComparer.Default.Equals(owner.FindImplementationForInterfaceMember(member), symbol))
                    yield return member;
            }
        }
    }

    private static bool RequiresDocumentation(MemberDeclarationSyntax member)
    {
        if (member is EnumMemberDeclarationSyntax enumMember)
            return enumMember.Parent is EnumDeclarationSyntax declaration && RequiresDocumentation(declaration);
        if (member.Parent is InterfaceDeclarationSyntax)
            return true;
        return member.Modifiers.Any(static modifier => modifier.IsKind(SyntaxKind.PublicKeyword)
            || modifier.IsKind(SyntaxKind.ProtectedKeyword));
    }

    private static void ValidateTypeParameters(
        string location,
        MemberDeclarationSyntax member,
        DocumentationCommentTriviaSyntax documentation,
        ICollection<string> failures
    ) {
        TypeParameterListSyntax? parameters = member switch
        {
            TypeDeclarationSyntax type => type.TypeParameterList,
            MethodDeclarationSyntax method => method.TypeParameterList,
            DelegateDeclarationSyntax callback => callback.TypeParameterList,
            _ => null
        };
        if (parameters is null)
            return;
        HashSet<string> documented = GetNamedElements(documentation, "typeparam");
        foreach (TypeParameterSyntax parameter in parameters.Parameters)
        {
            if (!documented.Contains(parameter.Identifier.ValueText))
                failures.Add($"{location}: type parameter '{parameter.Identifier.ValueText}' is missing XML documentation.");
        }
    }

    private static void ValidateParameters(
        string location,
        MemberDeclarationSyntax member,
        DocumentationCommentTriviaSyntax documentation,
        ICollection<string> failures
    ) {
        IEnumerable<ParameterSyntax> parameters = member switch
        {
            ClassDeclarationSyntax type => type.ParameterList?.Parameters ?? default,
            StructDeclarationSyntax type => type.ParameterList?.Parameters ?? default,
            RecordDeclarationSyntax type => type.ParameterList?.Parameters ?? default,
            MethodDeclarationSyntax method => method.ParameterList.Parameters,
            ConstructorDeclarationSyntax constructor => constructor.ParameterList.Parameters,
            DelegateDeclarationSyntax callback => callback.ParameterList.Parameters,
            OperatorDeclarationSyntax operation => operation.ParameterList.Parameters,
            ConversionOperatorDeclarationSyntax conversion => conversion.ParameterList.Parameters,
            IndexerDeclarationSyntax indexer => indexer.ParameterList.Parameters,
            _ => []
        };
        HashSet<string> documented = GetNamedElements(documentation, "param");
        foreach (ParameterSyntax parameter in parameters)
        {
            if (!documented.Contains(parameter.Identifier.ValueText))
                failures.Add($"{location}: parameter '{parameter.Identifier.ValueText}' is missing XML documentation.");
        }
    }

    private static bool RequiresReturns(MemberDeclarationSyntax member)
        => member switch
        {
            MethodDeclarationSyntax method => !IsVoid(method.ReturnType),
            DelegateDeclarationSyntax callback => !IsVoid(callback.ReturnType),
            OperatorDeclarationSyntax => true,
            ConversionOperatorDeclarationSyntax => true,
            _ => false
        };

    private static bool IsVoid(TypeSyntax type)
        => type is PredefinedTypeSyntax predefined && predefined.Keyword.IsKind(SyntaxKind.VoidKeyword);

    private static XmlElementSyntax? FindElement(
        DocumentationCommentTriviaSyntax documentation,
        string name
    )
        => documentation.Content.OfType<XmlElementSyntax>().FirstOrDefault(element =>
            element.StartTag.Name.LocalName.Text == name);

    private static HashSet<string> GetNamedElements(
        DocumentationCommentTriviaSyntax documentation,
        string name
    )
        => documentation.Content
            .OfType<XmlElementSyntax>()
            .Where(element => element.StartTag.Name.LocalName.Text == name)
            .SelectMany(static element => element.StartTag.Attributes.OfType<XmlNameAttributeSyntax>())
            .Where(static attribute => attribute.Name.LocalName.Text == "name")
            .Select(static attribute => attribute.Identifier.Identifier.ValueText)
            .ToHashSet(StringComparer.Ordinal);

    private static string GetText(XmlElementSyntax element)
        => string.Concat(element.Content.Select(static content => content.ToString())).Trim();
}
