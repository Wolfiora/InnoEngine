using System.Collections.Generic;

namespace Inno.Editor.Scripting.Tests;

public sealed partial class ShaderEditorWorkflowTests
{
    public static IEnumerable<object[]> CanvasMetrics()
    {
        foreach (float zoom in new[] { 0.75f, 0.85f, 0.90f, 1f, 1.10f, 1.25f, 1.50f })
            foreach (float density in new[] { 1f, 1.25f, 1.50f, 2f })
                foreach ((float width, float height) in new[] { (1280f, 720f), (1920f, 1080f) })
                    foreach (bool emptyGraph in new[] { false, true })
                        yield return [zoom, density, width, height, emptyGraph];
    }
}
