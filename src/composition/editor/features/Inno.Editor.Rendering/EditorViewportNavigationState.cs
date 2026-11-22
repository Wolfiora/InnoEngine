using System;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Stores rendering-model-neutral navigation state for one Editor viewport across provider reloads.
/// </summary>
public sealed class EditorViewportNavigationState
{
    private const float C_MINIMUM_ORTHOGRAPHIC_SIZE = 0.001f;
    private const float C_MINIMUM_FIELD_OF_VIEW = 1f;
    private const float C_MAXIMUM_FIELD_OF_VIEW = 179f;

    private Vector3 m_position;
    private Quaternion m_rotation = Quaternion.identity;
    private Vector3 m_pivot;
    private EditorViewportProjection m_projection = EditorViewportProjection.Orthographic;
    private EditorViewportNavigationMode m_mode = EditorViewportNavigationMode.Planar;
    private float m_orthographicSize = 5f;
    private float m_fieldOfView = 60f;
    private float m_nearClip = 0.01f;
    private float m_farClip = 1000f;
    private float m_focusDistance = 10f;
    private float m_movementSpeed = 5f;

    /// <summary>
    /// Gets whether a provider or restored panel state initialized this state.
    /// </summary>
    public bool isInitialized { get; private set; }

    /// <summary>
    /// Gets or sets the current projection family.
    /// </summary>
    public EditorViewportProjection projection
    {
        get => m_projection;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_projection = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the active host navigation mode.
    /// </summary>
    public EditorViewportNavigationMode mode
    {
        get => m_mode;
        set
        {
            if (!Enum.IsDefined(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_mode = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the provider-defined world-space view position.
    /// </summary>
    public Vector3 position
    {
        get => m_position;
        set
        {
            ValidateFinite(value, nameof(value));
            m_position = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the normalized provider-defined world-space view rotation.
    /// </summary>
    public Quaternion rotation
    {
        get => m_rotation;
        set
        {
            ValidateFinite(value, nameof(value));
            if (value.LengthSquared() < 0.000001f)
            {
                throw new ArgumentException("Navigation rotation must have a usable magnitude.", nameof(value));
            }
            m_rotation = value.normalized;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the world-space orbit and framing pivot.
    /// </summary>
    public Vector3 pivot
    {
        get => m_pivot;
        set
        {
            ValidateFinite(value, nameof(value));
            m_pivot = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the positive orthographic half-height in provider world units.
    /// </summary>
    public float orthographicSize
    {
        get => m_orthographicSize;
        set
        {
            if (!float.IsFinite(value) || value < C_MINIMUM_ORTHOGRAPHIC_SIZE)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_orthographicSize = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the perspective vertical field of view in degrees.
    /// </summary>
    public float fieldOfView
    {
        get => m_fieldOfView;
        set
        {
            if (!float.IsFinite(value) || value < C_MINIMUM_FIELD_OF_VIEW || value > C_MAXIMUM_FIELD_OF_VIEW)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_fieldOfView = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the positive near clipping distance.
    /// </summary>
    public float nearClip
    {
        get => m_nearClip;
        set
        {
            if (!float.IsFinite(value) || value <= 0f || value >= m_farClip)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_nearClip = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the far clipping distance greater than the near distance.
    /// </summary>
    public float farClip
    {
        get => m_farClip;
        set
        {
            if (!float.IsFinite(value) || value <= m_nearClip)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_farClip = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the positive orbit and framing distance.
    /// </summary>
    public float focusDistance
    {
        get => m_focusDistance;
        set
        {
            if (!float.IsFinite(value) || value <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_focusDistance = value;
            isInitialized = true;
        }
    }

    /// <summary>
    /// Gets or sets the positive base movement speed in world units per second.
    /// </summary>
    public float movementSpeed
    {
        get => m_movementSpeed;
        set
        {
            if (!float.IsFinite(value) || value <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_movementSpeed = value;
        }
    }

    /// <summary>
    /// Atomically initializes an orthographic navigation view.
    /// </summary>
    /// <param name="position">
    /// Provider-defined world-space position.
    /// </param>
    /// <param name="rotation">
    /// Provider-defined world-space rotation.
    /// </param>
    /// <param name="orthographicSize">
    /// Positive orthographic half-height.
    /// </param>
    public void ConfigureOrthographic(
        Vector3 position,
        Quaternion rotation,
        float orthographicSize
    ) {
        ValidateFinite(position, nameof(position));
        ValidateRotation(rotation, nameof(rotation));
        if (!float.IsFinite(orthographicSize)
            || orthographicSize < C_MINIMUM_ORTHOGRAPHIC_SIZE
        ) {
            throw new ArgumentOutOfRangeException(nameof(orthographicSize));
        }
        m_position = position;
        m_rotation = rotation.normalized;
        m_orthographicSize = orthographicSize;
        m_projection = EditorViewportProjection.Orthographic;
        isInitialized = true;
    }

    /// <summary>
    /// Atomically initializes a perspective navigation view.
    /// </summary>
    /// <param name="position">
    /// Provider-defined world-space position.
    /// </param>
    /// <param name="rotation">
    /// Provider-defined world-space rotation.
    /// </param>
    /// <param name="fieldOfView">
    /// Perspective vertical field of view in degrees.
    /// </param>
    /// <param name="nearClip">
    /// Positive near clipping distance.
    /// </param>
    /// <param name="farClip">
    /// Far clipping distance greater than <paramref name="nearClip"/>.
    /// </param>
    public void ConfigurePerspective(
        Vector3 position,
        Quaternion rotation,
        float fieldOfView,
        float nearClip,
        float farClip
    ) {
        ValidateFinite(position, nameof(position));
        ValidateRotation(rotation, nameof(rotation));
        if (!float.IsFinite(fieldOfView)
            || fieldOfView < C_MINIMUM_FIELD_OF_VIEW
            || fieldOfView > C_MAXIMUM_FIELD_OF_VIEW
        ) {
            throw new ArgumentOutOfRangeException(nameof(fieldOfView));
        }
        if (!float.IsFinite(nearClip) || nearClip <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(nearClip));
        }
        if (!float.IsFinite(farClip) || farClip <= nearClip)
        {
            throw new ArgumentOutOfRangeException(nameof(farClip));
        }
        m_position = position;
        m_rotation = rotation.normalized;
        m_fieldOfView = fieldOfView;
        m_nearClip = nearClip;
        m_farClip = farClip;
        m_projection = EditorViewportProjection.Perspective;
        isInitialized = true;
    }

    private static void ValidateFinite(
        Vector3 value,
        string parameterName
    ) {
        if (!float.IsFinite(value.x) || !float.IsFinite(value.y) || !float.IsFinite(value.z))
        {
            throw new ArgumentException("Navigation position components must be finite.", parameterName);
        }
    }

    private static void ValidateFinite(
        Quaternion value,
        string parameterName
    ) {
        if (!float.IsFinite(value.x)
            || !float.IsFinite(value.y)
            || !float.IsFinite(value.z)
            || !float.IsFinite(value.w)
        ) {
            throw new ArgumentException("Navigation rotation components must be finite.", parameterName);
        }
    }

    private static void ValidateRotation(
        Quaternion value,
        string parameterName
    ) {
        ValidateFinite(value, parameterName);
        if (value.LengthSquared() < 0.000001f)
        {
            throw new ArgumentException("Navigation rotation must have a usable magnitude.", parameterName);
        }
    }
}
