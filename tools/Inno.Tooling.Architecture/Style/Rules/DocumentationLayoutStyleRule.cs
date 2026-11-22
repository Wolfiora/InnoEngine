using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// D-02 and D-04: every documentation element is expanded over separate lines, elements follow the tag order,
// a block has no empty lines, and references use the short forms.
internal sealed partial class DocumentationLayoutStyleRule : StyleRule
{
    private const string C_ID = "D-02";

    private const string C_EXTERIOR = "///";

    private static readonly string[] TAG_ORDER = ["summary", "typeparam", "param", "returns", "value", "exception", "remarks"];

    // Elements whose inner line breaks and indentation carry meaning.
    private static readonly string[] PRESERVED = ["<code", "<list", "<para", "<!--", "<![CDATA["];

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleEdit edit in Fix(source))
        {
            findings.Add(source.Finding(C_ID, edit.start, "Expand the documentation elements, order them and remove empty lines."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        foreach (SyntaxTrivia trivia in source.root.DescendantTrivia())
        {
            if (!trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia)) continue;
            if (trivia.GetStructure() is not DocumentationCommentTriviaSyntax comment) continue;
            string original = source.content[trivia.FullSpan.Start..trivia.FullSpan.End];
            string? normal = Normalize(source, trivia, comment, original);
            if (normal is not null && !string.Equals(normal, original, StringComparison.Ordinal))
            {
                edits.Add(new StyleEdit(trivia.FullSpan.Start, trivia.FullSpan.End, normal));
            }
        }

        return edits;
    }

    private static string? Normalize(
        StyleSource source,
        SyntaxTrivia trivia,
        DocumentationCommentTriviaSyntax comment,
        string original
    ) {
        if (comment.ContainsDiagnostics) return null;
        List<(int rank, int order, List<string> lines)> elements = [];
        IReadOnlyList<string> signature = SignatureNames(trivia.Token.Parent);
        foreach (XmlNodeSyntax node in comment.Content)
        {
            switch (node)
            {
                case XmlTextSyntax text:
                    if (StripExterior(text.ToFullString()).Any(static line => line.Trim().Length > 0)) return null;
                    break;
                case XmlEmptyElementSyntax empty:
                    if (empty.ToString().Contains('\n')) return null;
                    elements.Add((Rank(empty.Name.ToString(), null, signature), elements.Count, [References(empty.ToString())]));
                    break;
                case XmlElementSyntax element:
                    if (element.StartTag.ToString().Contains('\n') || element.EndTag.IsMissing) return null;
                    string inner = source.content[element.StartTag.Span.End..element.EndTag.SpanStart];
                    bool isPreserved = PRESERVED.Any(marker => inner.Contains(marker, StringComparison.Ordinal));
                    List<string> lines = [element.StartTag.ToString()];
                    List<string> content = StripExterior(inner)
                        .Select(line => isPreserved ? (line.StartsWith(' ') ? line[1..] : line).TrimEnd() : References(line.Trim()))
                        .ToList();
                    while (content.Count > 0 && content[0].Length == 0)
                    {
                        content.RemoveAt(0);
                    }
                    while (content.Count > 0 && content[^1].Length == 0)
                    {
                        content.RemoveAt(content.Count - 1);
                    }
                    if (!isPreserved) content.RemoveAll(static line => line.Length == 0);
                    lines.AddRange(content);
                    lines.Add(element.EndTag.ToString());
                    string? name = element.StartTag.Attributes
                        .OfType<XmlNameAttributeSyntax>()
                        .FirstOrDefault()?.Identifier.Identifier.ValueText;
                    elements.Add((Rank(element.StartTag.Name.ToString(), name, signature), elements.Count, lines));
                    break;
                default:
                    return null;
            }
        }

        if (elements.Count == 0) return null;
        var indent = new string(' ', source.Column(trivia.FullSpan.Start));
        IEnumerable<string> ordered = elements
            .OrderBy(static element => element.rank)
            .ThenBy(static element => element.order)
            .SelectMany(static element => element.lines)
            .Select(static line => line.Length == 0 ? C_EXTERIOR : C_EXTERIOR + " " + line);
        return string.Join("\n" + indent, ordered) + (original.EndsWith('\n') ? "\n" : string.Empty);
    }

    private static IEnumerable<string> StripExterior(string text)
    {
        foreach (string line in text.Split('\n'))
        {
            string trimmed = line.TrimStart();
            yield return trimmed.StartsWith(C_EXTERIOR, StringComparison.Ordinal) ? trimmed[C_EXTERIOR.Length..] : line;
        }
    }

    private static int Rank(
        string tag,
        string? name,
        IReadOnlyList<string> signature
    ) {
        int group = Array.IndexOf(TAG_ORDER, tag);
        if (group < 0) group = TAG_ORDER.Length;
        int position = name is null ? -1 : signature.ToList().IndexOf(name);
        // Parameters follow the signature; an unknown name keeps its place after the known ones.
        return group * 1000 + (tag is "param" or "typeparam" ? (position < 0 ? 999 : position) : 0);
    }

    private static IReadOnlyList<string> SignatureNames(SyntaxNode? owner)
    {
        SyntaxNode? declaration = owner
            ?.AncestorsAndSelf()
            .FirstOrDefault(static node => node is MemberDeclarationSyntax or LocalFunctionStatementSyntax);
        if (declaration is null) return [];
        IEnumerable<string> typeParameters = declaration.ChildNodes()
            .OfType<TypeParameterListSyntax>()
            .SelectMany(static list => list.Parameters)
            .Select(static parameter => parameter.Identifier.ValueText);
        IEnumerable<string> parameters = declaration.ChildNodes()
            .OfType<BaseParameterListSyntax>()
            .SelectMany(static list => list.Parameters)
            .Select(static parameter => parameter.Identifier.ValueText);
        return typeParameters.Concat(parameters).ToArray();
    }

    // D-04: a self-closing tag has no space before '/>', and keywords use 'langword'.
    private static string References(string line)
        => KeywordPattern().Replace(SelfClosingPattern().Replace(line, "/>"), "<see langword=\"$1\"/>");

    [GeneratedRegex(@"\s+/>")]
    private static partial Regex SelfClosingPattern();

    [GeneratedRegex("<c>(null|true|false)</c>")]
    private static partial Regex KeywordPattern();
}
