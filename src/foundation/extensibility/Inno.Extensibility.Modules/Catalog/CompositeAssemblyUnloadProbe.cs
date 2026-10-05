using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

internal sealed class CompositeAssemblyUnloadProbe : IAssemblyUnloadProbe
{
    private readonly IAssemblyUnloadProbe[] m_children;
    internal CompositeAssemblyUnloadProbe(IReadOnlyList<IAssemblyUnloadProbe> children)
        => m_children = children.ToArray();
    /// <inheritdoc />
    public string description => string.Join(", ", m_children.Select(static child => child.description));

    /// <inheritdoc />
    public bool isCompleted => m_children.All(static child => child.isCompleted);
}
