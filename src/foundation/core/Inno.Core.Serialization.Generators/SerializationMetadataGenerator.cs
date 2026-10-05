using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Core.Serialization.Generators;

/// <summary>
/// Generates closed member access, restoration hooks and collection construction for static deployments.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class SerializationMetadataGenerator : IIncrementalGenerator
{
    private const string C_SERIALIZABLE = "Inno.Core.Serialization.ISerializable";
    private const string C_PROPERTY = "Inno.Core.Serialization.SerializablePropertyAttribute";
    private const string C_RESTORED = "Inno.Core.Serialization.OnSerializableRestored";
    private static readonly DiagnosticDescriptor InvalidMember = new(
        "INNOSER010", "Invalid static serialization declaration", "Serialization member '{0}' cannot supply its declared access: {1}",
        "Inno.Serialization", DiagnosticSeverity.Error, isEnabledByDefault: true);
    private static readonly DiagnosticDescriptor InvalidHook = new(
        "INNOSER011", "Invalid static restoration callback", "Restoration declaration '{0}' must contain at most one nonstatic, nonvirtual void hook with zero arguments or one SerializationContext argument",
        "Inno.Serialization", DiagnosticSeverity.Error, isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor InvalidOwner = new(
        "INNOSER012", "Inaccessible static serialization declaration",
        "Serialization declaration '{0}' requires a nongeneric partial containing owner; '{1}' cannot host its generated accessors",
        "Inno.Serialization", DiagnosticSeverity.Error, isEnabledByDefault: true);

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, static (
            output,
            compilation
        ) => Generate(output, compilation));
    }

    private static void Generate(
        SourceProductionContext output,
        Compilation compilation
    ) {
        if (compilation.AssemblyName is null || compilation.GetTypeByMetadataName(C_SERIALIZABLE) is null)
            return;
        var pending = new Queue<ITypeSymbol>();
        foreach (INamedTypeSymbol type in EnumerateTypes(compilation.Assembly.GlobalNamespace))
            if (IsSerializable(type))
                pending.Enqueue(type);
        INamespaceSymbol serializationNamespace = compilation.GlobalNamespace.GetNamespaceMembers()
            .Single(static scope => scope.Name == "Inno").GetNamespaceMembers()
            .Single(static scope => scope.Name == "Core").GetNamespaceMembers()
            .Single(static scope => scope.Name == "Serialization");
        var genericMethodNames = new HashSet<string>(EnumerateTypes(serializationNamespace)
            .SelectMany(static type => type.GetMembers().OfType<IMethodSymbol>())
            .Where(static method => method.IsGenericMethod).Select(static method => method.Name), StringComparer.Ordinal);
        foreach (SyntaxTree tree in compilation.SyntaxTrees)
        {
            SemanticModel model = compilation.GetSemanticModel(tree);
            foreach (InvocationExpressionSyntax syntax in tree.GetRoot(output.CancellationToken).DescendantNodes()
                .OfType<InvocationExpressionSyntax>())
            {
                string? methodName = syntax.Expression switch
                {
                    SimpleNameSyntax name => name.Identifier.ValueText,
                    MemberAccessExpressionSyntax access => access.Name.Identifier.ValueText,
                    MemberBindingExpressionSyntax binding => binding.Name.Identifier.ValueText,
                    _ => null
                };
                if (methodName is null || !genericMethodNames.Contains(methodName))
                    continue;
                if (model.GetSymbolInfo(syntax, output.CancellationToken).Symbol is IMethodSymbol method
                    && (method.ContainingNamespace.ToDisplayString() == "Inno.Core.Serialization"
                        || method.ContainingNamespace.ToDisplayString().StartsWith("Inno.Core.Serialization.", StringComparison.Ordinal)))
                    foreach (ITypeSymbol argument in method.TypeArguments)
                        pending.Enqueue(argument);
            }
        }
        var shapes = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        var root = new SerializationMetadataScope(null);
        var scopes = new Dictionary<INamedTypeSymbol, SerializationMetadataScope>(SymbolEqualityComparer.Default);
        var rejectedOwners = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        while (pending.Count > 0)
        {
            ITypeSymbol type = pending.Dequeue().WithNullableAnnotation(NullableAnnotation.NotAnnotated);
            if (!shapes.Add(type) || ContainsParameters(type) || type is INamedTypeSymbol { IsRefLikeType: true })
                continue;
            if (type is INamedTypeSymbol nullable && nullable.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
            {
                pending.Enqueue(nullable.TypeArguments[0]);
                continue;
            }
            if (type.SpecialType != SpecialType.None || type.ToDisplayString() is "System.Guid" or "System.Decimal")
                continue;
            if (IsSerializable(type) && !SymbolEqualityComparer.Default.Equals(type.ContainingAssembly, compilation.Assembly))
                continue;
            ISymbol[] members = type is INamedTypeSymbol declaration ? CollectMembers(declaration, [declaration]).ToArray() : [];
            SerializationMetadataScope? scope = ResolveScope(type, members, compilation, root, scopes, rejectedOwners, output);
            if (scope is null)
                continue;
            GenerateMetadata(type, members, scope, compilation, pending, output);
        }
        output.AddSource("Registration/RuntimeSerializationMetadata.g.cs", root.Emit(compilation.AssemblyName));
        int scopeIndex = 0;
        foreach (SerializationMetadataScope scope in scopes.Values)
            output.AddSource("Registration/RuntimeSerializationOwner" + scopeIndex++ + ".g.cs", scope.Emit(compilation.AssemblyName));
    }

    private static void GenerateMetadata(
        ITypeSymbol type,
        ISymbol[] members,
        SerializationMetadataScope scope,
        Compilation compilation,
        Queue<ITypeSymbol> pending,
        SourceProductionContext output
    ) {
        StringBuilder accessors = scope.accessors;
        string typeName = Name(type);
        string? collection = SerializationCollectionGenerator.Generate(type, compilation, pending, accessors, ref scope.index);
        if (collection is null && !IsSerializable(type) && type.TypeKind != TypeKind.Struct)
            return;
        string creator = "CreateMetadata" + scope.index++;
        if (type is not INamedTypeSymbol named)
        {
            scope.registrations.Append("        register(").Append(creator).Append("());\n");
            scope.creators.Append("    private static global::Inno.Core.Serialization.SerializationTypeMetadata ")
                .Append(creator).Append("() => new(typeof(").Append(typeName)
                .Append("), [], null, collection: ").Append(collection).Append(");\n");
            scope.lookup.Append("        if (type == typeof(").Append(typeName).Append(")) return ").Append(creator).Append("();\n");
            return;
        }
        if (members.Length == 0 && !IsSerializable(type) && collection is null)
            return;
        var memberCode = new List<string>();
        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (ISymbol member in members)
        {
            ITypeSymbol valueType = MemberType(member);
            if (!keys.Add(member.Name))
            {
                output.ReportDiagnostic(Diagnostic.Create(InvalidMember, member.Locations.FirstOrDefault(),
                    member.ToDisplayString(), "duplicate key"));
                continue;
            }
            pending.Enqueue(valueType);
            int visibility = GetVisibility(member);
            string? getter = (visibility & 5) != 0 ? CreateAccessor(member, true, scope.index++, accessors) : null;
            string? setter = (visibility & 10) != 0 ? CreateAccessor(member, false, scope.index++, accessors) : null;
            if (((visibility & 5) != 0 && getter is null) || ((visibility & 10) != 0 && setter is null))
            {
                output.ReportDiagnostic(Diagnostic.Create(InvalidMember, member.Locations.FirstOrDefault(),
                    member.ToDisplayString(), "required getter or writable setter is absent"));
                continue;
            }
            memberCode.Add("new global::Inno.Core.Serialization.SerializationMemberMetadata(" + Quote(member.Name)
                + ", typeof(" + Name(valueType) + "), (global::Inno.Core.Serialization.PropertyVisibility)"
                + visibility + ", " + (getter ?? "null") + ", " + (setter ?? "null") + ")");
        }
        string factory = GenerateFactory(named, scope.index++, accessors);
        string hooks = GenerateHooks([named], ref scope.index, accessors, output);
        bool requiresConverter = Ancestors(named).Any(static ancestor => ancestor.GetAttributes().Any(static attribute =>
            attribute.AttributeClass?.ToDisplayString() == "Inno.Core.Serialization.RequiresSerializationConverterAttribute"));
        string inherited = "null";
        if (named.BaseType is INamedTypeSymbol baseType && IsSerializable(baseType))
        {
            if (SymbolEqualityComparer.Default.Equals(baseType.ContainingAssembly, compilation.Assembly))
                pending.Enqueue(baseType);
            inherited = "global::" + baseType.ContainingAssembly.Name
                + ".Generated.RuntimeSerializationMetadataCatalog.GetMetadata(typeof(" + Name(baseType) + "))";
        }
        scope.registrations.Append("        register(").Append(creator).Append("());\n");
        scope.lookup.Append("        if (type == typeof(").Append(typeName).Append(")) return ").Append(creator).Append("();\n");
        scope.creators.Append("    private static global::Inno.Core.Serialization.SerializationTypeMetadata ")
            .Append(creator).Append("() => new(typeof(")
            .Append(typeName).Append("), [").Append(string.Join(", ", memberCode)).Append("], ")
            .Append(factory).Append(", ").Append(hooks).Append(", ").Append(collection ?? "null")
            .Append(", ").Append(requiresConverter ? "true" : "false").Append(", ").Append(inherited).Append(");\n");
    }

    private static SerializationMetadataScope? ResolveScope(
        ITypeSymbol type,
        ISymbol[] members,
        Compilation compilation,
        SerializationMetadataScope root,
        Dictionary<INamedTypeSymbol, SerializationMetadataScope> scopes,
        HashSet<INamedTypeSymbol> rejectedOwners,
        SourceProductionContext output
    ) {
        ITypeSymbol[] referencedTypes = new[] { type }.Concat(members.Select(MemberType)).ToArray();
        if (referencedTypes.All(value => IsAccessible(value, compilation.Assembly)))
            return root;
        INamedTypeSymbol? owner = FindInaccessibleOwner(type, compilation.Assembly)
            ?? (type as INamedTypeSymbol);
        while (owner is not null && !referencedTypes.All(value => compilation.IsSymbolAccessibleWithin(value, owner)))
            owner = owner.ContainingType;
        if (owner is null)
        {
            output.ReportDiagnostic(Diagnostic.Create(InvalidMember, type.Locations.FirstOrDefault(),
                type.ToDisplayString(), "no source owner can access all serialized member types"));
            return null;
        }
        var chain = new Stack<INamedTypeSymbol>();
        for (INamedTypeSymbol? current = owner; current is not null; current = current.ContainingType)
        {
            if (!SerializationMetadataScope.CanExtend(current))
            {
                if (rejectedOwners.Add(current))
                    output.ReportDiagnostic(Diagnostic.Create(InvalidOwner, current.Locations.FirstOrDefault(),
                        type.ToDisplayString(), current.ToDisplayString()));
                return null;
            }
            chain.Push(current);
        }
        SerializationMetadataScope parent = root;
        foreach (INamedTypeSymbol declaration in chain)
        {
            if (!scopes.TryGetValue(declaration, out SerializationMetadataScope? scope))
            {
                scope = new SerializationMetadataScope(declaration);
                scopes.Add(declaration, scope);
                parent.Add(scope);
            }
            parent = scope;
        }
        return parent;
    }

    private static INamedTypeSymbol? FindInaccessibleOwner(
        ITypeSymbol type,
        IAssemblySymbol assembly
    ) {
        if (type is IArrayTypeSymbol array)
            return FindInaccessibleOwner(array.ElementType, assembly);
        if (type is not INamedTypeSymbol named)
            return null;
        foreach (ITypeSymbol argument in named.TypeArguments)
            if (FindInaccessibleOwner(argument, assembly) is INamedTypeSymbol owner)
                return owner;
        return IsAccessible(named, assembly) ? null : named.ContainingType;
    }

    private static ITypeSymbol MemberType(ISymbol member)
        => member is IFieldSymbol field ? field.Type : ((IPropertySymbol)member).Type;

    private static IEnumerable<ISymbol> CollectMembers(
        INamedTypeSymbol type,
        IEnumerable<INamedTypeSymbol> hierarchy
    ) {
        foreach (INamedTypeSymbol ancestor in hierarchy.Reverse())
        {
            foreach (ISymbol member in ancestor.GetMembers().Where(static member => !member.IsStatic)
                .OrderBy(GetOrder).ThenBy(static member => member.Locations.FirstOrDefault()?.SourceSpan.Start ?? int.MaxValue))
            {
                bool annotated = member.GetAttributes().Any(static attribute => attribute.AttributeClass?.ToDisplayString() == C_PROPERTY);
                bool defaultStructMember = type.TypeKind == TypeKind.Struct && (member is IFieldSymbol
                    { DeclaredAccessibility: Accessibility.Public, IsReadOnly: false, IsConst: false }
                    || member is IPropertySymbol { GetMethod.DeclaredAccessibility: Accessibility.Public,
                        SetMethod.DeclaredAccessibility: Accessibility.Public, IsIndexer: false });
                if ((annotated || defaultStructMember) && member is IFieldSymbol or IPropertySymbol { IsIndexer: false })
                    yield return member;
            }
        }
    }

    private static int GetOrder(ISymbol member)
        => member.GetAttributes().FirstOrDefault(static attribute => attribute.AttributeClass?.ToDisplayString() == C_PROPERTY)
            ?.NamedArguments.FirstOrDefault(static argument => argument.Key == "order").Value.Value as int? ?? int.MaxValue;

    private static int GetVisibility(ISymbol member)
        => member.GetAttributes().FirstOrDefault(static attribute => attribute.AttributeClass?.ToDisplayString() == C_PROPERTY)
            ?.ConstructorArguments.FirstOrDefault().Value as int? ?? 15;

    private static string? CreateAccessor(
        ISymbol member,
        bool read,
        int index,
        StringBuilder accessors
    ) {
        string declaring = Name(member.ContainingType);
        ITypeSymbol valueType = member is IFieldSymbol field ? field.Type : ((IPropertySymbol)member).Type;
        string value = Name(valueType);
        bool isStruct = member.ContainingType.IsValueType;
        string target = isStruct ? "ref global::System.Runtime.CompilerServices.Unsafe.Unbox<" + declaring + ">(target)"
            : "(" + declaring + ")target";
        if (member is IFieldSymbol fieldMember)
        {
            if (!read && (fieldMember.IsReadOnly || fieldMember.IsConst))
                return null;
            accessors.Append("    [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = ")
                .Append(Quote(member.Name)).Append(")]\n    private static extern ref ").Append(value)
                .Append(" Access").Append(index).Append("(").Append(isStruct ? "ref " : "").Append(declaring).Append(" target);\n");
            return read ? "static target => Access" + index + "(" + target + ")"
                : "static (target, value) => Access" + index + "(" + target + ") = (" + value + ")value!";
        }
        IPropertySymbol property = (IPropertySymbol)member;
        IMethodSymbol? method = read ? property.GetMethod : property.SetMethod;
        if (method is null || (!read && method.IsInitOnly))
            return null;
        if (method.DeclaredAccessibility == Accessibility.Public)
        {
            string receiver = isStruct ? "global::System.Runtime.CompilerServices.Unsafe.Unbox<" + declaring + ">(target)"
                : "((" + declaring + ")target)";
            return read ? "static target => " + receiver + ".@" + member.Name
                : "static (target, value) => " + receiver + ".@" + member.Name + " = (" + value + ")value!";
        }
        accessors.Append("    [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Method, Name = ")
            .Append(Quote(method.MetadataName)).Append(")]\n    private static extern ").Append(read ? value : "void")
            .Append(" Access").Append(index).Append("(").Append(isStruct ? "ref " : "").Append(declaring).Append(" target")
            .Append(read ? "" : ", " + value + " value").Append(");\n");
        return read ? "static target => Access" + index + "(" + target + ")"
            : "static (target, value) => Access" + index + "(" + target + ", (" + value + ")value!)";
    }

    private static string GenerateFactory(
        INamedTypeSymbol type,
        int index,
        StringBuilder accessors
    ) {
        if (type.IsValueType)
            return "static () => default(" + Name(type) + ")";
        if (type.IsAbstract || type.TypeKind != TypeKind.Class || Ancestors(type).Any(static ancestor =>
            ancestor.GetMembers().Any(static member => member is IPropertySymbol { IsRequired: true } or IFieldSymbol { IsRequired: true })))
            return "null";
        IMethodSymbol? constructor = type.InstanceConstructors.FirstOrDefault(static constructor => constructor.Parameters.Length == 0);
        if (constructor is null)
            return "null";
        if (constructor.DeclaredAccessibility == Accessibility.Public)
            return "static () => new " + Name(type) + "()";
        accessors.Append("    [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Constructor)]\n")
            .Append("    private static extern ").Append(Name(type)).Append(" Construct").Append(index).Append("();\n");
        return "static () => Construct" + index + "()";
    }

    private static string GenerateHooks(
        IReadOnlyList<INamedTypeSymbol> hierarchy,
        ref int index,
        StringBuilder accessors,
        SourceProductionContext output
    ) {
        var calls = new List<string>();
        foreach (INamedTypeSymbol ancestor in hierarchy.Reverse())
        {
            IMethodSymbol[] hooks = ancestor.GetMembers().OfType<IMethodSymbol>().Where(static method =>
                method.GetAttributes().Any(static attribute => attribute.AttributeClass?.ToDisplayString() == C_RESTORED)).ToArray();
            if (hooks.Length > 1)
                output.ReportDiagnostic(Diagnostic.Create(InvalidHook, ancestor.Locations.FirstOrDefault(), ancestor.ToDisplayString()));
            foreach (IMethodSymbol hook in hooks)
            {
                if (hook.IsStatic || hook.IsVirtual || hook.IsOverride || hook.IsAbstract || hook.IsGenericMethod
                    || !hook.ReturnsVoid || hook.Parameters.Length > 1
                    || hook.Parameters.Length == 1 && (hook.Parameters[0].RefKind != RefKind.None
                        || hook.Parameters[0].Type.ToDisplayString() != "Inno.Core.Serialization.SerializationContext"))
                {
                    output.ReportDiagnostic(Diagnostic.Create(InvalidHook, hook.Locations.FirstOrDefault(), hook.ToDisplayString()));
                    continue;
                }
                string name = "Restore" + index++;
                accessors.Append("    [global::System.Runtime.CompilerServices.UnsafeAccessor(global::System.Runtime.CompilerServices.UnsafeAccessorKind.Method, Name = ")
                    .Append(Quote(hook.MetadataName)).Append(")]\n    private static extern void ").Append(name)
                    .Append("(").Append(ancestor.IsValueType ? "ref " : "").Append(Name(ancestor)).Append(" target")
                    .Append(hook.Parameters.Length == 0 ? "" : ", global::Inno.Core.Serialization.SerializationContext context")
                    .Append(");\n");
                string target = ancestor.IsValueType ? "ref global::System.Runtime.CompilerServices.Unsafe.Unbox<" + Name(ancestor) + ">(target)"
                    : "(" + Name(ancestor) + ")target";
                calls.Add(name + "(" + target + (hook.Parameters.Length == 0 ? "" : ", context") + ");");
            }
        }
        return calls.Count == 0 ? "null" : "static (target, context) => { " + string.Join(" ", calls) + " }";
    }

    private static IEnumerable<INamedTypeSymbol> Ancestors(INamedTypeSymbol type)
    {
        for (INamedTypeSymbol? current = type; current is not null && current.SpecialType != SpecialType.System_Object; current = current.BaseType)
            yield return current;
    }

    private static IEnumerable<INamedTypeSymbol> EnumerateTypes(INamespaceOrTypeSymbol scope)
    {
        foreach (ISymbol member in scope.GetMembers())
        {
            if (member is INamedTypeSymbol type)
                yield return type;
            if (member is INamespaceOrTypeSymbol nested)
                foreach (INamedTypeSymbol nestedType in EnumerateTypes(nested))
                    yield return nestedType;
        }
    }

    private static bool IsSerializable(ITypeSymbol type)
        => type.AllInterfaces.Any(static contract => contract.ToDisplayString() == C_SERIALIZABLE);

    private static bool ContainsParameters(ITypeSymbol type)
        => type.TypeKind == TypeKind.TypeParameter || type is INamedTypeSymbol named && (named.TypeArguments.Any(ContainsParameters)
            || named.ContainingType is not null && ContainsParameters(named.ContainingType))
            || type is IArrayTypeSymbol array && ContainsParameters(array.ElementType);

    private static bool IsAccessible(
        ITypeSymbol type,
        IAssemblySymbol assembly
    ) {
        if (type is IArrayTypeSymbol array)
            return IsAccessible(array.ElementType, assembly);
        if (type is not INamedTypeSymbol named)
            return false;
        for (INamedTypeSymbol? current = named; current is not null; current = current.ContainingType)
            if (current.IsFileLocal || current.DeclaredAccessibility != Accessibility.Public
                && (current.ContainingAssembly is null || !SymbolEqualityComparer.Default.Equals(current.ContainingAssembly, assembly)
                    || current.DeclaredAccessibility is not (Accessibility.Internal or Accessibility.ProtectedOrInternal)))
                return false;
        return named.TypeArguments.All(argument => argument.TypeKind == TypeKind.TypeParameter || IsAccessible(argument, assembly));
    }

    private static string Name(ITypeSymbol type) => type.WithNullableAnnotation(NullableAnnotation.NotAnnotated)
        .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    private static string Quote(string value) => SymbolDisplay.FormatLiteral(value, quote: true);
}
