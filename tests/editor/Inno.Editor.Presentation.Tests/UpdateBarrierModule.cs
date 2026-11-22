using System;
using Inno.Editor.Core;

namespace Inno.Editor.Presentation.Tests;

[EditorModule("tests.update-barrier", order: -200)]
public sealed class UpdateBarrierModule : EditorModule
{
    public static bool block;
    public static bool throwWhenRead;
    public static int barrierReadCount;

    public override bool blocksFollowingUpdates
    {
        get
        {
            barrierReadCount++;
            return throwWhenRead ? throw new InvalidOperationException("Injected module barrier failure.") : block;
        }
    }
}
