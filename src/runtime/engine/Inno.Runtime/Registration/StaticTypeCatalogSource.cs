using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Types;

using Inno.Extensibility.Catalogs;

namespace Inno.Runtime;

/// <summary>
/// Resolves type metadata and factories from linked registrations without runtime type discovery.
/// </summary>
public sealed class StaticTypeCatalogSource : ITypeCatalogSource
{
    private readonly Dictionary<Assembly, IReadOnlyList<Type>> m_types;
    private readonly Dictionary<Type, Func<object>?> m_factories;
    private readonly Dictionary<Type, TypeCatalogMetadata> m_metadata;
    private readonly Dictionary<Type, Type[]> m_constructions;
    private readonly Dictionary<Type, List<Type[]>> m_rejectedConstructions;

    /// <summary>
    /// Executes generated contributions once and freezes the resulting metadata and factory closure.
    /// </summary>
    /// <param name="catalogs">
    /// Assembly-local registration methods. Callbacks and the registrar are released after construction.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A declaration is registered twice, a generic construction lacks its definition, or a contributor is null.
    /// </exception>
    public StaticTypeCatalogSource(IReadOnlyList<Action<ITypeCatalogRegistrar>> catalogs)
    {
        ArgumentNullException.ThrowIfNull(catalogs);
        var registrations = new Registration();
        var assemblies = new HashSet<Assembly>();
        try
        {
            foreach (Action<ITypeCatalogRegistrar> catalog in catalogs)
            {
                ArgumentNullException.ThrowIfNull(catalog);
                assemblies.Add(catalog.Method.DeclaringType!.Assembly);
                catalog(registrations);
            }
        }
        finally
        {
            registrations.Seal();
        }
        m_factories = registrations.factories;
        m_metadata = registrations.metadata;
        m_rejectedConstructions = registrations.rejectedConstructions;
        foreach (Type type in m_factories.Keys.Where(static type => type.IsConstructedGenericType))
        {
            if (!m_metadata.ContainsKey(type.GetGenericTypeDefinition()))
                throw new ArgumentException($"Linked construction '{type}' has no registered declaration.", nameof(catalogs));
        }
        m_constructions = m_factories.Keys.Where(static type => type.IsConstructedGenericType)
            .GroupBy(static type => type.GetGenericTypeDefinition())
            .ToDictionary(static group => group.Key, static group => group.ToArray());
        foreach ((Type definition, List<Type[]> arguments) in m_rejectedConstructions)
        {
            if (!m_metadata.ContainsKey(definition))
                throw new ArgumentException($"Rejected construction '{definition}' has no registered declaration.", nameof(catalogs));
            if (m_constructions.TryGetValue(definition, out Type[]? constructions) &&
                constructions.Any(construction => arguments.Any(values => construction.GenericTypeArguments.SequenceEqual(values))))
                throw new ArgumentException($"A linked construction of '{definition}' is also recorded as rejected.", nameof(catalogs));
        }
        m_types = m_metadata.Keys.GroupBy(static type => type.Assembly).ToDictionary(
            static group => group.Key,
            static group => (IReadOnlyList<Type>)Array.AsReadOnly(group.OrderBy(static type => type.FullName,
                StringComparer.Ordinal).ToArray()));
        foreach (Assembly assembly in assemblies)
            m_types.TryAdd(assembly, Array.Empty<Type>());
    }

    /// <inheritdoc />
    public IReadOnlyList<Type> GetTypes(Assembly assembly)
        => m_types.TryGetValue(assembly, out IReadOnlyList<Type>? types) ? types
            : throw new InvalidOperationException($"Linked assembly '{assembly.GetName().Name}' has no generated type catalog.");

    /// <inheritdoc />
    public TypeCatalogMetadata GetMetadata(Type type)
        => m_metadata.TryGetValue(type, out TypeCatalogMetadata? metadata) ? metadata
            : throw new InvalidOperationException($"Linked declaration '{type}' has no generated metadata.");

    /// <inheritdoc />
    public Type? ConstructGenericType(
        Type definition,
        IReadOnlyList<Type> arguments
    ) {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(arguments);
        ValidateArguments(definition, arguments);
        if (m_rejectedConstructions.TryGetValue(definition, out List<Type[]>? rejected) &&
            rejected.Any(values => values.SequenceEqual(arguments)))
            return null;
        if (m_constructions.TryGetValue(definition, out Type[]? constructions))
        {
            foreach (Type construction in constructions)
            {
                if (construction.GenericTypeArguments.SequenceEqual(arguments))
                    return construction;
            }
        }
        throw new NotSupportedException($"Generic construction '{definition}' with arguments " +
            $"'{string.Join(", ", arguments)}' is absent from the linked factory catalog.");
    }

    /// <inheritdoc />
    public bool CanCreateInstance(Type type)
        => m_factories.TryGetValue(type, out Func<object>? factory) && factory is not null;

    /// <inheritdoc />
    public object CreateInstance(Type type)
    {
        if (!m_factories.TryGetValue(type, out Func<object>? factory) || factory is null)
            throw new InvalidOperationException($"Linked type '{type}' has no generated parameterless factory.");
        object instance = factory();
        return type.IsInstanceOfType(instance) ? instance
            : throw new InvalidOperationException($"The linked factory for '{type}' returned an incompatible instance.");
    }

    private static void ValidateArguments(
        Type definition,
        IReadOnlyList<Type> arguments
    ) {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(arguments);
        if (!definition.IsGenericTypeDefinition)
            throw new ArgumentException("The declaration must be a generic type definition.", nameof(definition));
        if (arguments.Count != definition.GetGenericArguments().Length ||
            arguments.Any(static argument => argument is null || argument.ContainsGenericParameters))
            throw new ArgumentException("The complete closed type argument set is required.", nameof(arguments));
    }

    private sealed class Registration : ITypeCatalogRegistrar
    {
        internal readonly Dictionary<Type, Func<object>?> factories = [];
        internal readonly Dictionary<Type, TypeCatalogMetadata> metadata = [];
        internal readonly Dictionary<Type, List<Type[]>> rejectedConstructions = [];
        private bool m_sealed;
        private readonly int m_ownerThread = Environment.CurrentManagedThreadId;

        /// <inheritdoc />
        public void Register(
            TypeCatalogMetadata metadata,
            Func<object>? factory
        ) {
            RequireOpen();
            ArgumentNullException.ThrowIfNull(metadata);
            if (!this.metadata.TryAdd(metadata.type, metadata))
                throw new ArgumentException($"Static declaration '{metadata.type}' is registered more than once.");
            factories.Add(metadata.type, factory);
        }

        /// <inheritdoc />
        public void RegisterFactory(
            Type type,
            Func<object> factory
        ) {
            RequireOpen();
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(factory);
            if (!type.IsConstructedGenericType || type.ContainsGenericParameters)
                throw new ArgumentException("A linked factory requires a fully closed generic type.", nameof(type));
            // Different assembly-local catalogs can link the same closed construction.
            factories.TryAdd(type, factory);
        }

        /// <inheritdoc />
        public void RejectGenericConstruction(
            Type definition,
            IReadOnlyList<Type> arguments
        ) {
            RequireOpen();
            ValidateArguments(definition, arguments);
            if (!rejectedConstructions.TryGetValue(definition, out List<Type[]>? rejected))
                rejectedConstructions.Add(definition, rejected = []);
            if (!rejected.Any(values => values.SequenceEqual(arguments)))
                rejected.Add(arguments.ToArray());
        }

        internal void Seal() => m_sealed = true;

        private void RequireOpen()
        {
            if (Environment.CurrentManagedThreadId != m_ownerThread)
                throw new InvalidOperationException("Type catalog contributions must run on the composition thread.");
            if (m_sealed)
                throw new InvalidOperationException("The type catalog contribution has already completed.");
        }
    }
}
