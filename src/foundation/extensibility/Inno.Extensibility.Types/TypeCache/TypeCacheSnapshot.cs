using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inno.Extensibility.Catalogs;

namespace Inno.Extensibility.Types;

/// <summary>
/// Represents an immutable, internally consistent view of discoverable runtime types.
/// </summary>
/// <remarks>
/// A snapshot strongly retains its discovered <see cref="Type"/> instances. Callers must not cache an
/// obsolete snapshot or one of its type lists beyond the operation that needs generation consistency,
/// because doing so delays unloading the corresponding collectible assembly load context.
/// </remarks>
public sealed class TypeCacheSnapshot
{
    internal static TypeCacheSnapshot empty { get; } = CreateEmpty();

    private readonly ITypeCatalogSource? m_source;
    private readonly Dictionary<Type, TypeCatalogMetadata> m_metadata;
    private readonly Type[] m_types;
    private readonly IReadOnlyList<TypeRef> m_typeRefs;
    private readonly Dictionary<Assembly, Type[]> m_typesByAssembly;
    private readonly TypeIdentityRegistry m_identityRegistry;
    private readonly TypeQueryRegistry m_queryRegistry;

    private TypeCacheSnapshot(
        long version,
        ITypeCatalogSource? source,
        Dictionary<Type, TypeCatalogMetadata> metadata,
        Type[] types,
        Dictionary<Assembly, Type[]> typesByAssembly,
        TypeIdentityRegistry identityRegistry,
        TypeQueryRegistry queryRegistry
    ) {
        this.version = version;
        m_source = source;
        m_metadata = metadata;
        m_types = types;
        m_typeRefs = Array.AsReadOnly(types.Select(identityRegistry.GetTypeRef).ToArray());
        m_typesByAssembly = typesByAssembly;
        m_identityRegistry = identityRegistry;
        m_queryRegistry = queryRegistry;
    }

    /// <summary>
    /// Gets the monotonically increasing catalog version.
    /// </summary>
    public long version { get; }

    /// <summary>
    /// Gets every type included in this snapshot.
    /// </summary>
    public IReadOnlyList<TypeRef> types => m_typeRefs;

    /// <summary>
    /// Gets all concrete discovered types assignable to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The base type used to select assignable concrete types.
    /// </typeparam>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public IReadOnlyList<TypeRef> GetSubTypesOf<T>() => m_queryRegistry.GetSubTypesOf<T>(m_identityRegistry);

    /// <summary>
    /// Gets all concrete discovered types implementing <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">
    /// The interface contract implemented by every selected concrete type.
    /// </typeparam>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public IReadOnlyList<TypeRef> GetTypesImplementing<TInterface>()
        => m_queryRegistry.GetTypesImplementing<TInterface>(m_identityRegistry);

    /// <summary>
    /// Gets all concrete discovered types marked with <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TAttribute">
    /// The attribute type required on every selected concrete type.
    /// </typeparam>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public IReadOnlyList<TypeRef> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        => m_queryRegistry.GetTypesWithAttribute<TAttribute>(m_identityRegistry);

    /// <summary>
    /// Gets the reference for a CLR type in this snapshot.
    /// </summary>
    /// <returns>
    /// The logical and generation-local identity of <paramref name="type"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="type"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the type does not belong to this snapshot.
    /// </exception>
    /// <param name="type">
    /// The type consumed by get type ref; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public TypeRef GetTypeRef(Type type) => m_identityRegistry.GetTypeRef(type);

    /// <summary>
    /// Tries to get the reference for a CLR type in this snapshot.
    /// </summary>
    /// <param name="type">
    /// The CLR type to identify.
    /// </param>
    /// <param name="typeRef">
    /// Receives its logical and generation-local identity.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the type belongs to this snapshot.
    /// </returns>
    public bool TryGetTypeRef(
        Type type,
        out TypeRef typeRef
    ) => m_identityRegistry.TryGetTypeRef(type, out typeRef);

    /// <summary>
    /// Determines whether the current deployment can construct a type in this exact generation.
    /// </summary>
    /// <param name="type">
    /// The type reference to resolve against this snapshot.
    /// </param>
    /// <returns>
    /// Whether a declared type resolves and has an available parameterless factory.
    /// </returns>
    public bool CanCreateInstance(TypeRef type)
        => TryResolve(type, out Type? runtimeType) && m_source!.CanCreateInstance(runtimeType!);

    /// <summary>
    /// Constructs a declared type through this generation's selected metadata source.
    /// </summary>
    /// <param name="type">
    /// A declaration in this snapshot, or a closed construction of its registered generic declaration.
    /// </param>
    /// <returns>
    /// The new instance owned by the caller.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The reference does not resolve or its deployment does not provide a factory.
    /// </exception>
    public object CreateInstance(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Type declaration = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        if (!m_identityRegistry.TryGetTypeRef(declaration, out _))
            throw new InvalidOperationException($"Type '{type}' does not belong to this generation.");
        return m_source!.CreateInstance(type);
    }

    /// <summary>
    /// Resolves a generic construction belonging to this exact generation.
    /// </summary>
    /// <param name="definition">
    /// A registered generic declaration.
    /// </param>
    /// <param name="arguments">
    /// Ordered closed type arguments.
    /// </param>
    /// <returns>
    /// The resolved closed type, or null when generic constraints reject the arguments.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The declaration does not belong to this generation.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// This deployment has not linked the requested construction.
    /// </exception>
    public Type? ConstructGenericType(
        Type definition,
        IReadOnlyList<Type> arguments
    ) {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(arguments);
        if (!m_identityRegistry.TryGetTypeRef(definition, out _))
            throw new InvalidOperationException($"Type '{definition}' does not belong to this generation.");
        return m_source!.ConstructGenericType(definition, arguments);
    }

    /// <summary>
    /// Reads the immutable discovery facts for a declaration in this exact generation.
    /// </summary>
    /// <param name="type">
    /// The logical declaration to resolve against this snapshot.
    /// </param>
    /// <returns>
    /// Complete metadata; stale or unregistered declarations fail during resolution.
    /// </returns>
    public TypeCatalogMetadata GetMetadata(TypeRef type) => m_metadata[type.Resolve(this)];

    /// <summary>
    /// Reads a single extension attribute without requiring runtime reflection.
    /// </summary>
    /// <typeparam name="TAttribute">
    /// The attribute contract requested by the caller.
    /// </typeparam>
    /// <param name="type">
    /// A declaration in this snapshot.
    /// </param>
    /// <param name="inherit">
    /// Whether effective inherited attributes participate.
    /// </param>
    /// <returns>
    /// The matching attribute, or null when absent; duplicate singleton attributes throw.
    /// </returns>
    public TAttribute? GetAttribute<TAttribute>(
        TypeRef type,
        bool inherit = true
    ) where TAttribute : Attribute
    {
        TypeCatalogMetadata metadata = GetMetadata(type);
        return (inherit ? metadata.inheritedAttributes : metadata.declaredAttributes)
            .OfType<TAttribute>().SingleOrDefault();
    }

    internal IReadOnlyList<Type> runtimeTypes => m_types;

    internal bool TryResolve(
        TypeRef typeRef,
        out Type? type
    ) => m_identityRegistry.TryResolveType(typeRef, out type);

    internal static TypeCacheSnapshot Build(
        IEnumerable<Assembly> assemblies,
        ITypeCatalogSource source,
        TypeCacheSnapshot? previous,
        long version
    ) {
        ArgumentNullException.ThrowIfNull(assemblies);

        var discoveredTypes = new List<Type>();
        var typesByAssembly = new Dictionary<Assembly, Type[]>(ReferenceEqualityComparer.Instance);
        var loaderExceptions = new List<Exception>();
        foreach (Assembly assembly in assemblies.Where(static value => !value.IsDynamic))
        {
            if (typesByAssembly.ContainsKey(assembly))
                continue;
            if (previous is not null && previous.m_typesByAssembly.TryGetValue(assembly, out Type[]? cachedTypes))
            {
                typesByAssembly.Add(assembly, cachedTypes);
                discoveredTypes.AddRange(cachedTypes);
                continue;
            }

            try
            {
                Type[] assemblyTypes = source.GetTypes(assembly).ToArray();
                typesByAssembly.Add(assembly, assemblyTypes);
                discoveredTypes.AddRange(assemblyTypes);
            }
            catch (ReflectionTypeLoadException exception)
            {
                Type[] assemblyTypes = exception.Types.OfType<Type>().ToArray();
                typesByAssembly.Add(assembly, assemblyTypes);
                discoveredTypes.AddRange(assemblyTypes);
                loaderExceptions.AddRange(exception.LoaderExceptions.OfType<Exception>());
            }
            catch (Exception exception)
            {
                loaderExceptions.Add(exception);
            }
        }

        if (loaderExceptions.Count > 0)
        {
            throw new TypeCacheBuildException(
                $"Type discovery failed with {loaderExceptions.Count} loader error(s).",
                loaderExceptions);
        }

        Type[] types = discoveredTypes
            .Distinct()
            .OrderBy(static type => type.Assembly.GetName().Name, StringComparer.Ordinal)
            .ThenBy(static type => type.FullName, StringComparer.Ordinal)
            .ToArray();
        var identities = new TypeIdentityRegistry();
        Dictionary<Type, TypeCatalogMetadata> metadata = types.ToDictionary(static type => type,
            type => previous is not null && previous.m_metadata.TryGetValue(type, out TypeCatalogMetadata? existing)
                ? existing : source.GetMetadata(type));
        identities.Rebuild(metadata.Values, previous?.m_identityRegistry);
        var queries = new TypeQueryRegistry();
        queries.Rebuild(metadata.Values, identities);
        return new TypeCacheSnapshot(version, source, metadata, types, typesByAssembly, identities, queries);
    }

    private static TypeCacheSnapshot CreateEmpty()
    {
        var identities = new TypeIdentityRegistry();
        identities.Rebuild([], previous: null);
        var queries = new TypeQueryRegistry();
        queries.Rebuild([], identities);
        return new TypeCacheSnapshot(0, null, [], [], new Dictionary<Assembly, Type[]>(), identities, queries);
    }
}
