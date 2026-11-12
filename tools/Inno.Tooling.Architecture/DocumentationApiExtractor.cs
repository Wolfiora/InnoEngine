using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class DocumentationApiExtractor
{
    private static readonly SymbolDisplayFormat S_SIGNATURE = new(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints,
        delegateStyle: SymbolDisplayDelegateStyle.NameAndSignature,
        kindOptions: SymbolDisplayKindOptions.IncludeTypeKeyword,
        memberOptions: SymbolDisplayMemberOptions.IncludeContainingType | SymbolDisplayMemberOptions.IncludeParameters
            | SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeModifiers
            | SymbolDisplayMemberOptions.IncludeAccessibility | SymbolDisplayMemberOptions.IncludeConstantValue
            | SymbolDisplayMemberOptions.IncludeExplicitInterface | SymbolDisplayMemberOptions.IncludeRef,
        parameterOptions: SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName
            | SymbolDisplayParameterOptions.IncludeDefaultValue | SymbolDisplayParameterOptions.IncludeParamsRefOut,
        propertyStyle: SymbolDisplayPropertyStyle.ShowReadWriteDescriptor,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
            | SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers);

    internal static IReadOnlyList<DocumentationApiEntry> Capture(
        string root,
        string project,
        string? page,
        Compilation? compilation,
        ICollection<string> failures,
        CancellationToken cancellationToken
    ) {
        var result = new List<DocumentationApiEntry>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (SyntaxTree tree in compilation?.SyntaxTrees ?? [])
        {
            cancellationToken.ThrowIfCancellationRequested();
            SemanticModel model = compilation!.GetSemanticModel(tree);
            foreach (SyntaxNode node in model.SyntaxTree.GetRoot(cancellationToken).DescendantNodes()
                .Where(static node => node is BaseTypeDeclarationSyntax or DelegateDeclarationSyntax))
            {
                if (model.GetDeclaredSymbol(node, cancellationToken) is not INamedTypeSymbol type)
                    continue;
                foreach (ISymbol symbol in new ISymbol[] { type }.Concat(type.GetMembers()))
                {
                    if (!IsVisible(symbol) || symbol.Name.StartsWith('<')
                        || symbol is IMethodSymbol { AssociatedSymbol: not null }
                        || symbol is IMethodSymbol { IsImplicitlyDeclared: true, MethodKind: MethodKind.Constructor,
                            ContainingType.TypeKind: TypeKind.Struct or TypeKind.Enum })
                        continue;
                    string? id = symbol.GetDocumentationCommentId();
                    if (id is null || !seen.Add(id))
                        continue;
                    if (HasUnresolvedContract(symbol))
                        failures.Add($"API '{Path.GetRelativePath(root, project)}' cannot resolve the complete public signature of '{id}'; prepare its evaluated reference closure.");
                    string signature = Signature(symbol);
                    string identity = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(signature)));
                    Location location = symbol.Locations.FirstOrDefault(static location => location.IsInSource) ?? node.GetLocation();
                    string source = Path.GetRelativePath(root, location.SourceTree?.FilePath ?? tree.FilePath).Replace('\\', '/');
                    result.Add(new(Path.GetFileNameWithoutExtension(project), id, signature, identity, source,
                        location.GetLineSpan().StartLinePosition.Line + 1,
                        page is null ? string.Empty : Path.GetRelativePath(root, page).Replace('\\', '/'),
                        Documentation(symbol, new HashSet<ISymbol>(SymbolEqualityComparer.Default), cancellationToken),
                        symbol.IsImplicitlyDeclared,
                        symbol is IFieldSymbol { HasConstantValue: true } field ? field.ConstantValue : null,
                        symbol.Kind.ToString(), symbol.ContainingType?.GetDocumentationCommentId(),
                        symbol is INamedTypeSymbol named ? named.BaseType?.ToDisplayString() : null,
                        symbol is INamedTypeSymbol namedType
                            ? namedType.Interfaces.Select(static contract => contract.ToDisplayString()).ToArray() : []));
                }
            }
        }
        return result;
    }

    private static bool HasUnresolvedContract(ISymbol symbol)
    {
        IEnumerable<ITypeSymbol> types = symbol switch
        {
            INamedTypeSymbol type => type.Interfaces.Cast<ITypeSymbol>()
                .Concat(type.BaseType is null ? [] : new ITypeSymbol[] { type.BaseType })
                .Concat(type.TypeParameters.SelectMany(static parameter => parameter.ConstraintTypes)),
            IMethodSymbol method => new[] { method.ReturnType }.Concat(method.Parameters.Select(static parameter => parameter.Type))
                .Concat(method.TypeParameters.SelectMany(static parameter => parameter.ConstraintTypes)),
            IPropertySymbol property => new[] { property.Type }.Concat(property.Parameters.Select(static parameter => parameter.Type)),
            IFieldSymbol field => [field.Type],
            IEventSymbol signal => [signal.Type],
            _ => []
        };
        return types.Any(IsUnresolved);
    }

    internal static IReadOnlyList<DocumentationApiEntry> CaptureReference(
        IAssemblySymbol assembly,
        Func<ISymbol, DocumentationApiEntry> resolveOrigin,
        CancellationToken cancellationToken
    ) {
        var entries = new SortedDictionary<string, DocumentationApiEntry>(StringComparer.Ordinal);
        VisitNamespace(assembly.GlobalNamespace);
        return entries.Values.ToArray();

        void VisitNamespace(INamespaceSymbol space)
        {
            foreach (INamespaceSymbol child in space.GetNamespaceMembers())
                VisitNamespace(child);
            foreach (INamedTypeSymbol type in space.GetTypeMembers())
                VisitType(type);
        }

        void VisitType(INamedTypeSymbol type)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!IsVisible(type))
                return;
            foreach (ISymbol symbol in new ISymbol[] { type }.Concat(type.GetMembers()))
            {
                if (!IsVisible(symbol) || symbol is INamedTypeSymbol nested && !SymbolEqualityComparer.Default.Equals(nested, type)
                    || symbol.Name.StartsWith('<') || symbol is IMethodSymbol { AssociatedSymbol: not null }
                    || symbol is IMethodSymbol { MethodKind: MethodKind.Constructor, ContainingType.TypeKind: TypeKind.Struct or TypeKind.Enum,
                        IsImplicitlyDeclared: true })
                    continue;
                string? id = symbol.GetDocumentationCommentId();
                if (id is null)
                    continue;
                if (HasUnresolvedContract(symbol))
                    throw new InvalidDataException("The projected API reference closure is incomplete: " + id);
                DocumentationApiEntry origin = resolveOrigin(symbol);
                string signature = Signature(symbol);
                string documentation = Documentation(symbol, new HashSet<ISymbol>(SymbolEqualityComparer.Default), cancellationToken);
                entries.Add(id, new(origin.project, id, signature,
                    Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(signature))), origin.source, origin.line,
                    string.Empty, string.IsNullOrWhiteSpace(documentation) ? origin.documentation : documentation,
                    origin.implicitDeclaration, symbol is IFieldSymbol { HasConstantValue: true } field ? field.ConstantValue : null,
                    symbol.Kind.ToString(), symbol.ContainingType?.GetDocumentationCommentId(),
                    symbol is INamedTypeSymbol named ? named.BaseType?.ToDisplayString() : null,
                    symbol is INamedTypeSymbol namedType
                        ? namedType.Interfaces.Select(static contract => contract.ToDisplayString()).ToArray() : [], origin.projectionOnly));
            }
            foreach (INamedTypeSymbol child in type.GetTypeMembers())
                VisitType(child);
        }
    }

    private static bool IsUnresolved(ITypeSymbol type) => type switch
    {
        IErrorTypeSymbol => true,
        IArrayTypeSymbol array => IsUnresolved(array.ElementType),
        IPointerTypeSymbol pointer => IsUnresolved(pointer.PointedAtType),
        INamedTypeSymbol named => named.TypeArguments.Any(IsUnresolved),
        IFunctionPointerTypeSymbol pointer => HasUnresolvedContract(pointer.Signature),
        _ => false
    };

    private static string Signature(ISymbol symbol)
    {
        string signature = symbol.ToDisplayString(S_SIGNATURE);
        if (symbol is not INamedTypeSymbol type)
            return signature;
        var modifiers = new List<string>();
        modifiers.Add(type.DeclaredAccessibility switch
        {
            Accessibility.Public => "public",
            Accessibility.Protected => "protected",
            Accessibility.ProtectedOrInternal => "protected internal",
            _ => throw new InvalidOperationException("A Wiki type signature must have externally visible accessibility.")
        });
        if (type.IsStatic)
            modifiers.Add("static");
        else if (type.TypeKind == TypeKind.Class)
        {
            if (type.IsAbstract)
                modifiers.Add("abstract");
            if (type.IsSealed)
                modifiers.Add("sealed");
        }
        signature = string.Join(' ', modifiers) + " " + signature;
        if (type.TypeKind == TypeKind.Delegate)
            return signature;
        SymbolDisplayFormat reference = S_SIGNATURE.WithKindOptions(SymbolDisplayKindOptions.None);
        var bases = new List<string>();
        if (type.EnumUnderlyingType is not null)
            bases.Add(type.EnumUnderlyingType.ToDisplayString(reference));
        else if (type.BaseType is { SpecialType: not SpecialType.System_Object and not SpecialType.System_ValueType } parent)
            bases.Add(parent.ToDisplayString(reference));
        bases.AddRange(type.Interfaces.Select(contract => contract.ToDisplayString(reference)));
        return bases.Count == 0 ? signature : signature + " : " + string.Join(", ", bases);
    }

    private static string Documentation(
        ISymbol symbol,
        ISet<ISymbol> visited,
        CancellationToken cancellationToken
    ) {
        if (!visited.Add(symbol))
            return string.Empty;
        if (symbol is IMethodSymbol { MethodKind: MethodKind.DelegateInvoke } invoke)
            return Documentation(invoke.ContainingType, visited, cancellationToken);
        string xml = symbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: cancellationToken) ?? string.Empty;
        XElement? own;
        try
        {
            own = string.IsNullOrWhiteSpace(xml) ? null : XElement.Parse(xml);
        }
        catch (System.Xml.XmlException)
        {
            // The XML validator reports malformed source; inventory capture must still complete.
            return string.Empty;
        }
        if (own is not null && own.Element("inheritdoc") is null)
            return xml;
        foreach (ISymbol inherited in PublicApiDocumentationValidator.InheritedMembers(symbol))
        {
            string contract = Documentation(inherited, visited, cancellationToken);
            if (contract.Length == 0)
                continue;
            XElement merged = XElement.Parse(contract);
            if (own is not null)
            {
                foreach (XElement item in own.Elements().Where(static element => element.Name != "inheritdoc"))
                {
                    string? identity = (string?)item.Attribute("name") ?? (string?)item.Attribute("cref");
                    merged.Elements(item.Name).Where(element =>
                        ((string?)element.Attribute("name") ?? (string?)element.Attribute("cref")) == identity).Remove();
                    merged.Add(new XElement(item));
                }
            }
            return merged.ToString(SaveOptions.DisableFormatting);
        }
        return xml;
    }

    private static bool IsVisible(ISymbol symbol)
    {
        if (symbol is not INamedTypeSymbol and not IMethodSymbol and not IPropertySymbol and not IFieldSymbol and not IEventSymbol)
            return false;
        for (ISymbol? current = symbol; current is not null && current is not INamespaceSymbol; current = current.ContainingType)
            if (current.DeclaredAccessibility is not Accessibility.Public and not Accessibility.Protected and not Accessibility.ProtectedOrInternal)
                return false;
        return true;
    }

    internal sealed record DocumentationApiEntry(
        string project,
        string id,
        string signature,
        string signatureHash,
        string source,
        int line,
        string page,
        string documentation,
        bool implicitDeclaration,
        object? constantValue,
        string kind,
        string? containingType,
        string? baseType,
        IReadOnlyList<string> interfaces,
        [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool projectionOnly = false
    );
}
