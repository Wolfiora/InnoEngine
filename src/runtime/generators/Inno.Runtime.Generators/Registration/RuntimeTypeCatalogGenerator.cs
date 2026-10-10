using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Runtime.Generators;

internal static class RuntimeTypeCatalogGenerator
{
    internal static (
        IReadOnlyList<INamedTypeSymbol> factories,
        IReadOnlyList<(INamedTypeSymbol definition, ITypeSymbol[] arguments)> rejected
    ) Analyze(
        Compilation compilation,
        SourceProductionContext output
    ) {
        var values = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        foreach (INamedTypeSymbol owner in EnumerateTypes(compilation.Assembly.GlobalNamespace))
        {
            foreach (ISymbol member in owner.GetMembers().Where(static member => member.GetAttributes().Any(static attribute =>
                attribute.AttributeClass?.ToDisplayString() == "Inno.Core.Serialization.SerializablePropertyAttribute")))
            {
                ITypeSymbol? value = member switch
                {
                    IPropertySymbol property => property.Type,
                    IFieldSymbol field => field.Type,
                    _ => null
                };
                if (value is not null && !ContainsParameters(value))
                    AddValueClosure(value, values);
            }
        }
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (GenericNameSyntax syntax in tree.GetRoot(output.CancellationToken).DescendantNodes().OfType<GenericNameSyntax>())
            {
                if (model.GetTypeInfo(syntax, output.CancellationToken).Type is INamedTypeSymbol type && !ContainsParameters(type))
                    AddValueClosure(type, values);
                if (model.GetSymbolInfo(syntax, output.CancellationToken).Symbol is IMethodSymbol method)
                {
                    foreach (ITypeSymbol argument in method.TypeArguments.Where(static argument => !ContainsParameters(argument)))
                        AddValueClosure(argument, values);
                }
            }
        }
        INamedTypeSymbol[] converters = new[] { compilation.Assembly }
            .Concat(compilation.SourceModule.ReferencedAssemblySymbols.Where(static assembly =>
                assembly.GetTypeByMetadataName(assembly.Name + ".Generated.RuntimeModuleCatalog") is not null))
            .SelectMany(static assembly => EnumerateTypes(assembly.GlobalNamespace))
            .Where(static type => type.IsGenericType && type.TypeParameters.Length != 0 && GetConverterPattern(type) is not null)
            .ToArray();
        var factories = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        var rejected = new List<(INamedTypeSymbol definition, ITypeSymbol[] arguments)>();
        foreach (ITypeSymbol value in values)
        {
            if (value is INamedTypeSymbol { IsGenericType: true } named && IsAvailable(named, compilation))
                factories.Add(named);
            foreach (INamedTypeSymbol converter in converters)
            {
                var bindings = new Dictionary<ITypeParameterSymbol, ITypeSymbol>(SymbolEqualityComparer.Default);
                if (!Unify(GetConverterPattern(converter)!, value, bindings) ||
                    converter.TypeParameters.Any(parameter => !bindings.ContainsKey(parameter)))
                    continue;
                ITypeSymbol[] arguments = converter.TypeParameters.Select(parameter => bindings[parameter]).ToArray();
                if (SatisfiesConstraints(converter, bindings, compilation))
                {
                    INamedTypeSymbol construction = converter.Construct(arguments);
                    if (IsAvailable(construction, compilation))
                        factories.Add(construction);
                    else
                        RuntimeRegistrationDiagnostics.ReportUnavailable(output, construction,
                            "the converter or an argument is inaccessible from the generated assembly catalog");
                }
                else if (IsAvailable(converter, compilation) &&
                    arguments.All(argument => argument is not INamedTypeSymbol named || IsArgumentAccessible(named, compilation)))
                {
                    rejected.Add((converter, arguments));
                }
            }
        }
        return (factories.OrderBy(static type => type.ToDisplayString(), StringComparer.Ordinal).ToArray(),
            rejected.OrderBy(static value => value.definition.ToDisplayString(), StringComparer.Ordinal)
                .ThenBy(static value => string.Join(",", value.arguments.Select(static argument => argument.ToDisplayString())), StringComparer.Ordinal)
                .ToArray());
    }

    private static void AddValueClosure(
        ITypeSymbol type,
        HashSet<ITypeSymbol> values
    ) {
        if (!values.Add(type))
            return;
        if (type is IArrayTypeSymbol array)
            AddValueClosure(array.ElementType, values);
        if (type is INamedTypeSymbol named)
        {
            foreach (ITypeSymbol argument in named.TypeArguments)
                AddValueClosure(argument, values);
            foreach (ISymbol member in named.GetMembers().Where(static member => member.GetAttributes().Any(static attribute =>
                attribute.AttributeClass?.ToDisplayString() == "Inno.Core.Serialization.SerializablePropertyAttribute")))
            {
                ITypeSymbol? value = member switch
                {
                    IPropertySymbol property => property.Type,
                    IFieldSymbol field => field.Type,
                    _ => null
                };
                if (value is not null && !ContainsParameters(value))
                    AddValueClosure(value, values);
            }
        }
    }

    private static bool IsAvailable(
        INamedTypeSymbol type,
        Compilation compilation
    ) {
        if (!SymbolEqualityComparer.Default.Equals(type.ContainingAssembly, compilation.Assembly) &&
            type.ContainingAssembly.GetTypeByMetadataName(type.ContainingAssembly.Name + ".Generated.RuntimeModuleCatalog") is null)
            return false;
        for (INamedTypeSymbol? owner = type; owner is not null; owner = owner.ContainingType)
        {
            if (owner.IsFileLocal || owner.DeclaredAccessibility is not
                (Accessibility.Public or Accessibility.Internal or Accessibility.ProtectedOrInternal))
                return false;
            if (owner.DeclaredAccessibility != Accessibility.Public &&
                !SymbolEqualityComparer.Default.Equals(owner.ContainingAssembly, compilation.Assembly))
                return false;
        }
        return type.TypeArguments.All(argument => argument is not INamedTypeSymbol named || IsArgumentAccessible(named, compilation));
    }

    private static bool IsArgumentAccessible(
        INamedTypeSymbol type,
        Compilation compilation
    ) {
        for (INamedTypeSymbol? owner = type; owner is not null; owner = owner.ContainingType)
        {
            if (owner.IsFileLocal || owner.DeclaredAccessibility != Accessibility.Public &&
                (!SymbolEqualityComparer.Default.Equals(owner.ContainingAssembly, compilation.Assembly) ||
                    owner.DeclaredAccessibility is not (Accessibility.Internal or Accessibility.ProtectedOrInternal)))
                return false;
        }
        return type.TypeArguments.All(argument => argument is not INamedTypeSymbol named || IsArgumentAccessible(named, compilation));
    }

    private static ITypeSymbol? GetConverterPattern(INamedTypeSymbol type)
    {
        for (INamedTypeSymbol? current = type.BaseType; current is not null; current = current.BaseType)
        {
            if (current.OriginalDefinition.ToDisplayString() == "Inno.Core.Serialization.Converters.SerializationConverter<T>")
                return current.TypeArguments[0];
        }
        return null;
    }

    private static bool Unify(
        ITypeSymbol pattern,
        ITypeSymbol value,
        Dictionary<ITypeParameterSymbol, ITypeSymbol> bindings
    ) {
        if (pattern is ITypeParameterSymbol parameter)
        {
            if (bindings.TryGetValue(parameter, out ITypeSymbol? existing))
                return SymbolEqualityComparer.Default.Equals(existing, value);
            bindings.Add(parameter, value);
            return true;
        }
        if (pattern is IArrayTypeSymbol array)
            return value is IArrayTypeSymbol other && array.Rank == other.Rank && Unify(array.ElementType, other.ElementType, bindings);
        if (pattern is INamedTypeSymbol named && value is INamedTypeSymbol concrete &&
            SymbolEqualityComparer.Default.Equals(named.OriginalDefinition, concrete.OriginalDefinition))
        {
            for (int index = 0; index < named.TypeArguments.Length; index++)
            {
                if (!Unify(named.TypeArguments[index], concrete.TypeArguments[index], bindings))
                    return false;
            }
            return named.ContainingType is null || concrete.ContainingType is not null &&
                Unify(named.ContainingType, concrete.ContainingType, bindings);
        }
        return SymbolEqualityComparer.Default.Equals(pattern, value);
    }

    private static bool SatisfiesConstraints(
        INamedTypeSymbol definition,
        Dictionary<ITypeParameterSymbol, ITypeSymbol> bindings,
        Compilation compilation
    ) {
        foreach (ITypeParameterSymbol parameter in definition.TypeParameters)
        {
            ITypeSymbol argument = bindings[parameter];
            if (parameter.HasReferenceTypeConstraint && !argument.IsReferenceType ||
                parameter.HasValueTypeConstraint && (!argument.IsValueType || argument.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T) ||
                parameter.HasUnmanagedTypeConstraint && !argument.IsUnmanagedType ||
                parameter.HasConstructorConstraint && !argument.IsValueType &&
                (argument is not INamedTypeSymbol named || named.IsAbstract || !named.InstanceConstructors.Any(static constructor =>
                    constructor.Parameters.Length == 0 && constructor.DeclaredAccessibility == Accessibility.Public)))
                return false;
            foreach (ITypeSymbol constraint in parameter.ConstraintTypes)
            {
                ITypeSymbol substituted = Substitute(constraint, bindings, compilation);
                if (!compilation.ClassifyCommonConversion(argument, substituted).IsImplicit)
                    return false;
            }
        }
        return true;
    }

    private static ITypeSymbol Substitute(
        ITypeSymbol type,
        Dictionary<ITypeParameterSymbol, ITypeSymbol> bindings,
        Compilation compilation
    ) => type switch
    {
        ITypeParameterSymbol parameter => bindings[parameter],
        IArrayTypeSymbol array => compilation.CreateArrayTypeSymbol(Substitute(array.ElementType, bindings, compilation), array.Rank),
        INamedTypeSymbol { IsGenericType: true } named => named.OriginalDefinition.Construct(named.TypeArguments
            .Select(argument => Substitute(argument, bindings, compilation)).ToArray()),
        _ => type
    };

    private static bool ContainsParameters(ITypeSymbol type)
        => type is ITypeParameterSymbol || type is INamedTypeSymbol named &&
            (named.IsUnboundGenericType || named.TypeArguments.Any(ContainsParameters) ||
                named.ContainingType is not null && ContainsParameters(named.ContainingType)) ||
            type is IArrayTypeSymbol array && ContainsParameters(array.ElementType);

    private static IEnumerable<INamedTypeSymbol> EnumerateTypes(INamespaceOrTypeSymbol scope)
    {
        foreach (ISymbol member in scope.GetMembers())
        {
            if (member is INamedTypeSymbol type)
                yield return type;
            if (member is INamespaceOrTypeSymbol child)
            {
                foreach (INamedTypeSymbol nested in EnumerateTypes(child))
                    yield return nested;
            }
        }
    }
}
