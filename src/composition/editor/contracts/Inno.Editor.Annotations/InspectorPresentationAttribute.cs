using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Provides editor-only presentation metadata for one serialized field or property.
/// </summary>
/// <remarks>
/// Presentation attributes never participate in serialization. Concrete attributes are conditional on
/// INNO_EDITOR, so their applications are omitted from Player script assemblies.
/// </remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
[Conditional("INNO_EDITOR")]
public abstract class InspectorPresentationAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the relative order among presentation attributes declared on the same member.
    /// </summary>
    public int order { get; set; }
}
