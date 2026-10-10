using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Describes the ownership classification of one planned assembly without loading its code.
/// </summary>
/// <param name="domain">
/// The owning extension domain.
/// </param>
/// <param name="scope">
/// The dependency scope declared by the module source.
/// </param>
public readonly record struct ModuleAssemblyDescriptor(
    AssemblyDomain domain,
    AssemblyScope scope
);
