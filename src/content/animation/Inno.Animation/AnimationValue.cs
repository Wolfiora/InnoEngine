using System;
using Inno.Core.Serialization;

namespace Inno.Animation;

/// <summary>
/// Stores one backend-neutral scalar, vector, or quaternion animation value.
/// </summary>
public struct AnimationValue
{
    /// <summary>
    /// Creates a four-component animation value.
    /// </summary>
    /// <param name="kind">
    /// Interpretation of the populated components.
    /// </param>
    /// <param name="x">
    /// First component.
    /// </param>
    /// <param name="y">
    /// Second component.
    /// </param>
    /// <param name="z">
    /// Third component.
    /// </param>
    /// <param name="w">
    /// Fourth component.
    /// </param>
    public AnimationValue(
        AnimationValueKind kind,
        float x,
        float y = 0f,
        float z = 0f,
        float w = 0f
    ) {
        this.kind = kind;
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    /// <summary>
    /// Gets or sets the component interpretation.
    /// </summary>
    [SerializableProperty]
    public AnimationValueKind kind { get; set; }

    /// <summary>
    /// Gets or sets the first component.
    /// </summary>
    [SerializableProperty]
    public float x { get; set; }

    /// <summary>
    /// Gets or sets the second component.
    /// </summary>
    [SerializableProperty]
    public float y { get; set; }

    /// <summary>
    /// Gets or sets the third component.
    /// </summary>
    [SerializableProperty]
    public float z { get; set; }

    /// <summary>
    /// Gets or sets the fourth component.
    /// </summary>
    [SerializableProperty]
    public float w { get; set; }

    internal static AnimationValue Lerp(
        AnimationValue left,
        AnimationValue right,
        float amount
    ) {
        if (left.kind != right.kind)
        {
            throw new InvalidOperationException("Animation values with different kinds cannot be interpolated.");
        }
        float t = Math.Clamp(amount, 0f, 1f);
        var value = new AnimationValue(
            left.kind,
            left.x + ((right.x - left.x) * t),
            left.y + ((right.y - left.y) * t),
            left.z + ((right.z - left.z) * t),
            left.w + ((right.w - left.w) * t)
        );
        return left.kind == AnimationValueKind.Quaternion ? NormalizeQuaternion(value) : value;
    }

    internal static AnimationValue WeightedAverage(
        AnimationValue sum,
        float inverseWeight
    ) {
        var value = new AnimationValue(
            sum.kind,
            sum.x * inverseWeight,
            sum.y * inverseWeight,
            sum.z * inverseWeight,
            sum.w * inverseWeight
        );
        return sum.kind == AnimationValueKind.Quaternion ? NormalizeQuaternion(value) : value;
    }

    internal static AnimationValue AddWeighted(
        AnimationValue sum,
        AnimationValue value,
        float weight
    ) {
        if (sum.kind != value.kind)
        {
            throw new InvalidOperationException("Animation tracks targeting one binding must use one value kind.");
        }
        sum.x += value.x * weight;
        sum.y += value.y * weight;
        sum.z += value.z * weight;
        sum.w += value.w * weight;
        return sum;
    }

    private static AnimationValue NormalizeQuaternion(AnimationValue value)
    {
        float length = MathF.Sqrt(
            (value.x * value.x)
            + (value.y * value.y)
            + (value.z * value.z)
            + (value.w * value.w)
        );
        return length <= float.Epsilon
               ? new AnimationValue(AnimationValueKind.Quaternion, 0f, 0f, 0f, 1f)
               : new AnimationValue(
                   AnimationValueKind.Quaternion,
                   value.x / length,
                   value.y / length,
                   value.z / length,
                   value.w / length
               );
    }
}

/// <summary>
/// Describes the number and interpretation of components in an animation value.
/// </summary>
public enum AnimationValueKind
{
    /// <summary>
    /// A single scalar component.
    /// </summary>
    Scalar,
    /// <summary>
    /// Two independent components.
    /// </summary>
    Vector2,
    /// <summary>
    /// Three independent components.
    /// </summary>
    Vector3,
    /// <summary>
    /// Four independent components.
    /// </summary>
    Vector4,
    /// <summary>
    /// Four normalized rotation components.
    /// </summary>
    Quaternion
}
