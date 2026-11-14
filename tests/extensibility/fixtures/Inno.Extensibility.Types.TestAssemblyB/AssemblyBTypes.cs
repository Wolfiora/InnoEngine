using System;

namespace Inno.Extensibility.Types.TestAssets.B
{
    /// <summary>
    /// Marks declarations from fixture assembly B independently of fixture assembly A.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AssemblyBMarkerAttribute : Attribute;

    /// <summary>
    /// Provides a distinct marked declaration for combined-source discovery and source-removal tests.
    /// </summary>
    [AssemblyBMarker]
    public sealed class AssemblyBMarkedType;
}
