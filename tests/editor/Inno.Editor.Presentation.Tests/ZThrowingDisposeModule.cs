using System;
using Inno.Editor.Core;

namespace Inno.Editor.Presentation.Tests;

[EditorModule("tests.throwing-dispose", order: 901)]
public sealed class ZThrowingDisposeModule : EditorModule
{
    public static bool throwOnDispose;
    public static int disposeCount;

    protected override void OnDispose()
    {
        disposeCount++;
        if (throwOnDispose)
        {
            throw new InvalidOperationException("Injected extension disposal failure.");
        }
    }
}
