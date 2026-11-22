using System;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;

namespace Inno.Runtime;

/// <summary>
/// Defines the project-wide game presentation area shared by Editor previews and deployed Players.
/// </summary>
[GenerateSerializationConverter]
[StableTypeID("4068d172-87ac-41b3-ae64-247d25889336")]
[ProjectSettingDefinition(GamePresentationSettings.C_SETTING_PROTOCOL_ID)]
public sealed class GamePresentationSettings : ISerializable
{
    /// <summary>
    /// Gets the immutable project-setting protocol value used by discovery metadata.
    /// </summary>
    public const string C_SETTING_PROTOCOL_ID = "inno.runtime.game-presentation";

    /// <summary>
    /// Gets the stable project-setting identity for game presentation.
    /// </summary>
    public static ProjectSettingID settingID => new(C_SETTING_PROTOCOL_ID);

    /// <summary>
    /// Gets or sets whether the complete reference frame is fitted inside the available surface.
    /// </summary>
    [SerializableProperty]
    public bool preserveAspectRatio { get; set; } = true;

    /// <summary>
    /// Gets or sets the positive reference-frame width used to derive the presentation aspect ratio.
    /// </summary>
    [SerializableProperty]
    public int referenceWidth { get; set; } = 1280;

    /// <summary>
    /// Gets or sets the positive reference-frame height used to derive the presentation aspect ratio.
    /// </summary>
    [SerializableProperty]
    public int referenceHeight { get; set; } = 720;

    /// <summary>
    /// Calculates the centered content region for an available presentation surface.
    /// </summary>
    /// <param name="availableWidth">
    /// Positive available surface width in pixels or logical preview units.
    /// </param>
    /// <param name="availableHeight">
    /// Positive available surface height in pixels or logical preview units.
    /// </param>
    /// <returns>
    /// A positive region that fills the surface or fits the configured reference aspect ratio.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when an available or reference dimension is not positive.
    /// </exception>
    public GamePresentationViewport CalculateViewport(
        int availableWidth,
        int availableHeight
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(availableWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(availableHeight);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(referenceWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(referenceHeight);
        if (!preserveAspectRatio)
        {
            return new GamePresentationViewport(0, 0, availableWidth, availableHeight);
        }

        double targetAspect = referenceWidth / (double)referenceHeight;
        int width = availableWidth;
        int height = Math.Max(1, (int)Math.Floor(width / targetAspect));
        if (height > availableHeight)
        {
            height = availableHeight;
            width = Math.Max(1, (int)Math.Floor(height * targetAspect));
        }

        return new GamePresentationViewport(
            (availableWidth - width) / 2,
            (availableHeight - height) / 2,
            width,
            height
        );
    }
}
