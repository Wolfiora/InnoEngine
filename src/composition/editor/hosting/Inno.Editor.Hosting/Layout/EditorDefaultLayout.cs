using System;
using System.IO;

namespace Inno.Editor.Hosting;

internal static class EditorDefaultLayout
{
    private const string C_RESOURCE_NAME = "Inno.Editor.Hosting.DefaultEditorLayout.ini";

    internal static string document { get; } = ReadDocument();

    private static string ReadDocument()
    {
        using Stream stream = typeof(EditorDefaultLayout).Assembly.GetManifestResourceStream(C_RESOURCE_NAME)
            ?? throw new InvalidOperationException("The standard Editor layout resource is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
