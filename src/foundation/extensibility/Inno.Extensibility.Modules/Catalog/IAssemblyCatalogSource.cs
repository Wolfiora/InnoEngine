using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Owns the host assembly source independently of any module loading implementation.
/// </summary>
public interface IAssemblyCatalogSource : IDisposable
{
    /// <summary>
    /// Notifies the owner that the host assembly snapshot should be refreshed at a safe point.
    /// </summary>
    event Action? changed;
    /// <summary>
    /// Gets the complete host assemblies that participate in extension discovery.
    /// </summary>
    /// <returns>
    /// The current host snapshot; the caller must not retain it beyond its catalog generation.
    /// </returns>
    IReadOnlyList<Assembly> GetAssemblies();
    /// <summary>
    /// Gets host contracts and framework assemblies available to candidate modules.
    /// </summary>
    /// <returns>
    /// A snapshot of shareable assemblies owned by the host implementation.
    /// </returns>
    IReadOnlyList<Assembly> GetSharedAssemblies();
    /// <summary>
    /// Determines whether a referenced simple name belongs to the selected managed framework.
    /// </summary>
    /// <param name="assemblyName">
    /// The referenced managed assembly simple name.
    /// </param>
    /// <returns>
    /// Whether the framework owns this dependency.
    /// </returns>
    bool IsFrameworkReference(string assemblyName);
    /// <summary>
    /// Determines whether an assembly is a framework contract rather than a contributed module.
    /// </summary>
    /// <param name="assembly">
    /// The candidate shared assembly.
    /// </param>
    /// <returns>
    /// Whether the assembly is owned by the selected managed framework.
    /// </returns>
    bool IsFrameworkAssembly(Assembly assembly);
}
