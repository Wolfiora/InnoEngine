using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// N-04: namespace usings, static usings and aliases form three groups separated by one blank line. Inside a
// group, System comes first, then other namespaces, then Inno, each tier in ordinal order.
internal sealed class UsingOrderStyleRule : StyleRule
{
    private const string C_ID = "N-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Reorder;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        if (Expected(source) is { } expected)
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    expected.start,
                    "Order the using directives: namespaces, static usings, aliases; System, others, Inno."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => Expected(source) is { } expected ? [new StyleEdit(expected.start, expected.end, expected.text)] : [];

    // The span of the using block and the text it must have, or null when the block is already in order
    // or holds comments or directives that an automatic reorder could misplace.
    private static (int start, int end, string text)? Expected(StyleSource source)
    {
        if (source.root is not CompilationUnitSyntax unit || unit.Usings.Count == 0) return null;
        UsingDirectiveSyntax[] usings = unit.Usings.ToArray();
        if (usings.Any(static directive => !directive.GlobalKeyword.IsKind(SyntaxKind.None))) return null;
        int start = usings[0].SpanStart;
        int end = usings[^1].Span.End;
        for (int index = 1; index < usings.Length; index++)
        {
            if (!StyleLayout.IsBlank(source.content, usings[index - 1].Span.End, usings[index].SpanStart)) return null;
        }

        List<string> lines = [];
        int previousGroup = -1;
        foreach (UsingDirectiveSyntax directive in usings.OrderBy(Group).ThenBy(Tier).ThenBy(Key, StringComparer.Ordinal))
        {
            int group = Group(directive);
            if (previousGroup >= 0 && group != previousGroup) lines.Add(string.Empty);
            lines.Add(source.content[directive.SpanStart..directive.Span.End]);
            previousGroup = group;
        }

        string text = string.Join('\n', lines);
        return text == source.content[start..end] ? null : (start, end, text);
    }

    private static int Group(UsingDirectiveSyntax directive)
        => directive.Alias is not null ? 2 : directive.StaticKeyword.IsKind(SyntaxKind.None) ? 0 : 1;

    private static int Tier(UsingDirectiveSyntax directive)
    {
        if (directive.Alias is not null) return 0;
        string name = directive.NamespaceOrType.ToString();
        if (name == "System" || name.StartsWith("System.", StringComparison.Ordinal)) return 0;
        return name == "Inno" || name.StartsWith("Inno.", StringComparison.Ordinal) ? 2 : 1;
    }

    private static string Key(UsingDirectiveSyntax directive)
        => directive.Alias is not null ? directive.Alias.Name.Identifier.Text : directive.NamespaceOrType.ToString();
}
