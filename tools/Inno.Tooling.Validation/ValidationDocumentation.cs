using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Syntax;

namespace Inno.Tooling.Validation;

internal static class ValidationDocumentation
{
    internal static bool RequiresScriptReferences(string file)
        => Markdown.Parse(File.ReadAllText(file)).Descendants<FencedCodeBlock>()
            .Where(static block => block.Info is "csharp" or "cs")
            .Any(static block => Regex.IsMatch(block.Lines.ToString(), @"\b(?:InnoEngine|InnoEditor)(?:\.|\s*;)"));
}
