using System.Collections.Generic;

namespace Inno.Editor.Presentation.Tests;

internal static class ShutdownOrder
{
    internal static List<string> events { get; } = [];
}
