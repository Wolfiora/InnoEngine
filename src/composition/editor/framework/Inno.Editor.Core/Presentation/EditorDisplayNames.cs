using System;
using System.Text;

namespace Inno.Editor.Core;

/// <summary>
/// Formats readable Editor labels independently from a native UI context or session.
/// </summary>
public static class EditorDisplayNames
{
    /// <summary>
    /// Converts an identifier into a readable editor label.
    /// </summary>
    /// <param name="name">
    /// Source identifier.
    /// </param>
    /// <returns>
    /// A label with identifier separators and lower-to-upper transitions spaced; empty when the input is blank.
    /// </returns>
    public static string Nicify(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        var builder = new StringBuilder(name.Length + 8);
        char previous = '\0';
        for (int i = 0; i < name.Length; i++)
        {
            char current = name[i];
            if (current == '_' || current == '-')
            {
                if (builder.Length > 0 && builder[builder.Length - 1] != ' ')
                {
                    builder.Append(' ');
                }

                previous = current;
                continue;
            }

            if (i > 0 && char.IsUpper(current) && char.IsLower(previous) && builder[builder.Length - 1] != ' ')
            {
                builder.Append(' ');
            }

            builder.Append(builder.Length == 0 ? char.ToUpperInvariant(current) : current);
            previous = current;
        }

        return builder.ToString();
    }
}
