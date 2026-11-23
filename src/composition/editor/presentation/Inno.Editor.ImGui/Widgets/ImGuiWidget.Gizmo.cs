using System;
using Inno.Native.ImGuizmo;

using EngineMatrix = Inno.Core.Mathematics.Matrix;
using NativeGizmo = Inno.Native.ImGuizmo.ImGuizmo;
using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeOperation = Inno.Native.ImGuizmo.ImGuizmoOperation;
using NativeSpace = Inno.Native.ImGuizmo.ImGuizmoMode;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Owns session-local controls, native adaptation and reversible edit gestures.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Draws a transform handle using stack-owned ABI buffers and restores native global style and masks before returning.
    /// </summary>
    /// <param name="world">
    /// The current world matrix, copied into the native call.
    /// </param>
    /// <param name="options">
    /// The explicit viewport and interaction policy.
    /// </param>
    /// <returns>
    /// Copied output with no native pointer or domain object.
    /// </returns>
    public TransformGizmoResult TransformGizmo(
        EngineMatrix world,
        TransformGizmoOptions options
    ) {
        m_session.EnsureDrawing();
        float* view = stackalloc float[16];
        float* projection = stackalloc float[16];
        float* model = stackalloc float[16];
        WriteColumnMajor(options.view, view);
        WriteColumnMajor(options.projection, projection);
        WriteColumnMajor(world, model);
        NativeOperation operation = ToNativeOperation(options.operation);
        NativeGizmo.BeginFrame();
        NativeGizmo.Raw.SetDrawlist((ImDrawList*)(nint)NativeImGui.Raw.GetWindowDrawList());
        NativeGizmo.SetRect(
            options.minimum.X,
            options.minimum.Y,
            MathF.Max(1f, options.maximum.X - options.minimum.X),
            MathF.Max(1f, options.maximum.Y - options.minimum.Y)
        );
        NativeGizmo.SetOrthographic(options.orthographic);
        NativeGizmo.SetGizmoSizeClipSpace(0.21f);
        Style* style = NativeGizmo.Raw.GetStyle();
        Style previousStyle = *style;
        try
        {
            style->TranslationLineThickness = 3.5f;
            style->TranslationLineArrowSize = 9f;
            style->RotationLineThickness = 3f;
            style->RotationOuterLineThickness = 4f;
            style->ScaleLineThickness = 3.5f;
            style->ScaleLineCircleSize = 9f;
            style->CenterCircleSize = 8f;
            NativeGizmo.Enable(options.enabled);
            NativeGizmo.SetAxisMask(options.maskX, options.maskY, options.maskZ);
            NativeGizmo.SetAxisLimit(options.maskX || options.maskY || options.maskZ ? float.PositiveInfinity : 0.0025f);
            bool changed = NativeGizmo.Raw.Manipulate(
                view,
                projection,
                operation,
                options.space == TransformGizmoSpace.Local ? NativeSpace.Local : NativeSpace.World,
                model,
                null,
                null,
                null,
                null
            ) != 0;
            return new TransformGizmoResult(ReadColumnMajor(model), changed, NativeGizmo.IsUsing(), NativeGizmo.IsOver(operation));
        }
        finally
        {
            *style = previousStyle;
            NativeGizmo.Enable(true);
            NativeGizmo.SetAxisMask(false, false, false);
            NativeGizmo.SetAxisLimit(0.0025f);
        }
    }

    private static NativeOperation ToNativeOperation(TransformGizmoOperation operation)
    {
        if (operation == TransformGizmoOperation.Translate)
        {
            return NativeOperation.Translate;
        }
        if (operation == TransformGizmoOperation.Rotate)
        {
            return NativeOperation.Rotate;
        }
        NativeOperation result = 0;
        if (operation.HasFlag(TransformGizmoOperation.ScaleX))
        {
            result |= NativeOperation.ScaleX;
        }
        if (operation.HasFlag(TransformGizmoOperation.ScaleY))
        {
            result |= NativeOperation.ScaleY;
        }
        if (operation.HasFlag(TransformGizmoOperation.ScaleZ))
        {
            result |= NativeOperation.ScaleZ;
        }
        if (result == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(operation));
        }
        return result;
    }

    private static unsafe void WriteColumnMajor(
        EngineMatrix matrix,
        float* destination
    ) {
        destination[0] = matrix.m11;
        destination[1] = matrix.m21;
        destination[2] = matrix.m31;
        destination[3] = matrix.m41;
        destination[4] = matrix.m12;
        destination[5] = matrix.m22;
        destination[6] = matrix.m32;
        destination[7] = matrix.m42;
        destination[8] = matrix.m13;
        destination[9] = matrix.m23;
        destination[10] = matrix.m33;
        destination[11] = matrix.m43;
        destination[12] = matrix.m14;
        destination[13] = matrix.m24;
        destination[14] = matrix.m34;
        destination[15] = matrix.m44;
    }

    private static unsafe EngineMatrix ReadColumnMajor(float* source)
        => new(
            source[0],
            source[4],
            source[8],
            source[12],
            source[1],
            source[5],
            source[9],
            source[13],
            source[2],
            source[6],
            source[10],
            source[14],
            source[3],
            source[7],
            source[11],
            source[15]
        );
}

/// <summary>
/// Chooses the transform handles rendered by the viewport presenter.
/// </summary>
[Flags]
public enum TransformGizmoOperation
{
    /// <summary>
    /// Renders translation handles.
    /// </summary>
    Translate = 1,
    /// <summary>
    /// Renders rotation handles.
    /// </summary>
    Rotate = 2,
    /// <summary>
    /// Renders the X scale handle.
    /// </summary>
    ScaleX = 4,
    /// <summary>
    /// Renders the Y scale handle.
    /// </summary>
    ScaleY = 8,
    /// <summary>
    /// Renders the Z scale handle.
    /// </summary>
    ScaleZ = 16,
    /// <summary>
    /// Renders all local scale handles.
    /// </summary>
    Scale = ScaleX | ScaleY | ScaleZ
}

/// <summary>
/// Chooses how the manipulation axes are oriented.
/// </summary>
public enum TransformGizmoSpace
{
    /// <summary>
    /// Uses the object's local orientation.
    /// </summary>
    Local,
    /// <summary>
    /// Uses the world's orientation.
    /// </summary>
    World
}

/// <summary>
/// Returns copied manipulation output; the domain owner decides how to apply and record it.
/// </summary>
/// <param name="matrix">
/// The resulting world matrix.
/// </param>
/// <param name="changed">
/// Whether the native presenter reported an edited value.
/// </param>
/// <param name="isUsing">
/// Whether the handle currently captures the pointer.
/// </param>
/// <param name="isOver">
/// Whether the pointer is over a handle.
/// </param>
public readonly record struct TransformGizmoResult(
    EngineMatrix matrix,
    bool changed,
    bool isUsing,
    bool isOver
);
