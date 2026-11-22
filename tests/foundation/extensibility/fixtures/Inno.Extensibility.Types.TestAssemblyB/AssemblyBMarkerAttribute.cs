using System;

namespace Inno.Extensibility.Types.TestAssets.B;

/// <summary>
/// Marks declarations from fixture assembly B independently of fixture assembly A.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class AssemblyBMarkerAttribute : Attribute;
