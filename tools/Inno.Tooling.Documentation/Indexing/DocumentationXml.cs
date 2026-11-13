using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationXml
{
    internal static string Render(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml)) return "No XML description is available.\n";
        using var reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null });
        XElement root = XElement.Load(reader);
        return string.Join("\n\n", root.Elements().Select(element => element.Name.LocalName switch
        {
            "summary" => Children(element).Trim(),
            "remarks" => "## Remarks\n\n" + Children(element).Trim(),
            "param" => "**Parameter `" + element.Attribute("name")?.Value + "`:** " + Children(element).Trim(),
            "typeparam" => "**Type parameter `" + element.Attribute("name")?.Value + "`:** " + Children(element).Trim(),
            "returns" => "**Returns:** " + Children(element).Trim(),
            "value" => "**Value:** " + Children(element).Trim(),
            "exception" => "**Exception `" + element.Attribute("cref")?.Value + "`:** " + Children(element).Trim(),
            "example" => "## Example\n\n" + Children(element).Trim(),
            "seealso" => Node(element),
            "inheritdoc" => throw new InvalidDataException("Resolve inherited XML before publishing API content."),
            _ => throw new InvalidDataException("Unsupported API XML element: " + element.Name)
        }).Where(text => text.Length != 0)) + "\n";
    }

    private static string Children(XElement element) => string.Concat(element.Nodes().Select(Node));

    private static string Node(XNode node)
    {
        if (node is XText text) return Regex.Replace(text.Value, @"\s+", " ")
            .Replace("\\", "\\\\", StringComparison.Ordinal).Replace("<", "\\<", StringComparison.Ordinal)
            .Replace(">", "\\>", StringComparison.Ordinal).Replace("[", "\\[", StringComparison.Ordinal)
            .Replace("]", "\\]", StringComparison.Ordinal).Replace("*", "\\*", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal).Replace("`", "\\`", StringComparison.Ordinal);
        if (node is XComment) return "";
        if (node is not XElement element) throw new InvalidDataException("Unsupported XML documentation node.");
        return element.Name.LocalName switch
        {
            "see" or "seealso" => element.Attribute("href") is XAttribute href
                ? "[" + (element.Value.Length == 0 ? href.Value : element.Value) + "](" + href.Value + ")"
                : Code((element.Attribute("cref") ?? element.Attribute("langword"))?.Value
                    ?? throw new InvalidDataException("An XML reference requires cref, href or langword.")),
            "paramref" or "typeparamref" => Code(element.Attribute("name")?.Value
                ?? throw new InvalidDataException("An XML parameter reference requires its name.")),
            "c" => Code(element.Value),
            "code" => CodeBlock(element),
            "para" => "\n\n" + Children(element).Trim() + "\n\n",
            "b" or "strong" => "**" + Children(element) + "**",
            "i" or "em" => "*" + Children(element) + "*",
            "list" => List(element),
            "item" or "description" => Children(element),
            "term" => "**" + Children(element).Trim() + "** — ",
            _ => throw new InvalidDataException("Unsupported inline API XML element: " + element.Name)
        };
    }

    private static string Code(string value)
    {
        string fence = new('`', Math.Max(1, Regex.Matches(value, "`+").Select(match => match.Length + 1).DefaultIfEmpty(1).Max()));
        return fence + (value.StartsWith('`') ? " " : "") + value + (value.EndsWith('`') ? " " : "") + fence;
    }

    private static string CodeBlock(XElement element)
    {
        string value = element.Value;
        string[] lines = value.ReplaceLineEndings("\n").Trim('\n').Split('\n');
        int indentation = lines.Where(line => !string.IsNullOrWhiteSpace(line)).Select(line => line.Length - line.TrimStart().Length).DefaultIfEmpty(0).Min();
        string code = string.Join('\n', lines.Select(line => line.Length >= indentation ? line[indentation..] : ""));
        // XML code inside an example is executable C#; an unlabelled formula in remarks is explanatory text.
        // This is structural XML metadata, not a guess based on whether a snippet happens to compile.
        string? language = (string?)element.Attribute("language") ?? (string?)element.Attribute("lang");
        if (language is null && !element.Ancestors("example").Any() && !code.Contains('\n')) return Code(code.Trim());
        language ??= element.Ancestors("example").Any() ? "csharp" : "text";
        if (!Regex.IsMatch(language, "^[a-zA-Z0-9#+-]+$")) throw new InvalidDataException("Invalid XML example language.");
        string fence = new('`', Math.Max(3, Regex.Matches(code, "`+").Select(match => match.Length + 1).DefaultIfEmpty(3).Max()));
        return "\n\n" + fence + language + "\n" + code + "\n" + fence + "\n\n";
    }

    private static string List(XElement element)
    {
        string kind = (string?)element.Attribute("type") ?? "bullet";
        XElement[] items = element.Elements("item").ToArray();
        if (items.Length == 0 || element.Elements().Any(child => child.Name.LocalName is not ("item" or "listheader")))
            throw new InvalidDataException("An XML list requires supported, nonempty items.");
        if (kind == "table")
        {
            XElement? header = element.Element("listheader");
            string Cell(XElement? value) => value is null ? "" : Children(value).Trim().Replace("|", "\\|", StringComparison.Ordinal).ReplaceLineEndings(" ");
            return "\n\n| " + (header is null ? "Term | Description" : Cell(header.Element("term")) + " | " + Cell(header.Element("description")))
                + " |\n| --- | --- |\n" + string.Join('\n', items.Select(item => "| " + Cell(item.Element("term")) + " | " + Cell(item.Element("description")) + " |")) + "\n\n";
        }
        if (kind is not ("bullet" or "number")) throw new InvalidDataException("Unsupported XML list type: " + kind);
        string prefix = kind == "number" ? "1. " : "- ";
        return "\n\n" + string.Join('\n', items.Select(item => prefix + Children(item).Trim()
            .Replace("\n", "\n" + new string(' ', prefix.Length), StringComparison.Ordinal))) + "\n\n";
    }
}
