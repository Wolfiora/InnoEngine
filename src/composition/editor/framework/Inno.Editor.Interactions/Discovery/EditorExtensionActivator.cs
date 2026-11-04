using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inno.Editor.Core;
using Inno.Core.Diagnostics;
using Inno.Extensibility.Types;

namespace Inno.Editor.Interactions;

internal sealed class EditorExtensionActivator
{
    private readonly TypeCacheSnapshot m_types;
    private readonly Action<object> m_ownCandidate;
    private readonly Dictionary<Type, Diagnostic> m_rejections = [];
    private readonly EditorContext m_context;
    private readonly EditorInteractions m_interactions;
    private readonly Type[] m_moduleTypes;
    private readonly object[] m_hostServices;
    private readonly Dictionary<Type, object> m_instances = [];
    private readonly HashSet<Type> m_constructing = [];
    private readonly Dictionary<Type, bool> m_creatable = [];

    internal EditorExtensionActivator(
        EditorContext context,
        EditorInteractions interactions,
        IEnumerable<Type> moduleTypes,
        TypeCacheSnapshot types,
        IEnumerable<object> hostServices,
        Action<object> ownCandidate,
        IEnumerable<object>? retainedInstances = null
    ) {
        m_ownCandidate = ownCandidate ?? throw new ArgumentNullException(nameof(ownCandidate));
        m_context = context ?? throw new ArgumentNullException(nameof(context));
        m_interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
        m_moduleTypes = moduleTypes
            .OrderBy(static type => type.FullName, StringComparer.Ordinal)
            .ToArray();
        m_hostServices = (hostServices ?? throw new ArgumentNullException(nameof(hostServices))).ToArray();
        m_types = types ?? throw new ArgumentNullException(nameof(types));
        var activeTypeSet = new HashSet<Type>(types.types.Select(typeRef => typeRef.Resolve(types)));
        if (retainedInstances is null)
            return;
        foreach (object instance in retainedInstances)
        {
            Type instanceType = instance.GetType();
            if (activeTypeSet.Contains(instanceType))
                m_instances.TryAdd(instanceType, instance);
        }
    }

    internal IReadOnlyCollection<object> instances => m_instances.Values;

    internal Diagnostic[] rejections => m_rejections.OrderBy(static pair => pair.Key.FullName, StringComparer.Ordinal).Select(static pair => pair.Value).ToArray();

    internal bool CanCreate(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return CanCreate(type, new HashSet<Type>());
    }

    internal EditorModule CreateModule(Type type) => (EditorModule)Create(type, typeof(EditorModule));

    internal TExtension CreateExtension<TExtension>(Type type)
        where TExtension : class
        => (TExtension)Create(type, typeof(TExtension));

    private bool CanCreate(
        Type type,
        HashSet<Type> visiting
    ) {
        if (m_instances.ContainsKey(type))
        {
            return true;
        }

        if (m_creatable.TryGetValue(type, out bool cached))
        {
            return cached;
        }

        ConstructorInfo[] constructors = type.GetConstructors(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (constructors.Length != 1 || !visiting.Add(type))
        {
            return true;
        }

        bool result = true;
        foreach (ParameterInfo parameter in constructors[0].GetParameters())
        {
            Type parameterType = parameter.ParameterType;
            if (parameterType == typeof(EditorContext)
                || IsInteractionContract(parameterType)
                || parameterType == typeof(IEditorDocumentService))
            {
                continue;
            }

            int serviceCount = m_hostServices.Count(parameterType.IsInstanceOfType);
            if (serviceCount != 0)
            {
                continue;
            }

            Type[] matches = m_moduleTypes
                .Where(parameterType.IsAssignableFrom)
                .ToArray();
            if (matches.Length == 0
                || matches.Length == 1 && !CanCreate(matches[0], visiting))
            {
                string reason = matches.Length == 0
                    ? $"No host service or module supplies constructor dependency '{parameterType.FullName}'."
                    : $"Constructor dependency '{parameterType.FullName}' is unavailable: {m_rejections[matches[0]].message}";
                m_rejections[type] = new Diagnostic(
                    "EDITOR-EXTENSION-DEPENDENCY",
                    $"Extension '{type.FullName}' from '{type.Assembly.FullName}' is unavailable in generation {m_types.version}. {reason}",
                    DiagnosticSeverity.Error,
                    semanticId: m_types.GetTypeRef(type).stableId.ToString("D"));
                result = false;
                break;
            }
        }

        _ = visiting.Remove(type);
        m_creatable[type] = result;
        return result;
    }

    private object Create(
        Type type,
        Type contract
    ) {
        if (m_instances.TryGetValue(type, out object? existing))
        {
            if (!contract.IsInstanceOfType(existing))
                throw CreateContractException(type, contract);
            return existing;
        }
        if (type.IsAbstract || !contract.IsAssignableFrom(type))
            throw CreateContractException(type, contract);
        if (!m_constructing.Add(type))
            throw new InvalidOperationException(
                $"Editor extension constructor dependency cycle contains '{type.FullName}'.");

        try
        {
            ConstructorInfo constructor = SelectConstructor(type);
            ParameterInfo[] parameters = constructor.GetParameters();
            var arguments = new object?[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                arguments[i] = Resolve(parameters[i].ParameterType, type);
            object instance;
            try
            {
                instance = constructor.Invoke(arguments);
            }
            catch (TargetInvocationException exception)
            {
                throw new InvalidOperationException(
                    $"Editor extension '{type.FullName}' constructor failed.",
                    exception.InnerException ?? exception);
            }
            m_ownCandidate(instance);
            m_instances.Add(type, instance);
            return instance;
        }
        catch (Exception exception)
        {
            throw new ExtensionActivationException(
                m_types.GetTypeRef(type).stableId,
                type.FullName ?? type.Name,
                type.Assembly.GetName().Name!,
                m_types.version,
                exception);
        }
        finally
        {
            _ = m_constructing.Remove(type);
        }
    }

    private object Resolve(
        Type parameterType,
        Type ownerType
    ) {
        if (parameterType == typeof(EditorContext))
            return m_context;
        if (IsInteractionContract(parameterType))
            return m_interactions;
        if (parameterType == typeof(IEditorDocumentService))
            return m_interactions.documents;

        object[] services = m_hostServices
            .Where(parameterType.IsInstanceOfType)
            .ToArray();
        if (services.Length == 1)
            return services[0];
        if (services.Length > 1)
        {
            throw new InvalidOperationException(
                $"Editor extension '{ownerType.FullName}' has ambiguous host service dependency " +
                $"'{parameterType.FullName}'.");
        }

        Type[] matches = m_moduleTypes
            .Where(parameterType.IsAssignableFrom)
            .ToArray();
        if (matches.Length == 1)
            return CreateModule(matches[0]);
        if (matches.Length == 0)
        {
            throw new InvalidOperationException(
                $"Editor extension '{ownerType.FullName}' requests unsupported constructor dependency " +
                $"'{parameterType.FullName}'. Only EditorContext, EditorInteractions, and discovered " +
                "EditorModule types or explicit host services are injectable.");
        }
        throw new InvalidOperationException(
            $"Editor extension '{ownerType.FullName}' has ambiguous module dependency " +
            $"'{parameterType.FullName}': {string.Join(", ", matches.Select(static type => type.FullName))}.");
    }

    private static ConstructorInfo SelectConstructor(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (constructors.Length == 0)
            throw new InvalidOperationException($"Editor extension '{type.FullName}' has no instance constructor.");
        if (constructors.Length > 1)
        {
            throw new InvalidOperationException(
                $"Editor extension '{type.FullName}' must declare exactly one constructor.");
        }
        return constructors[0];
    }

    private static bool IsInteractionContract(Type type) => type != typeof(object) && type.IsAssignableFrom(typeof(EditorInteractions));

    private static InvalidOperationException CreateContractException(
        Type type,
        Type contract
    )
        => new($"Editor extension '{type.FullName}' must be a non-abstract '{contract.FullName}'.");
}
