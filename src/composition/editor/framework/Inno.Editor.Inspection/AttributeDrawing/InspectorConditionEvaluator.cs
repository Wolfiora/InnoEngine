using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection;

internal static class InspectorConditionEvaluator
{
    internal static bool Evaluate(
        object? value,
        InspectorCondition condition,
        object? expected
    ) => condition switch
    {
        InspectorCondition.Truthy => IsTruthy(value),
        InspectorCondition.Falsy => !IsTruthy(value),
        InspectorCondition.Equal => AreEqual(value, expected),
        InspectorCondition.NotEqual => !AreEqual(value, expected),
        InspectorCondition.Null => value is null,
        InspectorCondition.NotNull => value is not null,
        InspectorCondition.Assigned => IsAssigned(value),
        InspectorCondition.NotAssigned => !IsAssigned(value),
        _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, "Unknown Inspector condition.")
    };

    private static bool IsTruthy(object? value)
        => value switch
    {
        null => false,
        bool boolean => boolean,
        string text => !string.IsNullOrWhiteSpace(text),
        ICollection collection => collection.Count > 0,
        _ => true
    };

    private static bool IsAssigned(object? value)
    {
        if (value is null)
        {
            return false;
        }
        PropertyInfo? property = value
            .GetType()
            .GetProperty("isAssigned", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        return property?.PropertyType == typeof(bool) && property.GetIndexParameters().Length == 0
               ? (bool)(property.GetValue(value) ?? false)
               : true;
    }

    private static bool AreEqual(
        object? value,
        object? expected
    ) {
        if (Equals(value, expected))
        {
            return true;
        }
        if (value is null || expected is null)
        {
            return false;
        }
        Type valueType = value.GetType();
        try
        {
            object converted = valueType.IsEnum
                               ? expected is string name
                                 ? Enum.Parse(valueType, name, ignoreCase: false)
                                 : Enum.ToObject(valueType, expected)
                               : Convert.ChangeType(expected, valueType, CultureInfo.InvariantCulture);
            return Equals(value, converted);
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidCastException or FormatException or OverflowException)
        {
            return false;
        }
    }
}
