using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.ExceptionServices;
using Inno.Extensibility.Types;

using Inno.Extensibility.Catalogs;

namespace Inno.Adapter.Modules.DotNet;

/// <summary>
/// Discovers desktop authoring types through the selected managed runtime's reflection service.
/// </summary>
public sealed class ReflectionTypeCatalogSource : ITypeCatalogSource
{
    /// <inheritdoc />
    public IReadOnlyList<Type> GetTypes(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        return assembly.GetTypes();
    }

    /// <inheritdoc />
    public TypeCatalogMetadata GetMetadata(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var ancestors = new List<Type>();
        for (Type? current = type.BaseType; current is not null && current != typeof(object); current = current.BaseType)
            ancestors.Add(current);
        return new TypeCatalogMetadata(type, ancestors, type.GetInterfaces(),
            type.GetCustomAttributes(inherit: false).OfType<Attribute>().ToArray(),
            type.GetCustomAttributes(inherit: true).OfType<Attribute>().ToArray(),
            type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(static method => !method.IsGenericMethod && method.GetParameters().Length == 0 &&
                    method.GetBaseDefinition().DeclaringType != method.DeclaringType)
                .Select(static method => (method.Name, method.GetBaseDefinition().DeclaringType!)).ToArray());
    }
    /// <inheritdoc />
    public Type? ConstructGenericType(
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
        try
        {
            return definition.MakeGenericType(arguments.ToArray());
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public bool CanCreateInstance(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return !type.IsAbstract && !type.ContainsGenericParameters &&
            (type.IsValueType || type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null, Type.EmptyTypes, modifiers: null) is not null);
    }

    /// <inheritdoc />
    public object CreateInstance(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        try
        {
            return Activator.CreateInstance(type, nonPublic: true)
                ?? throw new InvalidOperationException($"Type '{type}' did not produce an instance.");
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }
}
