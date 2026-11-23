using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Makes an otherwise writable serialized property read-only in the Inspector.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class InspectorReadOnlyAttribute : InspectorPresentationAttribute;
