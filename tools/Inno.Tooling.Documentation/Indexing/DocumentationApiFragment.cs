using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Markdig;
using Markdig.Extensions.Yaml;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiFragment(
    string path,
    string text,
    string body,
    string uid,
    string symbol,
    string source,
    string? sourceFingerprint,
    string? outdatedFor,
    bool outdated,
    YamlMappingNode metadata
) {
    private static readonly MarkdownPipeline S_PIPELINE = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();

    internal bool ExplicitlyOutdated => outdated && !metadata.Children.ContainsKey(new YamlScalarNode("reviewed"));

    // Compare the prose that was present at the source change with the text being displayed.
    // Ordinary Markdown edits take effect immediately without writing generated metadata.
    internal string? StaleProse(
        string contract,
        string? legacyContract = null
    ) {
        if (source == "xml") return null;
        bool changed = !string.IsNullOrEmpty(sourceFingerprint) && sourceFingerprint != contract && sourceFingerprint != legacyContract;
        bool legacyStale = metadata.Children.ContainsKey(new YamlScalarNode("reviewed")) && outdated;
        return changed || legacyStale ? ProseFingerprint() : outdatedFor;
    }

    internal bool IsOutdated(string? staleProse)
        => source != "xml"
           && (outdated || (staleProse is not null && staleProse == ProseFingerprint()));

    internal string[] Summaries(string documentation)
    {
        if (metadata.Children.TryGetValue(new YamlScalarNode("description"), out YamlNode? value)
            && value is YamlSequenceNode sequence
        ) {
            return sequence.Children.Cast<YamlScalarNode>().Select(item => item.Value!).ToArray();
        }
        return DocumentationXml.Read(documentation).summaries.ToArray();
    }

    internal string TableDescription(string documentation)
        => string.Join(
            "<br><br>",
            Summaries(documentation).Select(summary => DocumentationApiPresentation.Cell(
                Markdown.ToPlainText(summary).Trim()
            ))
        );

    internal static DocumentationApiFragment? Read(
        string path,
        string text
    ) {
        YamlFrontMatterBlock? block = Markdown.Parse(text, S_PIPELINE).OfType<YamlFrontMatterBlock>().SingleOrDefault();
        if (block is null) return null;
        YamlMappingNode map = ReadMetadata(path, block);
        string? symbol = Field("symbol");
        if (symbol is null) return null;
        string source = Field("source") ?? "xml";
        if (string.IsNullOrWhiteSpace(source))
        {
            throw new InvalidDataException("API source must be xml or a nonempty author name: " + path);
        }
        if (map.Children.TryGetValue(new YamlScalarNode("description"), out YamlNode? description)
            && !IsNull(description)
            && (description is not YamlSequenceNode items || items.Children.Count == 0
                || items.Children.Any(
                    item => item is not YamlScalarNode scalar || (string.IsNullOrWhiteSpace(scalar.Value) || IsNull(scalar))
                ))
        ) {
            throw new InvalidDataException("API description must be null or a nonempty list of text: " + path);
        }
        string uid = Field("uid") ?? throw new InvalidDataException("API fragment has no stable identity: " + path);
        string body = text[(block.Span.End + 1)..];
        string? stale = Field("outdated");
        if (stale is not null && stale is not ("true" or "false"))
        {
            throw new InvalidDataException("API outdated must be true or false: " + path);
        }
        return new(
            path,
            text,
            body,
            uid,
            symbol,
            source,
            Field("sourceFingerprint") ?? Field("reviewed"),
            Field("outdatedFor"),
            stale == "true",
            map
        );

        string? Field(string name)
            => map.Children.TryGetValue(new YamlScalarNode(name), out YamlNode? node)
               ? node is YamlScalarNode scalar
                 ? scalar.Value
                 : throw new InvalidDataException("API frontmatter requires a scalar " + name + ": " + path)
               : null;
    }

    internal static bool IsNull(YamlNode node)
        => node is YamlScalarNode { Style: ScalarStyle.Plain, Value: null or "" or "~" or "null" or "Null" or "NULL" };

    internal static YamlMappingNode ReadMetadata(
        string path,
        string text
    ) {
        YamlFrontMatterBlock? block = Markdown.Parse(text, S_PIPELINE).OfType<YamlFrontMatterBlock>().SingleOrDefault();
        return block is null
               ? throw new InvalidDataException("Documentation requires identity metadata: " + path)
               : ReadMetadata(path, block);
    }

    private string ProseFingerprint()
        => DocumentationMarkdown.Hash(
            JsonSerializer.Serialize(new
            {
                body = body.ReplaceLineEndings("\n").Trim(),
                description = Summaries(string.Empty).Select(value => value.ReplaceLineEndings("\n").Trim())
            })
        );

    private static YamlMappingNode ReadMetadata(
        string path,
        YamlFrontMatterBlock block
    ) {
        var yaml = new YamlStream();
        yaml.Load(new StringReader(block.Lines.ToString()));
        if (yaml.Documents.Count != 1 || yaml.Documents[0].RootNode is not YamlMappingNode map)
        {
            throw new InvalidDataException("Expected one frontmatter mapping: " + path);
        }
        return map;
    }
}
