using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Core.Serialization.Generators;

internal static class SerializationCollectionGenerator
{
    internal static string? Generate(
        ITypeSymbol type,
        Compilation compilation,
        Queue<ITypeSymbol> pending,
        StringBuilder accessors,
        ref int index
    ) {
        if (type is IArrayTypeSymbol array)
        {
            if (array.Rank != 1)
                return null;
            pending.Enqueue(array.ElementType);
            string element = Name(array.ElementType);
            return "new global::Inno.Core.Serialization.SerializationCollectionMetadata(typeof(" + element
                + "), null, static values => BuildArray<" + element + ">(values), null, null)";
        }
        if (type is not INamedTypeSymbol named || type.SpecialType == SpecialType.System_String)
            return null;
        INamedTypeSymbol[] contracts = new[] { named }.Concat(named.AllInterfaces).ToArray();
        INamedTypeSymbol? map = contracts.FirstOrDefault(static contract =>
            contract.OriginalDefinition.ToDisplayString() is "System.Collections.Generic.IDictionary<TKey, TValue>"
                or "System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>");
        INamedTypeSymbol? sequence = contracts.FirstOrDefault(static contract =>
            contract.OriginalDefinition.SpecialType == SpecialType.System_Collections_Generic_IEnumerable_T);
        if (map is null && sequence?.TypeArguments[0] is INamedTypeSymbol entry
            && entry.OriginalDefinition.ToDisplayString() == "System.Collections.Generic.KeyValuePair<TKey, TValue>")
            map = entry;
        if (map is not null)
        {
            ITypeSymbol key = map.TypeArguments[0];
            ITypeSymbol value = map.TypeArguments[1];
            pending.Enqueue(key);
            pending.Enqueue(value);
            INamedTypeSymbol dictionary = compilation.GetTypeByMetadataName("System.Collections.Generic.Dictionary`2")!.Construct(key, value);
            INamedTypeSymbol pair = compilation.GetTypeByMetadataName("System.Collections.Generic.KeyValuePair`2")!.Construct(key, value);
            string? construction = Construct(named, dictionary, [key, value], pair, compilation, accessors, ref index);
            if (construction is null)
                return null;
            string keyName = Name(key);
            string valueName = Name(value);
            return "new global::Inno.Core.Serialization.SerializationCollectionMetadata(typeof(" + valueName
                + "), typeof(" + keyName + "), null, static entries => { var result = new " + Name(dictionary)
                + "(); foreach (var entry in entries) result.Add((" + keyName + ")entry.Key!, (" + valueName
                + ")entry.Value!); return " + construction + "; }, static value => { var result = new global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<object?, object?>>(); foreach (var entry in (global::System.Collections.Generic.IEnumerable<"
                + Name(pair) + ">)value) result.Add(new(entry.Key, entry.Value)); return result; })";
        }
        if (sequence is null)
            return null;
        ITypeSymbol elementType = sequence.TypeArguments[0];
        pending.Enqueue(elementType);
        INamedTypeSymbol standard = compilation.GetTypeByMetadataName("System.Collections.Generic.List`1")!.Construct(elementType);
        if (named.TypeKind == TypeKind.Interface && !compilation.ClassifyConversion(standard, named).IsImplicit)
            standard = compilation.GetTypeByMetadataName("System.Collections.Generic.HashSet`1")!.Construct(elementType);
        string? result = Construct(named, standard, [elementType], elementType, compilation, accessors, ref index);
        if (result is null)
            return null;
        string elementName = Name(elementType);
        return "new global::Inno.Core.Serialization.SerializationCollectionMetadata(typeof(" + elementName
            + "), null, static values => { var result = new " + Name(standard)
            + "(); foreach (var value in values) result.Add((" + elementName
            + ")value!); return " + result + "; }, null, null)";
    }

    private static string? Construct(
        INamedTypeSymbol type,
        INamedTypeSymbol standard,
        ITypeSymbol[] addArguments,
        ITypeSymbol elementType,
        Compilation compilation,
        StringBuilder accessors,
        ref int index
    ) {
        if (compilation.ClassifyConversion(standard, type).IsImplicit)
            return "result";
        if (type.TypeKind == TypeKind.Interface || type.IsAbstract)
            return null;
        foreach (IMethodSymbol constructor in type.InstanceConstructors.Where(static constructor => constructor.Parameters.Length == 1))
        {
            if (!compilation.ClassifyConversion(standard, constructor.Parameters[0].Type).IsImplicit)
                continue;
            if (IsAccessible(constructor, compilation))
                return "new " + Name(type) + "(result)";
            if (SymbolEqualityComparer.Default.Equals(type.ContainingAssembly, compilation.Assembly))
            {
                string name = "ConstructCollection" + index++;
                accessors.Append("    [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Constructor)]\n")
                    .Append("    private static extern ").Append(Name(type)).Append(' ').Append(name).Append('(')
                    .Append(Name(constructor.Parameters[0].Type)).Append(" values);\n");
                return name + "(result)";
            }
        }
        foreach (IMethodSymbol factory in type.GetMembers().OfType<IMethodSymbol>().Where(static method =>
            method.IsStatic && !method.IsGenericMethod && method.Parameters.Length == 1 && method.Name is "CreateRange" or "Create"))
        {
            if (IsAccessible(factory, compilation) && compilation.ClassifyConversion(standard, factory.Parameters[0].Type).IsImplicit
                && compilation.ClassifyConversion(factory.ReturnType, type).IsImplicit)
                return Name(type) + ".@" + factory.Name + "(result)";
        }
        string definition = type.OriginalDefinition.ToDisplayString();
        if (definition == "System.Collections.Immutable.ImmutableArray<T>")
            return "global::System.Collections.Immutable.ImmutableArray.CreateRange<" + Name(elementType) + ">(result)";
        if (definition == "System.Collections.Immutable.ImmutableList<T>")
            return "global::System.Collections.Immutable.ImmutableList.CreateRange<" + Name(elementType) + ">(result)";
        if (definition == "System.Collections.Immutable.ImmutableHashSet<T>")
            return "global::System.Collections.Immutable.ImmutableHashSet.CreateRange<" + Name(elementType) + ">(result)";
        if (definition == "System.Collections.Immutable.ImmutableDictionary<TKey, TValue>")
            return "global::System.Collections.Immutable.ImmutableDictionary.CreateRange(result)";

        IMethodSymbol? emptyConstructor = type.InstanceConstructors.FirstOrDefault(static constructor => constructor.Parameters.Length == 0);
        if (emptyConstructor is null || !IsAccessible(emptyConstructor, compilation))
            return null;
        IMethodSymbol? add = Ancestors(type).SelectMany(static owner => owner.GetMembers("Add")).OfType<IMethodSymbol>()
            .FirstOrDefault(method => !method.IsStatic && !method.IsGenericMethod && IsAccessible(method, compilation)
                && method.Parameters.Length == addArguments.Length
                && method.Parameters.Select(static parameter => parameter.Type).SequenceEqual(addArguments, SymbolEqualityComparer.Default));
        INamedTypeSymbol? addContract = null;
        if (add is null)
        {
            string contractName = addArguments.Length == 2 ? "System.Collections.Generic.IDictionary`2" : "System.Collections.Generic.ICollection`1";
            INamedTypeSymbol contract = compilation.GetTypeByMetadataName(contractName)!.Construct(addArguments);
            if (!compilation.ClassifyConversion(type, contract).IsImplicit)
                return null;
            addContract = contract;
        }
        string builder = "BuildCollection" + index++;
        string receiver = addContract is null ? "value" : "((" + Name(addContract) + ")value)";
        string addCall = addArguments.Length == 2 ? "entry.Key, entry.Value" : "entry";
        accessors.Append("    private static ").Append(Name(type)).Append(' ').Append(builder).Append('(')
            .Append(Name(standard)).Append(" values)\n    {\n        var value = new ").Append(Name(type))
            .Append("();\n        foreach (var entry in values)\n            ").Append(receiver).Append(".Add(")
            .Append(addCall).Append(");\n        return value;\n    }\n");
        return builder + "(result)";
    }

    private static bool IsAccessible(
        ISymbol symbol,
        Compilation compilation
    ) => symbol.DeclaredAccessibility == Accessibility.Public
        || symbol.DeclaredAccessibility is Accessibility.Internal or Accessibility.ProtectedOrInternal
            && SymbolEqualityComparer.Default.Equals(symbol.ContainingAssembly, compilation.Assembly);

    private static IEnumerable<INamedTypeSymbol> Ancestors(INamedTypeSymbol type)
    {
        for (INamedTypeSymbol? current = type; current is not null; current = current.BaseType)
            yield return current;
    }

    private static string Name(ITypeSymbol type) => type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)
        .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
}
