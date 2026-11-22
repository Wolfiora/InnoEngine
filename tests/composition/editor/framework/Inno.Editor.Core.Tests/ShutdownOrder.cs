using System.Collections.Generic;

namespace Inno.Editor.Core.Tests;

internal static class ShutdownOrder
{
    internal static List<string> events { get; } = [];
}
