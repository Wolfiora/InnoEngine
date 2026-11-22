using Inno.Core.Mathematics;
using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

public sealed class TestViewportCoordinates : IEditorViewportCoordinateConverter
{
    public Vector2 ScreenToWorld(Vector2 screenPosition) => screenPosition * 2f;

    public Vector2 WorldToScreen(Vector2 worldPosition) => worldPosition / 2f;
}
