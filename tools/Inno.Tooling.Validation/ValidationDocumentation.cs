using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inno.Tooling.Validation;

internal static class ValidationDocumentation
{
    internal static bool RequiresScriptReferences(string file)
        => Regex.Matches(File.ReadAllText(file), @"(?ms)^```(?:csharp|cs)\s*\r?\n(.*?)^```\s*$")
            .Any(static block => Regex.IsMatch(block.Groups[1].Value, @"\b(?:InnoEngine|InnoEditor)(?:\.|\s*;)"));
}
