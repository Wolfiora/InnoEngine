using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Inno.Testing.Source;

namespace Inno.Tooling.Architecture.Integration.Tests;

[Collection("Architecture diagnostics")]
public sealed class RepositoryRuleSelectionTests
{
    [Fact]
    public void RepositoryExamplesSelectExactlyTheirDeclaredRules()
    {
        string root = TestRepository.GetRoot(typeof(RepositoryRuleSelectionTests).Assembly);
        string path = Path.Combine(root, "tests/tools/Inno.Tooling.Architecture.Integration.Tests/fixtures/rule-selection.json");
        using JsonDocument examples = JsonDocument.Parse(File.ReadAllBytes(path));
        Assert.NotEmpty(examples.RootElement.EnumerateArray());
        foreach (JsonElement example in examples.RootElement.EnumerateArray())
        {
            string result = Select(root, "--paths", example.GetProperty("path").GetString()!);
            foreach (string field in new[] { "includes", "excludes" })
            {
                if (!example.TryGetProperty(field, out JsonElement ids))
                {
                    continue;
                }
                foreach (JsonElement id in ids.EnumerateArray())
                {
                    string rule = ".agents/rulebook/" + id.GetString() + ".md";
                    Assert.True(File.Exists(Path.Combine(root, rule)), "Unknown rule in fixture: " + rule);
                    Assert.Equal(
                        field == "includes",
                        result.Split('\n').Any(line => line.StartsWith(rule + " | ", StringComparison.Ordinal))
                    );
                }
            }
        }
        Assert.Equal(
            Select(root, "--paths", "Directory.Build.targets,Directory.Solution.targets"),
            Select(root, "--paths", "Directory.Build.targets", "--paths", "Directory.Solution.targets")
        );
    }

    [Fact]
    public void DocsRulesRequireAnExplicitCheckoutAndStayInTheirOwnScope()
    {
        DirectoryInfo root = Directory.CreateTempSubdirectory("InnoRules-");
        try
        {
            File.WriteAllText(Path.Combine(root.FullName, "AGENTS.md"), "# Engine rules");
            string docs = Path.Combine(root.FullName, "docs-checkout");
            WriteRule(root.FullName, "coding", "paths: [\"**/*.cs\", \"src/*.txt\"]\ntags: [shared]");
            WriteRule(docs, "writing", "paths: [\"content/**\"]\ntags: [writing]");
            Assert.Throws<ArgumentException>(() => Select(root.FullName, "--tags", "writing"));
            Assert.Throws<ArgumentException>(() => Select(root.FullName, "--paths", "../outside.cs"));
            string rule = Path.Combine(docs, ".agents", "rulebook", "writing.md");
            Assert.Contains(rule, Select(root.FullName, "--docs-root", docs, "--rules", "docs/writing"));
            Assert.Contains(rule, Select(root.FullName, "--docs-root", docs, "--tags", "writing"));
            string result = Select(root.FullName, "--docs-root", docs, "--paths", Path.Combine(docs, "content/Guide.cs"));
            Assert.Contains(rule, result);
            Assert.DoesNotContain("coding.md |", result);
            Assert.Contains("coding.md |", Select(root.FullName, "--paths", "Root.cs"));
            Assert.Contains("coding.md |", Select(root.FullName, "--paths", "src/file.txt"));
            Assert.DoesNotContain("coding.md |", Select(root.FullName, "--paths", "src/nested/file.txt,Root.CS"));
            Assert.Throws<ArgumentException>(() => Select(root.FullName, "--rules", "unknown"));
            File.WriteAllText(Path.Combine(root.FullName, "AGENTS.md"), new string('x', 8193));
            Assert.Throws<InvalidDataException>(() => Select(root.FullName));
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Theory]
    [InlineData("# Missing metadata")]
    [InlineData("---\ntags:\n  - block-list\n---\n")]
    [InlineData("---\n---\n")]
    [InlineData("---\nrequires: [coding]\n---\n")]
    [InlineData("---\ntags: [one]\ntags: [two]\n---\n")]
    [InlineData("---\ntags: []\n---\n")]
    [InlineData("---\ntags: [Not Valid]\n---\n")]
    [InlineData("---\npaths: [\"/src/**\"]\n---\n")]
    [InlineData("---\npaths: [\"src/../**\"]\n---\n")]
    [InlineData("oversized")]
    public void InvalidRuleCatalogsFailAtSelection(string content)
    {
        DirectoryInfo root = Directory.CreateTempSubdirectory("InnoRules-");
        try
        {
            File.WriteAllText(Path.Combine(root.FullName, "AGENTS.md"), "# Root");
            DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(root.FullName, ".agents/rulebook"));
            File.WriteAllText(Path.Combine(directory.FullName, "broken.md"), content == "oversized" ? new string('x', 16_385) : content);
            Assert.Throws<InvalidDataException>(() => Select(root.FullName));
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    private static void WriteRule(
        string root,
        string id,
        string header
    ) {
        DirectoryInfo rules = Directory.CreateDirectory(Path.Combine(root, ".agents/rulebook"));
        File.WriteAllText(Path.Combine(rules.FullName, id + ".md"), "---\n" + header + "\n---\n# Rule\n");
    }

    private static string Select(
        string root,
        params string[] arguments
    ) {
        using var output = new StringWriter(CultureInfo.InvariantCulture);
        TextWriter original = Console.Out;
        try
        {
            Console.SetOut(output);
            Assert.Equal(0, RepositoryRuleSelection.Execute(["--engine-root", root, ..arguments]));
            return output.ToString();
        }
        finally
        {
            Console.SetOut(original);
        }
    }
}
