using System;
using System.IO;
using System.Linq;
using Markdig;
using Markdig.Extensions.Yaml;
using YamlDotNet.RepresentationModel;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiFragment(string path, string text, string body, string uid, string symbol, string source, string? reviewed, YamlMappingNode metadata)
{
    private static readonly MarkdownPipeline S_PIPELINE = new MarkdownPipelineBuilder().UseYamlFrontMatter().Build();

    internal static DocumentationApiFragment? Read(string path, string text)
    {
        YamlFrontMatterBlock? block = Markdown.Parse(text, S_PIPELINE).OfType<YamlFrontMatterBlock>().SingleOrDefault();
        if (block is null) return null;
        var yaml = new YamlStream();
        yaml.Load(new StringReader(block.Lines.ToString()));
        if (yaml.Documents.Count != 1 || yaml.Documents[0].RootNode is not YamlMappingNode map)
            throw new InvalidDataException("Expected one frontmatter mapping: " + path);
        string? symbol = Field("symbol");
        if (symbol is null) return null;
        string source = Field("source") ?? "xml";
        if (source is not ("xml" or "manual"))
            throw new InvalidDataException("API source must be xml or manual: " + path);
        string uid = Field("uid") ?? throw new InvalidDataException("API fragment has no stable identity: " + path);
        string body = text[(block.Span.End + 1)..];
        return new(path, text, body, uid, symbol, source, Field("reviewed"), map);

        string? Field(string name) => map.Children.TryGetValue(new YamlScalarNode(name), out YamlNode? node)
            ? node is YamlScalarNode scalar ? scalar.Value : throw new InvalidDataException("API frontmatter requires a scalar " + name + ": " + path)
            : null;
    }
}
