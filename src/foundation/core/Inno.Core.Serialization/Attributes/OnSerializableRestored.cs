using System;

namespace Inno.Core.Serialization;

/// <summary>
/// Marks one nonvirtual instance method to run after restoration succeeds, optionally receiving its SerializationContext.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class OnSerializableRestored : Attribute;
