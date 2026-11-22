using System.Collections.Generic;

namespace Inno.Editor.Shaders.Native.Tests;

public sealed partial class ShaderEditorWorkflowTests
{
    public static IEnumerable<object[]> CanvasMetrics()
    {
        foreach (float zoom in new[] { 0.75f, 0.85f, 0.9f, 1f, 1.1f, 1.25f, 1.5f })
        {
            foreach (float density in new[] { 1f, 1.25f, 1.5f, 2f })
            {
                foreach ((float width, float height) in new[] { (1280f, 720f), (1920f, 1080f) })
                {
                    foreach (bool emptyGraph in new[] { false, true })
                    {
                        yield return [zoom, density, width, height, emptyGraph];
                    }
                }
            }
        }
    }
}
