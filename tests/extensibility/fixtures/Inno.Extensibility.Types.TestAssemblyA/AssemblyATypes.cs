using System;

namespace Inno.Extensibility.Types.TestAssets.A
{
    /// <summary>
    /// Marks declarations from fixture assembly A for source-scoped discovery tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AssemblyAMarkerAttribute : Attribute;

    /// <summary>
    /// Provides a marked declaration whose visibility follows fixture assembly A's active source registration.
    /// </summary>
    [AssemblyAMarker]
    public sealed class AssemblyAMarkedType;
}
