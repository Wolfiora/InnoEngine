using System;
using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection.Native.Tests;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class ProbeInspectorAttribute : InspectorPresentationAttribute;
