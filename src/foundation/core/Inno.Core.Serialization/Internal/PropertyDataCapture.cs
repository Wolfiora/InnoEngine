using System;
using System.Collections.Generic;

namespace Inno.Core.Serialization;

internal sealed class PropertyDataCapture(KeyValuePair<string, SerializationNode>[] members)
{
    internal KeyValuePair<string, SerializationNode>[] members { get; } = members;
}
