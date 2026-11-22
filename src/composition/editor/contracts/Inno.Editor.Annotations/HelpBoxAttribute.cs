using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Draws a wrapped explanatory message before the annotated Inspector property.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class HelpBoxAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates an Inspector help message.
    /// </summary>
    /// <param name="text">
    /// Non-empty message text.
    /// </param>
    /// <param name="messageType">
    /// Visual severity.
    /// </param>
    public HelpBoxAttribute(
        string text,
        InspectorMessageType messageType = InspectorMessageType.Info
    ) : this(text, messageType, string.Empty, InspectorCondition.Truthy, null) {}

    /// <summary>
    /// Creates an Inspector help message shown when a sibling satisfies a named condition.
    /// </summary>
    /// <param name="text">
    /// Non-empty message text.
    /// </param>
    /// <param name="conditionMember">
    /// Sibling field or property name.
    /// </param>
    /// <param name="condition">
    /// Comparison applied to the sibling value.
    /// </param>
    /// <param name="messageType">
    /// Visual severity.
    /// </param>
    public HelpBoxAttribute(
        string text,
        string conditionMember,
        InspectorCondition condition,
        InspectorMessageType messageType = InspectorMessageType.Info
    ) : this(text, messageType, conditionMember, condition, null) {}

    /// <summary>
    /// Creates an Inspector help message shown when a sibling equals a compile-time value.
    /// </summary>
    /// <param name="text">
    /// Non-empty message text.
    /// </param>
    /// <param name="conditionMember">
    /// Sibling field or property name.
    /// </param>
    /// <param name="expectedValue">
    /// The expected value consumed by help box attribute; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="messageType">
    /// Visual severity.
    /// </param>
    public HelpBoxAttribute(
        string text,
        string conditionMember,
        object? expectedValue,
        InspectorMessageType messageType = InspectorMessageType.Info
    ) : this(text, messageType, conditionMember, InspectorCondition.Equal, expectedValue) {}

    private HelpBoxAttribute(
        string text,
        InspectorMessageType messageType,
        string conditionMember,
        InspectorCondition condition,
        object? expectedValue
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        this.text = text;
        this.messageType = messageType;
        this.conditionMember = conditionMember ?? string.Empty;
        this.condition = condition;
        this.expectedValue = expectedValue;
    }

    /// <summary>
    /// Gets the message text.
    /// </summary>
    public string text { get; }

    /// <summary>
    /// Gets the visual severity.
    /// </summary>
    public InspectorMessageType messageType { get; }

    /// <summary>
    /// Gets the optional sibling member controlling message visibility.
    /// </summary>
    public string conditionMember { get; }

    /// <summary>
    /// Gets the optional sibling comparison.
    /// </summary>
    public InspectorCondition condition { get; }

    /// <summary>
    /// Gets the optional expected constant.
    /// </summary>
    public object? expectedValue { get; }
}
