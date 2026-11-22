using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

// E-10: numeric literals use one spelling for suffixes, hexadecimal digits, digit groups and 'float' values.
internal sealed class NumericLiteralStyleRule : StyleRule
{
    private const string C_ID = "E-10";

    private const int C_DECIMAL_GROUP = 3;

    private const int C_HEX_GROUP = 4;

    // A decimal value is grouped from this many digits on.
    private const int C_MIN_GROUPED_DIGITS = 5;

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((SyntaxToken token, string normal) in Violations(source.root))
        {
            findings.Add(source.Finding(C_ID, token.SpanStart, $"Write the numeric literal as '{normal}'."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Violations(source.root).Select(static item => new StyleEdit(item.token.SpanStart, item.token.Span.End, item.normal)).ToArray();

    internal static string Normalize(string text)
    {
        bool isHex = text.StartsWith("0x", StringComparison.OrdinalIgnoreCase);
        bool isBinary = text.StartsWith("0b", StringComparison.OrdinalIgnoreCase);
        if (isHex || isBinary)
        {
            int end = text.Length;
            while (end > 2 && text[end - 1] is 'u' or 'U' or 'l' or 'L')
            {
                end--;
            }
            string digits = text[2..end];
            if (isHex)
            {
                digits = digits.ToUpperInvariant();
                if (!digits.Contains('_') && digits.Length > C_HEX_GROUP) digits = Group(digits, C_HEX_GROUP);
            }

            return (isHex ? "0x" : "0b") + digits + IntegerSuffix(text[end..]);
        }

        bool isReal = text.Contains('.') || text.Contains('e') || text.Contains('E');
        char last = text[^1];
        if (last is 'f' or 'F')
        {
            string body = text[..^1];
            if (body.Contains('.') && !body.Contains('e') && !body.Contains('E'))
            {
                body = body.TrimEnd('0');
                if (body.EndsWith('.')) body = body[..^1];
                if (body.Length == 0 || body[0] == '.') body = "0" + body;
            }

            return body + "f";
        }

        if (last is 'd' or 'D' or 'm' or 'M') return text[..^1] + char.ToLowerInvariant(last);
        if (isReal) return text;
        int suffixStart = text.Length;
        while (suffixStart > 0 && !char.IsAsciiDigit(text[suffixStart - 1]))
        {
            suffixStart--;
        }
        string integer = text[..suffixStart];
        if (!integer.Contains('_') && integer.Length >= C_MIN_GROUPED_DIGITS) integer = Group(integer, C_DECIMAL_GROUP);
        return integer + IntegerSuffix(text[suffixStart..]);
    }

    private static IEnumerable<(SyntaxToken token, string normal)> Violations(SyntaxNode root)
    {
        foreach (SyntaxToken token in root.DescendantTokens())
        {
            if (!token.IsKind(SyntaxKind.NumericLiteralToken)) continue;
            string normal = Normalize(token.Text);
            if (!string.Equals(normal, token.Text, StringComparison.Ordinal)) yield return (token, normal);
        }
    }

    private static string IntegerSuffix(string suffix)
        => suffix.Length switch
    {
        0 => string.Empty,
        1 when suffix is "u" or "U" => "u",
        1 => "L",
        _ => "UL"
    };

    private static string Group(
        string digits,
        int size
    ) {
        List<string> parts = [];
        for (int end = digits.Length; end > 0; end -= size)
        {
            parts.Add(digits[Math.Max(0, end - size)..end]);
        }

        parts.Reverse();
        return string.Join('_', parts);
    }
}
