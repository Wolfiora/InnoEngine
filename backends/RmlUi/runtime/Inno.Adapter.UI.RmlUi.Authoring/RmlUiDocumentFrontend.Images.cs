using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Inno.Assets;
using Inno.UI.Assets;

namespace Inno.Adapter.UI.RmlUi.Authoring;

/// <summary>
/// Resolves image and sprite-sheet declarations through the existing source-aware import graph.
/// </summary>
public sealed partial class RmlUiDocumentFrontend
{
    private static readonly Regex S_IMAGE_ELEMENT = new(
        """(?<comment><!--[\s\S]*?-->)|<img\b[^>]*>""",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex S_IMAGE_ATTRIBUTE = new(
        """\bsrc\s*=\s*(?:"(?<double>[^"]*)"|'(?<single>[^']*)'|(?<bare>[^\s>]+))""",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex S_SPRITE_SHEET = new(
        """(?<comment>/\*[\s\S]*?\*/)|@spritesheet\s+[^{}]+\{(?<body>[^{}]*)\}""",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static string RewriteImageDeclarations(
        string text,
        AssetPath owner,
        Dictionary<string, UiDocumentImageDeclaration> images
    ) {
        string Declare(string relative)
        {
            AssetPath path = ResolvePath(owner, WebUtility.HtmlDecode(relative));
            string source = "inno-image-resource://" + Convert.ToHexStringLower(
                SHA256.HashData(Encoding.UTF8.GetBytes(path.ToString())));
            images.TryAdd(source, new(path.ToString(), source));
            return source;
        }
        text = S_IMAGE_ELEMENT.Replace(text, element => element.Groups["comment"].Success
            ? element.Value
            : S_IMAGE_ATTRIBUTE.Replace(element.Value, attribute =>
                "src=\"" + Declare(AttributeSource(attribute)) + "\""));
        return RewriteSpriteSheetSources(text, Declare);
    }

    private static string RewriteSpriteSheetSources(
        string text,
        Func<string, string> rewrite
    ) => S_SPRITE_SHEET.Replace(text, sheet =>
    {
        if (sheet.Groups["comment"].Success)
            return sheet.Value;
        Group body = sheet.Groups["body"];
        string rewritten = S_PROPERTY.Replace(body.Value, property =>
        {
            if (!property.Groups["name"].Value.Equals("src", StringComparison.OrdinalIgnoreCase))
                return property.Value;
            string value = property.Groups["value"].Value.Trim();
            Match url = S_URL.Match(value);
            value = url.Success ? url.Groups["path"].Value : value.Trim('\'', '"');
            return "src: " + rewrite(value) + ";";
        });
        int offset = body.Index - sheet.Index;
        return sheet.Value[..offset] + rewritten + sheet.Value[(offset + body.Length)..];
    });

    private static string AttributeSource(Match attribute)
    {
        foreach (string group in new[] { "double", "single", "bare" })
            if (attribute.Groups[group].Success)
                return attribute.Groups[group].Value;
        throw new ArgumentException("An image source attribute is empty.");
    }
}
